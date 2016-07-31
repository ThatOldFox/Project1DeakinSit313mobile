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

            Button AddRec = FindViewById<Button>(Resource.Id.AddRecp);
            AddRec.Click += delegate { AddRecipe(); };
            
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
        //https://developer.xamarin.com/guides/cross-platform/application_fundamentals/data/part_4_using_adonet/

        public void AddRecipe()
        {
            var connection = new SqliteConnection("Data Source=" + DatabaseFile);
            EditText RecipeName = FindViewById<EditText>(Resource.Id.ETrecipeName);
            bool TableExists;

            try
            {
                SqliteCommand test = new SqliteCommand("SELECT Name FROM Recipes", connection);
                connection.Open();
                test.ExecuteNonQuery();
                connection.Close();

                TableExists = true;
            }
            catch
            {
                connection.Close();
                TableExists = false;
            }


           //check if the database file exists
            if (File.Exists(DatabaseFile) && TableExists == true)
            {
                SqliteCommand AddData = new SqliteCommand(@"INSERT INTO Recipes (Name,Quantities,Steps) VALUES ('" + RecipeName.Text + "', '" + Ingridients + "', '" + Steps + "' )", connection);
                
                connection.Open();

                AddData.ExecuteNonQuery();

                connection.Close();

                Toast.MakeText(this, "Recipe Added", ToastLength.Long).Show();
            }
            else
            {
                SqliteConnection.CreateFile(DatabaseFile);
                SqliteCommand CreateRecipeTable = new SqliteCommand("CREATE TABLE [Recipes] ( [Id] INTEGER NOT NULL, [Name] nvarchar(100) NULL, [Quantities] nvarchar(4000) NULL, [Steps] nvarchar(4000) NULL, CONSTRAINT[PK_Recipes] PRIMARY KEY([Id])); ", connection);
                connection.Open();
                CreateRecipeTable.ExecuteNonQuery();
                connection.Close();

                SqliteCommand AddData = new SqliteCommand(@"INSERT INTO Recipes (Name,Quantities,Steps) VALUES ('" + RecipeName.Text + "', '" + Ingridients + "', '"+ Steps +"' )", connection);

                connection.Open();

                AddData.ExecuteNonQuery();

                connection.Close();

                Toast.MakeText(this, "Recipe Added", ToastLength.Long).Show();


            }

        }

        #endregion

    }
}