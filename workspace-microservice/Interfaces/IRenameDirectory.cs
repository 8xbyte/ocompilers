namespace WorkspaceMicroservice.Interfaces {
    public class IRenameDirectoryHttpRequest {
        public int WorkspaceId { get; set; }
        public string OldPath { get; set; }
        public string NewPath { get; set; }
    }

    public class IRenameDirectoryHttpResponse {
        public string Path { get; set; }
    }
}
