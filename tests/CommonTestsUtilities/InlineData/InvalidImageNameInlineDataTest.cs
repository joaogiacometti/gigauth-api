using System.Collections;

namespace CommonTestsUtilities.InlineData;

public class InvalidImageExtensionInlineDataTest : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return ["test.exe"];
        yield return ["test.pdf"];
        yield return ["test.java"];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
}