import java.io.*;

// SINGLETON (the much-hated)
// Used for components for which only one instance is sensible
// E.g. database repositories, factories
// Or anywhere the constructor is expensive
// Needs to be lazy and thread-safe, and prevent copying
// SIMPLE SINGLETON IMPLEMENTATION

class SimpleSingleton implements Serializable {
    // private ctor - disables creation of new objects
    private SimpleSingleton() {}

    private int val = 0;

    // create a static final (const) instance
    private static final SimpleSingleton INSTANCE = new SimpleSingleton();

    // makes sure the singleton behaviour is retained
    // always resolve into the singular instance
    // comment this out to see the singleton principle break
    protected Object readResolve() {
        return INSTANCE;
    }

    // getters and setters
    // no setter for the instance
    public static SimpleSingleton getInstance() {
        return INSTANCE;
    }

    // getter and setter for the value
    public int getVal() {
        return val;
    }

    public void setVal(int val) {
        this.val = val;
    }
}

class DriverCode {

    // file IO
    static void saveToFile(SimpleSingleton s, String filename) throws Exception {
        try (FileOutputStream fout = new FileOutputStream(filename);
            ObjectOutputStream oout = new ObjectOutputStream(fout)) {
            oout.writeObject(s);
        }
    }

    static SimpleSingleton readFromFile(String filename) throws Exception {
        try (FileInputStream fin = new FileInputStream(filename);
            ObjectInputStream oin = new ObjectInputStream(fin)) {
            return (SimpleSingleton) oin.readObject();
        }
    }

    public static void main(String[] args) throws Exception {
        // create an instance
        SimpleSingleton singleton = SimpleSingleton.getInstance();
        // display the initial value
        System.out.println(singleton.getVal());
        // make a change
        singleton.setVal(394);
        // display it
        System.out.println(singleton.getVal());

        // testing the singleton behaviour
        // store to a file
        String filename = "singleton.bin";
        saveToFile(singleton, filename);

        // change the value here
        singleton.setVal(343);

        // try to read another instance from the file
        SimpleSingleton doubleton = readFromFile(filename);

        // test the results
        System.out.println("After some voodoo...");
        System.out.println(singleton.getVal());
        System.out.println(doubleton.getVal());
        System.out.println(singleton == doubleton);
    }
}