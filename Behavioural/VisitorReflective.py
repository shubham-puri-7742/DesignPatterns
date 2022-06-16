from abc import ABC

# VISITOR
# Traverses a data structure of (possibly-related) types
# For adding extra behaviour to entire hierarchies of classes
# e.g. a new operation on a hierarchy without modifying the classes
# e.g. an external class to handle rendering documents in various formats that doesn't check types explicitly

# Reflective (type-checking) approach
# separates concerns

# types of expressions
# base class (To facilitate easy syntax)
class Expr(ABC):
    pass

# doubles (numbers)
class DoubleExpr(Expr):
    def __init__(self, value):
        self.value = value

# addition
class AddExpr(Expr):
    def __init__(self, left, right):
        self.left = left
        self.right = right

# multiplication
class MultExpr(Expr):
    def __init__(self, left, right):
        self.left = left
        self.right = right

# Visitors
# these need to be modified when adding additional expression types

# for printing
class ExprPrinter:
    @staticmethod
    # handle typewise printing
    def print(e, buffer):
        # doubles
        if isinstance(e, DoubleExpr):
            buffer.append(str(e.value))
        # addition
        elif isinstance(e, AddExpr):
            buffer.append('(')
            ExprPrinter.print(e.left, buffer)
            buffer.append(' + ')
            ExprPrinter.print(e.right, buffer)
            buffer.append(')')
        # multiplication
        elif isinstance(e, MultExpr):
            buffer.append('(')
            ExprPrinter.print(e.left, buffer)
            buffer.append(' × ')
            ExprPrinter.print(e.right, buffer)
            buffer.append(')')
        # unknown
        else:
            raise Exception('Unknown expression type')

    # to facilitate easy syntax
    Expr.print = lambda self, b: ExprPrinter.print(self, b)

# for evaluation
class ExprEval:
    @staticmethod
    # handle typewise evaluation
    def eval(e):
        # doubles
        if isinstance(e, DoubleExpr):
            return e.value
        # addition
        elif isinstance(e, AddExpr):
            return ExprEval.eval(e.left) + ExprEval.eval(e.right)
        # multiplication
        elif isinstance(e, MultExpr):
            return ExprEval.eval(e.left) * ExprEval.eval(e.right)
        # unknown
        else:
            raise Exception('Unknown expression type')

    # to facilitate easy syntax
    Expr.eval = lambda self: ExprEval.eval(self)

if __name__ == '__main__':
    # (1 + (2 × (3 + 4))) = 15
    e = AddExpr(DoubleExpr(1), (MultExpr(DoubleExpr(2), AddExpr(DoubleExpr(3), DoubleExpr(4)))))

    buffer = []
    # see the lambdas above
    e.print(buffer)
    print(''.join(buffer), '=', e.eval())