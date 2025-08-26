using System.Collections.Generic;
using CampusConnect.WebSite.Models;

namespace CampusConnect.WebSite.Services
{
    /// <summary>
    /// Interface for the Employer service.
    /// </summary>
    public interface IEmployerService
    {
        /// <summary>
        /// Retrieves a list of all employers.
        /// </summary>
        List<Employer> GetEmployers();

        /// <summary>
        /// Gets an employer by its name (used as unique identifier).
        /// </summary>
        /// <param name="employerName">The name of the employer to retrieve.</param>
        /// <returns>The employer if found, otherwise null.</returns>
        Employer GetEmployerByName(string employerName);

        /// <summary>
        /// Adds a new employer to the system.
        /// </summary>
        /// <param name="employer">The employer to add.</param>
        void AddEmployer(Employer employer);

        /// <summary>
        /// Updates an existing employer in the system.
        /// </summary>
        /// <param name="employer">The employer with updated information.</param>
        void UpdateEmployer(Employer employer);

        /// <summary>
        /// Deletes an employer from the system.
        /// </summary>
        /// <param name="employerName">The name of the employer to delete.</param>
        void DeleteEmployer(string employerName);
    }
}