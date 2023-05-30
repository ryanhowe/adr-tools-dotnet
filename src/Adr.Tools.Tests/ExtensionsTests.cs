using Xunit.Abstractions;

namespace Adr.Tools.Tests;

public class ExtensionsTests
{
    [Fact]
    public void should_parse_entry_number_and_title_from_file_name()
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

    public class when_parsing_link_parameters
    {
        [Fact]
        public void should_parse_link_parameters()
        {
            string link = "2:Clarifies:Clarified by";
            var links = new[] { link };
            var param = links.ParseLinkParameters();
            param.First().Number.Should().Be(2);
            param.First().SourceText.Should().Be("Clarifies");
            param.First().TargetText.Should().Be("Clarified by");
        }

        [Fact]
        public void should_report_invalid_non_number_entry_number()
        {
            string link = "A:Clarifies:Clarified by";
            var links = new[] { link };
            var param = links.ParseLinkParameters();
            param.Any().Should().BeFalse();
        }

        [Fact]
        public void should_report_invalid_link_parameters()
        {
            string link = "1:Clarifies";
            var links = new[] { link };
            var param = links.ParseLinkParameters();
            param.Any().Should().BeFalse();
        }
    }
}
