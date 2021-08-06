
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using BrightSign.Core.ViewModels;
using BrightSign.Core.ViewModels.SearchUnits;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Views.Attributes;
using MvvmCross.Droid.Views.Fragments;

namespace BrightSign.Droid.Views.Fragments.SearchUnits
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, false)]
    public class SearchUnitsFragment : MvxFragment<SearchUnitsViewModel>
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.searchunitslayout, null);

            return view;

        }
    }
}
