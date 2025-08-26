using CampusConnect.WebSite.Models;
using CampusConnect.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System; // For Exception handling

namespace CampusConnect.WebSite.Pages.Employers
{
    /// <summary>
    /// PageModel for creating a new Employer.
    /// </summary>
    public class CreateModel : PageModel
    {
        private readonly ILogger<CreateModel> _logger;
        private readonly IEmployerService _employerService;

        /// <summary>
        /// Gets or sets the new employer to be created.
        /// </summary>
        [BindProperty]
        public Employer New_Employer { get; set; } = new Employer();

        /// <summary>
        /// Gets or sets the search term to preserve state.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string? Search_Term { get; set; }

        /// <summary>
        /// Gets or sets the location filter to preserve state.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string? Location_Filter { get; set; }

        /// <summary>
        /// Gets or sets the current page to preserve state.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public int Current_Page { get; set; } = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateModel"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="employerService">The employer service.</param>
        public CreateModel(ILogger<CreateModel> logger, IEmployerService employerService)
        {
            _logger = logger;
            _employerService = employerService;
        }

        /// <summary>
        /// Handles GET requests for the Create page.
        /// Preserves search and pagination state from the query string.
        /// </summary>
        /// <param name="search_Term">The search term.</param>
        /// <param name="location_Filter">The location filter.</param>
        /// <param name="current_Page">The current page number.</param>
        public void OnGet(string? search_Term, string? location_Filter, int current_Page = 1)
        {
            Search_Term = search_Term;
            Location_Filter = location_Filter;
            Current_Page = current_Page;
        }

        /// <summary>
        /// Handles POST requests for the Create page.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/>.</returns>
        public IActionResult OnPost()
        {
            if (New_Employer == null)
            {
                _logger.LogError("New_Employer object was null during OnPost.");
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again.");
                return Page();
            }

            // Server-side validation for Employer Name
            if (string.IsNullOrEmpty(New_Employer.EmployerName))
            {
                ModelState.AddModelError("New_Employer.EmployerName", "Employer Name is required.");
            }

            // Server-side validation for Company Vision
            if (string.IsNullOrEmpty(New_Employer.CompanyVision))
            {
                ModelState.AddModelError("New_Employer.CompanyVision", "Company Vision is required.");
            }

            // Server-side validation for Tech Stack
            if (string.IsNullOrEmpty(New_Employer.TechStack))
            {
                ModelState.AddModelError("New_Employer.TechStack", "Tech Stack is required.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("CreateModel.OnPost for Employer called with invalid ModelState.");
                return Page(); // Re-render the page to show validation errors
            }

            try
            {
                _logger.LogInformation($"Attempting to add new employer: {New_Employer.EmployerName}");
                _employerService.AddEmployer(New_Employer); 
                _logger.LogInformation($"Successfully added new employer: {New_Employer.EmployerName}");

                // Success message
                TempData["Message"] = $"Employer '{New_Employer.EmployerName}' created successfully.";

                // Redirect to the Dashboard page
                return RedirectToPage("/Dashboard", new { ActiveView = "Employers" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding new employer with name: {New_Employer.EmployerName}");
                // Failure message
                ModelState.AddModelError(string.Empty, "An error occurred while creating the employer. Please try again.");
                return Page(); // Re-render with error
            }
        }
    }
}