#nullable enable
using System.Collections.Generic;
using CampusConnect.WebSite.Models;

namespace CampusConnect.WebSite.Services
{
    /// <summary>
    /// Interface for services providing job listing data.
    /// </summary>
    public interface IJobService
    {
        /// <summary>
        /// Retrieves all job listings, optionally filtered by search term, location, job type, and employer name.
        /// </summary>
        /// <param name="searchTerm">Optional keyword to search in title, company, or description.</param>
        /// <param name="location">Optional location filter.</param>
        /// <param name="jobType">Optional job type filter.</param>
        /// <param name="employerNameFilter">Optional employer name filter.</param>
        /// <returns>An enumerable collection of JobListing objects.</returns>
        IEnumerable<JobListing> GetJobs(string? searchTerm = null, string? location = null, string? jobType = null, string? employerNameFilter = null);

        /// <summary>
        /// Retrieves a specific job listing by its ID.
        /// </summary>
        /// <param name="id">The ID of the job listing to retrieve.</param>
        /// <returns>The JobListing object if found; otherwise, null.</returns>
        JobListing? GetJobById(string id);
        /// <summary>
        /// Adds a new job listing.
        /// </summary>
        /// <param name="newJob">The JobListing object representing the new job.</param>
        void AddJob(JobListing newJob);
        /// <summary>
        /// Updates an existing job listing.
        /// </summary>
        /// <param name="updatedJob">The JobListing object with updated information.</param>
        void UpdateJob(JobListing updatedJob);
        /// <summary>
        /// Deletes a job listing by its ID.
        /// </summary>
        /// <param name="id">The ID of the job listing to delete.</param>
        void DeleteJob(string id);
    }
}