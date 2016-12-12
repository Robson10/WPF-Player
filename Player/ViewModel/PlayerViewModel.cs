namespace Player.ViewModel
{
    using System.Dynamic;
    using System.Windows.Input;
    using Prism.Mvvm;
    using Prism.Commands;
    using Player.Model;
    using System.Media;
    using System.Collections.ObjectModel;
    using System.Collections.Generic;
    using System.Collections;
    using System.Threading.Tasks;
    using System.Threading;
    using System.Windows.Media;
    using System;
    using System.Linq;
    using System.Windows.Forms;
    using Player.Services;

    public class PlayerViewModel : BindableBase
    {
        #region done
        private PlaylistService PlaylistService;
        public PlayerService PlayerService{ set; get; }

        public MusicFilesAndPlaylists MusicList
        {
            set { PlayerService.MusicList = value; }
            get { return PlayerService.MusicList; }
        }

        public int Volume
        {
            get { return PlayerService.Volume; }
            set { PlayerService.Volume = value; }
        }
        public bool _isRandom = false;
        public string isRandom
        {
            get 
            {
                if (_isRandom) return "Random is On"; 
                else return "Random is Off";
            }
            set { SetProperty(ref _isRandom, !_isRandom); }
        }

        public int SelectedIndex { get; set; }

        private TimeSpan _durationAsTimeSpan;
        public int Duration
        {
            set
            {
                PlayerService.Duration = value;
            }
            get
            {
                SetProperty(ref _durationAsTimeSpan, new TimeSpan(0, 0, PlayerService.Duration));
                return PlayerService.Duration;
            }
        }

        private Task _sliderChangePosition;

        private string _LabelTime;
        public string LabelTime
        {
            get { return _LabelTime; }
            set { SetProperty(ref _LabelTime, value); }
        }

        private int _rewindMusic;
        public int RewindMusic
        {
            get
            {
                return (int)_rewindMusic;
            }
            set
            {
                if (value < Duration && value >= 0)
                {
                    SetProperty(ref _rewindMusic, value);
                    LabelTime = (new TimeSpan(0, 0, RewindMusic) + "/" + _durationAsTimeSpan.ToString());
                }
            }
        }
        #endregion

   
        #region Done Commands
        public DelegateCommand ChangePosition { get; private set; }
        public DelegateCommand Cmd_Play { get; private set; }
        public DelegateCommand Cmd_Stop { get; private set; }
        public DelegateCommand Cmd_Pause { get; private set; }
        public DelegateCommand Cmd_Next { get; private set; }
        public DelegateCommand Cmd_Prev { get; private set; }

        public DelegateCommand RandomButton { get; private set; }

        public DelegateCommand<object> ListNew_Folder { get; private set; }
        public DelegateCommand<object> ListNew_Files { get; private set; }
        public DelegateCommand List_Clear { get; private set; }
        public DelegateCommand ListSavePlaylist { get; private set; }
        public DelegateCommand ListLoadPlaylist { get; private set; }

        public DelegateCommand SortBy_Title { get; private set; }
        public DelegateCommand SortBy_Path { get; private set; }
        public DelegateCommand SortBy_Name { get; private set; }
        public DelegateCommand SortBy_Random { get; private set; }

        public DelegateCommand<object> Shortcuts { get; private set; }
        #endregion
        public PlayerViewModel()
        {
            #region Done
            PlaylistService = new PlaylistService();
            PlayerService = new PlayerService();
            PlayerService.PropertyChanged += PlayerService_PropertyChanged;
            MusicList = new MusicFilesAndPlaylists();

            Cmd_Play = new DelegateCommand(Exe_Play);
            Cmd_Stop = new DelegateCommand(Exe_Stop);
            Cmd_Next = new DelegateCommand(Exe_Next);
            Cmd_Prev = new DelegateCommand(Exe_Prev);
            Cmd_Pause = new DelegateCommand(Exe_Pause);

            RandomButton = new DelegateCommand(Exe_PlayRandomly);

            ListNew_Folder = new DelegateCommand<object>(Exe_ListNew_Folder);
            ListNew_Files = new DelegateCommand<object>(Exe_ListNew_Files);
            List_Clear = new DelegateCommand(Exe_List_Clear);
            ListSavePlaylist = new DelegateCommand(Exe_ListSaveAsPlaylist);
            ListLoadPlaylist = new DelegateCommand(Exe_ListLoadPlaylist);

            SortBy_Title = new DelegateCommand(Exe_SortByTitle);
            SortBy_Random = new DelegateCommand(Exe_SortByRandom);
            SortBy_Path = new DelegateCommand(Exe_SortByPath);
            SortBy_Name = new DelegateCommand(Exe_SortByName);

            ChangePosition = new DelegateCommand(Exe_ChangePosition);
            _sliderChangePosition = new Task(SliderChangePosition);
            _sliderChangePosition.Start();

            Shortcuts = new DelegateCommand<object>(Exe_Shortcuts);
    
            #endregion
        }

        #region done


        private void Exe_Play()
        {
            PlayerService.Play(SelectedIndex);
            RewindMusic = 0;
        }
        private void Exe_Next()
        {
            PlayerService.Next();
            RewindMusic = 0;
        }
        private void Exe_Prev()
        {
            PlayerService.Prev();
            RewindMusic = 0;
        }
        private void Exe_Stop()
        {
            PlayerService.Stop();
        }
        private void Exe_Pause()
        {
            PlayerService.Pause();
        }

        private void Exe_ListNew_Folder(object isNewList)
        {
            MusicList = PlaylistService.ListNew_Folder(isNewList, MusicList);
        }
        private void Exe_ListNew_Files(object isNewList)
        {
            MusicList = PlaylistService.ListNew_Files(isNewList, MusicList);
        }
        private void Exe_ListSaveAsPlaylist()
        {
            PlaylistService.ListSave(MusicList);
        }
        private void Exe_ListLoadPlaylist()
        {
            MusicList = PlaylistService.ListLoad(MusicList);
        }
        private void Exe_List_Clear()
        {
            MusicList = PlaylistService.ListClear(MusicList);
        }

        private void Exe_SortByTitle()
        {
            MusicList = PlaylistService.SortByTitle(MusicList);
        }
        private void Exe_SortByPath()
        {
            MusicList = PlaylistService.SortByPath(MusicList);
        }
        private void Exe_SortByName()
        {
            MusicList = PlaylistService.SortByName(MusicList);
        }
        private void Exe_SortByRandom()
        {
            MusicList = PlaylistService.SortByRandom(MusicList);
        }
        private void Exe_PlayRandomly()
        {
            PlayerService.isRandom = !PlayerService.isRandom;
            isRandom=isRandom;
            
        }

        void PlayerService_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(e.PropertyName);
        }

        private void Exe_ChangePosition()
        {
            PlayerService.Position=(RewindMusic); //.ChangePosition(RewindMusic);
        }
        private void SliderChangePosition()
        {
            while (true)
            {
                if (RewindMusic < Duration && PlayerService.isPlaying)
                {
                    RewindMusic++;
                    Thread.Sleep(1000);
                }
                else if (!PlayerService.isPlaying)
                {
                    RewindMusic = 0;
                    Thread.Sleep(1000);
                }
            }
        }
        public void Exe_Shortcuts(object _key)
        {
            try
            {
                var key = ((System.Windows.Input.KeyEventArgs)_key).Key;
                if (key == Configuration.KeyStop || key == Configuration.KeyStopB)
                    Exe_Stop();
                else if (key == Configuration.KeyPlay || key == Configuration.KeyPlayB)
                    Exe_Play();
                else if (key == Configuration.KeyNext || key == Configuration.KeyNextB)
                    Exe_Next();
                else if (key == Configuration.KeyPrev || key == Configuration.KeyPrevB)
                    Exe_Prev();
                else if (key == Configuration.KeyPause || key == Configuration.KeyPauseB)
                    Exe_Pause();
                else if (key == Configuration.KeyRewL || key == Configuration.KeyRewLB)
                {
                    RewindMusic -= 10;
                    Exe_ChangePosition();
                }
                else if (key == Configuration.KeyRewR || key == Configuration.KeyRewRB)
                {
                    RewindMusic += 10;
                    Exe_ChangePosition();
                }
                else if (key == Configuration.KeyVolU || key == Configuration.KeyVolUB)
                    Volume += 1;
                else if (key == Configuration.KeyVolD || key == Configuration.KeyVolDB)
                    Volume -= 1;
            }
            catch { }
        }
        #endregion
        
    }
}
//zrobic podsiwetlanie na liscie co jest w tej chwili odpalone
//korektor
// czas index
//wizualizacja 

//okładki
//korektor
//wizualizacja
//zmodyfikowac grafe
//potem karaoke dołączanie wideo
//lokalizacje zrób

//plik konfiguracyjny
