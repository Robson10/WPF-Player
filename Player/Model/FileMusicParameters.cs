using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism;
namespace Player.Model
{
    public class FileMusicParameters:Prism.Mvvm.BindableBase
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public int Time { get; set; }
        private bool _isPlaying;
        public bool IsPlaying
        {
            get
            {
                return _isPlaying;
            }
            set
            {
                _isPlaying = value;
                OnPropertyChanged();
            }
        }
        private bool _isSelected;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }

    }
}
