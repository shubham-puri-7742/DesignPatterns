import Foundation

// STRATEGY (POLICY) PATTERN
// Provides the ability to define system behaviour partially at runtime.
// By splitting the algorithm into high-level and low-level parts, the high-level parts can be reused.
// THIS PROGRAM IMPLEMENTS A DYNAMIC STRATEGY (changeable at runtime)

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
class TextProcessor: CustomStringConvertible
{
    private var buffer = ""
    // select strategy based on format
    private var listStrategy: ListStrategy

    // initialise with a specific config
    init(_ outFormat: OutputFormat)
    {
        switch outFormat
        {
            case .markdown: listStrategy = MarkdownListStrategy()
            case .html: listStrategy = HtmlListStrategy()
        }
    }

    // set a specific config
    func setOutFormat(_ outFormat: OutputFormat)
    {
        switch outFormat
        {
            case .markdown: listStrategy = MarkdownListStrategy()
            case .html: listStrategy = HtmlListStrategy()
        }
    }

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
    let t = TextProcessor(.markdown)

    print("MARKDOWN LIST")
    t.appendList(["spam", "eggs", "allan", "folan"])
    print(t)
    t.clear()

    print("\nHTML LIST")

    t.setOutFormat(.html)
    t.appendList(["spam", "eggs", "allan", "folan"])
    print(t)
    t.clear()
}

main()