import Foundation

// BUILDER
// For complicated, piecewise constructions
// Builder provides an API for constructing a complex object step by step

// Object-orienting HTML

// VARIANT WITH A FLUENT INTERFACE (chained calls)

class HtmlElement : CustomStringConvertible
{
    var name = ""
    var text = ""
    var children = [HtmlElement]()
    private let indentSize = 2

    init(){}
    init(name: String, text: String)
    {
        self.name = name
        self.text = text
    }

    // description of a single HTML element
    private func description(_ indent: Int) -> String
    {
        var result = ""
        
        // initial indent
        let i = String(repeating: " ", count: indent)
        // opening tag name
        result += "\(i)<\(name)>\n"
        
        // if there is some text
        if !text.isEmpty
        {
            // add text after indenting further
            result += String(repeating: " ", count: indent + indentSize) + text + "\n"
        }

        // for each child
        for c in children
        {
            // recurse, indenting further
            result += c.description(indent + indentSize)
        }

        // closing tag name
        result += "\(i)</\(name)>\n"

        return result
    }

    public var description: String
    {
        return description(0)
    }
}

// The builder
class HtmlBuilder : CustomStringConvertible
{
    private let rootName: String
    var root = HtmlElement()

    init(rootName: String)
    {
        self.rootName = rootName
        root.name = rootName
    }

    // make this return a reference to the builder to make it fluent
    func addChild(name: String, text: String) -> HtmlBuilder
    {
        let e = HtmlElement(name: name, text: text)
        root.children.append(e)
        return self
    }

    var description: String
    {
        return root.description
    }

    func clear()
    {
        root = HtmlElement(name: rootName, text: "")
    }
}


func main() {
    // init the HTML
    let builder = HtmlBuilder(rootName: "ul")
    builder.addChild(name: "li", text: "hello")
            .addChild(name: "li", text: "world")
            .addChild(name: "li", text: "spam")
            .addChild(name: "li", text: "eggs")
            .addChild(name: "li", text: "foo")
            .addChild(name: "li", text: "bar")

    print(builder)
}

main()