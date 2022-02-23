//using SelectPdf;
//using System.IO;
//using System.Threading.Tasks;

//namespace ButlerLee.API.Helpers
//{
//    public class PDFHelper
//    {
//        ///<summary>
//        /// PDF 파일 출력
//        /// </summary>
//        /// <param name="html">HTML로 변환된 결재문서</param>        
//        public async Task<byte[]> ExportPDFFrom(string html)
//        {
//            byte[] htmlToPdf = new byte[16 * 1024];

//            string body = string.Empty;

//            await Task.Run(() =>
//            {
//                using (MemoryStream stream = new MemoryStream())
//                {
//                    HtmlToPdf converter = new HtmlToPdf();

//                    converter.Options.PdfPageSize = PdfPageSize.A4;
//                    converter.Options.MarginTop = 30;
//                    converter.Options.MarginBottom = 30;

//                    PdfDocument doc = new PdfDocument();

//                    doc = converter.ConvertHtmlString(html);
//                    doc.Save(stream);
//                    doc.Close();

//                    htmlToPdf = stream.ToArray();
//                }
//            });

//            return htmlToPdf;
//        }
//    }
//}
