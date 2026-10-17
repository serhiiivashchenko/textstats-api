using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

public class SmokeTests : IAsyncLifetime
{
    private IPlaywright _playwright = null!;
    private IBrowser _browser = null!;
    private IPage _page = null!;

    public async Task InitializeAsync()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(
            new BrowserTypeLaunchOptions
            {
                Headless = true
            });
        _page = await _browser.NewPageAsync();
    }

    public async Task DisposeAsync()
    {
        if (_browser is not null)
        {
            await _browser.DisposeAsync();
        }
        _playwright.Dispose();
    }

    [Fact]
    public async Task ImproveText_ShouldDisplayImprovedText()
    {
        var baseUrl = Environment.GetEnvironmentVariable("E2E_BASE_URL") ?? "http://localhost:8082";
        await _page.GotoAsync(baseUrl);

        await _page.GetByPlaceholder("Enter text to improve...").FillAsync("He go to school yesterday.");
        await _page.GetByRole(AriaRole.Button, new() { Name = "Improve text" }).ClickAsync();

        await Expect(_page.GetByText("This is an improved text.")).ToBeVisibleAsync();
    }
}