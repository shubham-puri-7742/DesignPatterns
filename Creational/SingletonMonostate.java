// SINGLETON (the much-hated)
// Used for components for which only one instance is sensible
// E.g. database repositories, factories
// Or anywhere the constructor is expensive
// Needs to be lazy and thread-safe, and prevent copying
// MONOSTATE IMPLEMENTATION for the CEO
// stored data becomes static
// you can have many instances, but the data is shared
// Limitations: Possibly deceptive, looking at the rest of the code, which seems to create different instances

class CEO {
    private static String name;
    private static int age;

    public String getName() {
        return name;
    }

    public void setName(String name) {
        CEO.name = name;
    }

    public int getAge() {
        return age;
    }

    public void setAge(int age) {
        CEO.age = age;
    }

    @Override
    public String toString() {
        return "CEO{" +
                "name='" + name + '\'' +
                ", age=" + age +
                '}';
    }
}

class DriverCode {
    public static void main(String[] args) throws Exception {
        // instance 1
        CEO ceo1 = new CEO();
        ceo1.setName("Jon Doe");
        ceo1.setAge(55);
        System.out.println("CEO 1\n" + ceo1);

        // instance 2
        CEO ceo2 = new CEO();
        ceo2.setName("Don Joe");
        ceo2.setAge(52);
        System.out.println("CEO 2\n" + ceo1);

        // compare the two
        System.out.println("\nCHECKING BACK:");
        System.out.println("CEO 1\n" + ceo1);
        System.out.println("CEO 2\n" + ceo1);
    }
}