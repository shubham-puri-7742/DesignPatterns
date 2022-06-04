using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

// COMPOSITE PATTERN
// Treat individual and aggregate objects identically
// COMPOSITE SPEC EXAMPLE

namespace DesignPatterns
{
    // ENUMS
    public enum Colour
    {
        Red, Green, Blue
    }

    public enum Size
    {
        Small, Medium, Large, Yuge // making design patterns great again
    }

    // data that we're working on
    public class Product
    {
        public string Name;
        public Colour Colour;
        public Size Size;

        public Product(string name, Colour colour, Size size)
        {
            Name = name ?? throw new ArgumentNullException(paramName: nameof(name));
            Colour = colour;
            Size = size;
        }
    }
    
    // ABSTRACT INTERFACES
    // specification template
    public abstract class ISpecification<T>
    {
        public abstract bool Satisfied(T t);

        // overload the & and | operators to combine specs
        public static ISpecification<T> operator &(ISpecification<T> s1, ISpecification<T> s2)
        {
            return new AndCombinator<T>(s1, s2);
        }
        public static ISpecification<T> operator |(ISpecification<T> s1, ISpecification<T> s2)
        {
            return new OrCombinator<T>(s1, s2);
        }
    }

    // filter template
    public interface IFilter<T>
    {
        IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
    }

    // composite spec template
    public abstract class CompositeSpec<T> : ISpecification<T>
    {
        protected readonly ISpecification<T>[] specs;
        
        public CompositeSpec(params ISpecification<T>[] specs)
        {
            this.specs = specs;
        }
    }

    // combinator - and
    public class AndCombinator<T> : CompositeSpec<T>
    {
        public AndCombinator(params ISpecification<T>[] specs) : base(specs) { }

        public override bool Satisfied(T t)
        {
            return specs.All(i => i.Satisfied(t));
        }
    }
    
    // combinator - or
    public class OrCombinator<T> : CompositeSpec<T>
    {
        public OrCombinator(params ISpecification<T>[] specs) : base(specs) { }

        public override bool Satisfied(T t)
        {
            return specs.Any(i => i.Satisfied(t));
        }
    }

    // CONCRETE CLASSES
    // inheritance defines specific specifications and filters
    public class ProductFilter : IFilter<Product>
    {
        public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> spec)
        {
            // for each item in the list
            foreach (var i in items)
            {
                // if the specification is satisfied (match)
                if (spec.Satisfied(i))
                {
                    // add it to the result (return one at a time)
                    yield return i;
                }
            }
        }
    }
    
    public class ColourSpec : ISpecification<Product>
    {
        private Colour colour;

        // ctor
        public ColourSpec(Colour colour)
        {
            this.colour = colour;
        }

        // override satisfied (match by colour)
        public override bool Satisfied(Product p)
        {
            return p.Colour == colour;
        }
    }
    
    public class SizeSpec : ISpecification<Product>
    {
        private Size size;

        // ctor
        public SizeSpec(Size size)
        {
            this.size = size;
        }

        // override satisfied (match by size)
        public override bool Satisfied(Product p)
        {
            return p.Size == size;
        }
    }

    // DRIVER CODE
    static class DriverCode
    {
        static void Main(string[] args)
        {
            // sample data
            var p1 = new Product("Shoes", Colour.Blue, Size.Small);
            var p2 = new Product("XMas Tree", Colour.Green, Size.Large);
            var p3 = new Product("Apple", Colour.Green, Size.Small);
            var p4 = new Product("Pen", Colour.Red, Size.Small);
            var p5 = new Product("T-Shirt", Colour.Red, Size.Medium);
            
            // make a list of it
            var products = new List<Product> {p1, p2, p3, p4, p5};
            
            // make a filter
            var f = new ProductFilter();
            // colour spec
            var r = new ColourSpec(Colour.Red);

            WriteLine("RED ITEMS:");
            foreach (var i in f.Filter(products, r))
            {
                WriteLine(i.Name);
            }
            WriteLine();

            // size spec
            var s = new SizeSpec(Size.Small);

            WriteLine("SMALL ITEMS:");
            foreach (var i in f.Filter(products, s))
            {
                WriteLine(i.Name);
            }
            WriteLine();

            // size and colour spec
            var andSpec = new ColourSpec(Colour.Red) & new SizeSpec(Size.Small);
            
            WriteLine("SMALL RED ITEMS:");
            foreach (var i in f.Filter(products, andSpec))
            {
                WriteLine(i.Name);
            }
            WriteLine();
            
            // size or colour spec
            var orSpec = new ColourSpec(Colour.Red) | new SizeSpec(Size.Small);
            
            WriteLine("SMALL OR RED ITEMS:");
            foreach (var i in f.Filter(products, orSpec))
            {
                WriteLine(i.Name);
            }
            WriteLine();
        }
    }
}