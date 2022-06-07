import Foundation

// FLYWEIGHT PATTERN
// Space optimisation by storing some data associated with similar objects externally.
// e.g. repeated names

class User
{
    var name: String
    
    init(_ name: String)
    {
        self.name = name
    }

    var charCount: Int
    {
        return name.utf8.count
    }
}

// flyweight
class User2
{
    static var strings = [String]()
    private var nameIdxs = [Int]() // array of indices to reconstruct names

    init(_ name: String)
    {
        func getOrAdd(_ s: String) -> Int
        {
            // if i exists in the strings array, return its index
            if let i = type(of: self).strings.index(of: s)
            {
                return i
            }
            else
            {
                // append the string to the string array
                type(of: self).strings.append(s)
                // and return its index
                return type(of: self).strings.count - 1
            }
        }
        // names is an array of indices to the strings array
        nameIdxs = name.components(separatedBy: " ").map{ (getOrAdd($0)) }
    }

    static var charCount: Int
    {
        return strings.map{ $0.utf8.count }.reduce(0, +)
    }
}

func main()
{
    let user1 = User("Jon Doe")
    let user2 = User("Jane Doe")
    let user3 = User("Tom Smith")
    let user4 = User("Jane Smith")

    let totalChars = user1.charCount + user2.charCount + user3.charCount + user4.charCount
    print("Total chars: \(totalChars)")

    let user5 = User2("Jon Doe")
    let user6 = User2("Jane Doe")
    let user7 = User2("Tom Smith")
    let user8 = User2("Jane Smith")
    print("Total chars: \(User2.charCount)")
}

main()