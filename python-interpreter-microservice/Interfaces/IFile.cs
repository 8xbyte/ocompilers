namespace PythonInterpreterMicroservice.Interfaces {
    public class IFile {
        public bool IsExecutable { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
    }
}
