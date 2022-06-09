import Foundation

// OBSERVER PATTERN
// for event-driven programming
// events notify of a change in state, which can be handled appropriately by subscribers
// Observer: the object that subscribes to be informed about a state change
// PROPERTY OBSERVERS (built-in)

class Person
{
    var age: Int = 0
    {
        // fired before the value is set
        willSet(newVal)
        {
            print("Setting the age to \(newVal)")
        }
        // fired after the value is set
        didSet
        {
            print("Age changed from \(oldValue) to \(age)")
        }
    }
}

class DriverCode
{
    init()
    {
        let p = Person()
        p.age = 16
        p.age = 20    
    }
}

func main()
{
    let _ = DriverCode()
}

main()