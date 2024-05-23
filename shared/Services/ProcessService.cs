using System.Diagnostics;

namespace Shared.Services {
    public class ProcessConfig {
        public string? Arguments { get; set; }
        public Func<string, StreamWriter?, Task>? OutputAction { get; set; }
        public Func<string, StreamWriter?, Task>? ErrorAction { get; set; }
        public Func<StreamWriter, Task>? InputAction { get; set; }
        public Func<bool, Task>? StartAction { get; set; }
        public Func<int, Task>? EndAction { get; set; }
    }

    public interface IProcessService {
        public Task<int> RunAsync(string path, ProcessConfig? processConfig = null);
    }
    public class ProcessService : IProcessService {
        public async Task<int> RunAsync(string path, ProcessConfig? processConfig = null) {
            var processInfo = new ProcessStartInfo {
                FileName = path,
                Arguments = processConfig?.Arguments,
                RedirectStandardOutput = (processConfig?.OutputAction) != null,
                RedirectStandardError = (processConfig?.ErrorAction) != null,
                RedirectStandardInput = (processConfig?.InputAction) != null,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            using (var process = new Process { StartInfo = processInfo }) {
                if (processConfig?.OutputAction != null) {
                    process.OutputDataReceived += async (sender, e) => {
                        if (!string.IsNullOrEmpty(e.Data)) {
                            await processConfig.OutputAction(e.Data, (processConfig?.InputAction) != null ? process.StandardInput : null);
                        }
                    };
                }

                if (processConfig?.ErrorAction != null) {
                    process.ErrorDataReceived += async (sender, e) => {
                        if (!string.IsNullOrEmpty(e.Data)) {
                            await processConfig.ErrorAction(e.Data, (processConfig?.InputAction) != null ? process.StandardInput : null);
                        }
                    };
                }

                process.Start();

                if (processConfig?.StartAction != null) {
                    await processConfig.StartAction(!process.HasExited);
                }

                if (process.HasExited) {
                    return process.ExitCode;
                }

                if (processConfig?.OutputAction != null) {
                    process.BeginOutputReadLine();
                }
                if (processConfig?.ErrorAction != null) {
                    process.BeginErrorReadLine();
                }

                if (processConfig?.InputAction != null) {
                    using var inputStream = process.StandardInput;
                    while (!process.HasExited) {
                        try {
                            var inputActionTask = processConfig.InputAction(inputStream);
                            var endProcessTask = process.WaitForExitAsync();
                            await Task.WhenAny(inputActionTask, endProcessTask);
                        } catch {
                            break;
                        }
                    }
                } else {
                    await process.WaitForExitAsync();
                }

                if (processConfig?.EndAction != null) {
                    await processConfig.EndAction(process.ExitCode);
                }

                return process.ExitCode;
            }
        }
    }
}
