import Foundation

// DEPENDENCY INVERSION PRINCIPLE
// 1. Higher-level modules should not depend on lower-level modules
// but both should depend on abstractions
// 2. Abstractions should not depend on details but details should depend on abstractions
// This enables loose coupling

enum Relationship
{
    case parent
    case child
    case sibling
}

class Person
{
    var name = ""
    init(_ name: String)
    {
        self.name = name
    }
}

// abstraction
protocol RelationshipBrowser
{
    func findAllChildren(_ name: String) -> [Person]
}

// low-level
class Relationships : RelationshipBrowser
{
    private var relations = [(Person, Relationship, Person)]()

    func addParentChild(_ p: Person, _ c: Person)
    {
        relations.append((p, .parent, c))
        relations.append((c, .child, p))
    }

    // implemented the abstraction
    func findAllChildren(_ name: String) -> [Person]
    {
        return relations.filter { $0.name == name && $1 == .parent && $2 === $2 }.map { $2 }
    }
}

// high-level
class Research
{
    init(_ browser: RelationshipBrowser)
    {
        for c in browser.findAllChildren("John")
        {
            print("John is \(c.name)'s parent")
        }
    }
}

func main()
{
    let parent = Person("John")
    let child1 = Person("Mary")
    let child2 = Person("Ed")

    let relationships = Relationships()
    relationships.addParentChild(parent, child1)
    relationships.addParentChild(parent, child2)

    let _ = Research(relationships)
}

main()