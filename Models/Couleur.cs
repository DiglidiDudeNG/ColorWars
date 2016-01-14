using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using ColorWars.ViewModels.Couleur;
using ColorWars.ViewModels.Squad;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Formatters.Json.Internal;

namespace ColorWars.Models
{
    /// <summary>
    /// La classe Couleur.
    /// </summary>
    public class Couleur
    {
        // ----- Constantes -----

        /// <summary>La valeur minimale pour les stats (Force, Dextérité, Endurance)</summary>
        public const int StatMinValue = 25;
        /// <summary>La valeur maximale pour les stats (Force, Dextérité, Endurance)</summary>
        public const int StatMaxValue = 255;

        // -- Privates --
        [NotMapped]
        private int _force { get; set; }
        [NotMapped]
        private int _dexterite { get; set; }
        [NotMapped]
        private int _endurance { get; set; }

        /// <summary>
        /// L'identifiant de la couleur.
        /// </summary>
        [Key]
        public int Id { get; private set; }

        /// <summary>
        /// Le code hexadécimal de la couleur.
        /// </summary>
        [ScaffoldColumn(false)]
        [RegularExpression("^#([A-Fa-f0-9]{6})$")]
        public string CodeHex => GetCodeHex();

        public StatCollection Stats { get; set; }

        /// <summary>
        /// Le niveau de force (Rouge) de la couleur.
        /// </summary>
        [Range(StatMinValue, StatMaxValue)]
        public int Force {
            get { return Stats.Rouge.Total; }
            set { _force = value; }
        }

        /// <summary>
        /// Le niveau de dextérité (Vert) de la couleur.
        /// </summary>
        [Range(StatMinValue, StatMaxValue)]
        public int Dexterite
        {
            get { return _dexterite; }
            set { _dexterite = value; }
        }

        /// <summary>
        /// Le niveau d'endurance (Bleu) de la couleur.
        /// </summary>
        [Range(StatMinValue, StatMaxValue)]
        public int Endurance
        {
            get { return _endurance; }
            set { _endurance = value; }
        }

        /// <summary>
        /// Le niveau de luminence de la couleur.
        /// </summary>
        [ScaffoldColumn(false)]
        [Range(0, 100)]
        public int Luminence { get; private set; }

        /// <summary>
        /// Le niveau de saturation de la couleur.
        /// </summary>
        [Range(0, 100)]
        [ScaffoldColumn(false)]
        public int Saturation { get; private set; }

        /// <summary>
        /// Le niveau du hue de la couleur.
        /// </summary>
        [Range(0, 360)]
        [ScaffoldColumn(false)]
        public int Hue { get; private set; }

        // TODO: Modifier HSL et CodeHex lorsque RGB changent.

        [Display(Name = "Nom de la couleur")]
        public string Nom { get; set; }

        [ScaffoldColumn(false)]
        public int? SquadCouranteId { get; set; }
        public virtual Squad SquadCourante { get; set; }

        [ScaffoldColumn(false)]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        /// <summary>
        /// Génère une string représentant une couleur en hexadécimal basé sur les valeurs des stats
        /// de la couleur.
        /// </summary>
        /// <returns>La string générée.</returns>
        public string GetCodeHex()
        {
            return "#" + Force.ToString("X2")
                   + Dexterite.ToString("X2")
                   + Endurance.ToString("X2");
        }

        // Voir: 

        /// <summary>
        /// Calcule le pourcentage de luminence dans la couleur.
        /// </summary>
        /// <seealso cref="http://www.niwa.nu/2013/05/math-behind-colorspace-conversions-rgb-hsl/"/>
        /// <returns>Le pourcentage de luminence.</returns>
        public int GetLuminence()
        {
            float[] stats = new float[] { Force, Dexterite, Endurance };
            float max = stats.Max(),
                  min = stats.Min();

            return Convert.ToInt32((((max + min)/2)/255)*100);
        }
        
        public static Decimal DivideByMaxValue(int val)
        {
            return Convert.ToDecimal((val/255).ToString("#.##"));
        }

        public int GetSaturation() => 0;

        /// <summary>
        /// Constructeur de base. Requis sinon ça plante. D'oh!
        /// </summary>
        private Couleur() {}

        /// <summary>
        /// Constructeur de couleur lors de la création d'une nouvelle couleur à partir des
        /// informations du formulaire.
        /// </summary>
        /// <param name="colorTemp">Les données du formulaire de création de couleur.</param>
        /// <param name="pUserId">L'Id de l'utilisateur connecté.</param>
        /// TODO trouver comment mettre le user.
        public Couleur(CreerCouleurViewModel colorTemp, string pUserId)
        {
            Force = colorTemp.Force;
            Dexterite = colorTemp.Dexterite;
            Endurance = colorTemp.Endurance;

            // Si on n'a pas dépassé la limite des points assignables sur une nouvelle couleur,
            if ( Force + Dexterite + Endurance != (StatMinValue * 3) + 50 )
            {
                throw new PointsRestantsException("Le nombre de points assignés n'est pas exact.");
            }

            Luminence = GetLuminence();

            Nom = colorTemp.Nom;
            //CodeHex = this.GetCodeHex();
            UserId = pUserId;
        }
        
        /// <summary>
        /// Valide une donnée décimale destinée à être convertie en hexadécimale à 2 digits.
        /// </summary>
        /// <param name="pAttribut"></param>
        /// <returns></returns>
        private static bool ValiderRgb(int pAttribut)
        {
            return (pAttribut >= StatMinValue && pAttribut <= StatMaxValue);
        }
    }

    /// <summary>
    /// La classe Squad.
    /// </summary>
    public class Squad
    {
        [Key]
        public int Id { get; private set; }

        [ScaffoldColumn(false)]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [MaxLength(5)]
        public virtual IList<Couleur> Couleurs { get; set; }

        private Squad() {}

        public Squad(CreerSquadViewModel squadTemp, string pUserId)
        {


            foreach (var creerCouleurViewModel in squadTemp.Couleurs)
            {
                Couleurs.Add(new Couleur(creerCouleurViewModel, pUserId));
            }

            UserId = pUserId;
        }
    }
}