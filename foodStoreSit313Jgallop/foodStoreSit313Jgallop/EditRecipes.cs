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
        private List<string> Recipes = new List<string>();
        private ListView RecipeList;
        private string DatabaseFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Recipes.db3");
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
                rdr = GetRows.ExecuteReader();

                while (rdr.Read())
                {
                    Recipes.Add(rdr[0].ToString());
                }

                connection.Close();
            }
            catch
            {
                Recipes.Add("error sql not found");
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

        private void ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            string Name = Recipes[e.Position];
            Toast.MakeText(this, Name, ToastLength.Long).Show();

            Intent ViewRecipe = new Intent(this, typeof(RecipeEdit));
            ViewRecipe.PutExtra("RecipeName", Name);
            StartActivity(ViewRecipe); 
        }
    }
} 