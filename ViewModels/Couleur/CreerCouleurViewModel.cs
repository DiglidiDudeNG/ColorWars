using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ColorWars.ViewModels.Squad;
using ColorWars.Models;
using ColorWars.Services;

namespace ColorWars.ViewModels.Couleur
{
    public class CreerCouleurViewModel
    {
        /*
        private const int DEFAULT_FORCE = 25;
        private const int DEFAULT_DEXTERITE = 25;
        private const int DEFAULT_ENDURANCE = 25;
        private const int DEFAULT_NB_PTS_RESTANTS = 50;
        */

        private const int DEFAULT_ROUGE_BASE = 25;
        private const int DEFAULT_VERT_BASE = 25;
        private const int DEFAULT_BLEU_BASE = 25;

        [Key]
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nom de la couleur")]
        public string Nom { get; set; }

        // La collection de stats!!!

        public MainStatEnCreationViewModel Rouge { get; set; }

        public MainStatEnCreationViewModel Vert { get; set; }

        public MainStatEnCreationViewModel Bleu { get; set; }

        /*
        [Range(Models.Couleur.StatMinValue, Models.Couleur.StatMaxValue)]
        [DefaultValue(DEFAULT_FORCE)]
        public int Force { get; set; }

        [Display(Name = "Dextérité")]
        [Range(Models.Couleur.StatMinValue, Models.Couleur.StatMaxValue)]
        [DefaultValue(DEFAULT_DEXTERITE)]
        public int Dexterite { get; set; }

        [Range(Models.Couleur.StatMinValue, Models.Couleur.StatMaxValue)]
        [DefaultValue(DEFAULT_ENDURANCE)]
        public int Endurance { get; set; }

        [DefaultValue(DEFAULT_NB_PTS_RESTANTS)]
        public int NbPtsRestants { get; set; }
        */

        /// <summary>
        /// Construit un modèle de vue pour la création d'une couleur lors de l'accès initial de la page.
        /// </summary>
        public CreerCouleurViewModel()
        {
            Rouge = new Rouge( );


            /*
            Force = DEFAULT_FORCE;
            Dexterite = DEFAULT_DEXTERITE;
            Endurance = DEFAULT_ENDURANCE;
            NbPtsRestants = DEFAULT_NB_PTS_RESTANTS;
            */

        }

        /// <summary>
        /// Construit un modèle de vue pour la création d'une couleur qui reçoit des arguments.
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="force"></param>
        /// <param name="dexterite"></param>
        /// <param name="endurance"></param>
        /// <param name="nbPtsRestants"></param>
        public CreerCouleurViewModel(string nom, StatCollection stats, int nbPtsRestants)
        {
            /*
            Nom = nom;
            Force = force;
            Dexterite = dexterite;
            Endurance = endurance;
            NbPtsRestants = nbPtsRestants;
            */

        }
    }

    public class MainStatEnCreationViewModel
    {
        
    }

    public class SubStatEnCreationViewModel
    {

    }
}
