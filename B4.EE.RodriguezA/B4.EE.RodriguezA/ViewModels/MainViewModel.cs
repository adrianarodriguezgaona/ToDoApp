using B4.EE.RodriguezA.Domain.Models;
using B4.EE.RodriguezA.Domain.Services;
using FreshMvvm;
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
                // este metodo ya esta en classe FreshBasePageModel

                RaisePropertyChanged(nameof(IsBusy));
            }
        }


        protected async override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);

            await RefreshReminderTopicLists();
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

        private async Task RefreshReminderTopicLists()
        {
            IsBusy = true;
            var reminderTopic = await topicService.GetAll();
            //bind IEnumerable<ReminderTopic> to the ListView's ItemSource
            ReminderTopics = null;    //Important! ensure the list is empty first to force refresh!
            ReminderTopics = new ObservableCollection<ReminderTopic>(reminderTopic.OrderBy(e => e.Name));
            IsBusy = false;
        }

    }
}
