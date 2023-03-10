namespace CarRental.Domain.Entities
{
    public class Enquiry : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string CustomerContactEmail { get; set; } = string.Empty;
        public string AttachmentUrl { get; set; } = string.Empty;
    }
}
