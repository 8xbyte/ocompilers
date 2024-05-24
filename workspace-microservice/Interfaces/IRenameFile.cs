namespace WorkspaceMicroservice.Interfaces {
    public class IRenameFileHttpRequest {
        public int WorkspaceId { get; set; }
        public string OldPath { get; set; }
        public string NewPath { get; set; }
    }

    public class IRenameFileHttpResponse {
        public string Path { get; set; }
    }
}
