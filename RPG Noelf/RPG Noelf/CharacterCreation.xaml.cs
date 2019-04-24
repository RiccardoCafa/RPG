﻿using RPG_Noelf.Assets.Scripts.General;
using RPG_Noelf.Assets.Scripts.PlayerFolder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace RPG_Noelf
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class CharacterCreation : Page
    {

        public Dictionary<string, Image> PlayerImages;
        public Dictionary<string, Image> ClothesImages;
        Player CustomPlayer;

        public CharacterCreation()
        {
            this.InitializeComponent();
            
            PlayerImages = new Dictionary<string, Image>()
            {
                {"armsd0", xPlayerArm_d0 },
                {"armsd1", xPlayerArm_d1 },
                {"armse0", xPlayerArm_e0 },
                {"armse1", xPlayerArm_e1 },
                {"body", xPlayerBody },
                {"head", xPlayerHead },
                {"eye", xPlayerEye },
                {"hair", xPlayerHair },
                {"legsd0", xPlayerLeg_d0 },
                {"legsd1", xPlayerLeg_d1 },
                {"legse0", xPlayerLeg_e0 },
                {"legse1", xPlayerLeg_e1 }
            };
            ClothesImages = new Dictionary<string, Image>()
            {
                {"armsd0", xClothArm_d0 },
                {"armsd1", xClothArm_d1 },
                {"armse0", xClothArm_e0 },
                {"armse1", xClothArm_e1 },
                {"body", xClothBody },
                {"legsd0", xClothLeg_d0 },
                {"legsd1", xClothLeg_d1 },
                {"legse0", xClothLeg_e0 },
                {"legse1", xClothLeg_e1 }
            };
            CustomPlayer = new Player("0000000", PlayerImages, ClothesImages);
        }
        
        private string ChangeCustom(char current, int range, bool isNext)//metodo auxiliar de ClickCustom()
        {
            int.TryParse(current.ToString(), out int x);
            if (isNext)
            {
                if (x == range - 1) x = 0;
                else x++;
            }
            else
            {
                if (x == 0) x = range - 1;
                else x--;
            }
            return x.ToString();
        }
        private void ClickCustom(object sender, RoutedEventArgs e)//gerencia a customizaçao do player (temporario)
        {
            string id = CustomPlayer.Id;
            if (sender == xEsqRace ||
                sender == xDirRace) id = ChangeCustom(id[0], 3, sender == xDirRace) + id.Substring(1, 6);
            else if (sender == xEsqClass ||
                     sender == xDirClass) id = id.Substring(0, 1) + ChangeCustom(id[1], 3, sender == xDirClass) + id.Substring(2, 5);
            else if (sender == xEsqSex ||
                     sender == xDirSex) id = id.Substring(0, 2) + ChangeCustom(id[2], 2, sender == xDirSex) + id.Substring(3, 4);
            else if (sender == xEsqSkinTone ||
                     sender == xDirSkinTone) id = id.Substring(0, 3) + ChangeCustom(id[3], 3, sender == xDirSkinTone) + id.Substring(4, 3);
            else if (sender == xEsqEyeColor ||
                     sender == xDirEyeColor) id = id.Substring(0, 4) + ChangeCustom(id[4], 3, sender == xDirEyeColor) + id.Substring(5, 2);
            else if (sender == xEsqHairStyle ||
                     sender == xDirHairStyle) id = id.Substring(0, 5) + ChangeCustom(id[5], 4, sender == xDirHairStyle) + id.Substring(6, 1);
            else if (sender == xEsqHairColor ||
                     sender == xDirHairColor) id = id.Substring(0, 6) + ChangeCustom(id[6], 3, sender == xDirHairColor);
            CustomPlayer = new Player(id, PlayerImages, ClothesImages);
            CustomPlayer.Status(xPlayerStatus);
            CodigoChar.Text = CustomPlayer.Id;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var viewId = 0;
            CharacterCreationParams cParams = new CharacterCreationParams();
            cParams.playerCreated = CustomPlayer;
            var newView = CoreApplication.CreateNewView();
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                var frame = new Frame();
                frame.Navigate(typeof(Game), cParams);
                Window.Current.Content = frame;

                viewId = ApplicationView.GetForCurrentView().Id;

                //ApplicationView.GetForCurrentView().Consolidated += App.;

                Window.Current.Activate();
            });
            var viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(viewId);
        }
    }

    public class CharacterCreationParams
    {
        public Player playerCreated { get; set; }
    }
}