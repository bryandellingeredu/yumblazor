using Microsoft.Playwright;
using NUnit.Framework;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace YumBlazor.Tests.UI
{


    [TestFixture]
    public sealed class HomePageTests
    {
        private Process? _appProcess;
        private IPlaywright? _playwright;
        private IBrowser? _browser;
        private IPage? _page;
        private IPage Page => _page ?? throw new InvalidOperationException("Page is not initialized.");

        [SetUp]
        public async Task Setup()
        {
            StartApp();

            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false
            });
            _page = await _browser.NewPageAsync();

            _page.Console += (_, msg) =>
                Console.WriteLine($"[BrowserConsole] {msg.Type}: {msg.Text}");
        }

        [TearDown]
        public async Task TearDown()
        {
            if (_page is not null)
                await _page.CloseAsync();
            if (_browser is not null)
                await _browser.CloseAsync();
            if (_playwright is not null)
                _playwright.Dispose();

            StopApp();
        }



       [Test]
        public async Task HomePage_ShouldLoad()
        {
            await GotoHomePageAsync();

            await _page!.ScreenshotAsync(new PageScreenshotOptions
            {
                Path = $"HomePage_ShouldLoad_{DateTime.Now:yyyyMMdd_HHmmss}.png"
            });
            var title = await _page!.TitleAsync();
            NUnit.Framework.TestContext.Progress.WriteLine($"Page title is: {title}");
            NUnit.Framework.Assert.That(title, Is.EqualTo("Home"), "Page title should be 'Home'");
        }

        [Test]
        public async Task Check_Test_Search()
        {
            await GotoHomePageAsync();

            await Page.GetByLabel("Search for Food Items!").FillAsync("Jalebi");
            await Page.WaitForTimeoutAsync(500);
            var cards = await Page.Locator("div.mud-card").AllAsync();
            NUnit.Framework.TestContext.Progress.WriteLine($"Found {cards.Count} product card(s).");
            NUnit.Framework.Assert.That(cards.Count, Is.EqualTo(1), "Should show exactly 1 product card for 'Jalebi'");
            await Page.ScreenshotAsync(new PageScreenshotOptions
            {
                Path = $"Check_Test_Search_{DateTime.Now:yyyyMMdd_HHmmss}.png"
            });


        }


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

        private async Task GotoHomePageAsync()
        {
            if (_page is null)
                throw new InvalidOperationException("Page is not initialized.");

            await _page.GotoAsync("https://localhost:7132/", new PageGotoOptions
            {
                WaitUntil = WaitUntilState.NetworkIdle
            });
        }
    }
}