import Foundation

// ADAPTER
// adapts an interface you've got to conform to the one you need

class Point : CustomStringConvertible
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
}

class Line : CustomStringConvertible
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
    var points = [Point]()

    init(_ line: Line)
    {
        // increment the count
        type(of: self).count += 1
        
        // convert a line to points
        print("\(type(of: self).count): Generating points for [\(line.start.x), \(line.start.y)] - [\(line.end.x), \(line.end.y)]")

        let left = Swift.min(line.start.x, line.end.x)
        let right = Swift.max(line.start.x, line.end.x)
        let top = Swift.min(line.start.y, line.end.y)
        let bottom = Swift.max(line.start.y, line.end.y)
        let dx = right - left
        let dy = line.end.y - line.start.y

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
    }

    func makeIterator() -> IndexingIterator<Array<Point>>
    {
        return IndexingIterator(_elements: points)
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
    draw()
    draw()
}

main()