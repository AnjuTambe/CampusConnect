using System;
using CampusConnect.WebSite.Models;
using CampusConnect.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CampusConnect.WebSite.Pages.Jobs
{
    /// <summary>
    /// PageModel for the Delete Job listing confirmation page.
    /// Handles displaying job details for confirmation and processing deletion requests.
    /// </summary>
    public class DeleteModel : PageModel
    {
        // Logger for the DeleteModel.
        private readonly ILogger<DeleteModel> _logger;
        // Service to interact with job listing data.
        private readonly IJobService _jobService;

        // This property will hold the job details for confirmation display.
        public JobListing? JobToDelete { get; private set; }

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
        /// Initializes a new instance of the DeleteModel.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="jobService">The job service instance.</param>
        public DeleteModel(ILogger<DeleteModel> logger, IJobService jobService)
        {
            _logger = logger;
            _jobService = jobService;
        }

        /// <summary>
        /// Handles GET requests for the Delete confirmation page.
        /// Fetches the job details by ID for display.
        /// Preserves search and pagination state from the query string.
        /// </summary>
        /// <param name="id">The ID of the job to delete.</param>
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
                _logger.LogWarning("Delete.OnGet called with null or empty ID.");
                return NotFound("Job ID cannot be empty for deletion confirmation.");
            }

            // Fetch the job by ID using the job service.
            JobToDelete = _jobService.GetJobById(id);

            if (JobToDelete == null)
            {
                _logger.LogWarning($"Job with ID '{id}' not found for deletion confirmation.");
                return NotFound($"Job with ID '{id}' not found.");
            }

            _logger.LogInformation($"Displaying confirmation to delete job with ID '{id}'.");
            return Page();
        }

        /// <summary>
        /// Handles POST requests for the Delete confirmation page.
        /// Processes the deletion request for the specified job ID.
        /// </summary>
        /// <param name="id">The ID of the job to delete.</param>
        /// <returns>An IActionResult representing the result of the POST operation.</returns>
        public IActionResult OnPost(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _logger.LogWarning("Delete.OnPost called with null or empty ID.");
                return RedirectToPage("./Index");
            }

            try
            {
                _logger.LogInformation($"Attempting to delete job with ID '{id}'.");
                
                // Verify job exists before attempting delete
                var job = _jobService.GetJobById(id);
                if (job == null)
                {
                    _logger.LogWarning($"Job with ID '{id}' not found for deletion during OnPost.");
                    return NotFound($"Job with ID '{id}' not found to delete.");
                }

                // Delete the job using the job service.
                _jobService.DeleteJob(id);
                _logger.LogInformation($"Successfully deleted job with ID '{id}'.");
                // Set a success message in TempData for display on the redirected page.
                TempData["Message"] = $"Job '{job.Title}' deleted successfully.";
                
                // Redirect to the Dashboard page, showing the job list, preserving search context
                return RedirectToPage("/Dashboard", "ShowJobs", new {
                    searchTerm = SearchTerm,
                    locationFilter = LocationFilter,
                    employerNameFilter = EmployerNameFilter,
                    currentPage = CurrentPage
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting job with ID '{id}'.");
                
                // Add a model error
                ModelState.AddModelError(string.Empty, "An error occurred while deleting the job.");
                
                // Set TempData error message
                TempData["Error"] = "An error occurred while deleting the job.";
                
                // Return to the same page to show the error
                return Page();
            }
        }
    }
}