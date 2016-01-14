using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ColorWars.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public IList<Couleur> Couleurs { get; set; }
        public IList<Squad> Squads { get; set; }
        public IList<Bataille> BataillesPrecedentes { get; set; }
    }
}