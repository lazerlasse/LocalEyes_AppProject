﻿using LocalEyesTipApp.Pages;

namespace LocalEyesTipApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(SendTipPage), typeof(SendTipPage));
        }
    }
}