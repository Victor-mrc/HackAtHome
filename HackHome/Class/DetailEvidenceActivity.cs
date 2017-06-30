using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CapaEntidades;
using AccesoServicio;
using Android.Webkit;
using Android.Graphics;

namespace HackHome.Class
{
    [Activity(Label = "@string/ApplicationName")]
    public class DetailEvidenceActivity : Activity
    {
        string Token;
        string FullName;
        string Url;

        TextView textViewNombre;
        TextView textViewTitulo;
        TextView textViewStatus;
        WebView textViewDescriptio;
        ImageView imageView;
        Evidence ev;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.DetailEvidence);
            ev = new Evidence();

            Token = Intent.GetStringExtra("Token") ?? "Data not available";
            FullName = Intent.GetStringExtra("FullName") ?? "Data not available";
            ev.EvidenceID = Intent.GetIntExtra("EvidenceID",0) ;
            ev.Title = Intent.GetStringExtra("Title") ?? "Data not available";
            ev.Status = Intent.GetStringExtra("Status") ?? "Data not available";

            textViewNombre = FindViewById<TextView>(Resource.Id.textViewNombre);
            textViewTitulo = FindViewById<TextView>(Resource.Id.textViewTitulo);
            textViewStatus = FindViewById<TextView>(Resource.Id.textViewStatus);
            textViewDescriptio = FindViewById<WebView>(Resource.Id.webView);
            imageView = FindViewById<ImageView>(Resource.Id.imageView1);
            textViewDescriptio.SetBackgroundColor(Color.Transparent);

            textViewNombre.Text = FullName;
            textViewTitulo.Text = ev.Title;
            textViewStatus.Text = ev.Status;

            ObtenEvidencia();
        }

        public async void ObtenEvidencia()
        {
          EvidenceDetail  ed = await new ServiceClient().GetEvidenceByIDAsync(Token, ev.EvidenceID);

            textViewDescriptio.LoadDataWithBaseURL(null, "<FONT COLOR=WHITE>" + ed.Description + " </font>", "text/html","utf-8",null);
            Url = ed.Url;
            Koush.UrlImageViewHelper.SetUrlDrawable(imageView,Url);
            
        }
    }
}