using System;
using AlumniaNotes.data;
using AlumniaNotes.models;

namespace AlumniaNotes.services
{
    public static class ServiceLocator
    {
        private static IMessageBox _messageBox;
        private static IMessageService _messageService;
        private static IExportService _exportService;
        private static IRepository<Usuario> _usuarioRepository;
        private static IRepository<Alumno> _alumnoRepository;
        private static IRepository<Asignatura> _asignaturaRepository;
        private static IRepository<Calificacion> _calificacionRepository;
        private static IRepository<Asistencia> _asistenciaRepository;
        private static IRepository<Reporte> _reporteRepository;

        public static IMessageBox MessageBox
        {
            get => _messageBox ?? throw new InvalidOperationException("MessageBox no ha sido inicializado");
            set => _messageBox = value ?? throw new ArgumentNullException(nameof(value));
        }

        public static IMessageService MessageService
        {
            get => _messageService ?? throw new InvalidOperationException("MessageService no ha sido inicializado");
            set => _messageService = value ?? throw new ArgumentNullException(nameof(value));
        }

        public static IExportService ExportService
        {
            get => _exportService ?? throw new InvalidOperationException("ExportService no ha sido inicializado");
            set => _exportService = value ?? throw new ArgumentNullException(nameof(value));
        }

        public static IRepository<Usuario> UsuarioRepository
        {
            get => _usuarioRepository ?? throw new InvalidOperationException("UsuarioRepository no ha sido inicializado");
            set => _usuarioRepository = value ?? throw new ArgumentNullException(nameof(value));
        }

        public static IRepository<Alumno> AlumnoRepository
        {
            get => _alumnoRepository ?? throw new InvalidOperationException("AlumnoRepository no ha sido inicializado");
            set => _alumnoRepository = value ?? throw new ArgumentNullException(nameof(value));
        }

        public static IRepository<Asignatura> AsignaturaRepository
        {
            get => _asignaturaRepository ?? throw new InvalidOperationException("AsignaturaRepository no ha sido inicializado");
            set => _asignaturaRepository = value ?? throw new ArgumentNullException(nameof(value));
        }

        public static IRepository<Calificacion> CalificacionRepository
        {
            get => _calificacionRepository ?? throw new InvalidOperationException("CalificacionRepository no ha sido inicializado");
            set => _calificacionRepository = value ?? throw new ArgumentNullException(nameof(value));
        }

        public static IRepository<Asistencia> AsistenciaRepository
        {
            get => _asistenciaRepository ?? throw new InvalidOperationException("AsistenciaRepository no ha sido inicializado");
            set => _asistenciaRepository = value ?? throw new ArgumentNullException(nameof(value));
        }

        public static IRepository<Reporte> ReporteRepository
        {
            get => _reporteRepository ?? throw new InvalidOperationException("ReporteRepository no ha sido inicializado");
            set => _reporteRepository = value ?? throw new ArgumentNullException(nameof(value));
        }

        public static void Initialize()
        {
            var context = new DatabaseContext();
            
            MessageBox = new WpfMessageBox();
            MessageService = new MessageService(MessageBox);
            ExportService = new ExportService();
            
            UsuarioRepository = new UsuarioRepository(context);
            AlumnoRepository = new AlumnoRepository(context);
            AsignaturaRepository = new AsignaturaRepository(context);
            CalificacionRepository = new CalificacionRepository(context);
            AsistenciaRepository = new AsistenciaRepository(context);
            ReporteRepository = new ReporteRepository(context);
        }
    }
} 