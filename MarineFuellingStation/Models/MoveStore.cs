using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Models
{
    /// <summary>
    /// 转仓单
    /// </summary>
    public class MoveStore : EntityBase
    {
        /// <summary>
        /// 生产员
        /// </summary>
        public string Manufacturer { get; set; }
        #region 转出

        public int OutStoreTypeId { get; set; }

        public int OutStoreId { get; set; }
        /// <summary>
        /// 转出仓名称
        /// </summary>
        public string OutStoreName { get; set; }
        /// <summary>
        /// 密度
        /// </summary>
        public decimal OutDensity { get; set; }
        /// <summary>
        /// 油温
        /// </summary>
        public decimal OutTemperature { get; set; }
        /// <summary>
        /// 计划转出
        /// </summary>
        public decimal OutPlan { get; set; }
        /// <summary>
        /// 实际转出
        /// </summary>
        public decimal OutFact { get; set; }
        #endregion
        #region 转入
        public int InStoreTypeId { get; set; }
        public int InStoreId { get; set; }
        /// <summary>
        /// 转入仓名称
        /// </summary>
        public string InStoreName { get; set; }
        /// <summary>
        /// 密度
        /// </summary>
        public decimal InDensity { get; set; }
        /// <summary>
        /// 油温
        /// </summary>
        public decimal InTemperature { get; set; }
        /// <summary>
        /// 实际转入
        /// </summary>
        public decimal InFact { get; set; }
        /// <summary>
        /// 安排转入
        /// </summary>
        public decimal InPlan { get; set; }
        #endregion
        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 耗时（分钟）
        /// </summary>
        public int Elapsed { get; set; }
        /// <summary>
        /// 生产单状态
        /// </summary>
        public MoveStoreState State { get; set; } = MoveStoreState.已开单;
        /// <summary>
        /// 误差
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal Deviation
        {
            get
            {
                return OutPlan - InFact;
            }
        }
    }
    public enum MoveStoreState
    {
        已开单,
        施工中,
        已完成
    }
}
