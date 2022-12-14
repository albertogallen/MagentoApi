using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;

namespace MagentoApi.MagentoApi
{
    public class Magento
    {
       private RestClient Client { get; set; }
        private string Token { get; set; }

        public Magento(string magentoUrl)
        {
            Client = new RestClient(magentoUrl);           
        }
        public Magento(string magentoUrl,string token)
        {
            Client = new RestClient(magentoUrl);
            Token = token;
        }

        public string GetAdminToken(string userName, string passWord)
        {
            ///V1/integration/admin/token
            //var request = CreateRequest("/V1/integration/admin/token", Method.Post);
            var request = CreateRequest("/rest/V1/integration/admin/token", Method.Post);
            var user = new Credentials();
            user.username = userName;
            user.password = passWord;

            string json = JsonConvert.SerializeObject(user, Formatting.Indented);

            request.AddParameter("application/json", json, ParameterType.RequestBody);

            var response = Client.Execute(request);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                var x = response.Content;
                
                return   x.Replace("\"", string.Empty).Trim();
				
				
            }else
            {
                return "";
            }
        }
        private RestRequest CreateRequest(string endPoint,Method method)
        {
            var request = new RestRequest(endPoint, method);
            request.RequestFormat = DataFormat.Json;
            return request;
        }

        public void GetSku(string token,string sku)
        {
            var request = CreateRequest("/rest/V1/products/" + sku, Method.Get, token);

            var response = Client.Execute(request);

            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                M2Product product = JsonConvert.DeserializeObject<M2Product>(response.Content);

            }

        }

        public string CreateCategory(string categoryName)
        {
            var request = CreateRequest("/rest/V1/categories", Method.Post, Token);
            var cat = new ProductCategory();
            var category = new Category();
            category.Name = categoryName;

            cat.Category = category;

            string json = JsonConvert.SerializeObject(cat, Formatting.Indented);

            request.AddParameter("application/json", json, ParameterType.RequestBody);

            var response = Client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return response.Content;
            }
            else
            {
                return "";
            }

        }

        public bool UpdateProductQty(SkuUpdate update,string skuName)
        {
            var request = CreateRequest("/rest/V1/products/" + skuName + "/stockItems/" + update.StockItem.ItemId, Method.Put, Token);
            string json = JsonConvert.SerializeObject(update, Formatting.Indented);

            request.AddParameter("application/json", json, ParameterType.RequestBody);

            var result = Client.Execute(request);
            if(result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }else
            {
                return false;
            }
        }
        private RestRequest CreateRequest(string endPoint, Method method,string token)
        {
            var request = new RestRequest(endPoint, method);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddHeader("Accept", "application/json");
            return request;
        }
    }
}
