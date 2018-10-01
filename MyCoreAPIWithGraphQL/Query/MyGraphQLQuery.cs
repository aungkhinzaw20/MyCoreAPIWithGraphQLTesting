using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;
using MyCoreAPIWithGraphQL.Model;
using System.Runtime.InteropServices;

namespace MyCoreAPIWithGraphQL.Query
{
    public class MyGraphQLQuery : ObjectGraphType
    {
        public MyGraphQLQuery([Optional] ProductInformationModelTemp PMod)
        {
            Field<ProductInformationModelType>(
                "prdinfo",
                resolve: context => PMod
                );
        }
    }
}
