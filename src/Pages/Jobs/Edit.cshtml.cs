using System; // Added for Exception
using CampusConnect.WebSite.Models;
using CampusConnect.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace CampusConnect.WebSite.Pages.Jobs
{
    /// <summary>
    /// PageModel for the Edit Job listing page.
    /// Handles displaying the form with existing job data and processing updates.
    /// </summary>
    public class EditModel : PageModel
    {
        // Logger for the EditModel.
        private readonly ILogger<EditModel> _logger;
        // Service to interact with job listing data.
        private readonly IJobService _jobService;

        // Property to bind the job listing data from the form for updating.
        [BindProperty]
        public JobListing JobToUpdate { get; set; } = default!; // Initialize with default! to satisfy nullable context

        // Property to bind the search term from the query string (to preserve state).
        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        // Property to bind the location filter from the query string (to preserve state).
        [BindProperty(SupportsGet = true)]
        public string? LocationFilter { get; set; }

        // Property to bind the current page number from the query string (to preserve state).
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        // Property to bind the employer name filter from the query string (to preserve state).
        [BindProperty(SupportsGet = true)]
        public string? EmployerNameFilter { get; set; }

        /// <summary>
        /// Initializes a new instance of the EditModel.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="jobService">The job service instance.</param>
        public EditModel(ILogger<EditModel> logger, IJobService jobService)
        {
            _logger = logger;
            _jobService = jobService;
        }

        /// <summary>
        /// Handles GET requests for the Edit page.
        /// Fetches the job details by ID and populates the form.
        /// Preserves search and pagination state from the query string.
        /// </summary>
        /// <param name="id">The ID of the job to edit.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="locationFilter">The location filter.</param>
        /// <param name="currentPage">The current page number.</param>
        /// <returns>An IActionResult representing the result of the GET operation.</returns>
        public IActionResult OnGet(string id, string? searchTerm, string? locationFilter, string? employerNameFilter, string? currentPage = "1")
        {
            SearchTerm = searchTerm;
            LocationFilter = locationFilter;
            EmployerNameFilter = employerNameFilter;
            CurrentPage = int.TryParse(currentPage, out int page) ? page : 1;

            if (string.IsNullOrEmpty(id))
            {
                _logger.LogWarning("OnGet called with null or empty ID.");
                return NotFound("Job ID cannot be empty.");
            }

            // Fetch the job by ID using the job service.
            var job = _jobService.GetJobById(id);
            if (job == null)
            {
                _logger.LogWarning($"Job with ID '{id}' not found.");
                return NotFound($"Job with ID '{id}' not found.");
            }

            JobToUpdate = job; // Assign the fetched job to the bindable property
            _logger.LogInformation($"Loaded job with ID '{id}' for editing.");
            return Page(); // Render the page with the populated JobToUpdate
        } // End of OnGet

        /// <summary>
        /// Handles POST requests for the Edit page.
        /// Processes the submitted form data, performs validation, updates the job, and redirects.
        /// </summary>
        /// <returns>An IActionResult representing the result of the POST operation.</returns>
        public IActionResult OnPost()
        {
            // Basic validation (more robust validation can be added)
            if (ModelState.IsValid == false)
            {
                _logger.LogWarning("EditModel.OnPost called with invalid ModelState (initial check).");
                // Re-render the page to show validation errors
                // JobToUpdate should retain its values due to [BindProperty]
                return Page();
            }

            try
            {
                // Stricter server-side URL validation for ApplyUrl
                if (!string.IsNullOrEmpty(JobToUpdate.ApplyUrl))
                {
                    // Regex for a more comprehensive URL check (allows http, https, ftp)
                    // This is a common one, but URL regex can be very complex.
                    var urlRegex = new Regex(@"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_]*)?$", RegexOptions.IgnoreCase);
                    bool isValidUrl = urlRegex.IsMatch(JobToUpdate.ApplyUrl);
                    if (isValidUrl == false)
                    {
                        // Add a model error if the URL format is invalid.
                        ModelState.AddModelError("JobToUpdate.ApplyUrl", "The Apply URL format is invalid.");
                        _logger.LogWarning($"EditModel.OnPost: Invalid ApplyUrl format: {JobToUpdate.ApplyUrl}");
                    }
                }

                // Stricter server-side Location validation for "City, ST" format
                // This check is done if Location is provided, as [Required] already handles empty.
                if (!string.IsNullOrEmpty(JobToUpdate.Location))
                {
                    // Regex for "City, ST" format or "Remote".
                    var locationRegex = new Regex(@"^(?:[A-Za-z\s.-]+,\s[A-Z]{2}|Remote)$", RegexOptions.IgnoreCase);
                    bool isValidLocation = locationRegex.IsMatch(JobToUpdate.Location);
                    if (isValidLocation == false)
                    {
                        // Add a model error if the location format is invalid.
                        ModelState.AddModelError("JobToUpdate.Location", "Location must be in 'City, ST' format (e.g., Seattle, WA) or 'Remote'.");
                        _logger.LogWarning($"EditModel.OnPost: Invalid Location format: {JobToUpdate.Location}");
                    }
                }

                // Re-check ModelState after custom validation
                if (ModelState.IsValid == false)
                {
                    _logger.LogWarning("EditModel.OnPost called with invalid ModelState (after custom URL and Location checks).");
                    return Page();
                }

                _logger.LogInformation($"Attempting to update job with ID '{JobToUpdate.Id}'.");
                // Update the job using the job service.
                _jobService.UpdateJob(JobToUpdate); // DoD 1 Met
                _logger.LogInformation($"Successfully updated job with ID '{JobToUpdate.Id}'.");

                // Redirect to the Dashboard page, showing the details of the updated job, preserving search context
                return RedirectToPage("/Dashboard", "ShowJobDetails", new
                {
                    id = JobToUpdate.Id,
                    searchTerm = SearchTerm,
                    locationFilter = LocationFilter,
                    employerNameFilter = EmployerNameFilter,
                    currentPage = CurrentPage
                });
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, $"Error updating job with ID '{JobToUpdate?.Id}'.");
                // Optionally add a model error to display to the user
                ModelState.AddModelError(string.Empty, "An error occurred while updating the job.");
                // Re-render the page
                return Page();
            }
        } // End of OnPost
    } // End of EditModel class
} // End of namespace