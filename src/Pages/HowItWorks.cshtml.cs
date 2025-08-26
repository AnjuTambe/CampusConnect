using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CampusConnect.WebSite.Pages
{
    /// <summary>
    /// PageModel for the How It Works page.
    /// </summary>
    public class HowItWorksModel : PageModel
    {
        private readonly ILogger<HowItWorksModel> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HowItWorksModel"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public HowItWorksModel(ILogger<HowItWorksModel> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Handles the GET request for the How It Works page.
        /// </summary>
        public void OnGet()
        {
        }
    }
}