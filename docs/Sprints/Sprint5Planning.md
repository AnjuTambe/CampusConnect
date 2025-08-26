# CampusConnect - Sprint 5 Plan (Granular Breakdown - INVEST & 1-Point Estimates)

## Sprint 5 Goal

Enhance job listing and employer browsing with 'EmployerName' integration, filtering, and a dedicated Dashboard section including employer list, search, job display, and detail modal. Achieve 100% unit test coverage for implemented features. Establish comprehensive coding standards across the codebase, including creating PageModels and tests for simple pages, refactoring test naming/structure, enforcing detailed naming conventions, and enforcing code commenting rules.

## Task Board Simulation

### Backlog (Selected High-Level Goals for Future Sprints / Refinements)
1.  **Epic:** Advanced Search Capabilities (e.g., multi-select filters, date range)
2.  **Epic:** Company Landing Pages (Basic info, jobs by that company)
3.  **Epic:** User Profile Enhancements (e.g., resume upload, saved searches - if profiles were to be added later)
4.  **Epic:** Email Notification System (e.g., new job alerts - if user accounts were more developed)
5.  **Feature:** Accessibility Audit & Improvements (WCAG Compliance)
6.  **Feature:** Performance Optimization for Large Datasets
7.  **Task:** Refine UI/UX for Dashboard based on initial feedback.
8.  **Task:** Add "Date Posted" sorting option to job listings.
9.  **Task:** Unit Tests for Pagination Logic (if separated from IndexModel tests).
10. **Task:** Persist Admin Toggle State (e.g., using Local Storage).

### Current Sprint (56 Tasks)

*(Tasks distributed among 4 Team Members)*

**Team Member 1 (14 Tasks, Est: 14 points)**

*   **Task S5.1 (Model: Add `EmployerName` Property):**
    *   As a developer, I want to add an `EmployerName` property to the job model so employer information can be stored.
    *   Estimate: 1
    *   DoD:
        1.  [`JobListing.cs`](src/Models/JobListing.cs:1) includes a public string property `EmployerName`.
        2.  The project compiles successfully.
*   **Task S5.2 (Model: Add `[Required]` to `EmployerName`):**
    *   As a developer, I want the `EmployerName` field to be mandatory so employer information is reliably captured.
    *   Estimate: 1
    *   DoD:
        1.  The `EmployerName` property in [`JobListing.cs`](src/Models/JobListing.cs:1) has the `[Required]` attribute.
        2.  The property has the `[JsonPropertyName("employer_name")]` attribute for JSON serialization.
*   **Task S5.3 (Data: Update `jobs.json` with `employer_name`):**
    *   As a developer, I want the sample job data to include `employer_name` values so the application has test data for the new mandatory field.
    *   Estimate: 1
    *   DoD:
        1.  Each job object in [`src/wwwroot/data/jobs.json`](src/wwwroot/data/jobs.json:1) includes an `employer_name` field.
        2.  All `employer_name` values are plausible, non-empty strings.
*   **Task S5.4 (Service: `AddJob` Persist `EmployerName`):**
    *   As a developer, I want the job service's add method to save the `EmployerName` so new jobs correctly store this information.
    *   Estimate: 1
    *   DoD:
        1.  [`JsonFileJobService.AddJob`](src/Services/JsonFileJobService.cs:96) saves the `EmployerName` from the input object.
        2.  The method adheres to coding standards (usings, line breaks, file endings, naming).
*   **Task S5.5 (Service: `UpdateJob` Persist `EmployerName`):**
    *   As a developer, I want the job service's update method to save the `EmployerName` so edited jobs correctly update this information.
    *   Estimate: 1
    *   DoD:
        1.  [`JsonFileJobService.UpdateJob`](src/Services/JsonFileJobService.cs:169) saves the `EmployerName` from the input object.
        2.  The method adheres to coding standards (usings, line breaks, file endings, naming).
