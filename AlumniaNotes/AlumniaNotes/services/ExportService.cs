using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AlumniaNotes.data;
using AlumniaNotes.models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ClosedXML.Excel;
using Microsoft.Office.Interop.Excel;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace AlumniaNotes.services
{
    public interface IExportService
    {
        Task ExportToPdfAsync(IEnumerable<ReportDataItem> data, string title, string dateRange, bool includeCharts, byte[]? chartImage = null);
        Task ExportToExcelAsync(IEnumerable<ReportDataItem> data, string title, string dateRange, bool includeCharts, byte[]? chartImage = null);
        Task ExportToCsvAsync(IEnumerable<ReportDataItem> data, string title, string dateRange);
    }

    public class ExportService : IExportService
    {
        private readonly DatabaseContext _context;
        private readonly IChartService _chartService;

        public ExportService(DatabaseContext context, IChartService chartService)
        {
            _context = context;
            _chartService = chartService;
        }

        public bool ExportToPdf(Reporte reporte, string filePath)
        {
            try
            {
                using (var document = new Document())
                {
                    PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                    document.Open();

                    // Título
                    var title = new Paragraph($"Reporte de {reporte.Tipo}", new Font(Font.FontFamily.HELVETICA, 18, Font.BOLD));
                    title.Alignment = Element.ALIGN_CENTER;
                    document.Add(title);

                    // Información del reporte
                    document.Add(new Paragraph($"Fecha de inicio: {reporte.FechaInicio:dd/MM/yyyy}"));
                    document.Add(new Paragraph($"Fecha de fin: {reporte.FechaFin:dd/MM/yyyy}"));
                    document.Add(new Paragraph(" "));

                    // Datos según el tipo de reporte
                    switch (reporte.Tipo)
                    {
                        case TipoReporte.Calificaciones:
                            ExportCalificacionesToPdf(document, reporte);
                            break;
                        case TipoReporte.Asistencias:
                            ExportAsistenciasToPdf(document, reporte);
                            break;
                    }

                    document.Close();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ExportToExcel(Reporte reporte, string filePath)
        {
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Reporte");

                    // Título
                    worksheet.Cell(1, 1).Value = $"Reporte de {reporte.Tipo}";
                    worksheet.Cell(1, 1).Style.Font.Bold = true;
                    worksheet.Cell(1, 1).Style.Font.FontSize = 14;

                    // Información del reporte
                    worksheet.Cell(3, 1).Value = "Fecha de inicio:";
                    worksheet.Cell(3, 2).Value = reporte.FechaInicio;
                    worksheet.Cell(4, 1).Value = "Fecha de fin:";
                    worksheet.Cell(4, 2).Value = reporte.FechaFin;

                    // Datos según el tipo de reporte
                    switch (reporte.Tipo)
                    {
                        case TipoReporte.Calificaciones:
                            ExportCalificacionesToExcel(worksheet, reporte);
                            break;
                        case TipoReporte.Asistencias:
                            ExportAsistenciasToExcel(worksheet, reporte);
                            break;
                    }

                    workbook.SaveAs(filePath);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ExportToCsv(Reporte reporte, string filePath)
        {
            try
            {
                using (var writer = new StreamWriter(filePath))
                {
                    // Encabezado
                    writer.WriteLine($"Reporte de {reporte.Tipo}");
                    writer.WriteLine($"Fecha de inicio: {reporte.FechaInicio:dd/MM/yyyy}");
                    writer.WriteLine($"Fecha de fin: {reporte.FechaFin:dd/MM/yyyy}");
                    writer.WriteLine();

                    // Datos según el tipo de reporte
                    switch (reporte.Tipo)
                    {
                        case TipoReporte.Calificaciones:
                            ExportCalificacionesToCsv(writer, reporte);
                            break;
                        case TipoReporte.Asistencias:
                            ExportAsistenciasToCsv(writer, reporte);
                            break;
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void ExportCalificacionesToPdf(Document document, Reporte reporte)
        {
            var calificaciones = ServiceLocator.CalificacionRepository.GetAll()
                .Where(c => c.Fecha >= reporte.FechaInicio && c.Fecha <= reporte.FechaFin)
                .ToList();

            var table = new PdfPTable(4);
            table.AddCell("Alumno");
            table.AddCell("Asignatura");
            table.AddCell("Fecha");
            table.AddCell("Nota");

            foreach (var calificacion in calificaciones)
            {
                var alumno = ServiceLocator.AlumnoRepository.GetById(calificacion.AlumnoId);
                var asignatura = ServiceLocator.AsignaturaRepository.GetById(calificacion.AsignaturaId);

                table.AddCell($"{alumno.Nombre} {alumno.Apellidos}");
                table.AddCell(asignatura.Nombre);
                table.AddCell(calificacion.Fecha.ToString("dd/MM/yyyy"));
                table.AddCell(calificacion.Nota.ToString());
            }

            document.Add(table);
        }

        private void ExportAsistenciasToPdf(Document document, Reporte reporte)
        {
            var asistencias = ServiceLocator.AsistenciaRepository.GetAll()
                .Where(a => a.Fecha >= reporte.FechaInicio && a.Fecha <= reporte.FechaFin)
                .ToList();

            var table = new PdfPTable(4);
            table.AddCell("Alumno");
            table.AddCell("Asignatura");
            table.AddCell("Fecha");
            table.AddCell("Estado");

            foreach (var asistencia in asistencias)
            {
                var alumno = ServiceLocator.AlumnoRepository.GetById(asistencia.AlumnoId);
                var asignatura = ServiceLocator.AsignaturaRepository.GetById(asistencia.AsignaturaId);

                table.AddCell($"{alumno.Nombre} {alumno.Apellidos}");
                table.AddCell(asignatura.Nombre);
                table.AddCell(asistencia.Fecha.ToString("dd/MM/yyyy"));
                table.AddCell(asistencia.Estado.ToString());
            }

            document.Add(table);
        }

        private void ExportCalificacionesToExcel(IXLWorksheet worksheet, Reporte reporte)
        {
            var calificaciones = ServiceLocator.CalificacionRepository.GetAll()
                .Where(c => c.Fecha >= reporte.FechaInicio && c.Fecha <= reporte.FechaFin)
                .ToList();

            worksheet.Cell(6, 1).Value = "Alumno";
            worksheet.Cell(6, 2).Value = "Asignatura";
            worksheet.Cell(6, 3).Value = "Fecha";
            worksheet.Cell(6, 4).Value = "Nota";

            int row = 7;
            foreach (var calificacion in calificaciones)
            {
                var alumno = ServiceLocator.AlumnoRepository.GetById(calificacion.AlumnoId);
                var asignatura = ServiceLocator.AsignaturaRepository.GetById(calificacion.AsignaturaId);

                worksheet.Cell(row, 1).Value = $"{alumno.Nombre} {alumno.Apellidos}";
                worksheet.Cell(row, 2).Value = asignatura.Nombre;
                worksheet.Cell(row, 3).Value = calificacion.Fecha;
                worksheet.Cell(row, 4).Value = calificacion.Nota;
                row++;
            }
        }

        private void ExportAsistenciasToExcel(IXLWorksheet worksheet, Reporte reporte)
        {
            var asistencias = ServiceLocator.AsistenciaRepository.GetAll()
                .Where(a => a.Fecha >= reporte.FechaInicio && a.Fecha <= reporte.FechaFin)
                .ToList();

            worksheet.Cell(6, 1).Value = "Alumno";
            worksheet.Cell(6, 2).Value = "Asignatura";
            worksheet.Cell(6, 3).Value = "Fecha";
            worksheet.Cell(6, 4).Value = "Estado";

            int row = 7;
            foreach (var asistencia in asistencias)
            {
                var alumno = ServiceLocator.AlumnoRepository.GetById(asistencia.AlumnoId);
                var asignatura = ServiceLocator.AsignaturaRepository.GetById(asistencia.AsignaturaId);

                worksheet.Cell(row, 1).Value = $"{alumno.Nombre} {alumno.Apellidos}";
                worksheet.Cell(row, 2).Value = asignatura.Nombre;
                worksheet.Cell(row, 3).Value = asistencia.Fecha;
                worksheet.Cell(row, 4).Value = asistencia.Estado.ToString();
                row++;
            }
        }

        private void ExportCalificacionesToCsv(StreamWriter writer, Reporte reporte)
        {
            var calificaciones = ServiceLocator.CalificacionRepository.GetAll()
                .Where(c => c.Fecha >= reporte.FechaInicio && c.Fecha <= reporte.FechaFin)
                .ToList();

            writer.WriteLine("Alumno,Asignatura,Fecha,Nota");
            foreach (var calificacion in calificaciones)
            {
                var alumno = ServiceLocator.AlumnoRepository.GetById(calificacion.AlumnoId);
                var asignatura = ServiceLocator.AsignaturaRepository.GetById(calificacion.AsignaturaId);

                writer.WriteLine($"{alumno.Nombre} {alumno.Apellidos},{asignatura.Nombre},{calificacion.Fecha:dd/MM/yyyy},{calificacion.Nota}");
            }
        }

        private void ExportAsistenciasToCsv(StreamWriter writer, Reporte reporte)
        {
            var asistencias = ServiceLocator.AsistenciaRepository.GetAll()
                .Where(a => a.Fecha >= reporte.FechaInicio && a.Fecha <= reporte.FechaFin)
                .ToList();

            writer.WriteLine("Alumno,Asignatura,Fecha,Estado");
            foreach (var asistencia in asistencias)
            {
                var alumno = ServiceLocator.AlumnoRepository.GetById(asistencia.AlumnoId);
                var asignatura = ServiceLocator.AsignaturaRepository.GetById(asistencia.AsignaturaId);

                writer.WriteLine($"{alumno.Nombre} {alumno.Apellidos},{asignatura.Nombre},{asistencia.Fecha:dd/MM/yyyy},{asistencia.Estado}");
            }
        }

        public async Task ExportToPdfAsync(IEnumerable<ReportDataItem> data, string title, string dateRange, bool includeCharts, byte[]? chartImage = null)
        {
            await Task.Run(() =>
            {
                using (var stream = new FileStream("Reporte.pdf", FileMode.Create))
                {
                    var document = new Document(PageSize.A4);
                    var writer = PdfWriter.GetInstance(document, stream);

                    document.Open();

                    // Título
                    var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                    var titleParagraph = new Paragraph(title, titleFont);
                    titleParagraph.Alignment = Element.ALIGN_CENTER;
                    document.Add(titleParagraph);

                    // Rango de fechas
                    var dateFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
                    var dateParagraph = new Paragraph(dateRange, dateFont);
                    dateParagraph.Alignment = Element.ALIGN_CENTER;
                    document.Add(dateParagraph);

                    document.Add(new Paragraph("\n"));

                    // Tabla de datos
                    var table = new PdfPTable(2);
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 2, 1 });

                    var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                    var cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);

                    // Encabezados
                    var headerCell = new PdfPCell(new Phrase("Concepto", headerFont));
                    headerCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(headerCell);

                    headerCell = new PdfPCell(new Phrase("Valor", headerFont));
                    headerCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(headerCell);

                    // Datos
                    foreach (var item in data)
                    {
                        table.AddCell(new PdfPCell(new Phrase(item.Title, cellFont)));
                        table.AddCell(new PdfPCell(new Phrase(item.Value, cellFont)));
                    }

                    document.Add(table);

                    // Gráfico si está incluido
                    if (includeCharts && chartImage != null)
                    {
                        document.Add(new Paragraph("\n"));
                        using (var ms = new MemoryStream(chartImage))
                        {
                            var image = Image.GetInstance(ms);
                            image.Alignment = Element.ALIGN_CENTER;
                            image.ScaleToFit(document.PageSize.Width - document.LeftMargin - document.RightMargin, 400);
                            document.Add(image);
                        }
                    }

                    document.Close();
                }
            });
        }

        public async Task ExportToExcelAsync(IEnumerable<ReportDataItem> data, string title, string dateRange, bool includeCharts, byte[]? chartImage = null)
        {
            await Task.Run(() =>
            {
                var excel = new Application();
                var workbook = excel.Workbooks.Add();
                var worksheet = (Worksheet)workbook.ActiveSheet;

                // Título
                var titleRange = worksheet.Range["A1", "B1"];
                titleRange.Merge();
                titleRange.Value = title;
                titleRange.Font.Bold = true;
                titleRange.Font.Size = 14;
                titleRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;

                // Rango de fechas
                var dateRangeCell = worksheet.Range["A2", "B2"];
                dateRangeCell.Merge();
                dateRangeCell.Value = dateRange;
                dateRangeCell.Font.Size = 12;
                dateRangeCell.HorizontalAlignment = XlHAlign.xlHAlignCenter;

                // Encabezados
                worksheet.Cells[4, 1] = "Concepto";
                worksheet.Cells[4, 2] = "Valor";
                var headerRange = worksheet.Range["A4", "B4"];
                headerRange.Font.Bold = true;
                headerRange.Interior.Color = System.Drawing.Color.LightGray;

                // Datos
                var row = 5;
                foreach (var item in data)
                {
                    worksheet.Cells[row, 1] = item.Title;
                    worksheet.Cells[row, 2] = item.Value;
                    row++;
                }

                // Gráfico si está incluido
                if (includeCharts && chartImage != null)
                {
                    // Guardar la imagen temporalmente
                    var tempImagePath = Path.GetTempFileName() + ".png";
                    File.WriteAllBytes(tempImagePath, chartImage);

                    // Insertar la imagen
                    var chartRange = worksheet.Range[$"A{row + 2}", $"B{row + 2}"];
                    chartRange.Merge();
                    var shape = worksheet.Shapes.AddPicture(
                        tempImagePath,
                        Microsoft.Office.Core.MsoTriState.msoFalse,
                        Microsoft.Office.Core.MsoTriState.msoCTrue,
                        chartRange.Left,
                        chartRange.Top,
                        chartRange.Width,
                        chartRange.Height
                    );

                    // Eliminar el archivo temporal
                    File.Delete(tempImagePath);
                }

                // Ajustar columnas
                worksheet.Columns.AutoFit();

                workbook.SaveAs("Reporte.xlsx");
                workbook.Close();
                excel.Quit();
            });
        }

        public async Task ExportToCsvAsync(IEnumerable<ReportDataItem> data, string title, string dateRange)
        {
            await Task.Run(() =>
            {
                var csv = new StringBuilder();
                csv.AppendLine(title);
                csv.AppendLine(dateRange);
                csv.AppendLine();
                csv.AppendLine("Concepto,Valor");

                foreach (var item in data)
                {
                    csv.AppendLine($"{EscapeCsv(item.Title)},{EscapeCsv(item.Value)}");
                }

                File.WriteAllText("Reporte.csv", csv.ToString());
            });
        }

        private string EscapeCsv(string value)
        {
            if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
            {
                return $"\"{value.Replace("\"", "\"\"")}\"";
            }
            return value;
        }
    }
} 