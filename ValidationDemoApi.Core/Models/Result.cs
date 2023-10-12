namespace ValidationDemoApi.CORE.Models
{
    public class Result
    {
        public Dictionary<string, List<string>> Errors { get; set; } = new Dictionary<string, List<string>>();
        public bool Success => Errors.Count == 0;

    }
    public class Result<T> : Result
    {
        public T Data { get; set; }
    }
}
