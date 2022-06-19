using System;
using static System.Console;

// PROXY
// A class that acts as an interface to a particular resource by replicating an interface
// => Identical interfaces implementing different behaviour
// Similar to the decorator pattern but replicates the target interface instead of enhancing it
// COMPOSITE PROXY ('all' checkbox)
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
                if (Pillars == Walls && Walls == Floors && Floors == Windows && Windows == Doors)
                {
                    return Pillars;
                }
                // ternary logic in action. true/false/null
                return null;
            }
            set
            {
                // return if null
                if (!value.HasValue) return;
                // set all the other bools to the same value
                Pillars = value.Value;
                Walls = value.Value;
                Floors = value.Value;
                Windows = value.Value;
                Doors = value.Value;
            }
        }
        // bools for individual settings
        public bool Pillars, Walls, Floors, Windows, Doors;

        // display format
        public override string ToString()
        {
            // each bool with its value
            return $"Pillars = {Pillars}\nWalls = {Walls}\nFloors = {Floors}\nWindows = {Windows}\nDoors = {Doors}\n";
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