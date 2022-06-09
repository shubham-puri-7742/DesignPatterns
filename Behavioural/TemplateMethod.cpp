#include<iostream>
using namespace std;

// TEMPLATE METHOD
// A high-level blueprint/skeleton for an algorithm to be completed by inheritors
// Similar to the strategy pattern
// While the strategy pattern does this through composition, the template method does it through inheritance

// abstract structure of a turn-based game
class Game {
public:
    Game(int numPlayers) : numPlayers{ numPlayers } {}
    // outlines the basic logic
    void run() {
        // start
        start();
        // game loop
        while(!gameWon())
            turn(); // play a turn
        // announce the winner
        cout << "Player " << getWinner() << " wins.\n";

        //all of these steps are defined in overrides in derived classes
    }

protected:
    // methods to override
    virtual void start() = 0;
    virtual bool gameWon() = 0;
    virtual void turn() = 0;
    virtual int getWinner() = 0;
    int currentPlayer{ 0 };
    int numPlayers;
};

// fake simulation. illustrates the point
class Chess : public Game {
public:
    Chess() : Game{2} {}
protected:
    void start() override {
        cout << "Starting a game of chess with " << numPlayers << " players\n";
    }

    bool gameWon() override {
        return numTurn == maxTurns;
    }

    void turn() override {
        cout << "Turn " << numTurn++ << " played by Player " << currentPlayer++ << '\n';
        currentPlayer %= numPlayers;
    }

    int getWinner() override {
        return currentPlayer;
    }
    
private:
    int numTurn{ 0 }, maxTurns{ 10 };
};

int main() {
    Chess c;
    // calls a base class method that runs as per the implementation in the derived class
    c.run();
    return 0;
}