using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using CampusConnect.WebSite.Services; // Required for IJobService
using CampusConnect.WebSite.Models;   // Required for JobListing in the handler
using Microsoft.AspNetCore.Mvc.Rendering; // Required for SelectListItem
using System.Linq; // Required for LINQ methods like Select, Distinct, OrderBy
using System.Collections.Generic; // Required for List<>
using System; // Required for StringComparison

namespace CampusConnect.WebSite.Pages
{
    /// <summary>
    /// PageModel for the Dashboard page, displaying job listings and details.
    /// </summary>
    public class DashboardModel : PageModel
    {
        // Logger for the DashboardModel.
        private readonly ILogger<DashboardModel> _logger;
        // Service to interact with job listing data.
        private readonly IJobService _jobService;
        // Service to interact with employer data.
        private readonly IEmployerService _employerService;

        /// <summary>
        /// Gets or sets the active view state of the dashboard (e.g., "Jobs", "Employers").
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string ActiveView { get; set; } = "Jobs"; // Default to "Jobs" view

        // Property to bind the search term from the query string or form.
        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        // Property to bind the location filter from the query string or form.
        [BindProperty(SupportsGet = true)]
        public string? LocationFilter { get; set; }

        // Property to bind the current page number for pagination.
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        // Property to bind the selected job type filter from the query string or form.
        [BindProperty(SupportsGet = true)]
        public string? JobTypeFilter { get; set; } // New: JobType selected from dropdown

        // Property to bind the selected employer name filter from the query string or form.
        [BindProperty(SupportsGet = true)]
        public string? EmployerNameFilter { get; set; }

        // Property to bind the employer filter from the query string or form.
        [BindProperty(SupportsGet = true)]
        public string? EmployerFilter { get; set; }

        /// <summary>
        /// Gets or sets the search term for filtering employers.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string? EmployerSearchTerm { get; set; }

        // Property to control visibility of the job list section on the dashboard.
        public bool ShowJobsList { get; private set; } = false;

        // Property to hold the currently selected job for detail view in the right pane.
        public JobListing? SelectedJob { get; private set; }

        /// <summary>
        /// Gets or sets the currently selected employer for detail view in the right pane.
        /// </summary>
        public Employer? SelectedEmployer { get; set; }

        // List of options for the location filter dropdown.
        public List<SelectListItem> LocationOptions { get; set; } = new List<SelectListItem>();

        // List of options for the employer filter dropdown.
        public List<SelectListItem> EmployerFilterOptions { get; set; } = new List<SelectListItem>();

