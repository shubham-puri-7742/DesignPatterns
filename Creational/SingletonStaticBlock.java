import java.io.*;

// SINGLETON (the much-hated)
// Used for components for which only one instance is sensible
// E.g. database repositories, factories
// Or anywhere the constructor is expensive
// Needs to be lazy and thread-safe, and prevent copying
// STATIC BLOCK SINGLETON IMPLEMENTATION
// for when the ctor may have exceptions

class StaticBlockSingleton {
    // file IO - may throw exceptions
    private StaticBlockSingleton() throws IOException {
        System.out.println("Initialising...");
        File.createTempFile(".", ".");
    }

    private static StaticBlockSingleton instance;

    // effectively a static ctor
    static {
        try {
            instance = new StaticBlockSingleton();
        } catch (Exception e) {
            System.err.println("Failed to create the singleton!");
        }
    }

    public static StaticBlockSingleton getInstance() {
        return instance;
    }
}

class DriverCode {
    public static void main(String[] args) throws Exception {
        StaticBlockSingleton singleton = StaticBlockSingleton.getInstance();
        System.out.println(singleton.getInstance());
    }
}