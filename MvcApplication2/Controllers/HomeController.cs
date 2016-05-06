// use TinyGPIO.cs
// https://github.com/sample-by-jsakamoto/SignalR-on-RaspberryPi/blob/master/myapp/TinyGPIO.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using MvcApplication2;
using MvcApplication2.SignalR;
using System.Diagnostics;
using MvcApplication2.PInvoke;
using System.Runtime.InteropServices;
using SharpNFC;
using MvcApplication2.Models;
using System.IO;
using Newtonsoft.Json;




namespace MvcApplication2.Controllers
{

    public class HomeController : Controller
    {
        public ActionResult Index()
        {   
            List<SelectListItem> PCList = new List<SelectListItem>();
            PCList.Add(new SelectListItem { Text = "PC#1", Value = "PC#1" });
            PCList.Add(new SelectListItem { Text = "PC#2", Value = "PC#2" });
            PCList.Add(new SelectListItem { Text = "PC#3", Value = "PC#3" });
            PCList.Add(new SelectListItem { Text = "PC#4", Value = "PC#4" });
            PCList.Add(new SelectListItem { Text = "PC#5", Value = "PC#5" });
            ViewData["pcNum"] = PCList;

            var store = MvcApplication.PeopleStore;
            lock (store)
            {
                return View("Index", store.People.ToArray());
            }

        }

        public ActionResult NFC()
        {
            {
                return View();
            }
        }
        public ActionResult Log()
        {
            var store = MvcApplication.PeopleStore;
            lock (store)
            {
                return View("Log", store.People.ToArray());
            }
        }

