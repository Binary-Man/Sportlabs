using SportsLabs.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsLabs.ViewModels
{
    public class TeamViewModel
    {
        //public int Id { get; set; }
        //public string Name { get; set; }
        //public bool Eliminated { get; set; }
        //public List<string> Countries { get; set; }
        public List<Team> Teams { get; set; }
        public string Notification { get; set; }
    }
}