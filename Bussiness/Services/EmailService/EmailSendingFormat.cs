   namespace Internship.Business.Services.EmailService
{
    public class EmailSendingFormat
    {
        public required string Information { get; set; }
        public required string Subject { get; set; } = "";
        public required string customer { get; set; }
    }
}