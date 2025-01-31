using System.Security.Claims;
using frontendnet.Models;
using frontendnet.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace frontendnet;

public class AuthController(AuthClientService auth) : Controller{
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
}