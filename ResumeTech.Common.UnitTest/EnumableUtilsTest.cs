using ResumeTech.Common.Utility;

namespace ResumeTech.Common.UnitTest; 

[TestFixture]
public class EnumerableUtilsTests
{
    [Test]
    public void ContainsAny_ShouldReturnTrue_WhenAnyElementExists()
    {
        // Arrange
        var lhs = new List<int> { 1, 2, 3, 4, 5 };
        var rhs = new List<int> { 3, 6, 7 };

        // Act
        var result = lhs.ContainsAny(rhs);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void ContainsAny_ShouldReturnFalse_WhenNoElementExists()
    {
        // Arrange
        var lhs = new List<int> { 1, 2, 3, 4, 5 };
        var rhs = new List<int> { 6, 7 };

        // Act
        var result = lhs.ContainsAny(rhs);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void ContainsAll_ShouldReturnTrue_WhenAllElementsExist()
    {
        // Arrange
        var lhs = new List<int> { 1, 2, 3, 4, 5 };
        var rhs = new List<int> { 3, 5 };

        // Act
        var result = lhs.ContainsAll(rhs);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void ContainsAll_ShouldReturnFalse_WhenNotAllElementsExist()
    {
        // Arrange
        var lhs = new List<int> { 1, 2, 3, 4, 5 };
        var rhs = new List<int> { 3, 6 };

        // Act
        var result = lhs.ContainsAll(rhs);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void ContainsNone_ShouldReturnTrue_WhenNoElementExists()
    {
        // Arrange
        var lhs = new List<int> { 1, 2, 3, 4, 5 };
        var rhs = new List<int> { 6, 7 };

        // Act
        var result = lhs.ContainsNone(rhs);

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void ContainsNone_ShouldReturnFalse_WhenAnyElementExists()
    {
        // Arrange
        var lhs = new List<int> { 1, 2, 3, 4, 5 };
        var rhs = new List<int> { 3, 6, 7 };

        // Act
        var result = lhs.ContainsNone(rhs);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsEmpty_ShouldReturnTrue_WhenCollectionIsNull()
    {
        // Arrange
        IEnumerable<int>? collection = null;

        // Act
        var result = collection.IsEmpty();

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void IsEmpty_ShouldReturnTrue_WhenCollectionIsEmpty()
    {
        // Arrange
        IEnumerable<int> collection = new List<int>();

        // Act
        var result = collection.IsEmpty();

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void IsEmpty_ShouldReturnFalse_WhenCollectionIsNotEmpty()
    {
        // Arrange
        IEnumerable<int> collection = new List<int> { 1, 2, 3 };

        // Act
        var result = collection.IsEmpty();

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsNotEmpty_ShouldReturnFalse_WhenCollectionIsNull()
    {
        // Arrange
        IEnumerable<int>? collection = null;

        // Act
        var result = collection.IsNotEmpty();

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsNotEmpty_ShouldReturnFalse_WhenCollectionIsEmpty()
    {
        // Arrange
        IEnumerable<int> collection = new List<int>();

        // Act
        var result = collection.IsNotEmpty();

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void IsNotEmpty_ShouldReturnTrue_WhenCollectionIsNotEmpty()
    {
        // Arrange
        IEnumerable<int> collection = new List<int> { 1, 2, 3 };

        // Act
        var result = collection.IsNotEmpty();

        // Assert
        Assert.That(result, Is.True);
    }
}