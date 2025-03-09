using System.Collections;

namespace CommonTestsUtilities.InlineData;

public class PasswordInlineDataTest : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return ["A1@"];
        yield return ["password1@"];
        yield return ["PASSWORD1@"];
        yield return ["Password@@"];
        yield return ["Password123"];
        yield return [new string('A', 129) + "1@"];
        yield return ["        "];
        yield return ["Ab1@"];
        yield return ["Password"];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}