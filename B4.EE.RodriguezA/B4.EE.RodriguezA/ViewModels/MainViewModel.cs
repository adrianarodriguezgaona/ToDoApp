using B4.EE.RodriguezA.Domain.Models;
using B4.EE.RodriguezA.Domain.Services;
using FreshMvvm;
using Plugin.Toasts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace B4.EE.RodriguezA.ViewModels
{
    public class MainViewModel : FreshBasePageModel
    {
        private readonly ITopicRepository _topicRepository;
        private readonly ITopicService topicService;
        int counter = 0;
        public MainViewModel(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
            topicService = new TopicService(_topicRepository);

        }

        private ObservableCollection<ReminderTopic> reminderTopics;
        public ObservableCollection<ReminderTopic> ReminderTopics
        {
            get { return reminderTopics; }
            set { reminderTopics = value; RaisePropertyChanged(nameof(ReminderTopics)); }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
     
                RaisePropertyChanged(nameof(IsBusy));
            }
        }


        private string headerForList;
        public string HeaderForList
        {
            get { return headerForList; }
            set { headerForList = value; RaisePropertyChanged(nameof(HeaderForList)); }
        }

        protected async override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);           
            await RefreshReminderTopicLists();
            await WelkomToast();
        }


        public ICommand OpenReminderTopicPageCommand => new Command<ReminderTopic>(
            async (ReminderTopic topic) => {
                await CoreMethods.PushPageModel<ReminderTopicViewModel>(topic, false, true);
            }
        );

        public ICommand DeleteReminderTopicCommand => new Command<ReminderTopic>(
            async (ReminderTopic topic) => {
                await topicService.Delete(topic.Id);
                await RefreshReminderTopicLists();
            }
        );

        private async Task WelkomToast()
        {
            counter++;
            if (counter == 1 && ReminderTopics.Count > 0)
            {
                var message = "Welkom Terug";
                ShowToast(message);
            }
        }
        private async Task RefreshReminderTopicLists()
        {
            
            IsBusy = true;
            var reminderTopic = await topicService.GetAll();
            //bind IEnumerable<ReminderTopic> to the ListView's ItemSource
            ReminderTopics = null;    //Important! ensure the list is empty first to force refresh!
            ReminderTopics = new ObservableCollection<ReminderTopic>(reminderTopic.OrderBy(e => e.Name));
           
            if (ReminderTopics.Count == 0)
            {
                var message = "";
                HeaderForList = "Nog geen To Do lijst!";
                message = "Voeg jouw eerste To Do Lijst nu!!";
                ShowToast(message);
            }
            else
            {
                HeaderForList = "Mijn To Do lijst:";
               
            }
            IsBusy = false;
        }

        private async void ShowToast(string message)
        {
            //see https://github.com/EgorBo/Toasts.Forms.Plugin for usage
            //of this Toasts plugin
            var notificator = DependencyService.Get<IToastNotificator>();

            var options = new NotificationOptions()
            {
                Title = "To Do App",
                Description = message,
                IsClickable = true,
                WindowsOptions = new WindowsOptions() { LogoUri = "icon.png" },
                ClearFromHistory = false,
                AllowTapInNotificationCenter = false,
                AndroidOptions = new AndroidOptions()
                {
                    HexColor = "#F99D1C",
                    ForceOpenAppOnNotificationTap = true
                }
            };

            await notificator.Notify(options);

        }

    }
}
