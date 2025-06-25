using Microsoft.Playwright;
using NUnit.Framework;


namespace YumBlazor.Tests.UI
{
    [TestFixture]
    public sealed class HomePageTests : BaseTest
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