#define _USE_MATH_DEFINES
#include<iostream>
#include<cmath>
using namespace std;

// FACTORIES
// a component that creates other components wholesale
// effectively outsourcing object creation for when object creation logic becomes convoluted
// can be a factory method (a separate, possibly static, function)
// or a factory class (in compliance with the single-responsible principle)
// or a hierarchy of factories with an abstract factory

// The INNER FACTORY here solves the issues of having public ctors (or violating the open-closed principle with a friend class) when having a separate factory class
// (for OOP purists only!)

enum class CoordinateSystem {
    Cartesian,
    Polar
};

class Point {
    Point(double x, double y) : x{(abs(x - round(x)) < 1e-10) ? round(x) : x}, y{(abs(y - round(y)) < 1e-10) ? round(y) : y} {}

    // factory class - inner classes can access private members of the outer class. Exists as a singleton (see the singleton pattern code)
    class PointFactory {
        PointFactory() {}
    public:
        // factory methods (these methods, with private ctors, inside the Point class are the classic factory method pattern)
        static Point createCartesian(double x, double y) {
            return {x, y};
        }
        static Point createPolar(double rho, double theta) {
            return {rho * cos(theta), rho * sin(theta)};
        }
    };

public:
    double x, y;

    friend ostream& operator<<(ostream& o, const Point& p) {
        o << "(" << p.x << ", " << p.y << ")";
        return o;
    }

    // this static member exposes the point factory (the inner class) as a singleton
    static PointFactory Factory;
};

int main() {
    auto p1 = Point::Factory.createPolar(1, M_PI_2);
    auto p2 = Point::Factory.createCartesian(4, 5);

    cout << p1 << '\n' << p2 << endl;
    return 0;
}