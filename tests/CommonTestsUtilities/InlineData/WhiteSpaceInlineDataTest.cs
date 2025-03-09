using System.Collections;

namespace CommonTestsUtilities.InlineData;

public class WhiteSpaceInlineDataTest : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return [""];
        yield return [" "];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}