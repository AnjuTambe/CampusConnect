# Sprint 5 Task Flow (Updated)

This document outlines a suggested task flow for Sprint 5, showing dependencies between tasks based on the updated Sprint 5 plan. Tasks on the same line or grouped without explicit arrows can often be worked on in parallel.

## Core 'EmployerName' Implementation (Model, Data, Service)

- Task S5.1 (Model: Add `EmployerName` Property)
- Task S5.2 (Model: Add `[Required]` to `EmployerName`) (depends on S5.1)
- Task S5.7 (UT Model: `EmployerName` `[Required]` Validation) (depends on S5.1, S5.2)
- Task S5.49 (Commenting - `EmployerName` in Model) (depends on S5.2)

- Task S5.3 (Data: Update `jobs.json` with `employer_name`)

- Task S5.4 (Service: `AddJob` Persist `EmployerName`) (depends on S5.1)
- Task S5.8 (UT Service: `AddJob` for `EmployerName`) (depends on S5.4)

- Task S5.5 (Service: `UpdateJob` Persist `EmployerName`) (depends on S5.1)
- Task S5.9 (UT Service: `UpdateJob` for `EmployerName`) (depends on S5.5)

- Task S5.6 (Service: `EmployerName` Filter in `GetJobs`) (depends on S5.1)
- Task S5.10 (UT Service: `GetJobs` Filtering by `EmployerName`) (depends on S5.6)
- Task S5.50 (Commenting - `EmployerName` in Service) (depends on S5.6)

## Basic PageModel and Test Creation (Can be done in parallel with Core Implementation)

- Task S5.11 (Create AboutUs.cshtml.cs)
- Task S5.12 (Create About_Us_Page_Tests.cshtml.cs) (depends on S5.11)
- Task S5.13 (UT AboutUsModel Basic Coverage) (depends on S5.11, S5.12)

- Task S5.25 (Create Blog.cshtml.cs)
- Task S5.26 (Create Blog_Page_Tests.cshtml.cs) (depends on S5.25)
- Task S5.27 (UT BlogModel Basic Coverage) (depends on S5.25, S5.26)

- Task S5.39 (Create CareerTips.cshtml.cs)
- Task S5.40 (Create Career_Tips_Page_Tests.cshtml.cs) (depends on S5.39)
- Task S5.41 (UT CareerTipsModel Basic Coverage) (depends on S5.39, S5.40)

- Task S5.54 (Create HowItWorks.cshtml.cs)
- Task S5.55 (Create How_It_Works_Page_Tests.cshtml.cs) (depends on S5.54)
- Task S5.56 (UT HowItWorksModel Basic Coverage) (depends on S5.54, S5.55)

## Core 'EmployerName' UI and PageModel (Depends on Core Implementation)

- Task S5.15 (UI Create Form: Add `EmployerName` Input) (depends on S5.1)
- Task S5.17 (UT PageModels: Server-Side `[Required]` Validation Create) (depends on S5.15)
- Task S5.51 (Commenting - `EmployerName` in Create PageModel) (depends on S5.17)

- Task S5.16 (UI Edit Form: Add `EmployerName` Input) (depends on S5.1)
- Task S5.18 (UT PageModels: Server-Side `[Required]` Validation Edit) (depends on S5.16)

- Task S5.19 (UI Display: `EmployerName` in Job List ViewComponent) (depends on S5.1)
- Task S5.20 (UI Display: `EmployerName` in Dashboard Details) (depends on S5.1)
- Task S5.21 (UI Display: `EmployerName` in Delete View) (depends on S5.1)

- Task S5.22 (Dashboard PageModel: Populate `EmployerFilterOptions`) (depends on S5.6)
- Task S5.23 (UI: Add `EmployerName` Filter Dropdown) (depends on S5.22)
- Task S5.24 (Dashboard PageModel: `EmployerFilter` Property & Handler Updates) (depends on S5.6)

## New Employer Data and Service

- Task S5.58 (Data: Create `employer.json`)
- Task S5.59 (Model: Create `Employer.cs`) (depends on S5.58)
- Task S5.60 (Model: Add `[Required]` to Employer Model properties) (depends on S5.59)
- Task S5.61 (Service: Create `IEmployerService.cs`)
- Task S5.62 (Service: Create `JsonFileEmployerService.cs`) (depends on S5.59, S5.61)
- Task S5.63 (UT Model: Employer Model Validation) (depends on S5.60)
- Task S5.64 (UT Service: `GetEmployers`) (depends on S5.62)

## Updated Dashboard 'Employers' Section (Depends on New Employer Data and Service, and Core Implementation)

- Task S5.29 (UI: Add "Employers" Navigation Link)
- Task S5.30 (Dashboard PageModel: "Employers" Tab Handler & View State)
- Task S5.31 (Dashboard PageModel: Populate Employer Data) (depends on S5.62)
- Task S5.32 (UI: Display Employer List in Employers View Left Panel) (depends on S5.31)
- Task S5.33 (UI: Add Employer Search Input in Employers View)
- Task S5.34 (Dashboard PageModel: Implement Employer Search Filtering Logic) (depends on S5.31)
- Task S5.65 (Dashboard PageModel: Fetch Employer Data) (depends on S5.62)
- Task S5.66 (Dashboard UI: Display Employer Details) (depends on S5.65, S5.32)
- Task S5.67 (Dashboard UI Logic: Select Employer) (depends on S5.65, S5.66)
- Task S5.68 (UT Dashboard PageModel: Fetch Employer Data) (depends on S5.65)
- Task S5.69 (Dashboard UI: Conditional Filter Panel) (depends on S5.30)
- Task S5.45 (UT Dashboard PageModel: Employer Data Population) (depends on S5.31)
- Task S5.46 (UT Dashboard PageModel: Employer Search Filtering) (depends on S5.34)

## General Refactoring and Standards (Can be done throughout, some depend on files/tests existing)

- Task S5.42 (Refactor Code Standard - Avoid Negation)

- Task S5.14 (Refactor Test Case Method Naming) (depends on completion of most UT tasks)
- Task S5.28 (Refactor UT Structure - Arrange/Act/Assert) (depends on completion of most UT tasks)
- Task S5.53 (Refactoring - Comprehensive Comment Review & Formatting) (depends on completion of most C# implementation tasks)
- Task S5.57 (Refactor Naming Conventions & 1 Var Per Line) (depends on completion of most implementation tasks and test file creation tasks)
- Task S5.24 (Commenting - ViewComponent for `EmployerName` Display) (This task number seems incorrect based on the plan, assuming it was meant to be a commenting task related to the ViewComponent display)

---
**Total Current Sprint Tasks: 59**
**Total Current Sprint Estimates: 59 points**