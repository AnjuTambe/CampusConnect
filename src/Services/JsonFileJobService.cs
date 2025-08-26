using System; // For StringComparison
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using CampusConnect.WebSite.Models;
using Microsoft.AspNetCore.Hosting;

namespace CampusConnect.WebSite.Services
{
    /// <summary>
    /// Service to read job listing data from a JSON file.
    /// </summary>
    public class JsonFileJobService : IJobService
    {
        /// <summary>
        /// Initializes a new instance of the JsonFileJobService.
        /// </summary>
        /// <param name="webHostEnvironment">The web hosting environment.</param>
        public JsonFileJobService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        // The web hosting environment, used to get the content root path.
        public IWebHostEnvironment WebHostEnvironment { get; }

        // Full path to the jobs.json file in wwwroot/data
        private string JsonFileName =>
            Path.Combine(WebHostEnvironment.WebRootPath, "data", "jobs.json");

        /// <summary>
        /// Retrieves job listings and optionally filters by searchTerm, location, jobType, and employerNameFilter.
        /// </summary>
        /// <param name="searchTerm">Optional keyword (title, company, description)</param>
        /// <param name="location">Optional location filter (e.g., "Seattle, WA")</param>
        /// <param name="jobType">Optional job type filter.</param>
        /// <param name="employerNameFilter">Optional employer name filter.</param>
        public virtual IEnumerable<JobListing> GetJobs(string? searchTerm = null, string? location = null, string? jobType = null, string? employerNameFilter = null)
        {
            // Collection to hold all job listings read from the file.
            IEnumerable<JobListing> allJobs;

            // Variable to store the resolved file path.
            string currentJsonFileName;
            try
            {
                currentJsonFileName = JsonFileName; // Attempt to resolve path first
            }
            catch (Exception ex) // Catch issues from JsonFileName resolution (e.g., mocked exception)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR: An unexpected error occurred in GetJobs during path resolution: {ex.Message}");
                return Enumerable.Empty<JobListing>();
            }

