using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace segtrack.Models
{
    public enum IngresoEstado
    {
        Activo,
        Inactivo
    }

    public enum IngresoTipo
    {
        Entrada,
        Salida
    }

    public class Ingreso
    {
        public int IdIngreso { get; set; }
        public string ObservacionesIngreso { get; set; }
        public IngresoTipo TipoMoviento { get; set; }
        public DateTime FechaIngreso { get; set; } = DateTime.Now;
        public IngresoEstado Estado { get; set; } = IngresoEstado.Activo;
        public int IdSede { get; set; }
        public int IdParqueadero { get; set; }
        public int IdFuncionario { get; set; }
    }
}
