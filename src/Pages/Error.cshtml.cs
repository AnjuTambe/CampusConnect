using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CampusConnect.WebSite.Pages
{
    /// <summary>
    /// PageModel for the Error page.
    /// Configures response caching to prevent caching of error responses.
    /// </summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : PageModel
    {
        // The request ID for the current request.
        public string RequestId { get; set; } = default!;

        // Indicates whether the RequestId should be shown.
        public bool ShowRequestId => string.IsNullOrEmpty(RequestId) == false;

        // Logger for the ErrorModel.
        private readonly ILogger<ErrorModel> _logger;

        /// <summary>
        /// Initializes a new instance of the ErrorModel.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Handles GET requests for the Error page.
        /// Retrieves the RequestId for display.
        /// </summary>
        public void OnGet()
        {
            // Get the current activity ID or the HTTP context trace identifier.
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }
    }
}