# CampusConnect - Sprint 1 Plan

## Sprint 1 Goal

Implement the core Index page functionality for displaying job listings and integrate the standard unit tests from the reference project (`mckeemseattleu/ContosoCraftsmckee2024.WebSite`) to establish a baseline for quality assurance.

## Task Board Simulation

### Backlog (Ordered - Top Priority First)

1.  **Card:** Display Job Listings

2.  **Card:** Basic Job Search Functionality

3.  **Card:** Company Profiles Page

4.  **Card:** Link Jobs to Company Profiles

5.  **Card:** Display Company Logos on Job Listings

6.  **Card:** Filter Jobs by Type (Internship, Full-time)

7.  **Card:** Student User Profiles (Basic)

8.  **Card:** Add 'Apply Now' button linking externally

### Current Sprint (Tasks to be pulled into Active)

*   **Task 1:**

    *   **User Story:** As a developer, I want to create a `JsonFileJobService` similar to `JsonFileProductService`, so that job listings can be read from a `jobs.json` file.

    *   **Estimate:** 2 points

    *   **Acceptance Criteria (DoD):**

        *   `JsonFileJobService.cs` created in the `Services` folder.

        *   Service reads data from `wwwroot/data/jobs.json` (create a sample file).

        *   `GetJobs()` method returns `IEnumerable<JobListing>`.

        *   Service is registered in `Startup.cs` or `Program.cs` for dependency injection.

        *   Code compiles successfully.

*   **Task 2:**

    *   **User Story:** As a developer, I want to create an `Index.cshtml` Razor Page for Jobs that displays a list of job listings fetched by the controller/page model, so that users can see available jobs.

    *   **Estimate:** 2 points

    *   **Acceptance Criteria (DoD):**

        *   `Pages/Jobs/Index.cshtml` and `Index.cshtml.cs` created.

        *   Page Model fetches job listings using `JsonFileJobService`.

        *   The `Index.cshtml` view iterates through the `JobListings` property and displays at least the Job Title and Company Name for each job.

        *   Page renders without errors.

*   **Task 3:**

    *   **User Story:** As a developer, I want to incorporate unit tests for the `JobListing` model (assuming structure from reference repo), so that the model's integrity is verified.

    *   **Estimate:** 1 point

    *   **Acceptance Criteria (DoD):**

        *   Relevant model tests (similar to any existing product model tests or based on reference repo structure) are added to `UnitTests/Models/`.

        *   Tests cover basic property validation or instantiation.

        *   All model unit tests pass.

*   **Task 4:**

    *   **User Story:** As a developer, I want to incorporate unit tests for the `JobsController` Index action (or `Jobs/Index.cshtml.cs` PageModel `OnGet` method), adapting tests from the reference repository, so that the controller/page model logic for fetching jobs is verified.

    *   **Estimate:** 2 points

    *   **Acceptance Criteria (DoD):**

        *   Relevant controller/page model tests are added to `UnitTests/Controllers/` or a new `UnitTests/Pages/` directory.

        *   Tests mock the `JsonFileJobService` dependency.

        *   Tests verify that the correct data is fetched and passed to the view/page.

        *   All controller/page model unit tests pass.

### Active Tasks (1 per Team Member)

*   **Task 5 (Team Member 1):**

    *   **User Story:** As a developer, I want to define the `JobListing` model class with relevant properties (e.g., Id, Title, CompanyName, Description, Location, ApplyUrl), so that job data can be structured consistently within the application.

    *   **Estimate:** 1 point

    *   **Acceptance Criteria (DoD):**

        *   `JobListing.cs` created in the `Models` folder.

        *   Class includes properties: `string Id`, `string Title`, `string CompanyName`, `string Description`, `string Location`, `string ApplyUrl`. (Adjust properties as needed).

        *   Appropriate data types and annotations (like `[JsonPropertyName("...")]` if matching JSON) are used.

        *   Code compiles successfully.

*   **Task 6 (Team Member 2):**

    *   **User Story:** As a developer, I want to create a `JobsController` (or modify `Pages/Jobs/Index.cshtml.cs` PageModel) with an `Index` action (or `OnGet` method) that uses the `JsonFileJobService`, so that job data can be retrieved and prepared for display on the Index page.

    *   **Estimate:** 1 point

    *   **Acceptance Criteria (DoD):**

        *   `JobsController.cs` created (or `Index.cshtml.cs` modified).

        *   `JsonFileJobService` is injected via the constructor.

        *   `Index` action (or `OnGet` method) calls the service's `GetJobs()` method.

        *   The retrieved job list is stored in a public property (e.g., `JobListings`) for the view/page to access.

        *   Code compiles successfully.

*   **Task 7 (Team Member 3):**

    *   **User Story:** As a developer, I want to add navigation links and routing for the new Jobs Index page, so that users can easily access the job listings from the main site layout.

    *   **Estimate:** 1 point

    *   **Acceptance Criteria (DoD):**

        *   A navigation link "Jobs" is added to `Pages/Shared/_Layout.cshtml` (or equivalent).

        *   The link correctly points to the `/Jobs` or `/Jobs/Index` route.

        *   The Jobs Index page is accessible by clicking the link and by typing the URL directly.

*   **Task 8 (Team Member 4):**

    *   **User Story:** As a developer, I want to incorporate unit tests for the `JsonFileJobService`, adapting tests from the reference repository (likely based on `JsonFileProductService` tests), so that the job data retrieval logic is verified.

    *   **Estimate:** 2 points

    *   **Acceptance Criteria (DoD):**

        *   Relevant service tests are added to `UnitTests/Services/` (create directory if needed).

        *   Tests cover scenarios like reading the JSON file (mocking file system if necessary) and returning the correct data structure.

        *   All service unit tests pass.