acsharp
using System;
using System.Collections.Generic;
using Modelos;
using Datos;

namespace Logica
{
    public class AsistenciaBLL
    {
        private AsistenciaDAL asistenciaDAL;

        public AsistenciaBLL(string connectionString)
        {
            asistenciaDAL = new AsistenciaDAL(connectionString);
        }

        public void Insert(Asistencia asistencia)
        {
            asistenciaDAL.Insert(asistencia);
        }

        public void Update(Asistencia asistencia)
        {
            asistenciaDAL.Update(asistencia);
        }

        public void Delete(int id)
        {
            asistenciaDAL.Delete(id);
        }

        public List<Asistencia> GetAll()
        {
            return asistenciaDAL.GetAll();
        }

        public Asistencia GetById(int id)
        {
            return asistenciaDAL.GetById(id);
        }
    }
}