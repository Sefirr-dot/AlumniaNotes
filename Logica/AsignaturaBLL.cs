csharp
using System;
using System.Collections.Generic;
using Modelos;
using Datos;

namespace Logica
{
    public class AsignaturaBLL
    {
        private AsignaturaDAL asignaturaDAL;

        public AsignaturaBLL(string connectionString)
        {
            asignaturaDAL = new AsignaturaDAL(connectionString);
        }


        public List<Asignatura> GetAll()
        {
            return asignaturaDAL.GetAll();
        }

        public Asignatura GetById(int id)
        {
            return asignaturaDAL.GetById(id);
        }

        public void Insert(Asignatura asignatura)
        {
            asignaturaDAL.Insert(asignatura);
        }

        public void Update(Asignatura asignatura)
        {
            asignaturaDAL.Update(asignatura);
        }

        public void Delete(int id)
        {
            asignaturaDAL.Delete(id);
        }
    }
}