*   **Task S5.6 (Service: `EmployerName` Filter in `GetJobs`):**
    *   As a developer, I want the job service to filter jobs by Employer Name so users can find jobs from specific employers.
    *   Estimate: 1
    *   DoD:
        1.  `IJobService.GetJobs` includes an optional `employerNameFilter` string parameter.
        2.  [`JsonFileJobService.GetJobs`](src/Services/JsonFileJobService.cs:32) filters results by `employerNameFilter` (case-insensitive) and adheres to coding standards.
*   **Task S5.7 (UT Model: `EmployerName` `[Required]` Validation):**
    *   As a developer, I want unit tests for the job model's `EmployerName` `[Required]` validation so I can be confident the validation works.
    *   Estimate: 1
    *   DoD:
        1.  A test in `Job_Listing_Model_Tests.cs` verifies an empty `EmployerName` causes a validation error.
        2.  A test in `Job_Listing_Model_Tests.cs` verifies a null `EmployerName` causes a validation error.
*   **Task S5.8 (UT Service: `AddJob` for `EmployerName`):**
    *   As a developer, I want a unit test to verify the job service's add method correctly handles `EmployerName`.
    *   Estimate: 1
    *   DoD:
        1.  A test in `Json_File_Job_Service_Tests.cs` verifies `AddJob` saves the provided `EmployerName`.
        2.  The test passes and follows the standard UT template/naming.
*   **Task S5.9 (UT Service: `UpdateJob` for `EmployerName`):**
    *   As a developer, I want a unit test to verify the job service's update method correctly handles `EmployerName`.
    *   Estimate: 1
    *   DoD:
        1.  A test in `Json_File_Job_Service_Tests.cs` verifies `UpdateJob` updates the `EmployerName`.
        2.  The test passes and follows the standard UT template/naming.
*   **Task S5.10 (UT Service: `GetJobs` Filtering by `EmployerName`):**
    *   As a developer, I want a unit test to verify the job service's get jobs method correctly filters by `EmployerName`.
    *   Estimate: 1
    *   DoD:
        1.  A test in `Json_File_Job_Service_Tests.cs` verifies `GetJobs` returns only jobs matching the specified `employerNameFilter`.
        2.  The test passes and follows the standard UT template/naming.
*   **Task S5.11 (Create AboutUs.cshtml.cs):**
    *   As a developer, I want to create the PageModel for the About Us page so it follows the standard structure.
    *   Estimate: 1
    *   DoD:
        1.  `AboutUs.cshtml.cs` file is created in the correct directory.
        2.  The file has a basic PageModel class structure and adheres to coding standards.
*   **Task S5.12 (Create About_Us_Page_Tests.cs):**
    *   As a developer, I want to create the unit test file for the About Us PageModel using the standard naming convention.
    *   Estimate: 1
    *   DoD:
        1.  `About_Us_Page_Tests.cs` file is created in the correct directory.
        2.  The file has a basic test class structure and adheres to coding standards.
*   **Task S5.13 (UT AboutUsModel Basic Coverage):**
    *   As a developer, I want basic unit test coverage for the About Us PageModel.
    *   Estimate: 1
    *   DoD:
        1.  A test exists for the `OnGet` method in `About_Us_Page_Tests.cs`.
        2.  The test passes and follows the standard UT template/naming.
*   **Task S5.14 (Refactor Test Case Method Naming):**
    *   As a developer, I want to rename all unit test methods to follow the `Method_Condition_State_Reason_Expected` format with underscores and include Valid/Invalid where applicable.
    *   Estimate: 1
    *   DoD:
        1.  All unit test method names across all test files follow the `Method_Condition_State_Reason_Expected` format.
        2.  Method names include `Valid` or `Invalid` where applicable.
        3.  Underscores are used after every word in method names.
        4.  All tests still pass after renaming.

**Team Member 2 (14 Tasks, Est: 14 points)**

