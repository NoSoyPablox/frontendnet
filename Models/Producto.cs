using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace frontendnet.Models;

public class Producto
{
    [Display(Name = "Id")]
    public int? ProductoId { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public required string Titulo { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [DataType(DataType.MultilineText)]
    public string Descripcion { get; set; } ="Sin descripción";

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [DataType(DataType.Currency)]
    [RegularExpression(@"^\d+.?\d{0,2}$", ErrorMessage = "El valor del campo debe ser un precio valido.")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    [Display(Name = "Precio")]
    public decimal Precio {get; set;}

    [Display(Name = "Portada")]
    public int? ArchivoId { get; set; }

    [Display(Name = "Eliminable")]
    public bool Protegida { get; set; } = false;

    public ICollection<Categoria> Categorias { get; set; }
}