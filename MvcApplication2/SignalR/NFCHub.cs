using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace MvcApplication2.SignalR
{
    public class NFCHub : Hub
    {
        public object UpdateNFCStatus(bool status)
        {
            this.Clients.All.updateStatus(status);

            return new
            {
                Foo = "Bar",
                Now = DateTime.UtcNow
            };
        }
    }
}