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
    public partial class SpecificWorkerPage : ContentPage
    {
        WorkersHelper helper = new WorkersHelper();
        bool edited = true;
        public Workers Worker { get; set; }

        public SpecificWorkerPage(Workers worker)
        {
            InitializeComponent();
            Worker = worker;
            if (worker == null)
            {
                deleteWorkerButton.IsVisible = false;
                Worker = new Workers();
                edited = false;
            }
            BindingContext = Worker;
        }

        async void AddWorkerClick(object sender, EventArgs e)
        {
            await Navigation.PopAsync();

            if (edited == false)
            {
                await helper.AddWorker(
                    workerNameEntry.Text,
                    workerSurnameEntry.Text,
                    workerPatronymicEntry.Text,
                    workerPositionEntry.Text,
                    workerSalaryEntry.Text,
                    workerPhoneNumberEntry.Text,
                    workerAddressEntry.Text,
                    workerAdditionalInfoEntry.Text);
            }
            if (edited == true)
            {
                await helper.UpdateWorker(
                    Worker.w_id,
                     workerNameEntry.Text,
                     workerSurnameEntry.Text,
                     workerPatronymicEntry.Text,
                     workerPositionEntry.Text,
                     workerSalaryEntry.Text,
                     workerPhoneNumberEntry.Text,
                     workerAddressEntry.Text,
                     workerAdditionalInfoEntry.Text);

            }
        }

        async void deleteWorkerButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
            await helper.DeleteWorker(Worker.w_id);
        }
    }
}