            // If path resolution succeeded, proceed to file operations
            try
            {
                using (var jsonFileReader = File.OpenText(currentJsonFileName))
                {
                    allJobs = JsonSerializer.Deserialize<JobListing[]>(jsonFileReader.ReadToEnd(),
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }) ?? Enumerable.Empty<JobListing>();
                }
            }
            catch (ArgumentException argEx) // Handle if currentJsonFileName is invalid for File.OpenText
            {
                // Log the argument exception.
                System.Diagnostics.Debug.WriteLine($"ERROR (ArgumentException) opening/reading {currentJsonFileName} in GetJobs: {argEx.Message}");
                return Enumerable.Empty<JobListing>();
            }
            catch (FileNotFoundException)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR: Job data file not found at {currentJsonFileName}");
                return Enumerable.Empty<JobListing>();
            }
            catch (JsonException ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR: JSON error reading {currentJsonFileName}: {ex.Message}");
                return Enumerable.Empty<JobListing>();
            }
            catch (IOException ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR: IO error reading {currentJsonFileName}: {ex.Message}");
                return Enumerable.Empty<JobListing>();
            }
            // The generic catch for path resolution is now separate and above.
            // This structure ensures file-specific IO/Json exceptions are caught after path is resolved.

            // Apply keyword search if provided
            if (!string.IsNullOrEmpty(searchTerm))
            {
                allJobs = allJobs.Where(job =>
                    (job.Title != null && job.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (job.EmployerName != null && job.EmployerName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (job.Description != null && job.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
            }

            // Apply location filter if provided
            if (!string.IsNullOrEmpty(location))
            {
                allJobs = allJobs.Where(job =>
                    string.Equals(job.Location, location, StringComparison.OrdinalIgnoreCase));
            }

            // Apply job type filter if provided
            if (!string.IsNullOrEmpty(jobType))
            {
                allJobs = allJobs.Where(job =>
                    string.Equals(job.JobType, jobType, StringComparison.OrdinalIgnoreCase));
            }

            // Apply employer name filter if provided
            // Filter by employer name if a filter is provided.
            // This is a case-insensitive search.
            if (!string.IsNullOrEmpty(employerNameFilter))
            {
                allJobs = allJobs.Where(job =>
                    job.EmployerName != null && job.EmployerName.Contains(employerNameFilter, StringComparison.OrdinalIgnoreCase));
            }

            return allJobs;
        }

        /// <summary>
        /// Retrieves a single job listing by its ID.
        /// </summary>
        public virtual JobListing? GetJobById(string id)
        {
            // Retrieve all jobs using the GetJobs method.
            var allJobs = GetJobs();
            // Find the first job that matches the provided ID (case-insensitive).
            return allJobs.FirstOrDefault(job =>
                string.Equals(job.Id, id, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Adds a new job listing to the JSON file.
        /// </summary>
        /// <param name="newJob">The JobListing object representing the new job.</param>
        public virtual void AddJob(JobListing newJob)
        {
            // Validate input parameter
            if (newJob == null)
                throw new ArgumentNullException(nameof(newJob));

            // 1. Read existing jobs
            // We can reuse the GetJobs() method, but it filters. For adding, we need the raw list.
            // Let's read directly or create a helper method later if needed.
            // Initialize an empty list to hold existing jobs.
            List<JobListing> allJobs = new List<JobListing>(); // Initialize upfront
            // Variable to store the resolved file name for reading.
            string? resolvedJsonFileName = null;

            try
            {
                resolvedJsonFileName = JsonFileName; // Attempt to resolve path first
            }
            catch (Exception ex) // Catch issues from JsonFileName resolution
            {
                System.Diagnostics.Debug.WriteLine($"ERROR (Generic) during path resolution for AddJob: {ex.Message}");
                // `allJobs` is already an empty list. WriteLogic will proceed with this.
                // The write operation will attempt to resolve JsonFileName again.
            }

            // If path resolution succeeded and file exists, read existing jobs
            if (resolvedJsonFileName != null && File.Exists(resolvedJsonFileName))
            {
                try
                {
                    using (var jsonFileReader = File.OpenText(resolvedJsonFileName))
                    {
                        // Deserialize directly into List<JobListing> or handle null
                        var existingJobs = JsonSerializer.Deserialize<List<JobListing>>(jsonFileReader.ReadToEnd(),
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        if (existingJobs != null)
                        {
                            allJobs = existingJobs;
                        }
                    }
                }
                catch (JsonException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"ERROR (JsonException) reading {resolvedJsonFileName} in AddJob: {ex.Message}");
                    // Potentially re-throw or handle as a critical error if adding requires valid existing data.
                    // For now, proceeds with an empty list, which might overwrite if not desired.
                }
                catch (IOException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"ERROR (IOException) reading {resolvedJsonFileName} in AddJob: {ex.Message}");
                }
            }

            // 2. Add the new job
            // Always generate a new ID regardless of whether one was provided
            newJob.Id = Guid.NewGuid().ToString();

            // Always set DatePosted to the current UTC time
            newJob.DatePosted = DateTime.UtcNow;

            // Ensure EmployerName is set by the caller
            // For this task, we assume newJob.EmployerName is already populated by the caller.

            allJobs.Add(newJob); // Add the new job to the list

            // 3. Write the updated list back to the file
            try
            {
                // Resolve JsonFileName again for writing to ensure it's current
                string writeJsonFileName = JsonFileName;
                using (var outputStream = File.Create(writeJsonFileName)) // Use File.Create to overwrite
                {
                    JsonSerializer.Serialize<IEnumerable<JobListing>>(
                        new Utf8JsonWriter(outputStream, new JsonWriterOptions
                        {
                            SkipValidation = true, // Consider if validation is needed
                            Indented = true
                        }),
                        allJobs
                    );
                }
            }
            catch (Exception ex) // Catch all exceptions during file writing & path resolution
            {
                System.Diagnostics.Debug.WriteLine($"ERROR (Generic) writing to JSON file in AddJob: {ex.Message}");
                // Re-throw the exception to indicate failure as expected by tests
                throw;
            }
        }
        /// <summary>
        /// Updates an existing job listing in the JSON file.
        /// </summary>
        /// <param name="updatedJob">The JobListing object with updated information.</param>
        public virtual void UpdateJob(JobListing updatedJob)
        {
            // Validate input parameter
            if (updatedJob == null)
                throw new ArgumentNullException(nameof(updatedJob));

            // 1. Read existing jobs
            // Collection to hold all job listings read from the file.
            List<JobListing> allJobs;
            // Variable to store the resolved file name for reading.
            string resolvedJsonFileName;
            try
            {
                resolvedJsonFileName = JsonFileName; // Attempt to resolve path first
            }
            catch (Exception ex) // Catch issues from JsonFileName resolution
            {
                System.Diagnostics.Debug.WriteLine($"ERROR (Generic) during path resolution for UpdateJob: {ex.Message}");
                return; // Cannot proceed if we can't resolve the path
            }

            // If path resolution succeeded, proceed to file operations
            try
            {
                using (var jsonFileReader = File.OpenText(resolvedJsonFileName))
                {
                    allJobs = JsonSerializer.Deserialize<List<JobListing>>(jsonFileReader.ReadToEnd(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<JobListing>();
                }
            }
            catch (ArgumentException argEx) // Handle if resolvedJsonFileName is invalid for File.OpenText
            {
                // Log the argument exception.
                System.Diagnostics.Debug.WriteLine($"ERROR (ArgumentException) opening/reading {resolvedJsonFileName} in UpdateJob: {argEx.Message}");
                return;
            }
            catch (FileNotFoundException)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR: Job data file not found at {resolvedJsonFileName} during UpdateJob.");
                return;
            }
            catch (Exception ex) when (ex is JsonException || ex is IOException)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR (Json/IO) reading {resolvedJsonFileName} in UpdateJob: {ex.Message}");
                return;
            }
            // Generic catch for path resolution is now separate.

            // 2. Find the index of the job to update
            // Assuming updatedJob.Id is valid and exists
            // Index of the job to be updated in the list.
            var jobIndex = allJobs.FindIndex(j => j.Id == updatedJob.Id);

            if (jobIndex == -1)
            {
                // Job not found, cannot update. Log error or throw.
                System.Diagnostics.Debug.WriteLine($"ERROR: Job with ID {updatedJob.Id} not found for update.");
                // Consider throwing an exception
                return; // Or throw new KeyNotFoundException($"Job with ID {updatedJob.Id} not found.");
            }

            // 3. Replace the job in the list
            // Preserve the original DatePosted
            updatedJob.DatePosted = allJobs[jobIndex].DatePosted;
            // Update the job with all new data including EmployerName
            allJobs[jobIndex] = updatedJob;

            // 4. Write the updated list back to the JSON file
            try
            {
                // Note: resolvedJsonFileName is from the successful read path resolution
                // Resolve the file name again inside the try block for the write operation.
                string currentJsonFileNameForWrite = JsonFileName; // Resolve path inside try for write
                // Use File.Create to overwrite the existing file.
                using (var outputStream = File.Create(currentJsonFileNameForWrite))
                {
                    // Serialize the updated list of jobs to the file stream.
                    JsonSerializer.Serialize<List<JobListing>>(
                        new Utf8JsonWriter(outputStream, new JsonWriterOptions
                        {
                            SkipValidation = true,
                            Indented = true
                        }),
                        allJobs
                    );
                }
            }
            catch (Exception ex)
            {
                // Use a placeholder or the variable that attempted the write if JsonFileName itself could be problematic
                System.Diagnostics.Debug.WriteLine($"ERROR writing jobs.json in UpdateJob: {ex.Message}");
                throw; // Re-throw to indicate failure
            }
        }
        /// <summary>
        /// Deletes a job listing by its ID from the JSON file.
        /// </summary>
        /// <param name="id">The ID of the job listing to delete.</param>
        public virtual void DeleteJob(string id)
        {
            // 1. Read existing jobs
            // Collection to hold all job listings read from the file.
            List<JobListing> allJobs;
            // Variable to store the resolved file name for reading.
            string resolvedJsonFileName;
            try
            {
                resolvedJsonFileName = JsonFileName; // Attempt to resolve path first
            }
            catch (Exception ex) // Catch issues from JsonFileName resolution
            {
                System.Diagnostics.Debug.WriteLine($"ERROR (Generic) during path resolution for DeleteJob: {ex.Message}");
                return; // Cannot proceed if we can't resolve the path
            }

            // If path resolution succeeded, proceed to file operations
            try
            {
                using (var jsonFileReader = File.OpenText(resolvedJsonFileName))
                {
                    allJobs = JsonSerializer.Deserialize<List<JobListing>>(jsonFileReader.ReadToEnd(),
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<JobListing>();
                }
            }
            catch (ArgumentException argEx) // Handle if resolvedJsonFileName is invalid for File.OpenText
            {
                // Log the argument exception.
                System.Diagnostics.Debug.WriteLine($"ERROR (ArgumentException) opening/reading {resolvedJsonFileName} in DeleteJob: {argEx.Message}");
                return;
            }
            catch (FileNotFoundException)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR: Job data file not found at {resolvedJsonFileName} during DeleteJob.");
                return;
            }
            catch (Exception ex) when (ex is JsonException || ex is IOException)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR (Json/IO) reading {resolvedJsonFileName} in DeleteJob: {ex.Message}");
                return;
            }
            // Generic catch for path resolution is now separate.

            // 2. Find the job to delete
            // The job listing to be removed.
            var jobToRemove = allJobs.FirstOrDefault(j => j.Id == id);

            if (jobToRemove == null)
            {
                // Job not found, nothing to delete. Log or handle as needed.
                System.Diagnostics.Debug.WriteLine($"INFO: Job with ID {id} not found for deletion.");
                return;
            }

            // 3. Remove the job from the list
            allJobs.Remove(jobToRemove);

            // 4. Write the updated list back to the JSON file
            try
            {
                // Note: resolvedJsonFileName is from the successful read path resolution
                // Resolve the file name again inside the try block for the write operation.
                string currentJsonFileNameForWrite = JsonFileName; // Resolve path inside try for write
                // Use File.Create to overwrite the existing file.
                using (var outputStream = File.Create(currentJsonFileNameForWrite))
                {
                    // Serialize the updated list of jobs to the file stream.
                    JsonSerializer.Serialize<List<JobListing>>(
                        new Utf8JsonWriter(outputStream, new JsonWriterOptions
                        {
                            SkipValidation = true,
                            Indented = true
                        }),
                        allJobs
                    );
                }
            }
            catch (Exception ex)
            {
                // Use a placeholder or the variable that attempted the write if JsonFileName itself could be problematic
                System.Diagnostics.Debug.WriteLine($"ERROR writing jobs.json in DeleteJob: {ex.Message}");
                throw; // Re-throw to indicate failure
            }
        }
        // Optional: You can add Create, Update, Delete methods here (CRUD)
    }
}