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


namespace MvcApplication2.Controllers
{

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
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

        [HttpPost]
        public ActionResult Add(string name, string card)
        {
            var store = MvcApplication.PeopleStore;
            lock (store)
            {
                store.Add(new Person
                {
                    Name = name,
                    Card = card
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

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var store = MvcApplication.PeopleStore;
            lock (store)
            {
                var personToEdit = store.People.First(person => person.Id == id);
                return View(personToEdit);
            }
        }

        [HttpPost]
        public ActionResult Edit(int id, string name, string card)
        {
            var store = MvcApplication.PeopleStore;
            lock (store)
            {
                var personToEdit = store.People.First(person => person.Id == id);
                if (ModelState.IsValid == false)
                    return View(personToEdit);

                personToEdit.Name = name;
                personToEdit.Card = card;
                store.SaveChanges();

                return RedirectToAction("Index");
            }
        }

        public ActionResult OnButtonTestNFC(string currentNFC)
        {
            if (currentNFC == "true") MvcApplication2.SignalR.NFC.Instance.UpdateNFCStatus("false");
            else MvcApplication2.SignalR.NFC.Instance.UpdateNFCStatus("true");
            return null;
        }
    }
}









