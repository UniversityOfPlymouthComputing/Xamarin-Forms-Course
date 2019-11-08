﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace BasicNavigation
{
    public class AboutPageViewModel : INotifyPropertyChanged
    {
        //Model
        private PersonDetailsModel model;
        public PersonDetailsModel Model
        {
            get => model;
            set
            {
                if (model != value)
                {
                    model = value;
                    OnPropertyChanged();
                }

            }
        }

        //Useful property to reference the navigation page
        protected INavigation Navigation => Application.Current.MainPage.Navigation;

        //Event handling
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand ButtonCommand { get; set; }

        //Bound Data Properties Exposed to the View
        public string Name => Model.Name;   //Read Only
        public int BirthYear                //Read / Write
        {
            get => Model.BirthYear;
            set
            {
                if (value != Model.BirthYear)
                {
                    Model.BirthYear = value;
                    OnPropertyChanged();
                }
            }
        }

        //Create events when properties change
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //Main constructor
        public AboutPageViewModel(PersonDetailsModel model = null)
        {
            //Instantiate the model
            Model = model ?? new PersonDetailsModel("NickO");

            //Subscribe to changes in the model
            model.PropertyChanged += OnModelPropertyChanged;

            //The command property - bound to a button in the view
            ButtonCommand = new Command(execute: NavigateToAboutAboutPage);
        }

        //Watch for events on the model object
        protected void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //Flag changes to the view-viewmodel binding layer -  very simple pass-through in this example
            if (e.PropertyName.Equals(nameof(Model.BirthYear)))
            {
                OnPropertyChanged(nameof(BirthYear));
            }
            else if (e.PropertyName.Equals(nameof(Model.Name)))
            {
                OnPropertyChanged(nameof(Name));
            }
        }

        // Navigate to the About page - providing both View and ViewModel pair
        void NavigateToAboutAboutPage()
        {
            MessagingCenter.Subscribe<AboutAboutViewModel, string>(this, "NameUpdate", (sender, arg) =>
            {
                Model.Name = arg;
            });
            //This has a concrete reference to a view inside a VM - is this good/bad/indifferent?

            // Create viewmodel and pass datamodel as a parameter
            // NOTE that Model is a reference type
            AboutAboutViewModel aavm = new AboutAboutViewModel(Model.Name); //VM knows about its model (reference)

            // Instantiate the view, and provide the viewmodel
            AboutAbout aabout = new AboutAbout(aavm); //View knows about it's VM
            Navigation.PushAsync(aabout);
        }
    }
}
