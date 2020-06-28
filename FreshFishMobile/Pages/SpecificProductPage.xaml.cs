using FreshFishMobile.Helpers;
using FreshFishMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FreshFishMobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SpecificProductPage : ContentPage
    {
        ProductsHelper helper = new ProductsHelper();
        bool edited = true;
        public Products Product { get; set; }
        public SpecificProductPage(Products product = null)
        {
            InitializeComponent();

            Product = product;
            if (product == null)
            {
                DeleteProductButton.IsVisible = false;
                Product = new Products();
                edited = false;
            }
            BindingContext = Product;
        }

        private async void SaveProduct(object sender, EventArgs e)
        {
            await Navigation.PopAsync();

            if (edited == false)
            {
                await helper.AddProduct(productNameEntry.Text,
                    priceEntry.Text,
                    productDateDatePicker.Date.ToString(),
                    statusPicker.SelectedItem.ToString());
            }
            if (edited == true)
            {
                await helper.UpdateProduct(Product.id,
                    productNameEntry.Text,
                    priceEntry.Text,
                    productDateDatePicker.Date.ToString(),
                    statusPicker.SelectedItem.ToString());

            }
        }

        async void DeleteProductButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
            await helper.DeleteProduct(Product.id);
        }
    }
}