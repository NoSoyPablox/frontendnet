using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace frontendnet.Models;

public class CarritoProducto
{
    //cantidad int, carritoid, productoid
    public int? CarritoId { get; set; }
    public int? ProductoId { get; set; }
    public int? Cantidad { get; set; }
    [JsonPropertyName("totalPrecio")]
    public decimal? TotalPrecio { get; set; }
    public Producto? Producto { get; set; }
}