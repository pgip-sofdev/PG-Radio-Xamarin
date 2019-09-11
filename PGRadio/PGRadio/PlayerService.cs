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
    [Service]
    //Public mediaPlayer for service so orientation switch doesn't restart service. 
    public class MediaPlayerService : Service, MediaPlayer.IOnPreparedListener
    {
        public MediaPlayer player;

        public override IBinder OnBind(Intent arg0)
        {
            return null;
        }        

        public void OnPrepared(MediaPlayer mp)
        {

            mp.Start();
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            player = new MediaPlayer();
            //player = MediaPlayer.Create(this, Resource.Raw.Test);
            player.SetDataSource("https://stream.radio.co/sc61caeedd/listen");
            player.SetOnPreparedListener(this);
            player.PrepareAsync();

            return StartCommandResult.Sticky;
        }
        
        public override void OnDestroy()
        {
            player.Stop();
            player.Release();
        }




    }
}