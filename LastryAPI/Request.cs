namespace LastryAPI
{
    public class Request
    {
        public int requestId { get; set; }

        public string requestDetails { get; set; } = string.Empty;

        public string requestStatus { get; set; } = "Pending";

        public int creatorId { get; set; }

        public string systemreq { get; set; } = string.Empty;


    }
}
