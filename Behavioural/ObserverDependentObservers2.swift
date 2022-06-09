import Foundation

// OBSERVER PATTERN
// for event-driven programming
// events notify of a change in state, which can be handled appropriately by subscribers
// Observer: the object that subscribes to be informed about a state change
// PROPERTY OBSERVERS (built-in)
// Major limitation: Work only when the property has a setter
// Overcoming that limitation with the simple event observers (see that script for more)
// The example uses a dependent property
// Variant without the Swift property observers

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

// reference wrapper for a boolean
final class RefBool
{
    var value: Bool
    init(_ value: Bool)
    {
        self.value = value
    }
}

// person class (version 2.0)
class Person
{
    private var _age: Int = 0

    var age: Int
    {
        get { return _age }
        set(newVal)
        {
            if (_age == newVal) { return }

            // cache the dependent properties
            let oldCanVote = canVote

            // we could simply perform the action and raise the event
            // but here, we allow other parts of the system can cancel the change
            
            // whether some other event has issued a cancel request (default: no)
            var cancel = RefBool(false)
            propertyChanging.raise(("age", newVal, cancel))

            // if a cancel is requested
            if cancel.value
            {
                // abort
                return
            }

            // make the changes and notify
            _age = newVal
            propertyChanged.raise(("age", _age))

            if canVote != oldCanVote
            {
                propertyChanged.raise(("canVote", canVote))
            }

        }
    }

    var canVote: Bool
    {
        return age >= 18
    }

    let propertyChanging = Event<(String, Any, RefBool)>() // like willSet
    let propertyChanged = Event<(String, Any)>() // like didSet
}

class DriverCode
{
    init()
    {
        let p = Person()
        p.propertyChanging.addHandler(target: self, handler: DriverCode.propChanging)
        p.propertyChanged.addHandler(target: self, handler: DriverCode.propChanged)
        p.age = 16
        p.age = 16
        p.age = 20
        p.age = 12 // disallowed in the handler 
    }

    // event handlers
    func propChanging(args: (String, Any, RefBool))
    {
        if args.0 == "age" && (args.1 as! Int) < 14
        {
            print("Cannot set age < 14")
            args.2.value = true // cancel flag
        }
    }

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