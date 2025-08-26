Here is the revised Sprint 3 plan, incorporating feedback to ensure all previously mentioned functionality (CRUD, Tests, Data, Pagination, Branding) is covered with a target of 6-7 tasks per team member, maximizing independence and using INVEST format.

**Total Tasks in Current Sprint: 27**

```markdown
# CampusConnect - Sprint 3 Plan (Team-Focused, Complete Coverage)

## Sprint 3 Goal

Implement Create, Update, and Delete functionality for Job Listings, accessible to authorized users (simulated for now), allowing management of job postings within the system. Ensure new functionality is covered by unit tests, the job listings page supports pagination for larger datasets, and basic site structure/branding is updated.

## Task Board Simulation

### Backlog (Selected High-Level Goals for Later Sprints)

1.  **Epic:** Implement Company Profiles Feature
2.  **Epic:** Implement Student Profiles & Basic Auth
3.  **Epic:** Enhance Filtering/Sorting (Job Type, etc.)
4.  **Epic:** Implement User Roles & Permissions
5.  **Epic:** Input Validation for CRUD Forms
6.  **Epic:** Populate Content Pages (About Us, Blog etc.)
7.  **Epic:** Unit Tests for Pagination Logic

### Current Sprint (27 Tasks)

**Team Member 1 Focus: Create Functionality (Backend & UI)**

*   **Task T1-1 (Create - Service Interface):**
    *   **User Story:** As a developer, I want to define the `AddJob(JobListing newJob)` method signature in the `IJobService` interface, so that a contract exists for adding jobs via the service layer.
    *   **Estimate:** 0.5
    *   **Acceptance Criteria (DoD):** 1. `IJobService.cs` contains the `AddJob` method signature. 2. Project compiles without errors related to the interface definition.
*   **Task T1-2 (Create - Service Impl: Read/ID/Timestamp):**
    *   **User Story:** As a developer, I want the `AddJob` method in `JsonFileJobService` to read existing jobs, assign a unique ID, and set the current timestamp for a new job, so that essential metadata is prepared before saving.
    *   **Estimate:** 1.0
    *   **Acceptance Criteria (DoD):** 1. `AddJob` method structure exists in `JsonFileJobService.cs`. 2. Method reads existing jobs, assigns `newJob.Id` (e.g., Guid), and assigns `newJob.DatePosted` (e.g., `DateTime.UtcNow`).
*   **Task T1-3 (Create - Service Impl: Append/Write):**
    *   **User Story:** As a developer, I want the `AddJob` method in `JsonFileJobService` to append the processed `newJob` to the jobs list and write the updated list back to `jobs.json`, so that the new job is persisted correctly.
    *   **Estimate:** 0.5
    *   **Acceptance Criteria (DoD):** 1. The `newJob` object is added to the list before writing. 2. The complete, updated list is serialized and written back to `jobs.json`.
*   **Task T1-4 (Create - PageModel Structure & Logic):**
    *   **User Story:** As a developer, I want to create the `Create.cshtml.cs` PageModel with `IJobService` injected, a bindable `NewJob` property, and an `OnPost` handler that calls the `AddJob` service method and redirects to Index, so the core page logic is implemented.
    *   **Estimate:** 1.0
    *   **Acceptance Criteria (DoD):** 1. `Create.cshtml.cs` exists, injects service, has `NewJob` property. 2. `OnPost` method calls `_jobService.AddJob(NewJob)` and returns `RedirectToPageResult("./Index")`.
*   **Task T1-5 (Create - View Structure):**
    *   **User Story:** As a developer, I want to create the `Create.cshtml` view file, link it to the `CreateModel`, and add the basic HTML form structure (`<form method="post">`), so that the page is ready for input fields.
    *   **Estimate:** 0.5
    *   **Acceptance Criteria (DoD):** 1. `Create.cshtml` exists with `@model` directive. 2. View contains an empty `<form method="post">` tag.
*   **Task T1-6 (Create - View Inputs):**
    *   **User Story:** As an admin creating a job, I want input fields for Title, Company Name, Location, and Apply URL in the Create Job form, so I can specify these core details.
    *   **Estimate:** 0.5
    *   **Acceptance Criteria (DoD):** 1. `Create.cshtml` contains `<input>` elements bound via `asp-for` to `NewJob.Title`, `NewJob.CompanyName`, `NewJob.Location`, `NewJob.ApplyUrl`. 2. Appropriate input types are used (e.g., text, url).
*   **Task T1-7 (Create - View Textarea & Button):**
    *   **User Story:** As an admin creating a job, I want a textarea for the Description and a "Create Job" submit button in the Create Job form, so I can provide details and submit the listing.
    *   **Estimate:** 0.5
    *   **Acceptance Criteria (DoD):** 1. `Create.cshtml` contains a `<textarea>` element bound via `asp-for` to `NewJob.Description`. 2. A `<button type="submit">` exists within the form.

**Team Member 2 Focus: Update Functionality (Backend & UI)**

*   **Task T2-1 (Update - Service Interface):**
    *   **User Story:** As a developer, I want to define the `UpdateJob(JobListing updatedJob)` method signature in the `IJobService` interface, so that a contract exists for modifying existing jobs.
    *   **Estimate:** 0.5
    *   **Acceptance Criteria (DoD):** 1. `IJobService.cs` contains the `UpdateJob` method signature. 2. Project compiles without errors related to the interface definition.
*   **Task T2-2 (Update - Service Impl):**
    *   **User Story:** As a developer, I want to implement the `UpdateJob` method in `JsonFileJobService` to read jobs, find the job by ID, replace it with the updated version, and write back to `jobs.json`, so that job modifications are persisted.
    *   **Estimate:** 1.0
    *   **Acceptance Criteria (DoD):** 1. `UpdateJob` method implemented in `JsonFileJobService.cs`. 2. Method reads, finds/replaces job by ID, and writes the updated list back to `jobs.json`.
*   **Task T2-3 (Update - PageModel Structure & OnGet):**
    *   **User Story:** As a developer, I want to create the `Edit.cshtml.cs` PageModel with `IJobService` injected, a bindable `JobToUpdate` property, and implement `OnGet` to fetch the job by ID for pre-filling the form, so the edit page can load existing data.
    *   **Estimate:** 1.0
    *   **Acceptance Criteria (DoD):** 1. `Edit.cshtml.cs` exists, injects service, has `JobToUpdate` property. 2. `OnGet(string id)` calls `GetJobById`, populates `JobToUpdate`, returns `NotFound` if null.
*   **Task T2-4 (Update - PageModel OnPost):**
    *   **User Story:** As a developer, I want to implement the `OnPost` method in `EditModel` to call the `UpdateJob` service method and redirect to the Index page, so that submitting the form saves changes and returns the user to the list.
    *   **Estimate:** 0.5
    *   **Acceptance Criteria (DoD):** 1. `OnPost` method calls `_jobService.UpdateJob(JobToUpdate)`. 2. Method returns `RedirectToPageResult("./Index")`.
*   **Task T2-5 (Update - View Structure):**
    *   **User Story:** As a developer, I want to create the `Edit.cshtml` view file, link it to the `EditModel`, add the basic HTML form structure (`<form method="post">`), and include a hidden input for the Job ID, so the page is ready for editable fields.
    *   **Estimate:** 0.5
    *   **Acceptance Criteria (DoD):** 1. `Edit.cshtml` exists with `@model` directive. 2. View contains `<form method="post">` and a hidden input for `JobToUpdate.Id`.
*   **Task T2-6 (Update - View Inputs):**
    *   **User Story:** As an admin editing a job, I want input fields pre-filled with the existing Title, Company Name, Location, and Apply URL, so I can easily modify these details.
    *   **Estimate:** 0.5
    *   **Acceptance Criteria (DoD):** 1. `Edit.cshtml` contains `<input>` elements bound via `asp-for` to `JobToUpdate.Title`, `JobToUpdate.CompanyName`, `JobToUpdate.Location`, `JobToUpdate.ApplyUrl`. 2. Fields display existing data when page loads.
*   **Task T2-7 (Update - View Textarea & Button):**
    *   **User Story:** As an admin editing a job, I want a textarea pre-filled with the existing Description and a "Save Changes" submit button, so I can modify details and save the update.
    *   **Estimate:** 0.5
    *   **Acceptance Criteria (DoD):** 1. `Edit.cshtml` contains a `<textarea>` element bound via `asp-for` to `JobToUpdate.Description`. 2. A `<button type="submit">` exists within the form.

**Team Member 3 Focus: Delete Functionality & UI Links/Tests**

*   **Task T3-1 (Delete - Service Interface):**
    *   **User Story:** As a developer, I want to define the `DeleteJob(string id)` method signature in the `IJobService` interface, so that a contract exists for removing jobs.
    *   **Estimate:** 0.5
    *   **Acceptance Criteria (DoD):** 1. `IJobService.cs` contains the `DeleteJob` method signature. 2. Project compiles without errors related to the interface definition.
*   **Task T3-2 (Delete - Service Impl):**
    *   **User Story:** As a developer, I want to implement the `DeleteJob` method in `JsonFileJobService` to read jobs, remove the job matching the given ID, and write the modified list back to `jobs.json`, so that jobs can be permanently removed.
    *   **Estimate:** 0.75
    *   **Acceptance Criteria (DoD):** 1. `DeleteJob` method implemented in `JsonFileJobService.cs`. 2. Method reads, removes job by ID, and writes the updated list back to `jobs.json`.
*   **Task T3-3 (Delete - PageModel Structure & Logic):**
    *   **User Story:** As a developer, I want to create the `Delete.cshtml.cs` PageModel with `IJobService` injected, implement `OnGet` to fetch the job for confirmation display, and implement `OnPost` to call the `DeleteJob` service method and redirect, so the delete workflow is handled.
    *   **Estimate:** 1.0
    *   **Acceptance Criteria (DoD):** 1. `Delete.cshtml.cs` exists, injects service, has `JobToDelete` property. 2. `OnGet` fetches job/returns `NotFound`; `OnPost` calls `DeleteJob` and redirects to Index.
*   **Task T3-4 (Delete - View Structure & Confirmation):**
    *   **User Story:** As an admin wanting to delete a job, I want a confirmation page (`Delete.cshtml`) that displays the job details and requires me to click a "Confirm Delete" button within a form, so I don't delete jobs accidentally.
    *   **Estimate:** 0.75
    *   **Acceptance Criteria (DoD):** 1. `Delete.cshtml` exists with `@model`, displays `JobToDelete` details. 2. View contains `<form method="post">`, hidden ID input, "Confirm Delete" submit button, and "Cancel" link.
*   **Task T3-5 (UI - Add Edit/Delete Links):**
    *   **User Story:** As an admin viewing the job listings, I want "Edit" and "Delete" links next to each job, so I can easily access the management functions for that specific job.
    *   **Estimate:** 0.5
    *   **Acceptance Criteria (DoD):** 1. `Index.cshtml` job card/row includes an `<a>` tag using `asp-page="./Edit"` with the job ID route parameter. 2. `Index.cshtml` job card/row includes an `<a>` tag using `asp-page="./Delete"` with the job ID route parameter.
*   **Task T3-6 (Unit Tests - CRUD Service):**
    *   **User Story:** As a developer, I want unit tests for the `AddJob`, `UpdateJob`, and `DeleteJob` methods in `JsonFileJobServiceTests.cs`, so that the core data manipulation logic is verified.
    *   **Estimate:** 1.5 *(Covers testing all 3 service methods)*
    *   **Acceptance Criteria (DoD):** 1. Tests exist for `AddJob` (verify ID/Timestamp/write), `UpdateJob` (found/not found), `DeleteJob` (found/not found). 2. All added service tests pass.
*   **Task T3-7 (Unit Tests - Delete PageModel):**
    *   **User Story:** As a developer, I want unit tests for the `DeleteModel` (`DeleteTests.cs`), so that its interaction with the service and action results (`OnGet` found/not found, `OnPost` redirect) are verified.
    *   **Estimate:** 0.75
    *   **Acceptance Criteria (DoD):** 1. `DeleteTests.cs` exists with necessary mocks. 2. Tests for `OnGet` (found/not found) and `OnPost` (service call/redirect) exist and pass.

**Team Member 4 Focus: Data, Pagination, Branding & UI Tests**

*   **Task T4-1 (Data - Populate jobs.json):**
    *   **User Story:** As a developer/tester, I want the `jobs.json` file to contain significantly more sample job listings (approx. 50), so that features like pagination can be tested more realistically.
    *   **Estimate:** 0.5
    *   **Acceptance Criteria (DoD):** 1. `src/wwwroot/data/jobs.json` file updated. 2. File contains approximately 50 valid `JobListing` objects with varied data.
*   **Task T4-2 (Pagination - PageModel Logic):**
    *   **User Story:** As a developer, I want to modify the `IndexModel` (`Index.cshtml.cs`) to handle pagination logic, including accepting a page number, calculating total pages, and using Skip/Take, so only the correct subset of jobs is fetched.
    *   **Estimate:** 1.0
    *   **Acceptance Criteria (DoD):** 1. `IndexModel` has properties for `CurrentPage`, `TotalPages`. 2. `OnGet` calculates `TotalPages` and uses `.Skip().Take()` correctly *after* filtering.
*   **Task T4-3 (Pagination - View Controls):**
    *   **User Story:** As a user browsing many jobs, I want pagination controls (like Previous, Next, page numbers) displayed on the Job Listings page, so I can navigate through the results easily.
    *   **Estimate:** 1.25
    *   **Acceptance Criteria (DoD):** 1. `Index.cshtml` displays pagination UI elements conditionally (`TotalPages > 1`). 2. Controls link correctly (`asp-route-currentPage`), preserve filters, and disable appropriately.
*   **Task T4-4 (Branding - Update Text & Pages):**
    *   **User Story:** As a user, I want the site branding to be consistent ("CampusConnect") and be able to navigate to basic informational pages (About Us, Blog, etc.), so the site feels complete and professional.
    *   **Estimate:** 1.0
    *   **Acceptance Criteria (DoD):** 1. "Contoso Crafts" replaced with "CampusConnect" in relevant files/titles. 2. Placeholder pages (AboutUs, Blog, HowItWorks, CareerTips) created with basic content and header links updated.
*   **Task T4-5 (Unit Tests - Create PageModel):**
    *   **Description:** As a developer, I want unit tests for the `CreateModel` (`CreateTests.cs`), so that its interaction with the service and action result are verified with full coverage, ensuring the create logic works as expected. *(V)* Depends only on `CreateModel` structure. *(I)*
    *   **Estimate:** 1.0 *(E, S)*
    *   **Acceptance Criteria (DoD):** 1. Tests for `OnPost` (service call/redirect) exist and pass. 2. Achieves 100% code coverage for the `CreateModel` class. *(T)*
*   **Task T4-6 (Unit Tests - Edit PageModel):**
    *   **Description:** As a developer, I want unit tests for the `EditModel` (`EditTests.cs`), so that its interaction with the service and action results are verified with full coverage, ensuring the edit logic works correctly. *(V)* Depends only on `EditModel` structure. *(I)*
    *   **Estimate:** 1.25 *(E, S)*
    *   **Acceptance Criteria (DoD):** 1. Tests for `OnGet` (found/not found) & `OnPost` (service call/redirect) exist and pass. 2. Achieves 100% code coverage for the `EditModel` class. *(T)*
*   **Task T4-7 (Homepage Button Update):**
    *   **Description:** As a user landing on the homepage, I want the main call-to-action button to say "Start your career here" and link to a new dashboard page, so the purpose is clearer and directs me to the main application area. *(V)* Simple text and link change, independent of dashboard content. *(I)*
    *   **Estimate:** 0.5 *(E, S)*
    *   **Acceptance Criteria (DoD):** 1. Button text on `src/Pages/Index.cshtml` (hero section) changed to "Start your career here". 2. Button's `href` or `asp-page` attribute points to `/Dashboard`. *(T)*
*   **Task T4-8 (Dashboard Page Creation):**
    *   **Description:** As a developer, I want to create the basic Razor Page files (`Dashboard.cshtml` and `Dashboard.cshtml.cs`) for the new dashboard area, so there's a destination for the homepage button and a place to build the layout. *(V)* Creates necessary files, independent of layout details. *(I)*
    *   **Estimate:** 0.5 *(E, S)*
    *   **Acceptance Criteria (DoD):** 1. `Pages/Dashboard.cshtml` file exists. 2. `Pages/Dashboard.cshtml.cs` file exists with a basic `DashboardModel` class inheriting `PageModel`. *(T)*
*   **Task T4-9 (Dashboard Layout - Outer Sidebar & Main Content):**
    *   **Description:** As a developer, I want to implement a two-column layout on `Dashboard.cshtml` with a main left sidebar (containing links like Jobs, Events, etc.) and a main content area on the right, so the primary dashboard structure is established. *(V)* Depends on T4-8, focuses only on the outer layout. *(I)*
    *   **Estimate:** 0.75 *(E, S)*
    *   **Acceptance Criteria (DoD):** 1. `Dashboard.cshtml` uses CSS (e.g., Bootstrap grid) for a main left sidebar and a right content area. 2. The left sidebar contains navigation links (can be `#` initially) for "Jobs", "Events", "People", "Employers", "Career Center". *(T)*
