using PuppeteerSharp;

string url = "https://listado.mercadolibre.com.co/memorias-usb#D[A:memorias%20usb]";
string chrome = @"C:\Program Files\Google\Chrome\Application\chrome.exe";

var browserFetcher = new BrowserFetcher();
await browserFetcher.DownloadAsync();

await using var browser = await Puppeteer.LaunchAsync(
    new LaunchOptions
    {
        Headless = true,
        ExecutablePath = chrome
    });

await using var page = await browser.NewPageAsync();

await page.GoToAsync(url);

var titles = new List<string>();
var result = await page.EvaluateFunctionAsync<string[]>("()=>{" +
    "const a = document.querySelectorAll('h2.poly-box.poly-component__title');" +
    "const res = [];" +
    "for(let i = 0; i < a.length; i++)" +
    "   res.push(a[i].innerText);" +
    "return res;" +
    "}");

foreach (var e in result)
    Console.WriteLine(e);

