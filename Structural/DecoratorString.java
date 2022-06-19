import java.io.UnsupportedEncodingException;
import java.lang.invoke.MethodHandles;
import java.nio.charset.Charset;
import java.util.Locale;
import java.util.Optional;
import java.util.function.Function;
import java.util.stream.IntStream;
import java.util.stream.Stream;

// DECORATOR
// Adds behaviour without altering the class or inheriting from it
// (augments the interface)
// allows adding functionality without rewriting existing code (conforms with the open-closed principle)
// and allows keeping the new functionality separate (conforms with the single-responsibility principle)
// STRING DECORATOR
// adds functionality to the (final) String class
// e.g. number of vowels

class StringProMax { // cannot extend String
    private String s;

    public StringProMax(String s) {
        this.s = s;
    }
    
    // extra functionality
    public long getVowelCount() {
        // step-by-step
        // map to chars
        // filter to vowels, converting each to lowercase (to make it case-insensitive)
        // and count the number of elements
        return s.chars().mapToObj(c -> (char)c).filter(c -> "aeiou".contains(c.toString().toLowerCase())).count();
    }
    
    // display format
    @Override
    public String toString() {
        return s;
    }

    // DELEGATE METHODS
    // Expose as many of these as desired
    
    public int length() {
        return s.length();
    }

    public boolean isEmpty() {
        return s.isEmpty();
    }

    public char charAt(int index) {
        return s.charAt(index);
    }

    public int codePointAt(int index) {
        return s.codePointAt(index);
    }

    public int codePointBefore(int index) {
        return s.codePointBefore(index);
    }

    public int codePointCount(int beginIndex, int endIndex) {
        return s.codePointCount(beginIndex, endIndex);
    }

    public int offsetByCodePoints(int index, int codePointOffset) {
        return s.offsetByCodePoints(index, codePointOffset);
    }

    public void getChars(int srcBegin, int srcEnd, char[] dst, int dstBegin) {
        s.getChars(srcBegin, srcEnd, dst, dstBegin);
    }

    @Deprecated(since = "1.1")
    public void getBytes(int srcBegin, int srcEnd, byte[] dst, int dstBegin) {
        s.getBytes(srcBegin, srcEnd, dst, dstBegin);
    }

    public byte[] getBytes(String charsetName) throws UnsupportedEncodingException {
        return s.getBytes(charsetName);
    }

    public byte[] getBytes(Charset charset) {
        return s.getBytes(charset);
    }

    public byte[] getBytes() {
        return s.getBytes();
    }

    public boolean contentEquals(StringBuffer sb) {
        return s.contentEquals(sb);
    }

    public boolean contentEquals(CharSequence cs) {
        return s.contentEquals(cs);
    }

    public boolean equalsIgnoreCase(String anotherString) {
        return s.equalsIgnoreCase(anotherString);
    }

    public int compareTo(String anotherString) {
        return s.compareTo(anotherString);
    }

    public int compareToIgnoreCase(String str) {
        return s.compareToIgnoreCase(str);
    }

    public boolean regionMatches(int toffset, String other, int ooffset, int len) {
        return s.regionMatches(toffset, other, ooffset, len);
    }

    public boolean regionMatches(boolean ignoreCase, int toffset, String other, int ooffset, int len) {
        return s.regionMatches(ignoreCase, toffset, other, ooffset, len);
    }

    public boolean startsWith(String prefix, int toffset) {
        return s.startsWith(prefix, toffset);
    }

    public boolean startsWith(String prefix) {
        return s.startsWith(prefix);
    }

    public boolean endsWith(String suffix) {
        return s.endsWith(suffix);
    }

    public int indexOf(int ch) {
        return s.indexOf(ch);
    }

    public int indexOf(int ch, int fromIndex) {
        return s.indexOf(ch, fromIndex);
    }

    public int lastIndexOf(int ch) {
        return s.lastIndexOf(ch);
    }

    public int lastIndexOf(int ch, int fromIndex) {
        return s.lastIndexOf(ch, fromIndex);
    }

    public int indexOf(String str) {
        return s.indexOf(str);
    }

    public int indexOf(String str, int fromIndex) {
        return s.indexOf(str, fromIndex);
    }

    public int lastIndexOf(String str) {
        return s.lastIndexOf(str);
    }

    public int lastIndexOf(String str, int fromIndex) {
        return s.lastIndexOf(str, fromIndex);
    }

    public String substring(int beginIndex) {
        return s.substring(beginIndex);
    }

    public String substring(int beginIndex, int endIndex) {
        return s.substring(beginIndex, endIndex);
    }

    public CharSequence subSequence(int beginIndex, int endIndex) {
        return s.subSequence(beginIndex, endIndex);
    }

    public String concat(String str) {
        return s.concat(str);
    }

    public String replace(char oldChar, char newChar) {
        return s.replace(oldChar, newChar);
    }

    public boolean matches(String regex) {
        return s.matches(regex);
    }

    public boolean contains(CharSequence s) {
        return this.s.contains(s);
    }

    public String replaceFirst(String regex, String replacement) {
        return s.replaceFirst(regex, replacement);
    }

    public String replaceAll(String regex, String replacement) {
        return s.replaceAll(regex, replacement);
    }

    public String replace(CharSequence target, CharSequence replacement) {
        return s.replace(target, replacement);
    }

    public String[] split(String regex, int limit) {
        return s.split(regex, limit);
    }

    public String[] split(String regex) {
        return s.split(regex);
    }

    public String toLowerCase(Locale locale) {
        return s.toLowerCase(locale);
    }

    public String toLowerCase() {
        return s.toLowerCase();
    }

    public String toUpperCase(Locale locale) {
        return s.toUpperCase(locale);
    }

    public String toUpperCase() {
        return s.toUpperCase();
    }

    public String trim() {
        return s.trim();
    }

    public String strip() {
        return s.strip();
    }

    public String stripLeading() {
        return s.stripLeading();
    }

    public String stripTrailing() {
        return s.stripTrailing();
    }

    public boolean isBlank() {
        return s.isBlank();
    }

    public Stream<String> lines() {
        return s.lines();
    }

    public String indent(int n) {
        return s.indent(n);
    }

    public String stripIndent() {
        return s.stripIndent();
    }

    public String translateEscapes() {
        return s.translateEscapes();
    }

    public <R> R transform(Function<? super String, ? extends R> f) {
        return s.transform(f);
    }

    public IntStream chars() {
        return s.chars();
    }

    public IntStream codePoints() {
        return s.codePoints();
    }

    public char[] toCharArray() {
        return s.toCharArray();
    }

    public String formatted(Object... args) {
        return s.formatted(args);
    }

    public String intern() {
        return s.intern();
    }

    public String repeat(int count) {
        return s.repeat(count);
    }

    public Optional<String> describeConstable() {
        return s.describeConstable();
    }

    public String resolveConstantDesc(MethodHandles.Lookup lookup) {
        return s.resolveConstantDesc(lookup);
    }
}

class DriverCode {
    public static void main(String[] args) throws Exception {
        // initialise text
        var s = new StringProMax("hEllO sOme RanDoM STriNg");
        // result
        System.out.println(s + " " + s.getVowelCount() + " vowels.");
    }
}