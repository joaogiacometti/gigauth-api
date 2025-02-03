using System.Collections;

namespace CommonTestsUtilities.InlineData;

public class NullOrWhiteSpaceInlineDataTest : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { null! };
        yield return new object[] { "" };
        yield return new object[] { " " };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}