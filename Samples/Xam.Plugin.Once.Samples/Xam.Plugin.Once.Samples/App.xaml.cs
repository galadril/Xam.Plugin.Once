using System;
using Xam.Plugin.Once.Samples;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Xam.Plugin.Once
{
   /// <summary>
   /// Sample app for Once
   /// </summary>
   public partial class App : Application
   {
      /// <summary>
      /// Default constructor
      /// </summary>
      public App()
      {
         InitializeComponent();

         MainPage = new NavigationPage(new MainPage());
      }
   }
}
