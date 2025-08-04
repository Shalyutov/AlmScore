using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Gaming.XboxLive.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AlmScore
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        Random rand = new Random();
        private ObservableCollection<RatingItem> RatingItems { get; set; } = new ObservableCollection<RatingItem>();
        public MainWindow()
        {
            InitializeComponent();
            RatingItems.Add(new() { Participant = "������", Points = 0 });
            RatingItems.Add(new() { Participant = "��������", Points = 0 });
            RatingItems.Add(new() { Participant = "���������", Points = 0 });
            RatingItems.Add(new() { Participant = "��������", Points = 0 });
            RatingItems.Add(new() { Participant = "�������", Points = 0 });
            RatingItems.Add(new() { Participant = "�����", Points = 0 });
            RatingItems.Add(new() { Participant = "�������� �����", Points = 0 });
            RatingItems.Add(new() { Participant = "����������", Points = 0 });
            RatingItems.Add(new() { Participant = "�����������", Points = 0 });
            RatingItems.Add(new() { Participant = "�������", Points = 0 });
            RatingItems.Add(new() { Participant = "������", Points = 0 });
            RatingItems.Add(new() { Participant = "������", Points = 0 });
            RatingItems.Add(new() { Participant = "�����", Points = 0 });
            RatingItems.Add(new() { Participant = "������", Points = 0 });
            RatingItems.Add(new() { Participant = "��������", Points = 0 });
            RatingItems.Add(new() { Participant = "����������", Points = 0 });
            RatingItems.Add(new() { Participant = "��������", Points = 0 });
            RatingItems.Add(new() { Participant = "��������������", Points = 0 });
            RatingItems.Add(new() { Participant = "�������", Points = 0 });
            RatingItems.Add(new() { Participant = "������", Points = 0 });
            RatingItems.Add(new() { Participant = "������", Points = 0 });
            RatingItems.Add(new() { Participant = "�����������", Points = 0 });
            RatingItems.Add(new() { Participant = "������", Points = 0 });
            RatingItems.Add(new() { Participant = "������", Points = 0 });
            RatingItems.Add(new() { Participant = "�������", Points = 0 });
            RatingItems.Add(new() { Participant = "������", Points = 0 });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            for (int j = 0; j < RatingItems.Count; j++)
            {
                var element = RatingList.GetOrCreateElement(j);
                (element as ScoreControl)?.AnimateReveal(j);
            }
        }
        private async void Button_Click2(object sender, RoutedEventArgs e)
        {
            int i = rand.Next(0, 26);
            int amount = rand.Next(1, 100);
            while (amount > 0) {
                if (amount / 2 == 0) {
                    RatingItems[i].Points += amount;
                    break;
                }
                RatingItems[i].Points += amount / 2;
                amount -= amount / 2;
                await Task.Delay(20);
            }
            var element = RatingList.GetOrCreateElement(i);
            await Task.Delay(1000);
            (element as ScoreControl)?.AnimateHide();
            while (true)
            {
                if (i > 0)
                {
                    if (RatingItems[i].Points > RatingItems[i - 1].Points)
                    {
                        (RatingItems[i], RatingItems[i - 1]) = (RatingItems[i - 1], RatingItems[i]);
                        i--;
                        await Task.Delay(100);
                    }
                    else
                    {
                        break;
                    }
                }
                else 
                { 
                    break; 
                }
            }
            (element as ScoreControl)?.AnimateReveal(0);
        }
    }
}
