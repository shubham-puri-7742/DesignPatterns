#include<iostream>
#include<memory>
#include<map>
#include<functional>
using namespace std;

// FACTORIES
// a component that creates other components wholesale
// effectively outsourcing object creation for when object creation logic becomes convoluted
// can be a factory method (a separate, possibly static, function)
// or a factory class (in compliance with the single-responsible principle)
// or a hierarchy of factories with an abstract factory

// ABSTRACT FACTORY creates families of objects
// This is an abstract factory incorporating the functional paradigm

// family/hierarchy of hot drinks
struct HotDrink {
    virtual ~HotDrink() = default;
    virtual void consume() = 0;
};

struct Tea : HotDrink {
    void consume() override {
        cout << "The tea is fantastic!\n";
    }
};

struct Coffee : HotDrink {
    void consume() override {
        cout << "Blacker than a moonless night, hotter and more bitter than the depths of hell itself...!\n";
    }
};

// interface that exposes the various factories
// variant factory - functional style
class DrinkWithVolumeFactory {
    map<string, function<unique_ptr<HotDrink>(const int&)>> factories;
    
public:
    // functional definitions for each factory
    // technically violates the open-closed principle - extension requires additional cases
    DrinkWithVolumeFactory() {
        factories["tea"] = [](const int& vol) {
            cout << "Put in the tea bag, boil water, pour " << vol << "ml, add lemon, enjoy!\n";
            auto tea = make_unique<Tea>();
            tea->consume();
            return tea;
        };
        
        factories["coffee"] = [](const int& vol) {
            cout << "Grind some beans, boil water, pour " << vol << "ml, enjoy bitter!\n";
            auto coffee = make_unique<Coffee>();
            coffee->consume();
            return coffee;
        };
    }
    
    unique_ptr<HotDrink> makeDrink(const string& name, const int& vol = 250) {
        // run the corresponding function
        return factories[name](vol);
    }
};

int main() {
    DrinkWithVolumeFactory drinkFactory;
    auto c = drinkFactory.makeDrink("coffee");
    auto t = drinkFactory.makeDrink("tea", 100);
}