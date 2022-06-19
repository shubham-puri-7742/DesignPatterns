import java.util.ArrayList;
import java.util.List;

// MEDIATOR
// Facilitates communication between components by letting the components be unaware of each other's presence or absence.
// e.g. people in a chat room or players in an MMORPG
// direct references can be disastrous because they may go dead (null) any time
// The mediator, as a central component that coordinates the communication between the components, comes to the rescue 
// CHAT ROOM EXAMPLE
// The chat room is a mediator

// user data
class Person {
    public String name;
    // reference to the chatroom (mediator)
    public ChatRoom room;
    private List<String> chatLog = new ArrayList<>();

    // a person may be booted but must retain the name
    public Person(String name) {
        this.name = name;
    }

    // say to all
    public void say(String msg) {
        room.broadcast(name, msg);
    }

    // private message
    public void pm(String to, String msg) {
        room.message(name, to, msg);
    }

    // receive message
    public void receive(String sender, String msg) {
        // message format
        String s = sender + ": '" + msg + "'";
        System.out.println("[" + name + "'s session] " + s);
        // add to the log
        chatLog.add(s);
    }
}

class ChatRoom {
    // list of members
    private List<Person> people = new ArrayList<>();

    // method for joining
    public void join(Person p) {
        String joinMsg = p.name + " has joined the room.";
        broadcast("Server:", joinMsg);

        p.room = this;
        people.add(p);
    }

    // say to all
    public void broadcast(String src, String msg) {
        // invoke receive on every person except the sender
        for (Person p : people)
            if (!p.name.equals(src))
                p.receive(src, msg);
    }

    // private message
    public void message(String src, String to, String msg) {
        // STEPWISE
        // convert people to a stream
        // filter all names that equal 'to'
        // get the first (in a real world situation, we'd need additional code to avoid duplicate names OR send based on username rather than name)
        // if found, invoke receive on the recipient
        people.stream().filter(p -> p.name.equals(to)).findFirst().ifPresent(p -> p.receive(src, msg));
    }
}

class DriverCode {
    public static void main(String[] args) throws Exception {
        // initialise the room and two users
        ChatRoom room = new ChatRoom();
        Person john = new Person("John");
        Person jane = new Person("Jane");

        // two users join
        room.join(john);
        room.join(jane);

        // broadcasts
        john.say("Howdy fellas?");
        jane.say("Hey john!");

        // third person (test sessions)
        Person ed = new Person("Ed");
        room.join(ed);
        ed.say("What'd I miss?");

        // test private messages
        ed.pm("John", "Hey, how's it going?");
        john.pm("Ed", "Glad to have you here!");
    }
}