#include<iostream>
using namespace std;

// BRIDGE
// Connects components through abstractions
// Decouples interface from implementation
// Prevents a cartesian product complexity explosion

// Bridge example for avoiding a state space (cartesian product) explosion

// base renderer (base class enables bridging, because a base class reference can point to a derived class object)
struct Renderer {
    virtual void renderCircle(float x, float y, float r) = 0;
};

// derived classes
struct VectorRenderer : Renderer {
    void renderCircle(float x, float y, float r) override {
        cout << "Vector rendering a circle of radius " << r << endl;
    }
};

struct RasterRenderer : Renderer {
    void renderCircle(float x, float y, float r) override {
        cout << "Rasterising a circle of radius " << r << endl;
    }
};

// base shape class
struct Shape {
protected:
    // bridge to the renderer
    Renderer& renderer;
    Shape(Renderer &renderer) : renderer(renderer) {}
public:
    virtual void draw() = 0;
    virtual void resize(float factor) = 0;
};

// concrete class
struct Circle : Shape {
    float x, y, r;
    Circle(Renderer &renderer, float x, float y, float r) : Shape{renderer}, x{x}, y{y}, r{r} {}

    void draw() override {
        renderer.renderCircle(x, y, r);
    }

    void resize(float factor) override {
        r *= factor;
    }
};

int main() {
    RasterRenderer r1;
    Circle rc{r1, 5, 5, 5};
    rc.draw();
    rc.resize(2);
    rc.draw();
    
    VectorRenderer r2;
    Circle vc(r2, 5, 5, 5);
    vc.draw();
    vc.resize(2);
    vc.draw();
    
    return 0;
}