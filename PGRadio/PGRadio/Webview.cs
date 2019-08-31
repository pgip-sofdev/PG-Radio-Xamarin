using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Webkit;
using Android.Content;
using Android.Views;
using Android.Content.PM;

namespace PGRadio
{
    //Configuration prevents the activity from being restarted on orientation change
    [Activity(Label = "", MainLauncher = false, ConfigurationChanges = ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class Webview : AppCompatActivity
    {
        private WebView webview; 
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.WebViews);

            //Connects Toolbar as ActionBar
            Android.Support.V7.Widget.Toolbar toolbar = (Android.Support.V7.Widget.Toolbar)FindViewById(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            webview = (WebView)FindViewById(Resource.Id.webView1);
            //Builds WebView and set invisible until load complete
            webview.Visibility = ViewStates.Invisible;
            webview.Settings.JavaScriptEnabled = true;
            webview.SetWebViewClient(new HelloWebViewClient(this));
            webview.LoadUrl(Intent.GetStringExtra("URL"));
            //webview.LoadUrl("https://www.purdueglobalradio.com/program-schedule/");
            //webview.LoadUrl("https://www.purdueglobalradio.com/internships/");
            //webview.LoadUrl("https://www.purdueglobalradio.com/meet-the-team/");
            //webview.LoadUrl("https://www.purdueglobalradio.com/contact-us/");
            //webview.LoadUrl("https://www.purdueglobalradio.com/");


        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {

            //Added Menu to Toolbar
            MenuInflater.Inflate(Resource.Menu.toolbarMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Intent intent;
            /* Handle item selection
             * intent.PutExtra passes value to new Activity
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

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }        

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

                //Removes unwanted itmes and sets WebView as visible
                view.LoadUrl("javascript:(function() { " +
                                "document.getElementsByClassName('page-header-block')[0].style.display='none'; })()");
                view.LoadUrl("javascript:(function() { " +
                                "document.getElementsByClassName('radiocontainer')[0].style.display='none'; })()");

                view.Visibility = ViewStates.Visible;

            }

        }






    }
}