using B4.EE.RodriguezA.Domain.Services;
using B4.EE.RodriguezA.Domain.Services.FileIO;
using B4.EE.RodriguezA.ViewModels;
using FreshMvvm;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Push;

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
          
            AppCenter.Start("270d236d-fd06-45df-a9e1-e78e9d6a3dbd", typeof(Push));
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
