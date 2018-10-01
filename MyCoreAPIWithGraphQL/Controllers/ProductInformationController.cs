using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using MyCoreAPIWithGraphQL.Model;
using MyCoreAPIWithGraphQL.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using GraphQL.Types;
using GraphQL;


namespace MyCoreAPIWithGraphQL.Controllers
{
    [Route("productinfo")]
    public class ProductInformationController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public ProductInformationController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpPost]
        public bool PutProductInformation([FromBody] ProductInformationModel PInfo)
        {
            try
            {                

                XDocument doc = XDocument.Load(_hostingEnvironment.ContentRootPath + "\\Reference\\ProductInfo.Xml");

                var PID = doc.Root.Elements("ProductsInfo")
                    .Where(a => (string)a.Element("ID").Value == PInfo.id)
                    .Select(a => (string)a.Element("ID").Value).FirstOrDefault();

                if (!string.IsNullOrEmpty(PID))
                    throw new Exception("Duplicate Product ID");

                doc.Element("DocumentElement").Add(
                        new XElement("ProductsInfo",
                        new XElement("ID", PInfo.id),
                        new XElement("TimeStamp", PInfo.timestamp),
                        from PdInfo in PInfo.PInfo
                        select
                            new XElement("Product",
                            new XElement("Pid", PdInfo._id),
                            new XElement("PName", PdInfo.name),
                            new XElement("quantity", PdInfo.quantity),
                            new XElement("sale_amount", PdInfo.Sale_Amount))
                        ));

                doc.Save(_hostingEnvironment.ContentRootPath + "\\Reference\\ProductInfo.Xml");
                return true;
            }
            catch
            {
                return false;
            }
        }
        [HttpGet]
        public ProductInformationModel GetProducts(string ProductID)
        {
            ProductInformationModel _Product = new ProductInformationModel();
            ProductInfo _PInfo = new ProductInfo();
            try
            {
                XDocument doc = XDocument.Load(_hostingEnvironment.ContentRootPath + "\\Reference\\ProductInfo.Xml");

                IEnumerable<XElement> pdocut = (
                    from result in doc.Root.Elements("ProductsInfo")
                    where result.Element("ID").Value == ProductID.ToString()
                    select result);

                _Product.id = pdocut.ElementAt(0).Element("ID").Value;
                _Product.timestamp = pdocut.ElementAt(0).Element("TimeStamp").Value;
                _Product.PInfo = new List<ProductInfo>();
                foreach (var item in pdocut)
                {
                    _PInfo._id = Convert.ToInt64(item.Element("Product").Element("Pid").Value);
                    _PInfo.name = item.Element("Product").Element("PName").Value;
                    _PInfo.quantity = Convert.ToInt16(item.Element("Product").Element("quantity").Value);
                    _PInfo.Sale_Amount = Convert.ToDouble(item.Element("Product").Element("sale_amount").Value);
                    _Product.PInfo.Add(_PInfo);
                }
            }
            catch (Exception ex)
            {

            }
            return _Product;
        }

        [Route("gql")]
        [HttpPost]
        public async Task<IActionResult> GetProductsWithGQL(string ProductID, [FromBody] GraphQLQuery query)
        {
            ProductInformationModelTemp _Product = new ProductInformationModelTemp();
            ProductInfo _PInfo = new ProductInfo();
            try
            {
                XDocument doc = XDocument.Load(_hostingEnvironment.ContentRootPath + "\\Reference\\ProductInfo.Xml");

                IEnumerable<XElement> pdocut = (
                    from result in doc.Root.Elements("ProductsInfo")
                    where result.Element("ID").Value == ProductID.ToString()
                    select result);

                _Product.id = pdocut.ElementAt(0).Element("ID").Value;
                _Product.timestamp = pdocut.ElementAt(0).Element("TimeStamp").Value;
            }
            catch (Exception ex)
            {

            }

            var schema = new Schema { Query = new MyGraphQLQuery(_Product) };

            var _result = await new DocumentExecuter().ExecuteAsync(_ =>
            {
                _.Schema = schema;
                _.Query = query.Query;

            }).ConfigureAwait(false);

            if (_result.Errors?.Count > 0)
            {
                return BadRequest();
            }

            return Ok(_result);

        }
    }
}
