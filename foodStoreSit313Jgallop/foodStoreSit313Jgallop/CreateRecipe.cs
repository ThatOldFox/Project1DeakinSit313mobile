/*Purpose to create the recipe and store it in the sqlite database
 * References...
 * Xamarin,2016,Configuration,Xamarin, Retrived 18/07/2016 https://developer.xamarin.com/guides/cross-platform/application_fundamentals/data/part_2_configuration/
 * Xamarin,2016,Using ADO.net,Xamarin,Retrieved 18/07/2016 https://developer.xamarin.com/guides/cross-platform/application_fundamentals/data/part_4_using_adonet/
 */
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
        //create variables
        private string Steps = "";
        private string Ingridients = "";
        private string DatabaseFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),"Recipes.db3"); //(Xamarin, 2016)
        private EditText Ingri;
        private EditText Step;
        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            #region Listen And Call
            base.OnCreate(savedInstanceState);
            //call methods when a button is clicked
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
            //create the add ingredient string
            Ingridients += Ingri.Text + ",";
            string In = "";
            In = Ingri.Text + "\n";
            TextView TVIngri = FindViewById<TextView>(Resource.Id.Ingredients);
            TVIngri.Text += In;
            Ingri.Text = "";
           
        }

        private void addStep()
        {
            //create the add step string 
            Steps += Step.Text + ",";
            string st = "";
            st = Step.Text + "\n";
            TextView TVSteps = FindViewById<TextView>(Resource.Id.Steps);
            TVSteps.Text += st;
            Step.Text = "";

        }

        #endregion

        #region Sqlite Methods
        

        public void AddRecipe()
        {
            //create the connection to the db
            var connection = new SqliteConnection("Data Source=" + DatabaseFile);
            EditText RecipeName = FindViewById<EditText>(Resource.Id.ETrecipeName);
            bool TableExists;

            //check if the table exists
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
                bool NameExists = false;
                string Check = "";
                SqliteCommand CheckName = new SqliteCommand("SELECT Name FROM Recipes WHERE Name ='" + RecipeName.Text + "'", connection);
                SqliteDataReader rdr;

                connection.Open();

                rdr = CheckName.ExecuteReader();
                while(rdr.Read())
                {
                    Check = rdr[0].ToString();
                }
                connection.Close();


                if (Check != "")
                {
                    NameExists = true;
                }

                if(NameExists == false)
                {

                    //create the insert statement
                    SqliteCommand AddData = new SqliteCommand(@"INSERT INTO Recipes (Name,Quantities,Steps) VALUES ('" + RecipeName.Text + "', '" + Ingridients + "', '" + Steps + "' )", connection);
                    //open the connection
                    connection.Open();
                    //execute the query 
                    AddData.ExecuteNonQuery();

                    connection.Close();

                    //start toast
                    Toast.MakeText(this, "Recipe Added", ToastLength.Long).Show();
                }
                else
                {
                    Toast.MakeText(this, "Recipe Name Already In Use", ToastLength.Long).Show();
                }
            }
            else
            {
                //(Xamarin,2016)
                SqliteConnection.CreateFile(DatabaseFile);
                SqliteCommand CreateRecipeTable = new SqliteCommand("CREATE TABLE [Recipes] ( [Id] INTEGER NOT NULL, [Name] nvarchar(100) NULL, [Quantities] nvarchar(4000) NULL, [Steps] nvarchar(4000) NULL, CONSTRAINT[PK_Recipes] PRIMARY KEY([Id])); ", connection);
                //(Xamarin,2016)
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