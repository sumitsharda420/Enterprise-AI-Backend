namespace DotNetAiChat.Services
{
    using UglyToad.PdfPig;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Wordprocessing;
    using System.Text;

    public class DocumentTextExtractor
    {
        public string ExtractText(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName).ToLower();

            return ext switch
            {
                ".pdf" => ExtractPdf(file),
                ".docx" => ExtractWord(file),
                _ => throw new NotSupportedException("Only PDF and DOCX supported")
            };
        }

        private string ExtractPdf(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            using var pdf = PdfDocument.Open(stream);

            var sb = new StringBuilder();
            foreach (var page in pdf.GetPages())
                sb.AppendLine(page.Text);

            return sb.ToString();
        }

        private string ExtractWord(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            using var doc = WordprocessingDocument.Open(stream, false);

            var sb = new StringBuilder();
            foreach (var text in doc.MainDocumentPart!.Document.Descendants<Text>())
                sb.AppendLine(text.Text);

            return sb.ToString();
        }
    }
}
