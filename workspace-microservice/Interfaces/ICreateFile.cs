namespace WorkspaceMicroservice.Interfaces {
    public class ICreateFileHttpRequest {
        public int WorkspaceId { get; set; }
        public string Path { get; set; }
    }

    public class ICreateFileHttpResponse {
        public string Path { get; set; }
    }
}
