/*
 * Purpose to call activities when there coresponding button is clicked
 */
using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace foodStoreSit313Jgallop
{
    [Activity(Label = "foodStoreSit313Jgallop", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            //set instance of create recipie button to a new button object
            Button CreateRecipe = FindViewById<Button>(Resource.Id.bt1);
            Button ViewRecipes = FindViewById<Button>(Resource.Id.bt2);
            Button EditRecipes = FindViewById<Button>(Resource.Id.bt3);

            CreateRecipe.Click += delegate { StartActivity(typeof(CreateRecipe)); };
            ViewRecipes.Click += delegate { StartActivity(typeof(ViewRecipes)); };
            EditRecipes.Click += delegate { StartActivity(typeof(EditRecipes)); };

                
        }
    }
}

