using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Views;
using Java.Lang;
using Android.Media;
using System;
using System.IO;

namespace PGRadio
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Button play;
        Button stop;
        SeekBar progress;
        MediaPlayer mediaPlayer = new MediaPlayer();
        Thread thread1;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            play = FindViewById<Button>(Resource.Id.Play);
            this.play.Click += this.Play_Click;

            stop = FindViewById<Button>(Resource.Id.Stop);
            this.stop.Click += this.Stop_Click;

            progress = FindViewById<SeekBar>(Resource.Id.Progress);
            
        }


        private void Play_Click(object sender, EventArgs e)
        {
            if (!mediaPlayer.IsPlaying)
            {
                mediaPlayer = new MediaPlayer();
                mediaPlayer.SetDataSource("https://stream.radio.co/sc61caeedd/listen");
                mediaPlayer.Prepare();
                mediaPlayer.Start();      
            }
        }       

        private void Stop_Click(object sender, EventArgs e)
        {
            try
            {
                mediaPlayer.Stop();
            }
            catch { };            
        }
    }
}
