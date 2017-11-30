using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Models
{
    /// <summary>
    /// 采购计划
    /// </summary>
    public class Purchase : EntityBase
    {
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public decimal Price { get; set; }
        /// <summary>
        /// 计划订单数量 单位吨
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 实际卸油数量 单位升
        /// </summary>
        public decimal OilCount { get; set; } = 0;
        public decimal DiffLitre { get; set; }
        public decimal DiffTon { get; set; }
        /// <summary>
        /// 始发地
        /// </summary>
        public string Origin { get; set; }
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 预计到达时间
        /// </summary>
        public DateTime? ArrivalTime { get; set; }
        public string CarNo { get; set; }
        /// <summary>
        /// 挂车号
        /// </summary>
        public string TrailerNo { get; set; }
        public string Driver1 { get; set; }
        public string IdCard1 { get; set; }
        public string Phone1 { get; set; }
        public string Driver2 { get; set; }
        public string IdCard2 { get; set; }
        public string Phone2 { get; set; }
        /// <summary>
        /// 卸油对应的仓库Id，多个用','分隔
        /// </summary>
        public string ToStoreIds { get; set; }
        /// <summary>
        /// 卸油对应的仓库名称，多个用','分隔
        /// </summary>
        public string ToStoreNames { get; set; }
        /// <summary>
        /// 卸油对应的仓库的数量，多个用','分隔
        /// </summary>
        public string ToStoreCounts { get; set; }
        /// <summary>
        /// 卸油对应的仓库的表前数，多个用','分隔
        /// </summary>
        public string ToStoreInstruBf { get; set; }
        /// <summary>
        /// 卸油对应的仓库的表后数，多个用','分隔
        /// </summary>
        public string ToStoreInstruAf { get; set; }

        public List<ToStoreModel> ToStoresList { get; set; }

        ///陆上卸油用到的字段
        ///
        /// <summary>
        /// 表1
        /// </summary>
        public decimal Instrument1 { get; set; }
        /// <summary>
        /// 表2
        /// </summary>
        public decimal Instrument2 { get; set; }
        /// <summary>
        /// 表3
        /// </summary>
        public decimal Instrument3 { get; set; }
        /// <summary>
        /// 卸车时需要测量的密度
        /// </summary>
        public double Density { get; set; } = 0;
        /// <summary>
        /// 油车磅秤数
        /// </summary>
        public decimal ScaleWithCar { get; set; } = 0;

        /// <summary>
        /// 油车磅秤图片地址
        /// </summary>
        public string ScaleWithCarPic { get; set; }
        /// <summary>
        /// 施工人员
        /// </summary>

        public string Worker { get; set; }

        /// <summary>
        /// 空车磅秤数
        /// </summary>
        public decimal Scale { get; set; } = 0;

        /// <summary>
        /// 油车磅秤图片地址
        /// </summary>
        public string ScalePic { get; set; }

        /// <summary>
        /// 油重 陆上(净重)
        /// </summary>
        public decimal DiffWeight
        {
            get
            {
                return ScaleWithCar - Scale;
            }
        }

        /// <summary>
        /// 化验单
        /// </summary>
        public int? AssayId { get; set; }
        public virtual Assay Assay { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public UnloadState State { get; set; } = UnloadState.已开单;

        public enum UnloadState
        {
            已开单,
            已到达,
            选择油仓,
            油车过磅,
            化验,
            卸油中,
            空车过磅,
            完工
        }

        public decimal TotalMoney
        {
            get
            {
                return Price * Count;
            }
        }
    }
    /// <summary>
    /// 选择多个卸油仓卸油用到的Model
    /// </summary>
    public class ToStoreModel
    {
        public int Id { get; set; }
        public decimal Count { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 卸油前表数
        /// </summary>
        public decimal InstrumentBf { get; set; }
        /// <summary>
        /// 卸油后表数
        /// </summary>
        public decimal InstrumentAf { get; set; }
    }
}