*   **Task T4-10 (Dashboard Jobs Layout - Middle & Right Panes):**
    *   **Description:** As a developer, when the "Jobs" section is active, I want the main content area of the dashboard to be further divided into a middle pane (for job listings) and a right pane (for job details), so the three-pane structure for jobs is set up. *(V)* Depends on T4-9, focuses only on the inner layout structure for the Jobs view. *(I)*
    *   **Estimate:** 0.75 *(E, S)*
    *   **Acceptance Criteria (DoD):** 1. Within the main content area of `Dashboard.cshtml` (or a partial loaded for Jobs), CSS creates a middle column and a right column. 2. Placeholder content exists in both the middle and right columns to verify layout. *(T)*
*   **Task T4-11 (Dashboard Jobs - Render List in Middle Pane):**
    *   **Description:** As a developer, I want the middle pane of the "Jobs" dashboard section to render the scrollable list of job listings (using existing logic like search/filters/pagination), so users can browse available jobs within the dashboard. *(V)* Depends on T4-10 structure and existing Index logic. *(I)*
    *   **Estimate:** 1.5 *(E, S)*
    *   **Acceptance Criteria (DoD):** 1. The middle pane renders job listings fetched via `IJobService`. 2. Search, filter, and pagination controls are present and functional within this middle pane (may require adapting `IndexModel` logic or creating a ViewComponent). *(T)*
