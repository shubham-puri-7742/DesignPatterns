using static System.Console;

// LISKOV SUBSTITUTION PRINCIPLE
// You should be able to substitute a base class for a derived class
// This makes sure that base class methods are not used for objects of derived classes

namespace DesignPatterns
{
    public class Rectangle
    {
        // LISKOV SUBSTITUTION: make properties virtual in the base class so that any calls are passed to derived classes if appropriate
        public virtual double width { get; set; }
        public virtual double height { get; set; }

        public Rectangle() {}

        public Rectangle(double w, double h)
        {
            width = w;
            height = h;
        }

        // display dimensions as a string
        public override string ToString()
        {
            return $"{nameof(width)}: {width}, {nameof(height)}: {height}";
        }
    }

    public class Square : Rectangle
    {
        // LISKOV SUBSTITUTION: override explicitly in the derived class
        public override double width
        {
            set { base.width = base.height = value; }
        }
        // these overridden setters adjust the height to match the width (to maintain square dimensions)
        public override double height
        {
            set { base.width = base.height = value; }
        }
    }

    public class DriverCode
    {
        // define an area function for a rectangle (and any derived classes)
        static public double Area(Rectangle r) => r.width * r.height;
        
        static void Main(string[] args)
        {
            Rectangle r = new Rectangle(3.14, 5.12);
            WriteLine($"{r} has area {Area(r)}");

            // an object of the derived class can be assigned to a variable of the base type
            Rectangle s = new Square();
            s.width = 4;
            WriteLine($"{s} has area {Area(s)}");
        }
    }
}