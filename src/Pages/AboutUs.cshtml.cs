using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CampusConnect.WebSite.Pages
{
    /// <summary>
    /// PageModel for the About Us page.
    /// </summary>
    public class AboutUsModel : PageModel
    {
        // Logger for the AboutUsModel.
        private readonly ILogger<AboutUsModel> _logger;

        /// <summary>
        /// Initializes a new instance of the AboutUsModel.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public AboutUsModel(ILogger<AboutUsModel> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// This method is called when the page is requested using a GET request.
        /// </summary>
        public void OnGet()
        {
        }
    }
}