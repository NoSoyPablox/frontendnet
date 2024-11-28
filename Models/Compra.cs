using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace frontendnet.Models;

//id, fechaPedido, usuarioId

public class Compra
{
    public int? CompraId { get; set; }
    public DateTime FechaPedido { get; set; }
    public string? UsuarioId { get; set; }
    public List<CompraProducto>? CompraProductos { get; set; }
    public int? TotalProductos { get; set; }
    public decimal? TotalCosto { get; set; }
}