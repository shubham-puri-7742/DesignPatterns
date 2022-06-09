import Foundation

// OBSERVER PATTERN
// for event-driven programming
// events notify of a change in state, which can be handled appropriately by subscribers
// Observer: the object that subscribes to be informed about a state change
// Built into Swift only for properties

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

class Person
{
    // event
    let fallsIll = Event<String>()

    init() {}

    func catchCold()
    {
        // raise the event
        // => send out a notification
        fallsIll.raise("Doc @ 123 Cardiff Road")
    }
}

class DriverCode
{
    init()
    {
        let p = Person()
        // subscription
        let sub = p.fallsIll.addHandler(target: self, handler: DriverCode.callDoc)

        // test subscription
        p.catchCold()

        // test unsubscription
        sub.dispose()
        p.catchCold()
    }

    func callDoc(address: String)
    {
        print("We need a doctor. \(address)")
    }
}

func main()
{
    let _ = DriverCode()
}

main()