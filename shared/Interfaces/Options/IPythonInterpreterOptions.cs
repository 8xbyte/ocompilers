namespace Shared.Interfaces.Options {
    public class IPythonInterpreterOptions : IBaseOptions {
        public string Directory { get; set; }
        public IDatabaseOptions Database { get; set; }
    }
}
