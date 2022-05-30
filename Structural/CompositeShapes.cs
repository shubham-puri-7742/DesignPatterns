using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

// COMPOSITE PATTERN
// Treat individual and aggregate objects identically
// GEOMETRIC SHAPES EXAMPLE

namespace DesignPatterns
{
    public class GraphicObject
    {
        // the base class can be used for groups (composites) of objects
        public virtual string Name { get; set; } = "Group";
        public string Colour;

        // lazy = initialise only when necessary
        private Lazy<List<GraphicObject>> children = new Lazy<List<GraphicObject>>();
        // should probably be private in a real application. Public for simplicity here
        public List<GraphicObject> Children => children.Value;

        // printing mechanism - print the shape's details and those of all its children (if any)
        private void Print(StringBuilder s, int depth)
        {
            s.Append(new string ('*', depth)).Append(string.IsNullOrWhiteSpace(Colour) ? String.Empty : $"{Colour} ").AppendLine(Name);
            
            foreach (var c in Children)
            {
                c.Print(s, depth + 1);
            }
        }
        
        public override string ToString()
        {
            var s = new StringBuilder();
            Print(s, 0);
            return s.ToString();
        }
    }

    public class Circle : GraphicObject
    {
        public override string Name => "Circle";
    }

    public class Square : GraphicObject
    {
        public override string Name => "Square";
    }
    
    static class DriverCode
    {
        static void Main(string[] args)
        {
            // initialise a composite shape
            var drawing = new GraphicObject { Name = "Some composite drawing" };
            drawing.Children.Add(new Square {Colour = "Red"});
            drawing.Children.Add(new Circle {Colour = "Green"});
            drawing.Children.Add(new Square {Colour = "Blue"});

            // ... that contains another composite shape
            var group = new GraphicObject();
            group.Children.Add(new Circle { Colour = "Green" });
            group.Children.Add(new Square { Colour = "Blue" });
            group.Children.Add(new Circle { Colour = "Blue" });
            
            drawing.Children.Add(group);
            
            WriteLine(drawing);
        }
    }
}