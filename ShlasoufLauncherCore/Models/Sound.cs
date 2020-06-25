using System;
using System.Collections.Generic;
using System.Text;

namespace ShlasoufLauncherCore.Models
{
    using System.IO;
    using System.Windows.Media;

    public class Sound
    {
        private MediaPlayer _mediaPlayer;
        private int count = 0;
        private bool _isPlaying;
        public Sound()
        {
            _mediaPlayer = new MediaPlayer();
            _mediaPlayer.MediaEnded += _mediaPlayer_MediaEnded;
        }

        public Sound(bool noRepeat)
        {
            _mediaPlayer = new MediaPlayer();
        }

        public MediaPlayer MediaPlayer => _mediaPlayer;

        private void _mediaPlayer_MediaEnded(object sender, EventArgs e)
        {
            count++;
            if (count == 4)
            {
                count = 0;
            }
            Play(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Assets\\" + "music" + count + ".mp3");
        }

        public void Play(string filename)
        {
            _mediaPlayer.Open(new Uri(filename));
            _mediaPlayer.Play();
            _isPlaying = true;
        }

        // `volume` is assumed to be between 0 and 100.
        public void SetVolume(int volume)
        {
            // MediaPlayer volume is a float value between 0 and 1.
            _mediaPlayer.Volume = volume / 100.0f;
        }

        public void Pause()
        {
            _mediaPlayer.Pause();
            _isPlaying = false;
        }

        public void Play()
        {
            _mediaPlayer.Play();
            _isPlaying = true;

        }

        public bool IsPlaying => _isPlaying;
    }
}
