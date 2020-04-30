using Firebase.Database;
using Firebase.Database.Query;
using FreshFishMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshFishMobile.Helpers
{
    public class ProductsHelper
    {
        double sum;
        FirebaseClient client = new FirebaseClient("https://freshfish-bf927.firebaseio.com");

        public async Task<List<Products>> GetAllProductsAsync()
        {
            return (await client
                .Child("freshfish")
                .OnceAsync<Products>()).Select(item => new Products
                {
                    id = item.Object.id,
                    productname = item.Object.productname,
                    price = item.Object.price,
                    date = item.Object.date,
                    status = item.Object.status

                }).ToList();

        }

        public async Task<double> GetPricesSumAsync()
        {
            var productsList = await GetAllProductsAsync();

            var prices = from p in productsList
                         where p.status == "Yes"
                         select p.price;

            sum = prices.Sum(v => Convert.ToDouble(v));

            return sum;
        }

        public async Task AddProduct(string productName, string Price, string Date, string Status)
        {
            //string randomID = GetRandomId();
            await client
                .Child("freshfish/")
                .PostAsync(new Products()
                {
                    id = GetRandomId(),
                    productname = productName,
                    price = Price,
                    date = Date,
                    status = Status
                });
        }

        public async Task UpdateProduct(string ID,
            string productName,
            string Price,
            string Date,
            string Status)
        {
            var toUpdateProduct = (await client
                .Child("freshfish")
                .OnceAsync<Products>()).Where(a => a.Object.id == ID).FirstOrDefault();

            await client
                .Child("freshfish")
                .Child(toUpdateProduct.Key)
                .PutAsync(new Products { id = ID, productname = productName, price = Price, date = Date, status = Status });
        }

        public async Task DeleteProduct(string ID)
        {
            var toDeleteProduct = (await client
                .Child("freshfish")
                .OnceAsync<Products>()).Where(a => a.Object.id == ID).FirstOrDefault();
            await client.Child("freshfish").Child(toDeleteProduct.Key).DeleteAsync();
        }

        string GetRandomId()
        {
            Random rnd = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string x = new string(Enumerable.Repeat(chars, 4)
                .Select(s => s[rnd.Next(s.Length)]).ToArray());
            const string nums = "123456789";
            string y = new string(Enumerable.Repeat(nums, 4)
                .Select(s => s[rnd.Next(s.Length)]).ToArray());

            string sDate = DateTime.Now.ToString();
            DateTime value = (Convert.ToDateTime(sDate.ToString()));

            return x + y +
                value.Day.ToString() +
                value.Month.ToString() +
                value.Year.ToString() +
                value.Minute.ToString() +
                value.Hour.ToString() +
                value.Second.ToString();

        }


    }
}
