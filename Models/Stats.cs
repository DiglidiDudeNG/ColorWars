using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Threading.Tasks;
using dia2;
using Newtonsoft.Json;
using Remotion.Linq.Clauses.Expressions;

namespace ColorWars.Models
{
    /// <summary>
    /// Collection de stats
    /// </summary>
    [NotMapped]
    public class StatCollection
    {
        public Rouge Rouge { get; private set; }
        public Vert Vert { get; private set; }
        public Bleu Bleu { get; private set; }

        private StatCollection() {}

        public StatCollection(Rouge pRouge, Vert pVert, Bleu pBleu)
        {
            Rouge = pRouge;
            Vert = pVert;
            Bleu = pBleu;
        }

        public StatCollection(int bRouge, int bVert, int bBleu)
        {

        }

        public StatCollection(string json)
        {
            JsonReader reader = new JsonTextReader(new StringReader(json));
            // TODO: Créer les stats par rapport au JSON fourni.
        }
    }

    /// <summary>
    /// Stat Générique
    /// </summary>
    [NotMapped]
    public abstract class GenericStat
    {
        /// La valeur de base assignée lors de la création du personnage.
        public int Base { get; protected set; }
        public int BonusLevelUp { get; protected set; }
        public int BonusCombat { get; protected set; }

        /// Total calculé du stat.
        public virtual int Total => Base + BonusLevelUp + BonusCombat;

        /// <summary>
        /// Constructeur de base.
        /// </summary>
        protected GenericStat() { }

        /// <summary>
        /// Constructeur qui assigne les propriétés non-virtuelles du Stat.
        /// </summary>
        /// <param name="_base">La valeur de base assignée lors de la création du personnage.</param>
        /// <param name="bLevelUp"></param>
        /// <param name="bCombat"></param>
        protected GenericStat(int _base, int bLevelUp = 0, int bCombat = 0)
        {
            Base = _base;
            BonusLevelUp = bLevelUp;
            BonusCombat = bCombat;
        }

        /// Ajoute des points pour le level up.
        /// TODO: Un check si la valeur à ajouter est négative.
        public void AddLevelUpBonus(int add) => BonusLevelUp += add;

        /// Modifie les points reçus par rapport à la bataille.
        public void AddCombatBonus(int add) => BonusCombat += add;

        /// Remet les points de combat à zéro.
        public virtual void ResetAllCombatBonuses() => BonusCombat = 0;
    }

    /// <summary>
    /// Main Stat
    /// </summary>
    [NotMapped]
    public abstract class MainStat : GenericStat
    {
        public const int CREATION_BASE = 25;

        /*public new int Base
        {
            get { return base.Base; }
            protected set
            {
                base.Base = value;
            }
        }*/

        /// Bonus pour la classe choisie.
        public int BonusClasse { get; private set; }

        /// Total calculé du stat. 
        public new int Total => base.Total + BonusClasse;

        /// Sous-stat de catégorie Attaque.
        protected SubStat AtkSubStat { get; set; }

        /// Sous-stat de catégorie Dextérité.
        protected SubStat DexSubStat { get; set; }

        /// Sous-stat de catégorie Défense.
        protected SubStat DefSubStat { get; set; }

        protected MainStat() : base() {}

        /// <summary>
        /// Constructeur du MainStat.
        /// </summary>
        /// <param name="_base">La valeur de base assignée lors de la création du personnage.</param>
        /// <param name="atk"></param>
        /// <param name="dex"></param>
        /// <param name="def"></param>
        /// <param name="bClasse"></param>
        /// <param name="bLevelUp"></param>
        /// <param name="bCombat"></param>
        /// <exception cref="TotalPourcentageInvalideException">Si le total </exception>
        protected MainStat(int _base, SubStat atk, SubStat dex, SubStat def, int bClasse = 0, int bLevelUp = 0, int bCombat = 0) 
            : this(_base, new List<SubStat>(3) { atk, dex, def }, bLevelUp, bCombat)
        {
            // TODO: Vérifier si c'est ainsi qu'il faut valider si ça fait un total de 100%.
            if (ValiderSubStatPourcentageTotal(atk.Percent, dex.Percent, def.Percent))
            {
                atk.UpdateBonusMainStatPercent(Base);
                dex.UpdateBonusMainStatPercent(Base);
                def.UpdateBonusMainStatPercent(Base);
                AtkSubStat = atk;
                DexSubStat = dex;
                DefSubStat = def;
            }
            else
            {
                throw new TotalPourcentageInvalideException();
            }

            BonusClasse = bClasse;
        }

