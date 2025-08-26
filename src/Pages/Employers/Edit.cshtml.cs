using CampusConnect.WebSite.Models;      // Required for Employer model
using CampusConnect.WebSite.Services;     // Required for IEmployerService
using Microsoft.AspNetCore.Mvc;             // Required for IActionResult, BindProperty
using Microsoft.AspNetCore.Mvc.RazorPages;  // Required for PageModel
using Microsoft.Extensions.Logging;       // Required for ILogger
using System; // For Exception handling

namespace CampusConnect.WebSite.Pages.Employers
{
    /// <summary>
    /// PageModel for editing an existing Employer.
    /// </summary>
    public class EditModel : PageModel
    {
        private readonly ILogger<EditModel> _logger;
        private readonly IEmployerService _employerService;

        /// <summary>
        /// Gets or sets the employer to be updated.
        /// </summary>
        [BindProperty]
        public Employer Employer_To_Update { get; set; } = default!;

        /// <summary>
        /// Gets or sets the search term to preserve state.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string? EmployerSearchTerm { get; set; }

        /// <summary>
        /// Gets or sets the current page to preserve state.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditModel"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="employerService">The employer service.</param>
        public EditModel(ILogger<EditModel> logger, IEmployerService employerService)
        {
            _logger = logger;
            _employerService = employerService;
        }

        /// <summary>
        /// Handles GET requests for the Edit page.
        /// Loads the employer data based on the provided ID and preserves search/pagination state.
        /// </summary>
        /// <param name="id">The name of the employer to edit (used as ID).</param>
        /// <param name="employerSearchTerm">The current search term.</param>
        /// <param name="currentPage">The current page number.</param>
        /// <returns>An <see cref="IActionResult"/>.</returns>
        public IActionResult OnGet(string? id, string? employerSearchTerm, int currentPage = 1)
        {
            EmployerSearchTerm = employerSearchTerm;
            CurrentPage = currentPage;

            if (string.IsNullOrEmpty(id))
            {
                _logger.LogWarning("EditModel.OnGet called with no ID.");
                return NotFound(); // Or BadRequest(), depending on desired behavior for missing ID
            }

            var employer = _employerService.GetEmployerByName(id);
            if (employer == null)
            {
                _logger.LogWarning($"Employer with name {id} not found.");
                return NotFound();
            }

            Employer_To_Update = employer;
            return Page();
        }

        /// <summary>
        /// Handles POST requests for the Edit page.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/>.</returns>
        public IActionResult OnPost()
        {
            if (Employer_To_Update == null)
            {
                _logger.LogError("Employer_To_Update object was null during OnPost.");
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again.");
                return Page();
            }

            // Server-side validation for Employer Name
            if (string.IsNullOrEmpty(Employer_To_Update.EmployerName))
            {
                ModelState.AddModelError("Employer_To_Update.EmployerName", "Employer Name is required.");
            }

            // Server-side validation for Company Vision
            if (string.IsNullOrEmpty(Employer_To_Update.CompanyVision))
            {
                ModelState.AddModelError("Employer_To_Update.CompanyVision", "Company Vision is required.");
            }

            // Server-side validation for Tech Stack
            if (string.IsNullOrEmpty(Employer_To_Update.TechStack))
            {
                ModelState.AddModelError("Employer_To_Update.TechStack", "Tech Stack is required.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("EditModel.OnPost for Employer called with invalid ModelState.");
                return Page(); 
            }

            try
            {
                _logger.LogInformation($"Attempting to update employer: {Employer_To_Update.EmployerName}");
                _employerService.UpdateEmployer(Employer_To_Update);
                _logger.LogInformation($"Successfully updated employer: {Employer_To_Update.EmployerName}");

                // Success message
                TempData["Message"] = $"Employer '{Employer_To_Update.EmployerName}' updated successfully.";

                // Redirect to the Dashboard page, showing the employers with preserved search context
                return RedirectToPage("/Dashboard", "ShowEmployers", new {
                    employerSearchTerm = EmployerSearchTerm,
                    currentPage = CurrentPage
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating employer with name: {Employer_To_Update.EmployerName}");
                // Failure message
                ModelState.AddModelError(string.Empty, "An error occurred while updating the employer. Please try again.");
                return Page(); // Re-render with error
            }
        }
    }
}