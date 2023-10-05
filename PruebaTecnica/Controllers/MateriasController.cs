using PruebaTecnica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static PruebaTecnica.Models.Enum;

namespace PruebaTecnica.Controllers
{
    public class MateriasController : Controller
    {
        // GET: Materias
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListarMaterias()
        {
            List<Materia> list = new List<Materia>();
            using (Entities db = new Entities())
            {
                list = (from c in db.Materias

                        select new Materia
                        {
                            ID_Materia = c.ID_Materia,
                            materia = c.Materia,
                            creditos = c.Creditos
                            
                        }).ToList();
            }
           
            ViewBag.titulo = "Lista Materias";
            return View(list);
            

        }


        [HttpGet]
        public ActionResult Eliminar_Materia(int id)
        {
            try
            {

                using (Entities context = new Entities())
                {
                    PruebaTecnica.Models.Materias _materia = context.Materias.Where(
                        f => f.ID_Materia == id).FirstOrDefault();
                    context.Materias.Remove(_materia);
                    context.SaveChanges();
                }

                Alert("Materia eliminada con exito", NotificationType.success);
                return Redirect("~/Materias/ListarMaterias");
            }
            catch (Exception ex)
            {
                Alert("Error al eliminar materia" + ex.Message, NotificationType.error);
                return Redirect("~/Materias/Listarmaterias");
            }
        }

        public void Alert(string message, NotificationType notificationType)
        {
            var msg = "<script language='javascript'>Swal.fire('" + notificationType.ToString().ToUpper() +
                "','" + message + "','" + notificationType + "')" + "</script>";
            TempData["notification"] = msg;
        }

    }
}