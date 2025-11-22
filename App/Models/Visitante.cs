using System;                    // Funciones básicas del lenguaje (tipos, fechas, errores, etc.)
using System.Collections.Generic; // Para usar listas y colecciones
using System.Linq;               // Para hacer búsquedas y filtros en listas
using System.Text;               // Para trabajar textos de forma avanzada (poco usado)
using System.Threading.Tasks;    // Para usar métodos asincrónicos (async/await)


namespace segtrack.Models
{ 
  public enum VisitanteEstado
  {
      Activo,
      Inactivo
    }
    public class Visitante
    {
        public int IdVisitante { get; set; }
        public string IdentificacionVisitante { get; set; }
        public VisitanteEstado Estado { get; set; } = VisitanteEstado.Activo;
        public string NombreVisitante { get; set; }

    }

}