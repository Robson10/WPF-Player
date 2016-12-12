namespace Player
{
    using System.IO;
    using System.Windows;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Player.Model;
    using System.Xml.Serialization;
    using System.Collections.ObjectModel;
    using System.Windows.Media;
    using System.Windows.Forms;
    public class MusicFilesAndPlaylists : ObservableCollection<FileMusicParameters>
    {
        public MusicFilesAndPlaylists()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (System.IO.File.Exists(Configuration.DefaultPlaylistPath))
            {
                LoadPlaylist(Configuration.DefaultPlaylistPath);
            }
        }

        public void LoadPlaylist(string name)
        {
            this.Clear();
            XmlSerializer mySerializer = new XmlSerializer(typeof(ObservableCollection<FileMusicParameters>));
            FileStream myFileStream = new FileStream(name, FileMode.Open);
            var data = (ObservableCollection<FileMusicParameters>)mySerializer.Deserialize(myFileStream);
            this.AddRange(data);
            myFileStream.Close();
        }

        public void AddMusicFromDirectory(string path)
        {
            List<string> fileList = new List<string>();
            Regex searchPattern = new Regex(@"$(?<=\.(mp3|m4a|flac|wav))", RegexOptions.IgnoreCase);
            fileList = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).Where((f) => searchPattern.IsMatch(f)).ToList();
            AddMusic(fileList);
        }

        public void AddMusic(List<string> fileList)
        {
            
            for (int i = 0; i < fileList.Count; i++)
            {
                var file = TagLib.File.Create(fileList[i]);
                var title = file.Tag.Title;
                var path = file.Name;
                StringBuilder name = new StringBuilder(path.Substring(path.LastIndexOf("\\") + 1));
                name = name.Remove(name.ToString().LastIndexOf("."), name.Length - name.ToString().LastIndexOf("."));
                var bitrate = file.Properties.AudioBitrate;
                int time = (int)file.Properties.Duration.TotalSeconds;

                this.Add(new FileMusicParameters { Path = fileList[i], Title = title, Name = name.ToString(), Time = time, IsPlaying=false});
            }
        }        

        public void SavePlaylist(string name)//after any action on list
        {
            XmlSerializer ser = new XmlSerializer(typeof(ObservableCollection<FileMusicParameters>));
            TextWriter writer = new StreamWriter(name);
            ser.Serialize(writer, this);
            writer.Close();
        }
    }
}
