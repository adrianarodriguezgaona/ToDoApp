using B4.EE.RodriguezA.Domain.Models;
using B4.EE.RodriguezA.Validators;
using FluentValidation;
using FreshMvvm;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace B4.EE.RodriguezA.ViewModels
{
    public class ReminderTopicItemViewModel : FreshBasePageModel
    {
        private TopicItem _topicItem;
        private IValidator _topicItemValidator;

        public ReminderTopicItemViewModel()
        {
            _topicItemValidator = new TopicItemValidator();
        }

        #region Properties


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

        private string itemName;
        public string ItemName
        {
            get { return itemName; }
            set
            {
                itemName = value;
                RaisePropertyChanged(nameof(ItemName));
            }
        }

        private string itemNameError;
        public string ItemNameError
        {
            get { return itemNameError; }
            set
            {
                itemNameError = value;
                RaisePropertyChanged(nameof(ItemNameError));
                RaisePropertyChanged(nameof(ItemNameErrorVisible));
            }
        }

        public bool ItemNameErrorVisible
        {
            get { return !string.IsNullOrWhiteSpace(ItemNameError); }
        }


        private DateTime toDoDate;
        public DateTime ToDoDate
        {
            get { return toDoDate; }
            set
            {
                toDoDate = value;
                RaisePropertyChanged(nameof(ToDoDate));
            }
        }

        private string myTopicItem;
        public string MyTopicItem
        {
            get { return myTopicItem; }
            set { myTopicItem = value; RaisePropertyChanged(nameof(MyTopicItem)); }
        }

        private string photoSource;
        public string PhotoSource
        {
            get { return photoSource; }
            set { photoSource = value; RaisePropertyChanged(nameof(PhotoSource)); }
        }

        #endregion

        public override void Init(object initData)
        {
            TopicItem item = initData as TopicItem;
            _topicItem = item;
            if (item.Id == Guid.Empty)
            {
                PageTitle = "Nieuwe Item";
            }
            else
            {
                PageTitle = "Edit Item";
            }

            LoadItemState();
            base.Init(initData);
        }

        private void LoadItemState()
        {
            ItemName = _topicItem.ItemName;
            ToDoDate = _topicItem.ToDoDate ?? DateTime.Now;
            PhotoSource = _topicItem.PhotoSource;
            MyTopicItem = _topicItem.MyTopicItem;
        }
     

        public ICommand TakeAPictureCommand => new Command(
            async () =>

            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await Application.Current.MainPage.DisplayAlert("No Camera", ":( No camera available.", "OK");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "Sample",
                    Name = $"{ItemName}.jpg",
                    PhotoSize = PhotoSize.Small
                });

                if (file == null)
                    return;

                await Application.Current.MainPage.DisplayAlert("File Location", file.Path, "OK");

                PhotoSource = file.Path;

                var source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });


            });


        private void SaveItemState()
        {
            _topicItem.ItemName = ItemName;
            _topicItem.ToDoDate = new DateTime?(ToDoDate);
            _topicItem.MyTopicItem = MyTopicItem;
            _topicItem.PhotoSource = PhotoSource;
        }

        public ICommand SaveTopicItemCommand => new Command(
            async () => {
                try
                {
                    SaveItemState();
                    if (Validate(_topicItem))
                    {

                        if (_topicItem.Id == Guid.Empty)
                        {
                            _topicItem.Id = Guid.NewGuid();
                            _topicItem.ParentTopic.Items.Add(_topicItem);
                        }
                        //use coremethodes to Pop pages in FreshMvvm!
                        await CoreMethods.PopPageModel(_topicItem, false, true);
                    }

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    throw;
                }
            }
        );
       
        private bool Validate(TopicItem item)
        {
            var validationResult = _topicItemValidator.Validate(item);
            //loop through error to identify properties
            foreach (var error in validationResult.Errors)
            {
                if (error.PropertyName == nameof(item.ItemName))
                {
                    ItemNameError = error.ErrorMessage;
                }
            }
            return validationResult.IsValid;
        }



    }
}
