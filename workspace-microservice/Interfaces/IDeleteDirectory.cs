namespace WorkspaceMicroservice.Interfaces {
    public class IDeleteDirectoryHttpRequest {
        public int WorkspaceId { get; set; }
        public string Path { get; set; }
    }

    public class IDeleteDirectoryHttpResponse {
        public string Path { get; set; }
    }
}
