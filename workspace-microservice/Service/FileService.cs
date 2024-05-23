namespace WorkspaceMicroservice.Service {
    public interface IFileService {
        public bool CreateFile(string path);
        public string ReadFile(string path);
        public void WriteFile(string path, string content);
        public bool DeleteFile(string path);
    }
    public class FileService : IFileService {
        public bool CreateFile(string path) {
            if (!File.Exists(path)) {
                File.Create(path);
                return true;
            }
            return false;
        }

        public string ReadFile(string path) {
            return File.ReadAllText(path);
        }

        public void WriteFile(string path, string content) {
            File.WriteAllText(path, content);
        }

        public bool DeleteFile(string path) {
            if (File.Exists(path)) {
                File.Delete(path);
                return true;
            }
            return false;
        }
    }
}
