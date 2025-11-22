using System;                    // Funciones básicas del lenguaje (tipos, fechas, errores, etc.)
using System.Collections.Generic; // Para usar listas y colecciones
using System.Linq;               // Para hacer búsquedas y filtros en listas
using System.Text;               // Para trabajar textos de forma avanzada (poco usado)
using System.Threading.Tasks;    // Para usar métodos asincrónicos (async/await)

namespace segtrack.Models
{
	public enum VehiculoTipo 	{
		Bicicleta,
		Moto,
		Carro,
		Otro
	}

	public enum parqueaderoEstado
	{
		Activo,
		Inactivo
	}

	public class parqueadero 
	{ 
	public int IdParqueadero { get; set; }
	public VehiculoTipo TipoVehiculo { get; set; }
	public string PlacaVehiculo { get; set; }
	public string DescripcionVehiculo { get; set; }
	public string QrCodigoVehiculo { get; set; }
	public string TarjetaPropiedad  { get; set; }
	public DateTime FechaParqueadero { get; set; } = DateTime.Now;
	public parqueaderoEstado Estado { get; set; } = parqueaderoEstado.Activo;
	public int IdSede { get; set; }


	}
}