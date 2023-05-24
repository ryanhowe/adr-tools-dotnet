namespace Adr.Tools.Tests;

public class ExtentionsTests
{
    [Fact]
    public void Test1()
    {
        var fileName = @"0001-record-adr.md";
        var (number, title) = fileName.ParseEntryNumberAndTitle();
        number.Should().Be(1);
        title.Should().Be("Record Adr");
    }

    [Fact]
    public void given_entry_title_should_convert_to_file_name()
    {
        var title = "My Title here";
        title.ToFileName().Should().Be("my-title-here.md");
    }

    [Fact]
    public void give_empty_title_should_throw_exception()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => string.Empty.ToFileName());
    }
}
