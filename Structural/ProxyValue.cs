using System;
using System.Diagnostics;
using static System.Console;

// PROXY
// A class that acts as an interface to a particular resource by replicating an interface
// => Identical interfaces implementing different behaviour
// Similar to the decorator pattern but replicates the target interface instead of enhancing it
// VALUE PROXY
// Constructed over a primitive type (here, a double)
// For instance, for stronger typing
// Representing percentages example

namespace DesignPatterns
{
    // Value proxy masquerading as a number
    [DebuggerDisplay("{val * 100.0}%")]
    public struct Percentage : IEquatable<Percentage>
    {
        // internal value (a double)
        private readonly double val;
        // ctor
        internal Percentage(double val)
        {
            this.val = val;
        }

        // operators
        // double + Percentage
        public static double operator +(double n, Percentage p)
        {
            return n + p.val;
        }
        // double * Percentage
        public static double operator *(double n, Percentage p)
        {
            return n * p.val;
        }
        // Percentage + Percentage
        public static Percentage operator +(Percentage p1, Percentage p2)
        {
            return new Percentage(p1.val + p2.val);
        }
        // Percentage * Percentage
        public static Percentage operator *(Percentage p1, Percentage p2)
        {
            return new Percentage(p1.val * p2.val);
        }
        // equality members
        // Percent = Percent
        public bool Equals(Percentage other)
        {
            return val.Equals(other.val);
        }
        // Percent = Obj
        public override bool Equals(object obj)
        {
            return obj is Percentage other && Equals(other);
        }
        // Hash code
        public override int GetHashCode()
        {
            return val.GetHashCode();
        }
        // == operator
        public static bool operator ==(Percentage left, Percentage right)
        {
            return left.Equals(right);
        }
        // != operator
        public static bool operator !=(Percentage left, Percentage right)
        {
            return !left.Equals(right);
        }

        // display format
        public override string ToString()
        {
            return $"{val * 100.0}%";
        }
    }

    static class PercentageExtensions
    {
        // the value in %
        public static Percentage Percent(this int val)
        {
            return new Percentage(val / 100.0);
        }
        public static Percentage Percent(this double val)
        {
            return new Percentage(val / 100.0);
        }
    }

    public class DriverCode
    {
        static void Main(string[] args)
        {
            WriteLine(10.3 * 2.Percent() * 2);
            WriteLine(5.Percent() + 7.7.Percent());
        }
    }
}