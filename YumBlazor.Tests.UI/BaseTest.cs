// BaseTest.cs
using System.Diagnostics;
using NUnit.Framework;

namespace YumBlazor.Tests.UI
{
    public abstract class BaseTest
    {
        protected Process? _appProcess;

        [SetUp]
        public void StartApp()
        {
            var solutionDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../"));
            var projectPath = Path.Combine(solutionDir, "YumBlazor", "YumBlazor.csproj");

            Console.WriteLine($"[INFO] Starting app from: {projectPath}");

            var startInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"run --project \"{projectPath}\" --urls=https://localhost:7132",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            _appProcess = Process.Start(startInfo);

            if (_appProcess == null)
            {
                throw new Exception("Failed to start YumBlazor app.");
            }

            var started = false;
            var timeout = DateTime.UtcNow.AddSeconds(30);

            while (!started && !_appProcess.HasExited)
            {
                var line = _appProcess.StandardOutput.ReadLine();
                if (line != null)
                {
                    Console.WriteLine($"[Blazor] {line}");

                    if (line.Contains("https://localhost:7132"))
                    {
                        started = true;
                    }
                }

                if (DateTime.UtcNow > timeout)
                {
                    throw new TimeoutException("Timed out waiting for YumBlazor to start.");
                }
            }

            Thread.Sleep(1000);
        }

        [TearDown]
        public void StopApp()
        {
            if (_appProcess is not null && !_appProcess.HasExited)
            {
                Console.WriteLine($"[INFO] Stopping app (PID {_appProcess.Id})...");
                _appProcess.Kill(entireProcessTree: true);
                _appProcess.Dispose();
                _appProcess = null;
            }
            else
            {
                Console.WriteLine("[INFO] App process was already stopped.");
            }
        }
    }
}
