import Foundation

// SINGLE RESPONSIBILITY PRINCIPLE DEMO
// Separating Persistence from the Journal structure

// maintains the journal
class Journal : CustomStringConvertible
{
    var entries = [String]()
    var count = 0;

    func addEntry(_ text: String) -> Int
    {
        count += 1
        entries.append("\(count): \(text)")
        // return the index
        return count - 1
    }

    func removeEntry(_ index: Int)
    {
        entries.remove(at: index)
    }

    var description: String
    {
        return entries.joined(separator: "\n")
    }
}

// persistence - a separate class (NOT merged into Journal)
class persistence
{
    func saveToFile(_ journal: Journal, _ filename: String, _ overwrite: Bool = false)
    {
        // save code here
        let path = FileManager.default.urls(for: .documentDirectory, in: .userDomainMask)[0].appendingPathComponent(filename)

        if let data = journal.description.data(using: .utf8)
        {
            if overwrite
            {
                try? data.write(to: path, options: [.atomic])
            }
            else
            {
                try? data.write(to: path, options: [.atomic, .completeFileProtection])
            }
        }
    }
}

// sample driver
func main()
{
    let journal = Journal()
    journal.addEntry("Some entry.")
    journal.addEntry("Another entry.")
    journal.addEntry("Yet another entry.")
    journal.addEntry("Filling up the entries.")
    let last = journal.addEntry("Last entry here")
    print("Full journal:\n\(journal)\n")

    journal.removeEntry(last)
    print("Removed last:\n\(journal)\n")

    journal.removeEntry(2)
    print("Removed 2:\n\(journal)\n")

    let p = Persistence();
    let filename = "journal.txt"
    p.saveToFile(journal, filename, false)
}

main()