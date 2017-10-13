using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SportsStoreDomain.Entities
{
    public class ShippingDetail
    {
        [Required(ErrorMessage = "请输入姓名")]
        public string Name { get; set; }

        [Required(ErrorMessage = "请输入第一个地址")]
        [Display(Name = "联系方式1")]
        public string Line1 { get; set; }
        [Display(Name = "联系方式2")]
        public string Line2 { get; set; }
        [Display(Name = "联系方式3")]
        public string Line3 { get; set; }

        [Required(ErrorMessage = "请输入城市名称")]
        [Display(Name ="城市")]
        public string City { get; set; }

        [Required(ErrorMessage = "请输入省/州的地址")]
        [Display(Name ="省/州地址")]
        public string State { get; set; }

        [Display(Name = "省/州地址缩写")]
        public string Zip { get; set; }

        [Required(ErrorMessage = "请输入国家名称")]
        [Display(Name = "国家名称")]
        public string Country { get; set; }

        public bool GiftWrap { get; set; }
    }
}