*   **Task S5.15 (UI Create Form: Add `EmployerName` Input):**
    *   As an admin, I want a text input for "Employer Name" on the Create Job form so I can enter this information when adding a job.
    *   Estimate: 1
    *   DoD:
        1.  [`Pages/Jobs/Create.cshtml`](src/Pages/Jobs/Create.cshtml:1) includes a `label` and `input type="text"` for `EmployerName`.
        2.  The input is bound to `Model.NewJob.EmployerName` and has a validation message span.
*   **Task S5.16 (UI Edit Form: Add `EmployerName` Input):**
    *   As an admin, I want a text input for "Employer Name" on the Edit Job form so I can update this information when editing a job.
    *   Estimate: 1
    *   DoD:
        1.  [`Pages/Jobs/Edit.cshtml`](src/Pages/Jobs/Edit.cshtml:1) includes a `label` and `input type="text"` for `EmployerName`.
        2.  The field is pre-filled on page load, and a validation message span is present.
*   **Task S5.17 (UT PageModels: Server-Side `[Required]` Validation Create):**
    *   As a developer, I want a unit test to ensure the server prevents creating jobs if `EmployerName` is missing.
    *   Estimate: 1
    *   DoD:
        1.  A test in `Create_Page_Tests.cs` verifies `CreateModel.OnPost` with empty `EmployerName` results in `ModelState.IsValid` being false.
        2.  A test in `Create_Page_Tests.cs` verifies `CreateModel.OnPost` with null `EmployerName` results in `ModelState.IsValid` being false.
*   **Task S5.18 (UT PageModels: Server-Side `[Required]` Validation Edit):**
    *   As a developer, I want a unit test to ensure the server prevents editing jobs if `EmployerName` is missing.
    *   Estimate: 1
    *   DoD:
        1.  A test in `Edit_Page_Tests.cs` verifies `EditModel.OnPost` with empty `EmployerName` results in `ModelState.IsValid` being false.
        2.  A test in `Edit_Page_Tests.cs` verifies `EditModel.OnPost` with null `EmployerName` results in `ModelState.IsValid` being false.
*   **Task S5.19 (UI Display: `EmployerName` in Job List ViewComponent):**
    *   As a user, I want to see the Employer Name on job cards in the list so I can quickly identify the employer.
    *   Estimate: 1
    *   DoD:
        1.  The `Default.cshtml` for the `JobListingViewComponent` displays `EmployerName`.
        2.  The display is integrated cleanly into the job card layout.
*   **Task S5.20 (UI Display: `EmployerName` in Dashboard Details):**
    *   As a user, I want to see the Employer Name on the job details page so I have complete information about the job.
    *   Estimate: 1
    *   DoD:
        1.  The job details section on the Dashboard page displays `EmployerName`.
        2.  `DashboardModel` provides `EmployerName` to the details view.
*   **Task S5.21 (UI Display: `EmployerName` in Delete View):**
    *   As an admin, I want to see the Employer Name on the delete confirmation page so I can confirm I'm deleting the correct job.
    *   Estimate: 1
    *   DoD:
        1.  [`Pages/Jobs/Delete.cshtml`](src/Pages/Jobs/Delete.cshtml:1) displays `EmployerName`.
        2.  `DeleteModel` provides `EmployerName` to the view.
*   **Task S5.22 (Dashboard PageModel: Populate `EmployerFilterOptions`):**
    *   As a developer, I want the Dashboard to dynamically generate a list of unique employer names for the filter dropdown so the UI dropdown has data.
    *   Estimate: 1
    *   DoD:
        1.  `DashboardModel` has a `public List<SelectListItem> EmployerFilterOptions { get; set; }` property.
        2.  `OnGetShowJobs` populates `EmployerFilterOptions` with unique, sorted employer names.
*   **Task S5.23 (UI: Add `EmployerName` Filter Dropdown):**
    *   As a user, I want an Employer filter dropdown on the job listings page so I can filter jobs by employer.
    *   Estimate: 1
    *   DoD:
        1.  The filter panel in [`Dashboard.cshtml`](src/Pages/Dashboard.cshtml:1) has an employer filter dropdown.
        2.  The dropdown is bound to `Model.EmployerFilter` and populated with `Model.EmployerFilterOptions`.
