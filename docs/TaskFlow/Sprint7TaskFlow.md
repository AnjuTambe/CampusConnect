# Sprint 7 Task Flow

This document outlines a suggested order of task implementation for Sprint 7. While some tasks can be parallelized, this flow respects key dependencies to ensure a smoother development process.

## I. Core Navigation Logic & Basic "Clear Filter" Button UI

These tasks establish fundamental page navigation for employer management and the initial UI for the new clear filter button.

1.  **Task S7.1:** Employer Edit Save Redirect
2.  **Task S7.2:** Employer Edit Cancel Redirect
3.  **Task S7.3:** Employer Delete Confirm Redirect
4.  **Task S7.4:** Employer Delete Cancel Redirect
5.  **Task S7.5:** UI for "Clear Filter" button in Employer filter panel

## II. "Clear Filter" Functionality & Initial Filter Panel Refinements

Focus shifts to implementing the logic for the "Clear Filter" buttons and starting UI consistency work on filter panels.

6.  **Task S7.6:** Functionality: Clear Employer Keyword Filter (depends on S7.5)
7.  **Task S7.7:** Functionality: Clear Employer Location Filter (depends on S7.5)
8.  **Task S7.17:** Full Functionality: "Clear Filter" for Jobs Panel (can be parallel with S7.6, S7.7)
9.  **Task S7.18:** Full Functionality: "Clear Filter" for Employers Panel (builds upon S7.6, S7.7; ensures S7.5 UI is functional)
10. **Task S7.8:** UI: Standardize Spacing in Jobs Filter Panel
11. **Task S7.9:** UI: Standardize Spacing in Employers Filter Panel (aim for consistency with S7.8)
12. **Task S7.10:** UI: Consistent Admin Mode Button Positioning (best done after S7.8 & S7.9 structures are refined)

## III. Index Page UI Enhancements

Small but impactful UI tweaks on the homepage.

13. **Task S7.11:** UI: Spacing for "Explore Opportunities" on Index
14. **Task S7.12:** UI: Spacing for "Learn More" on Index (ensure consistency with S7.11)

## IV. Dashboard Visual Block Styling

Implementing the distinct visual blocks for different sections of the dashboard page as per the UI mockups. These can often be worked on in parallel once the basic dashboard structure is stable.

15. **Task S7.13:** UI: Style Dashboard Left Navigation Panel
16. **Task S7.14:** UI: Style Dashboard Main Filter Panel Area
17. **Task S7.15:** UI: Style Dashboard Main List Area
18. **Task S7.16:** UI: Style Dashboard Details Area

## V. Filter Robustness Testing

Ensuring the "Clear Filter" functionality is robust across multiple filter selections.

19. **Task S7.19:** Test: "Clear Filter" with multiple Job filters (depends on S7.17)
20. **Task S7.20:** Test: "Clear Filter" with multiple Employer filters (depends on S7.18)

## VI. Card Animations and Consistency

Bringing consistent animations and hover effects to the Employer cards.

21. **Task S7.21:** CSS: Define Employer Card Hover Effects (to match Job cards)
22. **Task S7.22:** UI: Apply Employer Card Hover Effects (depends on S7.21)
23. **Task S7.23:** Animation: Employer Card Entry/Transition Animations (to match Job cards)

## VII. Navigation Element Animations

Final touches to make dashboard navigation more interactive and consistent with card styles.

24. **Task S7.24:** Animation: "Jobs" Nav Item Hover Effect
25. **Task S7.25:** Animation: "Employers" Nav Item Hover Effect (ensure consistency with S7.24) 