using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Application.Dtos
{
    public class CustomerEnquiryDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public string CustomerContactEmail { get; set; } = string.Empty;

        public IFormFile? Attachment { get; set; }
    }
}
