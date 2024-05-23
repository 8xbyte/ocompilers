namespace Shared.Interfaces.Options {
    public class ICppCompilerOptions : IBaseOptions {
        public string Directory { get; set; }
        public IDatabaseOptions Database { get; set; }
    }
}
