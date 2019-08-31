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
using Android.Views;

namespace PGRadio
{
    [Activity(Label = "", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, Android.Media.MediaPlayer.IOnPreparedListener
    {
        //Declares local variables
        Button play;
        Button stop;       
        WebView webview;
        System.Timers.Timer timer;

        //Declare public variable to be used by other classes
        public static TextView tv;
        public static ImageView iv;
        public static Bitmap imageBitmap;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            //Connects Toolbar as ActionBar
            Android.Support.V7.Widget.Toolbar toolbar = (Android.Support.V7.Widget.Toolbar) FindViewById(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);


            // Botton setup and click events.
            play = FindViewById<Button>(Resource.Id.play);
            this.play.Click += this.Play_Click;

            stop = FindViewById<Button>(Resource.Id.Stop);
            this.stop.Click += this.Stop_Click;


            //imageView and TextView reference
            iv = FindViewById<ImageView>(Resource.Id.imageView1);
            tv = FindViewById<TextView>(Resource.Id.textView1);

            //New WebView object for HTML scraping
            webview = new WebView(this);

            //Checks for savedinstace when switching orientation.
            if (savedInstanceState != null)
            {
                imageBitmap = (Bitmap) savedInstanceState.GetParcelable("Image");
                iv.SetImageBitmap(imageBitmap);
                tv.Text = savedInstanceState.GetString("TrackInfo");
            }



        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            //Sets new menu
            MenuInflater.Inflate(Resource.Menu.toolbarMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Intent intent;
            /* Handle item selection
             * intent.PutExtra passes value to new activity
             */
            switch (item.ItemId)
            {
                case Resource.Id.AboutUs:
                    intent = new Intent(this, typeof(Webview));
                    intent.PutExtra("URL", "https://www.purdueglobalradio.com/about-us/");
                    StartActivityForResult(intent, 1);
                    return true;

                case Resource.Id.LiveRadio:
                    intent = new Intent(this, typeof(MainActivity));
                    StartActivityForResult(intent, 1);
                    return true;

                case Resource.Id.ProgramSchedule:
                    intent = new Intent(this, typeof(Webview));
                    intent.PutExtra("URL", "https://www.purdueglobalradio.com/program-schedule/");
                    StartActivityForResult(intent, 1);
                    return true;


                case Resource.Id.Podcasts:

                    return true;


                case Resource.Id.Internships:
                    intent = new Intent(this, typeof(Webview));
                    intent.PutExtra("URL", "https://www.purdueglobalradio.com/internships/");
                    StartActivityForResult(intent, 1);
                    return true;


                case Resource.Id.MeetTheTeam:

                    intent = new Intent(this, typeof(Webview));
                    intent.PutExtra("URL", "https://www.purdueglobalradio.com/meet-the-team/");
                    StartActivityForResult(intent, 1);
                    return true;

                case Resource.Id.ContactUs:

                    intent = new Intent(this, typeof(Webview));
                    intent.PutExtra("URL", "https://www.purdueglobalradio.com/contact-us/");
                    StartActivityForResult(intent, 1);
                    return true;

                case Resource.Id.Close:
                    System.Environment.Exit(0);
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }



        private void Play_Click(object sender, EventArgs e)
        {
            //Checks if mediaPlayer is defined
            if(mp.mediaPlayer == null)
            {
                mp.mediaPlayer = new MediaPlayer();
            }

            //Check if player is active
            if (!mp.mediaPlayer.IsPlaying)
            {
                //Creates 10 second timer for scraping site for Image and Text
                timer = new System.Timers.Timer();
                timer.Interval = 10000;
                timer.Elapsed += OnTimedEvent;
                timer.Enabled = true;
                
                //Sets mediaPlayer source, prepared state and starts when ready
                mp.mediaPlayer.SetDataSource("https://stream.radio.co/sc61caeedd/listen");
                mp.mediaPlayer.SetOnPreparedListener(this);
                //PrepareAsync prepares on separate thread
                mp.mediaPlayer.PrepareAsync();

                //First HTML scrape when first starting player.
                setText();

            }            
        }

        private void OnTimedEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            RunOnUiThread(setText);
        }

        public void setText()
        {

            /*WebView naviagtion and JavaScript enabled
            WebView can only be used on the UI thread/ */
            WebView webview = new WebView(this);
            webview.Settings.JavaScriptEnabled = true;

            //Override event to check if page is fully loaded and parse.
            webview.SetWebViewClient(new HelloWebViewClient(this));
            webview.LoadUrl("https://www.purdueglobalradio.com/wp-content/uploads/2018/05/radio.html");

        }

        //Override event to save Image and Text
        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            outState.PutParcelable("Image", imageBitmap);
            outState.PutString("TrackInfo", tv.Text);
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            //Stop player and timer with clean up.
            try
            {
                mp.mediaPlayer.Stop();
                mp.mediaPlayer.Reset();
                timer.Stop();
                timer.Dispose();
            }
            catch { };
        }

        public void OnPrepared(MediaPlayer mp)
        {
            mp.Start();
        }

        //Class to parse site after fully loaded
        public class HelloWebViewClient : WebViewClient
        {
            
            Context ctx;

            public HelloWebViewClient(Context ctx)
            {
                //Set UI context
                this.ctx = ctx;
            }

            public override void OnPageFinished(WebView view, System.String url)
            {
                //Parse Site with JavascriptResult class (0 - Text, 1 - Image)
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
                //Item defines Image or Text
                this.Item = Item;
            }

            //Reads JSON and post back to ImageView or TextView
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
                                //GetImageBitmapFromURL converts Image to Bitmap for ImageView
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
                    //Checks if Image exists and converts
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