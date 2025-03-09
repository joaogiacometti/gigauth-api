using System.Collections;

namespace CommonTestsUtilities.InlineData;

public class CultureInlineDataTest : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return ["en"];
        yield return ["pt-BR"];
        yield return ["pt-PT"];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}