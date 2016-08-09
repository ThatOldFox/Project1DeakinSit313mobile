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
    [Activity(Label = "EditRecipes")]
    public class EditRecipes : Activity
    {
        //create variables
        private List<string> Recipes = new List<string>();
        private ListView RecipeList;
        private string DatabaseFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Recipes.db3"); //(Xamarin,2016)
        private SqliteDataReader rdr;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EditRecipes);

            #region database Connections
            //connect and retrive data base values

            try
            {
                var connection = new SqliteConnection("Data Source=" + DatabaseFile);
                SqliteCommand GetRows = new SqliteCommand("SELECT Name FROM Recipes", connection);
                connection.Open();
                //start reader
                rdr = GetRows.ExecuteReader();
                //add rdr data to the list
                while (rdr.Read())
                {
                    Recipes.Add(rdr[0].ToString());
                }

                connection.Close();
            }
            catch
            {
                //set default
                Recipes.Add("No Recipes Created");
            }

            #endregion


            #region Array Adapter + list View
            //retrive the list view and populate it using the array adapter 
            RecipeList = FindViewById<ListView>(Resource.Id.RecipeList);

            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Resource.Layout.ListItem, Recipes); //(Xamarin,2016)

            RecipeList.Adapter = adapter;
            RecipeList.ItemClick += ItemClick;
            #endregion
        }

        #region onlistclick method
        private void ItemClick(object sender, AdapterView.ItemClickEventArgs e) //(Joe Rock,2014)
        {
            string Name = Recipes[e.Position];
            Toast.MakeText(this, Name, ToastLength.Long).Show();
            //create the intent to start activity recipe edit and add the name of the recipe to be transfered
            Intent ViewRecipe = new Intent(this, typeof(RecipeEdit));
            ViewRecipe.PutExtra("RecipeName", Name); //(Xamarin,2016)
            StartActivity(ViewRecipe); 
        }
        #endregion
    }
} 