namespace SearchAPI.Models
{
    public class SearchHistory:BaseEntity
    {
        public string SearchKey { get; set; }
        public string SearchCondition {  get; set; }
        public DateTime SearchStarted { get; set; }
        public DateTime SearchCompleted { get; set; }
        public string SearchStatus { get; set; }
        public int ResultCount { get; set; }
        public string ErrorMessage { get; set; }
        public string RequestedIP { get; set; }
    }
}
