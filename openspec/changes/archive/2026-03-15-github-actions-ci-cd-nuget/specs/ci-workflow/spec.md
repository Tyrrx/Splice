## ADDED Requirements

### Requirement: Automated Build and Test on Push/PR
The CI workflow SHALL automatically execute when a developer pushes code or creates a pull request targeting the `main` branch.

#### Scenario: Workflow triggered by push
- **WHEN** code is pushed to the `main` branch
- **THEN** the GitHub Actions workflow `ci.yml` starts executing

#### Scenario: Workflow triggered by pull request
- **WHEN** a pull request is opened or updated targeting the `main` branch
- **THEN** the GitHub Actions workflow `ci.yml` starts executing

### Requirement: Solution Compilation
The CI workflow SHALL compile the entire solution using the latest stable .NET SDK.

#### Scenario: Successful build
- **WHEN** the workflow executes `dotnet build` on the solution file
- **THEN** all projects in the solution compile without errors

### Requirement: Unit Test Execution and Reporting
The CI workflow SHALL run all unit tests and report the results within the GitHub Actions UI.

#### Scenario: Tests run and results surfaced
- **WHEN** the workflow executes `dotnet test`
- **THEN** the results are uploaded and displayed in the GitHub Actions "Checks" or "Summary" tab
