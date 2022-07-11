using System;
using System.Text.Json;

namespace Persistence
{  
    public class JsonUtil
    {
        public void ProductsSave(List<Product> products)
        {
            var options = new JsonSerializerOptions 
            { 
                WriteIndented = true 
            };
            string jsonString = JsonSerializer.Serialize(products, options);
            File.WriteAllText("Products.json", jsonString);
        }
        public List<Product>? ProductsLoad()
        {
            List<Product>? products;
            if (!File.Exists("Product.json"))
            {
                products = new List<Product>();
            }
            StreamReader reader = new StreamReader("Products.json");
            string jsonString = reader.ReadToEnd();
            reader.Close();
            products = JsonSerializer.Deserialize<List<Product>>(jsonString);
            return products;
        }       
    }
}