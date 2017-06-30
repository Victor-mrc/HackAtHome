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
using CapaComponentes;

namespace HackHome.Class
{
    [Activity(Label = "@string/ApplicationName")]
    public class EvidencesActivity : Activity
    {

        ListaEvidencia lstEvidencias;
        TextView nombre;
        ListView lvEvidences;
        string Token;
        string FullName;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Evidences);

            Token = Intent.GetStringExtra("Token") ?? "Data not available";
            FullName = Intent.GetStringExtra("FullName") ?? "Data not available";
            nombre = FindViewById<TextView>(Resource.Id.textViewNombre);
            lvEvidences = FindViewById<ListView>(Resource.Id.listViewEvidences);

            nombre.Text = FullName;
            CargarEvidencias(Token);

            lvEvidences.ItemClick += LvEvidences_Click;
        }

        private void LvEvidences_Click(object sender, AdapterView.ItemClickEventArgs e)
        {

            
            
            Evidence ev = lstEvidencias.Lista.ElementAt<Evidence>(e.Position);
                    
            var intent = new Android.Content.Intent(this, typeof(DetailEvidenceActivity));
            intent.PutExtra("Token", Token);
            intent.PutExtra("FullName", FullName);
            intent.PutExtra("EvidenceID", ev.EvidenceID);
            intent.PutExtra("Title", ev.Title);
            intent.PutExtra("Status", ev.Status);

            StartActivity(intent);

        }

        private async void CargarEvidencias(string token)
        {
            lstEvidencias = (ListaEvidencia)this.FragmentManager.FindFragmentByTag("DatosResultado");
            if (lstEvidencias == null)
            {
                lstEvidencias = new ListaEvidencia();
                ServiceClient sc = new ServiceClient();
                lstEvidencias.Lista = await sc.GetEvidencesAsync(token);
                var FragmentTransation = this.FragmentManager.BeginTransaction();
                FragmentTransation.Add(lstEvidencias, "DatosResultado");
                FragmentTransation.Commit();
            }

            lvEvidences.Adapter = new EvidencesAdapter(
                this,
                lstEvidencias.Lista,
                Resource.Layout.layoutEvidence,
                Resource.Id.textViewNombreLab,
                Resource.Id.textViewStatus);

        }

        
    }
}