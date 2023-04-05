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

namespace LocalEyesTipApp.Pages;

public partial class SendTipPage : ContentPage
{
    public SendTipPage(SendTipViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}