import Foundation

// FACTORIES
// a component that creates other components wholesale
// effectively outsourcing object creation for when object creation logic becomes convoluted
// can be a factory method (a separate, possibly static, function)
// or a factory class (in compliance with the single-responsible principle)
// or a hierarchy of factories with an abstract factory

class Point
{
    var x, y: Double

    init(x: Double, y: Double)
    {
        self.x = x
        self.y = y
    }

    // Swift (like Objective-C) allows overloading ctors with identical parameters with different names
    init(rho: Double, theta: Double)
    {
        x = rho * cos(theta)
        y = rho * sin(theta)

        if abs(x - round(x)) < 1e-10 { x = round(x) }
        if abs(y - round(y)) < 1e-10 { y = round(y) }
    }

    

    var description: String
    {
        return "(\(x), \(y))"
    }
}

// factory class
class PointFactory
{
    // factory methods (these methods, with private inits, inside the Point class are the classic factory method pattern)
    static func createCartesian(x: Double, y: Double) -> Point
    {
        return Point(x: x, y: y)
    }

    static func createPolar(rho: Double, theta: Double) -> Point
    {
        return Point(rho: rho, theta: theta)
    }
}

func main()
{
    var p1 = PointFactory.createPolar(rho: 1, theta: Double.pi / 2)
    var p2 = PointFactory.createCartesian(x: 4, y: 5)

    print("p1 = \(p1.description)")
    print("p2 = \(p2.description)")
}

main()