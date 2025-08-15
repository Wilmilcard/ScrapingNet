using System;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

string url = "https://listado.mercadolibre.com.co/memorias-usb#D[A:memorias%20usb]";

using HttpClient client = new HttpClient();
string html = await client.GetStringAsync(url);

HtmlDocument doc = new HtmlDocument();
doc.LoadHtml(html);

var productos = doc.DocumentNode.SelectNodes("//li[contains(@class, 'ui-search-layout__item')]");

var listaProductos = new List<(string Nombre, decimal Precio)>();

if (productos != null)
{
    foreach (var producto in productos)
    {
        var nombreNode = producto.SelectSingleNode(".//h3[contains(@class,'poly-component__title-wrapper')]/a");
        string nombre = nombreNode?.InnerText.Trim() ?? "Sin nombre";

        var precioNode = producto.SelectSingleNode(".//div[contains(@class,'poly-price__current')]//span[contains(@class,'andes-money-amount__fraction')]");
        string precioTexto = precioNode?.InnerText.Trim().Replace(".", "") ?? "0";

        if (decimal.TryParse(precioTexto, out decimal precio))
        {
            listaProductos.Add((nombre, precio));
        }

        Console.WriteLine($"Nombre: {nombre}");
        Console.WriteLine($"Precio: {precioTexto}");
        Console.WriteLine(new string('-', 40));
    }

    if (listaProductos.Any())
    {
        var masBarato = listaProductos.Where(x => x.Precio > 0).OrderBy(p => p.Precio).First();
        Console.WriteLine($"Producto más barato:\n\n{masBarato.Nombre}\nPrecio: ${masBarato.Precio:N0}");
        Console.ReadKey();
    }
}
else
{
    Console.WriteLine("No se encontraron productos.");
}

/*
 (() => {
    const elements = document.querySelectorAll('h3.poly-component__title-wrapper');
    const res = [];
    
    for(let i = 0; i < elements.length; i++)
        res.push(elements[i].innerText);
    
    return res;
})();
 */