        /// <summary>
        /// Gets or sets a list of distinct employer names for display in the Employers view.
        /// <summary>
        /// Gets or sets a list of distinct employer names for display in the Employers view.
        /// </summary>
        public List<string> DistinctEmployerNames { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the list of Employer objects.
        /// </summary>
        public List<Employer> Employers { get; set; } = new List<Employer>();

        /// <summary>
        /// Initializes a new instance of the DashboardModel.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="jobService">The job service instance.</param>
        /// <param name="employerService">The employer service instance.</param>
        public DashboardModel(ILogger<DashboardModel> logger, IJobService jobService, IEmployerService employerService)
        {
            _logger = logger;
            _jobService = jobService;
            _employerService = employerService;
        }

        /// <summary>
        /// Handles GET requests for the main Dashboard page load.
        /// Populates filter options and determines initial view state.
        /// </summary>
        public void OnGet()
        {
            PopulateLocationOptions();
            PopulateEmployerFilterOptions();
            // Placeholder for any logic needed when the main page loads.
            // The ViewComponent handles its own data fetching based on bound properties.

            // If employerSearchTerm is provided and ActiveView is Employers, call OnGetShowEmployers
            if (!string.IsNullOrEmpty(EmployerSearchTerm) && ActiveView == "Employers")
            {
                _logger.LogInformation($"OnGet redirecting to OnGetShowEmployers with search term: {EmployerSearchTerm}");
                OnGetShowEmployers(null, EmployerSearchTerm);
                return;
            }

            // Only set default view if ActiveView wasn't explicitly provided
            // This allows the form's hidden ActiveView field to be respected
            if (Request?.Query == null || string.IsNullOrEmpty(Request.Query["ActiveView"]))
            {
                // If job-related filter parameters are present, set to Jobs view
                bool hasJobFilters = HasAnyJobFilterParameters();
                if (hasJobFilters)
                {
                    ShowJobsList = true;
                    ActiveView = "Jobs";
                }
                // If employer search term is present, set to Employers view
                else if (!string.IsNullOrEmpty(EmployerSearchTerm))
                {
                    ShowJobsList = false;
                    ActiveView = "Employers";
                }
                else
                {
                    // Default to showing the job list on initial load if no specific handler was called
                    ShowJobsList = true;
                    ActiveView = "Jobs"; // Explicitly set default view
                }
            }

            // Set ShowJobsList based on ActiveView
            ShowJobsList = (ActiveView == "Jobs");

            _logger.LogInformation($"Dashboard OnGet called. ActiveView: '{ActiveView}', Search: '{SearchTerm}', Location: '{LocationFilter}', JobType: '{JobTypeFilter}', EmployerName: '{EmployerNameFilter}', EmployerSearchTerm: '{EmployerSearchTerm}', Page: {CurrentPage}, ShowJobs: {ShowJobsList}");
        } // End of OnGet

        /// <summary>
        /// Handles GET requests specifically to show the jobs list section of the dashboard.
        /// Used when filters or pagination are applied.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="locationFilter">The selected location filter.</param>
        /// <param name="jobTypeFilter">The selected job type filter.</param>
        /// <param name="employerNameFilter">The selected employer name filter.</param>
        /// <param name="currentPage">The current page number.</param>
        public void OnGetShowJobs(string? searchTerm, string? locationFilter, string? jobTypeFilter, string? employerNameFilter, int currentPage = 1)
        {
            PopulateLocationOptions();
            PopulateEmployerFilterOptions();
            _logger.LogInformation($"Dashboard OnGetShowJobs called. Search: '{searchTerm}', Location: '{locationFilter}', JobType: '{jobTypeFilter}', EmployerName: '{employerNameFilter}', Page: {currentPage}");
            ShowJobsList = true;
            ActiveView = "Jobs"; // Set view state to Jobs
            // Preserve search, filter, and page for the ViewComponent
            SearchTerm = searchTerm;
            LocationFilter = locationFilter;
            JobTypeFilter = jobTypeFilter;
            EmployerNameFilter = employerNameFilter;
            CurrentPage = currentPage;
            // The OnGet method will be called again by the framework after this handler,
            // or the page will re-render and the ViewComponent will use these updated properties.
        }

        /// <summary>
        /// Handles GET requests to show details of a specific job in the right pane.
        /// </summary>
        /// <param name="id">The ID of the job to show details for.</param>
        /// <param name="searchTerm">The current search term (to preserve state).</param>
        /// <param name="locationFilter">The current location filter (to preserve state).</param>
        /// <param name="jobTypeFilter">The current job type filter (to preserve state).</param>
        /// <param name="employerNameFilter">The current employer name filter (to preserve state).</param>
        /// <param name="currentPage">The current page number (to preserve state).</param>
        public void OnGetShowJobDetails(string id, string? searchTerm, string? locationFilter, string? jobTypeFilter, string? employerNameFilter, int currentPage = 1)
        {
            PopulateLocationOptions();
            PopulateEmployerFilterOptions();
            _logger.LogInformation($"Dashboard OnGetShowJobDetails called. Job ID: '{id}', Search: '{searchTerm}', Location: '{locationFilter}', JobType: '{jobTypeFilter}', EmployerName: '{employerNameFilter}', Page: {currentPage}");

            if (string.IsNullOrEmpty(id))
            {
                _logger.LogWarning("OnGetShowJobDetails called with null or empty ID.");
                SelectedJob = null;
            }
            else
            {
                SelectedJob = _jobService.GetJobById(id);
                if (SelectedJob == null)
                {
                    _logger.LogWarning($"Job with ID '{id}' not found for OnGetShowJobDetails.");
                }
            }

            ShowJobsList = true; // Keep the job list visible
            ActiveView = "Jobs"; // Keep view state to Jobs when showing job details
            // Preserve search, filter, and page for the ViewComponent and job list state
            SearchTerm = searchTerm;
            LocationFilter = locationFilter;
            JobTypeFilter = jobTypeFilter;
            EmployerNameFilter = employerNameFilter;
            CurrentPage = currentPage;
        }

        /// <summary>
        /// Handles GET requests specifically to show the employers list section of the dashboard.
        /// </summary>
        /// <summary>
        /// Handles GET requests specifically to show the employers list section of the dashboard.
        /// Populates the list of distinct employer names.
        /// </summary>
        public void OnGetShowEmployers(string? employerId = null, string? employerSearchTerm = null)
        {
            _logger.LogInformation($"Dashboard OnGetShowEmployers called. EmployerId: {employerId}, EmployerSearchTerm: {employerSearchTerm}");
            ShowJobsList = false; // Hide the job list when showing employers
            ActiveView = "Employers"; // Set view state to Employers

            // Set the EmployerSearchTerm property from the parameter
            if (!string.IsNullOrEmpty(employerSearchTerm))
            {
                EmployerSearchTerm = employerSearchTerm;
                _logger.LogInformation($"Set EmployerSearchTerm to: {EmployerSearchTerm}");
            }

            // Get all employers using the employer service
            var allEmployers = _employerService.GetEmployers() ?? new List<Employer>();
            _logger.LogInformation($"All employers count: {allEmployers.Count}");

            // Debug: Log all employer names
            foreach (var employer in allEmployers)
            {
                _logger.LogInformation($"Employer in database: {employer.EmployerName}");
            }

            // Apply employer search filter if provided
            IEnumerable<Employer> filteredEmployers = allEmployers; // Explicitly type as IEnumerable
            if (!string.IsNullOrEmpty(EmployerSearchTerm))
            {
                _logger.LogInformation($"Searching for employers with name containing '{EmployerSearchTerm}'");

                // Use case-insensitive search
                filteredEmployers = allEmployers.Where(e =>
                    e.EmployerName != null &&
                    e.EmployerName.IndexOf(EmployerSearchTerm, StringComparison.OrdinalIgnoreCase) >= 0);

                _logger.LogInformation($"Filtered employers count: {filteredEmployers.Count()}");

                // Debug: Log filtered employer names
                foreach (var employer in filteredEmployers)
                {
                    _logger.LogInformation($"Filtered employer: {employer.EmployerName}");
                }
            }

            // Extract unique employer names from filtered employers, filter out null/empty, order alphabetically, and convert to list
            DistinctEmployerNames = filteredEmployers
                                   .Where(e => !string.IsNullOrEmpty(e.EmployerName))
                                   .Select(e => e.EmployerName!) // Explicitly cast to non-nullable string
                                   .Distinct()
                                   .OrderBy(emp => emp)
                                   .ToList();

            _logger.LogInformation($"DistinctEmployerNames count: {DistinctEmployerNames.Count}");

            // Set the selected employer if employerId is provided or select the first one by default
            if (!string.IsNullOrEmpty(employerId))
            {
                SelectedEmployer = allEmployers.FirstOrDefault(e => e.EmployerName == employerId);

                // If no employer matches the provided ID, fall back to selecting the first employer
                if (SelectedEmployer == null && DistinctEmployerNames.Any())
                {
                    string firstEmployerName = DistinctEmployerNames.First();
                    SelectedEmployer = allEmployers.FirstOrDefault(e => e.EmployerName == firstEmployerName);
                }
            }
            else if (DistinctEmployerNames.Any())
            {
                // Select the first employer by default
                string firstEmployerName = DistinctEmployerNames.First();
                SelectedEmployer = allEmployers.FirstOrDefault(e => e.EmployerName == firstEmployerName);
            }
        }


        /// <summary>
        /// Populates the LocationOptions list with unique locations from the job data.
        /// </summary>
        private void PopulateLocationOptions()
        {
            // Get all jobs to extract unique locations.
            var allJobs = _jobService.GetJobs(); // Get all jobs to extract locations
            // Extract unique locations, filter out null/empty, order alphabetically, and convert to list.
            var uniqueLocations = allJobs
                                    .Where(j => !string.IsNullOrEmpty(j.Location))
                                    .Select(j => j.Location)
                                    .Distinct()
                                    .OrderBy(loc => loc)
                                    .ToList();

            // Add a default "All Locations" option to the dropdown.
            LocationOptions.Add(new SelectListItem("All Locations", "", string.IsNullOrEmpty(LocationFilter)));
            // Add unique locations as dropdown options.
            foreach (var loc in uniqueLocations)
            {
                LocationOptions.Add(new SelectListItem(loc, loc, loc == LocationFilter));
            }
        }

        /// <summary>
        /// Populates the EmployerFilterOptions list with unique employer names from the job data.
        /// </summary>
        private void PopulateEmployerFilterOptions()
        {
            // Get all jobs to extract unique employer names
            var allJobs = _jobService.GetJobs();

            // Extract unique employer names, filter out null/empty, order alphabetically, and convert to list
            var uniqueEmployers = allJobs
                                   .Where(j => !string.IsNullOrEmpty(j.EmployerName))
                                   .Select(j => j.EmployerName)
                                   .Distinct()
                                   .OrderBy(emp => emp)
                                   .ToList();

            // Clear existing options to prevent duplicates when repopulating
            EmployerFilterOptions.Clear();

            // Add a default "All Employers" option to the dropdown
            EmployerFilterOptions.Add(new SelectListItem("All Employers", "", string.IsNullOrEmpty(EmployerNameFilter)));

            // Add unique employer names as dropdown options
            foreach (var employer in uniqueEmployers)
            {
                EmployerFilterOptions.Add(new SelectListItem(employer, employer, employer == EmployerNameFilter));
            }
        }

        /// <summary>
        /// Handles GET requests to retrieve employer details as JSON.
        /// Used for dynamically loading employer details via AJAX.
        /// </summary>
        /// <param name="employerId">The name of the employer to get details for.</param>
        /// <returns>A JsonResult containing the employer details.</returns>
        public JsonResult OnGetEmployerDetailsJson(string employerId)
        {
            _logger.LogInformation($"Dashboard OnGetEmployerDetailsJson called for employerId: '{employerId}'");

            if (string.IsNullOrEmpty(employerId))
            {
                _logger.LogWarning("OnGetEmployerDetailsJson called with null or empty employerId.");
                return new JsonResult(new { error = "Employer ID cannot be empty." }) { StatusCode = 400 };
            }

            // Get all employers to find the one matching the provided name
            var employer = _employerService.GetEmployers().FirstOrDefault(e => e.EmployerName == employerId);

            if (employer == null)
            {
                _logger.LogWarning($"Employer with name '{employerId}' not found for OnGetEmployerDetailsJson.");
                return new JsonResult(new { error = $"Employer with name '{employerId}' not found." }) { StatusCode = 404 };
            }

            _logger.LogInformation($"Returning JSON for employer name '{employerId}'.");
            // Return the employer object as JSON with a 200 status code
            return new JsonResult(employer) { StatusCode = 200 };
        }

        /// <summary>
        /// Helper method to check if any job filter parameters have values.
        /// </summary>
        /// <returns>True if any job filter parameter has a value, false otherwise.</returns>
        private bool HasAnyJobFilterParameters()
        {
            return !string.IsNullOrEmpty(SearchTerm) ||
                   !string.IsNullOrEmpty(LocationFilter) ||
                   !string.IsNullOrEmpty(JobTypeFilter) ||
                   !string.IsNullOrEmpty(EmployerNameFilter);
        }
    } // End of DashboardModel class
} // End of namespace
