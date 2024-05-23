namespace Shared.Interfaces.Options {
    public class IWorkspaceOptions : IBaseOptions {
        public string Directory { get; set; }
        public int MaxWorkspaces { get; set; }
        public long MaxWorkspaceSize { get; set; }
        public IDatabaseOptions Database { get; set; }
    }
}
