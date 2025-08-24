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
        private List<Vote> Votes { get; set; } = new();
        private List<string> juryOrder = new(["Россия","Беларусь"]);
        private List<string> publicOrder = new();
        private List<string> participants = new(["Россия","Беларусь","Казахстан","Монголия","Армения","Китай","Северная Корея","Узбекистан","Таджикистан","Молдова", 
        "Сербия","Грузия","Литва","Латвия","Словакия","Недерланды","Германия","Великобритания","Испания","Италия","Греция","Азербайджан","Турция","Мальта","Австрия","Швеция",]);
        private List<int> marks = new([1, 2, 3, 4, 5, 6, 7, 8, 10, 12]);
        private List<Vote> currentPacket;

        private bool isPacketGiven = false;
        private bool isGighMarkGiven = false;
        private bool isReady = true;
        private int currentJury = 0;
        public MainWindow()
        {
            InitializeComponent();

            foreach (string participant in participants)
            {
                RatingItems.Add(new() { Participant = participant, Points = 0 });
            }

            foreach (var item in RatingItems)
            {
                ItemsList.Items.Add(new ScoreControl() { Rating = item, Margin = new Thickness(2) });
            }

            foreach(var mark in marks)
            {
                Votes.Add(new() { From = "Россия", Points = mark, Issuer=Vote.VoteIssuer.Jury, To = participants[mark] });
                Votes.Add(new() { From = "Беларусь", Points = mark, Issuer = Vote.VoteIssuer.Jury, To = participants[mark+4] });
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RevealRating();
        }
        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            HideRating();
        }

        private void RevealRating()
        {
            int place = 0;
            foreach (ScoreControl item in ItemsList.Items)
            {
                item.AnimateReveal(place % 13 + place / 13 * 5);
                place++;
            }
        }
        private void HideRating()
        {
            foreach (ScoreControl item in ItemsList.Items)
            {
                item.AnimateHide();
            }
        }
        private async void LoadMarksPacket(string participant)
        {
            foreach (ScoreControl sc in ItemsList.Items)
            {
                if (sc.Rating.Delta > 0)
                {
                    sc.AnimateHideMark();
                }
            }
            await Task.Delay(1000);
            foreach (var ri in RatingItems)
            {
                ri.Delta = 0;
            }

            currentPacket = Votes.FindAll((v) => { return v.From == participant && v.Issuer == Vote.VoteIssuer.Jury; });
            isPacketGiven = false;
            isGighMarkGiven = false;
        }

        private async void GiveMarksPacket()
        {
            foreach(Vote vote in currentPacket)
            {
                if (vote.Points == marks.Last())
                {
                    continue;
                }
                var ri = RatingItems.First((ri) => ri.Participant == vote.To);
                ri.AddPointsAnimate(vote.Points);
                var i = RatingItems.IndexOf(ri);
                (ItemsList.Items[i] as ScoreControl)?.AnimateReceive();
            }
            isPacketGiven = true;
            await Task.Delay(3000);
            ReorderScoreboard();
        }

        private async void GiveHighMark()
        {
            Vote? vote = currentPacket.Find((v) => v.Points == marks.Last());
            var ri = RatingItems.First((ri) => ri.Participant == vote?.To);
            ri.AddPointsAnimate(vote!.Points);
            var i = RatingItems.IndexOf(ri);
            (ItemsList.Items[i] as ScoreControl)?.AnimateHighMark();
            isGighMarkGiven = true;
            await Task.Delay(3000);
            ReorderScoreboard();
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
                            if (RatingItems[oldIndex].Points > RatingItems[oldIndex - offset].Points)//todo compare quantity of high marks (eurovision rule)
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
            if (isReady)
            {
                LoadMarksPacket(juryOrder[currentJury]);
                isReady = false;
                return;
            }
            if (!isPacketGiven)
            {
                GiveMarksPacket();
                isPacketGiven = true;
                return;
            }
            if (!isGighMarkGiven)
            {
                GiveHighMark();
                isGighMarkGiven = true;
                isReady = true;
                currentJury++;
                return;
            }

            return;
            int i = rand.Next(0, 26);
            var r = new List<int>();
            foreach (int amount in marks)
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
        }
    }
}