*   **Task S5.24 (Dashboard PageModel: `EmployerFilter` Property & Handler Updates):**
    *   As a developer, I want the Dashboard PageModel to manage the state of the employer filter and use it when fetching job data so filtering works correctly.
    *   Estimate: 1
    *   DoD:
        1.  `DashboardModel` has a `[BindProperty(SupportsGet = true)]` string property `EmployerFilter`.
        2.  `OnGetShowJobs` and `OnGetShowDetails` use `Model.EmployerFilter` when calling the service/ViewComponent.
*   **Task S5.25 (Create Blog.cshtml.cs):**
    *   As a developer, I want to create the PageModel for the Blog page so it follows the standard structure.
    *   Estimate: 1
    *   DoD:
        1.  `Blog.cshtml.cs` file is created in the correct directory.
        2.  The file has a basic PageModel class structure and adheres to coding standards.
*   **Task S5.26 (Create Blog_Page_Tests.cshtml.cs):**
    *   As a developer, I want to create the unit test file for the Blog PageModel using the standard naming convention for cshtml test pages.
    *   Estimate: 1
    *   DoD:
        1.  `Blog_Page_Tests.cshtml.cs` file is created in the correct directory.
        2.  The file has a basic test class structure and adheres to coding standards.
*   **Task S5.27 (UT BlogModel Basic Coverage):**
    *   As a developer, I want basic unit test coverage for the Blog PageModel.
    *   Estimate: 1
    *   DoD:
        1.  A test exists for the `OnGet` method in `Blog_Page_Tests.cshtml.cs`.
        2.  The test passes and follows the standard UT template/naming.
*   **Task S5.28 (Refactor UT Structure - Arrange/Act/Assert):**
    *   As a developer, I want to update the structure of existing unit tests to use the Arrange/Act/Assert template with standard variable names and preferred assertion style.
    *   Estimate: 1
    *   DoD:
        1.  All unit tests follow the Arrange/Act/Assert template including `// Arrange`, `// Act`, `// Reset`, and `// Assert` comments.
        2.  Standard variable names (`data`, `result`) are used where applicable.
        3.  `Assert.AreEqual` is preferred for assertions.

**Team Member 3 (14 Tasks, Est: 14 points)**

*   **Task S5.29 (UI: Add "Employers" Navigation Link):**
    *   As a user, I want an "Employers" link in the Dashboard navigation so I can access the list of employers.
    *   Estimate: 1
    *   DoD:
        1.  The main navigation in [`Dashboard.cshtml`](src/Pages/Dashboard.cshtml:1) includes an "Employers" link (`<a>` tag).
        2.  The link targets the `asp-page-handler="ShowEmployers"`.
*   **Task S5.30 (Dashboard PageModel: "Employers" Tab Handler & View State):**
    *   As a developer, I want a handler in the Dashboard PageModel to show the Employers view so the application can switch views when the link is clicked.
    *   Estimate: 1
    *   DoD:
        1.  `DashboardModel` has an `OnGetShowEmployers()` handler.
        2.  The handler sets a view state property (e.g., `ActiveView`) to indicate the Employers view is active.
*   **Task S5.31 (Dashboard PageModel: Populate `DistinctEmployerNames`):**
    *   As a developer, I want the Dashboard PageModel to prepare a unique, sorted list of employer names so the UI can display them in the Employers view.
    *   Estimate: 1
    *   DoD:
        1.  `DashboardModel` has a `public List<string> DistinctEmployerNames { get; set; }` property.
        2.  `OnGetShowEmployers` populates `DistinctEmployerNames` with unique, sorted names from all jobs.
*   **Task S5.32 (UI: Display Employer List in Employers View):**
    *   As a user, I want to see a list of employer names when I'm in the Employers view so I can browse employers.
    *   Estimate: 1
    *   DoD:
        1.  [`Dashboard.cshtml`](src/Pages/Dashboard.cshtml:1) conditionally renders `Model.DistinctEmployerNames` when the Employers view is active.
        2.  Each employer name is displayed as a clickable item (e.g., using a loop and `<a>` tags).
