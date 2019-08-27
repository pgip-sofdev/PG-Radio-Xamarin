using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Java.Lang;
using Android.Media;
using System;
using Android.Webkit;
using Android.Content;
using Android.Graphics;
using System.Net;

namespace PGRadio
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, MediaPlayer.IOnPreparedListener
    {
        Button play;
        Button stop;
        public static TextView tv;
        public static ImageView iv;
        WebView webview;
        System.Timers.Timer timer;
        public static Bitmap imageBitmap;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main); 

            play = FindViewById<Button>(Resource.Id.play);
            this.play.Click += this.Play_Click;

            stop = FindViewById<Button>(Resource.Id.Stop);
            this.stop.Click += this.Stop_Click;



            iv = FindViewById<ImageView>(Resource.Id.imageView1);
            tv = FindViewById<TextView>(Resource.Id.textView1);

            webview = new WebView(this);

            if (savedInstanceState != null)
            {
                imageBitmap = (Bitmap) savedInstanceState.GetParcelable("Image");
                iv.SetImageBitmap(imageBitmap);
                tv.Text = savedInstanceState.GetString("TrackInfo");
            }



        }



        private void Play_Click(object sender, EventArgs e)
        {
            mp.CheckMP(true);
            if (!mp.CheckPlay())
            {

                timer = new System.Timers.Timer();
                timer.Interval = 10000;
                timer.Elapsed += OnTimedEvent;
                timer.Enabled = true;
                                
                mp.mediaPlayer.SetDataSource("https://stream.radio.co/sc61caeedd/listen");
                mp.mediaPlayer.SetOnPreparedListener(this);
                mp.mediaPlayer.PrepareAsync();
                mp.mediaPlayer.Start();

                setText();

            }
            else
            {
                timer = new System.Timers.Timer();
                timer.Interval = 10000;
                timer.Elapsed += OnTimedEvent;
                timer.Enabled = true;
                setText();
            }
        }

        private void OnTimedEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            RunOnUiThread(setText);
        }

        public void setText()
        {

            WebView webview = new WebView(this);
            webview.Settings.JavaScriptEnabled = true;

            webview.SetWebViewClient(new HelloWebViewClient(this));
            webview.LoadUrl("https://www.purdueglobalradio.com/wp-content/uploads/2018/05/radio.html");

        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            outState.PutParcelable("Image", imageBitmap);
            outState.PutString("TrackInfo", tv.Text);
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            try
            {
                mp.StopPlay();
                timer.Stop();
                timer.Dispose();
            }
            catch { };
        }

        public void OnPrepared(MediaPlayer mp)
        {
            mp.Start();
        }

        public class HelloWebViewClient : WebViewClient
        {
            Context ctx;

            public HelloWebViewClient(Context ctx)
            {
                this.ctx = ctx;
            }

            public override void OnPageFinished(WebView view, System.String url)
            {

                var jsr = new JavascriptResult(0);
                view.EvaluateJavascript("document.getElementsByClassName('radioco-nowPlaying')[0].innerText.toString()", jsr);

                jsr = new JavascriptResult(1);
                view.EvaluateJavascript("document.getElementsByClassName('radioco-image')[0].src.toString()", jsr);

            }

        }



        public class JavascriptResult : Java.Lang.Object, Android.Webkit.IValueCallback
        {
            int Item;
            public JavascriptResult(int Item)
            {
                this.Item = Item;
            }


            public void OnReceiveValue(Java.Lang.Object result)
            {
                string json = ((Java.Lang.String)result).ToString();

                json = json.Trim('"');

                if (json != "")
                {
                    switch (Item)
                    {
                        case 0:
                            MainActivity.tv.Text = json.Replace("\\", "");
                            break;
                        case 1:
                            if (json.Contains("https://"))
                            {
                                MainActivity.imageBitmap = GetImageBitmapFromUrl(json);
                                MainActivity.iv.SetImageBitmap(imageBitmap);
                            }
                            break;

                    }
                }

            }

            public Bitmap GetImageBitmapFromUrl(string url)
            {
                Bitmap imageBitmap = null;
                

                using (var webClient = new WebClient())
                {
                    var imageBytes = webClient.DownloadData(url);
                    if (imageBytes != null && imageBytes.Length > 0)
                    {
                        imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                    }
                }

                return imageBitmap;
            }

        }
    }        
    
}




    class WebViewClientClass : WebViewClient
    {
        public override void OnPageFinished(WebView view, string url)
        {
            Toast.MakeText(Application.Context, "this", ToastLength.Long).Show();
        }

    }
