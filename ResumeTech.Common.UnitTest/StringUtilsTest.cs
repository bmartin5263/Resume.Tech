using ResumeTech.Common.Utility;

namespace ResumeTech.Common.UnitTest;

public class StringUtilsTest {
    [Test]
    public void ToExpandedString_NullEnumerable_ReturnsEmptyBrackets() {
        IEnumerable<int>? enumerable = null;
        string result = enumerable.ToExpandedString();

        Assert.That(result, Is.EqualTo("[]"));
    }

    [Test]
    public void ToExpandedString_EmptyEnumerable_ReturnsEmptyBrackets() {
        IEnumerable<int> enumerable = new List<int>();
        string result = enumerable.ToExpandedString();

        Assert.That(result, Is.EqualTo("[]"));
    }

    [Test]
    public void ToExpandedString_SingleElementEnumerable_ReturnsSingleElement() {
        IEnumerable<int> enumerable = new List<int> { 42 };
        string result = enumerable.ToExpandedString();

        Assert.That(result, Is.EqualTo("[42]"));
    }

    [Test]
    public void ToExpandedString_MultipleElementEnumerable_ReturnsCommaSeparatedElements() {
        IEnumerable<int> enumerable = new List<int> { 1, 2, 3 };
        string result = enumerable.ToExpandedString();

        Assert.That(result, Is.EqualTo("[1, 2, 3]"));
    }

    [Test]
    public void ToExpandedString_StringEnumerable_ReturnsMultipleElements() {
        IEnumerable<string> enumerable = new List<string> { "apple", "banana", "cherry" };
        string result = enumerable.ToExpandedString();

        Assert.That(result, Is.EqualTo("[apple, banana, cherry]"));
    }

    [Test]
    public void ToExpandedString_NullDictionary_ReturnsEmptyBraces() {
        IDictionary<string, int>? dictionary = null;
        string result = dictionary.ToExpandedString();

        Assert.That(result, Is.EqualTo("{}"));
    }

    [Test]
    public void ToExpandedString_EmptyDictionary_ReturnsEmptyBraces() {
        IDictionary<string, int> dictionary = new Dictionary<string, int>();
        string result = dictionary.ToExpandedString();

        Assert.That(result, Is.EqualTo("{}"));
    }

    [Test]
    public void ToExpandedString_SingleElementDictionary_ReturnsSingleElement() {
        IDictionary<string, int> dictionary = new Dictionary<string, int> {
            { "apple", 42 }
        };

        string result = dictionary.ToExpandedString();

        Assert.That(result, Is.EqualTo("{apple: 42}"));
    }

    [Test]
    public void ToExpandedString_MultipleElementDictionary_ReturnsCommaSeparatedElements() {
        IDictionary<string, int> dictionary = new Dictionary<string, int> {
            { "apple", 42 },
            { "banana", 12 },
            { "cherry", 7 }
        };

        string result = dictionary.ToExpandedString();

        Assert.That(result, Is.EqualTo("{apple: 42, banana: 12, cherry: 7}"));
    }

    [Test]
    public void ToExpandedString_StringKeysAndValues_ReturnsMultipleElements() {
        IDictionary<string, string> dictionary = new Dictionary<string, string> {
            { "name", "John" },
            { "city", "New York" }
        };

        string result = dictionary.ToExpandedString();

        Assert.That(result, Is.EqualTo("{name: John, city: New York}"));
    }
}