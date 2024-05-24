namespace WorkspaceMicroservice.Service {
    public interface IFileService {
        public bool CreateFile(string path);
        public string? ReadFile(string path);
        public bool RenameFile(string oldPath, string newPath);
        public bool WriteFile(string path, string content);
        public bool DeleteFile(string path);
    }
    public class FileService : IFileService {
        public bool CreateFile(string path) {
            if (!File.Exists(path)) {
                File.Create(path).Close();
                return true;
            }
            return false;
        }

        public string? ReadFile(string path) {
            if (File.Exists(path)) {
                return File.ReadAllText(path);
            }
            return null;
        }

        public bool RenameFile(string oldPath, string newPath) {
            if (File.Exists(oldPath) && !File.Exists(newPath)) {
                File.Move(oldPath, newPath);
                return true;
            }
            return false;
        }

        public bool WriteFile(string path, string content) {
            if (File.Exists(path)) {
                File.WriteAllText(path, content);
                return true;
            }
            return false;
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
