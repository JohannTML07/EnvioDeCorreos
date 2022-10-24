using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManejoDeTablas
{
    //cambiar internal por public en la clase
    public class EnvioCorreo
    {
        public bool Enviado { get; set; }
        public string Correo { get; set; }
        public String Alumno { get; set; }

        public double Calificacion { get; set; }
        public EnvioCorreo() { }

        public EnvioCorreo(bool enviado, string correo, string alumno, double calificacion)
        {
            Enviado = enviado;
            Correo = correo;
            Alumno = alumno;
            Calificacion = calificacion;
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            EnvioCorreo other = (EnvioCorreo)obj;
            return other.Correo.Equals(Correo);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            throw new NotImplementedException();
            return base.GetHashCode();
        }
    }
}
