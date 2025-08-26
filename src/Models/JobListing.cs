using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CampusConnect.WebSite.Models
{
    /// <summary>
    /// Represents a job listing in the CampusConnect application.
    /// </summary>
    public class JobListing
    {
        [JsonPropertyName("id")]
        // Unique identifier for the job listing.
        public string? Id { get; set; }

        [Required]
        [JsonPropertyName("title")]
        // The title of the job posting.
        public string? Title { get; set; }

        [Required]
        [JsonPropertyName("employer_name")]
        /// <summary>
        /// Gets or sets the name of the employer offering the job.
        /// </summary>
        public string? EmployerName { get; set; }

        [Required]
        [JsonPropertyName("description")]
        // A detailed description of the job responsibilities and requirements.
        public string? Description { get; set; }

        [Required]
        [JsonPropertyName("location")]
        // The location of the job (e.g., city, state, remote).
        public string? Location { get; set; }

        [Required]
        [Url]
        [JsonPropertyName("apply_url")]
        // The URL where candidates can apply for the job.
        public string? ApplyUrl { get; set; }

        [JsonPropertyName("date_posted")]
        // The date and time the job was posted.
        public System.DateTime DatePosted { get; set; }

        [Required]
        [JsonPropertyName("job_type")]
        // The type of job (e.g., Full-time, Part-time, Internship, Contract).
        public string? JobType { get; set; }

        // Consider adding other relevant properties later, e.g., JobType (Full-time/Internship)

        /// <summary>
        /// Returns a JSON representation of the JobListing object.
        /// </summary>
        public override string ToString() => JsonSerializer.Serialize<JobListing>(this);
    }
}