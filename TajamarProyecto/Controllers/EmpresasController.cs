using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TajamarProyecto.Data;
using TajamarProyecto.Models;
using TajamarProyecto.Repositories;

namespace TajamarProyecto.Controllers
{
    public class EmpresasController : Controller
    {
        private RepositoryEmpresa repo;
        private UsuariosContext context;
  

        public EmpresasController(RepositoryEmpresa repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            List<Empresa> empresas = this.repo.GetEmpresas();
            return View(empresas);
        }


        public IActionResult EmpresasAlumnos()
        {
            List<Empresa> empresas = this.repo.GetEmpresas();
            return View(empresas);
        }



        //public IActionResult EmpresasSeleccionadas(int[] empresas)
        //{
        //    // Recuperar las empresas de la base de datos
        //    var empresasSeleccionadas = context.Empresas.Where(x => empresas.Contains(x.IdEmpresa)).ToList();

        //    // Mostrar las empresas en una vista o realizar otra acción
        //    return View(empresasSeleccionadas);
        //}
        //public IActionResult _EmpresasSeleccionadas(int idempresa)
        //{
        //    var consulta = this.repo.GetEmpresaById(idempresa);
        //    return PartialView("_EmpresasSeleccionadas", consulta);
        //}

        public class EmpresaSeleccionada
        {
            public int IdEmpresa { get; set; }
        }

        public async Task<IActionResult> _AlumnosSeleccionados(int idEmpresa)
        {
            var usuariosTask = this.repo.FindUsuariosPorEmpresa(idEmpresa);
            var usuarios = await usuariosTask;
            return PartialView("_AlumnosSeleccionados", usuarios);
        }


        public IActionResult _EmpresasSeleccionadas(int? idempresa1, int? idempresa2, int? idempresa3, int? idempresa4, int? idempresa5, int? idempresa6)
        {
            List<Empresa> empresas = new List<Empresa>();

            if (idempresa1.HasValue)
            {
                var empresa1 = this.repo.GetEmpresaById(idempresa1.Value);
                if (empresa1 != null)
                {
                    empresas.Add(empresa1);
             
                }
            }

            if (idempresa2.HasValue)
            {
                var empresa2 = this.repo.GetEmpresaById(idempresa2.Value);
                if (empresa2 != null)
                {
                    empresas.Add(empresa2);
                 
                }
            }

            if (idempresa3.HasValue)
            {
                var empresa3 = this.repo.GetEmpresaById(idempresa3.Value);
                if (empresa3 != null)
                {
                    empresas.Add(empresa3);
                  
                }
            }

            if (idempresa4.HasValue)
            {
                var empresa4 = this.repo.GetEmpresaById(idempresa4.Value);
                if (empresa4 != null)
                {
                    empresas.Add(empresa4);
              
                }
            }

            if (idempresa5.HasValue)
            {
                var empresa5 = this.repo.GetEmpresaById(idempresa5.Value);
                if (empresa5 != null)
                {
                    empresas.Add(empresa5);
            
                }
            }

            if (idempresa6.HasValue)
            {
                var empresa6 = this.repo.GetEmpresaById(idempresa6.Value);
                if (empresa6 != null)
                {
                    empresas.Add(empresa6);
                 
                }
            }

            return PartialView("_EmpresasSeleccionadas", empresas);
        }


        


        //public IActionResult _EmpresasSeleccionadas(int? idemp1, int? idemp2, int? idemp3, int? idemp4, int? idemp5, int? idemp6)
        //{
        //    // Recuperar las empresas de la base de datos
        //    var empresasSeleccionadas = context.Empresas.Where(x => empresas.Contains(x.IdEmpresa)).ToList();

        //    // Mostrar las empresas en una vista o realizar otra acción
        //    return PartialView("_EmpresasSeleccionadas", empresasSeleccionadas);
        //}
    }
}
//FirstOrDefault(x => x.IdCoche == id);