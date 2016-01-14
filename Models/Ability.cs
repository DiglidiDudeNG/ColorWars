using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ColorWars.Models
{
    public class Ability
    {
        [Key]
        public int Id { get; set; }

        public string Nom { get; set; }
        
        // TODO les abilities.
    }
}
