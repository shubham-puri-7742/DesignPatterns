using static System.Console;

// MEMEMTO PATTERN
// Saves the state of an object to allow returning to it (facilitates undo/redo). Also see the command pattern code.
// The memento is typically immutable that may or may not expose state information
// SIMPLE RESTORE IMPLEMENTATION
// This implementation records mementos, but not the sequence of operations

namespace DesignPatterns
{
    public class Memento
    {
        // property with a private setter (so a malicious user cannot manipulate the account)
        public int balance { get; }

        public Memento(int balance)
        {
            this.balance = balance;
        }
    }

    public class BankAccount
    {
        private int balance;

        public BankAccount(int balance)
        {
            this.balance = balance;
        }
        
        // return a memento from a mutator function
        public Memento Deposit(int amount)
        {
            balance += amount;
            return new Memento(balance);
        }

        // restores the balance to that in the memento
        public void Restore(Memento m)
        {
            balance = m.balance;
        }

        // display format
        public override string ToString()
        {
            return $"{nameof(balance)}: {balance}";
        }
    }
    public class DriverCode
    {
        static void Main(string[] args)
        {
            var b = new BankAccount(100);
            var m1 = b.Deposit(10);
            var m2 = b.Deposit(15);
            WriteLine(b);
            
            // restore to the first memento
            b.Restore(m1);
            WriteLine(b);
            
            // restore to the second memento
            b.Restore(m2);
            WriteLine(b);
        }
    }
}