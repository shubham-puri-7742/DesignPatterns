import java.util.ArrayList;
import java.util.List;

// ITERATOR PATTERN
// Provides sequential access to elements of an aggregate without exposing its underlying representation.
// Ensures that changing the internals of a class don't affect other classes that may be using it

// general purpose abstract interface
interface Iterator<T> {
    boolean hasNext();
    T current();
    void next();
}

// data we're working with - the history of a hypothetical browser
class BrowseHistory {
    // list of urls
    private List<String> urls = new ArrayList<>();

    // add to the list
    public void push(String url) {
        urls.add(url);
    }

    // remove the last item from the list
    public String pop() {
        var lastIndex = urls.size() - 1;
        var lastUrl = urls.get(lastIndex);
        urls.remove(lastUrl);

        return lastUrl;
    }

    // return an iterator over the internal representation of the url list
    public Iterator createIterator() {
        return new ListIterator(this);
    }

    // concrete implementation of the iterator interface
    // nested class so it can access the list of urls.
    // This is the only part that should be concerned with how to iterate over a BrowseHistory object
    class ListIterator implements Iterator<String> {
        // reference to the history object
        private BrowseHistory history;
        // current index
        private int index;

        // ctor
        public ListIterator(BrowseHistory history) {
            this.history = history;
        }

        // iteration can continue if we haven't reached the last element
        @Override
        public boolean hasNext() {
            return (index < history.urls.size());
        }

        // element at the current index
        @Override
        public String current() {
            return history.urls.get(index);
        }

        // increments the index
        @Override
        public void next() {
            ++index;
        }
    }
}

public class Main {
    public static void main(String[] args) {
        var history = new BrowseHistory();
        history.push("a");
        history.push("b");
        history.push("c");
        history.push("d");
        history.push("e");

        for (Iterator i = history.createIterator(); i.hasNext(); i.next()) {
            var url = i.current();
            System.out.println(url);
        }
    }
}