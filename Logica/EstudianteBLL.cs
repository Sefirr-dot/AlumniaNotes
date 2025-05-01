csharp
﻿using System;

using Modelos;
using Datos;

namespace Logica
{
    public class EstudianteBLL
    {
        private EstudianteDAL estudianteDAL;

        public EstudianteBLL(string connectionString)
        {
            estudianteDAL = new EstudianteDAL(connectionString);
        }


        public List<Estudiante> GetAll()
        {
            return estudianteDAL.GetAll();
        }

        public Estudiante GetById(int id)
        {
            return estudianteDAL.GetById(id);
        }

        public void Insert(Estudiante estudiante)
        {
            estudianteDAL.Insert(estudiante);
        }

        public void Update(Estudiante estudiante)
        {
            estudianteDAL.Update(estudiante);
        }

        public void Delete(int id)
        {
            estudianteDAL.Delete(id);
        }

    }
}