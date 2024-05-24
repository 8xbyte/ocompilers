namespace WorkspaceMicroservice.Interfaces {
    public class ICreateDirectoryHttpRequest {
        public int WorkspaceId { get; set; }
        public string Path { get; set; }
    }

    public class ICreateDirectoryHttpResponse {
        public string Path { get; set; }
    }
}
