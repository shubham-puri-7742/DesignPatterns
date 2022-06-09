import Foundation

// ADAPTER
// adapts an interface you've got to conform to the one you need
// Adapter with caching to avoid repetition

// make this hashable
class Point : CustomStringConvertible, Hashable
{
    var x, y: Int

    init(_ x: Int, _ y: Int)
    {
        self.x = x
        self.y = y
    }

    var description: String
    {
        return "(\(x), \(y))"
    }

    // hash value and == override required for hashables

    var hashValue: Int
    {
        // x times a prime XOR'd with y. Hopefully sufficiently unique
        return (x * 397) ^ y
    }

    static func == (lhs: Point, rhs: Point) -> Bool
    {
        return lhs.x == rhs.x && lhs.y == rhs.y
    }
}

// make this hashable
class Line : CustomStringConvertible, Hashable
{
    var start, end: Point

    init(_ start: Point, _ end: Point)
    {
        self.start = start
        self.end = end
    }

    var description: String
    {
        return "Line from \(start) to \(end)"
    }

    // hash value and == override required for hashables

    var hashValue: Int
    {
        // x times a prime XOR'd with y. Hopefully sufficiently unique
        return (start.hashValue * 397) ^ end.hashValue
    }

    static func == (lhs: Line, rhs: Line) -> Bool
    {
        return lhs.start == rhs.start && lhs.end == rhs.end
    }
}

class VectorObject : Sequence
{
    var lines = [Line]()

    // iterator pattern
    func makeIterator() -> IndexingIterator<Array<Line>>
    {
        return IndexingIterator(_elements: lines)
    }
}

class VectorRect : VectorObject
{
    init(_ x: Int, _ y: Int, _ w: Int, _ h: Int)
    {
        super.init()
        // edges
        lines.append(Line(Point(x, y), Point(x + w, y)))
        lines.append(Line(Point(x + w, y), Point(x + w, y + h)))
        lines.append(Line(Point(x, y), Point(x, y + h)))
        lines.append(Line(Point(x, y + h), Point(x + w, y + h)))
    }
}

// adapter
class LineToPoint : Sequence
{
    private static var count = 0
    var hash: Int

    // add a cache here
    // key: hash, val: point
    static var cache = [Int: [Point]]()

    init(_ line: Line)
    {
        hash = line.hashValue
        // return if we've already done this
        if (type(of: self)).cache[hash] != nil { return }

        // increment the count
        type(of: self).count += 1
        
        // convert a line to points
        print("\(type(of: self).count): Generating points for [\(line.start.x), \(line.start.y)] - [\(line.end.x), \(line.end.y)]")

        // key points and dx, dy
        let left = Swift.min(line.start.x, line.end.x)
        let right = Swift.max(line.start.x, line.end.x)
        let top = Swift.min(line.start.y, line.end.y)
        let bottom = Swift.max(line.start.y, line.end.y)
        let dx = right - left
        let dy = line.end.y - line.start.y

        var points = [Point]()

        // vertical
        if dx == 0
        {
            for y in top...bottom
            {
                points.append(Point(left, y))
            }
        }
        // horizontal
        else if dy == 0
        {
            for x in left...right
            {
                points.append(Point(x, top))
            }
        }

        // cache the points
        type(of: self).cache[hash] = points
    }

    func makeIterator() -> IndexingIterator<Array<Point>>
    {
        // return the latest hash value (for this run) here (hackish)
        return IndexingIterator(_elements: type(of: self).cache[hash]!)
    }
}

// the target interface
func drawPoint(_ p: Point)
{
    print(".", terminator: "")
}

let VectorObjects = [
    VectorRect(1, 1, 10, 10),
    VectorRect(4, 5, 7, 9)
]

func draw()
{
    for v in VectorObjects
    {
        for line in v
        {
            // use the adapter
            let adapter = LineToPoint(line)
            adapter.forEach{ drawPoint($0) }
        }
    }
}

func main()
{
    // call twice to illustrate repetition
    draw()
    draw()
}

main()