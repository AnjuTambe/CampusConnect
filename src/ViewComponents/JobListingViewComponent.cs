using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CampusConnect.WebSite.Models;
using CampusConnect.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CampusConnect.WebSite.ViewComponents
{
    /// <summary>
    /// ViewComponent to display a paginated and filtered list of job listings.
    /// </summary>
    public class JobListingViewComponent : ViewComponent
    {
        // Service to interact with job listing data.
        private readonly IJobService _jobService;
        // Logger for the JobListingViewComponent.
        private readonly ILogger<JobListingViewComponent> _logger;
        // Number of jobs to display per page in the view component.
        private const int PageSize = 20; // Changed from 6 to 20

        /// <summary>
        /// Initializes a new instance of the JobListingViewComponent.
        /// </summary>
        /// <param name="jobService">The job service instance.</param>
        /// <param name="logger">The logger instance.</param>
        public JobListingViewComponent(IJobService jobService, ILogger<JobListingViewComponent> logger)
        {
            _jobService = jobService;
            _logger = logger;
        }

        /// <summary>
        /// Invokes the ViewComponent asynchronously to fetch and prepare job data for display.
        /// </summary>
        /// <param name="searchTerm">Optional keyword to filter jobs.</param>
        /// <param name="locationFilter">Optional location to filter jobs.</param>
        /// <param name="jobTypeFilter">Optional job type to filter jobs.</param>
        /// <param name="currentPage">The current page number for pagination.</param>
        /// <returns>A Task representing the asynchronous operation, returning an IViewComponentResult.</returns>
        /// <summary>
        /// Invokes the ViewComponent asynchronously to fetch and prepare job data for display.
        /// </summary>
        /// <param name="searchTerm">Optional keyword to filter jobs.</param>
        /// <param name="locationFilter">Optional location to filter jobs.</param>
        /// <param name="jobTypeFilter">Optional job type to filter jobs.</param>
        /// <param name="employerNameFilter">Optional employer name to filter jobs.</param>
        /// <param name="currentPage">The current page number for pagination.</param>
        /// <returns>A Task representing the asynchronous operation, returning an IViewComponentResult.</returns>
        public Task<IViewComponentResult> InvokeAsync(
            string? searchTerm, string? locationFilter, string? jobTypeFilter, string? employerNameFilter, int currentPage = 1)
        {
            _logger.LogInformation($"JobListingViewComponent invoked. Search: '{searchTerm}', Location: '{locationFilter}', JobType: '{jobTypeFilter}', EmployerName: '{employerNameFilter}', Page: {currentPage}");

            // Ensure current page is at least 1.
            if (currentPage < 1) currentPage = 1;

            // Get all filtered jobs using the job service.
            var allFilteredJobs = _jobService.GetJobs(searchTerm, locationFilter, jobTypeFilter, employerNameFilter) ?? Enumerable.Empty<JobListing>();
            // Get the total number of items after filtering.
            int totalItems = allFilteredJobs.Count();
            // Calculate the total number of pages.
            int totalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

            // Adjust current page if it's out of bounds.
            if (currentPage > totalPages && totalPages > 0) currentPage = totalPages;

            // Get the subset of jobs for the current page.
            var jobsForPage = allFilteredJobs
                                .Skip((currentPage - 1) * PageSize)
                                .Take(PageSize)
                                .ToList();

            // Create the ViewModel to pass data to the view.
            var model = new JobListingViewModel
            {
                // Job listings for the current page.
                JobListings = jobsForPage,
                // Current search term.
                SearchTerm = searchTerm,
                // Current location filter.
                LocationFilter = locationFilter,
                // Current job type filter.
                JobTypeFilter = jobTypeFilter,
                // Current employer name filter.
                EmployerNameFilter = employerNameFilter,
                // Current page number.
                CurrentPage = currentPage,
                // Total number of pages.
                TotalPages = totalPages,
                // Total number of items before pagination.
                TotalItems = totalItems
            };

            // The view will be Views/Shared/Components/JobListing/Default.cshtml
            // Return the default view with the populated ViewModel.
            return Task.FromResult((IViewComponentResult)View(model));
        }
    }

    /// <summary>
    /// ViewModel for the JobListingViewComponent.
    /// Contains data needed to render the job list and pagination controls.
    /// </summary>
    public class JobListingViewModel
    {
        // Collection of job listings to display on the current page.
        public IEnumerable<JobListing> JobListings { get; set; } = Enumerable.Empty<JobListing>();
        // The current search term.
        public string? SearchTerm { get; set; }
        // The current location filter.
        public string? LocationFilter { get; set; }
        // The current job type filter.
        public string? JobTypeFilter { get; set; }
        // The current employer name filter.
        public string? EmployerNameFilter { get; set; }
        // The current page number.
        public int CurrentPage { get; set; }
        // The total number of pages.
        public int TotalPages { get; set; }
        // The total number of items before pagination.
        public int TotalItems { get; set; }
    }
}