*   **Task T4-12 (Dashboard Jobs - Render Details Placeholder in Right Pane):**
    *   **Description:** As a developer, I want the right pane of the "Jobs" dashboard section to initially display a placeholder message (e.g., "Select a job to view details"), so the user knows where details will appear. *(V)* Simple UI element, depends on T4-10 structure. *(I)*
    *   **Estimate:** 0.25 *(E, S)*
    *   **Acceptance Criteria (DoD):** 1. The right pane in `Dashboard.cshtml` (or Jobs partial) displays placeholder text. 2. This placeholder is visible when no job is selected. *(T)*
*   **Task T4-13 (Dashboard Jobs - Load Details on Click - Basic):**
    *   **Description:** As a user browsing jobs on the dashboard, when I click a job title in the middle list pane, I want the details of that job to load and display in the right details pane, so I can view information without leaving the dashboard. *(V)* High value, but complex. Depends on T4-10, T4-11, T4-12. *(I)*
    *   **Estimate:** 2.0 *(E, S)*
    *   **Acceptance Criteria (DoD):** 1. Clicking a job link/title in the middle pane triggers an action (e.g., JS call, partial page update). 2. The corresponding job details (fetched via `GetJobById`) are displayed in the right pane, replacing the placeholder. *(T)*
*   **Task T4-14 (Pagination - PageModel Logic):**
    *   **Description:** As a developer, I want to modify the relevant PageModel (likely `IndexModel` initially, potentially adapted for the Dashboard later) to handle pagination logic, including accepting a page number, calculating total pages, and using Skip/Take, so only the correct subset of jobs is fetched for display. *(V)* Core logic for pagination. *(I)*
    *   **Estimate:** 1.0 *(E, S)*
    *   **Acceptance Criteria (DoD):** 1. The PageModel handling job listings has properties for `CurrentPage`, `TotalPages`. 2. `OnGet` calculates `TotalPages` & uses `.Skip().Take()` correctly *after* filtering. *(T)*
*   **Task T4-15 (Pagination - View Controls):**
    *   **Description:** As a user browsing many jobs, I want pagination controls (like Previous, Next, page numbers) displayed below the job list, so I can navigate through the results easily. *(V)* UI for pagination. Depends on T4-14. *(I)*
    *   **Estimate:** 1.25 *(E, S)*
    *   **Acceptance Criteria (DoD):** 1. View (Index or Dashboard middle pane) displays pagination UI conditionally (`TotalPages > 1`). 2. Controls link correctly, preserve filters, disable appropriately. *(T)*

```