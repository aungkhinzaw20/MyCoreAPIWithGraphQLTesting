using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCoreAPIWithGraphQL.Model
{
    public class ProductInformationModel
    {
        public string id { get; set; }
        public string timestamp { get; set; }
        public List<ProductInfo> PInfo { get; set; }
    }
    public class ProductInformationModelTemp
    {
        public string id { get; set; }
        public string timestamp { get; set; }
    }
    public class ProductInfo
    {
        public Int64 _id { get; set; }
        public string name { get; set; }
        public int quantity { get; set; }
        public double Sale_Amount { get; set; }
    }
}
