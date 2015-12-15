using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace D07_Circuit_Class
{
    public delegate void Eval();

    public class Wire
    {
        public string Label;

        public List<Component> Dependents = new List<Component>();

        public virtual void Reset() { _value = null; }

        protected ushort? _value;
        public virtual ushort? Value
        {
            get { return _value; }
            set 
            {
                if (value == null) throw new ArgumentNullException("value");
                _value = value;

                foreach(var dep in Dependents)
                {
                    Console.WriteLine("  {0} invoking {1}", Label, dep.Description);
                    dep.Recalc();
                }
            } 
        } 
    }
    public class Const : Wire
    {
        public Const (ushort value) { _value = value; }

        public override void Reset() { }

        public override ushort? Value
        {
            get { return _value; }
            set { throw new Exception("Setting const!"); }
        }
    }

    public abstract class Component
    {
        public string Description;
        public Wire Result;
        public abstract void Recalc();
    }

    public class LShift : Component
    {
        public Wire A;
        public ushort B;
        public override void Recalc()
        {
            if (A.Value != null )
                Result.Value = (ushort)(A.Value << B);
        }
    }

    public class RShift : Component
    {
        public Wire A;
        public ushort B;
        public override void Recalc()
        {
            if (A.Value != null)
                Result.Value = (ushort)(A.Value >> B);
        }
    }

    public class Assign : Component
    {
        public Wire In;
        public override void Recalc()
        {
            if (In.Value != null)
                Result.Value = In.Value;
        }
    }

    public class Not : Component
    {
        public Wire In;
        public override void Recalc()
        {
            if (In.Value != null)
                Result.Value = (ushort)~In.Value;
        }
    }

    public class And : Component
    {
        public Wire A;
        public Wire B;
        public override void Recalc()
        {
            if (A.Value != null && B.Value != null)
                Result.Value = (ushort)(A.Value & B.Value);
        }
    }

    public class Or : Component
    {
        public Wire A;
        public Wire B;
        public override void Recalc()
        {
            if (A.Value != null && B.Value != null)
                Result.Value = (ushort)(A.Value | B.Value);
        }
    }

    public class D07_Circuit_Class
    {
        public Dictionary<string, Wire> wires = new Dictionary<string, Wire>();
        public List<Component> components = new List<Component>();

        public void Run()
        {
            var s = File.ReadAllLines("input.txt");

            Construct(s);

            //foreach (var w in p.wires)
            //    w.Value.Reset();

            foreach (var dep in wires["b"].Dependents)                 
                dep.Recalc();

            WriteOut();
        }

        public void WriteOut()
        {
            foreach(var v in wires.OrderBy(kv => kv.Key))
                Console.WriteLine("{0}: {1}", v.Key, v.Value.Value);
        }

        public void Construct(string[] instructions)
        { 
            var rVAssign = new Regex(@"^([a-z0-9]+) -> ([a-z]+)");
            var rNot = new Regex(@"^NOT ([a-z0-9]+) -> ([a-z]+)");
            var rBinop = new Regex(@"^([a-z0-9]+) ([A-Z]+) ([a-z0-9]+) -> ([a-z]+)");
            var rBinIop = new Regex(@"^([a-z0-9]+) ([LR])SHIFT (\d+) -> ([a-z]+)");

            // lshift, rshift, and, or
            foreach(var line in instructions) 
            {
                var mVAssign = rVAssign.Match(line);
                if (mVAssign.Success)
                {
                    var arg = GetWire(mVAssign.Groups[1].Value);
                    var res = GetWire(mVAssign.Groups[2].Value);
                    if (res.Value != null) Console.WriteLine("Already assigned " + res.Label);
                    var comp = new Assign() { Description = line, In = arg, Result = res };
                    components.Add(comp);
                    arg.Dependents.Add(comp);
                    comp.Recalc();
                    continue;
                }

                var mNot = rNot.Match(line);
                if (mNot.Success) 
                {
                    var arg = GetWire(mNot.Groups[1].Value);
                    var res = GetWire(mNot.Groups[2].Value);
                    if (res.Value != null) throw new Exception("Already assigned " + res.Label);
                    var comp = new Not { Description = line, In = arg, Result = res };
                    components.Add(comp);
                    arg.Dependents.Add(comp);
                    comp.Recalc();
                    continue;
                }

                var mBinIop = rBinIop.Match(line);
                if (mBinIop.Success) 
                {
                    var arg = GetWire(mBinIop.Groups[1].Value);
                    var dir = mBinIop.Groups[2].Value;
                    var i = ushort.Parse(mBinIop.Groups[3].Value);
                    var res = GetWire(mBinIop.Groups[4].Value);
                    if (res.Value != null) throw new Exception("Already assigned " + res.Label);
                    var comp = dir == "L" 
                        ? (Component)new LShift { Description = line, A = arg, B = i, Result = res }
                        : (Component)new RShift { Description = line, A = arg, B = i, Result = res };
                    components.Add(comp);
                    arg.Dependents.Add(comp);
                    comp.Recalc();
                    continue;
                }

                var mBinop = rBinop.Match(line);
                if (mBinop.Success)
                {
                    var arg1 = GetWire(mBinop.Groups[1].Value);
                    var op = mBinop.Groups[2].Value;
                    var arg2 = GetWire(mBinop.Groups[3].Value);
                    var res = GetWire(mBinop.Groups[4].Value);
                    if (res.Value != null) throw new Exception("Already assigned " + res.Label);
                    var comp = op == "AND"
                        ? (Component)new And { Description = line, A = arg1, B = arg2, Result = res }
                        : (Component)new Or { Description = line, A = arg1, B = arg2, Result = res };
                    components.Add(comp);
                    arg1.Dependents.Add(comp);
                    arg2.Dependents.Add(comp);
                    comp.Recalc();
                    continue;
                }
                throw new Exception("Unknown command " + line);
            }
        }

        Wire GetWire(string name)
        {
            ushort con = 0;
            if (ushort.TryParse (name, out con))
            {
                return new Const(con) { Label = "Const" };
            }
            
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException();

            Wire w;
            if (!wires.TryGetValue(name, out w))
            {
                w = new Wire() { Label = name };
                wires.Add(name, w);
            }
            return w;
        }

        ushort abort(string message) { throw new Exception(message); }
    }
}