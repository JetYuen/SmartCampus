﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
// using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using MvcApplication2.SignalR;
using System.Diagnostics;
using SharpNFC;
using MvcApplication2.PInvoke;
using System.Runtime.InteropServices;
using MvcApplication2.Services;
using System.Web.WebPages.Scope;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;
using MvcApplication2.Models;


namespace MvcApplication2
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static PeopleStore PeopleStore { get; private set; }
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            MvcApplication.PeopleStore = new PeopleStore();
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            MvcApplication.PeopleStore = new PeopleStore();
            RouteTable.Routes.MapHubs();

            AreaRegistration.RegisterAllAreas();

            // Use LocalDB for Entity Framework by default
            // Database.DefaultConnectionFactory = new SqlConnectionFactory(@"Data Source=(localdb)\v11.0; Integrated Security=True; MultipleActiveResultSets=True");

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            InitiGPIO();

            Thread threadSmartCard = new Thread(new ThreadStart(ThreadSmartCard));
            threadSmartCard.Start();

            return;
        }
        protected void Application_BeginRequest()
        {
            typeof(AspNetRequestScopeStorageProvider)
                .Assembly.GetType("System.Web.WebPages.WebPageHttpModule")
                .GetProperty("AppStartExecuteCompleted", BindingFlags.NonPublic | BindingFlags.Static)
                .SetValue(null, true, null);
        }

        public static TinyGPIO GPIO23_buz { get; set; }
        public static TinyGPIO GPIO25_LED1 { get; set; }
        public static NFCState CurrentNFC { get; set; }
        public static string Arguments { get; set; }
        public static string FileName { get; set; }
        public static bool RedirectStandardOutput { get; set; }
        public static bool UseShellExecute { get; set; }

        private static void InitiGPIO()
        {
            // init GPIO 25 for LED1
            GPIO25_LED1 = TinyGPIO.Export(25);
            GPIO25_LED1.Direction = GPIODirection.Out;

            // init GPIO 23 for Buzzer
            GPIO23_buz = TinyGPIO.Export(23);
            GPIO23_buz.Direction = GPIODirection.Out;
        }

        static private void ThreadSmartCard()
        {
            var gpio23 = TinyGPIO.Export(23);
            gpio23.Direction = (GPIODirection)GPIODirection.Out;
            var gpio25 = TinyGPIO.Export(25);
            gpio25.Direction = (GPIODirection)GPIODirection.Out;

            List<string> deviceNameList = new List<string>();
            NFCContext nfcContext = new NFCContext();
            NFCDevice nfcDevice = nfcContext.OpenDevice(null);
            deviceNameList = nfcContext.ListDeviceNames();
            int rtn = nfcDevice.initDevice();
            nfc_target nfcTarget = new nfc_target();
            List<nfc_modulation> nfc_modulationList = new List<nfc_modulation>();
            nfc_modulation nfcModulation = new nfc_modulation();
            nfcModulation.nbr = nfc_baud_rate.NBR_106;
            nfcModulation.nmt = nfc_modulation_type.NMT_ISO14443A;
            nfc_modulationList.Add(nfcModulation);

            string currentSignalRStr = null;
            string currentConsoleStr = null;
            string signalRStr;
            string consoleStr;
            string state = "---";
            Person p;

            for (; ; )
            {
                gpio23.Value = 0;

                Thread.Sleep(100);
                rtn = nfcDevice.Pool(nfc_modulationList, 1, 2, out nfcTarget);
                if (rtn < 0)
                {
                    consoleStr = "NFC Targert Not Found!";
                    signalRStr = "---";
                    gpio23.Value = 0;
                    gpio25.Value = 0;
                }
                else
                {
                    signalRStr = string.Join(
                     separator: "",
                    values: nfcTarget.nti.abtUid.Take((int)nfcTarget.nti.szUidLen).Select(b => b.ToString("X2").ToLower())
                    );
                    consoleStr = string.Format("NFC Target Found: uid is [{0}]", signalRStr);


                    if (File.Exists("App_Data/people.json"))
                    {
                        using (StreamReader r = new StreamReader("App_Data/people.json"))
                        {
                            string json = r.ReadToEnd();
                            List<Person> items = JsonConvert.DeserializeObject<List<Person>>(json);
                            p = items.FirstOrDefault(x => x.Card == signalRStr);
                            Console.WriteLine(p.Name);
                            Console.WriteLine(p.Date);
                            Console.WriteLine(p.PCNum);
                            Console.WriteLine(p.State);

                            NFC.Instance.UpdateDateStatus(p);
                            NFC.Instance.UpdateNameStatus(p);
                            NFC.Instance.UpdatePCNumStatus(p);
                            NFC.Instance.UpdateTimeStatus(p);
                            Thread.Sleep(100);
                        }

                    }

                }
                
                    if (signalRStr != state)
                    {
                        if (signalRStr != currentSignalRStr)
                        {
                            NFC.Instance.CardIDCheck(signalRStr);
                            currentSignalRStr = signalRStr;
                            gpio23.Value = 1;
                            gpio25.Value = 1;
                            Thread.Sleep(100);
                        }
                        else 
                        {
                            gpio23.Value = 0;

                        }
                    }
                    else
                    {
                        gpio23.Value = 0;
                        NFC.Instance.CardIDCheck(signalRStr);
                        currentSignalRStr = signalRStr;
                    }

                    if (consoleStr != currentConsoleStr)
                    {
                        DateTime now = DateTime.Now;
                        FileStream ostrm;
                        StreamWriter writer;
                        TextWriter oldOut = Console.Out;
                        try
                        {
                            ostrm = new FileStream("Record.txt", FileMode.OpenOrCreate, FileAccess.Write);
                            writer = new StreamWriter(ostrm);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Cannot open Record.txt for writing");
                            Console.WriteLine(e.Message);
                            return;
                        }
                        Console.SetOut(writer);
                        Console.WriteLine(now);
                        Console.WriteLine(consoleStr);
                        Console.SetOut(oldOut);
                        writer.Close();
                        ostrm.Close();
                        Console.WriteLine(now);
                        Console.WriteLine(consoleStr);
                        currentConsoleStr = consoleStr;
                    }
                }
            }
        }
    }

