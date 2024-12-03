using System.Security.Claims;
using frontendnet.Models;
using frontendnet.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace frontendnet;

public class AuthController(AuthClientService auth, RolesClientService roles, UsuariosClientService usuarios) : Controller{
    [AllowAnonymous]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> IndexAsync(Login model)
    {
        if (ModelState.IsValid)
        {
            try{
                //Esta funcion verifica en backend que el correo y contraseña sean validos
                var token = await auth.ObtenTokenAsync(model.Email, model.Password);
                var claims = new List<Claim>
                {
                    //Todo esto se guarda en la cookie
                    new(ClaimTypes.Name, token.Email),
                    new(ClaimTypes.GivenName, token.Nombre),
                    new("jwt", token.Jwt),
                    new(ClaimTypes.Role, token.Rol),
                };
                auth.IniciaSesionAsync(claims);
                //Usuario valido
                if(token.Rol == "Administrador")
                    return RedirectToAction("Index", "Productos");
                else
                    return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                ModelState.AddModelError("Email", "Credenciales no validas. Intentelo nuevamente.");
            }
        }
        return View(model);
    }


    [Authorize (Roles ="Administrador, Usuario")]
    public async Task<IActionResult> SalirAsync()
    {
        //Cierra la sesion
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //Sino redirige a la pagina inicial
        return RedirectToAction("Index", "Auth");
    }

    public async Task<IActionResult> Crear()
    {
        await RolesDropDownListAsync();
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> CrearAsync(UsuarioPwd itemToCreate)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await usuarios.PostAsync(itemToCreate);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("Email", "No ha sido posible crear la cuenta. Intentelo nuevamente.");
            }
        }
        return View(itemToCreate);

    }

    private async Task RolesDropDownListAsync(object? rolSelecionado = null)
    {
        var listado = await roles.GetAsync();

        // Filtrar solo el rol con el nombre "Usuario"
        if (listado == null) return;
        var usuarioRol = listado.FirstOrDefault(rol => rol.Nombre == "Usuario");

        // Crear la lista para el DropDownList
        if (usuarioRol != null)
        {
            ViewBag.Rol = new SelectList(new[] { usuarioRol }, "Nombre", "Nombre", rolSelecionado);
        }
        else
        {
            ViewBag.Rol = new SelectList(Enumerable.Empty<object>(), "Nombre", "Nombre");
        }
}
}