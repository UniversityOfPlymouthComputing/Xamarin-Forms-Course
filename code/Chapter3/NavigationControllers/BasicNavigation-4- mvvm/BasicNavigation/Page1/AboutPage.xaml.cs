﻿using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace BasicNavigation
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage(AboutPageViewModel vm = null)
        {
            InitializeComponent();

            //This next page does non-destructive editing - back button title reflects this
            NavigationPage.SetBackButtonTitle(this, "Cancel");
            
            //Bind to the view model
            BindingContext = vm ?? new AboutPageViewModel();
        }
    }
}
