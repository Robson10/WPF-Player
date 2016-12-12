using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
namespace Player
{
    public class Configuration
    {
        public const string DefaultPlaylistPath = "TempPlaylist.xml";
        public const double DefaultVolume = 0.9;
        public const bool isRandom = false;
        public const Key KeyPlay = Key.NumPad5;
        public const Key KeyStop = Key.NumPad1;
        public const Key KeyNext = Key.NumPad6;
        public const Key KeyPrev = Key.NumPad4;
        public const Key KeyPause = Key.NumPad3;
        public const Key KeyRewR = Key.NumPad9;
        public const Key KeyRewL = Key.NumPad7;
        public const Key KeyVolU = Key.NumPad8;
        public const Key KeyVolD = Key.NumPad2;

        public const Key KeyPlayB = Key.Z;
        public const Key KeyStopB = Key.X;
        public const Key KeyNextB = Key.D;
        public const Key KeyPrevB = Key.A;
        public const Key KeyPauseB = Key.C;
        public const Key KeyRewRB = Key.E;
        public const Key KeyRewLB = Key.Q;
        public const Key KeyVolUB = Key.W;
        public const Key KeyVolDB = Key.D;

    }
}
