# FAÇADE PATTERN
# Expose several components through a single, simple interface
# Balances complexity with presentation/usability

# terminal interface
# text buffer
class Buffer:
    # ctor
    def __init__(self, width = 30, height = 20):
        self.width = width
        self.height = height
        self.buffer = [' '] * (width * height)

    # support indexing
    def __getitem__(self, item):
        return self.buffer.__getitem__(item)

    # write text to the buffer
    def write(self, text):
        self.buffer += text

# viewport that displays a part of the buffer
class Viewport:
    #ctor
    def __init__(self, buffer = Buffer()):
        self.buffer = buffer
        self.offset = 20

    # get a character at a particular index
    def getCharAt(self, index):
        return self.buffer[index + self.offset]

    # append text (added in open terminal interface)
    def append(self, text):
        self.buffer.write(text)

# the façade
class Console:
    # ctor
    def __init__(self):
        b = Buffer()
        self.currentViewport = Viewport(b)
        # lists to enable extension. these also allow access to low-level features
        self.buffers = [b]
        self.viewports = [self.currentViewport]

    # hides the complexity behind this simple function
    def write(self, text):
        self.currentViewport.append(text)

    # low-level API
    def getCharAt(self, index):
        return self.currentViewport.getCharAt(index)

if __name__ == '__main__':
    c = Console()
    c.write('Hello Façade! Writing random stuff here to test the mockup console and viewport classes!')
    print(c)
    print(c.buffers)
    print(c.viewports, end = '\n\n')
    ch1 = c.getCharAt(580)
    ch2 = c.getCharAt(581)
    print('First and second chars: {}, {}'.format(ch1, ch2))