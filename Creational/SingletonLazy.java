// SINGLETON (the much-hated)
// Used for components for which only one instance is sensible
// E.g. database repositories, factories
// Or anywhere the constructor is expensive
// Needs to be lazy and thread-safe, and prevent copying
// LAZY SINGLETON IMPLEMENTATION
// Only initialised when needed

class LazySingleton {
    private static LazySingleton instance;

    private LazySingleton() {
        System.out.println("Initialising...");
    }

    // synchronised = thread-safe (so parallel threads don't create separate instances
    // SIMPLE method (making it synchronised has a performance impact)
    /*
    public static synchronized LazySingleton getInstance() {
        // initialise if null
        if (instance == null)
            instance = new LazySingleton();
        return instance;
    }
    */

    // double-checked locking - textbook method
    public static LazySingleton getInstance() {
        if (instance == null) {
            synchronized (LazySingleton.class) {
                if (instance == null) {
                    instance = new LazySingleton();
                }
            }
        }
        return instance;
    }
}

class DriverCode {
    public static void main(String[] args) throws Exception {
        // create threads to try to instantiate two lazy singletons in parallel
        Thread thread1 = new Thread(() -> {
            System.out.println("Thread 1 running...");
            LazySingleton lazySingleton1 = LazySingleton.getInstance();
            System.out.println(lazySingleton1);
        });
        Thread thread2 = new Thread(() -> {
            System.out.println("Thread 2 running...");
            LazySingleton lazySingleton2 = LazySingleton.getInstance();
            System.out.println(lazySingleton2);
        });

        // tun both threads in parallel
        thread1.start();
        thread2.start();
    }
}