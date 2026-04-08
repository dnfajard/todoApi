namespace TodoApi.DTOs
{
    public class TodoRequest
    {
        public string Text { get; set; } = string.Empty;
        public bool? Completed { get; set; }
    }
}