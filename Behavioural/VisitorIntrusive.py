# VISITOR
# Traverses a data structure of (possibly-related) types
# For adding extra behaviour to entire hierarchies of classes
# e.g. a new operation on a hierarchy without modifying the classes
# e.g. an external class to handle rendering documents in various formats that doesn't check types explicitly

# Intrusive approach
# modifies the desired classes
# technically violating the open-closed principle

# The buffer is the visitor here

class DoubleExpr:
    def __init__(self, value):
        self.value = value

    # the intrusive parts
    def print(self, buffer):
        buffer.append(str(self.value))

    def eval(self):
        return self.value

class AddExpr:
    def __init__(self, left, right):
        self.left = left
        self.right = right

    # the intrusive parts
    def print(self, buffer):
        buffer.append('(')
        self.left.print(buffer)
        buffer.append(' + ')
        self.right.print(buffer)
        buffer.append(')')

    def eval(self):
        return self.left.eval() + self.right.eval()

class MultExpr:
    def __init__(self, left, right):
        self.left = left
        self.right = right

    # the intrusive parts
    def print(self, buffer):
        buffer.append('(')
        self.left.print(buffer)
        buffer.append(' × ')
        self.right.print(buffer)
        buffer.append(')')

    def eval(self):
        return self.left.eval() * self.right.eval()

if __name__ == '__main__':
    # (1 + (2 × (3 + 4))) = 15
    e = AddExpr(DoubleExpr(1), (MultExpr(DoubleExpr(2), AddExpr(DoubleExpr(3), DoubleExpr(4)))))

    buffer = []
    e.print(buffer)
    print(''.join(buffer), '=', e.eval())