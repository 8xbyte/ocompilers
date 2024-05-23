namespace WorkspaceMicroservice.Interfaces {
    public class IRenameWorkspaceHttpRequest {
        public int Id { get; set; }
        public string NewName { get; set; }
    }
}
