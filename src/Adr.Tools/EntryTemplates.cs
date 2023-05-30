namespace Adr.Tools;

public class EntryTemplates
{
    public static string InitTemplate() =>
        @$"# 1. Record Architecture Decisions
    
    {DateTime.Now:yyyy-M-d}
    
    ## Status
    
    Accepted
    
    ## Context
    
    We need to record the architectural decisions made on this project.
    
    ## Decision
    
    We will use Architecture Decision Records, as described by Michael Nygard in this article: <http://thinkrelevance.com/blog/2011/11/15/documenting-architecture-decisions>.
    
    ## Consequences
    
    See Michael Nygard's article, linked above.";

    public static string TemplateText => 
@"# NUMBER. TITLE 

Date: DATE 

## Status

STATUS

## Context

The issue motivating this decision, and any context that influences or constrains the decision.

## Decision

The change that we're proposing or have agreed to implement.

## Consequences

What becomes easier or more difficult to do and any risks introduced by the change that will need to be mitigated.";
}
