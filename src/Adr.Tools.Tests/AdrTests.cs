namespace Adr.Tools.Tests;

public class when_creating_new_entries 
{
    private Adr _adr;
    public when_creating_new_entries()
    {
        _adr = new Adr();
    }
    
    // [Fact]
    // public void should_
    //
}


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
