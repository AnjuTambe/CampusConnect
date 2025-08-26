# CampusConnect - Sprint 6 Plan (Granular Breakdown - INVEST & 1-Point Estimates)

## Sprint 6 Goal

Implement comprehensive Employer management functionality (create, edit, delete) with admin controls, enhance UI aesthetics focusing on the index page and filter panels, and relocate admin controls from the header to their respective filter panels for improved usability and a more intuitive interface.

## Task Board Simulation

### Backlog (Selected High-Level Goals for Future Sprints / Refinements)
1. **Epic:** Advanced Search Capabilities (e.g., multi-select filters, date range)
2. **Epic:** Company Landing Pages with Company-specific Analytics 
3. **Feature:** Graph visualizations for job trends by industry/location
4. **Feature:** Job application tracking dashboard
5. **Feature:** Mobile-responsive design optimization
6. **Feature:** Accessibility Audit & Improvements (WCAG Compliance)
7. **Feature:** Performance Optimization for Large Datasets
8. **Task:** Add "Date Posted" sorting option to job listings
9. **Task:** Implement pagination for employer listings
10. **Task:** Add bulk import/export functionality for jobs and employers

### Current Sprint (20 Tasks)

*(Tasks distributed among 4 Team Members)*

**Team Member 1 (5 Tasks, Est: 5 points)**

* **Task S6.1 (UI: Create Employer Directory Structure):**
    * As a developer, I want to set up the Employers directory and basic file structure so we have a foundation for employer management features.
    * Estimate: 1
    * DoD:
        1. The `Pages/Employers` directory is created.
        2. Basic file structure for Create, Edit, and Delete pages is established.

* **Task S6.2 (UI: Design Employer Create Form):**
    * As a user, I want a well-designed form to enter new employer information so I can easily add employers to the system.
    * Estimate: 1
    * DoD:
        1. The `Create.cshtml` form has proper layout and styling consistent with the site design.
        2. Form fields include appropriate labels, placeholders and validation messages.

* **Task S6.3 (UI: Implement Employer Name Input):**
    * As a user, I want to enter an employer name when creating a new employer so the employer can be uniquely identified in the system.
    * Estimate: 1
    * DoD:
        1. The Employer Name field is implemented with required validation.
        2. The field has appropriate styling and error messages.

* **Task S6.4 (UI: Implement Company Vision Input):**
    * As a user, I want to enter a company vision statement when creating a new employer so students can understand the company's goals and values.
    * Estimate: 1
    * DoD:
        1. The Company Vision field is implemented as a multi-line text area.
        2. The field has appropriate styling and validation.

* **Task S6.5 (UI: Implement Tech Stack Input):**
    * As a user, I want to enter technology stack details when creating a new employer so students can understand what technologies the company uses.
    * Estimate: 1
    * DoD:
        1. The Tech Stack field is implemented with appropriate validation.
        2. The field has appropriate styling and error messages.

**Team Member 2 (5 Tasks, Est: 5 points)**

* **Task S6.6 (PageModel: Initialize Employer Create Model):**
    * As a developer, I want to initialize the Create PageModel with proper dependencies and properties so it's ready to handle user requests.
    * Estimate: 1
    * DoD:
        1. `Create.cshtml.cs` file includes proper constructor with dependency injection.
        2. The model includes necessary properties and initialization in OnGet.

* **Task S6.7 (PageModel: Implement Form Submission Handler):**
    * As a developer, I want to implement form submission handling in the Create PageModel so user-submitted data can be processed.
    * Estimate: 1
    * DoD:
        1. OnPost method is implemented to handle form submission.
        2. Method correctly validates input before proceeding.

* **Task S6.8 (PageModel: Implement Employer Creation Logic):**
    * As a user, I want my employer creation requests to be properly processed and saved so new employers are added to the system.
    * Estimate: 1
    * DoD:
        1. OnPost successfully calls the employer service to add new employers.
        2. Success or failure status is properly communicated back to the user.

* **Task S6.9 (UI: Create Admin Visibility Controls):**
    * As an admin, I want Create Employer functionality to only be visible when admin mode is enabled so normal users can't modify employer data.
    * Estimate: 1
    * DoD:
        1. Create controls are only displayed when admin mode is active.
        2. Non-admin users are redirected if they try to access the page directly.

* **Task S6.10 (UI: Design Employer Edit Form):**
    * As a user, I want a well-designed form to modify existing employer information so I can keep employer details current.
    * Estimate: 1
    * DoD:
        1. The `Edit.cshtml` form has proper layout and styling consistent with the site design.
        2. Form fields are pre-populated with existing employer data.

**Team Member 3 (5 Tasks, Est: 5 points)**

