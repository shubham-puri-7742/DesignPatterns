// FACTORIES
// a component that creates other components wholesale
// effectively outsourcing object creation for when object creation logic becomes convoluted
// can be a factory method (a separate, possibly static, function)
// or a factory class (in compliance with the single-responsible principle)
// or a hierarchy of factories with an abstract factory

namespace DesignPatterns
{
    using System.Threading.Tasks;
    using static System.Console;

    public class Spam
    {
        private Spam()
        {
            WriteLine("ctor");
        }

        private async Task<Spam> InitAsync()
        {
            WriteLine("InitAsync called.");
            await Task.Delay(1000);
            WriteLine("InitAsync returning.");
            return this;
        }

        // Async invocation cannot happen in ctors
        // so use a factory method that does it for you
        public static Task<Spam> CreateAsync()
        {
            WriteLine("CreateAsync called.");
            var result = new Spam();
            WriteLine("CreateAsync returning.");
            return result.InitAsync();
        }
    }

    public class DriverCode
    {
        public static async Task Main(string[] args)
        {
            WriteLine("Starting...");
            // one-line initialisation
            Spam x = await Spam.CreateAsync();
            WriteLine("Done.");
        }
    }
}