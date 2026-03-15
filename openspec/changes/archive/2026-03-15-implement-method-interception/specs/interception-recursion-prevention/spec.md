## ADDED Requirements

### Requirement: Exclude Self-Interception Call Sites
The system SHALL NOT generate interception attributes for method calls that occur within the body of the interceptor method that is intended to replace those calls.

#### Scenario: Avoiding StackOverflow
- **WHEN** `MyInterceptor.WriteLine` calls `Console.WriteLine` within its own body
- **THEN** the generator SHALL NOT apply an `[InterceptsLocation]` for that specific call site to `MyInterceptor.WriteLine`
