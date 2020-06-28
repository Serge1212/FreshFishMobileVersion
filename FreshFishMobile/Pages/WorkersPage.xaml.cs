using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace FreshFishMobile.Pages
{
    public partial class WorkersPage : ContentPage
    {
        static bool executed = true;
        static ObservableCollection<Workers> WorkersCollection = new ObservableCollection<Workers>();
        static FirebaseClient client = new FirebaseClient("https://freshfish-bf927.firebaseio.com");
        public EmployeePage()
        {
            InitializeComponent();
            if (executed)
            {
                GetWorkers();
                executed = false;
            }
            workersListView.ItemsSource = WorkersCollection;
        }

        void GetWorkers()
        {
            WorkersCollection = client
                .Child("workers")
                .AsObservable<Workers>().AsObservableCollection();
        }

        void workersSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (workersSearchBar.Text != null)
            {
                var searchResult = (from worker in WorkersCollection
                                    where worker.name.ToLower().Contains(workersSearchBar.Text.ToLower()) ||
                                    worker.surname.ToLower().Contains(workersSearchBar.Text.ToLower()) ||
                                    worker.patronymic.ToLower().Contains(workersSearchBar.Text.ToLower()) ||
                                    worker.position.ToLower().Contains(workersSearchBar.Text.ToLower()) ||
                                    worker.salary.ToLower().Contains(workersSearchBar.Text.ToLower()) ||
                                    worker.phonenumber.ToLower().Contains(workersSearchBar.Text.ToLower()) ||
                                    worker.address.ToLower().Contains(workersSearchBar.Text.ToLower())
                                    select worker).ToList();
                workersListView.ItemsSource = searchResult;
            }
        }

        void workersListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
        }

        void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            if (workersSearchBar.IsVisible == false)
                workersSearchBar.IsVisible = true;
            else
            {
                workersSearchBar.IsVisible = false;
            }
        }

        async void AddWorkerClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SpecificWorkerPage(null));

        }
    }
}
