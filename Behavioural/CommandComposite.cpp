#include<iostream>
#include<vector>
using namespace std;

// COMMAND PATTERN
// An object represents a particular operation (all the information necessary for an action)
// Used to record a sequence of commands (e.g. a macro in a GUI application), undo/redo etc.
// Commands mutate the state
// Unlike queries that get the state
// COMPOSITE COMMAND (MACRO/ACTION) IMPLEMENTATION (for commands that depend on other commands)

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
    bool withdraw(int amount) {
        if (balance - amount >= overdraftLimit) {
            balance -= amount;
            cout << "Withdrew " << amount << ", balance = " << balance << endl;
            return true;
        }
        cout << "Cannot withdraw " << amount << ", balance = " << balance << ". Overdraft limit: " << -1 * overdraftLimit << endl;
        return false;
    }

    // output operator
    friend ostream &operator<<(ostream &os, const BankAccount &account) {
        os << "Balance: " << account.balance;
        return os;
    }
};

// abstract structure
struct Command {
    bool success;
    virtual void call() = 0;
    virtual void undo() = 0;
};

// concrete struct for implementing the command pattern for the bank account struct above
struct BankAccountCommand : Command {
    // reference to the account
    BankAccount& acc;
    // enum of possible actions
    enum Action { deposit, withdraw } action;
    int amount;

    // ctor for a command
    BankAccountCommand(BankAccount &acc, Action action, int amount) : acc{acc}, action{action}, amount{amount} {
        success = false;
    }

    // override call function
    void call() override {
        // switch on the selected action and call the appropriate method on the account
        switch (action) {
            case deposit:
                acc.deposit(amount);
                success = true;
                break;
            case withdraw:
                success = acc.withdraw(amount);
                break;
        }
    }

    void undo() override {
        if (!success) return;

        switch (action) {
            case deposit:
                acc.withdraw(amount);
                break;
            case withdraw:
                acc.deposit(amount);
                break;
        }
    }
};

// initialise this struct with a set of commands, and call/undo them in one function
// this is independent (deposits even if withdrawal fails). Don't use this if you don't want SCAM 1992 happening
struct CompositeBankAccountCommand : vector<BankAccountCommand>, Command {
    CompositeBankAccountCommand(const initializer_list<BankAccountCommand> &commands) : vector{commands} {}

    void call() override {
        // for each command
        for (auto& c : *this) {
            // run it
            c.call();
        }
    }

    void undo() override {
        for (auto i = rbegin(); i != rend(); ++i) {
            i->undo();
        }
    }
};

// dependent - because you cannot deposit into the second account if the withdrawal from the first failed
// usable
struct DependentCompositeCommand : CompositeBankAccountCommand {
    DependentCompositeCommand(const initializer_list<BankAccountCommand>& commands) : CompositeBankAccountCommand{commands} {}
    
    void call() override {
        // flag for the last command
        bool ok = true;
        // for each command in the list
        for (auto& c : *this) {
            // if the flag is set to true
            if (ok) {
                // execute the command. Set success state
                c.call();
                ok = c.success;
            } else {
                // set failure state
                c.success = false;
            }
        }
    }
};

// money transfer - withdraw from one account, deposit in another
struct MoneyTransferCommand : DependentCompositeCommand {
    MoneyTransferCommand(BankAccount& from, BankAccount& to, int amount) : DependentCompositeCommand {
        BankAccountCommand {from, BankAccountCommand::withdraw, amount},
        BankAccountCommand {to, BankAccountCommand::deposit, amount}
    } {}
};

int main() {
    // initialise accounts
    BankAccount a, b;
    a.deposit(1000);
    
    // initialise the transfer commands
    MoneyTransferCommand c1{a, b, 500};
    MoneyTransferCommand c2{a, b, 5000};
    
    // execute the first
    c1.call();
    
    // display the final balances
    cout << a << endl << b << endl;
    
    cout << "\nUNDOING\n\n";
    // undo
    c1.undo();

    // display the final balances
    cout << a << endl << b << endl;
    
    // execute the second (intended to fail)
    cout << "\nFAILING TEST\n\n";
    c2.call();

    // display the final balances
    cout << a << endl << b << endl;

    cout << "\nUNDOING\n\n";
    // undo
    c2.undo();

    // display the final balances
    cout << a << endl << b << endl;

    return 0;
}