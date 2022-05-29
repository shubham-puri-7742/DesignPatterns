#include <iostream>
#include <vector>
#include <string>
using namespace std;

// OPEN-CLOSED PRINCIPLE
// open to extension, closed to modification

// Also demonstrates the SPECIFICATION PATTERN

// STRUCTS
enum class Colour {
    red,
    green,
    blue
};

enum class Size {
    small,
    medium,
    large
};

struct Product {
    string name;
    Colour colour;
    Size size;
};

// TEMPLATES
// combinator forward declaration
template <typename T> struct Combinator;

// specification template
template <typename T> struct Specification {
    virtual bool satisfied(T* t) = 0;

    // overload the && operator to combine specs
    Combinator<T> operator&&(Specification<T>&& s) {
        return Combinator<T>(*this, s);
    }
};

// filter template
template <typename T> struct Filter {
    virtual vector<T*> filter(vector<T*> v, Specification<T>& s) = 0;
};

// combines two specs
template <typename T> struct Combinator : Specification<T> {
    Specification<T>& s1;
    Specification<T>& s2;

    Combinator(Specification<T>& s1, Specification<T>& s2) : s1{s1}, s2{s2} {}

    bool satisfied(T* t) override {
        return s1.satisfied(t) && s2.satisfied(t);
    }
};

// CONCRETE DERIVED CLASSES
// inheritance defines specific specifications and filters
struct ProductFilter : Filter<Product> {
    vector<Product*> filter (vector<Product*> v, Specification<Product>& s) {
        vector<Product*> result;
        // for each item in the vector
        for (auto& i : v)
            // if the specification is satisfied (match)
            if (s.satisfied(i))
                // add it to the result
                result.push_back(i);
        return result;
    }
};

struct ColourSpec : Specification<Product> {
    Colour colour;
    // ctor
    ColourSpec(Colour colour) : colour{colour} {}
    // override satisfied (match by colour)
    bool satisfied(Product* p) override {
        return p->colour == colour;
    }
};

struct SizeSpec : Specification<Product> {
    Size size;
    // ctor
    SizeSpec(Size size) : size{size} {}
    // override satisfied (match by size)
    bool satisfied(Product* p) override {
        return p->size == size;
    }
};

// DRIVER CODE
int main() {
    // sample data
    Product p1 { "Shoes", Colour::blue, Size::small };
    Product p2 { "XMas Tree", Colour::green, Size::large };
    Product p3 { "Apple", Colour::green, Size::small };
    Product p4 { "Pen", Colour::red, Size::small };
    Product p5 { "T-Shirt", Colour::red, Size::medium };

    // make a vector of it
    vector<Product*> items { &p1, &p2, &p3, &p4, &p5 };

    // make a filter
    ProductFilter f;
    // colour spec
    ColourSpec r(Colour::red);

    cout << "RED ITEMS:\n";
    for (auto& i : f.filter(items, r))
        cout << i->name << '\n';
    cout << endl;

    // size spec
    SizeSpec s(Size::small);

    cout << "SMALL ITEMS:\n";
    for (auto& i : f.filter(items, s))
        cout << i->name << '\n';
    cout << endl;

    // size and colour spec
    auto spec = ColourSpec(Colour::red) && SizeSpec(Size::small);

    cout << "SMALL RED ITEMS:\n";
    for (auto& i : f.filter(items, spec))
        cout << i->name << '\n';
    cout << endl;

    return 0;
}