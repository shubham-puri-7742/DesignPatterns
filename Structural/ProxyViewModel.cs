using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using static System.Console;

// PROXY
// A class that acts as an interface to a particular resource by replicating an interface
// => Identical interfaces implementing different behaviour
// Similar to the decorator pattern but replicates the target interface instead of enhancing it
// VIEW MODEL
// A representation of the data to be shown with additional functionality (validation, notification, etc.)
// (From the MVVM pattern)
// 1. Model (internal representation of data)
// 2. View (user interface)
// 3. View-Model (representation of the data shown on the UI)

namespace DesignPatterns
{
    public class Person
    {
        public string FirstName, LastName;
    }
    
    // view model - a proxy over the internal representation
    public class PersonViewModel : INotifyPropertyChanged
    {
        private readonly Person p;
        
        public PersonViewModel(Person p)
        {
            this.p = p;
        }

        // replicate the fields as properties
        // so that they provide change notifications
        // these properties could be bound to the UI
        // first name property
        public string FirstName
        {
            // get normally
            get => p.FirstName;
            // set only if changed; notify as such
            set
            {
                if (p.FirstName == value) return;
                p.FirstName = value;
                OnPropertyChanged();
                // because full name is affected by the first name
                OnPropertyChanged(nameof(FullName));
            }
        }
        
        // last name property
        public string LastName
        {
            // get normally
            get => p.LastName;
            // set only if changed; notify as such
            set
            {
                if (p.LastName == value) return;
                p.LastName = value;
                OnPropertyChanged();
                // because full name is affected by the last name
                OnPropertyChanged(nameof(FullName));
            }
        }
        
        // taking this into decorator territory by augmenting the interface, but okay
        public string FullName
        {
            get => $"{FirstName} {LastName}".Trim();
            set
            {
                // null check
                if (value == null)
                {
                    FirstName = LastName = null;
                    return;
                }
    
                // split on space
                var elems = value.Split();
                // if there is at least one element
                if (elems.Length > 0)
                {
                    // make the first element the first name
                    FirstName = elems[0];
                }
                // if there are at least two elements
                if (elems.Length > 1)
                {
                    // make the second element the last name
                    LastName = elems[1];
                }
            }
        }

        // property changed event
        public event PropertyChangedEventHandler PropertyChanged;
        
        // on property changed
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // invoke the property changed event
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            // Debug message
            WriteLine($"{propertyName} changed.");
        }
    }
    
    public class DriverCode
    {
        static void Main(string[] args)
        {
            var p = new Person();
            var m = new PersonViewModel(p);
            m.FirstName = "John";
            m.LastName = "Doe";
            m.FullName = "Jon Smith";
        }
    }
}