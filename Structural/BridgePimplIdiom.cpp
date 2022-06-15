#include<iostream>
using namespace std;

// BRIDGE
// Connects components through abstractions
// Decouples interface from implementation
// Prevents a cartesian product complexity explosion

// PIMPL (Pointer to an IMPL) IDIOM
// hides the implementation of a class by sticking it into its implementation

// header file content
class Person {
public:
    string  name;

    // the bridge. Implemented only in the implementation file (when this class is made into a separate header-implementation file pair) and thus hidden
    class PersonImpl;
    PersonImpl* impl;

    // The implementations are deferred into a separate class
    Person(const string& name = "John Doe");
    ~Person();
    void greet();
};

// cpp file content (concealed from the API)
// including stuff here also speeds up compile times
class Person::PersonImpl {
public:
    void greet(Person* p);
};

void Person::PersonImpl::greet(Person *p) {
    cout << "Hello, my name is " << p->name << endl;
}

Person::Person(const string& name) : name{ name }, impl { new PersonImpl } {}

Person::~Person() {
    delete impl;
}

void Person::greet() {
    impl->greet(this);
}

// driver code
int main() {
    Person p;
    p.greet();
    return 0;
}