/*Purpose to allow the users to view a recipe stored in the database
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
    [Activity(Label = "RecipeView")]
    public class RecipeView : Activity
    {
        private TextView Name;
        private string RecipeName = "";
        private string DatabaseFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Recipes.db3"); //(Xamarin,2016)

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.RecipeView);
            //get the recipe name from the intent and set it to default if its not there
            RecipeName = Intent.GetStringExtra("RecipeName") ?? "Data Not There"; //(Xamarin,2016)

            Name = FindViewById<TextView>(Resource.Id.RecipeName);

            Name.Text = "  " + RecipeName;

          
            LoadSqlData();
        }


        #region load sql data
        private void LoadSqlData()
        {
            TextView Ingridients = FindViewById<TextView>(Resource.Id.Ingredients);
            TextView Steps = FindViewById<TextView>(Resource.Id.Steps);
            var connection = new SqliteConnection("Data Source=" + DatabaseFile);
            //load the recipe data into the text views using sql
            try
            {
                SqliteDataReader rdr;
                SqliteCommand test = new SqliteCommand("SELECT * FROM Recipes WHERE Name = '" + RecipeName + "' ", connection);
                connection.Open();
                rdr = test.ExecuteReader();

                while (rdr.Read())
                {

                    Ingridients.Text = rdr[2].ToString().Replace(",", "\n");
                    Steps.Text = rdr[3].ToString().Replace(",", "\n");
                }

                connection.Close();
            }
            catch
            {
                Toast.MakeText(this, "There Are No Recipes", ToastLength.Long);
            }

        }
        #endregion



    }
}