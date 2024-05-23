using Shared.Services;
using System.Reflection.Metadata.Ecma335;

namespace CppCompilerMicroservice.Services {
    public interface IRuntimeService {
        public Task<int> RunAsync(string path, bool isDebug, ProcessConfig config);
        public Task<int> RunReleaseAsync(string path, ProcessConfig config);
        public Task<int> RunDebugAsync(string path, ProcessConfig config);
    }
    public class RuntimeService(IProcessService processService) : IRuntimeService {
        private readonly IProcessService _processService = processService;

        public async Task<int> RunAsync(string path, bool isDebug, ProcessConfig config) {
            if (isDebug) {
                return await RunDebugAsync(path, config);
            } else {
                return await RunReleaseAsync(path, config);
            }
        }

        public async Task<int> RunReleaseAsync(string path, ProcessConfig config) {
            var processExitCode = await _processService.RunAsync("bash", new ProcessConfig {
                Arguments = $"-c \"timeout --kill-after=30s 30s docker run -i --rm -v ./{path}:/app -m 100m --cpu-period=100000 --cpu-quota=5000 cpp/runtime\"",
                StartAction = config.StartAction,
                OutputAction = config.OutputAction,
                ErrorAction = config.ErrorAction,
                InputAction = config.InputAction,
                EndAction = config.EndAction
            });

            return processExitCode;
        }
        public async Task<int> RunDebugAsync(string path, ProcessConfig config) {
            var processExitCode = await _processService.RunAsync("bash", new ProcessConfig {
                Arguments = $"-c \"timeout --kill-after=60s 60s docker run -i --rm -v ./{path}:/app -m 100m --cpu-period=100000 --cpu-quota=5000 cpp/runtime/debug\"",
                StartAction = config.StartAction,
                OutputAction = config.OutputAction,
                ErrorAction = config.ErrorAction,
                InputAction = config.InputAction,
                EndAction = config.EndAction
            });

            return processExitCode;
        }
    }
}
