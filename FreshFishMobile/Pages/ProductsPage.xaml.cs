using Firebase.Database;
using FreshFishMobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                ProductsCollection = client
                 .Child("freshfish")
                 .AsObservable<Products>().AsObservableCollection();
                executed = false;
            }
            productsListView.ItemsSource = ProductsCollection;
        }
        private void productsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
    }
}
