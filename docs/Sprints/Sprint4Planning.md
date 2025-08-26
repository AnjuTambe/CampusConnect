Okay, I understand. You want to add tasks to Sprint 4 to ensure unit tests exist for the header pages (Privacy, HowItWorks, Error, CareerTips, Blog, AboutUs) and the `JobListing` model.

I will add these tasks to the Sprint 4 plan, distributing them among the team members to maintain a balanced workload.

Here's the updated Sprint 4 plan:

```markdown
# CampusConnect - Sprint 4 Plan (Granular Breakdown - Rounded Estimates, Full Coverage, Commenting & Bug Fix Focus)

## Sprint 4 Goal

Enhance the job listing functionality by integrating a `JobType` field, implement client-side **and server-side** input validation (including search length limits), **implement an admin control visibility toggle**, fix identified bugs related to date handling, search, and UI spacing, achieve 100% unit test coverage across all existing C# components (PageModels, Services, Models, ViewComponents), **write basic unit tests for key JavaScript logic**, and ensure all C# code adheres to the project's commenting standards.

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
11. **Task:** Setup End-to-End (E2E) UI Testing Framework.

### Current Sprint (48 Tasks)

*(Tasks distributed among 4 Team Members)*

**Team Member 1: `JobType` Data & Display, CRUD PageModel Tests, Core Commenting, Server Validation, UI Spacing, Admin Toggle UI/State, JS Tests, Model Tests** (15 Tasks, Est: 15 points)

*   **Task S4T1-1 (Model Update - `JobType`):** Estimate: 1
    *   User Story: As a developer, I need to update the application's job data structure to include a `JobType` field.
    *   DoD: 1. [`JobListing.cs`](src/Models/JobListing.cs:1) contains `JobType` property. 2. Project compiles.
*   **Task S4T1-2 (Data Update - `jobs.json`):** Estimate: 1
    *   User Story: As a developer, I need to update the sample job data file to include plausible `JobType` values for all entries.
    *   DoD: 1. [`src/wwwroot/data/jobs.json`](src/wwwroot/data/jobs.json:1) includes `JobType` for every job. 2. Application reads data without deserialization errors.
*   **Task S4T1-3 (Display `JobType` - Index List):** Estimate: 1
    *   User Story: As a user browsing jobs, I want to see the `JobType` displayed on the main job listings page.
    *   DoD: 1. `JobType` visible on [`Pages/Jobs/Index.cshtml`](src/Pages/Jobs/Index.cshtml:1) and Dashboard. 2. Display integrates cleanly.
*   **Task S4T1-4 (Display `JobType` - Details Page):** Estimate: 1
    *   User Story: As a user viewing job details, I want the `JobType` clearly displayed on the Job Details page (Dashboard & standalone).
    *   DoD: 1. `JobType` visible on [`Pages/Jobs/Details.cshtml`](src/Pages/Jobs/Details.cshtml:1) and Dashboard details pane. 2. Respective Models provide `JobType`.
*   **Task S4T1-5 (Unit Tests - CreateModel Coverage):** Estimate: 1
    *   User Story: As a developer, I want to ensure the PageModel responsible for creating jobs has 100% unit test coverage.
    *   DoD: 1. Tests cover all code paths in [`CreateModel`](src/Pages/Jobs/Create.cshtml.cs:1), including checks for `ModelState.IsValid` in `OnPost`. 2. Coverage report shows 100%.
*   **Task S4T1-6 (Unit Tests - EditModel Coverage):** Estimate: 1
    *   User Story: As a developer, I want to ensure the PageModel responsible for editing jobs has 100% unit test coverage.
    *   DoD: 1. Tests cover all code paths in [`EditModel`](src/Pages/Jobs/Edit.cshtml.cs:1), including checks for `ModelState.IsValid` in `OnPost`. 2. Coverage report shows 100%.
*   **Task S4T1-7 (Code Comments - Models & Services):** Estimate: 1
    *   User Story: As a developer maintaining the codebase, I want clear comments in the Models and Services layers.
    *   DoD: 1. Relevant `.cs` files contain standard comments. 2. Comments accurately reflect code purpose.
*   **Task S4T1-8 (Code Comments - Core Pages):** Estimate: 1
    *   User Story: As a developer maintaining the codebase, I want clear comments in the core PageModels (Index, Privacy, Error, Dashboard).
    *   DoD: 1. Relevant `.cs` files contain standard comments. 2. Comments accurately reflect code purpose.
*   **Task S4T1-9 (Bug Fix - Server-Side Required Field Validation):** Estimate: 1
    *   User Story: As a developer, I need to ensure job postings cannot be created/updated with empty required fields server-side.
    *   DoD: 1. `OnPost` handlers check `ModelState.IsValid`. 2. Submitting invalid data programmatically fails gracefully.
*   **Task S4T1-10 (Bug Fix - UI Spacing for Location):** Estimate: 1
    *   User Story: As a user viewing job listings, I want appropriate spacing between the location icon and the location text.
    *   DoD: 1. CSS adjusted for space. 2. Spacing is consistent.
*   **Task S4T1-11 (Admin Toggle - Add Button UI):** Estimate: 1
    *   User Story: As a developer, I need to add an "Admin Mode: OFF" button to a prominent location (e.g., site header in [`_Layout.cshtml`](src/Pages/Shared/_Layout.cshtml:1)), so the toggle control is visible.
    *   DoD: 1. A button with initial text "Admin Mode: OFF" (or similar) is present in the chosen location. 2. The button has a unique ID for JavaScript targeting.
*   **Task S4T1-12 (Admin Toggle - Implement State Tracking):** Estimate: 1
    *   User Story: As a developer, I need client-side JavaScript to manage a simple boolean flag indicating if admin mode is currently active, so the application knows the toggle's state.
    *   DoD: 1. A JavaScript variable (e.g., `isAdminModeOn`) is defined and initialized (likely to `false`). 2. This variable's scope allows access by other relevant JS functions.
*   **Task S4T1-13 (Admin Toggle - Implement Toggle Logic):** Estimate: 1
    *   User Story: As a developer, I need JavaScript event handling so that clicking the admin toggle button flips the admin mode state flag and updates the button's text (e.g., to "Admin Mode: ON").
    *   DoD: 1. A click event listener is attached to the admin toggle button. 2. Clicking the button correctly toggles the `isAdminModeOn` flag and updates the button's display text.
*   **Task S4T1-14 (Unit Tests - Admin Toggle JS Logic):** Estimate: 1
    *   User Story: As a developer, I want basic JavaScript unit tests for the admin toggle state management and button text update logic, so the core client-side behavior is verified.
    *   DoD: 1. Basic JS unit tests exist (using a simple framework or assertion approach if feasible) verifying the state flag toggles correctly. 2. Tests verify the button text update logic works as expected (may require mocking DOM elements).
*   **Task S4T1-15 (Unit Tests - JobListing Model):** Estimate: 1 *(New)*
    *   User Story: As a developer, I need to ensure the `JobListing` model has basic unit tests to verify its properties and any validation attributes.
    *   DoD: 1. Basic tests exist in `UnitTests/Models/JobListingTests.cs` to verify property setting and validation attributes. 2. All tests pass.

**Team Member 2: `JobType` in CRUD Forms, Tests, Commenting, Server Validation, Admin Control Visibility, Header Page Tests** (12 Tasks, Est: 12 points)

*   **Task S4T2-1 (Create Form - Add `JobType` Input):** Estimate: 1
    *   User Story: As an admin managing jobs, I want a dropdown/select input field for `JobType` on the Create Job form, so I can select the appropriate category.
    *   DoD: 1. [`Pages/Jobs/Create.cshtml`](src/Pages/Jobs/Create.cshtml:1) includes `<select>` for `NewJob.JobType`. 2. `CreateModel` supports binding.
*   **Task S4T2-2 (Create Service - Handle `JobType`):** Estimate: 1
    *   User Story: As a developer, I need the job service's "add job" method to correctly persist the `JobType` value.
    *   DoD: 1. [`AddJob`](src/Services/JsonFileJobService.cs:96) saves `JobType`. 2. Retrieved new job shows correct `JobType`.
*   **Task S4T2-3 (Edit Form - Add `JobType` Input):** Estimate: 1
    *   User Story: As an admin editing a job, I want the `JobType` dropdown/select input field on the Edit Job form pre-filled.
    *   DoD: 1. [`Pages/Jobs/Edit.cshtml`](src/Pages/Jobs/Edit.cshtml:1) includes `<select>` for `JobToUpdate.JobType`. 2. Dropdown shows existing `JobType`.
*   **Task S4T2-4 (Update Service - Handle `JobType`):** Estimate: 1
    *   User Story: As a developer, I need the job service's "update job" method to correctly update and persist the `JobType` value.
    *   DoD: 1. [`UpdateJob`](src/Services/JsonFileJobService.cs:169) saves changed `JobType`. 2. Retrieved edited job shows updated `JobType`.
*   **Task S4T2-5 (Delete View - Display `JobType`):** Estimate: 1
    *   User Story: As an admin confirming a deletion, I want to see the `JobType` displayed on the Delete confirmation page.
    *   DoD: 1. [`Pages/Jobs/Delete.cshtml`](src/Pages/Jobs/Delete.cshtml:1) displays `JobType`. 2. `DeleteModel` provides `JobType`.
*   **Task S4T2-6 (Unit Tests - DeleteModel Coverage):** Estimate: 1
    *   User Story: As a developer, I want to ensure the PageModel responsible for deleting jobs has 100% unit test coverage.
    *   DoD: 1. Tests cover all code paths in [`DeleteModel`](src/Pages/Jobs/Delete.cshtml.cs:1). 2. Coverage report shows 100%.
*   **Task S4T2-7 (Code Comments - Job CRUD Pages):** Estimate: 1
    *   User Story: As a developer maintaining the codebase, I want clear comments in the Job CRUD PageModels.
    *   DoD: 1. Relevant `.cs` files contain standard comments. 2. Comments accurately reflect code purpose.
*   **Task S4T2-8 (Code Comments - Job Index Page & ViewComponent):** Estimate: 1
    *   User Story: As a developer maintaining the codebase, I want clear comments in the Job Index PageModel and the Job Listing ViewComponent.
    *   DoD: 1. Relevant `.cs` files contain standard comments. 2. Comments accurately reflect code purpose.
*   **Task S4T2-9 (Bug Fix - Server-Side URL Validation):** Estimate: 1
    *   User Story: As a developer, I need to implement stricter server-side validation for the Apply URL field.
    *   DoD: 1. `OnPost` handlers include stricter URL validation. 2. Submitting invalid URLs programmatically fails.
*   **Task S4T2-10 (Admin Toggle - Modify Job Index UI):** Estimate: 1
    *   User Story: As a developer, I need to prepare the Job Index page UI so that the Edit and Delete links/buttons for each job can be easily shown or hidden via JavaScript.
    *   DoD: 1. Edit/Delete controls on [`Pages/Jobs/Index.cshtml`](src/Pages/Jobs/Index.cshtml:1) have `admin-control d-none` classes. 2. Controls initially hidden.
*   **Task S4T2-11 (Admin Toggle - Modify Job Details UI - Dashboard):** Estimate: 1
    *   User Story: As a developer, I need to prepare the Job Details pane on the Dashboard so Edit/Delete buttons can be toggled.
    *   DoD: 1. Edit/Delete buttons in Dashboard details pane have `admin-control d-none` classes. 2. Controls initially hidden.
*   **Task S4T2-12 (Admin Toggle - Modify Layout/Nav UI - Dashboard FAB):** Estimate: 1
    *   User Story: As a developer, I need to prepare the main layout/nav so the "Create Job" FAB can be toggled.
    *   DoD: 1. "Create Job" FAB on Dashboard has `admin-control d-none` classes. 2. Control initially hidden.
*   **Task S4T2-13 (Unit Tests - PrivacyModel Coverage):** Estimate: 1 *(New)*
    *   User Story: As a developer, I want to ensure the `PrivacyModel` has 100% unit test coverage, so its basic logic is verified.
    *   DoD: 1. Unit tests cover all code paths in [`PrivacyModel`](src/Pages/Privacy.cshtml.cs:1). 2. Code coverage report shows 100% for [`PrivacyModel`](src/Pages/Privacy.cshtml.cs:1).

**Team Member 3: `JobType` Filtering, Client-Side Validation (Create), Test Commenting, Date Bug Fix, Search Limit, Simple Page Tests** (11 Tasks, Est: 11 points)

*   **Task S4T3-1 (Filtering Service - Add `JobType` Logic):** Estimate: 1
    *   User Story: As a developer, I need to modify the job service's "get jobs" method to filter results by `jobType`.
    *   DoD: 1. [`GetJobs`](src/Services/JsonFileJobService.cs:32) signature updated. 2. Implementation correctly filters.
*   **Task S4T3-2 (Filtering UI - Add `JobType` Dropdown - Dashboard & Index):** Estimate: 1
    *   User Story: As a user searching for jobs, I want a dropdown filter for `JobType` on the job listings pages.
    *   DoD: 1. [`Pages/Jobs/Index.cshtml`](src/Pages/Jobs/Index.cshtml:1) and [`Pages/Dashboard.cshtml`](src/Pages/Dashboard.cshtml:1) include `<select>` for `JobTypeFilter`. 2. Selected value passed as query parameter.
*   **Task S4T3-3 (Client-Side Validation - Create Form Required Fields):** Estimate: 1
    *   User Story: As a user creating a job, I want immediate feedback if required fields (Title, Company, Desc, ApplyUrl, JobType, Location) are empty.
    *   DoD: 1. `[Required]` annotations added to model for these fields. 2. Client-side messages appear on Create page.
*   **Task S4T3-4 (Client-Side Validation - Create Form URL):** Estimate: 1
    *   User Story: As a user creating a job, I want immediate feedback if the Apply URL is invalid.
    *   DoD: 1. `[Url]` annotation on `ApplyUrl` in model. 2. Client-side message appears on Create page for format.
*   **Task S4T3-5 (Unit Tests - DashboardModel Coverage):** Estimate: 1
    *   User Story: As a developer, I want to ensure the PageModel for the dashboard has 100% unit test coverage.
    *   DoD: 1. Tests cover all code paths in [`DashboardModel`](src/Pages/Dashboard.cshtml.cs:1). 2. Coverage report shows 100%.
*   **Task S4T3-6 (Unit Tests - JobListing Model):** Estimate: 1
    *   User Story: As a developer, I want unit tests for the job data model verifying all validation Data Annotations (`[Required]`, `[Url]`).
    *   DoD: 1. Tests in [`JobListingTests.cs`](UnitTests/Models/JobListingTests.cs:1) verify all validation attributes. 2. All model tests pass.
*   **Task S4T3-7 (Code Comments - Core Test Files):** Estimate: 1
    *   User Story: As a developer maintaining the tests, I want clear comments in the core test files (Model tests, Service tests).
    *   DoD: 1. Relevant test `.cs` files contain standard comments. 2. Comments accurately reflect test purpose.
*   **Task S4T3-8 (Code Comments - PageModel Test Files):** Estimate: 1
    *   User Story: As a developer maintaining the tests, I want clear comments in the PageModel test files.
    *   DoD: 1. Relevant test `.cs` files contain standard comments. 2. Comments accurately reflect test purpose.
*   **Task S4T3-9 (Bug Fix - Edit Resets DatePosted):** Estimate: 1
    *   User Story: As a developer, I need to fix the job update logic so editing does not reset `DatePosted`.
    *   DoD: 1. [`UpdateJob`](src/Services/JsonFileJobService.cs:169) preserves `DatePosted`. 2. Unit test verifies `DatePosted` unchanged after update.
*   **Task S4T3-10 (UI - Limit Search Input Length):** Estimate: 1
    *   User Story: As a user searching for jobs, I want the search keyword input field limited to 50 characters.
    *   DoD: 1. Search `<input>` has `maxlength="50"`. 2. Browser prevents typing > 50 chars.
*   **Task S4T3-11 (Unit Tests - ErrorModel Coverage):** Estimate: 1
    *   User Story: As a developer, I want to ensure the `ErrorModel` has 100% unit test coverage, so its basic logic is verified.
    *   DoD: 1. Unit tests cover all code paths in [`ErrorModel`](src/Pages/Error.cshtml.cs:1). 2. Coverage report shows 100% for [`ErrorModel`](src/Pages/Error.cshtml.cs:1).

**Team Member 4: Client-Side Validation (Edit), Core/Component Tests & Commenting, Date/Search Bug Fixes, Simple Page Tests** (11 Tasks, Est: 11 points)

*   **Task S4T4-1 (Client-Side Validation - Edit Form Required Fields):** Estimate: 1
    *   User Story: As a user editing a job, I want immediate feedback if required fields (Title, Company, Desc, ApplyUrl, JobType, Location) are cleared.
    *   DoD: 1. `[Required]` validation active on Edit page for all required fields. 2. Error messages appear if fields cleared.
*   **Task S4T4-2 (Client-Side Validation - Edit Form URL):** Estimate: 1
    *   User Story: As a user editing a job, I want immediate feedback if the Apply URL format is invalid.
    *   DoD: 1. `[Url]` validation active on Edit page for `ApplyUrl`. 2. Error message appears if URL format invalid.
*   **Task S4T4-3 (Unit Tests - `JsonFileJobService` Coverage):** Estimate: 1
    *   User Story: As a developer, I want to expand unit tests for the job data service to achieve 100% coverage, including `JobType` handling/filtering.
    *   DoD: 1. Tests cover all code paths in [`JsonFileJobService.cs`](src/Services/JsonFileJobService.cs:1). 2. Coverage report shows 100%.
*   **Task S4T4-4 (Unit Tests - IndexModel Coverage):** Estimate: 1
    *   User Story: As a developer, I want to ensure the PageModel for the main job listing page has 100% unit test coverage, including `JobType` filtering logic.
    *   DoD: 1. Tests cover all code paths in [`IndexModel`](src/Pages/Jobs/Index.cshtml.cs:1). 2. Coverage report shows 100%.
*   **Task S4T4-5 (Unit Tests - DetailsModel Coverage):** Estimate: 1
    *   User Story: As a developer, I want to ensure the PageModel for displaying job details has 100% unit test coverage.
    *   DoD: 1. Tests cover all code paths in [`DetailsModel`](src/Pages/Jobs/Details.cshtml.cs:1). 2. Coverage report shows 100%.
*   **Task S4T4-6 (Unit Tests - JobListingViewComponent Coverage):** Estimate: 1
    *   User Story: As a developer, I want to ensure the ViewComponent used for displaying job listings has 100% unit test coverage.
    *   DoD: 1. Tests cover all code paths in [`JobListingViewComponent`](src/ViewComponents/JobListingViewComponent.cs:1). 2. Coverage report shows 100%.
*   **Task S4T4-7 (Code Comments - Startup/Program Files):** Estimate: 1
    *   User Story: As a developer maintaining the application setup, I want clear comments in the main application setup files.
    *   DoD: 1. [`Program.cs`](src/Program.cs:1)/[`Startup.cs`](src/Startup.cs:1) contain standard comments. 2. Comments accurately reflect code purpose.
*   **Task S4T4-8 (Code Comments - ViewComponent Test Files):** Estimate: 1
    *   User Story: As a developer maintaining the tests, I want clear comments in the ViewComponent test files.
    *   DoD: 1. ViewComponent test `.cs` files contain standard comments. 2. Comments accurately reflect test purpose.
*   **Task S4T4-9 (Bug Fix - Add Job Date Issue):** Estimate: 1
    *   User Story: As a developer, I need to investigate and fix the logic for setting `DatePosted` when adding a new job.
    *   DoD: 1. [`AddJob`](src/Services/JsonFileJobService.cs:96) correctly assigns `DatePosted`. 2. Unit test verifies `DatePosted` is accurate.
*   **Task S4T4-10 (Bug Fix - Search Logic):** Estimate: 1
    *   User Story: As a user searching for jobs, I expect the keyword search to only return relevant results.
    *   DoD: 1. Search logic in [`GetJobs`](src/Services/JsonFileJobService.cs:32) is corrected. 2. Unit tests verify only relevant results returned.
*   **Task S4T4-11 (Unit Tests - PrivacyModel Coverage):** Estimate: 1
    *   User Story: As a developer, I want to ensure the `PrivacyModel` has 100% unit test coverage, so its basic logic is verified.
    *   DoD: 1. Unit tests cover all code paths in [`PrivacyModel`](src/Pages/Privacy.cshtml.cs:1). 2. Coverage report shows 100% for [`PrivacyModel`](src/Pages/Privacy.cshtml.cs:1).

---
**Total Current Sprint Tasks: 46**
**Total Current Sprint Estimates: 46 points**
```