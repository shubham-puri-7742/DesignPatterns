using System;
using System.CodeDom;
using System.Collections.Generic;
using static System.Console;

// PROXY
// A class that acts as an interface to a particular resource by replicating an interface
// => Identical interfaces implementing different behaviour
// Similar to the decorator pattern but replicates the target interface instead of enhancing it
// PROPERTY PROXY
// Using an object as a property

namespace DesignPatterns
{
    public class Property<T> : IEquatable<Property<T>> where T : new()
    {
        private T value;

        // publicly exposed property that works with the private property
        public T Value
        {
            get => value;
            set
            {
                // only if unequal
                if (Equals(this.value, value)) return;
                WriteLine($"Assigning the value {value}...");
                this.value = value;
            }
        }

        public Property() : this(default(T)) {}

        public Property(T value)
        {
            this.value = value;
        }
        
        // implicit conversions to and from T
        public static implicit operator Property<T>(T val)
        {
            return new Property<T>(val); // Property<int> p = 123;
        }
        
        public static implicit operator T(Property<T> prop)
        {
            return prop.value; // int n = p_int;
        }

        // tests for equality
        public bool Equals(Property<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<T>.Default.Equals(value, other.value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Property<T>)obj);
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public static bool operator ==(Property<T> left, Property<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Property<T> left, Property<T> right)
        {
            return !Equals(left, right);
        }
    }

    public class Creature
    {
        private Property<int> agility = new Property<int>();

        // this separation is required to make = assignments work
        // because the = operator cannot be overloaded in C#
        // (a statement like obj.Agility = 10 really uses an implicit conversion)
        public int Agility
        {
            get => agility.Value;
            set => agility.Value = value;
        }
    }
    
    public class DriverCode
    {
        static void Main(string[] args)
        {
            var c = new Creature();
            c.Agility = 10;
            // test repeated assignment
            c.Agility = 10;
            c.Agility = 20;
        }
    }
}