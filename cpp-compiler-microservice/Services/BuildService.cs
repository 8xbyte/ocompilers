using Shared.Services;
using System.IO;

namespace CppCompilerMicroservice.Services {
    public interface IBuildService {
        public Task BuildAsync(string path, bool isDebug, ProcessConfig config);
        public Task BuildReleaseAsync(string path, ProcessConfig config);
        public Task BuildDebugAsync(string path, ProcessConfig config);
    }
    public class BuildService(IProcessService processService) : IBuildService {
        private readonly IProcessService _processService = processService;

        public async Task BuildAsync(string path, bool isDebug, ProcessConfig config) {
            if (isDebug) {
                await BuildDebugAsync(path, config);
            } else {
                await BuildReleaseAsync(path, config);
            }
        }
        public async Task BuildReleaseAsync(string path, ProcessConfig config) {
            await _processService.RunAsync("bash", new ProcessConfig {
                Arguments = $"-c \"g++ -w ./{path}/*.cpp -o {path}/a.out\"",
                StartAction = config.StartAction,
                OutputAction = config.OutputAction,
                ErrorAction = config.ErrorAction,
                InputAction = config.InputAction,
                EndAction = config.EndAction
            });
        }
        public async Task BuildDebugAsync(string path, ProcessConfig config) {
            await _processService.RunAsync("bash", new ProcessConfig {
                Arguments = $"-c \"g++ -w -g ./{path}/*.cpp -o {path}/a.out\"",
                StartAction = config.StartAction,
                OutputAction = config.OutputAction,
                ErrorAction = config.ErrorAction,
                InputAction = config.InputAction,
                EndAction = config.EndAction
            });
        }
    }
}
