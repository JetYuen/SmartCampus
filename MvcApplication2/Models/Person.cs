using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication2.Models
{
    public class Person
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Card { get; set; }

        public override string ToString()
        {
            return string.Format("{{Id:{0}, Name:{1}, Card:{2}}}", this.Id, this.Name, this.Card);
        }
    }
}