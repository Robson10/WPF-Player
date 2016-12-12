namespace Player.Services
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Dynamic;
    using System.Linq;
    using System.Media;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using System.Windows.Input;
    using System.Windows.Media;

    using Player.Model;

    using Prism.Mvvm;
    using Prism.Commands;

    class PlaylistService
    {
        public PlaylistService() { }

        public MusicFilesAndPlaylists ListNew_Folder(object isNewList, MusicFilesAndPlaylists MusicList)
        {
            bool createNewList = bool.Parse(isNewList.ToString());
            string selectedPath = null;
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.ShowDialog();
                selectedPath = dialog.SelectedPath;
            }
            if (selectedPath != null && selectedPath != "")
            {
                if (createNewList == true)
                    MusicList.Clear();
                MusicList.AddMusicFromDirectory(selectedPath);
                MusicList.SavePlaylist(Configuration.DefaultPlaylistPath);
            }
            return MusicList;
        }
        
        public MusicFilesAndPlaylists ListNew_Files(object isNewList, MusicFilesAndPlaylists MusicList)
        {
            bool createNewList = bool.Parse(isNewList.ToString());

            List<string> selectedFiles = new List<string>();
            using (var dialog = new System.Windows.Forms.OpenFileDialog())
            {
                dialog.Filter = "Music (Music)|*.mp3;*.m4a;*.flac;*.wav";
                dialog.Multiselect = true;
                dialog.ShowDialog();
                selectedFiles.AddRange(dialog.FileNames);
                dialog.Dispose();
            }
            if (selectedFiles.Count > 0)
            {
                if (createNewList == true)
                    MusicList.Clear();
                MusicList.AddMusic(selectedFiles);
                MusicList.SavePlaylist(Configuration.DefaultPlaylistPath);
            }
            return MusicList;
        }
        
        public void ListSave(MusicFilesAndPlaylists MusicList)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.ShowDialog();
                saveFileDialog.Filter = "Playlist File (*.xml)|*.xml";
                MusicList.SavePlaylist(saveFileDialog.FileName);
                saveFileDialog.Dispose();
            }
        }
        
        public MusicFilesAndPlaylists ListLoad(MusicFilesAndPlaylists MusicList)
        {
            string path = null;
            using (var dialog = new System.Windows.Forms.OpenFileDialog())
            {
                dialog.Filter = "Playlist File (Xml)|*.xml";
                dialog.ShowDialog();
                if (dialog.FileName != "")
                    path = dialog.FileName;
                dialog.Dispose();
            }
            MusicList.LoadPlaylist(path);
            MusicList.SavePlaylist(Configuration.DefaultPlaylistPath);
            return MusicList;
        }
        
        public MusicFilesAndPlaylists ListClear(MusicFilesAndPlaylists MusicList)
        {
            MusicList.Clear();
            MusicList.SavePlaylist(Configuration.DefaultPlaylistPath);
            return MusicList;
        }

        #region SortBy
        public MusicFilesAndPlaylists SortByTitle(MusicFilesAndPlaylists MusicList)
        {
            var x = MusicList.OrderBy(s => s.Title).ToList();
            MusicList.Clear();
            MusicList.AddRange(x);
            MusicList.SavePlaylist(Configuration.DefaultPlaylistPath);
            return MusicList;
        }

        public MusicFilesAndPlaylists SortByPath(MusicFilesAndPlaylists MusicList)
        {
            var x = MusicList.OrderBy(s => s.Path).ToList();
            MusicList.Clear();
            MusicList.AddRange(x);
            MusicList.SavePlaylist(Configuration.DefaultPlaylistPath);
            return MusicList;
        }

        public MusicFilesAndPlaylists SortByName(MusicFilesAndPlaylists MusicList)
        {
            var x = MusicList.OrderBy(s => s.Name).ToList();
            MusicList.Clear();
            MusicList.AddRange(x);
            MusicList.SavePlaylist(Configuration.DefaultPlaylistPath);
            return MusicList;
        }

        public MusicFilesAndPlaylists SortByRandom(MusicFilesAndPlaylists MusicList)
        {
            var x = MusicList.OrderBy(a => Guid.NewGuid()).ToList();
            MusicList.Clear();
            MusicList.AddRange(x.ToList());
            MusicList.SavePlaylist(Configuration.DefaultPlaylistPath);
            return MusicList;
        }
        #endregion


    }
}
