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

        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
