#include<iostream>
#include<vector>
using namespace std;

// COMMAND PATTERN
// An object represents a particular operation (all the information necessary for an action)
// Used to record a sequence of commands (e.g. a macro in a GUI application), undo/redo etc.
// Commands mutate the state
// Unlike queries that get the state
// SIMPLE IMPLEMENTATION OF A COMMAND STRUCTURE

// struct we're working with
struct BankAccount {
    int balance {0};
    int overdraftLimit{-100};
    
    // deposits an amount
    void deposit(int amount) {
        balance += amount;
        cout << "Deposited " << amount << ", balance = " << balance << endl;
    }
    
    // withdraws an amount if the balance after subtraction is within the overdraft limit
    void withdraw(int amount) {
        if (balance - amount >= overdraftLimit) {
            balance -= amount;
            cout << "Withdrew " << amount << ", balance = " << balance << endl;
        } else {
            cout << "Cannot withdraw " << amount << ", balance = " << balance << ". Overdraft limit: " << -1 * overdraftLimit << endl;
        }
    }

    // output operator
    friend ostream &operator<<(ostream &os, const BankAccount &account) {
        os << "Balance: " << account.balance;
        return os;
    }
};

// abstract structure
struct Command {
    virtual void call() = 0;
};

// concrete struct for implementing the command pattern for the bank account struct above
struct BankAccountCommand : Command {
    // reference to the account
    BankAccount& acc;
    // enum of possible actions
    enum Action { deposit, withdraw } action;
    int amount;

    // ctor for a command
    BankAccountCommand(BankAccount &acc, Action action, int amount) : acc{acc}, action{action}, amount{amount} {}
    
    // override call function
    void call() override {
        // switch on the selected action and call the appropriate method on the account
        switch (action) {
            case deposit:
                acc.deposit(amount);
                break;
            case withdraw:
                acc.withdraw(amount);
                break;
        }
    }
};

int main() {
    // account
    BankAccount b;
    
    // commands vector
    vector<BankAccountCommand> commands { BankAccountCommand{b, BankAccountCommand::deposit, 100 },
                                        BankAccountCommand{b, BankAccountCommand::deposit, 10 },
                                        BankAccountCommand{b, BankAccountCommand::withdraw, 50 },
                                        BankAccountCommand{b, BankAccountCommand::deposit, 20 },
                                        BankAccountCommand{b, BankAccountCommand::withdraw, 250 }};

    // Display the initial account state    
    cout << b << endl;
    
    // for each command
    for (auto& c : commands) {
        // run it
        c.call();
        // and show the updated balance
        cout << b << endl;
    }
    return 0;
}