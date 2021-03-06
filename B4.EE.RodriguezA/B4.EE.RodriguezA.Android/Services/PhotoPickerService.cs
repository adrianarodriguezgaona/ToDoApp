﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using B4.EE.RodriguezA.Domain.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(B4.EE.RodriguezA.Droid.Services.PhotoPickerService))]

namespace B4.EE.RodriguezA.Droid.Services
{   
      
        public class PhotoPickerService : IPhotoPickerService
        {
        public Task<string> GetImagePathAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Stream> GetImageStreamAsync()
            {
                // Define the Intent for getting images
                Intent intent = new Intent();
                intent.SetType("image/*");
                intent.SetAction(Intent.ActionGetContent);

                // Start the picture-picker activity (resumes in MainActivity.cs)
                MainActivity.Instance.StartActivityForResult(
                    Intent.CreateChooser(intent, "Select Picture"),
                    MainActivity.PickImageId);

                // Save the TaskCompletionSource object as a MainActivity property
                MainActivity.Instance.PickImageTaskCompletionSource = new TaskCompletionSource<Stream>();

                // Return Task object
                return MainActivity.Instance.PickImageTaskCompletionSource.Task;
            }
        }

}