using LocalEyesTipApp.Interfaces;
using LocalEyesTipApp.Models;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Media;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;
using LocalEyesTipApp.ViewModels;
using LocalEyesTipApp.Helpers;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm;

namespace LocalEyesTipApp.Pages;

public partial class SendTipPage : ContentPage
{
    public SendTipPage(SendTipViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (!await PermissionsChecker.CheckPermissions())
        {
            await Shell.Current.DisplayAlert("Manglende tilladelser!", "Nogle af de påkrævede tilladelser mangler. For at benytte tip funktionen skal disse tilladelser manuelt accepteres under indstillinger i din tlf!", "Ok");
        }
    }

    private void Entry_Focused(object sender, FocusEventArgs e)
    {
        //Shell.SetTabBarIsVisible(this, false);
    }

    private void Entry_Unfocused(object sender, FocusEventArgs e)
    {
        //Shell.SetTabBarIsVisible(this, true);
    }
}