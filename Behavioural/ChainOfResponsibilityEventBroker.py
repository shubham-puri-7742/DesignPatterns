# CHAIN OF RESPONSIBILITY
# A sequence of handlers processing an event one after another

# Immediate, implicit application of modifiers with an event broker (observer)
# See the observer pattern code
# Uses the CQS (command (loosely set)-query (loosely get) separation)

from abc import ABC
from enum import Enum

# event = basically a list of functions that can be called
class Event(list):
    def __call__(self, *args, **kwargs):
        for item in self:
            item(*args, **kwargs)

# possible queries
class EnumQueries(Enum):
    ATTACK = 1
    DEFENCE = 2

# query class
class Query:
    def __init__(self, creature, query, defaultVal):
        self.creature = creature
        self.query = query
        self.val = defaultVal # event handlers may change this

# abstract modifier base class
class CreatureModifier(ABC):
    def __init__(self, game, creature):
        self.game = game
        self.creature = creature
        self.game.queries.append(self.handle)

    # abstract method
    def handle(self, sender, query):
        pass

    # when you enter the scope (e.g. a with block)
    def __enter__(self):
        return self

    # when exiting the scope => unsubscribe
    def __exit__(self, exc_type, exc_val, exc_tb):
        self.game.queries.remove(self.handle)

# examples of modifiers
class DoubleAttackModifier(CreatureModifier):
    def handle(self, sender, query):
        if sender.name == self.creature.name and query.query == EnumQueries.ATTACK:
            query.val *= 2

class HalveDefenceModifier(CreatureModifier):
    def handle(self, sender, query):
        if sender.name == self.creature.name and query.query == EnumQueries.DEFENCE and self.creature.attack >= 2:
            query.val /= 2
        
# the event broker
class Game:
    def __init__(self):
        self.queries = Event() # any game object can subscribe to this event

    def performQuery(self, sender, query):
        self.queries(sender, query)

# the data on which the queries are performed
class Creature:
    # game => event broker
    def __init__(self, game, name, attack, defence):
        self.game = game
        self.name = name
        self.initialAttack = attack
        self.initialDefence = defence

    # the final attack and defence values
    @property
    def attack(self):
        q = Query(self.name, EnumQueries.ATTACK, self.initialAttack)
        self.game.performQuery(self, q)
        return q.val

    @property
    def defence(self):
        q = Query(self.name, EnumQueries.DEFENCE, self.initialDefence)
        self.game.performQuery(self, q)
        return q.val

    def __str__(self):
        return f'{self.name} ({self.attack}/{self.defence})'

if __name__ == '__main__':
    game = Game()
    goblin = Creature(game, 'Super Goblin', 2, 2)
    print(goblin)

    # applied instantaneously
    m0 = HalveDefenceModifier(game, goblin)
    m1 = DoubleAttackModifier(game, goblin)
    m2 = HalveDefenceModifier(game, goblin)

    print(goblin)

    with DoubleAttackModifier(game, goblin):
        print(goblin)

    print(goblin)