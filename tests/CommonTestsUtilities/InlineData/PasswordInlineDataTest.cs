using System.Collections;

namespace CommonTestsUtilities.InlineData;

public class PasswordInlineDataTest : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { "A1@" };
        yield return new object[] { "password1@" };
        yield return new object[] { "PASSWORD1@" };
        yield return new object[] { "Password@@" };
        yield return new object[] { "Password123" };
        yield return new object[] { new string('A', 129) + "1@" };
        yield return new object[] { "        " };
        yield return new object[] { "Ab1@" };
        yield return new object[] { "Password" };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}