using Microsoft.AspNetCore.Mvc;
using Syncfusion.Pdf;
using Syncfusion.HtmlConverter;
using System.IO;
using System;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult TarefaImprimir()
        {
            return View();
        }

        [HttpGet]
        public IActionResult TarefaPdf()
        {
            //https://help.syncfusion.com/file-formats/pdf/convert-html-to-pdf/webkit
            //https://www.syncfusion.com/kb/10363/how-to-convert-html-to-pdf-in-blazor-using-c
            //Copiar a pasta QtBinariesWindows, que está dentro da pasta do pacote instalado, para o C:/

            //Initialize HTML converter with WebKit rendering engine
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();

            WebKitConverterSettings webKitSettings = new WebKitConverterSettings();

            //Set WebKit path
            //webKitSettings.WebKitPath = @"\QtBinariesDotNetCore\";
            webKitSettings.WebKitPath = @"\QtBinariesWindows\";

            //Enable JavaScript 
            webKitSettings.EnableJavaScript = true;

            //Enable Repeat header/footer
            webKitSettings.EnableRepeatTableHeader = true;
            webKitSettings.EnableRepeatTableFooter = true;

            //Disable split images and text lines
            webKitSettings.SplitImages = false;
            webKitSettings.SplitTextLines = false;

            webKitSettings.Margin = new Syncfusion.Pdf.Graphics.PdfMargins { Top = 40, Left = 30, Right = 40, Bottom = 50 };

            //Set additional delay; units in milliseconds;
            webKitSettings.AdditionalDelay = 3000;

            //Assign WebKit settings to HTML converter
            htmlConverter.ConverterSettings = webKitSettings;

            //Get URL doc
            var url = (Request.Scheme ?? "http") + "://" + Request.Host.Value + @Url.Action("TarefaImprimir", "Home", values: null);

            //Convert URL to PDF
            PdfDocument document = htmlConverter.Convert(url);

            //Save the document into stream.
            MemoryStream stream = new MemoryStream();

            document.Save(stream);

            stream.Position = 0;

            //Close the document.
            document.Close(true);

            //Defining the ContentType for pdf file.
            string contentType = "application/pdf";

            //Define the file name.
            string fileName = " TASS.pdf";

            //Creates a FileContentResult object by using the file contents, content type, and file name.
            return File(stream, contentType, fileName);
        }
    }
}
