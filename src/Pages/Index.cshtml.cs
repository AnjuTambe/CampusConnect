using System;
using System.Collections.Generic; // Keep if other collections are used, otherwise remove

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using CampusConnect.WebSite.Models;
using CampusConnect.WebSite.Services;

namespace CampusConnect.WebSite.Pages
{
    /// <summary>
    /// PageModel for the Index page.
    /// </summary>
    public class IndexModel : PageModel
    {
        // Logger for the IndexModel.
        private readonly ILogger<IndexModel> _logger;

        /// <summary>
        /// Initializes a new instance of the IndexModel.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        // Updated constructor to remove ProductService injection
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        // Removed: public JsonFileProductService ProductService { get; }
        // Removed: public IEnumerable<ProductModel> Products { get; private set; }

        /// <summary>
        /// Handles GET requests for the Index page.
        /// </summary>
        public void OnGet()
        {
            // Removed: Products = ProductService.GetProducts();
            // OnGet is now empty, can add other logic if needed later

            // Set ViewData to use a fluid container for full-width layout
            ViewData["UseFluidContainer"] = true;
        }
    }
}