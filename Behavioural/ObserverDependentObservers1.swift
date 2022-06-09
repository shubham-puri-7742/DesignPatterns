import Foundation

// OBSERVER PATTERN
// for event-driven programming
// events notify of a change in state, which can be handled appropriately by subscribers
// Observer: the object that subscribes to be informed about a state change
// PROPERTY OBSERVERS (built-in)
// Major limitation: Work only when the property has a setter
// Overcoming that limitation with the simple event observers (see that script for more)
// The example uses a dependent property

protocol Invocable : class
{
    // take some data and perform an invocation on it
    func invoke(_ data: Any)
}

// for unsubscribing
protocol Disposable
{
    func dispose()
}

// generic event
class Event<T>
{
    // lambda from T to nothing
    typealias EventHandler = (T) -> ()

    // list of handlers (subscribers)
    var eventHandlers = [Invocable]()

    // functions
    // raise = notify subscribers
    func raise(_ data: T)
    {
        for h in eventHandlers
        {
            h.invoke(data)
        }
    }

    // add a subscriber
    // (TypeOfInvoker) -> (Arg) -> ()
    // escaping: allowed to escape (=> passed as an argument, but called after return)
    func addHandler <U: AnyObject>(target: U, handler: @escaping (U) -> EventHandler) -> Disposable
    {
        let sub = Subscription(target: target, handler: handler, event: self)
        eventHandlers.append(sub)
        return sub
    }
}

// subscription to an event
class Subscription<U: AnyObject, T> : Invocable, Disposable
{
    weak var target: U?
    let handler: (U) -> (T) -> ()
    var event = Event<T>()

    init(target: U?, handler: @escaping (U) -> (T) -> (), event: Event<T>)
    {
        self.target = target
        self.event = event
        self.handler = handler
    }

    func invoke(_ data: Any)
    {
        // if the target is available
        if let t = target
        {
            handler(t)(data as! T)
        }
    }

    func dispose()
    {
        // remove the handler from the set of all handlers
        event.eventHandlers = event.eventHandlers.filter{ $0 as AnyObject? !== self }
    }
}

// NEW CODE BELOW
class Person
{
    private var oldCanVote = false
    var age: Int = 0
    {
        // fired before the value is set
        willSet(newVal)
        {
            print("Setting the age to \(newVal)")
            oldCanVote = canVote
        }
        // fired after the value is set
        didSet
        {
            print("Age changed from \(oldValue) to \(age)")

            // if the age has changed
            if age != oldValue
            {
                // raise the event for the age
                propertyChanged.raise(("age", age))
            }

            // if the dependent bool has changed
            if canVote != oldCanVote
            {
                // raise the event for the bool
                propertyChanged.raise(("canVote", canVote))
            }
        }
    }

    // changing the age can possibly change this
    var canVote: Bool
    {
        return age >= 18
    }

    let propertyChanged = Event<(String, Any)>()
}

class DriverCode
{
    init()
    {
        let p = Person()
        p.propertyChanged.addHandler(target: self, handler: DriverCode.propChanged)
        p.age = 16
        p.age = 16
        p.age = 20    
    }

    // event handler
    func propChanged(args: (String, Any))
    {
        if args.0 == "age"
        {
            print("HANDLER: Age changed to \(args.1)")
        }
        else if args.0 == "canVote"
        {
            print("HANDLER: Voting status changed to \(args.1)")
        }
    }
}

func main()
{
    let _ = DriverCode()
}

main()