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
        private string DatabaseFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Recipes.db3");

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.RecipeView);

            RecipeName = Intent.GetStringExtra("RecipeName") ?? "Data Not There";

            Name = FindViewById<TextView>(Resource.Id.RecipeName);

            Name.Text = "  " + RecipeName;

          
            LoadSqlData();
        }

    

        private void LoadSqlData()
        {
            TextView Ingridients = FindViewById<TextView>(Resource.Id.Ingredients);
            TextView Steps = FindViewById<TextView>(Resource.Id.Steps);
            var connection = new SqliteConnection("Data Source=" + DatabaseFile);

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


    
    }
}