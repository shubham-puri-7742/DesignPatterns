import java.util.function.Supplier;

// DECORATOR
// Adds behaviour without altering the class or inheriting from it
// (augments the interface)
// allows adding functionality without rewriting existing code (conforms with the open-closed principle)
// and allows keeping the new functionality separate (conforms with the single-responsibility principle)
// STATIC DECORATOR COMPOSITION
// admittedly uglier than its dynamic equivalent

// interface
interface Shape {
    String info();
}

// implementers
class Circle implements Shape {
    private double radius;

    // ctors
    public Circle() {}
    public Circle(double radius) {
        this.radius = radius;
    }
    
    void scale(double factor) {
        radius *= factor;
    }

    @Override
    public String info() {
        return "A circle of radius " + radius;
    }
}

class Square implements Shape {
    private double side;

    // ctors
    public Square() {}
    public Square(double side) {
        this.side = side;
    }

    @Override
    public String info() {
        return "A square of side " + side;
    }
}

class ColouredShape<T extends Shape> implements Shape {
    private Shape shape;
    private String colour;
    
    // supplier, because we can't have shape = new T(); in Java
    public ColouredShape(Supplier<? extends T> ctor, String colour) {
        shape = ctor.get();
        this.colour = colour;
    }

    @Override
    public String info() {
        return shape.info() + " of " + colour + " colour";
    }
}

class TransparentShape<T extends Shape> implements Shape {
    private Shape shape;
    private int transparency;

    // supplier, because we can't have shape = new T(); in Java
    public TransparentShape(Supplier<? extends T> ctor, int transparency) {
        shape = ctor.get();
        this.transparency = transparency;
    }

    @Override
    public String info() {
        return shape.info() + " with " + transparency + "% transparency";
    }
}

class DriverCode {
    public static void main(String[] args) {
        // setup a simple shape
        Circle c = new Circle(10);
        System.out.println(c.info());

        // a coloured shape
        ColouredShape<Square> redSq = new ColouredShape<>(() -> new Square(5), "red");
        System.out.println(redSq.info());

        // a coloured transparent shape (a composite)
        TransparentShape<ColouredShape<Circle>> transGreenCirc = new TransparentShape<>(() -> new ColouredShape<>(() -> new Circle(7), "green"), 25);
        System.out.println(transGreenCirc.info());
        // we CAN'T transGreenCirc.scale(2);
    }
}