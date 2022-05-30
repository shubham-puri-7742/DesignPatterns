import copy

# PROTOTYPE FACTORY PATTERN
# Use a prototype (partially or fully initialised object) to create new objects
# Copy existing designs and customise it
# Requires a deepcopy
# Additionally wrap the deepcopy logic into a separate factory

# This example uses two classes because objects are reference variables

class Address:
    def __init__(self, street, city, country):
        self.street = street
        self.city = city
        self.country = country

    def __str__(self):
        return f'{self.street},\n{self.city},\n{self.country}\n'

class Person:
    def __init__(self, name, address):
        self.name = name
        self.address = address

    def __str__(self):
        return f'Name: {self.name}\nAddress:\n{self.address}\n'

class PersonFactory:
    # some useful, reusable templates
    cardiganer = Person('', Address('', 'Cardigan', 'UK'))
    londoner = Person('', Address('', 'London', 'UK'))

    @staticmethod
    def __newPerson(prototype, name, street):
        # use the prototype
        result = copy.deepcopy(prototype)
        # customise it
        result.name = name
        result.address.street = street
        return result

    # factory methods
    @staticmethod
    def newCardiganer(name, street):
        return PersonFactory.__newPerson(PersonFactory.cardiganer, name, street)
    
    @staticmethod
    def newLondoner(name, street):
        return PersonFactory.__newPerson(PersonFactory.londoner, name, street)

if __name__ == '__main__':
    will = PersonFactory.newCardiganer('William', '123 High Street')
    nate = PersonFactory.newLondoner('Nathan', '123 Abingdon Street')
    print(will)
    print(nate)