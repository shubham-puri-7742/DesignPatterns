import copy

# PROTOTYPE PATTERN
# Use a prototype (partially or fully initialised object) to create new objects
# Copy existing designs and customise it
# Requires a deepcopy

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
        return f'Name: {self.name}\nAddress:\n{self.address}\n\n'

if __name__ == '__main__':
    # works as a prototype
    sherlock = Person('Sherlock Holmes', Address('221B Baker St', 'London', 'UK'))

    watson = copy.deepcopy(sherlock)
    watson.name = 'John Watson'
    watson.address.street = '221C Baker St'

    print(sherlock)
    print(watson)