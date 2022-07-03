using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagentoApi.MagentoApi.Models
{
   

  

   
    public class SkuUpdate
    {
        [JsonProperty("stockItem")]
        public StockItem StockItem { get; set; }
    }
}
