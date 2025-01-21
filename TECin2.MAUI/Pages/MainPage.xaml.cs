using TECin2.MAUI.Models;
using TECin2.MAUI.PageModels;

namespace TECin2.MAUI.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}