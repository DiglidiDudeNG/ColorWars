using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ColorWars.Models
{
    /// <summary>
    /// Exception lancée lorsqu'il manque des points répartis dans les stats lors de la
    /// création et lors du lvl up.
    /// </summary>
    public class PointsRestantsException : Exception
    {
        public PointsRestantsException() { }
        public PointsRestantsException(string message) : base(message) { }
        public PointsRestantsException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// Exception lancée lorsque l'un des stats principaux n'obtient pas un total de 100%
    /// dans les pourcentages de base assignés lors de la création d'une couleur dans une squad.
    /// </summary>
    public class TotalPourcentageInvalideException : Exception
    {
        public TotalPourcentageInvalideException() {}
        public TotalPourcentageInvalideException(string message) : base(message) {}
        public TotalPourcentageInvalideException(string message, Exception innerException) : base(message, innerException) {}
    }
}
