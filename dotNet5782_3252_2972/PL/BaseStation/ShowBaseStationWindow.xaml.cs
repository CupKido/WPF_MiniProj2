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
using System.ComponentModel;

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
            Width = 300;
        }
        public ShowBaseStationWindow(int BaseStationId)
        {
            InitializeComponent();
            this.DataContext = myBL.GetBaseStation(BaseStationId);
            
            IfPresentation.IsChecked = true;
            BaseStationId_TextBox.IsEnabled = false;
            BaseStationChargeSlotsTaken_TextBox.IsEnabled = false;

        }

        private void CloseWindow_Button_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = disallowClosure;
        }
        private void AddBaseStation_Button_Click(object sender, RoutedEventArgs e)
        {
            int BsId , numOfCharges;
            double latitude, longitude;
            BO.Location location = new BO.Location();
            
            if (!int.TryParse(BaseStationId_TextBox.Text, out BsId))
            {
                MessageBox.Show("ID must be a Number!", "Wrong ID type", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (BaseStationName_TextBox.Text == "")
            {
                MessageBox.Show("Please enter the Base Station's name", "Empty name value", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            if (!double.TryParse(BaseStationLongitude_TextBox.Text, out longitude))
            {
                MessageBox.Show("Longitude must be a Number!", "Wrong Longitude type", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!double.TryParse(BaseStationLatitude_TextBox.Text, out latitude))
            {
                MessageBox.Show("LAtitude must be a Number!", "Wrong LAtitude type", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!int.TryParse(BaseStationChargeSlots_TextBox.Text, out numOfCharges))
            {
                MessageBox.Show("Number of charges must be a Number!", "Wrong Number of charges type", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            location.Latitude = latitude;
            location.Longitude = longitude;
            try
            {
                myBL.AddBaseStation(BsId, BaseStationName_TextBox.Text, location, numOfCharges);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            CloseWindow();
        }
        public void CloseWindow()
        {
            disallowClosure = false;
            this.Close();
        }

        private void Update_Button_Click(object sender, RoutedEventArgs e)
        {
            int BsId = int.Parse(BaseStationId_TextBox.Text);
            int takenSlots, availableSlots;
            string BsName = BaseStationName_TextBox.Text;
            if (BaseStationName_TextBox.Text == "")
            {
                MessageBox.Show("Please enter the Base station's name", "Empty name value", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            takenSlots = int.Parse(BaseStationChargeSlotsTaken_TextBox.Text);
            bool check = int.TryParse(BaseStationChargeSlotsAvailable_TextBox.Text, out availableSlots);
            if(!check)
            {
                MessageBox.Show(" Available slots has to be a number", "Wrong type number", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                myBL.UpdateBaseStation(BsId, BaseStationName_TextBox.Text , availableSlots + takenSlots);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            disallowClosure = false;
            this.Close();
        }

        private void syncWithSimulator()
        {
            List<AddDroneWindow> windows = (from Window w in App.Current.Windows
                                            where w.GetType() == typeof(AddDroneWindow)
                                            select (AddDroneWindow)w).ToList();
            foreach (AddDroneWindow ADW in windows)
            {
                if (ADW.thisDroneId != 0)
                {
                    ADW.SimulatorWorker.ProgressChanged += (s, e) =>
                    {
                        try
                        {
                            BO.Drone d = myBL.GetDrone(ADW.thisDroneId);
                            int BSId = 0;
                            if (d.Status == BO.DroneStatuses.Maintenance)
                            {
                                BSId = myBL.GetChrgingInBaseStationId(d);

                            }
                            else if (d.Status == BO.DroneStatuses.Availible && d.Battery > 90)
                            {
                                BSId = myBL.closestBaseStation(d.CurrentLocation.Longitude, d.CurrentLocation.Latitude).Id;
                            }
                            else
                            {
                                return;
                            }
                            if(BSId == int.Parse(BaseStationId_TextBox.Text))
                            {
                                this.DataContext = myBL.GetBaseStation(BSId);
                                
                            }
                        }
                        catch
                        {
                            
                        }
                    };
                }

            }
        }

        private void ChargedDronesList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AddDroneWindow ADW = new AddDroneWindow(((sender as ListView).SelectedItem as BO.DroneInCharge).Id);
            ADW.SimulatorWorker.ProgressChanged += (s, e) =>
            {
                try
                {
                    BO.Drone d = myBL.GetDrone(ADW.thisDroneId);
                    int BSId = 0;
                    if (d.Status == BO.DroneStatuses.Maintenance)
                    {
                        BSId = myBL.GetChrgingInBaseStationId(d);

                    }
                    else if (d.Status == BO.DroneStatuses.Availible && d.Battery > 90)
                    {
                        BSId = myBL.closestBaseStation(d.CurrentLocation.Longitude, d.CurrentLocation.Latitude).Id;
                    }
                    else
                    {
                        return;
                    }
                    if (BSId == int.Parse(BaseStationId_TextBox.Text))
                    {
                        this.DataContext = myBL.GetBaseStation(BSId);
                        
                    }
                }
                catch
                {

                }
            };
            ADW.Show();
        }
    }
}
