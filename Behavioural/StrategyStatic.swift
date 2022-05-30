import Foundation

// STRATEGY (POLICY) PATTERN
// Provides the ability to define system behaviour partially at runtime.
// By splitting the algorithm into high-level and low-level parts, the high-level parts can be reused.
// THIS PROGRAM IMPLEMENTS A STATIC STRATEGY (fixed at compile time)

enum OutputFormat
{
    case markdown // lists go like
    // * 1
    // * 2
    case html // lists go like
    // <ol>
    //   <li>1</li>
    //   <li>2</li>
    // </ol>
}

// abstract protocol for printing a list
// prints a start (prefix), item, and end (postfix)
protocol ListStrategy
{
    init()
    func start(_ buffer: inout String)
    func listItem(buffer: inout String, li: String)
    func end(_ buffer: inout String)
}

// concrete strategy 1: Markdown
// no start and end
class MarkdownListStrategy: ListStrategy
{
    required init() {}
    func start(_ buffer: inout String) {}
    func listItem(buffer: inout String, li: String)
    {
        buffer += " * \(li)\n"
    }
    func end(_ buffer: inout String) {}
}

// concrete strategy 2: HTML
// has a prefix and a suffix (opening and closing tags)
class HtmlListStrategy: ListStrategy
{
    required init() {}
    func start(_ buffer: inout String)
    {
        buffer += "<ol>\n"
    }
    func listItem(buffer: inout String, li: String)
    {
        buffer += "  <li>\(li)</li>\n"
    }
    func end(_ buffer: inout String)
    {
        buffer += "<ol>\n"
    }
}

// processes list input
class TextProcessor<ls>: CustomStringConvertible where ls: ListStrategy
{
    private var buffer = ""
    // select strategy based on format
    private let listStrategy = ls()

    // make a list in the appropriate format
    func appendList(_ list: [String])
    {
        listStrategy.start(&buffer)
        for li in list
        {
            listStrategy.listItem(buffer: &buffer, li: li)
        }
        listStrategy.end(&buffer)
    }

    func clear()
    {
        buffer = ""
    }

    var description: String
    {
        return buffer
    }
}

func main()
{
    let t1 = TextProcessor<MarkdownListStrategy>()

    print("MARKDOWN LIST")
    t1.appendList(["spam", "eggs", "allan", "folan"])
    print(t1)
    t1.clear()

    print("\nHTML LIST")
    
    let t2 = TextProcessor<HtmlListStrategy>()
    t2.appendList(["spam", "eggs", "allan", "folan"])
    print(t2)
    t2.clear()
}

main()