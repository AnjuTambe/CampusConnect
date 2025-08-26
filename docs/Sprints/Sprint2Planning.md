# CampusConnect - Sprint 2 Plan

## Sprint 2 Goal

Implement the "Details" page for job listings, allowing users to view more information about a specific job, add basic search functionality (including location filtering) to the Job Listings (Index) page, and begin basic UI styling enhancements. Ensure core functionality is covered by unit tests and UI elements are well-proportioned.

## Task Board Simulation

### Backlog (Ordered - Top Priority First)

1.  **Card: Company Profiles Page**

    *   **User Story:** As a student, I want to view a list of company profiles, so that I can learn about potential employers.

    *   **Estimate:** 5 points (Medium Epic)

    *   **Acceptance Criteria (DoD):** New page exists; Basic company data displayed; Navigation link exists.

2.  **Card: Link Jobs to Company Profiles**

    *   **User Story:** As a student viewing a job listing, I want to click on the company name to see its profile, so that I can get more context about the employer.

    *   **Estimate:** 3 points (Small Epic)

    *   **Acceptance Criteria (DoD):** Company name links work; Requires Company Profiles page; Requires job-company association.

3.  **Card: Display Company Logos on Job Listings**

    *   **User Story:** As a student browsing job listings, I want to see company logos next to the job titles, so that I can quickly identify employers visually.

    *   **Estimate:** 2 points

    *   **Acceptance Criteria (DoD):** `CompanyLogoUrl` property added; Data updated; Logos displayed on Index page; Placeholder handled.

4.  **Card: Filter Jobs by Type (Internship, Full-time)**

    *   **User Story:** As a student searching for jobs, I want to filter the job list by type, so that I can find relevant opportunities.

    *   **Estimate:** 3 points

    *   **Acceptance Criteria (DoD):** `JobType` property added; Data updated; UI filter elements added; Service/PageModel updated for filtering.

5.  **Card: Student User Profiles (Basic)**

    *   **User Story:** As a student, I want to create a basic profile, so that I can start building my presence.

    *   **Estimate:** 8 points (Large Epic)

    *   **Acceptance Criteria (DoD):** Basic auth/registration; `StudentProfile` model; Profile view/edit page; Data storage implemented.

6.  **Card: Add "Date Posted" to Job Listings**

    *   **User Story:** As a student browsing jobs, I want to see the date each job was posted, so that I know how recent it is.

    *   **Estimate:** 1 point

    *   **Acceptance Criteria (DoD):** `DatePosted` property added; Data updated; Date displayed on Index/Details pages.

7.  **Card: Implement "Create Job Listing" functionality (Admin/Employer)**

    *   **User Story:** As an admin/employer, I want a form to create new job listings, so that I can add opportunities.

    *   **Estimate:** 5 points (Medium Epic)

    *   **Acceptance Criteria (DoD):** Requires roles/auth; Create Job page/form; Service layer updated; Input validation.

8.  **Card: Implement "Update Job Listing" functionality (Admin/Employer)**

    *   **User Story:** As an admin/employer, I want to edit existing job listings, so that I can update information.

    *   **Estimate:** 5 points (Medium Epic)

    *   **Acceptance Criteria (DoD):** Requires roles/auth; Edit Job page/form; Service layer updated; Input validation.

9.  **Card: Implement "Delete Job Listing" functionality (Admin/Employer)**

    *   **User Story:** As an admin/employer, I want to delete job listings, so that I can remove outdated positions.

    *   **Estimate:** 3 points

    *   **Acceptance Criteria (DoD):** Requires roles/auth; Deletion mechanism; Confirmation step; Service layer updated.

### Current Sprint (Tasks to be pulled into Active)

*   **Task 9 (Details):** *[Completed]* Add `GetJobById` signature to `IJobService`.

*   **Task 10 (Details):** *[Completed]* Implement `GetJobById` in `JsonFileJobService`.

*   **Task 11 (Search):** *[Completed]* Add search input form to `Index.cshtml`.

*   **Task 12 (Search):** *[Completed]* Modify service signatures for search term.

*   **Task 17 (UI):** *[Completed]* Update fonts, colors, header/footer, home page image/layout.

*   **Task 18 (Unit Tests - Service):**

    *   **User Story:** As a developer, I want unit tests for the `JsonFileJobService` covering the `GetJobById` and `GetJobs` (with search/filter) methods, so that the core data retrieval and filtering logic is verified.

    *   **Estimate:** 2 points

    *   **Acceptance Criteria (DoD):** Tests added/updated; `GetJobById` covers found/not found; `GetJobs` covers search/filter cases; Mock data used; Tests pass.

*   **Task 19 (Unit Tests - PageModels):**

    *   **User Story:** As a developer, I want unit tests for the `DetailsModel` and `IndexModel` (search part) PageModels, so that their interaction with the service and property handling is verified.

    *   **Estimate:** 2 points

    *   **Acceptance Criteria (DoD):** Tests added/updated; `IJobService` mocked; `DetailsModel` tests cover `OnGet` logic; `IndexModel` tests verify `SearchTerm` binding and service calls; Tests pass.

*   **Task 20 (UI - Search Form):**

    *   **User Story:** As a user, I want the location filter dropdown in the search bar to be wider, so that longer location names are fully visible.

    *   **Estimate:** 0.5 points

    *   **Acceptance Criteria (DoD):**

        *   Width of the `<select name="LocationFilter">` element in `Pages/Jobs/Index.cshtml` is increased (e.g., by adjusting Bootstrap column classes or adding custom CSS).

        *   Longer options like "New York, NY" are fully visible within the dropdown.

        *   Overall search bar layout remains visually balanced.

### Active Tasks (1 per Team Member)

*   **Task 13 (Details - Team Member 1):** *[Completed]* Create `Details.cshtml.cs` PageModel.

*   **Task 14 (Details - Team Member 2):** *[Completed]* Create `Details.cshtml` Razor view.

*   **Task 15 (Details - Team Member 3):** *[Completed]* Link job titles on Index page to Details page.

*   **Task 16 (Search - Team Member 4):** *[Completed]* Update Index PageModel for search term.