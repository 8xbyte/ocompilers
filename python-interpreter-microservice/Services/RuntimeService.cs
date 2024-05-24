using Shared.Services;

namespace PythonInterpreterMicroservice.Services {
    public interface IRuntimeService {
        public Task<int> RunAsync(string path, string executableFile, bool isDebug, ProcessConfig config);
        public Task<int> RunReleaseAsync(string path, string executableFile, ProcessConfig config);
        public Task<int> RunDebugAsync(string path, string executableFile, ProcessConfig config);
    }

    public class RuntimeService(IProcessService processService) : IRuntimeService {
        private readonly IProcessService _processService = processService;

        public async Task<int> RunAsync(string path, string executableFile, bool isDebug, ProcessConfig config) {
            if (isDebug) {
                return await RunDebugAsync(path, executableFile, config);
            } else {
                return await RunReleaseAsync(path, executableFile, config);
            }
        }
        public async Task<int> RunReleaseAsync(string path, string executableFile, ProcessConfig config) {
            var processExitCode = await _processService.RunAsync("bash", new ProcessConfig {
                Arguments = $"-c \"timeout --kill-after=30s 30s docker run -i --rm -v ./{path}:/app -m 100m --cpu-period=100000 --cpu-quota=5000 python/runtime {executableFile}\"",
                StartAction = config.StartAction,
                OutputAction = config.OutputAction,
                ErrorAction = config.ErrorAction,
                InputAction = config.InputAction,
                EndAction = config.EndAction
            });

            return processExitCode;
        }
        public async Task<int> RunDebugAsync(string path, string executableFile, ProcessConfig config) {
            var processExitCode = await _processService.RunAsync("bash", new ProcessConfig {
                Arguments = $"-c \"timeout --kill-after=30s 30s docker run -i --rm -v ./{path}:/app -m 100m --cpu-period=100000 --cpu-quota=5000 python/runtime/debug {executableFile}\"",
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
