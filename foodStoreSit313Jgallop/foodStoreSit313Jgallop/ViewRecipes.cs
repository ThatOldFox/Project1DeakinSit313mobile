/*Purpose Loads the Sqlite data into a list view and handels on click events for the list
 * References...
 * Xamarin,2016,Configuration,Xamarin, Retrived 18/07/2016 https://developer.xamarin.com/guides/cross-platform/application_fundamentals/data/part_2_configuration/
 * Xamarin,2016,Use An Array Adapter,Xamarin,Retrieved 28/07/2016 https://developer.xamarin.com/recipes/android/data/adapters/use_an_arrayadapter/
 * Xamarin Android Tutorial 5 Listview Click Listeners,2014,Youtube,Joe Rock,10 November, Retrieved, 28/07/2016 https://www.youtube.com/watch?v=WMKrR_5uh0A&ab_channel=JoeRock
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
    [Activity(Label = "ViewRecipes")]
    public class ViewRecipes : Activity
    {
        private List<string> Recipes = new List<string>();
        private ListView RecipeList;
        private string DatabaseFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Recipes.db3"); //(Xamarin,2016) 
        private SqliteDataReader rdr;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ViewRecipes);

            #region database Connections
            //connect and retrive database values

            try
            {
                var connection = new SqliteConnection("Data Source=" + DatabaseFile);
                SqliteCommand GetRows = new SqliteCommand("SELECT Name FROM Recipes", connection);
                connection.Open();
                rdr = GetRows.ExecuteReader();

                while (rdr.Read())
                {
                    Recipes.Add(rdr[0].ToString());
                }

                connection.Close();
            }
            catch
            {
                Recipes.Add("No Recipes Created");
            }

            #endregion


            #region Array Adapter + list View
            //retrive the list view and populate it using the array adapter 
            RecipeList = FindViewById<ListView>(Resource.Id.RecipeList);

            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Resource.Layout.ListItem, Recipes);

            RecipeList.Adapter = adapter;
            RecipeList.ItemClick += ItemClick;
            #endregion
        }

        #region onlistclick method
        private void ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            string Name = Recipes[e.Position];
            Toast.MakeText(this, Name , ToastLength.Long).Show();
            //create the intent and pass the recipe name to the next activity
            Intent ViewRecipe = new Intent(this, typeof(RecipeView));
            ViewRecipe.PutExtra("RecipeName", Name); //(Xamarin,2016)
            StartActivity(ViewRecipe);
        }
        #endregion
    }
}