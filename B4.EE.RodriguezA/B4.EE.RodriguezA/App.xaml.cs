using B4.EE.RodriguezA.Domain.Services;
using B4.EE.RodriguezA.Domain.Services.FileIO;
using B4.EE.RodriguezA.ViewModels;
using FreshMvvm;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace B4.EE.RodriguezA
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTg4OTk5QDMxMzcyZTM0MmUzME13endTdWExSW05VnVMQWhHTW93VFlSYkVzaGpFN0xoaHU2UENxK2FFRnM9");

            //Register dependencies
            FreshIOC.Container.Register<ITopicRepository>(new JsonTopicRepository());

            MainPage = new FreshNavigationContainer(FreshPageModelResolver.ResolvePageModel<MainViewModel>());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