*   **Task S5.33 (UI: Add Employer Search Input in Employers View):**
    *   As a user, I want a search input in the Employers view so I can find specific employers by name.
    *   Estimate: 1
    *   DoD:
        1.  [`Dashboard.cshtml`](src/Pages/Dashboard.cshtml:1) displays a text input when the Employers view is active.
        2.  The input is bound to `Model.EmployerSearchTerm` and submitting the form re-calls `OnGetShowEmployers`.
*   **Task S5.34 (Dashboard PageModel: Implement Employer Search Filtering Logic):**
    *   As a developer, I want the Dashboard PageModel to filter the employer list by search term so the UI only shows matching employers.
    *   Estimate: 1
    *   DoD:
        1.  `OnGetShowEmployers` filters `DistinctEmployerNames` based on the value of `Model.EmployerSearchTerm`.
        2.  The filtering is case-insensitive.
*   **Task S5.39 (Create CareerTips.cshtml.cs):**
    *   As a developer, I want to create the PageModel for the Career Tips page so it follows the standard structure.
    *   Estimate: 1
    *   DoD:
        1.  `CareerTips.cshtml.cs` file is created in the correct directory.
        2.  The file has a basic PageModel class structure and adheres to coding standards.
*   **Task S5.40 (Create Career_Tips_Page_Tests.cshtml.cs):**
    *   As a developer, I want to create the unit test file for the Career Tips PageModel using the standard naming convention for cshtml test pages.
    *   Estimate: 1
    *   DoD:
        1.  `Career_Tips_Page_Tests.cshtml.cs` file is created in the correct directory.
        2.  The file has a basic test class structure and adheres to coding standards.
*   **Task S5.41 (UT CareerTipsModel Basic Coverage):**
    *   As a developer, I want basic unit test coverage for the Career Tips PageModel.
    *   Estimate: 1
    *   DoD:
        1.  A test exists for the `OnGet` method in `Career_Tips_Page_Tests.cshtml.cs`.
        2.  The test passes and follows the standard UT template/naming.
*   **Task S5.42 (Refactor Code Standard - Avoid Negation):**
    *   As a developer, I want to refactor code to avoid using negation (`!`) where possible, except for null checks on strings.
    *   Estimate: 1
    *   DoD:
        1.  Instances of `!` are reviewed and refactored to check for `false` or other positive conditions where appropriate across the codebase, except for null checks on strings.
        2.  The application logic remains unchanged.

**Team Member 4 (14 Tasks, Est: 14 points)**

*   **Task S5.31 (Dashboard PageModel: Populate Employer Data):**
    *   As a developer, I want the Dashboard PageModel to fetch and prepare employer data (Name, Vision, Tech Stack) so the UI can display it in the Employers view.
    *   Estimate: 1
    *   DoD:
        1.  `DashboardModel` has a property to hold a list of Employer objects.
        2.  `OnGetShowEmployers` fetches employer data using the Employer Service.
        3.  The fetched data is stored in the PageModel property.
*   **Task S5.32 (UI: Display Employer List in Employers View Left Panel):**
    *   As a user, I want to see a list of employer names in the left panel of the Employers view so I can browse employers.
    *   Estimate: 1
    *   DoD:
        1.  [`Dashboard.cshtml`](src/Pages/Dashboard.cshtml:1) includes a left panel for the Employers view.
        2.  The left panel displays a list of employer names from the PageModel's employer data.
        3.  Each employer name is displayed as a clickable item.
*   **Task S5.33 (UI: Add Employer Search Input in Employers View):**
    *   As a user, I want a search input in the Employers view so I can find specific employers by name.
    *   Estimate: 1
    *   DoD:
        1.  [`Dashboard.cshtml`](src/Pages/Dashboard.cshtml:1) displays a text input when the Employers view is active.
        2.  The input is bound to `Model.EmployerSearchTerm` and submitting the form re-calls `OnGetShowEmployers`.
