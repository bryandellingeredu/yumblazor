using Microsoft.Playwright;
using NUnit.Framework;


namespace YumBlazor.Tests.UI
{
    [TestFixture]
    public sealed class EmailPageTest : BaseTest
    {
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
                Headless = false,
                SlowMo = 500
            });

            var context = await _browser.NewContextAsync(new BrowserNewContextOptions
            {
                RecordVideoDir = "videos",
                RecordVideoSize = new() { Width = 1280, Height = 720 }
            });

            await context.Tracing.StartAsync(new()
            {
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });

            _page = await context.NewPageAsync();

            _page.Console += (_, msg) =>
                Console.WriteLine($"[BrowserConsole] {msg.Type}: {msg.Text}");
        }


        [TearDown]
        public async Task TearDown()
        {
            if (_page is not null)
            {
                // Stop trace
                var tracePath = $"trace_{DateTime.Now:yyyyMMdd_HHmmss}.zip";
                await _page.Context.Tracing.StopAsync(new() { Path = tracePath });

                // Save video
                var videoPath = await _page.Video.PathAsync();
                Console.WriteLine($"[INFO] Video saved to: {videoPath}");

                await _page.CloseAsync();
            }

            if (_browser is not null)
                await _browser.CloseAsync();

            _playwright?.Dispose();
            StopApp();
        }

        [Test]
        public async Task EmailPage_ShouldSendEmail_Success()
        {
            await GotoEmailPageAsync();

            // Type Hello World into the RadzenHtmlEditor
            await Page.Locator("div.rz-html-editor-content[contenteditable]").FillAsync("Hello World");

            // Click Send Test Email button
            await Page.ClickAsync("button:has-text(\"Send Test Email\")");

            // Locators for success and error snackbar
            var successLocator = Page.Locator("div.mud-snackbar:has-text(\"Email sent successfully!\")");
            var errorLocator = Page.Locator("div.mud-snackbar:has-text(\"Error sending email:\")");

            // Wait for either success or error — whichever happens first
            var successTask = successLocator.WaitForAsync(new() { Timeout = 5000 });
            var errorTask = errorLocator.WaitForAsync(new() { Timeout = 5000 });

            var completedTask = await Task.WhenAny(successTask, errorTask);

            // Screenshot for debugging
            await Page.ScreenshotAsync(new PageScreenshotOptions
            {
                Path = $"EmailPage_SendEmail_{DateTime.Now:yyyyMMdd_HHmmss}.png"
            });

            if (completedTask == errorTask)
            {
                NUnit.Framework.Assert.Fail("Error snackbar appeared — email send failed.");
            }
            else if (completedTask == successTask)
            {
                NUnit.Framework.TestContext.Progress.WriteLine("Success snackbar appeared — email send passed.");
            }
            else
            {
                NUnit.Framework.Assert.Fail("Neither success nor error snackbar appeared.");
            }
        }


        private async Task GotoEmailPageAsync()
        {
            if (_page is null)
                throw new InvalidOperationException("Page is not initialized.");

            await _page.GotoAsync("https://localhost:7132/", new PageGotoOptions
            {
                WaitUntil = WaitUntilState.NetworkIdle
            });

            await _page.ClickAsync("a:has-text(\"Test Email\")");

            await _page.WaitForSelectorAsync("h5:has-text(\"Test Email Page\")");


        }
    }
}