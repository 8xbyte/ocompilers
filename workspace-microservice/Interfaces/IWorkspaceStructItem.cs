namespace WorkspaceMicroservice.Interfaces {
    public class IWorkspaceStructItem {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public IEnumerable<IWorkspaceStructItem>? Items { get; set; }
    }
}
