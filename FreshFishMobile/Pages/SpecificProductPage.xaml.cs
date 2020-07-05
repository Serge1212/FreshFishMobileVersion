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
        bool edited = true;
        List<Workers> packers = new List<Workers>();
        List<Workers> drivers = new List<Workers>();
        string packerId;
        string driverId;
        ProductsHelper productsHelper = new ProductsHelper();
        WorkersHelper workersHelper = new WorkersHelper();
        public Products Product { get; set; }
        public SpecificProductPage(Products product)
        {
            InitializeComponent();
            UploadPackersAndDrivers();
            Product = product;
            if (product == null)
            {
                DeleteProductButton.IsVisible = false;
                Product = new Products();
                edited = false;
            }
            else
            {
                packerId = Product.packer;
                driverId = Product.driver;
            }
            BindingContext = Product;
            FillPackerAndDriverComboBoxes();
        }

        private async void SaveProduct(object sender, EventArgs e)
        {
            await Navigation.PopAsync();

            if (edited == false)
            {
                await productsHelper.AddProduct(productNameEntry.Text,
                    priceEntry.Text,
                    productDateDatePicker.Date.ToString("dd/MM/yyyy"),
                    statusPicker.SelectedItem.ToString(),
                    packerId,
                    driverId);
            }
            if (edited == true)
            {
                await productsHelper.UpdateProduct(Product.id,
                    productNameEntry.Text,
                    priceEntry.Text,
                    productDateDatePicker.Date.ToString(),
                    statusPicker.SelectedItem.ToString(),
                    packerId,
                    driverId);

            }
        }

        async void DeleteProductButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
            await productsHelper.DeleteProduct(Product.id);
        }

        async void UploadPackersAndDrivers()
        {
            var workers = await workersHelper.GetAllWorkersAsync();

            packers = (from p in workers
                       where p.position.ToLower() == "packer"
                       select p).ToList();
            PackerPicker.ItemsSource = packers;

            drivers = (from p in workers
                       where p.position.ToLower() == "driver"
                       select p).ToList();
            DriverPicker.ItemsSource = drivers;
        }

        async void FillPackerAndDriverComboBoxes()
        {
            if (edited == true)
            {
                Workers specificDriver = await workersHelper.GetWorker(Product.driver);
                Workers specificPacker = await workersHelper.GetWorker(Product.packer);
                if (specificPacker != null && specificDriver != null)
                {
                    string driverNameSurname = specificDriver.name + " " + specificDriver.surname;
                    string packerNameSurname = specificPacker.name + " " + specificPacker.surname;

                    DriverPicker.Title = driverNameSurname;
                    PackerPicker.Title = packerNameSurname;
                }
            }
        }

        private void PackerPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
                Workers selectedPacker = PackerPicker.SelectedItem as Workers;
                packerId = selectedPacker.w_id;  
        }

        private void DriverPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
                Workers selectedDriver = DriverPicker.SelectedItem as Workers;
                driverId = selectedDriver.w_id;
        }
    }
}