using AoS_DamageCalculator.Models;
using AoS_DamageCalculator.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AoS_DamageCalculator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; set; } = new MainViewModel();

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await ViewModel.Init();

            await ViewModel.Load();
        }

        private async void SelectModel(object sender, SelectionChangedEventArgs e)
        {
            var modelsList = sender as ListView;
            if (modelsList == null || modelsList.SelectedItem == ViewModel.SelectedModel) return;

            if (ViewModel.HasChanges)
            {
                var msg = new MessageDialog("You have unsaved changes.  Please click Save or Cancel", "Unsaved Changes");
                await msg.ShowAsync();
                modelsList.SelectedItem = ViewModel.SelectedModel;
                return;
            }
            ViewModel.SelectedModel = modelsList.SelectedItem as Model;
            //ModelListView.SelectedItem = ViewModel.SelectedModel;
            var weapon = ViewModel.SelectedModel.Weapons.FirstOrDefault();
            if (weapon != null)
            {
                ViewModel.SelectedWeapon = ViewModel.SelectedModel.Weapons.First() as Weapon;
                WeaponListView.SelectedItem = ViewModel.SelectedWeapon;
                ViewModel.CalculateExpectedDamage();
                ClearCheckBoxes();
            }
        }

        private void SelectWeapon(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SelectedWeapon = (sender as ListView)?.SelectedItem as Weapon;
            //WeaponListView.SelectedItem = ViewModel.SelectedWeapon;
            ViewModel.CalculateExpectedDamage();
            ClearCheckBoxes();
        }

        private void RerollHitsCheckBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Check to see which checkbox was tapped
            var thisCheckBox = (CheckBox)sender;
            var thisCheckBoxName = thisCheckBox.Name.ToString();

            if (thisCheckBoxName == "RerollHitsOneCheckBox")
            {
                RerollAllHitsCheckBox.IsChecked = false;
            }
            else
            {
                RerollHitsOneCheckBox.IsChecked = false;
            }
            
            var oneHitsChecked = RerollHitsOneCheckBox.IsChecked;
            var allHitsChecked = RerollAllHitsCheckBox.IsChecked;

            // Send the values of these boxes to the ViewModel
            if (oneHitsChecked == false && allHitsChecked == false)
            {
                ViewModel.HitRerolls = "None";
            }
            else if (oneHitsChecked == true && allHitsChecked == false)
            {
                ViewModel.HitRerolls = "One";
            }
            else if (oneHitsChecked == false && allHitsChecked == true)
            {
                ViewModel.HitRerolls = "All";
            }
            else
            {
                ViewModel.HitRerolls = "None";
            }
            ViewModel.CalculateExpectedDamage();
        }

        private void RerollWoundsCheckBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Check to see which checkbox was tapped
            var thisCheckBox = (CheckBox)sender;
            var thisCheckBoxName = thisCheckBox.Name.ToString();

            if (thisCheckBoxName == "RerollWoundsOneCheckBox")
            {
                RerollAllWoundsCheckBox.IsChecked = false;
            }
            else
            {
                RerollWoundsOneCheckBox.IsChecked = false;
            }

            // Check to see if this checkbox is checked
            var oneWoundsChecked = RerollWoundsOneCheckBox.IsChecked;
            var allWoundsChecked = RerollAllWoundsCheckBox.IsChecked;

            // Send the values of these boxes to the ViewModel
            if (oneWoundsChecked == false && allWoundsChecked == false)
            {
                ViewModel.WoundRerolls = "None";
            }
            else if (oneWoundsChecked == true && allWoundsChecked == false)
            {
                ViewModel.WoundRerolls = "One";
            }
            else if (oneWoundsChecked == false && allWoundsChecked == true)
            {
                ViewModel.WoundRerolls = "All";
            }
            else
            {
                ViewModel.WoundRerolls = "None";
            }
            ViewModel.CalculateExpectedDamage();
        }

        //private void HitMortalWoundsCheckBox_Tapped(object sender, TappedRoutedEventArgs e)
        //{
        //    if (HitMortalWoundsCheckBox.IsChecked == true)
        //    {
        //        ViewModel.HitMortalWounds = "Yes";
        //    }
        //    else
        //    {
        //        ViewModel.HitMortalWounds = "No";
        //    }
        //}
        private void ClearCheckBoxes()
        {
            RerollHitsOneCheckBox.IsChecked = false;
            RerollAllHitsCheckBox.IsChecked = false;
            RerollWoundsOneCheckBox.IsChecked = false;
            RerollAllWoundsCheckBox.IsChecked = false;
        }
    }
}
