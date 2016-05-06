using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using MvcApplication2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication2.SignalR
{
    public class NFC
    {

        private readonly static Lazy<NFC> _instance = new Lazy<NFC>(() => new NFC(GlobalHost.ConnectionManager.GetHubContext<NFCHub>().Clients));

        private IHubConnectionContext Clients { get; set; }

        private NFC(IHubConnectionContext clients)
        {
            Clients = clients;
        }

        public static NFC Instance
        {
            get { return _instance.Value; }
        }
        public void CardIDCheck(string check)
        {
            Clients.All.cardIDCheck(check);
        }

        public void UpdateNameStatus(Person status2)
        {
            Clients.All.updateNameStatus(status2);
        }
        public void UpdateDateStatus(Person status3)
        {
            Clients.All.updateDateStatus(status3);
        }
        public void UpdatePCNumStatus(Person status4)
        {
            Clients.All.updatePCNumStatus(status4);
        }
        public void UpdateTimeStatus(Person status5)
        {
            Clients.All.updateTimeStatus(status5);
        }
    }
}