import io.reactivex.rxjava3.core.Observable;
import io.reactivex.rxjava3.core.Observer;

import java.util.ArrayList;
import java.util.List;

// MEDIATOR
// Facilitates communication between components by letting the components be unaware of each other's presence or absence.
// e.g. people in a chat room or players in an MMORPG
// direct references can be disastrous because they may go dead (null) any time
// The mediator, as a central component that coordinates the communication between the components, comes to the rescue 
// EVENT BROKER with REACTIVE EXTENSIONS
// Mediator ultra pro max
// with reactive extensions

class EventBroker extends Observable<Integer> {
    // list of all observers
    private List<Observer<? super Integer>> observers = new ArrayList<>();
    
    // allow subscribing
    @Override
    protected void subscribeActual(Observer<? super Integer> observer) {
        // add the observer to the list
        observers.add(observer);
    }
    
    // event dispatch
    public void publish(int n) {
        // for each observer in the list
        for (Observer<? super Integer> o : observers)
            o.onNext(n); // onNext => put the event in the queue
    }
}

class FootballPlayer {
    public String name;
    private int goals = 0;
    // the mediator
    private EventBroker broker;

    // ctor


    public FootballPlayer(String name, EventBroker broker) {
        this.name = name;
        this.broker = broker;
    }

    // score function
    public void score() {
        // publish the score event for any subscribers
        broker.publish(++goals);
    }
}

// a subscriber
class Coach {
    // ctor
    public Coach(EventBroker b) {
        b.subscribe(i -> {
            System.out.println("You scored: " + i + " goals!");
        });
    }
}

class DriverCode {
    public static void main(String[] args) throws Exception {
        // setup variables
        EventBroker b = new EventBroker();
        FootballPlayer p1 = new FootballPlayer("Will", b);
        FootballPlayer p2 = new FootballPlayer("Nate", b);
        Coach c = new Coach(b);
        
        // test scoring
        p1.score();
        p2.score();
        p2.score();
    }
}