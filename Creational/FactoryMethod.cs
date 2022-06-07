using System;
using static System.Console;

// FACTORIES
// a component that creates other components wholesale
// effectively outsourcing object creation for when object creation logic becomes convoluted
// can be a factory method (a separate, possibly static, function)
// or a factory class (in compliance with the single-responsible principle)
// or a hierarchy of factories with an abstract factory

namespace DesignPatterns
{
    public enum CoordinateSystem
    {
        Cartesian,
        Polar
    }

    public class Point
    {
        private double x, y;

        private Point(double x, double y)
        {
            this.x = (Math.Abs(x - Math.Round(x)) < 1e-10) ? (int)x : x;
            this.y = (Math.Abs(y - Math.Round(y)) < 1e-10) ? (int)y : y;
        }

        // Two factory methods
        public static Point CreateCartesian(double x, double y)
        {
            return new Point(x, y);
        }
        public static Point CreatePolar(double rho, double theta)
        {
            return new Point(rho * Math.Cos(theta), rho * Math.Sin(theta));
        }

        public override string ToString()
        {
            return $"({x}, {y})";
        }
    }

    public class DriverCode
    {
        static void Main(string[] args)
        {
            var p1 = Point.CreatePolar(1.0, Math.PI / 2);
            var p2 = Point.CreateCartesian(4, 5);
            WriteLine(p1);
            WriteLine(p2);
        }
    }
}