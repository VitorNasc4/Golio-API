namespace Golio.Core.DTOs
{
    public class SuggestionVoteMessage
    {
        public int SuggestionId { get; set; }
        public bool IsValid { get; set; }
        public string EmailAutor { get; set; }
    }
}