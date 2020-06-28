using FreshFishMobile.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FreshFishMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    internal class Rate
    {
        [JsonProperty("ccy")]
        public string Ccy { get; set; }

        [JsonProperty("base_ccy")]
        public string BaseCcy { get; set; }

        [JsonProperty("buy")]
        public double Buy { get; set; }

        [JsonProperty("sale")]
        public string Sale { get; set; }
    }
    public partial class IncomePage : ContentPage
    {
        

        ProductsHelper helper = new ProductsHelper();
        List<Rate> rates;
        double calculatedIncome;
        public IncomePage()
        {
            InitializeComponent();
            GetIncomeAsync();
            GetCurrencyRates();
        }

        async void GetIncomeAsync()
        {
            calculatedIncome = await helper.GetPricesSumAsync();
            IncomeValue.Text = calculatedIncome.ToString();
            rates = GetCurrencyRates();
            try
            {
                dollarCurrencyValue.Text += (calculatedIncome / rates[0].Buy).ToString("F2");
                euroCurrencyValue.Text += (calculatedIncome / rates[1].Buy).ToString("F2");
                rubleCurrencyValue.Text += (calculatedIncome / rates[2].Buy).ToString("F2");
                btcCurrencyValue.Text += ((decimal)(calculatedIncome / rates[0].Buy / rates[3].Buy)).ToString("F2");
            }
            catch
            {
                dollarCurrencyValue.Text += "0";
                euroCurrencyValue.Text += "0";
                rubleCurrencyValue.Text += "0";
                btcCurrencyValue.Text += "0";
            }
 }

        List<Rate> GetCurrencyRates()
        {
            string json;
            using (WebClient wc = new WebClient())
                {
                    json = wc.DownloadString("https://api.privatbank.ua/p24api/pubinfo?json&exchange&coursid=5");
                } 

            return JsonConvert.DeserializeObject<List<Rate>>(json);
        }
    }
}