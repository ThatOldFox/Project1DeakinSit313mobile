/*Purpose to allow the users to edit a recipe stored in the database
 * References...
 * Xamarin,2016,Configuration,Xamarin, Retrived 18/07/2016 https://developer.xamarin.com/guides/cross-platform/application_fundamentals/data/part_2_configuration/
 * Xamarin,2016,Passing Data Between Activitys,Xamarin,Retrieved 29/07/2016 https://developer.xamarin.com/recipes/android/fundamentals/activity/pass_data_between_activity/
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
    [Activity(Label = "RecipeEdit")]
    public class RecipeEdit : Activity
    {
        private string RecipeName = "";
        static string DatabaseFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Recipes.db3"); //(Xamarin,2016)
        private SqliteConnection connection = new SqliteConnection("Data Source=" + DatabaseFile);

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.RecipeEdit);
            Button EditRecipe = FindViewById<Button>(Resource.Id.EditRecp);
            Button DelRecipe = FindViewById<Button>(Resource.Id.DelRecp);

            EditRecipe.Click += delegate { UpdateRecipe(); };
            DelRecipe.Click += delegate { DeleteRecipe(); };

            //get the recipe name from the intent if it isnt there set the string to data not there
            RecipeName = Intent.GetStringExtra("RecipeName") ?? "Data Not There"; //(Xamarin,2016)
            GetSqlData();
        }

        #region Sqlite Methods
        private void GetSqlData()
        {
            EditText Name = FindViewById<EditText>(Resource.Id.ETrecipeName);
            EditText Ingridients = FindViewById<EditText>(Resource.Id.ETIngri);
            EditText Steps = FindViewById<EditText>(Resource.Id.ETstep);

           

            Name.Text = RecipeName;
            try
            {
                //get all of the recipes information
                SqliteDataReader rdr;
                SqliteCommand test = new SqliteCommand("SELECT * FROM Recipes WHERE Name = '" + RecipeName + "' ", connection);
                connection.Open();
                rdr = test.ExecuteReader();

                //read the recipe info and set it to the edit texts
                while (rdr.Read())
                {

                    Ingridients.Text = rdr[2].ToString();
                    Steps.Text = rdr[3].ToString();
                }

                connection.Close();
            }
            catch
            {
                Toast.MakeText(this, "There Are No Recipes", ToastLength.Long);
            }
        }


        private void UpdateRecipe()
        {
            EditText Name = FindViewById<EditText>(Resource.Id.ETrecipeName);
            EditText Ingridients = FindViewById<EditText>(Resource.Id.ETIngri);
            EditText Steps = FindViewById<EditText>(Resource.Id.ETstep);
            //update the recipe with the new values
            SqliteCommand AddData = new SqliteCommand(@"UPDATE Recipes SET Name='"+ Name.Text + "', Quantities='"+ Ingridients.Text +"', Steps='"+ Steps.Text +"' WHERE Name='"+ RecipeName +"';", connection);

            connection.Open();
            //execute update statement
            AddData.ExecuteNonQuery();

            connection.Close();

            //close this activity and take them to the main activity
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);

            this.Finish();
        }
        


        private void DeleteRecipe()
        {
            //delete the recipe
            SqliteCommand DeleteRecipe = new SqliteCommand("DELETE FROM Recipes WHERE Name='" + RecipeName + "'", connection);

            connection.Open();

            DeleteRecipe.ExecuteNonQuery();
            
            connection.Close();
            //close this activity and take them to the main activity
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            
            this.Finish();


        }
        #endregion
    }

}