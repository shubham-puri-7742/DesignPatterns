using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using ImpromptuInterface; // gives the desired interface for a dynamic object
using static System.Console;

// PROXY
// A class that acts as an interface to a particular resource by replicating an interface
// => Identical interfaces implementing different behaviour
// Similar to the decorator pattern but replicates the target interface instead of enhancing it
// DYNAMIC PROXY
// static is faster while dynamic is constructed at runtime
// used here to log information

namespace DesignPatterns
{
    // bank account interface
    public interface IBankAccount
    {
        // core functions
        void Deposit(int amount);
        // bool to check the status (against an overdraft limit)
        bool Withdraw(int amount);
        string ToString();
    }

    // concrete class
    public class BankAccount : IBankAccount
    {
        private int balance;
        // max overdraft we allow
        private int overdraftLimit = -1000;

        // deposit
        public void Deposit(int amount)
        {
            // add the amount
            balance += amount;
            // notify
            WriteLine($"Deposited £{amount}, balance = £{balance}");
        }

        public bool Withdraw(int amount)
        {
            // if we don't exceed the overdraft limit after withdrawal
            if (balance - amount >= overdraftLimit)
            {
                // subtract the amount
                balance -= amount;
                // notify
                WriteLine($"Withdrew £{amount}, balance = £{balance}");
                // success flag
                return true;
            }
            // failure flag
            // (consider adding a failure notification)
            return false;
        }

        // display format
        public string ToString()
        {
            return $"{nameof(balance)}: £{balance}";
        }
    }

    public class Log<T> : DynamicObject
        where T : class, new() // a class with a default ctor
    {
        private readonly T subject;
        // mapping from the function name to the number of calls
        private Dictionary<string, int> callCount = new Dictionary<string, int>();

        // ctor
        protected Log(T subject)
        {
            this.subject = subject;
        }
        
        // expose an arbitrary interface with a factory method
        public static I As<I>() where I : class // technically I has to be an interface
        {
            if (!typeof(I).IsInterface)
            {
                throw new ArgumentException("ERROR: I must be an interface type!");
            }

            // return a new instance of itself constructing a new T object acting like I (uses impromptu interface)
            return new Log<T>(new T()).ActLike<I>();
        }
        
        // factory method that makes the proxy take on the required interface
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            try
            {
                // debug message
                WriteLine($"Invoking {subject.GetType().Name}.{binder.Name} with args ({string.Join(", ", args)})");
                // if the dictionary contains the key
                if (callCount.ContainsKey(binder.Name))
                {
                    // increment the corresponding count
                    callCount[binder.Name]++;
                }
                else
                {
                    // add the name -> 1 to the dictionary (first call)
                    callCount.Add(binder.Name, 1);
                }
                // invoke the method
                result = subject.GetType().GetMethod(binder.Name).Invoke(subject, args);
                // success! :)
                return true;
            }
            catch
            {
                // failure state
                result = null;
                return false;
            }
        }

        // info on function calls
        public string Info
        {
            get
            {
                // initialise a string builder
                var s = new StringBuilder();
                // foreach element in the dictionary
                foreach (var elem in callCount)
                {
                    // append info on the key (function) and value (number of calls)
                    s.AppendLine($"{elem.Key} called {elem.Value} times(s).");
                }

                return s.ToString();
            }
        }

        // output format
        public override string ToString()
        {
            return $"{Info}:\n{subject}";
        }
    }
    
    public class DriverCode
    {
        static void Main(string[] args)
        {
            var b = Log<BankAccount>.As<IBankAccount>();
            
            // perform some random operations (hopefully not how your banker does stuff!)
            b.Deposit(100);
            b.Withdraw(50);
            b.Deposit(25);
            b.Withdraw(2500);
            WriteLine();
            // final result
            WriteLine(b.ToString());
        }
    }
}