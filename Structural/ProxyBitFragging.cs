using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using static System.Console;

// PROXY
// A class that acts as an interface to a particular resource by replicating an interface
// => Identical interfaces implementing different behaviour
// Similar to the decorator pattern but replicates the target interface instead of enhancing it
// BIT FRAGGING
// Space-saving technique that uses bitwise operations to store multiple boolean values in a single variable
// The .NET framework has BitVector32 and BitArrays for collections of single bits (proxies on boolean arrays)
// but this example shows flexible fragging (e.g. in pairs)
// this allows non-binary logic (e.g. with 2 bits, it is quaternary)
// SAMPLE PROBLEM
// Given a bunch of numbers, place operators between them to get the appropriate result
// HERE: Nums = 1, 2, 3, 4, 5
// WANTED RESULT: 0 to 15
// Appropriate for quaternary logic as there are 4 operators (+, -, *, /)

namespace DesignPatterns
{
    // operators
    // SCHEME
    // 00 = *
    // 01 = /
    // 10 = +
    // 11 = -
    public enum Op : byte
    {
        // see the dictionaries below
        // these descriptions identify the operations
        [Description("*")] Mul = 0,
        [Description("/")] Div = 1,
        [Description("+")] Add = 2,
        [Description("-")] Sub = 3
    }

    public static class OpImpl
    {
        // dictionary
        // mapping: op -> char (e.g. *)
        private static readonly Dictionary<Op, char> opName = new Dictionary<Op, char>();

        // initialise the dictionary
        static OpImpl()
        {
            // get the type of the operation
            var type = typeof(Op);
            // for each possible enum value
            foreach (Op o in Enum.GetValues(type))
            {
                // reflection galore
                // get the information of the current member as a string
                MemberInfo[] memInfo = type.GetMember((o.ToString()));
                // if the info is found to have nonzero length
                if (memInfo.Length > 0)
                {
                    // get the description attribute from the first element
                    var att = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                    // if the attribute is found to have nonzero length
                    if (att.Length > 0)
                    {
                        // set the operator name at the corresponding index equal to the first (0th) element of the description
                        opName[o] = ((DescriptionAttribute)att[0]).Description[0];
                    }
                }
            }
        }

        // mapping: Op -> function to perform it
        // doubles here (but ints in input => see below) because a fractional value is used as an error flag (see the problem model)
        private static readonly Dictionary<Op, Func<double, double, double>> opImpl =
            new Dictionary<Op, Func<double, double, double>>()
            {
                [Op.Mul] = ((a, b) => a * b),
                [Op.Div] = ((a, b) => a / b),
                [Op.Add] = ((a, b) => a + b),
                [Op.Sub] = ((a, b) => a - b)
            };

        // calls the function for the given operator on a and b
        public static double Call(this Op o, int a, int b)
        {
            return opImpl[o](a, b);
        }

        // returns the name of the operator
        public static char Name(this Op o)
        {
            return opName[o];
        }
    }

    // The actual proxy
    public class BitPair
    {
        // 64 bits (unsigned long) => 32 values
        private readonly ulong data;

        public BitPair(ulong data)
        {
            this.data = data;
        }

        // indexer (get the corresponding pair)
        public byte this[int index]
        {
            get
            {
                // double the index (e.g. 0 => 0, 1 => 2, 2 => 4, 3 => 6)
                var shift = index << 1;
                // mask (to omit subsequent bits)
                // e.g.
                // 00 10 01 01 = val
                // 00 11 00 00 = mask
                ulong mask = (0b11U << shift);

                // apply the mask
                // bitwise AND and shift it right by the required number
                // (to get a value between 00 and 11, inclusive)
                return (byte)((data & mask) >> shift);
            }
        }
    }

    public class Problem
    {
        // list of nums
        private readonly List<int> nums;

        // list of operations
        private readonly List<Op> ops;

        public Problem(IEnumerable<int> nums, IEnumerable<Op> ops)
        {
            this.nums = new List<int>(nums);
            this.ops = new List<Op>(ops);
        }

        public int Eval()
        {
            // groups for operator precedence - D/M A/S
            var opGroups = new[]
            {
                new[] { Op.Mul, Op.Div },
                new[] { Op.Add, Op.Sub }
            };

            // see the goto
            startAgain:
            // for each group of operators
            foreach (var g in opGroups)
            {
                // for each operator in the group
                for (int i = 0; i < ops.Count; ++i)
                {
                    // if the operator is in the group
                    // (try to find the operation in the current group)
                    if (g.Contains(ops[i]))
                    {
                        // get the operation
                        var o = ops[i];
                        // perform the operation
                        // the result is a double.
                        // fractional = fail
                        var result = o.Call(nums[i], nums[i + 1]);

                        // if the result is fractional
                        if (result != (int)result)
                        {
                            // nonsensical value => error flag
                            return int.MinValue;
                        }

                        // replace the value at the index with the result
                        nums[i] = (int)result;
                        // remove the second number and the operator
                        nums.RemoveAt(i + 1);
                        ops.RemoveAt(i);

                        // when there is a single value left
                        // this is when we have evaluated everything. e.g.
                        // 1 2 3 4
                        //  + * +
                        // EVALUATES to
                        // 1 6 4
                        //  + +
                        // AND FINALLY
                        // 11
                        if (nums.Count == 1)
                        {
                            return nums[0];
                        }

                        // could be a loop here, but this is actually more readable here
                        goto startAgain;
                    }
                }
            }

            return nums[0];
        }

        // display format
        public override string ToString()
        {
            // initialise the string builder
            var s = new StringBuilder();
            // counter
            int i = 0;
            // for each number
            for (; i < ops.Count; ++i)
            {
                // append the number
                s.Append(nums[i]);
                // append the corresponding operator
                s.Append($" {ops[i].Name()} ");
            }

            // append the final number
            s.Append(nums[i]);
            // return the string result
            return s.ToString();
        }
    }

    public class DriverCode
    {
        static void Main(string[] args)
        {
            // HERE: Nums = 1, 2, 3, 4, 5
            var nums = new[] { 1, 2, 3, 4, 5 };
            int numOps = nums.Length - 1;
            // success flag for the previous result
            bool succ;
            
            // WANTED RESULT: 0 to 15
            for (int res = 0; res <= 15; ++res)
            {
                succ = false;
                // key = 0 (unsigned long) to 1 (unsigned long) shifted left by twice the number of operations
                // twice because each operation uses two bits
                for (var key = 0UL; key < (1UL << 2 * numOps); ++key)
                {
                    // create a bit pair with the given key
                    var b = new BitPair(key);
                    
                    // get the operations. stepwise:
                    // get the range from 0 to numOps,
                    // select the corresponding element from the bit pair,
                    // cast the results to an Op[]
                    var ops = Enumerable.Range(0, numOps).Select(i => b[i]).Cast<Op>().ToArray();

                    var problem = new Problem(nums, ops);

                    if (problem.Eval() == res)
                    {
                        // success message
                        // create a new instance because Eval mutates the problem object
                        WriteLine($"{new Problem(nums, ops)} = {res}");
                        // set the success flag
                        succ = true;
                        break;
                    }
                }
                // if the inner loop found nothing
                if (!succ)
                {
                    // failure message
                    WriteLine($"No way to get {res}.");
                }
            }
        }
    }
}