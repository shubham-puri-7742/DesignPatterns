using System;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Console;

// PROXY
// A class that acts as an interface to a particular resource by replicating an interface
// => Identical interfaces implementing different behaviour
// Similar to the decorator pattern but replicates the target interface instead of enhancing it
// COMPOSITE PROXY
// Combines the Proxy and Composite patterns
// Can implement a structure known as the AoS/SoA (array of structures/structures of array) duality
// Mainly for performance improvement here
// Required here because an array of creatures (see class below) is
// stored as Age1 X1 Y1 Age2 X2 Y2 ...
// the CPU would work much more efficiently if the data is separated into arrays of each field
// Age1 Age2 Age3 ...
// X1 X2 X3 ...
// Y1 Y2 Y3 ...

namespace DesignPatterns
{
    // an uneven data structure
    class Creature
    {
        // this one byte may be padded
        public byte Age;
        public int x, y;
    }

    class Creatures
    {
        // size
        private readonly int size;
        // separate arrays for the ages, xs, and ys
        private byte[] ages;
        private int[] xs, ys;

        // ctor
        public Creatures(int size)
        {
            this.size = size;
            ages = new byte[size];
            xs = new int[size];
            ys = new int[size];
        }

        // replicates the original Creature interface
        // allows access to members of the arrays
        public struct CreatureProxy
        {
            private readonly Creatures creatures;
            // could be public
            private readonly int index;

            // ctor
            public CreatureProxy(Creatures creatures, int index)
            {
                this.creatures = creatures;
                this.index = index;
            }
            
            // references to the arrays defined above
            public ref byte age => ref creatures.ages[index];
            public ref int x => ref creatures.xs[index];
            public ref int y => ref creatures.ys[index];
        }

        // override to enable a foreach loop
        public IEnumerator<CreatureProxy> GetEnumerator()
        {
            // basically run a vanilla for loop under the hood
            for (int i = 0; i < size; ++i)
            {
                // return one by one
                yield return new CreatureProxy(this, i);
            }
        }
    }
    
    public class DriverCode
    {
        static void Main(string[] args)
        {
            int a = 0;
            // initialise a timer
            Stopwatch s = new Stopwatch();
            
            // initialise the array normally
            var creatures1 = new Creature[100];
            
            // initialise each array member (for fair timing, we assume this exists already, hence this step is not timed)
            for (int i = 0; i < 100; ++i)
            {
                creatures1[i] = new Creature();
            }

            // start timing
            s.Start();
            // iterate over it and do whatever you want
            foreach (var c in creatures1)
            {
                WriteLine($"++x: {++c.x}");
                WriteLine($"y++: {c.y++}");
            }
            // stop timing
            s.Stop();
            
            // timing results
            WriteLine("\n=========================");
            WriteLine($"AoS: {s.ElapsedMilliseconds} ms");
            WriteLine("=========================\n");
            
            // initialise the array using the proxy
            var creatures2 = new Creatures(100);

            // restart timing
            s.Reset();
            s.Start();
            // iterate over it and do whatever you want
            foreach (var c in creatures2)
            {
                WriteLine($"++x: {++c.x}");
                WriteLine($"y++: {c.y++}");
            }
            // stop timing
            s.Stop();
            
            // timing results
            WriteLine("\n=========================");
            WriteLine($"SoA: {s.ElapsedMilliseconds} ms");
            WriteLine("=========================\n");
        }
    }
}