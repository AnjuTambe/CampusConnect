using CampusConnect.WebSite.Models;      // Required for Employer model
using CampusConnect.WebSite.Services;     // Required for IEmployerService
using Microsoft.AspNetCore.Mvc;             // Required for IActionResult, BindProperty
using Microsoft.AspNetCore.Mvc.RazorPages;  // Required for PageModel
using Microsoft.Extensions.Logging;       // Required for ILogger
using System; // For Exception handling
using System.Threading.Tasks;             // Required for Task for OnPostAsync if used

namespace CampusConnect.WebSite.Pages.Employers
{
    /// <summary>
    /// PageModel for deleting an existing Employer.
    /// </summary>
    public class DeleteModel : PageModel
    {
        private readonly ILogger<DeleteModel> _logger;
        private readonly IEmployerService _employerService;

        /// <summary>
        /// Gets or sets the employer to be displayed for deletion confirmation.
        /// </summary>
        [BindProperty]
        public Employer Employer_To_Delete { get; set; } = default!;

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
        /// Initializes a new instance of the <see cref="DeleteModel"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="employerService">The employer service.</param>
        public DeleteModel(ILogger<DeleteModel> logger, IEmployerService employerService)
        {
            _logger = logger;
            _employerService = employerService;
        }

        /// <summary>
        /// Handles GET requests for the Delete page.
        /// Loads the employer data based on the provided ID for confirmation.
        /// </summary>
        /// <param name="id">The name of the employer to delete (used as ID).</param>
        /// <param name="employerSearchTerm">The current search term.</param>
        /// <param name="currentPage">The current page number.</param>
        /// <returns>An <see cref="IActionResult"/>.</returns>
        public IActionResult OnGet(string? id, string? employerSearchTerm, int currentPage = 1)
        {
            EmployerSearchTerm = employerSearchTerm;
            CurrentPage = currentPage;
            if (string.IsNullOrEmpty(id))
            {
                _logger.LogWarning("DeleteModel.OnGet called with no ID.");
                return NotFound();
            }

            var employer = _employerService.GetEmployerByName(id);
            if (employer == null)
            {
                _logger.LogWarning($"Employer with name {id} not found for deletion.");
                return NotFound();
            }

            Employer_To_Delete = employer;
            return Page();
        }

        /// <summary>
        /// Handles POST requests for the Delete page.
        /// Deletes the specified employer.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/>.</returns>
        public IActionResult OnPost()
        {
            if (Employer_To_Delete == null || string.IsNullOrEmpty(Employer_To_Delete.EmployerName))
            {
                _logger.LogError("Employer_To_Delete was null or name was missing during OnPost for delete.");
                TempData["Error"] = "Could not delete employer: Information was missing. Please try again.";
                return RedirectToPage("/Dashboard", "ShowEmployers", new 
                {
                    employerSearchTerm = EmployerSearchTerm,
                    currentPage = CurrentPage
                });
            }

            try
            {
                _logger.LogInformation($"Attempting to delete employer: {Employer_To_Delete.EmployerName}");
                var employerName = Employer_To_Delete.EmployerName; // Capture name before potential deletion makes it null
                
                _employerService.DeleteEmployer(Employer_To_Delete.EmployerName); 
                _logger.LogInformation($"Successfully initiated deletion for employer: {employerName}");

                // Success message
                TempData["Message"] = $"Employer '{employerName}' deleted successfully.";

                return RedirectToPage("/Dashboard", "ShowEmployers", new 
                {
                    employerSearchTerm = EmployerSearchTerm,
                    currentPage = CurrentPage
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting employer: {Employer_To_Delete.EmployerName}");
                // Failure message
                TempData["Error"] = $"An error occurred while deleting employer '{Employer_To_Delete.EmployerName}'. Please try again.";
                
                // Redirect back to dashboard, as re-rendering delete page on error is often not ideal.
                return RedirectToPage("/Dashboard", "ShowEmployers", new 
                {
                    employerSearchTerm = EmployerSearchTerm,
                    currentPage = CurrentPage
                });
            }
        }
    }
}