        protected MainStat(int _base, IList<SubStat> SubStats, int bLevelUp, int bCombat) 
            : base (_base, bLevelUp, bCombat)
        {
            if (ValiderSubStatPourcentageTotal(SubStats))
            {
                foreach (SubStat sub in SubStats)
                {
                    
                }
            }
        }

        // Si c'est un total d'environ 100 ou que c'est 
        public static bool ValiderSubStatPourcentageTotal(double atk, double dex, double def)
        {
            bool retour = Math.Round(atk + dex + def) == 100;

            return retour;
        }

        public static bool ValiderSubStatPourcentageTotal(IList<SubStat> SubStats)
        {
            bool retour = Math.Round(SubStats.Sum(stat => stat.Percent)) == 100;

            return retour;
        }

        /// <summary>
        /// Réinitialise les bonus de combat pour autant lui-même que pour les sous-stats.
        /// </summary>
        public override void ResetAllCombatBonuses()
        {
            base.ResetAllCombatBonuses();
            AtkSubStat.ResetAllCombatBonuses();
            DexSubStat.ResetAllCombatBonuses();
            DefSubStat.ResetAllCombatBonuses();
        }

        /// <summary>
        /// Retourne le sous-stat au nom demandé
        /// </summary>
        /// <param name="nomSubStat">Le nom du SubStat.</param>
        /// <returns>SubStat: L'instance du SubStat demandé.</returns>
        /// <exception cref="InstanceNotFoundException">Si le SubStat au nom précisé n'existe pas.</exception>
        public SubStat GetSubStat(string nomSubStat)
        {
            var subStat = GetType().GetField(nomSubStat);

            if (subStat != null) {
                return (SubStat) subStat.GetValue(this);
            }

            throw new InstanceNotFoundException();
        }
    }

    /// <summary>
    /// Sous-Stat
    /// </summary>
    [NotMapped]
    public class SubStat : GenericStat
    {
        public const int CREATION_BASE = 5;
        public const int PERCENT_MULTIPLE = 5;

        /// Le pourcentage de MainStat ajouté sur la valeur totale du SubStat.
        public double Percent { get; protected set; }

        /// Le bonus de poucentage du MainStat attribué durant un combat.
        public double BonusPercent { get; protected set; }

        /// Le bonus calculé par rapport au pourcentage fois la valeur totale du main stat.
        public int BonusCalc { get; protected set; }

        /// Total calculé du stat.
        public new int Total => base.Total + BonusCalc;

        /// Constructeur privé pour EF7.
        private SubStat() : base() {}

        /// <summary>
        /// Crée un Sous-Stat à l'aide des 
        /// </summary>
        /// <param name="_base">La valeur de base assignée lors de la création du personnage.</param>
        /// <param name="bLevelup"></param>
        /// <param name="bCombat"></param>
        /// <param name="percent">Le pourcentage de MainStat ajouté sur la valeur totale du SubStat.</param>
        /// <param name="bPercent"> Le bonus de poucentage du MainStat attribué durant un combat.</param>
        public SubStat(int _base, double percent = 0, double bPercent = 0, int bLevelup = 0, int bCombat = 0) 
            : base(_base, bLevelup, bCombat)
        {
            Percent = percent;
            BonusPercent = bPercent;
        }

        public SubStat(SubStat subStat, int mainStatTotal) 
            : this(subStat.Base, subStat.Percent, subStat.BonusPercent, subStat.BonusLevelUp, subStat.BonusCombat)
        {
            UpdateBonusMainStatPercent(mainStatTotal);
        }

        public void UpdateBonusMainStatPercent(int mainStatTotal)
        {
            BonusCalc = (int) Math.Round((mainStatTotal * (Percent + BonusPercent)) / 100);
        }

