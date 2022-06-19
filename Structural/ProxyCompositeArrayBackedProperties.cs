using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using static System.Console;

// PROXY
// A class that acts as an interface to a particular resource by replicating an interface
// => Identical interfaces implementing different behaviour
// Similar to the decorator pattern but replicates the target interface instead of enhancing it
// COMPOSITE PROXY with ARRAY-BACKED PROPERTIES
// Combines the Proxy and Composite patterns
// UI example - checkboxes with a grouping checkbox
// A composite proxy property is used to set/unset all boxes

namespace DesignPatterns
{
    public class MasonrySettings
    {
        // composite proxy property
        // null if all the other bools have non-identical values
        public bool? All
        {
            get
            {
                // if all the flags are identical
                // (literally, if every subsequent flag has the same value as the first)
                if (flags.Skip(1).All(f => f == flags[0]))
                {
                    return flags[0];
                }
                // ternary logic in action. true/false/null
                return null;
            }
            set
            {
                // return if null
                if (!value.HasValue) return;
                // set all the other bools to the same value
                for (int i = 0; i < flags.Length; ++i)
                {
                    flags[i] = value.Value;
                }
            }
        }
        
        // array actually storing the flags
        private readonly bool[] flags = new bool[5];

        // properties exposing array elements by name
        // 0 - pillars
        public bool Pillars
        {
            get => flags[0];
            set => flags[0] = value;
        }
        // 1 - walls
        public bool Walls
        {
            get => flags[1];
            set => flags[1] = value;
        }
        // 2 - floors
        public bool Floors
        {
            get => flags[2];
            set => flags[2] = value;
        }
        // 3 - windows
        public bool Windows
        {
            get => flags[3];
            set => flags[3] = value;
        }
        // 4 - doors
        public bool Doors
        {
            get => flags[4];
            set => flags[4] = value;
        }

        // display format
        public override string ToString()
        {
            // initialise a string builder
            var res = new StringBuilder();
            // for each flag
            for (int i = 0; i < flags.Length; ++i)
            {
                // append its index and value
                res.Append($"flags[{i}] = {flags[i]}\n");
            }
            return res.ToString();
        }
    }
    
    public class DriverCode
    {
        static void Main(string[] args)
        {
            // test initialisation
            MasonrySettings s = new MasonrySettings();
            WriteLine("Initial state:\n" + s);
            
            // test random updates
            s.Floors = true;
            s.Doors = true;
            WriteLine("After modification:\n" + s);
            
            // test setting all
            s.All = true;
            WriteLine("All set:\n" + s);
        }
    }
}