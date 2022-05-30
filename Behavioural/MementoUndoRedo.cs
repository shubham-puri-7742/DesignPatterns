using System.Collections.Generic;
using System.ComponentModel.Design;
using static System.Console;

// MEMEMTO PATTERN
// Saves the state of an object to allow returning to it (facilitates undo/redo). Also see the command pattern code.
// The memento is typically immutable that may or may not expose state information
// COMPLETE UNDO/REDO IMPLEMENTATION

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
        private List<Memento> changelog = new List<Memento>();
        // used as an index to an element in the list
        // move back when undoing and forward when redoing
        private int currentState;

        public BankAccount(int balance)
        {
            this.balance = balance;
            // add the initial state to the changelog
            changelog.Add(new Memento(balance));
        }
        
        // return a memento from a mutator function
        public Memento Deposit(int amount)
        {
            balance += amount;
            // add a memento the the changelog
            var m = new Memento(balance);
            changelog.Add(m);
            // increment the current state
            ++currentState;
            // return the memento
            return m;
        }

        // restores the balance to that in the memento
        public Memento Restore(Memento m)
        {
            // null check - when an undo is attempted when there is nothing to undo, the memento returns null
            if (m != null)
            {
                // restore the balance
                balance = m.balance;
                // add the rollback to the changelog
                changelog.Add(m);
                return m;
            }

            return null;
        }

        public Memento Undo()
        {
            if (currentState > 0)
            {
                // get the token at the current state index and decrement the index
                var m = changelog[--currentState];
                balance = m.balance;
                return m;
            }
            // null when there is nothing to undo
            return null;
        }

        public Memento Redo()
        {
            // if we are not already at the last element
            if (currentState < changelog.Count - 1)
            {
                // get the token at the current state index and increment the index
                var m = changelog[++currentState];
                balance = m.balance;
                return m;
            }
            // null when there is nothing to redo
            return null;
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
            b.Deposit(10);
            b.Deposit(15);
            b.Deposit(25);
            b.Deposit(50);
            WriteLine(b);

            b.Undo();
            WriteLine($"Undo 1: {b}");
            
            b.Undo();
            WriteLine($"Undo 2: {b}");
            
            b.Undo();
            WriteLine($"Undo 3: {b}");
            
            b.Redo();
            WriteLine($"Undo 1: {b}");
            
            b.Redo();
            WriteLine($"Undo 2: {b}");
        }
    }
}