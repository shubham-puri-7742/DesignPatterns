#include<iostream>
#include<memory>
#include<map>
using namespace std;

// FACTORIES
// a component that creates other components wholesale
// effectively outsourcing object creation for when object creation logic becomes convoluted
// can be a factory method (a separate, possibly static, function)
// or a factory class (in compliance with the single-responsible principle)
// or a hierarchy of factories with an abstract factory

// ABSTRACT FACTORY creates families of objects
// This is an abstract factory in the object-oriented paradigm

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

// family/hierarchy of factories
struct HotDrinkFactory {
    // factory method
    virtual unique_ptr<HotDrink> make(int vol) const = 0;
};

struct TeaFactory : HotDrinkFactory {
    unique_ptr<HotDrink> make(int vol) const override {
        cout << "Put in the tea bag, boil water, pour " << vol << "ml, add lemon, enjoy!\n";
        return make_unique<Tea>();
    }
};

struct CoffeeFactory : HotDrinkFactory {
    unique_ptr<HotDrink> make(int vol) const override {
        cout << "Grind some beans, boil water, pour " << vol << "ml, enjoy bitter!\n";
        return make_unique<Coffee>();
    }
};

// interface that exposes the various factories
class DrinkFactory {
    // map of drink names to a pointer to the corresponding factory
    map<string , unique_ptr<HotDrinkFactory>> hotFactories;
    
public:
    DrinkFactory() {
        // instantiate all factories from the names
        // technically violates the open-closed principle - extension requires additional cases
        hotFactories["coffee"] = make_unique<CoffeeFactory>();
        hotFactories["tea"] = make_unique<TeaFactory>();
    }
    
    unique_ptr<HotDrink> makeDrink(const string& name) {
        auto drink = hotFactories[name]->make(250);
        drink->consume();
        return drink;
    }
};

int main() {
    DrinkFactory drinkFactory;
    auto c = drinkFactory.makeDrink("coffee");
    auto t = drinkFactory.makeDrink("tea");
}