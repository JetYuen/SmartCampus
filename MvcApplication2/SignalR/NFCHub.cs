using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace MvcApplication2.SignalR
{
    public class NFCHub : Hub
    {
        public void CardIDCheck(string check)
        {
            Clients.All.cardIDCheck(check);

        }

        public void UpdateNameStatus(string status2)
        {
            Clients.All.updateNameStatus(status2);

        }
        public void UpdateDateStatus(string status3)
        {
            Clients.All.updateDateStatus(status3);

        }
        public void UpdatePCNumStatus(string status4)
        {
            Clients.All.updatePCNumStatus(status4);

        }
        public void UpdateTimeStatus(string status5)
        {
            Clients.All.updateTimeStatus(status5);

        }
    }
}