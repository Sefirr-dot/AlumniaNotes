csharp
using System;
using System.Collections.Generic;
using Modelos;
using Datos;

using System.Collections.Generic;
using Datos;
using Modelos;

namespace Logica
{
   public class CalificacionBLL
   {
      private CalificacionDAL calificacionDAL;

      public CalificacionBLL(string connectionString)
      {
         calificacionDAL = new CalificacionDAL(connectionString);
      }

        public List<Calificacion> GetAll()
        {
            return calificacionDAL.GetAll();
        }

        public Calificacion GetById(int id)
        {
            return calificacionDAL.GetById(id);
        }

        public void Insert(Calificacion calificacion)
        {
            calificacionDAL.Insert(calificacion);
        }

        public void Update(Calificacion calificacion)
        {
            calificacionDAL.Update(calificacion);
        }
   }
}