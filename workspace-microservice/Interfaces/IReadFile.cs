namespace WorkspaceMicroservice.Interfaces {
    public class IReadFileHttpRequest {
        public int WorkspaceId { get; set; }
        public string Path { get; set; }
    }

    public class IReadFileHttpResponse {
        public string Path { get; set; }
        public string Content { get; set; }
    }
}
