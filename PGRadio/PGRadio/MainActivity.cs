using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Java.Lang;
using Android.Media;
using System;
using Android.Webkit;
using Android.Content;

namespace PGRadio
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Button play;
        Button stop;
        SeekBar progress;
        public static TextView tv;
        public static ImageView iv;
        MediaPlayer mediaPlayer = new MediaPlayer();
        WebView webview;
        Thread fetchData;
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

            iv = FindViewById<ImageView>(Resource.Id.imageView1);
            tv = FindViewById<TextView>(Resource.Id.textView1);

            webview = new WebView(this);

            

        }



        private void Play_Click(object sender, EventArgs e)
        {
            if (!mediaPlayer.IsPlaying)
            {

                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Interval = 10000;
                timer.Elapsed += OnTimedEvent;
                timer.Enabled = true;


                mediaPlayer = new MediaPlayer();
                mediaPlayer.SetDataSource("https://stream.radio.co/sc61caeedd/listen");
                mediaPlayer.Prepare();
                mediaPlayer.Start();



                //WebView webview = new WebView(this);
                //webview.Settings.JavaScriptEnabled = true;
                //webview.SetWebViewClient(new HelloWebViewClient(this));

                //webview.LoadUrl("https://www.purdueglobalradio.com/wp-content/uploads/2018/05/radio.html");


                //fetchData = new Thread(RefeshInternetData);
                //fetchData.Start();

            }
        }

        private void OnTimedEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            RunOnUiThread(setText);
            //webview.LoadUrl("https://www.purdueglobalradio.com/wp-content/uploads/2018/05/radio.html");
        }

        private void setText()
        {

            WebView webview = new WebView(this);
            webview.Settings.JavaScriptEnabled = true;
            webview.SetWebViewClient(new HelloWebViewClient(this));

            webview.LoadUrl("https://www.purdueglobalradio.com/wp-content/uploads/2018/05/radio.html");
            //webview.LoadUrl("https://www.purdueglobalradio.com/wp-content/uploads/2018/05/radio.html");
        }


        private void Stop_Click(object sender, EventArgs e)
        {
            try
            {
                mediaPlayer.Stop();
                //fetchData.Dispose();
            }
            catch { };
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

                var jsr = new JavascriptResult();
                view.EvaluateJavascript("document.getElementsByClassName('radioco-nowPlaying')[0].innerText.toString()", jsr);
                //view.EvaluateJavascript("document.getElementsByClassName('radioco-image')[0].src.toString()", jsr);

            }



            public class JavascriptResult : Java.Lang.Object, Android.Webkit.IValueCallback
            {
                public JavascriptResult()
                {

                }


                public void OnReceiveValue(Java.Lang.Object result)
                {
                    string json = ((Java.Lang.String)result).ToString();

                    json = json.Trim('"');

                    if (json != "")
                    {
                        MainActivity.tv.Text = json.Trim('"');
                    }
                    //iv.Source(json);


                    //latch.CountDown();

                    //switch (item)
                    //{
                    //    case 0:
                    //        TextView tx = (TextView)App1.Resource.Id.textView1;
                    //        tx.Text = json;
                    //        return;

                    //}



                }
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
