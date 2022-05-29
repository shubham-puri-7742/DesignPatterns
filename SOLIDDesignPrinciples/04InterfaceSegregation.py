# INTERFACE SEGREGATION PRINCIPLE
# Keep interfaces short (not too many functions)
# So implementers don't have to implement too much
# In other words, don't force implementers to define unnecessary functions

# INTERFACES
# separate interfaces for each function
class Printer:
    @abstractmethod
    def print(self, document):
        pass

class Scanner:
    @abstractmethod
    def scan(self, document):
        pass

class Faxer:
    @abstractmethod
    def fax(self, document):
        pass

# combined interface
class PrinterScanner(Printer, Scanner):
    @abstractmethod
    def print(self, document):
        pass

    @abstractmethod
    def scanner(self, document):
        pass

# don't expose unnecessary functions to implementations
class printer(Printer):
    def print(self, document):
        print("Printing...")

class scanner(Scanner):
    def scan(self, document):
        print("Faxing...")

class MultiFunctionPrinter1(PrinterScanner):
    def print(self, document):
        print("Printing...")

    def scan(self, document):
        print("Scanning...")

# implement multiple interfaces where necessary
class MultiFunctionMachine2(Printer, Scanner, Faxer):
    def print(self, document):
        print("Printing...")
    def scan(self, document):
        print("Scanning...")
    def fax(self, document):
        print("Faxing...")

# take an existing printer and scanner in the init (constructor)
class MultiFunctionMachine3(PrinterScanner):
    def __init__(self, printer, scanner):
        self.printer = printer
        self.scanner = scanner
    def print(self, document):
        self.printer.print(document)
    def scan(self, document):
        self.scanner.print(document)