* **Task S6.11 (PageModel: Initialize Employer Edit Model):**
    * As a developer, I want to initialize the Edit PageModel with proper dependencies and data loading so it's ready to show and modify employer data.
    * Estimate: 1
    * DoD:
        1. `Edit.cshtml.cs` file includes proper constructor with dependency injection.
        2. The model includes an OnGet method that loads the existing employer data by ID.

* **Task S6.12 (PageModel: Implement Employer Update Logic):**
    * As a user, I want my employer update requests to be properly processed and saved so employer information stays current.
    * Estimate: 1
    * DoD:
        1. OnPost successfully calls the employer service to update employers.
        2. Success or failure status is properly communicated back to the user.

* **Task S6.13 (UI: Design Employer Delete Confirmation):**
    * As a user, I want a clear confirmation page before deleting an employer so I can prevent accidental deletions.
    * Estimate: 1
    * DoD:
        1. The `Delete.cshtml` page clearly shows which employer will be deleted.
        2. The page includes a prominent warning about the consequences of deletion.

* **Task S6.14 (PageModel: Implement Employer Delete Logic):**
    * As a user, I want my employer deletion requests to be properly processed so outdated employers can be removed from the system.
    * Estimate: 1
    * DoD:
        1. OnPost successfully calls the employer service to delete employers.
        2. Success or failure status is properly communicated back to the user.

* **Task S6.15 (UI: Update Index Page Hero Section):**
    * As a user, I want an engaging hero section on the homepage so I can quickly understand the site's purpose.
    * Estimate: 1
    * DoD:
        1. The hero section includes an attention-grabbing headline and concise description.
        2. The section has visually appealing styling and responsive design.

**Team Member 4 (5 Tasks, Est: 5 points)**

* **Task S6.16 (UI: Update Index Page Feature Sections):**
    * As a user, I want organized feature sections on the homepage so I can easily understand what the site offers.
    * Estimate: 1
    * DoD:
        1. Feature sections have clear headings and concise descriptions.
        2. Sections use consistent visual styling and organization.

* **Task S6.17 (UI: Add Visual Elements to Index Page):**
    * As a user, I want engaging visual elements on the homepage so the site feels professional and modern.
    * Estimate: 1
    * DoD:
        1. The page includes appropriate icons, images, or illustrations.
        2. Visual elements are responsive and enhance the user experience.

* **Task S6.18 (UI: Redesign Jobs Filter Panel):**
    * As a user, I want an intuitive and visually appealing job filter panel so I can easily find relevant jobs.
    * Estimate: 1
    * DoD:
        1. The filter panel has improved visual hierarchy and organization.
        2. Filter controls have consistent styling and clear labels.

* **Task S6.19 (UI: Redesign Employers Filter Panel):**
    * As a user, I want an intuitive and visually appealing employer filter panel so I can easily find relevant employers.
    * Estimate: 1
    * DoD:
        1. The employer filter panel matches the improved design of the job filter panel.
        2. Filter controls have consistent styling and clear labels.

* **Task S6.20 (UI: Relocate Admin Controls to Filter Panels):**
    * As an admin, I want admin controls integrated into their relevant contexts so I can more intuitively manage the system.
    * Estimate: 1
    * DoD:
        1. Admin toggle button is relocated from the header to respective filter panels.
        2. Admin buttons are clearly labeled and only visible when admin mode is enabled.

## Detailed Implementation Requirements

### Employer Management Features
1. Create, Edit, and Delete functionality should mirror the existing Jobs functionality
2. Admin toggle should control the visibility of Create/Edit/Delete buttons
3. All forms should include proper validation with user-friendly error messages
4. Success/failure messages should be displayed to the user after operations

### UI Enhancements
1. Index page should use modern design principles:
   - Clear visual hierarchy
   - Appropriate white space
   - Consistent color scheme
   - Professional typography
   - Engaging visual elements
2. Filter panels should:
   - Have clear section headers
   - Use appropriate spacing between controls
   - Include visual feedback for active filters
   - Have a clean, organized layout
3. Admin controls should:
   - Be contextually placed in their respective filter panels
   - Have consistent visual styling
   - Include clear labeling of their purpose

### All implementations must follow:
1. Existing coding standards and patterns
2. Proper error handling
3. Complete unit test coverage
4. Accessibility best practices
5. Responsive design principles

### Atomic Commit Strategy
1. Each task should be implemented in a separate branch
2. Commits should be small and focused on specific changes
3. Commit messages should reference the task ID (e.g., "S6.3: Implement Employer Name Input")
4. Pull requests should be created for each completed task
5. Tasks should be implemented in logical order respecting dependencies

---

**Total Current Sprint Tasks: 20**
**Total Current Sprint Estimates: 20 points**