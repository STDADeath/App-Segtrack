using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace segtrack.Models
{
    public enum DispositivoTipo
    {
        Computador,
        Tablet,
        Portatil,
        Otro
    }

    public enum DispositivoEstado
    {
        Activo,
        Inactivo
    }

    public class Dispositivo
    {
        public int IdDispositivo { get; set; }
        public string QrDispositivo { get; set; }
        public DispositivoTipo TipoDispositivo { get; set; }
        public string MarcaDispositivo { get; set; }
        public DispositivoEstado Estado { get; set; } = DispositivoEstado.Activo;
        public int IdFuncionario { get; set; }
        public int? IdVisitante { get; set; } // Nullable porque no siempre hay visitante
    }
}