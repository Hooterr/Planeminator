﻿using Autofac;
using Planeminator.DataIO.Public.Services;
using Planeminator.DesktopApp.Core.ViewModels;
using Planeminator.DesktopApp.WPFViewModels;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Planeminator.DesktopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly WindowViewModel ViewModel;

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new WindowViewModel();
            DataContext = ViewModel;
        }
    }
}