        public override void ResetAllCombatBonuses()
        {
            base.ResetAllCombatBonuses();
            BonusPercent = 0;
        }
    }

    #region Définitions des MainStats (Rouge, Bleu, Vert)

    /// <summary>
    /// Rouge
    /// </summary>
    [NotMapped]
    public class Rouge : MainStat
    {
        public Rouge(int _base, SubStat atk, SubStat dex, SubStat def, int bLevelUp, int bCombat, int bClasse)
                : base(_base, atk, dex, def, bLevelUp, bCombat, bClasse) { }

        /// <summary><list type="bullet">
        /// <listheader>Affecte :</listheader>
        /// <item>Puissance des actions physiques.</item>
        /// </list></summary>
        public SubStat Force
        {
            get { return AtkSubStat; }
            set { AtkSubStat = value; }
        }

        /// <summary><list type="bullet">
        /// <listheader>Affecte :</listheader>
        /// <item>Points de moral.</item>
        /// <item>Résistance à la saturation.</item>
        /// </list></summary>
        public SubStat Volonté
        {
            get { return DexSubStat; }
            set { DexSubStat = value; }
        }

        /// <summary><list type="bullet">
        /// <listheader>Affecte :</listheader>
        /// <item>Nb de points de vie.</item>
        /// <item>Résistance aux éléments.</item>
        /// </list></summary>
        public SubStat Constitution
        {
            get { return DefSubStat; }
            set { DefSubStat = value; }
        }
    }

    /// <summary>
    /// Vert
    /// </summary>
    [NotMapped]
    public class Vert : MainStat
    {
        public Vert(int _base, SubStat atk, SubStat dex, SubStat def, int bLevelUp, int bCombat, int bClasse)
                : base(_base, atk, dex, def, bLevelUp, bCombat, bClasse) {}

        /// <summary><list type="bullet">
        /// <listheader>Affecte :</listheader>
        /// <item>Ordre pour faire une action.</item>
        /// <item>Le nb de fois qu'une action sera fait durant un tour, pour les habiletés appliquables.</item>
        /// </list></summary>
        public SubStat Vitesse
        {
            get { return AtkSubStat; }
            set { AtkSubStat = value; }
        }

        /// <summary><list type="bullet">
        /// <listheader>Affecte :</listheader>
        /// <item>Chance à toucher l'ennemi avec toute action.</item>
        /// <item>Chance d'un coup critique.</item>
        /// </list></summary>
        public SubStat Précision
        {
            get { return DexSubStat; }
            set { DexSubStat = value; }
        }

        /// <summary><list type="bullet">
        /// <listheader>Affecte :</listheader>
        /// <item>Taux de chance d'éviter une action.</item>
        /// <item>Résistance aux pièges.</item>
        /// </list></summary>
        public SubStat Évasion
        {
            get { return DefSubStat; }
            set { DefSubStat = value; }
        }
    }

    /// <summary>
    /// Bleu
    /// </summary>
    [NotMapped]
    public class Bleu : MainStat
    {
        public Bleu(int _base, SubStat atk, SubStat dex, SubStat def, int bLevelUp, int bCombat, int bClasse) 
            : base(_base, atk, dex, def, bLevelUp, bCombat, bClasse) {}

        /// <summary><list type="bullet">
        /// <listheader>Affecte :</listheader>
        /// <item>Puissance de beaucoup d'actions magiques appliquables.</item>
        /// </list></summary>
        public SubStat Intelligence
        {
            get { return AtkSubStat; }
            set { AtkSubStat = value; }
        }

        /// <summary><list type="bullet">
        /// <listheader>Affecte :</listheader>
        /// <item>Taux de chance d'effectuer les effets secondaires pour la plupart des actions</item>
        /// </list></summary>
        public SubStat Sagesse
        {
            get { return DexSubStat; }
            set { DexSubStat = value; }
        }

        /// <summary><list type="bullet">
        /// <listheader>Affecte :</listheader>
        /// <item>Taux de chance d'éviter les changements de status ainsi que les effets secondaires.</item>
        /// <item>Résistance à certains effets.</item>
        /// </list></summary>
        public SubStat Lucidité
        {
            get { return DefSubStat; }
            set { DefSubStat = value; }
        }
    }

    #endregion
}
