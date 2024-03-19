using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TajamarProyecto.Data;
using TajamarProyecto.Extensions;
using TajamarProyecto.Helpers;
using TajamarProyecto.Models;
using TajamarProyecto.Repositories;

namespace TajamarProyecto.Controllers
{
    public class UsuariosController : Controller
    {
        private RepositoryUsuarios repo;
        private HelperMails helperMails;
        private HelperPathProvider helperPathProvider;
        private readonly IHttpContextAccessor httpContextAccessor;
     


        public UsuariosController(RepositoryUsuarios repo, HelperMails helperMails, HelperPathProvider helperPathProvider)
        {
            this.helperPathProvider = helperPathProvider;
            this.helperMails = helperMails;
            this.repo = repo;
            this.httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register
            (string nombre, string email, string password, string linkedin)
        {
            Usuario user = await this.repo.RegisterUserAsync(nombre, email
                , password, linkedin);
            string serverUrl = this.helperPathProvider.MapUrlServerPath();
            //https://localhost:8555/Usuarios/ActivateUser/TOKEN???
            serverUrl = serverUrl + "/Usuarios/ActivateUser/token?token=" + user.TokenMail;
            string mensaje = "<h3>Buenas, "+user.Nombre+"</h3>";
            mensaje += "<p>Debe activar su cuenta con nosotros pulsando el siguiente enlace</p>";
            mensaje += "<a  href='" + serverUrl + "'>" + serverUrl + "</a>";
            mensaje += "<p>Muchas gracias</p>";
            await this.helperMails.SendMailAsync(email, "Registro Usuario", mensaje);
            ViewData["MENSAJE"] = "Usuario registrado correctamente. " +
                " Hemos enviado un mail para activar su cuenta";
            return View();
        }


        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(string email, string password)
        {
            Usuario user = await this.repo.LogInUserAsync(email, password);
            if (user == null)
            {
                ViewData["MENSAJE"] = "Credenciales incorrectas";
                return View();
            }
            else
            {
                // Guardar el usuario en la sesión
                HttpContext.Session.SetString("NOMBREUSUARIO", user.Nombre);
                HttpContext.Session.SetString("IDUSUARIO", user.IdUsuario.ToString());
                HttpContext.Session.SetString("ROLE", user.Role);
                HttpContext.Session.SetString("LINKEDIN", user.Linkedin);
                HttpContext.Session.SetString("EMAIL", user.Email);
                HttpContext.Session.SetString("EMPRESA1", user.Emp_1Id.ToString());
                HttpContext.Session.SetString("EMPRESA2", user.Emp_2Id.ToString());
                HttpContext.Session.SetString("EMPRESA3", user.Emp_3Id.ToString());
                HttpContext.Session.SetString("EMPRESA4", user.Emp_4Id.ToString());
                HttpContext.Session.SetString("EMPRESA5", user.Emp_5Id.ToString());
                HttpContext.Session.SetString("EMPRESA6", user.Emp_6Id.ToString());
                //return View(user);
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult LogOut()
        {
            // Eliminar todas las sesiones
            HttpContext.Session.Clear();
           

            // Redirigir al usuario a la página de inicio de sesión
            return RedirectToAction("LogIn", "Usuarios");
        }

        public async Task<IActionResult> ActivateUser(string token)
        {
            await this.repo.ActivateUserAsync(token);
            ViewData["MENSAJE"] = "Cuenta activada correctamente";
            return View();
        }


        //public IActionResult Perfil()
        //{
        //    return View();
        //}

        public IActionResult Perfil()
        {
            // Obtener el ID del usuario de la sesión
            int idUsuario = int.Parse(HttpContext.Session.GetString("IDUSUARIO"));

            // Buscar el usuario en el repositorio usando el ID
            Usuario usuario = repo.FindUsuario(idUsuario);

            // Verificar si se encontró el usuario
            if (usuario == null)
            {
                // Si no se encuentra el usuario, puedes devolver una vista de error o redirigir a otra página
                return NotFound(); // Devuelve un error 404
            }

            // Si se encuentra el usuario, pasarlo a la vista
            return View(usuario); // Pasar el objeto usuario a la vista Perfil
        }

        public async Task<IActionResult> Entrevistas()
        {
            int idUsuario = int.Parse(HttpContext.Session.GetString("IDUSUARIO"));
            EntrevistaAlumno entrevista = await repo.FindEntrevistaAsync(idUsuario);
            if (entrevista == null)
            {
                // Si no se encuentra la entrevista, puedes devolver una vista de error o redirigir a otra página
                return NotFound(); // Devuelve un error 404
            }

            // Si se encuentra la entrevista, pasarla a la vista
            return View(entrevista);
        }

        //scafolding

        public IActionResult AlumnosList()
        {
            List<Usuario> usuarios = this.repo.GetUsuarios();
            return View(usuarios);
        }

        
        public IActionResult Details(int id)
        { 
         Usuario usuario = this.repo.FindUsuario(id);
           return View(usuario);
           
        }
        public IActionResult _Details(int id)
        {
            Usuario usuario = this.repo.FindUsuario(id);
            // return View(usuario);
            return PartialView("_Details", usuario);
        }


        


        public async Task<IActionResult> InsertarEmpresaAlumno(int? idempresa1, int? idempresa2, int? idempresa3, int? idempresa4, int? idempresa5, int? idempresa6)
        {
            int idUsuario = int.Parse(HttpContext.Session.GetString("IDUSUARIO"));
            await repo.InsertarEmpresasEnUsuario(idUsuario, idempresa1, idempresa2, idempresa3, idempresa4, idempresa5, idempresa6);

            // Aquí puedes devolver una respuesta JSON u otro tipo de respuesta según tu necesidad
            return View();
        }



      
       

    }
}
