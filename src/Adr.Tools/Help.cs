namespace Adr.Tools;

public class Help
{
    internal static string LinkOptionDescription => @"TARGET:LINK:REVERSE-LINK
Links the new ADR to a previous ADR.
TARGET is a reference (Number or partial filename) of a previous decision.
LINK is the description of the link created in the new ADR.
REVERSE-LINK is the description of the link created in the
existing ADR that will refer to the new ADR.";

    internal static string NewDescription =>
        @"Creates a new, numbered ADR.  The TITLE_TEXT arguments are concatenated to form the title of the new ADR.
Multiple -s and -l options can be given, so that the new ADR can supersede
or link to multiple existing ADRs.

E.g. to create a new ADR with the title ""Use MySQL Database"":

    adr new Use MySQL Database

E.g. to create a new ADR that supersedes ADR 12:

    adr new -s 12 ""Use PostgreSQL Database""

E.g. to create a new ADR that supersedes ADRs 3 and 4, and amends ADR 5:

    adr new -s 3 -s 4 -l ""5:Amends:Amended by"" ""Use Riak CRDTs to cope with scale""";

    public static string SupersedeOptionDescription =>
        @"A reference (Number or partial filename) of a previous
decision that the new decision supersedes. A Markdown link
to the superseded ADR is inserted into the Status section.
The status of the superseded ADR is changed to record that
it has been superseded by the new ADR";

    public static string LinkCommandDescription => @"Creates a link between two ADRs, from SOURCE to TARGET new.
SOURCE and TARGET are both a reference (Number or partial filename) to an ADR
LINK is the description of the link created in the SOURCE.
REVERSE-LINK is the description of the link created in the TARGET

E.g. to create link ADR 12 to ADR 10

adr link 12 ""Amends"" 10 ""Amended by""";

    public static string? InitCommandDescription => @"Initialises the directory of architeture decision records:
* creates a subdirectory of the current working directory
* creates the first ADR in that subdirectory, recording the decision to record architectural decisions with ADRs.

If the DIRECTORY is not given, the ADRs are stored in the directory `doc/adr`.";

    public static string TitleOptionDescription => @"Ex: ""Record architecture decisions""";
    public static string PathOptionDescription => "Path to initialize ADR in (default docs/adr)";
    public static string ListCommandDescription => "Lists the architecture decision records";
    public static string TargetTextArgumentDescription => "target link text";
    public static string TargetArgumentDescription => "Target entry Number";
    public static string SourceArgumentDescription => "Source entry Number";
    public static string SourceTextArgumentDescription => "Source link text";
}
