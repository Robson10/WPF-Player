using Player.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Prism.Mvvm;
namespace Player.Services
{
    public class PlayerService : BindableBase
    {
        private volatile MediaPlayer _Player;
        public MusicFilesAndPlaylists MusicList { get; set; }
        private Random rnd = new Random();

        private bool _isRandom = Configuration.isRandom;
        public bool isRandom
        { 
            get { return _isRandom; }
            set { _isRandom = value; OnPropertyChanged(); } 
        }

        private bool _isPaused = false;
        public volatile bool isPlaying = false;

        private int _IndexToPlay;
        public int IndexToPlay
        {
            get { return _IndexToPlay; }
            set
            {
                if (isRandom == true)
                    value += rnd.Next(0, MusicList.Count / 3);

                if (value < MusicList.Count && value >= 0)
                    _IndexToPlay = value;
                else if (value >= MusicList.Count)
                    _IndexToPlay = value % MusicList.Count ;
                else if (value < 0)
                    _IndexToPlay = MusicList.Count - 1;
            }
        }

        private double _Volume = Configuration.DefaultVolume;
        public int Volume
        {
            get { return (int)Math.Round(_Volume * 100); }
            set
            {
                if (value >= 0 && value <= 100)
                {
                    _Player.Volume = value / 100.0;
                    _Volume=_Player.Volume;
                    OnPropertyChanged();
                }
            }
        }

        private int _duration = 1;
        public int Duration
        {
            set
            {
                SetProperty(ref _duration, value);
            }
            get
            {
                return _duration;
            }
        }

        public  int Position
        {
            set { _Player.Position = new TimeSpan(0, 0, value); }
        }

        public PlayerService()
        {
            _Player = new MediaPlayer();
            Initialize();
        }
        private void Initialize()
        {
            _Player.MediaEnded += _Player_MediaEnded;
        }
        
        public void Play(int SelectedIndex)
        {
            MusicList[IndexToPlay].IsPlaying = false;
            IndexToPlay = SelectedIndex;
            MusicList[IndexToPlay].IsPlaying = true;
            if (!_isPaused)
                _OpenFile();
            _Player.Play();
            _isPaused = false;
            isPlaying = true;
        }
        public void Next()
        {
            Play(IndexToPlay+1);

        }
        public void Prev()
        {
            Play(IndexToPlay-1);
        }
        public void Stop()
        {
            _isPaused = false;
            _Player.Stop();
            isPlaying = false;
        }
        public void Pause()
        {
            if (_isPaused == true)
                Play(IndexToPlay);
            else
            {
                _Player.Pause();
                _isPaused = true;
            }
            isPlaying = false;
        }
        private void _OpenFile()
        {
            _Player.Open(new Uri(MusicList[IndexToPlay].Path));
            Duration = MusicList[IndexToPlay].Time;
        }
        private void _Player_MediaEnded(object sender, EventArgs e)
        {
            Next();
        }
    }
}
