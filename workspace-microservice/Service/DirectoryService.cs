﻿namespace WorkspaceMicroservice.Service {
    public interface IDirectoryService {
        public bool CreateDirectory(string path);
        public long GetDirectorySize(string path);
        public bool DeleteDirectory(string path);
    }
    public class DirectoryService : IDirectoryService {
        public bool CreateDirectory(string path) {
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
                return true;
            }
            return false;
        }

        public long GetDirectorySize(string path) {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            return directoryInfo.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length);
        }

        public bool DeleteDirectory(string path) {
            if (Directory.Exists(path)) {
                Directory.Delete(path);
                return true;
            }
            return false;
        }
    }
}
