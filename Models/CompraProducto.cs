using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace frontendnet.Models;

public class CompraProducto
{
    public int? Cantidad { get; set; }
    public decimal Precio { get; set; }
    public int? CompraId { get; set; }
    public int? ProductoId { get; set; }
    public Producto? Producto { get; set; }
}