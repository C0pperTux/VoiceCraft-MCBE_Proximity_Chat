﻿using VoiceCraft.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VoiceCraft.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VoicePage : ContentPage
    {
        public VoicePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var viewModel = (VoicePageViewModel)BindingContext;
            if (viewModel.AppearingCommand.CanExecute(null))
                viewModel.AppearingCommand.Execute(null);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            var viewModel = (VoicePageViewModel)BindingContext;
            if (viewModel.DisappearingCommand.CanExecute(null))
                viewModel.DisappearingCommand.Execute(null);
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}