using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using AccesoServicio;
using CapaEntidades;
using HackHome.Class;

namespace HackHome
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/icon",  Theme = "@android:style/Theme.Holo")]
    public class MainActivity : Activity
    {
        Button btnValidate;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            btnValidate = FindViewById<Button>(Resource.Id.buttonValidate);

            btnValidate.Click += BtnValidate_Click;

        }

        private async void BtnValidate_Click(object sender, System.EventArgs e)
        {
            ServiceClient sc = new ServiceClient();
            ResultInfo resulInfo = new ResultInfo();
            EditText email = FindViewById<EditText>(Resource.Id.editTextEmail);
            EditText password = FindViewById<EditText>(Resource.Id.editTextPassword);

            
            resulInfo = await sc.AutenticateAsync(email.Text, password.Text);

            if (resulInfo.Status == Status.Success)
            {
                var MicrosoftEvidence = new LabItem()
                {
                    DeviceId = Android.Provider.Settings.Secure.GetString(ContentResolver, Android.Provider.Settings.Secure.AndroidId),
                    Email = email.Text,
                    Lab = "Hack@Home"
                };
                var MicrosoftClient = new MicrosoftServiceClient();
                await MicrosoftClient.SendEvidence(MicrosoftEvidence);

                var intent = new Android.Content.Intent(this, typeof(EvidencesActivity));
                intent.PutExtra("Token", resulInfo.Token);
                intent.PutExtra("FullName", resulInfo.FullName);
                StartActivity(intent);

            }else
            {
                MuestraMensaje("No se pudo autenticar \n en TI Capacitación.");
            }
        }

        public void MuestraMensaje(string mensaje)
        {
            Android.App.AlertDialog.Builder Builder = new AlertDialog.Builder(this);
            AlertDialog Alert = Builder.Create();
            Alert.SetTitle("Autenticación en TI Capacitación");
            Alert.SetIcon(Resource.Drawable.Icon);
            Alert.SetMessage(mensaje);
            Alert.SetButton("Ok", (s, ev) => { });
            Alert.Show();
        }
    }
}

