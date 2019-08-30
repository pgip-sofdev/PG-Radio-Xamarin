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
    [Activity(Label = "", MainLauncher = true)]
    public class Webview : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.WebViews);


            Android.Support.V7.Widget.Toolbar toolbar = (Android.Support.V7.Widget.Toolbar)FindViewById(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            WebView webview = (WebView)FindViewById(Resource.Id.webView1);

            webview.Visibility = ViewStates.Invisible;
            webview.Settings.JavaScriptEnabled = true;
            webview.SetWebViewClient(new HelloWebViewClient(this));
            //webview.LoadUrl("https://www.purdueglobalradio.com/about-us/");
            //webview.LoadUrl("https://www.purdueglobalradio.com/program-schedule/");
            //webview.LoadUrl("https://www.purdueglobalradio.com/internships/");
            //webview.LoadUrl("https://www.purdueglobalradio.com/meet-the-team/");
            //webview.LoadUrl("https://www.purdueglobalradio.com/contact-us/");
            webview.LoadUrl("https://www.purdueglobalradio.com/");


        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.toolbarMenu, menu);
            return base.OnCreateOptionsMenu(menu);
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

                view.LoadUrl("javascript:(function() { " +
                                "document.getElementsByClassName('page-header-block')[0].style.display='none'; })()");
                view.LoadUrl("javascript:(function() { " +
                                "document.getElementsByClassName('radiocontainer')[0].style.display='none'; })()");

                view.Visibility = ViewStates.Visible;

            }

        }






    }
}