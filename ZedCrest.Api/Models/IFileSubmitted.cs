namespace ZedCrest.Api.Models
{
    public interface IFileSubmitted
    {
        string[] Files { get; set; }
        int UserId { get; set; }
    }
}