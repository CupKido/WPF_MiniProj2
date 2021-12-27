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
using System.Windows.Shapes;
using BlApi;

namespace PL.BaseStation
{
    /// <summary>
    /// Interaction logic for ShowBaseStationWindow.xaml
    /// </summary>
    public partial class ShowBaseStationWindow : Window
    {

        BlApi.IBL myBL = BlFactory.GetBL();
        bool disallowClosure = true;
        public ShowBaseStationWindow()
        {
            InitializeComponent();
        }

        private void CloseWindow_Button_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        private void AddBaseStation_Button_Click(object sender, RoutedEventArgs e)
        {

        }
        public void CloseWindow()
        {
            disallowClosure = false;
            this.Close();
        }
    }
}