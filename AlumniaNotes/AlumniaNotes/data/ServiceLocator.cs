using System.Configuration;
using AlumniaNotes.models;
using AlumniaNotes.services;

namespace AlumniaNotes.data
{
    public static class ServiceLocator
    {
        private static IMessageService _messageService;
        private static IExportService _exportService;
        private static IRepository<Usuario> _usuarioRepository;
        private static IRepository<Alumno> _alumnoRepository;
        private static IRepository<Asignatura> _asignaturaRepository;
        private static IRepository<Calificacion> _calificacionRepository;
        private static IRepository<Asistencia> _asistenciaRepository;
        private static IRepository<Reporte> _reporteRepository;

        public static IMessageService MessageService
        {
            get => _messageService ??= new MessageService();
            set => _messageService = value;
        }

        public static IExportService ExportService
        {
            get => _exportService ??= new ExportService();
            set => _exportService = value;
        }

        public static IRepository<Usuario> UsuarioRepository
        {
            get => _usuarioRepository ??= new UsuarioRepository(new DatabaseContext());
            set => _usuarioRepository = value;
        }

        public static IRepository<Alumno> AlumnoRepository
        {
            get => _alumnoRepository ??= new AlumnoRepository(new DatabaseContext());
            set => _alumnoRepository = value;
        }

        public static IRepository<Asignatura> AsignaturaRepository
        {
            get => _asignaturaRepository ??= new AsignaturaRepository(new DatabaseContext());
            set => _asignaturaRepository = value;
        }

        public static IRepository<Calificacion> CalificacionRepository
        {
            get => _calificacionRepository ??= new CalificacionRepository(new DatabaseContext());
            set => _calificacionRepository = value;
        }

        public static IRepository<Asistencia> AsistenciaRepository
        {
            get => _asistenciaRepository ??= new AsistenciaRepository(new DatabaseContext());
            set => _asistenciaRepository = value;
        }

        public static IRepository<Reporte> ReporteRepository
        {
            get => _reporteRepository ??= new ReporteRepository(new DatabaseContext());
            set => _reporteRepository = value;
        }
    }
} 