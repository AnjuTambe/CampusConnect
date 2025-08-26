using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CampusConnect.WebSite.Pages
{
    /// <summary>
    /// PageModel for the Blog page.
    /// </summary>
    public class BlogModel : PageModel
    {
        private readonly ILogger<BlogModel> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogModel"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public BlogModel(ILogger<BlogModel> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Handles the GET request for the Blog page.
        /// </summary>
        public void OnGet()
        {
        }
    }
}