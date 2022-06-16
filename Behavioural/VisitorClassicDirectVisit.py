from abc import ABC

# VISITOR
# Traverses a data structure of (possibly-related) types
# For adding extra behaviour to entire hierarchies of classes
# e.g. a new operation on a hierarchy without modifying the classes
# e.g. an external class to handle rendering documents in various formats that doesn't check types explicitly

# Classic Gang of Four Visitor
# Using a double dispatch
# Without the statically-typed style using accept methods
# (more Pythonic because Python uses duck (dynamic) typing)

# get the qualified name of an object (including the module)
def _qualname(o):
    return o.__module__ + '.' + o.__qualname__

# gets the name of the class that declared an object
def _declaringClass(o):
    name = _qualname(o)
    return name[:name.rfind('.')]

# stores the visitor methods
_methods = {}

# Delegating visitor
# Actual visitor method
def _visitorImpl(self, arg):
    method = _methods[(_qualname(type(self)), type(arg))]
    return method(self, arg)

# the @visitor decorator
# creates a visitor method
def visitor(argType):
    def decorator(f):
        declaringClass = _declaringClass(f)
        _methods[(declaringClass, argType)] = f

        # replace all decorated methods with _visitorImpl
        return _visitorImpl
    return decorator

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
    def __init__(self):
        self.buffer = []

    @visitor(DoubleExpr)
    def visit(self, doubleExpr):
        self.buffer.append(str(doubleExpr.value))

    @visitor(AddExpr)
    def visit(self, addExpr):
        self.buffer.append('(')
        # directly invoke visit here instead of an accept method
        # 
        self.visit(addExpr.left)
        self.buffer.append(' + ')
        self.visit(addExpr.right)
        self.buffer.append(')')

    @visitor(MultExpr)
    def visit(self, multExpr):
        self.buffer.append('(')
        self.visit(multExpr.left)
        self.buffer.append(' × ')
        self.visit(multExpr.right)
        self.buffer.append(')')

    def __str__(self):
        return ''.join(self.buffer)

# for evaluation
class ExprEval:
    def __init__(self):
        self.result = None

    @visitor(DoubleExpr)
    def visit(self, doubleExpr):
        self.result = doubleExpr.value

    @visitor(AddExpr)
    def visit(self, addExpr):
        self.visit(addExpr.left)
        temp = self.result
        self.visit(addExpr.right)
        self.result += temp

    @visitor(MultExpr)
    def visit(self, multExpr):
        self.visit(multExpr.left)
        temp = self.result
        self.visit(multExpr.right)
        self.result *= temp

    def __str__(self):
        return str(self.result)

if __name__ == '__main__':
    # (1 + (2 × (3 + 4))) = 15
    e = AddExpr(DoubleExpr(1), (MultExpr(DoubleExpr(2), AddExpr(DoubleExpr(3), DoubleExpr(4)))))

    p = ExprPrinter()
    p.visit(e)

    r = ExprEval()
    r.visit(e)

    print(p, '=', r)