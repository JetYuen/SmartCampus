using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;

namespace MvcApplication2.Models
{
    public class Person
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Card { get; set; }

        public string Date { get; set; }

        public string PCNum { get; set; }

        public string State { get; set; }

        public override string ToString()
        {
            return string.Format("{{Id:{0}, Name:{1}, Card:{2}, Date:{3}, PCNum:{4}, Time:{5}}}", this.Id, this.Name, this.Card, this.Date, this.PCNum, this.State);
        }
    }

}