namespace Adr.Tools.Tests;

public class ExtentionsTests
{
    [Fact]
    public void Test1()
    {
        var fileName = "0001-record-adr.md";
        var (number, title) = fileName.ParseEntryNumberAndTitle();
        number.Should().Be(1);
        title.Should().Be("Record Adr");
    }
}