*   **Task S5.34 (Dashboard PageModel: Implement Employer Search Filtering Logic):**
    *   As a developer, I want the Dashboard PageModel to filter the employer list by search term so the UI only shows matching employers.
    *   Estimate: 1
    *   DoD:
        1.  `OnGetShowEmployers` filters the employer data based on the value of `Model.EmployerSearchTerm`.
        2.  The filtering is case-insensitive and searches the Employer Name.
*   **Task S5.39 (Create CareerTips.cshtml.cs):**
    *   As a developer, I want to create the PageModel for the Career Tips page so it follows the standard structure.
    *   Estimate: 1
    *   DoD:
        1.  `CareerTips.cshtml.cs` file is created in the correct directory.
        2.  The file has a basic PageModel class structure and adheres to coding standards.
*   **Task S5.40 (Create Career_Tips_Page_Tests.cshtml.cs):**
    *   As a developer, I want to create the unit test file for the Career Tips PageModel using the standard naming convention for cshtml test pages.
    *   Estimate: 1
    *   DoD:
        1.  `Career_Tips_Page_Tests.cshtml.cs` file is created in the correct directory.
        2.  The file has a basic test class structure and adheres to coding standards.
*   **Task S5.41 (UT CareerTipsModel Basic Coverage):**
    *   As a developer, I want basic unit test coverage for the Career Tips PageModel.
    *   Estimate: 1
    *   DoD:
        1.  A test exists for the `OnGet` method in `Career_Tips_Page_Tests.cshtml.cs`.
        2.  The test passes and follows the standard UT template/naming.
*   **Task S5.42 (Refactor Code Standard - Avoid Negation):**
    *   As a developer, I want to refactor code to avoid using negation (`!`) where possible, except for null checks on strings.
    *   Estimate: 1
    *   DoD:
        1.  Instances of `!` are reviewed and refactored to check for `false` or other positive conditions where appropriate across the codebase, except for null checks on strings.
        2.  The application logic remains unchanged.
*   **Task S5.58 (Data: Create `employer.json`):**
    *   As a developer, I want to create the `employer.json` file with sample data so the application has employer information.
    *   Estimate: 1
    *   DoD:
        1.  `src/wwwroot/data/employer.json` file is created.
        2.  The file contains an array of employer objects, each with "employer_name", "company_vision", and "tech_stack" fields.
        3.  Sample data is plausible and non-empty.
*   **Task S5.59 (Model: Create `Employer.cs`):**
    *   As a developer, I want to create the Employer model so employer data can be strongly typed.
    *   Estimate: 1
    *   DoD:
        1.  `src/Models/Employer.cs` file is created.
        2.  The file contains a public class `Employer` with public string properties `EmployerName`, `CompanyVision`, and `TechStack`.
        3.  Properties have `[JsonPropertyName]` attributes for JSON serialization.
*   **Task S5.60 (Model: Add `[Required]` to Employer Model properties):**
    *   As a developer, I want Employer model properties to be mandatory so employer information is reliably captured.
    *   Estimate: 1
    *   DoD:
        1.  `EmployerName`, `CompanyVision`, and `TechStack` properties in `Employer.cs` have the `[Required]` attribute.
*   **Task S5.61 (Service: Create `IEmployerService.cs`):**
    *   As a developer, I want an interface for the Employer service so the service can be abstracted and easily mocked for testing.
    *   Estimate: 1
    *   DoD:
        1.  `src/Services/IEmployerService.cs` file is created.
        2.  The file contains a public interface `IEmployerService` with a method like `List<Employer> GetEmployers()`.
*   **Task S5.62 (Service: Create `JsonFileEmployerService.cs`):**
    *   As a developer, I want an implementation of the Employer service that reads from `employer.json` so the application can access employer data.
    *   Estimate: 1
    *   DoD:
        1.  `src/Services/JsonFileEmployerService.cs` file is created.
        2.  The file contains a public class `JsonFileEmployerService` implementing `IEmployerService`.
        3.  The `GetEmployers` method reads and deserializes data from `src/wwwroot/data/employer.json`.
        4.  The service is registered in `Startup.cs`.
