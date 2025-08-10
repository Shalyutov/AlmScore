using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AlmScore
{
    public sealed partial class ScoreControl : UserControl
    {
        public static readonly DependencyProperty RatingProperty =
        DependencyProperty.Register(nameof(Rating), typeof(RatingItem), typeof(ScoreControl), new PropertyMetadata(null));
        /*public static readonly DependencyProperty ParticipantProperty =
        DependencyProperty.Register(nameof(Participant), typeof(string), typeof(ScoreControl), new PropertyMetadata(""));
        public static readonly DependencyProperty PointsProperty =
        DependencyProperty.Register(nameof(Points), typeof(int), typeof(ScoreControl), new PropertyMetadata(0));
        public string Participant
        {
            get => (string)GetValue(ParticipantProperty);
            set => SetValue(ParticipantProperty, value);
        }

        public int Points
        {
            get => (int)GetValue(PointsProperty);
            set => SetValue(PointsProperty, value);
        }*/
        public RatingItem Rating
        {
            get => (RatingItem)GetValue(RatingProperty);
            set => SetValue(RatingProperty, value);
        }

        public ScoreControl()
        {
            InitializeComponent();
        }

        public async void AnimateReveal(int place)
        {
            Reveal.BeginTime = new TimeSpan(0, 0, 0, 0, place * 50);
            Reveal.Begin();
            await Task.Delay(place * 50);
            Root.Opacity = 1;
        }
        public void AnimateHide()
        {
            Hide.Begin();
        }

        public void AnimateReceive()
        {
            Receive.Begin();
        }
    }
}
