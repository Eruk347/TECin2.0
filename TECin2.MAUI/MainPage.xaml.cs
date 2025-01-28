using TECin2.MAUI.Pages;

namespace TECin2.MAUI
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Registration());
        }

        private void CPR_entry_Completed(object sender, EventArgs e)
        {
            //https://github.com/afriscic/BarcodeScanning.Native.Maui
            NameLabel.Text = CPR_entry.Text;
            CPR_entry.Text = "";
        }
    }
}
