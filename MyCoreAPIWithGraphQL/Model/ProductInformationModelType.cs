using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;

namespace MyCoreAPIWithGraphQL.Model
{
    public class ProductInformationModelType : ObjectGraphType<ProductInformationModelTemp>
    {
        public ProductInformationModelType()
        {
            Field(x => x.id).Description("This is production service id");
            Field(x => x.timestamp).Description("This is production service timestamp");
        }
    }
}
