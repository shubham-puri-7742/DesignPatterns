// SINGLETON (the much-hated)
// Used for components for which only one instance is sensible
// E.g. database repositories, factories
// Or anywhere the constructor is expensive
// Needs to be lazy and thread-safe, and prevent copying
// INNER STATIC SINGLETON IMPLEMENTATION
// exposes a static member from a nested class
// thread-safe by design

class InnerStaticSingleton {
    private InnerStaticSingleton() {}
    
    // inner class
    private static class Implementation {
        // instance initialised with the outer class ctor
        private static final InnerStaticSingleton INSTANCE = new InnerStaticSingleton();
    }

    // getter
    public static InnerStaticSingleton getInstance() {
        return Implementation.INSTANCE;
    }
}

class DriverCode {
    public static void main(String[] args) throws Exception {
        // create threads to try to instantiate two lazy singletons in parallel
        Thread thread1 = new Thread(() -> {
            System.out.println("Thread 1 running...");
            InnerStaticSingleton InnerStaticSingleton1 = InnerStaticSingleton.getInstance();
            System.out.println(InnerStaticSingleton1);
        });
        Thread thread2 = new Thread(() -> {
            System.out.println("Thread 2 running...");
            InnerStaticSingleton InnerStaticSingleton2 = InnerStaticSingleton.getInstance();
            System.out.println(InnerStaticSingleton2);
        });

        // tun both threads in parallel
        thread1.start();
        thread2.start();
    }
}