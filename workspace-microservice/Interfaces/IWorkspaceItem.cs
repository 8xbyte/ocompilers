namespace WorkspaceMicroservice.Interfaces {
    public class IWorkspaceItem {
        public string Name { get; set; }
        public string Type { get; set; }
        public string? Content { get; set; }
        public IEnumerable<IWorkspaceItem>? Items { get; set; }
    }
}
