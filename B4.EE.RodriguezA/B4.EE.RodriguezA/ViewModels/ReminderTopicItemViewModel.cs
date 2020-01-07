using B4.EE.RodriguezA.Domain.Models;
using B4.EE.RodriguezA.Domain.Services;
using B4.EE.RodriguezA.Validators;
using FluentValidation;
using FreshMvvm;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace B4.EE.RodriguezA.ViewModels
{
    public class ReminderTopicItemViewModel : FreshBasePageModel
    {
        private TopicItem _topicItem;
        private IValidator _topicItemValidator;
        private string alarmId;
        

        public ReminderTopicItemViewModel()
        {
            _topicItemValidator = new TopicItemValidator();

        }

        #region Properties

        private MediaFile file;


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


        private string toDoDateString;
        public string ToDoDateString
        {
            get { return toDoDateString; }
            set { toDoDateString = value; RaisePropertyChanged(nameof(ToDoDateString)); }
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


        private string pathImageFromDevice;
        public string PathImageFromDevice
        {
            get { return pathImageFromDevice; }
            set { pathImageFromDevice = value; RaisePropertyChanged(nameof(PathImageFromDevice)); }
        }

        
        private ImageSource imageFromDeviceSource;
        public ImageSource ImageFromDeviceSource
        {
            get { return imageFromDeviceSource; }
            set { imageFromDeviceSource = value; RaisePropertyChanged(nameof(ImageFromDeviceSource)); }
        }


        private bool isPrior;
        public bool IsPrior
        {
            get { return isPrior; }
            set { isPrior = value; RaisePropertyChanged(nameof(IsPrior)); }
        }

        private string colorForPrior;
        public string ColorForPrior
        {
            get { return colorForPrior; }
            set { colorForPrior = value; RaisePropertyChanged(nameof(GiveColorForPrior)); }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; RaisePropertyChanged(nameof(Description)); }
        }


        private TimeSpan selectedTime;
        public TimeSpan SelectedTime
        {
            get { return selectedTime; }
            set { selectedTime = value; RaisePropertyChanged(nameof(SelectedTime)); }
        }


        private DateTime dateTimeForAlarm   ;
        public  DateTime  DateTimeForAlarm
        {
            get { return dateTimeForAlarm; }
            set { dateTimeForAlarm = value; RaisePropertyChanged(nameof(DateTimeForAlarm)); }
        }

        private bool isVisible  ;
        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; RaisePropertyChanged(nameof(IsVisible)); }
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
            if (item.PhotoSource == null)
            {
                _topicItem.PhotoSource = "noFoto.jpeg";
            }
            IsVisible = true;
            LoadItemState();
            base.Init(initData);
        }

        private void LoadItemState()
        {
            ItemName = _topicItem.ItemName;
            ToDoDate = _topicItem.ToDoDate ?? DateTime.Now;
            PhotoSource = _topicItem.PhotoSource;
            MyTopicItem = _topicItem.MyTopicItem;
            IsPrior = _topicItem.IsPrior;
            Description = _topicItem.Description;
            SelectedTime = _topicItem.SelectedTime;
            ToDoDateString = _topicItem.ToDoDateString;
            ColorForPrior = _topicItem.ColorForPrior;

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

                var sourceUser = await Application.Current.MainPage.DisplayActionSheet("Wil je een afbeelding opslaan?", "Nee!", null, "Nieuwe foto nemen", "Bestande Foto kiezen");

                if (sourceUser == "Nee!")
                {
                    this.file = null;
                     return;
                }

                if (sourceUser == "Nieuwe foto nemen")
                {
                    this.file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = $"{ItemName}.jpg",
                        PhotoSize = PhotoSize.Small
                    });

                }
                else
                {
                    this.file = await CrossMedia.Current.PickPhotoAsync();
                }

                if (this.file == null)
                    return;

                await Application.Current.MainPage.DisplayAlert("File Location", this.file.Path, "OK");

                PhotoSource = this.file.Path;

                var source = ImageSource.FromStream(() =>
                {
                    var stream = this.file.GetStream();
                    return stream;
                });


            });


        private void SaveItemState()
        {
            GiveColorForPrior();
            _topicItem.ItemName = ItemName;
            _topicItem.ToDoDate = new DateTime?(ToDoDate);
            _topicItem.MyTopicItem = MyTopicItem;
            _topicItem.PhotoSource = PhotoSource;
            _topicItem.IsPrior = IsPrior;
            _topicItem.Description = Description;
            _topicItem.SelectedTime = SelectedTime;
            _topicItem.ToDoDateString = ToDoDate.ToShortDateString();
            _topicItem.ColorForPrior = ColorForPrior;
        }

        public string GiveColorForPrior()
        {
            if (IsPrior)
            {
                ColorForPrior = "Red";
            }
            else
            {
                ColorForPrior = "Black";

            }

            return ColorForPrior;
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

        public ICommand PickPhotoCommand => new Command(
           async () =>
           {
               IsVisible = false;

               Stream stream = await DependencyService.Get<IPhotoPickerService>().GetImageStreamAsync();
               if (stream != null)
               {
                   ImageFromDeviceSource = ImageSource.FromStream(() => stream);
               }
           });

        public ICommand CreateAlarmCommand => new Command(
            async () =>
            {
                DateTimeForAlarm = ToDoDate.Date + SelectedTime;

                var alarmService = DependencyService.Get<IAlarmService>();

                alarmId = await alarmService.CreateAlarmAsync( ItemName, "Alert gemaakt vanuit ToDoApp", DateTimeForAlarm, DateTimeForAlarm.AddHours(2), 4);
                if (string.IsNullOrWhiteSpace(alarmId))
                    await Application.Current.MainPage.DisplayAlert("Error", "Geen alert gemaakt", "ok");
                else
                    await Application.Current.MainPage.DisplayAlert("Succes", $"Alert id:{alarmId}", "ok");
            });
           

    

           
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
