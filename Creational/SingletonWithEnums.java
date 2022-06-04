import java.io.*;

// SINGLETON (the much-hated)
// Used for components for which only one instance is sensible
// E.g. database repositories, factories
// Or anywhere the constructor is expensive
// Needs to be lazy and thread-safe, and prevent copying
// SINGLETON IMPLEMENTATION WITH ENUMS
// Limitations: Non-inheritable.
// Not every fully field is serialised
// (Only the name of the enum gets saved)

// singletons don't get simpler
// has a private default ctor
enum SingletonEnum {
    INSTANCE;
    
    SingletonEnum() {
        val = 394;
    }
    
    private int val;

    public int getVal() {
        return val;
    }

    public void setVal(int val) {
        this.val = val;
    }
}

class DriverCode {
    // file IO
    static void saveToFile(SingletonEnum s, String filename) throws Exception {
        try (FileOutputStream fout = new FileOutputStream(filename);
            ObjectOutputStream oout = new ObjectOutputStream(fout)) {
            oout.writeObject(s);
        }
    }

    static SingletonEnum readFromFile(String filename) throws Exception {
        try (FileInputStream fin = new FileInputStream(filename);
            ObjectInputStream oin = new ObjectInputStream(fin)) {
            return (SingletonEnum) oin.readObject();
        }
    }

    public static void main(String[] args) throws Exception {
        // create an instance
        SingletonEnum singleton = SingletonEnum.INSTANCE;
        // display the initial value
        System.out.println(singleton.getVal());
        // make a change
        singleton.setVal(343);
        // display it
        System.out.println(singleton.getVal());

        // testing the singleton behaviour
        // store to a file
        String filename = "singleton.bin";
        saveToFile(singleton, filename);

        // change the value here
        singleton.setVal(2401);

        // try to read another instance from the file
        SingletonEnum doubleton = readFromFile(filename);

        // test the results
        System.out.println("After some voodoo...");
        System.out.println(singleton.getVal());
        System.out.println(doubleton.getVal());
        System.out.println(singleton == doubleton);
    }
}