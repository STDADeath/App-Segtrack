using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace segtrack.Models
{
    public enum BitacoraTurno
    {
        JornadaManana,
        JornadaTarde,
        JornadaNocturna
    }

    public enum BitacoraEstado
    {
        Activo,
        Inactivo
    }

    public class Bitacora
    {
        public int IdBitacora { get; set; }
        public BitacoraTurno TurnoBitacora { get; set; }
        public string NovedadesBitacora { get; set; }
        public DateTime FechaBitacora { get; set; }
        public BitacoraEstado Estado { get; set; } = BitacoraEstado.Activo;
        public int IdFuncionario { get; set; }
        public int IdIngreso { get; set; }
        public int? IdDispositivo { get; set; }
        public int? IdVisitante { get; set; }
    }
}
