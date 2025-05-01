using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AlumniaNotes.models;
using ScottPlot;

namespace AlumniaNotes.services
{
    public interface IChartService
    {
        Task<byte[]> GenerateGradesChartAsync(IEnumerable<Calificacion> calificaciones);
        Task<byte[]> GenerateAttendanceChartAsync(IEnumerable<Asistencia> asistencias);
        Task<byte[]> GeneratePerformanceChartAsync(IEnumerable<Calificacion> calificaciones, IEnumerable<Asistencia> asistencias);
        Task<byte[]> GenerateSubjectsChartAsync(IEnumerable<Calificacion> calificaciones);
    }

    public class ChartService : IChartService
    {
        public async Task<byte[]> GenerateGradesChartAsync(IEnumerable<Calificacion> calificaciones)
        {
            return await Task.Run(() =>
            {
                var plot = new Plot(800, 400);
                plot.Title("Distribución de Calificaciones");
                plot.XLabel("Calificación");
                plot.YLabel("Frecuencia");

                var notas = calificaciones.Select(c => c.Nota).ToArray();
                var histogram = plot.AddHistogram(notas, 10);
                histogram.FillColor = Color.FromArgb(50, Color.Blue);
                histogram.BorderColor = Color.Blue;

                return plot.GetImageBytes();
            });
        }

        public async Task<byte[]> GenerateAttendanceChartAsync(IEnumerable<Asistencia> asistencias)
        {
            return await Task.Run(() =>
            {
                var plot = new Plot(800, 400);
                plot.Title("Distribución de Asistencias");
                plot.XLabel("Estado");
                plot.YLabel("Cantidad");

                var estados = asistencias.GroupBy(a => a.Estado)
                    .Select(g => new { Estado = g.Key, Cantidad = g.Count() })
                    .OrderBy(x => x.Estado)
                    .ToArray();

                var bar = plot.AddBar(
                    estados.Select(x => (double)x.Cantidad).ToArray(),
                    estados.Select(x => (double)(int)x.Estado).ToArray()
                );

                bar.FillColor = Color.FromArgb(50, Color.Green);
                bar.BorderColor = Color.Green;

                plot.XTicks(
                    estados.Select(x => (double)(int)x.Estado).ToArray(),
                    estados.Select(x => x.Estado.ToString()).ToArray()
                );

                return plot.GetImageBytes();
            });
        }

        public async Task<byte[]> GeneratePerformanceChartAsync(IEnumerable<Calificacion> calificaciones, IEnumerable<Asistencia> asistencias)
        {
            return await Task.Run(() =>
            {
                var plot = new Plot(800, 400);
                plot.Title("Rendimiento vs Asistencia");
                plot.XLabel("Asistencia (%)");
                plot.YLabel("Calificación Promedio");

                var datos = calificaciones
                    .GroupBy(c => c.AlumnoId)
                    .Select(g => new
                    {
                        AlumnoId = g.Key,
                        Promedio = g.Average(c => c.Nota),
                        Asistencia = (double)asistencias.Count(a => a.AlumnoId == g.Key && a.Estado == EstadoAsistencia.Presente) /
                                   asistencias.Count(a => a.AlumnoId == g.Key) * 100
                    })
                    .ToArray();

                var scatter = plot.AddScatter(
                    datos.Select(x => x.Asistencia).ToArray(),
                    datos.Select(x => x.Promedio).ToArray()
                );

                scatter.Color = Color.Blue;
                scatter.MarkerSize = 10;

                return plot.GetImageBytes();
            });
        }

        public async Task<byte[]> GenerateSubjectsChartAsync(IEnumerable<Calificacion> calificaciones)
        {
            return await Task.Run(() =>
            {
                var plot = new Plot(800, 400);
                plot.Title("Promedio por Asignatura");
                plot.XLabel("Asignatura");
                plot.YLabel("Promedio");

                var promedios = calificaciones
                    .GroupBy(c => c.AsignaturaId)
                    .Select(g => new
                    {
                        AsignaturaId = g.Key,
                        Promedio = g.Average(c => c.Nota)
                    })
                    .OrderByDescending(x => x.Promedio)
                    .ToArray();

                var bar = plot.AddBar(
                    promedios.Select(x => x.Promedio).ToArray(),
                    promedios.Select((x, i) => (double)i).ToArray()
                );

                bar.FillColor = Color.FromArgb(50, Color.Purple);
                bar.BorderColor = Color.Purple;

                plot.XTicks(
                    promedios.Select((x, i) => (double)i).ToArray(),
                    promedios.Select(x => x.AsignaturaId.ToString()).ToArray()
                );

                return plot.GetImageBytes();
            });
        }
    }
} 