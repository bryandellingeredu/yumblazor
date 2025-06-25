using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Playwright;
using NUnit.Framework;

namespace YumBlazor.Tests.UI
{
    [TestFixture]
    public sealed class HomePageTests
    {
        private Process? _appProcess;

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
            if (_appProcess != null && !_appProcess.HasExited)
            {
                _appProcess.Kill(entireProcessTree: true);
                _appProcess.Dispose();
            }
        }

        [Test]
        public async Task HomePage_ShouldLoad()
        {
            using var playwright = await Playwright.CreateAsync();

            var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false // run visible for now
            });

            var page = await browser.NewPageAsync();

            // log browser console messages
            page.Console += (_, msg) => Console.WriteLine($"[BrowserConsole] {msg.Type}: {msg.Text}");

            try
            {
                await page.GotoAsync("https://localhost:7132/", new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.NetworkIdle
                });

                await page.ScreenshotAsync(new PageScreenshotOptions
                {
                    Path = $"HomePage_ShouldLoad_{DateTime.Now:yyyyMMdd_HHmmss}.png"
                });
            }
            catch (Exception ex)
            {
                await page.ScreenshotAsync(new PageScreenshotOptions
                {
                    Path = $"Error_{DateTime.Now:yyyyMMdd_HHmmss}.png"
                });

                Console.WriteLine($"[ERROR] Test failed: {ex.Message}");
                throw;
            }
            finally
            {
                await browser.CloseAsync();
            }
        }
    }
}