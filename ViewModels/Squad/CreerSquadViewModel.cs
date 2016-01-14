using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ColorWars.Models;
using ColorWars.ViewModels.Couleur;

namespace ColorWars.ViewModels.Squad
{
    public class CreerSquadViewModel
    {
        public IList<CreerCouleurViewModel> Couleurs { get; set; }
    }
}