// DECORATOR
// Adds behaviour without altering the class or inheriting from it
// (augments the interface)
// allows adding functionality without rewriting existing code (conforms with the open-closed principle)
// and allows keeping the new functionality separate (conforms with the single-responsibility principle)
// DYNAMIC DECORATOR COMPOSITION

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

// this decorator adds a colour property
class ColouredShape implements Shape {
    private Shape shape;
    private String colour;

    public ColouredShape(Shape shape, String colour) {
        this.shape = shape;
        this.colour = colour;
    }

    @Override
    public String info() {
        return shape.info() + " of " + colour + " colour";
    }
}

// this decorator adds a transparency property
class TransparentShape implements Shape {
    
    private Shape shape;
    private int transparency;

    public TransparentShape(Shape shape, int transparency) {
        this.shape = shape;
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
        ColouredShape redSq = new ColouredShape(new Square(5), "red");
        System.out.println(redSq.info());
        
        // a coloured transparent shape (a composite)
        TransparentShape transGreenCirc = new TransparentShape(new ColouredShape(new Circle(7), "green"), 25);
        System.out.println(transGreenCirc.info());
        // we CAN'T transGreenCirc.scale(2);
    }
}