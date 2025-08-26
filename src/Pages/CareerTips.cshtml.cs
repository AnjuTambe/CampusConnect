using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CampusConnect.WebSite.Pages
{
    /// <summary>
    /// PageModel for the Career Tips page.
    /// </summary>
    public class CareerTipsModel : PageModel
    {
        private readonly ILogger<CareerTipsModel> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CareerTipsModel"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public CareerTipsModel(ILogger<CareerTipsModel> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Handles the GET request for the Career Tips page.
        /// </summary>
        public void OnGet()
        {
        }
    }
}