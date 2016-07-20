using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;
using System.Data;
using Mono.Data.Sqlite;

namespace foodStoreSit313Jgallop
{
    [Activity(Label = "CreateRecipe")]
    public class CreateRecipe : Activity
    {
        private string Steps = "";
        private string Ingridients = "";
        private string DatabaseFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),"Recipes.db3");
        private EditText Ingri;
        private EditText Step;
        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            #region Listen And Call
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.CreateRecipe);
            Ingri = FindViewById<EditText>(Resource.Id.ETIngri);
            Step = FindViewById<EditText>(Resource.Id.ETstep);

            Button AddIngri = FindViewById<Button>(Resource.Id.AddIngri);
            AddIngri.Click += delegate { addIngridient(); };

            Button AddStep = FindViewById<Button>(Resource.Id.AddStep);
            AddStep.Click += delegate { addStep(); };
            
            #endregion
            
        }


        #region Private Methods
        private void addIngridient()
        {
            Ingridients += Ingri.Text + ",";
            string In = "";
            In = Ingri.Text + "\n";
            TextView TVIngri = FindViewById<TextView>(Resource.Id.Ingredients);
            TVIngri.Text += In;           
        }

        private void addStep()
        {
            Steps += Step.Text + ",";
            string st = "";
            st = Step.Text + "\n";
            TextView TVSteps = FindViewById<TextView>(Resource.Id.Steps);
            TVSteps.Text += st;

        }

        #endregion

        #region Sqlite Methods
        //https://developer.xamarin.com/guides/cross-platform/application_fundamentals/data/part_3_using_sqlite_orm/

        public void AddRecipe()
        {
            var connection = new SqliteConnection("Data Source=" + DatabaseFile);
            connection.Open();




            connection.Close();
        }







        #endregion

    }
}