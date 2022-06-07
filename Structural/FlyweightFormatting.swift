import Foundation

// FLYWEIGHT PATTERN
// Space optimisation by storing some data associated with similar objects externally.
// e.g. string formatting (here - capitalisation)

extension String
{
    // substring function
    func substr(_ location: Int, _ length: Int) -> String?
    {
        // guard against out of bounds
        guard characters.count >= location + length else { return nil }
        // define start and end
        let start = index(startIndex, offsetBy: location)
        let end = index(startIndex, offsetBy: location + length)
        // return the substring
        return substring(with: start..<end)
    }
}

class FlyweightFormattedText: CustomStringConvertible
{
    private var text: String
    private var formatting = [TextRange]()

    init(_ text: String)
    {
        self.text = text
    }

    func getRange(_ start: Int, _ end: Int) -> TextRange
    {
        let range = TextRange(start, end)
        formatting.append(range)
        return range
    }

    var description: String
    {
        // buffer
        var s = ""
        for i in 0..<text.utf8.count
        {
            var c = text.substr(i, 1)! // ! = no problems expected
            for r in formatting
            {
                if r.covers(i) && r.cap
                {
                    c = c.capitalized
                }
            }
            s += c
        }
        return s
    }

    // flyweight
    class TextRange
    {
        var start, end: Int
        var cap = false

        init(_ start: Int, _ end: Int)
        {
            self.start = start
            self.end = end
        }

        // whether a point is covered in the range
        func covers(_ pos: Int) -> Bool
        {
            return pos >= start && pos <= end
        }

    }
}

func main()
{
    let t = FlyweightFormattedText("Some random text as a test subject... (text subject?)")
    t.getRange(10, 24).cap = true
    print(t)
}

main()