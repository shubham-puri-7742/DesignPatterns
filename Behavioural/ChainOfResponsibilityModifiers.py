# CHAIN OF RESPONSIBILITY
# A sequence of handlers processing an event one after another

# Chain of modifiers

class Creature:
    def __init__(self, name, attack, defence):
        self.name = name
        self.attack = attack
        self.defence = defence

    def __str__(self):
        return f'{self.name} ({self.attack}/{self.defence})'

class CreatureModifier:
    def __init__(self, creature):
        self.creature = creature
        # modifier chain. A function pointer effectively
        self.nextModifier = None

    # add the next modifier
    def addModifier(self, modifier):
        # if we already have a next modifier
        if self.nextModifier:
            # build the chain basically
            self.nextModifier.addModifier(modifier)
        # base case
        else:
            self.nextModifier = modifier

    # applies the modifier
    def handle(self):
        if self.nextModifier:
            self.nextModifier.handle()

# modifier 1
class DoubleAttackModifier(CreatureModifier):
    def handle(self):
        print(f'Doubling {self.creature.name}\'s attack')
        self.creature.attack *= 2
        super().handle() # the base class' modifier propagates the handle call down the chain

# modifier 2
class HalveDefenceModifier(CreatureModifier):
    def handle(self):
        if self.creature.attack >= 2:
            print(f'Halving {self.creature.name}\'s defence')
            self.creature.defence /= 2
        super().handle() # the base class' modifier propagates the handle call down the chain

# modifier disabler
class NoBonusesModifier(CreatureModifier):
    def handle(self):
        print('No more bonuses for you!')
    # with no super().handle(), the call is not propagated

if __name__ == '__main__':
    # initialise the creature
    goblin = Creature('Goblin', 1, 1)
    print(goblin)


    # set up modifiers
    root = CreatureModifier(goblin)

    root.addModifier(HalveDefenceModifier(goblin))
    root.addModifier(DoubleAttackModifier(goblin))
    root.addModifier(HalveDefenceModifier(goblin))

    # comment this out to test
    root.addModifier(NoBonusesModifier(goblin))

    root.addModifier(DoubleAttackModifier(goblin))
    root.addModifier(HalveDefenceModifier(goblin))

    # handle the modifiers
    root.handle()

    print(goblin)