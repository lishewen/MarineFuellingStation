using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Helper
{
    /// <summary>
    /// 单位转换
    /// </summary>
    public static class UnitExchange
    {
        /// <summary>
        /// 升转吨
        /// </summary>
        /// <param name="litre"></param>
        /// <param name="density"></param>
        /// <returns></returns>
        public static decimal ToTon(decimal litre, double density)
        {
            return litre * (decimal)Math.Round(density, 3) / 1000;
        }
        /// <summary>
        /// 吨转升
        /// </summary>
        /// <param name="ton"></param>
        /// <param name="density"></param>
        /// <returns></returns>
        public static decimal ToLitre(decimal ton, double density)
        {
            return ton / (decimal)Math.Round(density, 3) * 1000;
        }
    }
}
