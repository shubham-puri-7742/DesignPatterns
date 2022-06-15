// BUILDER
// For complicated, piecewise constructions
// Builder provides an API for constructing a complex object step by step

// FACETED BUILDER for complex objects
// Base builder builds information separately
// Takes the idea of chaining (see the fluent builder code) to the next level

class Person : CustomStringConvertible
{
    // name
    var honourific = "", firstName = "", middleName = "", lastName = ""
    // employment info
    var title = "", company = "", location = ""
    // address
    var street = "", city = "", country = ""
    var income = 0

    var description: String
    {
        return "\(honourific) \(firstName) \(middleName) \(lastName)\n\(title) at \(company) in \(location)\nAnnual Income: £\(income)\nAddress:\n\(street),\n\(city)\n\(country)\n"
    }
}

class PersonBuilder
{
    var person = Person()

    // properties (sub builders)
    var named : PersonNameBuilder
    {
        return PersonNameBuilder(person)
    }
    var livesAt : PersonAddressBuilder
    {
        return PersonAddressBuilder(person)
    }
    var employedAt : PersonEmploymentBuilder
    {
        return PersonEmploymentBuilder(person)
    }

    // getter (because there are no implicit conversions)
    func build() -> Person
    {
        return person
    }
}

// sub-builders
// name
class PersonNameBuilder : PersonBuilder
{
    internal init(_ person: Person)
    {
        super.init()
        self.person = person
    }

    // fluency galore
    func title(_ title: String) -> PersonNameBuilder
    {
        person.honourific = title
        return self
    }
    func first(_ first: String) -> PersonNameBuilder
    {
        person.firstName = first
        return self
    }
    func middle(_ middle: String) -> PersonNameBuilder
    {
        person.middleName = middle
        return self
    }
    func last(_ last: String) -> PersonNameBuilder
    {
        person.lastName = last
        return self
    }
}

// address
class PersonAddressBuilder : PersonBuilder
{
    internal init(_ person: Person)
    {
        super.init()
        self.person = person        
    }

    // fluency galore
    func street(_ street: String) -> PersonAddressBuilder
    {
        person.street = street
        return self
    }
    func city(_ city: String) -> PersonAddressBuilder
    {
        person.city = city
        return self
    }
    func country(_ country: String) -> PersonAddressBuilder
    {
        person.country = country
        return self
    }
}

// employment info
class PersonEmploymentBuilder : PersonBuilder
{
    internal init(_ person: Person)
    {
        super.init()
        self.person = person        
    }

    // fluency galore
    func title(_ title: String) -> PersonEmploymentBuilder
    {
        person.title = title
        return self
    }
    func company(_ company: String) -> PersonEmploymentBuilder
    {
        person.company = company
        return self
    }
    func location(_ location: String) -> PersonEmploymentBuilder
    {
        person.location = location
        return self
    }
    func salary(_ income: Int) -> PersonEmploymentBuilder
    {
        person.income = income
        return self
    }
}

func main()
{
    let b = PersonBuilder()
    let p = b
            .named.first("Will")
                  .middle("H.")
                  .last("Davis")
            .livesAt.street("123 Cardiff Road")
                    .city("Cardiff")
                    .country("Wales, UK")
            .employedAt.title("Software Engineer")
                       .company("Mango")
                       .location("London, UK")
                       .salary(75000)
            .build()

    print(p)
}

main()