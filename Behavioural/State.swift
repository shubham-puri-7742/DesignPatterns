import Foundation

// STATE PATTERN (STATE MACHINE)
// Object behaviour is determined by its state
// Objects transition from one state to another
// State machine: Manages states and transitions

// hypothetical phone program

// states
enum State
{
    case offHook
    case connecting
    case connected
    case onHold
}

// triggers for transitions
enum Trigger
{
    case dialled
    case hungUp
    case connected
    case onHold
    case offHold
    case leftMessage
}

// state machine - rules for transitions
let rules = [
    State.offHook: [
        (Trigger.dialled, State.connecting)
    ],
    State.connecting: [
        (Trigger.hungUp, State.offHook),
        (Trigger.connected, State.connected)
    ],
    State.connected: [
        (Trigger.onHold, State.onHold),
        (Trigger.leftMessage, State.offHook),
        (Trigger.hungUp, State.offHook)
    ],
    State.onHold: [
        (Trigger.offHold, State.connected),
        (Trigger.hungUp, State.offHook)
    ]
]

func main()
{
    // initial state
    var state = State.offHook

    // infinite loop
    while true
    {
        // current state
        print("The phone is currently \(state)")
        print("Select a trigger: ")

        // for each rule
        for i in 0..<rules[state]!.count
        {
            // show the trigger
            let (t, _) = rules[state]![i]
            print("\(i). \(t)")
        }

        // get input
        if let input = Int(readLine()!)
        {
            // transition to that state
            let (_, s) = rules[state]![input]
            state = s

            if state == State.offHook
            {
                break
            }
        }
        print()
    }
}

main()