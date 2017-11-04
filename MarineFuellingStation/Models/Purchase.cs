using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MFS.Models
{
    /// <summary>
    /// 进油计划
    /// </summary>
    public class Purchase : EntityBase
    {
        private string _toStoreIds;
        private string _toStoreNames;
        private string _toStoreCounts;

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
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
        /// <summary>
        /// 实际与订单相差 升数
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal DiffLitre
        {
            get
            {
                if (Density > 0)
                    return Math.Round(OilCount - ((Count / Density) * 1000), 2);
                else
                    return 0;
            }
        }
        /// <summary>
        /// 实际与订单相差 吨数
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal DiffTon
        {
            get
            {
                if(Density > 0)
                    return Math.Round(OilCount * Density / 1000 - Count, 2);
                else
                    return 0;
            }
        }
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

        ///陆上卸油用到的字段
        #region 卸油到多油仓用到的字段
        [NotMapped]
        public List<ToStoreModel> ToStoresList { get; set; }
        /// <summary>
        /// 卸油对应的仓库Id，多个用','分隔
        /// </summary>
        public string ToStoreIds {
            get { return _toStoreIds; }
            set
            {
                if(ToStoresList != null)
                {
                    string ids = "";
                    foreach(var s in ToStoresList)
                    {
                        ids += s.Id + ",";
                    }
                    ids = ids.Substring(0, ids.Length - 1);
                    _toStoreIds = ids;
                }
            }
        }
        /// <summary>
        /// 卸油对应的仓库名称，多个用','分隔
        /// </summary>
        public string ToStoreNames {
            get { return _toStoreNames; }
            set
            {
                if (ToStoresList != null)
                {
                    string names = "";
                    foreach (var s in ToStoresList)
                    {
                        names += s.Name + ",";
                    }
                    names = names.Substring(0, names.Length - 1);
                    _toStoreNames = names;
                }
            }
        }
        /// <summary>
        /// 卸油对应的仓库的数量，多个用','分隔
        /// </summary>
        public string ToStoreCounts {
            get { return _toStoreCounts; }
            set
            {
                if (ToStoresList != null)
                {
                    string counts = "";
                    foreach (var s in ToStoresList)
                    {
                        counts += s.Count + ",";
                    }
                    counts = counts.Substring(0, counts.Length - 1);
                    _toStoreCounts = counts;
                }
            }
        }
        #endregion
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
        /// 审核人
        /// </summary>
        public string Auditor { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditTime { get; set; }
        /// <summary>
        /// 卸车时需要测量的密度
        /// </summary>
        public decimal Density { get; set; } = 0;
        /// <summary>
        /// 油车磅秤数
        /// </summary>
        public float ScaleWithCar { get; set; } = 0;

        /// <summary>
        /// 油车磅秤图片地址
        /// </summary>
        public string ScaleWithCarPic { get; set; }

        /// <summary>
        /// 空车磅秤数
        /// </summary>
        public float Scale { get; set; } = 0;

        /// <summary>
        /// 油车磅秤图片地址
        /// </summary>
        public string ScalePic { get; set; }
        /// <summary>
        /// 施工人员
        /// </summary>

        public string Worker { get; set; }

        /// <summary>
        /// 化验单
        /// </summary>
        public int? AssayId { get; set; }
        [JsonIgnore, ForeignKey("AssayId")]
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
            完工,
            已审核
        }


        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
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
    [NotMapped]
    public class ToStoreModel
    {
        public int Id { get; set; }
        public decimal Count { get; set; }
        public string Name { get; set; }
    }
}
