using System;
using static System.Console;

// PROXY
// A class that acts as an interface to a particular resource by replicating an interface
// => Identical interfaces implementing different behaviour
// Similar to the decorator pattern but replicates the target interface instead of enhancing it
// PROTECTION PROXY
// Checks access permissions

namespace DesignPatterns
{
    public interface ICar
    {
        void Drive();
    }

    // the actual car
    public class Car : ICar
    {
        public void Drive()
        {
            WriteLine("Drive, drive, drive your car!");
        }
    }

    // driver info
    public class Driver
    {
        public int Age { get; set; }

        public Driver(int age)
        {
            Age = age;
        }
    }
    
    // car proxy - the same interface as Car, but additional checks
    public class CarProxy : ICar
    {
        private Driver driver;
        private Car car = new Car();

        public CarProxy(Driver driver)
        {
            this.driver = driver;
        }

        public void Drive()
        {
            // access rights
            if (driver.Age >= 17)
                car.Drive();
            else // break
            {
                WriteLine("Too young to drive.");
            }
        }
    }
    
    public class DriverCode
    {
        static void Main(string[] args)
        {
            // possible to store CarProxy in an ICar variable because of identical interface
            ICar car1 = new CarProxy(new Driver(12));
            car1.Drive();
            ICar car2 = new CarProxy(new Driver(18));
            car2.Drive();
        }
    }
}