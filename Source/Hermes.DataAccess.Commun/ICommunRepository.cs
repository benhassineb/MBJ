using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermes.DataAccess.Commun
{
    /// <summary>
    /// Définit les méthodes d'accès aux données "Communes"
    /// </summary>
    public interface ICommunRepository
    {
        /// <summary>
        /// Retourne l'heure actuelle UTC définie au niveau du serveur de données.
        /// </summary>
        /// <returns></returns>
        DateTime GetUtcNow();

    }
}
