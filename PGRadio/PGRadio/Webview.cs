using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Webkit;
using Android.Content;
using Android.Views;
using Android.Content.PM;
using System;
using Android.Widget;
using Android.Net;
using Java.Interop;

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

            //handler to stop mediaplayer when navigating

            try
            {
                Intent myService = new Intent(this, typeof(MediaPlayerService));
                StopService(myService);
            }
            catch { }

            //if (mp.mediaPlayer.IsPlaying)
            //{
            //    mp.mediaPlayer.Stop();
            //    mp.mediaPlayer.Reset();
            //}




            webview = (WebView)FindViewById(Resource.Id.webView1);
            //Builds WebView and set invisible until load complete
            webview.Visibility = ViewStates.Invisible;
            webview.Settings.SetSupportZoom(true);
            webview.Settings.BuiltInZoomControls = true;
            webview.Settings.JavaScriptEnabled = true;
            webview.Settings.DomStorageEnabled = true;
            webview.SetWebViewClient(new HelloWebViewClient(this));
            webview.SetWebChromeClient(new WebChromeClient());  
            webview.LoadUrl(Intent.GetStringExtra("URL"));
            webview.RequestFocus();
            //webview.LoadUrl("https://www.purdueglobalradio.com/program-schedule/");
            //webview.LoadUrl("https://www.purdueglobalradio.com/internships/");
            //webview.LoadUrl("https://www.purdueglobalradio.com/meet-the-team/");
            //webview.LoadUrl("https://www.purdueglobalradio.com/contact-us/");
            //webview.LoadUrl("https://www.purdueglobalradio.com/");


        }

        public static String changedHeaderHtml(String htmlText)
        {

            String head = "<head><meta name=\"viewport\" content=\"width=device-width, user-scalable=yes\" /></head>";

            String closedTag = "</body></html>";
            String changeFontHtml = head + htmlText + closedTag;
            return changeFontHtml;
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
                    this.StartActivity(intent);
                    Finish();
                    //StartActivityForResult(intent, 1);
                    return true;

                case Resource.Id.LiveRadio:
                    intent = new Intent(this, typeof(MainActivity));
                    this.StartActivity(intent);
                    Finish();
                    //StartActivityForResult(intent, 1);
                    return true;

                case Resource.Id.ProgramSchedule:
                    intent = new Intent(this, typeof(Webview));
                    intent.PutExtra("URL", "https://www.purdueglobalradio.com/program-schedule/");
                    this.StartActivity(intent);
                    Finish();
                    //StartActivityForResult(intent, 1);
                    return true;


                case Resource.Id.Podcasts:
                    intent = new Intent(this, typeof(Webview));
                    intent.PutExtra("URL", "https://www.purdueglobalradio.com/podcasts/");
                    this.StartActivity(intent);
                    Finish();
                    return true;


                case Resource.Id.Internships:
                    intent = new Intent(this, typeof(Webview));
                    intent.PutExtra("URL", "https://www.purdueglobalradio.com/internships/");
                    this.StartActivity(intent);
                    Finish();
                    //StartActivityForResult(intent, 1);
                    return true;


                case Resource.Id.MeetTheTeam:

                    intent = new Intent(this, typeof(Webview));
                    intent.PutExtra("URL", "https://www.purdueglobalradio.com/meet-the-team/");
                    this.StartActivity(intent);
                    Finish();
                    //StartActivityForResult(intent, 1);
                    return true;

                case Resource.Id.ContactUs:

                    intent = new Intent(this, typeof(Webview));
                    intent.PutExtra("URL", "https://www.purdueglobalradio.com/contact-us/");
                    this.StartActivity(intent);
                    Finish();
                    //StartActivityForResult(intent, 1);
                    return true;

                case Resource.Id.Close:
                    System.Environment.Exit(0);
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override void OnBackPressed()
        {
            //back button for returning to last page
            if (webview.CanGoBack())
            {
                webview.GoBack();
                webview.Visibility = ViewStates.Invisible;
            }
            else
            {
                base.OnBackPressed();
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

                var jsr = new JavascriptResult(ctx);

                //Removes unwanted itmes and sets WebView as visible
                view.LoadUrl("javascript:(function() { " +
                                "document.getElementsByClassName('page-header-block')[0].style.display='none'; })()");
                view.LoadUrl("javascript:(function() { " +
                                "document.getElementsByClassName('radiocontainer')[0].style.display='none'; })()");
                view.LoadUrl("javascript:(function() { " +
                                "document.getElementsByClassName('main-nav main-navigation')[0].style.display='none'; })()");
               

                view.Visibility = ViewStates.Visible;


            }


            public override bool ShouldOverrideUrlLoading(WebView view, IWebResourceRequest request)
            {

                string URL = request.Url.ToString();

                try
                {

                    Toast.MakeText(ctx, request.Url.ToString(), ToastLength.Long).Show();

                    //redirects Email to installed app
                    if (request.Url.ToString().StartsWith("mailto:"))
                    {

                        Intent emailIntent = new Intent(Intent.ActionSendto);
                        emailIntent.SetData(Android.Net.Uri.Parse(URL));

                        try
                        {
                            ctx.StartActivity(Intent.CreateChooser(emailIntent, "Send email using..."));
                        }
                        catch (Android.Content.ActivityNotFoundException ex)
                        {
                            Toast.MakeText(ctx, "No email clients installed.", ToastLength.Long).Show();
                        }
                        return true;
                    }

                    else
                    {
                        view.Visibility = ViewStates.Invisible;
                        view.LoadUrl(request.Url.ToString());
                    }

                    return true;
                }
                catch (Exception e)
                {
                    Toast.MakeText(ctx, e.Message, ToastLength.Long).Show();
                    return true;
                }

            }


        }

        public class JavascriptResult : Java.Lang.Object, Android.Webkit.IValueCallback
        {
            Context ctx;
            public JavascriptResult(Context ctx)
            {
                //Item defines Image or Text
                this.ctx = ctx;
            }

            //Reads JSON and post back to ImageView or TextView
            public void OnReceiveValue(Java.Lang.Object result)
            {
                string json = ((Java.Lang.String)result).ToString();

                Toast.MakeText(ctx, json, ToastLength.Long).Show();

            }

        }        
    }
}
