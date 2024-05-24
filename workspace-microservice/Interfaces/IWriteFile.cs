namespace WorkspaceMicroservice.Interfaces {
    public class IWriteFileHttpRequest {
        public int WorkspaceId { get; set; }
        public string Path { get; set; }
        public string Content { get; set; }
    }

    public class IWriteFileHttpResponse {
        public string Path { get; set; }
    }
}
