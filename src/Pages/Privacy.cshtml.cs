using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CampusConnect.WebSite.Pages
{
    /// <summary>
    /// PageModel for the Privacy page.
    /// </summary>
    public class PrivacyModel : PageModel
    {
        // Logger for the PrivacyModel.
        private readonly ILogger<PrivacyModel> _logger;

        /// <summary>
        /// Initializes a new instance of the PrivacyModel.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Handles GET requests for the Privacy page.
        /// </summary>
        public void OnGet()
        {
        }
    }
}