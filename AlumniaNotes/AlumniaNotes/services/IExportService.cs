using System;
using AlumniaNotes.models;

namespace AlumniaNotes.services
{
    public interface IExportService
    {
        bool ExportToPdf(Reporte reporte, string filePath);
        bool ExportToExcel(Reporte reporte, string filePath);
        bool ExportToCsv(Reporte reporte, string filePath);
    }
} 