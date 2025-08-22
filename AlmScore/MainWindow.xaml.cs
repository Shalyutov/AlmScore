using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
        private ObservableCollection<RatingItem> RatingItems { get; set; } = new();
        public MainWindow()
        {
            InitializeComponent();
            RatingItems.Add(new() { Participant = "Россия", Points = 0 });
            RatingItems.Add(new() { Participant = "Беларусь", Points = 0 });
            RatingItems.Add(new() { Participant = "Казахстан", Points = 0 });
            RatingItems.Add(new() { Participant = "Монголия", Points = 0 });
            RatingItems.Add(new() { Participant = "Армения", Points = 0 });
            RatingItems.Add(new() { Participant = "Китай", Points = 0 });
            RatingItems.Add(new() { Participant = "Северная Корея", Points = 0 });
            RatingItems.Add(new() { Participant = "Узбекистан", Points = 0 });
            RatingItems.Add(new() { Participant = "Таджикистан", Points = 0 });
            RatingItems.Add(new() { Participant = "Молдова", Points = 0 });
            RatingItems.Add(new() { Participant = "Сербия", Points = 0 });
            RatingItems.Add(new() { Participant = "Грузия", Points = 0 });
            RatingItems.Add(new() { Participant = "Литва", Points = 0 });
            RatingItems.Add(new() { Participant = "Латвия", Points = 0 });
            RatingItems.Add(new() { Participant = "Словакия", Points = 0 });
            RatingItems.Add(new() { Participant = "Недерланды", Points = 0 });
            RatingItems.Add(new() { Participant = "Германия", Points = 0 });
            RatingItems.Add(new() { Participant = "Великобритания", Points = 0 });
            RatingItems.Add(new() { Participant = "Испания", Points = 0 });
            RatingItems.Add(new() { Participant = "Италия", Points = 0 });
            RatingItems.Add(new() { Participant = "Греция", Points = 0 });
            RatingItems.Add(new() { Participant = "Азербайджан", Points = 0 });
            RatingItems.Add(new() { Participant = "Турция", Points = 0 });
            RatingItems.Add(new() { Participant = "Мальта", Points = 0 });
            RatingItems.Add(new() { Participant = "Австрия", Points = 0 });
            RatingItems.Add(new() { Participant = "Швеция", Points = 0 });

            foreach (var item in RatingItems)
            {
                ItemsList.Items.Add(new ScoreControl() { Rating = item, Margin = new Thickness(2) });
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            /*RatingItems[1].Points += 100;
            await Task.Delay(1000);
            
            var i = ItemsList.Items[1];
            (i as ScoreControl)?.AnimateHide();
            await Task.Delay(500);
            ItemsList.Items.RemoveAt(1);
            ItemsList.Items.Insert(0, i);
            (i as ScoreControl)?.AnimateReveal(0);
            return;*/
            int place = 0;
            foreach (ScoreControl item in ItemsList.Items)
            {
                item.AnimateReveal(place % 13 + place / 13 * 5);
                place++;
            }

            /*for (int j = 0; j < RatingItems.Count; j++)
            {
                element = RatingList.GetOrCreateElement(j);
                (element as ScoreControl)?.AnimateReveal(j);
            }*/
        }

        private async void ReorderScoreboard()
        {
            var marked = new List<RatingItem>();
            
            for (int i = 0; i < RatingItems.Count; i++)
            {
                var item = RatingItems[i];
                if (item.Delta > 0)
                {
                    marked.Add(item);
                    var score = ItemsList.Items[i] as ScoreControl;
                    score?.AnimateHide();

                    int oldIndex = i;
                    int offset = 1;
                    bool moved = false;
                    while (true)
                    {
                        if (oldIndex - offset >= 0)
                        {
                            if (RatingItems[oldIndex].Points > RatingItems[oldIndex - offset].Points)
                            {
                                offset++;
                                moved = true;
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
                    if (moved)
                    {
                        RatingItems.Move(oldIndex, oldIndex - offset + 1);
                    }
                }
            }

            await Task.Delay(500);
            for (int i = 0; i < RatingItems.Count; i++) {
                if (marked.Contains(RatingItems[i]))
                {
                    var score = ItemsList.Items.First((sc) => { return (sc as ScoreControl)?.Rating == RatingItems[i]; }) as ScoreControl;
                    ItemsList.Items.Remove(score);
                    ItemsList.Items.Insert(i, score);
                    score?.AnimateReceivePoints();
                }
            }
        }
        private async void Button_Click2(object sender, RoutedEventArgs e)
        {
            int i = rand.Next(0, 26);
            var r = new List<int>();
            foreach (int amount in new List<int>([1,2,3,4,5,6,7,8,10,12]))
            {
                while (r.Contains(i))
                {
                    i = rand.Next(0, 26);
                }
                RatingItems[i].AddPointsAnimate(amount);
                if (amount == 12)
                {
                    (ItemsList.Items[i] as ScoreControl)?.AnimateHighMark();
                }
                else
                {
                    (ItemsList.Items[i] as ScoreControl)?.AnimateReceive();
                }
                r.Add(i);
            }

            await Task.Delay(3000);
            ReorderScoreboard();
            foreach (RatingItem item in RatingItems)
            {
                //item.Delta = 0;
            }
        }
    }
}
