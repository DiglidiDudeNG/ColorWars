using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Data.Entity.Metadata.Internal;

namespace ColorWars.Models
{
    //
    // Batailles
    public class Bataille
    {
        [Key]
        public int Id { get; set; }

        [Range(2, 2)]
        public IList<BattleSet> BattleSets { get; set; }

        public virtual IList<Tour> Tours { get; set; }

        public int EtatBataille { get; set; }

        public enum Etat
        {
            Proposé,
            EnCours,
            JoueurUnGagne,
            JoueurDeuxGagne
        }
    }

    //
    // PlayerSets
    public class BattleSet
    {
        // Première partie de la clé composite.
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        // Deuxième partie de la clé composite.
        public int BatailleId { get; set; }
        public virtual Bataille Bataille { get; set; }

        public int SquadId { get; set; }
        public virtual Squad Squad { get; set; }

        public bool EstGagnant { get; set; }
    }

    //
    // Tours
    public class Tour
    {
        [Key]
        public int Id { get; set; }

        public int Num { get; set; }

        public int BatailleId { get; set; }
        public virtual Bataille Bataille { get; set; }

        public LogLine[] LogLines { get; set; }

        public int EtatTour { get; set; }

        public enum Etat
        {
            Débuté,
            GaucheDoitJouer,
            DroiteDoitJouer,
            Complété,
            Archivé
        }

        public Tour()
        {
            
        }
    }

    [NotMapped]
    public class LogLine
    {
        
    }
}