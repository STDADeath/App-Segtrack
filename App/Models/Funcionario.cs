using System;                    // Funciones básicas del lenguaje (tipos, fechas, errores, etc.)
using System.Collections.Generic; // Para usar listas y colecciones
using System.Linq;               // Para hacer búsquedas y filtros en listas
using System.Text;               // Para trabajar textos de forma avanzada (poco usado)
using System.Threading.Tasks;    // Para usar métodos asincrónicos (async/await)


namespace segtrack.Models
{
    // Enum que define los estados permitidos para un funcionario
    public enum EstadoFuncionario
    {
        Activo,
        Inactivo
    }

    // Modelo que representa a un funcionario en el sistema
    public class Funcionario
    {
        public int IdFuncionario { get; set; }        
        public string CargoFuncionario { get; set; }    
        public string QrCodigoFuncionario { get; set; } 
        public string NombreFuncionario { get; set; }   
        public string TelefonoFuncionario { get; set; } 
        public string DocumentoFuncionario { get; set; }
        public string CorreoFuncionario { get; set; }
        public EstadoFuncionario Estado { get; set; } = EstadoFuncionario.Activo;
        public int IdSede { get; set; }                 
    }
}
