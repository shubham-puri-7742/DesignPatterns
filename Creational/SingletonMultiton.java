import java.util.HashMap;

// SINGLETON (the much-hated)
// Used for components for which only one instance is sensible
// E.g. database repositories, factories
// Or anywhere the constructor is expensive
// Needs to be lazy and thread-safe, and prevent copying
// MULTITON (finite set of instances) IMPLEMENTATION
// each instance may come with additional features (e.g. laziness)

// we can have up to 3 instances - singletons for each element of this enum
// key for the hashmap (see implementation of the Printer class)
enum Subsystem {
    PRIMARY,
    AUX,
    FALLBACK
}

// the multiton - basically a lazy key-value store
class Printer {
    private static int instanceCount = 0;
    private Printer() {
        System.out.println("Instance " + ++instanceCount + " created.");
    }
    
    // hashmap of instances
    private static HashMap<Subsystem, Printer> instances = new HashMap<>();
    
    // get a singleton for the specified subsystem
    public static Printer getInstance(Subsystem s) {
        if (instances.containsKey(s))
            return instances.get(s);

        // laziness
        Printer instance = new Printer();
        instances.put(s, instance);
        return instance;
    }
    
}

class DriverCode {
    public static void main(String[] args) throws Exception {
        // step through this code to see each line in action
        Printer main = Printer.getInstance(Subsystem.PRIMARY);
        Printer aux = Printer.getInstance(Subsystem.AUX);
        Printer aux2 = Printer.getInstance(Subsystem.AUX);
        Printer main2 = Printer.getInstance(Subsystem.AUX);
        // UNCOMMENT TO CREATE THE THIRD INSTANCE
        // Printer fallback = Printer.getInstance(Subsystem.FALLBACK);
    }
}