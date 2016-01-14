using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ColorWars.ViewModels.Couleur;
using Microsoft.AspNet.Mvc;

namespace ColorWars.ViewComponents.Squads
{
    [ViewComponent(Name="CreerCouleur")]
    public class CreerCouleurViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(CreerCouleurViewModel model)
        {
            return View(model);
        }
    }
}
