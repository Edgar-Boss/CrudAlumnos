using PruebaTecnica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static PruebaTecnica.Models.Enum;

namespace PruebaTecnica.Controllers
{
    public class MateriasAlumnosController : Controller
    {
        // GET: MateriasAlumnos
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult ListarAlumnos()
        {
            List<Alumno> list = new List<Alumno>();
            using (Entities db = new Entities())
            {
                list = (from c in db.Alumnos

                        select new Alumno
                        {
                            ID_Alumno = c.ID_Alumno,
                            Nombre = c.Nombre,
                            Apaterno = c.Apaterno,
                            Amaterno = c.Amaterno,

                        }).ToList();
            }
            ViewBag.titulo = "Lista Alumnos";
            return View(list);

        }

        public ActionResult ListarMaterias(int id)
        {
            List<Materia> list = new List<Materia>();
            string alumno_name = "";
            using (Entities db = new Entities())
            {
                list = (from c in db.alumnmatrias_piv
                        join ch in db.Alumnos on c.Alumno_id equals ch.ID_Alumno
                        where c.Alumno_id == id
                        select new Materia
                        {
                            ID_Materia = (int)c.Materia_id,
                            

                        }).ToList();

                list = (from c in list
                        join ch in db.Materias on c.ID_Materia equals ch.ID_Materia
                        where c.ID_Materia == ch.ID_Materia
                        select new Materia
                        {
                            ID_Materia = ch.ID_Materia,
                            materia = ch.Materia
                        }).ToList();

                foreach (var item in db.Alumnos)
                {
                    if (item.ID_Alumno == id)
                        alumno_name = item.Nombre+" "+item.Apaterno+" "+item.Amaterno;
                }
            }



            ViewBag.titulo = alumno_name;
            return View(list);

        }


        [HttpGet]
        public ActionResult Eliminar_Alumno(int id)
        {
            try
            {

                using (Entities context = new Entities())
                {
                    PruebaTecnica.Models.Alumnos _alumno = context.Alumnos.Where(
                        f => f.ID_Alumno == id).FirstOrDefault();
                    context.Alumnos.Remove(_alumno);
                    context.SaveChanges();
                }

                Alert("Alumno Eliminado con exito", NotificationType.success);
                return Redirect("~/MateriasAlumnos/ListarAlumnos");
            }
            catch (Exception ex) 
            {
                Alert("Error al eliminar alumno" + ex.Message, NotificationType.error);
                return Redirect("~/MateriasAlumnos/ListarAlumnos");
            }
        }

        public ActionResult Editar_Alumno(int id)
        {
            PruebaTecnica.Models.Alumnos alumno = new PruebaTecnica.Models.Alumnos();


            ViewBag.titulo = "Editar Alumno";
            return View();
        }




        public ActionResult Nuevo_Alumno()
        {


            ViewBag.titulo = "Nueva materia";
            return View();
        }


        [HttpPost]
        public ActionResult Nuevo_Alumno(Alumno model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (Entities context = new Entities())
                    {
                        var alumnos = new PruebaTecnica.Models.Alumnos();
                        alumnos.ID_Alumno= model.ID_Alumno;
                        alumnos.Nombre = model.Nombre;
                        alumnos.Apaterno = model.Apaterno;
                        alumnos.Amaterno = model.Amaterno;
                        alumnos.Telefono = model.Telefono;
                       



                        context.Alumnos.Add(alumnos);
                        context.SaveChanges();
                        Alert("Alumno registrado con exito", NotificationType.success);
                        return Redirect("~/MateriasAlumnos/ListarAlumnos");
                    }
                }
                else
                {

                    Alert("Datos no válidos", NotificationType.warnig);

                    return View(model);
                }
            }
            catch (Exception ex)
            {
                Alert("Error" + ex.Message, NotificationType.error);

                return View(model);
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