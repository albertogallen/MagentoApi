using System;

using MagentoApi.MagentoApi;
using Microsoft.Extensions.Configuration;

namespace MagentoApi.ClientConsole
{
    class Program
    {
        private static IConfiguration _configuration;
        static private string Token;// = ConfigurationManager.AppSettings["Token"];
        static private string ip;// = ConfigurationManager.AppSettings["IP"];
        static private string baseUrl = "";
        static void Main(string[] args)
        {

            _configuration = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json", false, true)
                                .Build();
            
            
            Token = _configuration.GetSection("AppSettings")["Token"];
            ip = _configuration.GetSection("AppSettings")["ip"];
            string user = _configuration.GetSection("AppSettings")["userName"];
            string passWord = _configuration.GetSection("AppSettings")["passWord"];
                        

            baseUrl = string.Format("http://{0}", ip);

            Token = GetToken(user, passWord);
            System.Console.WriteLine (string.Format("Access Token : [ {0} ]",Token));
            
             GetSku("PR0001", Token);
            //var response =  CreateCategory("Dominos");
            //System.Console.WriteLine (string.Format("Response : [ {0} ]",Token));

            
            // UpdateProductQty();
        }

        static string GetToken(string userName, string passWord)
        {
            var m2 = new Magento(baseUrl);
            string token = m2.GetAdminToken(userName, passWord);
            return token;

        }

        static void GetSku(string skuName, string token)
        {
            var magento = new Magento(baseUrl, token);
            magento.GetSku(token, skuName);
        }

        static string CreateCategory(string name)
        {
            var magento = new Magento(baseUrl, Token);
           return magento.CreateCategory(name);
        }

        static void UpdateProductQty()
        {
            var magento = new Magento(baseUrl, Token);

            var update_this_sku = new SkuUpdate();

            var sku = new StockItem();
            sku.Qty = 6396;
            sku.ItemId = 1;
            sku.IsInStock = true;

            update_this_sku.StockItem = sku;

            var result = magento.UpdateProductQty(update_this_sku, "ProductOne");

        }
    }
    public static class ConfigExtenstions
    {
        public static T GetValue<T>(this IConfiguration configuration, string configSection, string keyName)
        {
            return (T)Convert.ChangeType(configuration[$"{configSection}:{keyName}"], typeof(T));
        }
        private static T GetValue<T>(this IConfiguration configuration, string configSection, string configSubSection, string keyName)
        {
            return (T)Convert.ChangeType(configuration[$"{configSection}:{configSubSection}:{keyName}"], typeof(T));
        }
    }
}