*   **Task S5.63 (UT Model: Employer Model Validation):**
    *   As a developer, I want unit tests for the Employer model's `[Required]` validation so I can be confident the validation works.
    *   Estimate: 1
    *   DoD:
        1.  A test file for the Employer model is created (e.g., `Employer_Model_Tests.cs`).
        2.  Tests verify that null or empty values for required properties cause validation errors.
        3.  Tests follow standard UT template/naming.
*   **Task S5.64 (UT Service: `GetEmployers`):**
    *   As a developer, I want a unit test to verify the Employer service correctly reads data from the JSON file.
    *   Estimate: 1
    *   DoD:
        1.  A test in `Json_File_Employer_Service_Tests.cs` verifies `GetEmployers` returns the expected data from a mock JSON source.
        2.  The test follows standard UT template/naming.
*   **Task S5.65 (Dashboard PageModel: Fetch Employer Data):**
    *   As a developer, I want the Dashboard PageModel to fetch employer data using the new service so the UI has data to display.
    *   Estimate: 1
    *   DoD:
        1.  `DashboardModel` is updated to inject `IEmployerService`.
        2.  `OnGetShowEmployers` calls `_employerService.GetEmployers()`.
        3.  A property is added to `DashboardModel` to hold the list of Employer objects.
*   **Task S5.66 (Dashboard UI: Display Employer Details):**
    *   As a user, when I select an employer from the list, I want to see their Vision and Tech Stack displayed in the right panel.
    *   Estimate: 1
    *   DoD:
        1.  [`Dashboard.cshtml`](src/Pages/Dashboard.cshtml:1) includes a right panel area for displaying employer details.
        2.  This area displays the selected employer's Vision and Tech Stack.
*   **Task S5.67 (Dashboard UI Logic: Select Employer):**
    *   As a user, I want to be able to click an employer name in the left panel and see their details appear in the right panel.
    *   Estimate: 1
    *   DoD:
        1.  JavaScript or Razor logic handles click events on employer names in the left panel.
        2.  Clicking an employer name updates the right panel to show the corresponding employer's details.
*   **Task S5.68 (UT Dashboard PageModel: Fetch Employer Data):**
    *   As a developer, I want a unit test for the Dashboard PageModel's logic that fetches employer data so I can be confident it works.
    *   Estimate: 1
    *   DoD:
        1.  A test verifies `OnGetShowEmployers` correctly calls the injected `IEmployerService.GetEmployers()`.
        2.  The test follows standard UT template/naming.
*   **Task S5.69 (Dashboard UI: Conditional Filter Panel):**
    *   As a user, I want the filter panel on the Dashboard to show different filtering options depending on whether I'm viewing Jobs or Employers.
    *   Estimate: 1
    *   DoD:
        1.  [`Dashboard.cshtml`](src/Pages/Dashboard.cshtml:1) includes conditional rendering logic for the filter panel.
        2.  When the Jobs view is active, the full set of job filters (including Employer dropdown) is displayed.
        3.  When the Employers view is active, only the employer search filter is displayed.
*   **Task S5.45 (UT Dashboard PageModel: Employer Data Population):**
    *   As a developer, I want a unit test for the Dashboard PageModel's employer data population so I can be confident the data is correct.
    *   Estimate: 1
    *   DoD:
        1.  A test verifies `OnGetShowEmployers` fetches and stores employer data correctly from a mock service.
        2.  The test verifies the data includes Name, Vision, and Tech Stack.
        3.  The test follows the standard UT template/naming.
*   **Task S5.46 (UT Dashboard PageModel: Employer Search Filtering):**
    *   As a developer, I want a unit test for the Dashboard PageModel's employer search filtering so I can be confident the search works.
    *   Estimate: 1
    *   DoD:
        1.  A test verifies `OnGetShowEmployers` filters employer data correctly based on a search term.
        2.  The test verifies the filtering is case-insensitive and searches the Employer Name.
        3.  The test follows the standard UT template/naming.
