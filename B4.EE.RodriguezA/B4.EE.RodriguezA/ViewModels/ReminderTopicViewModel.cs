using B4.EE.RodriguezA.Constants;
using B4.EE.RodriguezA.Domain.Models;
using B4.EE.RodriguezA.Domain.Services;
using FluentValidation;
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
    public class ReminderTopicViewModel : FreshBasePageModel
    {
        private readonly ITopicRepository _topicRepository;
        private readonly ITopicService topicService;
        private ReminderTopic _currentReminderTopic;
        private bool isNew = true;


        public ReminderTopicViewModel(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
            topicService = new TopicService(_topicRepository);

        }
        private string pageTitle;
        public string PageTitle
        {
            get { return pageTitle; }
            set
            {
                pageTitle = value;
                RaisePropertyChanged(nameof(pageTitle));
            }
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

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; RaisePropertyChanged(nameof(Name)); }
        }

        private string reminderTopicNameError;
        public string ReminderTopicNameError
        {
            get { return reminderTopicNameError; }
            set
            {
                reminderTopicNameError = value;
                RaisePropertyChanged(nameof(ReminderTopicNameError));
                RaisePropertyChanged(nameof(ReminderTopicNameErrorVisible));
            }
        }

        public bool ReminderTopicNameErrorVisible
        {
            get { return !string.IsNullOrWhiteSpace(ReminderTopicNameError); }
        }


      
        private ObservableCollection<TopicItem> topicItems;
        public ObservableCollection<TopicItem> TopicItems
        {
            get { return topicItems; }
            set { topicItems = value; RaisePropertyChanged(nameof(TopicItems)); }
        }

      

        /// Called whenever the page is navigated to.
        /// </summary>
        /// <param name="initData"></param>
        public async override void Init(object initData)
        {
            base.Init(initData);

            _currentReminderTopic = initData as ReminderTopic;


            await RefreshLocationUser();
        }

        /// <summary>
        /// Called when returning to this Model from a previous model
        /// </summary>
        /// <param name="returnedData"></param>
        public override void ReverseInit(object returnedData)
        {
            base.ReverseInit(returnedData);
            if (returnedData is TopicItem)
            {
                //refresh list, to update this item visually
                LoadReminderTopicState();
            }
        }

        /// <summary>
        /// Refreshes the currentlocationuser (to edit) or initializes a new one (to add)
        /// </summary>
        /// <returns></returns>
        private async Task RefreshLocationUser()
        {
            if (_currentReminderTopic != null)
            {
                //editing existing ReminderTopicList
                isNew = false;
                PageTitle = "Update herinneringen lijst";
                _currentReminderTopic = await topicService.GetById(_currentReminderTopic.Id);
            }
            else
            {
                //editing brand new locationlist
                isNew = true;
                PageTitle = "Nieuwe herinneringen lijst";
                _currentReminderTopic = new ReminderTopic();
                _currentReminderTopic.Id = Guid.NewGuid();
                _currentReminderTopic.Items = new List<TopicItem>();
            }
            LoadReminderTopicState();
        }


        public ICommand SaveReminderTopicCommand => new Command(
            async () => {
                SaveReminderTopicState();
                
                IsBusy = true;
                try
                {
                    await topicService.Save(_currentReminderTopic);
                    MessagingCenter.Send(this, MessageNames.ReminderTopicSaved, _currentReminderTopic);

                    await CoreMethods.PopPageModel(false, true);
                }               
             
                catch (ValidationException valEx)
                {
                    foreach (var error in valEx.Errors)
                    {
                        if (error.PropertyName == nameof(_currentReminderTopic.Name))
                        {
                            ReminderTopicNameError = error.ErrorMessage;
                        }
                    }
                }
                //catch (Exception ex)
                //{
                //    await DisplayAlert("Error while saving", ex.Message, "Ok");
                //}

                IsBusy = false;


            }
        );

        public ICommand OpenItemPageCommand => new Command<TopicItem>(
            async (TopicItem item) => {

                SaveReminderTopicState();

                if (item == null)
                {
                    //new locationuserList Item requested, let's make sure to
                    //pass a reference to the parent ReminderTopic to which the new item will belong
                    item = new TopicItem
                    {
                        ReminderTopicId = _currentReminderTopic.Id,
                        ParentTopic = _currentReminderTopic
                    };
                }
                await CoreMethods.PushPageModel<ReminderTopicItemViewModel>(item, false, true);
            }
        );


        public ICommand DeleteItemCommand => new Command<TopicItem>(
            (TopicItem item) => {
                _currentReminderTopic.Items.Remove(item);
                LoadReminderTopicState();
            }
        );


        /// <summary>
        /// Loads the ReminderTopic list properties into the VM properties for display in UI
        /// </summary>
        private void LoadReminderTopicState()
        {
            Name = _currentReminderTopic.Name;
            TopicItems = new ObservableCollection<TopicItem>(_currentReminderTopic.Items.OrderBy(e => e.ItemName));
        }


        private void SaveReminderTopicState()
        {
            _currentReminderTopic.Name = Name;
           
        }

        //private bool Validate(LocationUser location)
        //{
        //    var validationResult = _locationUserValidator.Validate(location);
        //    //loop through error to identify properties
        //    foreach (var error in validationResult.Errors)
        //    {
        //        if (error.PropertyName == nameof(location.Name))
        //        {
        //            ReminderTopicNameError = error.ErrorMessage;
        //        }
        //    }
        //    return validationResult.IsValid;
        //}


    }

   
}
