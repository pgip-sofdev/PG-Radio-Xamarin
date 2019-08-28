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

    //Public mediaPlayer for service so orientation switch doesn't restart service. 
    public static class mp
    {
        static public MediaPlayer mediaPlayer;
    }
}