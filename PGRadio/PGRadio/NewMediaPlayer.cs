using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace PGRadio
{  
    public static class mp
    {
        static public MediaPlayer mediaPlayer;
        public static bool CheckMP(bool Create)
        {
            if(mediaPlayer == null)
            {
                if (Create)
                {
                    mediaPlayer = new MediaPlayer();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }       

        public static void StopPlay()
        {
            if (CheckMP(false))
            {
                mediaPlayer.Stop();
                mediaPlayer.Dispose();
            }
        }

        public static bool CheckPlay()
        {
            if (CheckMP(false))
            {
                return mediaPlayer.IsPlaying;
            }
            else
            {
                return false;
            }

        }

        
    }
}