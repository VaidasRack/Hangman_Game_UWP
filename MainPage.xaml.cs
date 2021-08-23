using System;
using System.Collections.Generic;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Hangman_Game_UWP
{
    public sealed partial class MainPage : Page
    {
        List<Button> buttons;
        List<BitmapImage> images;
        List<TextBlock> fieldChar;
        string word;
        int counterMiss = 0;

        public MainPage()
        {
            this.InitializeComponent();
            ApplicationView view = ApplicationView.GetForCurrentView();
            view.TryEnterFullScreenMode();
            images = new List<BitmapImage>();
            LoadImage();
            DoWordArea();
        }

        private void LoadImage()
        {
            for (int i = 0; i < 7; i++)
            {
                var image = new BitmapImage(new Uri(@"ms-appx:/images/hang" + i.ToString() + ".png"));
                images.Add(image);
            }
        }

        private string RandomWord()
        {
            string[] words = { "DUBLINAS", "TIRANA", "SKOPJE", "ANDORA", "VALETA", "VIENA", "KISINIOVAS", "MINSKAS", "MONAKAS", "BRIUSELIS",
                               "AMSTERDAMAS", "SARAJEVAS", "OSLAS", "SOFIJA", "LISABONA", "PRAHA", "PARYZIUS", "KOPENHAGA", "BUKARESTAS",
                               "LONDONAS", "MASKVA", "TALINAS", "ATENAI", "LIUBLIANA", "REIKJAVIKAS", "BRATISLAVA", "ROMA", "HELSINKIS",
                               "BELGRADAS", "STOKHOLMAS", "ZAGREBAS", "BERNAS", "RYGA", "KIJEVAS", "VATIKANAS", "VARSUVA", "VADUCAS",
                               "VILNIUS", "BUDAPESTAS", "BERLYNAS", "LIUKSEMBURGAS" };
            Random r = new Random();
            return words[r.Next(words.Length)];
        }

        private void DoWordArea()
        {
            counterMiss = 0;
            CreateKeyboard();
            this.word = RandomWord();
            hang.Source = images[0];
            fieldChar = new List<TextBlock>();
            wordArea.Children.Clear();

            for (int i = 0; i < this.word.Length; i++)
            {
                TextBlock textBlock = new TextBlock()
                {
                    Text = "_",
                    Margin = new Thickness(10),
                    FontSize = 70
                };
                wordArea.Children.Add(textBlock);
                fieldChar.Add(textBlock);
            }
            fieldChar[0].Text = this.word[0].ToString();
            fieldChar[this.word.Length - 1].Text = this.word[this.word.Length - 1].ToString();
        }

        private void Btn_Click_Start(object sender, RoutedEventArgs e)
        {
            DoWordArea();
        }

        private void CreateKeyboard()
        {
            buttons = new List<Button>();
            firstRow.Children.Clear();
            secondRow.Children.Clear();
            thirdRow.Children.Clear();

            for (int i = 65; i < 91; i++)
            {
                Button b = new Button()
                {
                    Content = ((char)i).ToString(),
                    FontSize = 26,
                    Width = 80,
                    Height = 80,
                    Margin = new Thickness(2)
                };
                b.Click += Btn_Click_Key;

                if (i % 65 < 8) firstRow.Children.Add(b);
                else if (i % 65 >= 8 && i % 65 < 16) secondRow.Children.Add(b);
                else thirdRow.Children.Add(b);

                buttons.Add(b);
            }
        }

        private void Btn_Click_Key(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            string letter = b.Content.ToString();
            bool hit = false;

            for (int i = 1; i < this.word.Length - 1; i++)
            {
                if (this.word[i].ToString().ToUpper() == letter.ToUpper())
                {
                    hit = true;
                    fieldChar[i].Text = letter.ToUpper();
                }
            }

            if (hit == false)
            {
                counterMiss += 1;
                hang.Source = images[counterMiss];
            }

            if (counterMiss == 6)
            {
                MessageToUserAsync("Deja neatspėjote :(");
            }

            int count = 0;
            for (int i = 0; i < this.word.Length; i++)
            {
                if (fieldChar[i].Text != "_") count++;
            }

            if (count == this.word.Length)
            {
                MessageToUserAsync("SVEIKINAME!!! Jūs laimėjote!");
            }

            b.IsEnabled = false;
        }

        private async void MessageToUserAsync(string statement)
        {
            MessageDialog messageDialog = new MessageDialog($"Teisingas atsakymas: {word}", statement);
            await messageDialog.ShowAsync();
            DoWordArea();
        }
    }
}
