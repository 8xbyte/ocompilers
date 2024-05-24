namespace WorkspaceMicroservice.Interfaces {
    public class IDeleteFileHttpRequest {
        public int WorkspaceId { get; set; }
        public string Path { get; set; }
    }

    public class IDeleteFileHttpResponse {
        public string Path { get; set; }
    }
}