        [HttpPost]
        public ActionResult Add(string name, string card, string date, string pcnum, string state)
        {   
            List<SelectListItem> PCList = new List<SelectListItem>();
            PCList.Add(new SelectListItem {Text = "PC#1", Value = "PC#1"});
            PCList.Add(new SelectListItem {Text = "PC#2", Value = "PC#2"});
            PCList.Add(new SelectListItem {Text = "PC#3", Value = "PC#3"});
            PCList.Add(new SelectListItem {Text = "PC#4", Value = "PC#4"});
            PCList.Add(new SelectListItem {Text = "PC#5", Value = "PC#5"});
            ViewData["pcNum"] = PCList;


            var store = MvcApplication.PeopleStore;
            lock (store)
            {
                store.Add(new Person
                {
                    Name = name,
                    Card = card,
                    Date = date,
                    PCNum = pcnum,
                    State = state
                });
     
                store.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var store = MvcApplication.PeopleStore;
            lock (store)
            {
                store.Delete(id);
                store.SaveChanges();
            }

            return RedirectToAction("Log");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            List<SelectListItem> PCList = new List<SelectListItem>();
            PCList.Add(new SelectListItem {Text = "PC#1", Value = "PC#1"});
            PCList.Add(new SelectListItem {Text = "PC#2", Value = "PC#2"});
            PCList.Add(new SelectListItem {Text = "PC#3", Value = "PC#3"});
            PCList.Add(new SelectListItem {Text = "PC#4", Value = "PC#4"});
            PCList.Add(new SelectListItem {Text = "PC#5", Value = "PC#5"});
            ViewData["pcNum"] = PCList;


            var store = MvcApplication.PeopleStore;
            lock (store)
            {
                var personToEdit = store.People.First(person => person.Id == id);
                return View(personToEdit);
            }

        }
        [HttpPost]
        public ActionResult Edit(int id, string name, string card, string date, string pcnum, string state)
        {

            var store = MvcApplication.PeopleStore;
            lock (store)
            {
                var personToEdit = store.People.First(person => person.Id == id);
                if (ModelState.IsValid == false)
                    return View(personToEdit);

                personToEdit.Name = name;
                personToEdit.Card = card;
                personToEdit.Date = date;
                personToEdit.PCNum = pcnum;
                personToEdit.State = state;
                store.SaveChanges();

                return RedirectToAction("Log");
            }


        }
        public ActionResult OnButtonTest(string current)
        {
            if (current == "true") MvcApplication2.SignalR.NFC.Instance.CardIDCheck("false");
            else MvcApplication2.SignalR.NFC.Instance.CardIDCheck("true");
            return null;
        }

        public ActionResult OnButtonTestName(string currentname)
        {
            Person p = null;
            if (currentname == "true") MvcApplication2.SignalR.NFC.Instance.UpdateNameStatus(p);
            else MvcApplication2.SignalR.NFC.Instance.UpdateNameStatus(p);
            return null;
        }
        public ActionResult OnButtonTestDate(string currentdate)
        {
            Person p = null;
            if (currentdate == "true") MvcApplication2.SignalR.NFC.Instance.UpdateDateStatus(p);
            else MvcApplication2.SignalR.NFC.Instance.UpdateDateStatus(p);
            return null;
        }
        public ActionResult OnButtonTestPCNum(string currentpcnum)
        {
            Person p = null;
            if (currentpcnum == "true") MvcApplication2.SignalR.NFC.Instance.UpdatePCNumStatus(p);
            else MvcApplication2.SignalR.NFC.Instance.UpdatePCNumStatus(p);
            return null;
        }
        public ActionResult OnButtonTestTime(string currenttime)
        {
            Person p = null;
            if (currenttime == "true") MvcApplication2.SignalR.NFC.Instance.UpdateTimeStatus(p);
            else MvcApplication2.SignalR.NFC.Instance.UpdateTimeStatus(p);
            return null;
        }

        public JsonResult GetStates(string id)
        {
            List<SelectListItem> states = new List<SelectListItem>();
            switch (id)
            {
                case "PC#1":
                    states.Add(new SelectListItem { Text = "09:00-11:00", Value = "09:00-11:00" });
                    states.Add(new SelectListItem { Text = "11:00-13:00", Value = "11:00-13:00" });
                    states.Add(new SelectListItem { Text = "13:00-15:00", Value = "13:00-15:00" });
                    states.Add(new SelectListItem { Text = "15:00-17:00", Value = "15:00-17:00" });
                    states.Add(new SelectListItem { Text = "17:00-19:00", Value = "17:00-19:00" });
                    states.Add(new SelectListItem { Text = "19:00-21:00", Value = "19:00-21:00" });
                    break;
                case "PC#2":
                    states.Add(new SelectListItem { Text = "09:00-11:00", Value = "09:00-11:00" });
                    states.Add(new SelectListItem { Text = "11:00-13:00", Value = "11:00-13:00" });
                    states.Add(new SelectListItem { Text = "13:00-15:00", Value = "13:00-15:00" });
                    states.Add(new SelectListItem { Text = "15:00-17:00", Value = "15:00-17:00" });
                    states.Add(new SelectListItem { Text = "17:00-19:00", Value = "17:00-19:00" });
                    states.Add(new SelectListItem { Text = "19:00-21:00", Value = "19:00-21:00" });
                    break;
                case "PC#3":
                    states.Add(new SelectListItem { Text = "09:00-11:00", Value = "09:00-11:00" });
                    states.Add(new SelectListItem { Text = "11:00-13:00", Value = "11:00-13:00" });
                    states.Add(new SelectListItem { Text = "13:00-15:00", Value = "13:00-15:00" });
                    states.Add(new SelectListItem { Text = "15:00-17:00", Value = "15:00-17:00" });
                    states.Add(new SelectListItem { Text = "17:00-19:00", Value = "17:00-19:00" });
                    states.Add(new SelectListItem { Text = "19:00-21:00", Value = "19:00-21:00" });
                    break;
                case "PC#4":
                    states.Add(new SelectListItem { Text = "09:00-11:00", Value = "09:00-11:00" });
                    states.Add(new SelectListItem { Text = "11:00-13:00", Value = "11:00-13:00" });
                    states.Add(new SelectListItem { Text = "13:00-15:00", Value = "13:00-15:00" });
                    states.Add(new SelectListItem { Text = "15:00-17:00", Value = "15:00-17:00" });
                    states.Add(new SelectListItem { Text = "17:00-19:00", Value = "17:00-19:00" });
                    states.Add(new SelectListItem { Text = "19:00-21:00", Value = "19:00-21:00" });
                    break;
                case "PC#5":
                    states.Add(new SelectListItem { Text = "09:00-11:00", Value = "09:00-11:00" });
                    states.Add(new SelectListItem { Text = "11:00-13:00", Value = "11:00-13:00" });
                    states.Add(new SelectListItem { Text = "13:00-15:00", Value = "13:00-15:00" });
                    states.Add(new SelectListItem { Text = "15:00-17:00", Value = "15:00-17:00" });
                    states.Add(new SelectListItem { Text = "17:00-19:00", Value = "17:00-19:00" });
                    states.Add(new SelectListItem { Text = "19:00-21:00", Value = "19:00-21:00" });
                    break;

            }
            return Json(new SelectList(states, "Value", "Text"));
        }
        public ActionResult InsertDetails(FormCollection collection)
        {
            //DO LOGIC TO INSERT DETAILS
            ViewBag.result = "Record Inserted Successfully!";
            return View("Index");
        }
    }
}








