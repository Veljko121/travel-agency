﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for TourRequestBookingView.xaml
    /// </summary>
    public partial class TourRequestBookingView : Page
    {
        public TourRequestBookingView(int activeGuideId, NavigationService navigationService)
        {
            InitializeComponent();
            this.DataContext = new TourRequestBookingViewModel(activeGuideId, navigationService);
        }
    }
}
