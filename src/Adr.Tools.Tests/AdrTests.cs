namespace Adr.Tools.Tests;

public class EntryTests
{
    public class when_creating_entry
    {

        [Fact]
        public void should_populate_filename()
        {
            var entry = Entry.Create(202, "My Title");
            entry.FileName.Should().Be("0202-my-title.md");
        }
    }
}
