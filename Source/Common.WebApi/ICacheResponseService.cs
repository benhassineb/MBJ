using Marvin.Cache.Headers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.WebApi
{
    /// <summary>
    /// Décrit la capacité à purger le cache.
    /// </summary>
    public interface ICacheResponseService
    {
        /// <summary>
        /// Efface le cache.
        /// </summary>
        void Clear();
    }
}
