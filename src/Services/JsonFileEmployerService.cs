using System; // Required for Exception and ArgumentException
using System.Collections.Generic;
using System.IO;
using System.Linq; // Required for LINQ operations
using System.Text.Json;
using CampusConnect.WebSite.Models;
using Microsoft.AspNetCore.Hosting;

namespace CampusConnect.WebSite.Services
{
    /// <summary>
    /// Service to read employer data from a JSON file.
    /// </summary>
    public class JsonFileEmployerService : IEmployerService
    {
        // The web hosting environment, used to get the content root path.
        public IWebHostEnvironment WebHostEnvironment { get; }

        /// <summary>
        /// Initializes a new instance of the JsonFileEmployerService.
        /// </summary>
        /// <param name="webHostEnvironment">The web hosting environment.</param>
        public JsonFileEmployerService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        // Full path to the employer.json file in wwwroot/data
        private string JsonFileName =>
            Path.Combine(WebHostEnvironment.WebRootPath, "data", "employer.json");

        /// <summary>
        /// Retrieves a list of all employers by reading from the employer.json file.
        /// </summary>
        public virtual List<Employer> GetEmployers()
        {
            // Initialize an empty list to hold employer data.
            List<Employer> employers = new List<Employer>();

            // Variable to store the resolved file path.
            string currentJsonFileName;
            try
            {
                currentJsonFileName = JsonFileName; // Attempt to resolve path first
            }
            catch (Exception ex) // Catch issues from JsonFileName resolution (e.g., mocked exception)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR: An unexpected error occurred in GetEmployers during path resolution: {ex.Message}");
                return employers; // Return empty list on error
            }

            // If path resolution succeeded, proceed to file operations
            try
            {
                using (var jsonFileReader = File.OpenText(currentJsonFileName))
                {
                    // Deserialize directly into List<Employer> or handle null
                    var employerData = JsonSerializer.Deserialize<List<Employer>>(jsonFileReader.ReadToEnd(),
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                    if (employerData != null)
                    {
                        employers = employerData;
                    }
                }
            }
            catch (ArgumentException argEx) // Handle if currentJsonFileName is invalid for File.OpenText
            {
                // Log the argument exception.
                System.Diagnostics.Debug.WriteLine($"ERROR (ArgumentException) opening/reading {currentJsonFileName} in GetEmployers: {argEx.Message}");
                return new List<Employer>(); // Return empty list on error
            }
            catch (FileNotFoundException)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR: Employer data file not found at {currentJsonFileName}");
                return new List<Employer>(); // Return empty list if file not found
            }
            catch (JsonException ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR: JSON error reading {currentJsonFileName}: {ex.Message}");
                return new List<Employer>(); // Return empty list on JSON error
            }
            catch (IOException ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR: IO error reading {currentJsonFileName}: {ex.Message}");
                return new List<Employer>(); // Return empty list on IO error
            }
            // The generic catch for path resolution is now separate and above.
            // This structure ensures file-specific IO/Json exceptions are caught after path is resolved.

            return employers;
        }

        /// <summary>
        /// Gets an employer by its name (used as unique identifier).
        /// </summary>
        /// <param name="employerName">The name of the employer to retrieve.</param>
        /// <returns>The employer if found, otherwise null.</returns>
        public virtual Employer GetEmployerByName(string employerName)
        {
            if (string.IsNullOrEmpty(employerName))
            {
                return null;
            }

            // Get all employers
            var employers = GetEmployers();

            // Find the employer with the matching name
            return employers.FirstOrDefault(e =>
                string.Equals(e.EmployerName, employerName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Adds a new employer to the system.
        /// </summary>
        /// <param name="employer">The employer to add.</param>
        public virtual void AddEmployer(Employer employer)
        {
            if (employer == null)
            {
                throw new ArgumentNullException(nameof(employer));
            }

            if (string.IsNullOrEmpty(employer.EmployerName))
            {
                throw new ArgumentException("Employer must have a name", nameof(employer));
            }

            // Get existing employers
            var employers = GetEmployers();

            // Check if an employer with the same name already exists
            if (employers.Any(e => string.Equals(e.EmployerName, employer.EmployerName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"An employer with the name '{employer.EmployerName}' already exists.");
            }

            // Add the new employer
            employers.Add(employer);

            // Save the updated list
            SaveAllEmployers(employers);
        }

        /// <summary>
        /// Updates an existing employer in the system.
        /// </summary>
        /// <param name="employer">The employer with updated information.</param>
        public virtual void UpdateEmployer(Employer employer)
        {
            if (employer == null)
            {
                throw new ArgumentNullException(nameof(employer));
            }

            if (string.IsNullOrEmpty(employer.EmployerName))
            {
                throw new ArgumentException("Employer must have a name", nameof(employer));
            }

            // Get existing employers
            var employers = GetEmployers();

            // Find the index of the employer to update
            int index = employers.FindIndex(e =>
                string.Equals(e.EmployerName, employer.EmployerName, StringComparison.OrdinalIgnoreCase));

            if (index == -1)
            {
                throw new InvalidOperationException($"Employer '{employer.EmployerName}' not found.");
            }

            // Update the employer at the found index
            employers[index] = employer;

            // Save the updated list
            SaveAllEmployers(employers);
        }

        /// <summary>
        /// Deletes an employer from the system.
        /// </summary>
        /// <param name="employerName">The name of the employer to delete.</param>
        public virtual void DeleteEmployer(string employerName)
        {
            if (string.IsNullOrEmpty(employerName))
            {
                throw new ArgumentException("Employer name cannot be null or empty", nameof(employerName));
            }

            // Get existing employers
            var employers = GetEmployers();

            // Find the employer to remove
            var employer = employers.FirstOrDefault(e =>
                string.Equals(e.EmployerName, employerName, StringComparison.OrdinalIgnoreCase));

            if (employer == null)
            {
                throw new InvalidOperationException($"Employer '{employerName}' not found.");
            }

            // Remove the employer
            employers.Remove(employer);

            // Save the updated list
            SaveAllEmployers(employers);
        }

        /// <summary>
        /// Saves all employers to the JSON file.
        /// </summary>
        /// <param name="employers">The list of employers to save.</param>
        protected virtual void SaveAllEmployers(List<Employer> employers)
        {
            try
            {
                using (var outputStream = File.Create(JsonFileName))
                {
                    JsonSerializer.Serialize(
                        outputStream,
                        employers,
                        new JsonSerializerOptions
                        {
                            WriteIndented = true,
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR: Failed to save employers: {ex.Message}");
                throw new IOException($"Failed to save employers: {ex.Message}", ex);
            }
        }
    }
}
