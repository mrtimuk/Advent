using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace D07_Circuit_Lambda
{
    public delegate void Eval();

    public class Node
    {
        public string Label;
        public string SetBy;
        protected ushort? _value;
        public virtual ushort? Value
        {
            get { return _value; }
            set
            {
                Console.WriteLine("Setting {0} to {1}", Label, value);
                _value = value;
                if (Evaluated != null)
                {
                    Console.WriteLine("  invoking {0} nodes", Evaluated.GetInvocationList().Length);
                    Evaluated();
                }
            }
        }
        public event Eval Evaluated;
    }

    public class ConstNode : Node
    {
        public ConstNode(ushort? value) { _value = value; }
        public override ushort? Value { get { return _value; } set { throw new Exception("const!"); } }
    }

    public class D07_Circuit
    {
        public Dictionary<string, Node> nodes = new Dictionary<string, Node>();

        public void Run()
        {
            var s = File.ReadAllLines("D07.txt");

            Process(s);

            //foreach (var n in p.nodes.Where(i => string.Compare(i.Value.Label, "Const") != 0))
            //    n.Value._value = null;

            nodes["b"] = new ConstNode(3176) { SetBy = "Prog", Label = "b" };
            WriteOut();
        }

        public void WriteOut()
        {
            foreach (var v in nodes.OrderBy(kv => kv.Key))
                Console.WriteLine("{0}: {1}", v.Key, v.Value.Value);
        }

        public void Process(string[] instructions)
        {
            var rVAssign = new Regex(@"^([a-z0-9]+) -> ([a-z]+)");
            var rNot = new Regex(@"^NOT ([a-z0-9]+) -> ([a-z]+)");
            var rBinop = new Regex(@"^([a-z0-9]+) ([A-Z]+) ([a-z0-9]+) -> ([a-z]+)");
            var rBinIop = new Regex(@"^([a-z0-9]+) ([LR])SHIFT (\d+) -> ([a-z]+)");

            // lshift, rshift, and, or
            foreach (var line in instructions)
            {
                var mVAssign = rVAssign.Match(line);
                if (mVAssign.Success)
                {
                    var src = GetNode(mVAssign.Groups[1].Value);
                    var dest = GetNode(mVAssign.Groups[2].Value);
                    if (dest.Value != null) Console.WriteLine("Already assigned " + dest.Label);
                    src.Evaluated += () =>
                    {
                        dest.SetBy = "Arg eval";
                        dest.Value = src.Value;
                    };
                    if (src.Value != null)
                    {
                        dest.SetBy = "Init";
                        dest.Value = src.Value;
                    }
                    continue;
                }

                var mNot = rNot.Match(line);
                if (mNot.Success)
                {
                    var arg = GetNode(mNot.Groups[1].Value);
                    var dest = GetNode(mNot.Groups[2].Value);
                    if (dest.Value != null) Console.WriteLine("Already assigned " + dest.Label);
                    arg.Evaluated += () =>
                    {
                        dest.SetBy = "Arg eval";
                        dest.Value = (ushort)~arg.Value;
                    };
                    if (arg.Value != null)
                    {
                        dest.SetBy = "Init";
                        dest.Value = (ushort)~arg.Value;
                    }
                    continue;
                }

                var mBinIop = rBinIop.Match(line);
                if (mBinIop.Success)
                {
                    var arg = GetNode(mBinIop.Groups[1].Value);
                    var dir = mBinIop.Groups[2].Value;
                    var i = ushort.Parse(mBinIop.Groups[3].Value);
                    var dest = GetNode(mBinIop.Groups[4].Value);
                    if (dest.Value != null) Console.WriteLine("Already assigned " + dest.Label);
                    arg.Evaluated += () =>
                    {
                        if (dest.Value != null) Console.WriteLine("Already assigned " + dest.Label);
                        dest.SetBy = "Arg eval";
                        dest.Value =
                            dir == "L" ? (ushort)(arg.Value << i) :
                            dir == "R" ? (ushort)(arg.Value >> i) :
                            abort("unexpected dir " + dir);
                    };
                    if (arg.Value != null)
                    {
                        dest.SetBy = "Init";
                        dest.Value = dir == "L" ? (ushort)(arg.Value << i) :
                                    dir == "R" ? (ushort)(arg.Value >> i) :
                                    abort("unexpected dir " + dir);
                    }
                    continue;
                }

                var mBinop = rBinop.Match(line);
                if (mBinop.Success)
                {
                    var arg1 = GetNode(mBinop.Groups[1].Value);
                    var op = mBinop.Groups[2].Value;
                    var arg2 = GetNode(mBinop.Groups[3].Value);
                    var dest = GetNode(mBinop.Groups[4].Value);
                    if (dest.Value != null) Console.WriteLine("Already assigned " + dest.Label);
                    arg1.Evaluated += () =>
                    {
                        if (arg2.Value != null)
                        {
                            if (dest.Value != null) Console.WriteLine("Already assigned " + dest.Label);
                            dest.SetBy = "Arg1 eval";
                            dest.Value = (op == "AND") ? (ushort)(arg1.Value & arg2.Value) :
                                        (op == "OR") ? (ushort)(arg1.Value | arg2.Value) :
                                        abort("unexpected op " + op);
                        }
                    };
                    arg2.Evaluated += () =>
                    {
                        if (arg1.Value != null)
                        {
                            if (dest.Value != null) Console.WriteLine("Already assigned " + dest.Label);
                            dest.SetBy = "Arg2 eval";
                            dest.Value = (op == "AND") ? (ushort)(arg1.Value & arg2.Value) :
                                        (op == "OR") ? (ushort)(arg1.Value | arg2.Value) :
                                        abort("unexpected op " + op);
                        }
                    };
                    if (arg1.Value != null && arg2.Value != null)
                    {
                        dest.SetBy = "Init";
                        dest.Value = (op == "AND") ? (ushort)(arg1.Value & arg2.Value) :
                                    (op == "OR") ? (ushort)(arg1.Value | arg2.Value) :
                                    abort("unexpected op " + op);
                    }
                    continue;
                }
                throw new Exception("Unknown command " + line);
            }
        }

        Node GetNode(string name)
        {
            ushort con = 0;
            if (ushort.TryParse(name, out con))
            {
                return new ConstNode(con) { Label = "Const", SetBy = "Const" };
            }

            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException();

            Node n;
            if (!nodes.TryGetValue(name, out n))
            {
                n = new Node() { Label = name, SetBy = "Unset" };
                nodes.Add(name, n);
            }
            return n;
        }

        ushort abort(string message)
        {
            throw new Exception(message);
        }
    }
}