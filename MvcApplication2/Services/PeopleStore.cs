using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using MvcApplication2.Models;

namespace MvcApplication2.Services
{
    public class PeopleStore
    {
        private List<Person> _People;

        public IEnumerable<Person> People { get { return _People; } }

        public string StorePath { get; set; }

        public PeopleStore()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            this.StorePath = Path.Combine(baseDir, "App_Data", "people.json");
            InitialLoad();
        }

        private void InitialLoad()
        {
            if (File.Exists(this.StorePath))
            {
                var jsonText = File.ReadAllText(this.StorePath);
                this._People = JsonConvert.DeserializeObject<List<Person>>(jsonText);
            }
            else
            {
                this._People = new List<Person>();
            }
        }

        public void SaveChanges()
        {
            var jsonText = JsonConvert.SerializeObject(this._People);
            File.WriteAllText(this.StorePath, jsonText);
        }

        public void Add(Person person)
        {
            var nextId = this._People
                .Select(p => p.Id)
                .DefaultIfEmpty(0)
                .Max() + 1;
            person.Id = nextId;
            this._People.Add(person);
        }

        public bool Delete(int id)
        {
            var personToDelete = this._People
                .FirstOrDefault(p => p.Id == id);
            if (personToDelete != null)
            {
                this._People.Remove(personToDelete);
            }
            return personToDelete != null;
        }
    }
}
