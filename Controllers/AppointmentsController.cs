using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BeautySalon.Models;
using System.Globalization;

namespace BeautySalon.Controllers
{
    public class AppointmentsController : Controller
    {
        private SalonDatabaseEntities db = new SalonDatabaseEntities();

        // GET: Appointments
        public ActionResult Index()
        {
            var appointments = db.Appointments.Include(a => a.Service).Include(a => a.User);
            return View(appointments.ToList());
        }
        public ActionResult MyIndex()
        {
            int uid = Convert.ToInt32(Session["user_id"].ToString());
            var appointments = db.Appointments.Include(a => a.Service).Include(a => a.User).Where(x=>x.Ap_UserId==uid);
            return View(appointments.ToList());
        }
        // GET: Appointments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // GET: Appointments/Create
        public ActionResult Create()
        {
            if (Session["user_id"] != null)
            {
                ViewBag.Ap_Service = new SelectList(db.Services, "ServiceId", "ServiceName");
                ViewBag.Ap_UserId = new SelectList(db.Users, "user_id", "user_name");
                ViewBag.Message = "";
                return View();
            }
            else
            {
                ViewBag.Message = "Please Login first!";
                return RedirectToAction("Login", "Home");

            }
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Ap_Id,Ap_UserId,Ap_Service,Ap_Date")] Appointment appointment,string ap_date)
        {
            if (ModelState.IsValid)
            {
                //var tempdate = DateTime.ParseExact(ap_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //var timspan = TimeSpan.Parse(ap_time);
                //DateTime d = tempdate.Add(timspan);
                DateTime d = Convert.ToDateTime(ap_date);
                List<Appointment> lst = db.Appointments.Where(x => x.Ap_Date == d && x.Ap_Service == appointment.Ap_Service).ToList();
                if (lst.Count() < 3)
                {
                    int uid = Convert.ToInt32(Session["user_id"].ToString());
                    appointment.Ap_UserId = uid;
                    appointment.Ap_Date = Convert.ToDateTime(ap_date);
                    db.Appointments.Add(appointment);
                    db.SaveChanges();
                    return RedirectToAction("MyIndex");
                }
                else
                {
                    ViewBag.Ap_Service = new SelectList(db.Services, "ServiceId", "ServiceName", appointment.Ap_Service);
                    ViewBag.Ap_UserId = new SelectList(db.Users, "user_id", "user_name", appointment.Ap_UserId);
                    ViewBag.Message = "Slots are already booked.Please select another time";
                    return View(appointment);
                }
            }

            ViewBag.Ap_Service = new SelectList(db.Services, "ServiceId", "ServiceName", appointment.Ap_Service);
            ViewBag.Ap_UserId = new SelectList(db.Users, "user_id", "user_name", appointment.Ap_UserId);
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            ViewBag.Ap_Service = new SelectList(db.Services, "ServiceId", "ServiceName", appointment.Ap_Service);
            ViewBag.Ap_UserId = new SelectList(db.Users, "user_id", "user_name", appointment.Ap_UserId);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Ap_Id,Ap_UserId,Ap_Service,Ap_Date")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Ap_Service = new SelectList(db.Services, "ServiceId", "ServiceName", appointment.Ap_Service);
            ViewBag.Ap_UserId = new SelectList(db.Users, "user_id", "user_name", appointment.Ap_UserId);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            db.Appointments.Remove(appointment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