*   **Task S5.49 (Commenting - `EmployerName` in Model):**
    *   As a developer, I want clear comments for the `EmployerName` property in the model so other developers understand its purpose.
    *   Estimate: 1
    *   DoD:
        1.  The `EmployerName` property in [`JobListing.cs`](src/Models/JobListing.cs:1) has a `///` comment.
        2.  The comment accurately describes the property.
*   **Task S5.50 (Commenting - `EmployerName` in Service):**
    *   As a developer, I want clear comments for the service logic handling `EmployerName` so other developers understand how it's used.
    *   Estimate: 1
    *   DoD:
        1.  Relevant sections in [`JsonFileJobService.cs`](src/Services/JsonFileJobService.cs:1) handling `EmployerName` have `//` comments.
        2.  Comments accurately explain the purpose of the logic.
*   **Task S5.51 (Commenting - `EmployerName` in Create PageModel):**
    *   As a developer, I want comments in the Create PageModel related to `EmployerName` so other developers understand the handling.
    *   Estimate: 1
    *   DoD:
        1.  [`Pages/Jobs/Create.cshtml.cs`](src/Pages/Jobs/Create.cshtml.cs:1) has `///` or `//` comments for `EmployerName` logic/properties.
        2.  Comments accurately reflect code purpose.
*   **Task S5.53 (Refactoring - Comprehensive Comment Review & Formatting):**
    *   As a developer, I want to review all existing C# comments and code formatting to ensure they meet the defined coding standards, improving code readability and consistency.
    *   Estimate: 1
    *   DoD:
        1.  All `.cs` files include `///` comments for classes and methods.
        2.  All `.cs` files include `//` comments for attributes, properties, and variables.
        3.  Code formatting (line breaks, using statements, file endings) adheres to standards.
*   **Task S5.54 (Create HowItWorks.cshtml.cs):**
    *   As a developer, I want to create the PageModel for the How It Works page so it follows the standard structure.
    *   Estimate: 1
    *   DoD:
        1.  `HowItWorks.cshtml.cs` file is created in the correct directory.
        2.  The file has a basic PageModel class structure and adheres to coding standards.
*   **Task S5.55 (Create How_It_Works_Page_Tests.cshtml.cs):**
    *   As a developer, I want to create the unit test file for the How It Works PageModel using the standard naming convention for cshtml test pages.
    *   Estimate: 1
    *   DoD:
        1.  `How_It_Works_Page_Tests.cshtml.cs` file is created in the correct directory.
        2.  The file has a basic test class structure and adheres to coding standards.
*   **Task S5.56 (UT HowItWorksModel Basic Coverage):**
    *   As a developer, I want basic unit test coverage for the How It Works PageModel.
    *   Estimate: 1
    *   DoD:
        1.  A test exists for the `OnGet` method in `How_It_Works_Page_Tests.cshtml.cs`.
        2.  The test passes and follows the standard UT template/naming.
*   **Task S5.57 (Refactor Naming Conventions & 1 Var Per Line):**
    *   As a developer, I want to refactor code to follow naming conventions (underscores, file names, data models, casing) and formatting (1 variable per line).
    *   Estimate: 1
    *   DoD:
        1.  File names follow conventions, including `.cshtml.cs` for PageModels and `.cshtml.test.cs` for `.cshtml` test files.
        2.  Class names follow conventions (PascalCase).
        3.  Within test files (`.cshtml.test.cs` and other test files), method names, attribute names, property names, and variable names use underscores after every word.
        4.  Within non-test `.cs` files, method names, attribute names, property names, and variable names follow standard C# conventions (PascalCase for public/protected/internal members, camelCase for private members and local variables).
        5.  Properties/variables are defined one per line.
        6.  Public class variables are PascalCase.
        7.  Local method variables are camelCase.

---
**Total Current Sprint Tasks: 59**
**Total Current Sprint Estimates: 59 points**