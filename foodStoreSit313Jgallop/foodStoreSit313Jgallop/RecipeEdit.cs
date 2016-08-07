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
        static string DatabaseFile = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Recipes.db3");
        private SqliteConnection connection = new SqliteConnection("Data Source=" + DatabaseFile);

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.RecipeEdit);
            Button EditRecipe = FindViewById<Button>(Resource.Id.EditRecp);

            EditRecipe.Click += delegate { UpdateRecipe(); };

            RecipeName = Intent.GetStringExtra("RecipeName") ?? "Data Not There";
            GetSqlData();
        }

        private void GetSqlData()
        {
            EditText Name = FindViewById<EditText>(Resource.Id.ETrecipeName);
            EditText Ingridients = FindViewById<EditText>(Resource.Id.ETIngri);
            EditText Steps = FindViewById<EditText>(Resource.Id.ETstep);

           

            Name.Text = RecipeName;
            try
            {
                SqliteDataReader rdr;
                SqliteCommand test = new SqliteCommand("SELECT * FROM Recipes WHERE Name = '" + RecipeName + "' ", connection);
                connection.Open();
                rdr = test.ExecuteReader();

                while (rdr.Read())
                {

                    Ingridients.Text = rdr[2].ToString();
                    Steps.Text = rdr[3].ToString();
                }

                connection.Close();
            }
            catch
            {

            }
        }


        private void UpdateRecipe()
        {
            EditText Name = FindViewById<EditText>(Resource.Id.ETrecipeName);
            EditText Ingridients = FindViewById<EditText>(Resource.Id.ETIngri);
            EditText Steps = FindViewById<EditText>(Resource.Id.ETstep);

            SqliteCommand AddData = new SqliteCommand(@"UPDATE Recipes SET Name='"+ Name.Text + "', Quantities='"+ Ingridients.Text +"', Steps='"+ Steps.Text +"' WHERE Name='"+ RecipeName +"';", connection);

            connection.Open();

            AddData.ExecuteNonQuery();

            connection.Close();

            Toast.MakeText(this, "Recipe Updated", ToastLength.Long).Show();
        }
    }
    
}