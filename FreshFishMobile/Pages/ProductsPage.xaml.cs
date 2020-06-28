using Firebase.Database;
using FreshFishMobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace FreshFishMobile.Pages
{
    public partial class ProductsPage : ContentPage
    {
        static bool executed = true;
        static ObservableCollection<Products> ProductsCollection = new ObservableCollection<Products>();
        static FirebaseClient client = new FirebaseClient("https://freshfish-bf927.firebaseio.com");
        public ProductsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if(executed == true)
            {
                GetProducts();
                executed = false;
            }
            productsListView.ItemsSource = ProductsCollection;
            
        }

        void GetProducts()
        {
            ProductsCollection = client
                .Child("freshfish")
                .AsObservable<Products>().AsObservableCollection();
        }
        private async void productsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Products selectedProduct = e.SelectedItem as Products;
            if(e.SelectedItem != null)
            {
                productsListView.SelectedItem = null;
                await Navigation.PushAsync(new SpecificProductPage(selectedProduct));
            }
            
            
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SpecificProductPage(null));
        }

        private void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {
            if(productsSearchBar.IsVisible == false)
            productsSearchBar.IsVisible = true;
            else
            {
                productsSearchBar.IsVisible = false;
            }
        }

        private void productsSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (productsSearchBar.Text != null)
            {
                var searchResult = (from fish in ProductsCollection
                                    where fish.productname.ToLower().Contains(productsSearchBar.Text.ToLower()) ||
                                    fish.price.ToLower().Contains(productsSearchBar.Text.ToLower()) ||
                                    fish.date.ToLower().Contains(productsSearchBar.Text.ToLower()) ||
                                    fish.status.ToLower().Contains(productsSearchBar.Text.ToLower())
                                    select fish).ToList();
                productsListView.ItemsSource = searchResult;
            } 
        }
    }
}
