
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TCRD_GeneracionSentencia.Models;


namespace TCRD_GeneracionSentencia.Controllers
{
    public class SentenciasController : Controller
    {

        

        // GET: Sentencias
        public ActionResult Index()
        {

            var nombre = Session["nombre"] != null ? Session["nombre"].ToString() : "";

            if (!string.IsNullOrEmpty(nombre))
            {
                var sentencias = Helpper.SentenciaHelpper.GetSentencias();

                return View(sentencias);
            }
            else return RedirectToAction("Login", "Auth");


           

        }

        public ActionResult GenerarSentencia()
        {

            var nombre = Session["nombre"] != null ? Session["nombre"].ToString() : "";

            if (!string.IsNullOrEmpty(nombre))
            {

                ViewBag.Message = "Your contact page.";
                MemoryStream memoryStream = new MemoryStream();
                ViewBag.PdfStream = memoryStream;

                return View();
            }
            else return RedirectToAction("Login", "Auth");


        }

        public ActionResult DownloadWord(string wordBase64)
        {
            byte[] wordBytes = Convert.FromBase64String(wordBase64);
            return File(wordBytes, "application/msword", "sentencia.docx");
        }

        [HttpPost]
        public ActionResult GenerarSentencia(HttpPostedFileBase archivo, Sentencia sentencia)
        {
            if (ModelState.IsValid)
            {
                string resumen = string.Empty;
                Application wordApp = null;
                Document wordDoc = null;

                try
                {
                    string ultima_secuencia = Helpper.SentenciaHelpper.GetUltimaSecuencia();
                    sentencia.num_sentencia = ultima_secuencia;
                    List<Magistrado> magistrados = Helpper.MagistradoHelpper.GetMagistrados();
                    string referenciaSentencia = "TC/0000/23";
                    string referenciaExpediente = "TC-xxxxxx-xxxx";
                    string referenciaFecha = "a los _______________________________ (____) días del mes de __________________________________ del año dos mil veintitrés (2023).";


                    string wordFilePath = Path.Combine(Server.MapPath("~/App_Data"), archivo.FileName);
                    archivo.SaveAs(wordFilePath);

                    wordApp = new Application();
                    wordDoc = wordApp.Documents.Open(wordFilePath);

                    // Modificar fecha 
                    Find findObject = wordDoc.Content.Find;
                    findObject.MatchWholeWord = true;
                    findObject.Text = referenciaFecha;
                    findObject.Replacement.Text = Helpper.ConversionFecha.ConvertirFechaATexto(sentencia.fecha_sentencia.ToString().Split(' ')[0]);
                    findObject.Execute(Replace: WdReplace.wdReplaceAll);


                    // Modificar Sentencia
                    findObject.Text = referenciaSentencia;
                    findObject.Replacement.Text = ultima_secuencia;
                    findObject.Execute(Replace: WdReplace.wdReplaceAll);

                    // Modificar Expediente
                    findObject.Text = referenciaExpediente;
                    findObject.Replacement.Text = sentencia.num_expediente;
                    findObject.Execute(Replace: WdReplace.wdReplaceAll);


                    //ELIMINAR SECCION DE FIRMAS
                    Range startRange = wordDoc.Content;
                    startRange.Find.Text = "DISPONER su publicación en el Boletín del Tribunal Constitucional.";
                    startRange.Find.Execute();


                    if (startRange.Find.Found)
                    {
                        Range endRange = wordDoc.Content;
                        endRange.Find.Text = "La presente sentencia es dada y firmada por los señores jueces del Tribunal";
                        endRange.Find.Execute();

                        if (endRange.Find.Found)
                        {
                            Range sectionRange = wordDoc.Range(startRange.End, endRange.Start);
                            sectionRange.Delete();
                        }


                        Range emptyLineRange = wordDoc.Range(endRange.Start, endRange.Start);


                        //Insertar Firmas
                        string firmas = "\r\nFirmada: ";
                        foreach (var item in magistrados)
                        {
                            Range findRange = wordDoc.Content;
                            findRange.Find.Text = item.nombre_magistrado.Trim();
                            bool textoEncontrado = findRange.Find.Execute();
                            string signo = item.firma == "Secretaria" ? "." : "; ";
                            if (textoEncontrado)
                            {
                                firmas += item.nombre_magistrado + ", " + item.firma.ToLower() + signo;

                            }

                        }
                        firmas += "\r\n\r\n";
                        emptyLineRange.Text = firmas;

                    }

                    //Seccion del Tweet
                    Range startRange2 = wordDoc.Content;
                    startRange2.Find.Text = "II. ";
                    startRange2.Find.Execute();


                    if (startRange2.Find.Found)
                    {
                        Range endRange2 = wordDoc.Content;
                        endRange2.Find.Text = "III. ";
                        endRange2.Find.Execute();

                        if (endRange2.Find.Found)
                        {
                            Range sectionRange2 = wordDoc.Range(startRange2.End, endRange2.Start);
                            resumen = sectionRange2.Text;
                        }
                        else
                        {
                            Range sectionRange2 = wordDoc.Range(startRange2.End, wordDoc.Content.End);
                            resumen = sectionRange2.Text;

                        }

                    }

                    string selloImagePath = Server.MapPath("~/Content/images/GVR.jpg");

                    if (System.IO.File.Exists(selloImagePath))
                    {
                        Paragraph lastParagraph = null;
                        foreach (Paragraph paragraph in wordDoc.Paragraphs)
                        {
                            lastParagraph = paragraph;
                        }
                        Range lastParagraphRange = lastParagraph.Range;

                        InlineShape waterMarkShape = lastParagraphRange.InlineShapes.AddPicture(selloImagePath);
                        waterMarkShape.Width = 100;
                        waterMarkShape.Height = 100;

                        lastParagraph.Alignment = WdParagraphAlignment.wdAlignParagraphRight;

                    }

                    // Convertir el Word a PDF
                    string pdfFilePath = Path.ChangeExtension(wordFilePath, ".pdf");
                    wordDoc.ExportAsFixedFormat(pdfFilePath, WdExportFormat.wdExportFormatPDF);

                    // Guardar el Word en un archivo temporal
                    string tempWordFilePath = Path.Combine(Server.MapPath("~/App_Data"), "temp.docx");
                    wordDoc.SaveAs2(tempWordFilePath);

                    // Cerrar y liberar recursos de Word
                    wordDoc.Close();
                    wordApp.Quit();
                    Marshal.ReleaseComObject(wordDoc);
                    Marshal.ReleaseComObject(wordApp);

                    byte[] pdfBytes = System.IO.File.ReadAllBytes(pdfFilePath);
                    byte[] wordBytes = System.IO.File.ReadAllBytes(tempWordFilePath);

                    MemoryStream pdfStream = new MemoryStream(pdfBytes);
                    string pdfBase64 = Convert.ToBase64String(pdfStream.ToArray());

                    string wordBase64 = Convert.ToBase64String(wordBytes);

                    // Eliminar los archivos temporales
                    System.IO.File.Delete(wordFilePath);
                    System.IO.File.Delete(pdfFilePath);
                    System.IO.File.Delete(tempWordFilePath);

                    Secuencia secuencia = new Secuencia
                    {

                        secuencia = ultima_secuencia,
                        fecha = DateTime.Now
                    };

                    Helpper.SentenciaHelpper.AddSentencia(sentencia);
                    Helpper.SentenciaHelpper.AddSecuencia(secuencia);

                    return Json(new
                    {
                        success = true,
                        pdfBase64,
                        fileBase64 = wordBase64,
                        resumen
                    });

                }
                catch (Exception ex)
                {
                    if (wordDoc != null)
                    {
                        wordDoc.Close();
                        Marshal.ReleaseComObject(wordDoc);
                    }

                    if (wordApp != null)
                    {
                        wordApp.Quit();
                        Marshal.ReleaseComObject(wordApp);
                    }

                    return Json(false);
                }
            }
            else
            {
                return Json(false);
            }
        }



        public ActionResult Secuencia()
        {

            if (!string.IsNullOrEmpty(Session["usuario"].ToString()))
            {
                var secuencias = Helpper.SentenciaHelpper.GetSecuencias();
                return View(secuencias);
            }
            else return RedirectToAction("Login", "Auth");





        }

        [HttpPost]
        public ActionResult Secuencia(string secuencia)
        {

            Secuencia sec = new Secuencia
            {
                secuencia = secuencia,
                fecha = DateTime.Now
            };

            Helpper.SentenciaHelpper.AddSecuencia(sec);

            var secuencias = Helpper.SentenciaHelpper.GetSecuencias();
            return View(secuencias);
        }

    }
}
