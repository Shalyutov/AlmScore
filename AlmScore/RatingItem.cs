using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmScore
{
    public class RatingItem : INotifyPropertyChanged
    {
        private string _participant;
        private int _points;
        private int _delta;
        public string Participant
        {
            get => _participant;
            set
            {
                if (_participant != value)
                {
                    _participant = value;
                    OnPropertyChanged(nameof(Participant));
                }
            }
        }
        public int Points 
        { 
            get => _points; 
            set 
            {
                if (_points != value)
                {
                    _points = value; 
                    OnPropertyChanged(nameof(Points)); 
                }
            }
        }

        public int Delta
        {
            get => _delta;
            set
            {
                if (_delta != value)
                {
                    _delta = value;
                    OnPropertyChanged(nameof(Delta));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async void AddPointsAnimate(int points)
        {
            Delta = points;
            await Task.Delay(2000);

            int amount = points;
            while (amount > 0)
            {
                if (amount / 2 == 0)
                {
                    Points += amount;
                    break;
                }
                Points += amount / 2;
                amount -= amount / 2;
                await Task.Delay(20);
            }
        }
    }
}
