using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CampusConnect.WebSite.Models
{
    /// <summary>
    /// Represents an employer with details such as name, vision, and tech stack.
    /// </summary>
    public class Employer
    {
        /// <summary>
        /// Gets or sets the name of the employer.
        /// </summary>
        [Required]
        [JsonPropertyName("employer_name")]
        public string? EmployerName { get; set; }

        /// <summary>
        /// Gets or sets the company's vision.
        /// </summary>
        [Required]
        [JsonPropertyName("company_vision")]
        public string? CompanyVision { get; set; }

        /// <summary>
        /// Gets or sets the company's tech stack.
        /// </summary>
        [Required]
        [JsonPropertyName("tech_stack")]
        public string? TechStack { get; set; }
    }
}