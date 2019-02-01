using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Foundation;
using SQLite;
using UIKit;

namespace MultiViewControllerApp.Database
{
    //Todo: test it secondly
    public class DbManager
    {
        List<Contact> contacts;
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "MyTestSQLiteContactsDB.db3");
        public DbManager()
        {
            contacts = new List<Contact>();
        }

        public void DataInit()
        {
            CreateSQLLiteDatabase(); ;
        }
        public void CreateSQLLiteDatabase()
        {
            
            //TODO:Test Erstellen einer leeren Datenbank
            var db = new SQLiteConnection(dbPath);
            if (db.Table<Contact>() == null)
            {
                //Speichern von Daten
                db.CreateTable<Contact>();

                Contact initContact = new Contact()
                {
                    FirstName = "Polizei",
                    Phone = "110"
                };

                db.Insert(initContact);
                initContact = new Contact()
                {
                    FirstName = "Rettungsdienst",
                    Phone = "112"
                };
                db.Insert(initContact);
                initContact = new Contact()
                {
                    FirstName = "Feuerwehr",
                    Phone = "112"
                };
                db.Insert(initContact);
                if (db.Table<Contact>() != null)
                    if (db.Table<Contact>().Count() != 0)
                        contacts = db.Table<Contact>().ToList<Contact>();

                db.Close();
            }
        }

        public void InsertData(Contact contact)
        {
            var db = new SQLiteConnection(dbPath);
            if (db.Table<Contact>().Count() == 0)
            {
                db.CreateTable<Contact>();
                db.Insert(contact);
            }
            db.Close();
        }

        //TODO:Test Abrufen von Daten
        public Contact GetData(int id)
        {
            var db = new SQLiteConnection(dbPath);
            return db.Get<Contact>(id);
        }

        //TODO:Test Abrufen Alle Daten
        public List<Contact> QueryDate()
        {
            //List<Contact> contacts = new List<Contact>();
            //var db = new SQLiteConnection(dbPath);
            //Contact initContact = new Contact()
            //{
            //    FirstName = "Polizei",
            //    Phone = "110"
            //};
            //db.Insert(initContact);

            //var table = db.Query<Contact>("SELECT * FROM Items");
            //var table = db.Table<Contact>();
            //if (table != null)
            //    if (table.Count() != 0)
            //        foreach (var s in table)
            //        {
            //            Console.WriteLine(s.Id + " " + s.FirstName);
            //            contacts.Add(s);
            //        }

            //if (db.Table<Contact>() != null )
            //if(db.Table<Contact>().Count() != 0)
            //    contacts = db.Table<Contact>().ToList<Contact>();

            //db.Close();
            return contacts;
        }

        //TODO:TestLöschen von Daten
        public void DeleteData(int id)
        {
            var db = new SQLiteConnection(dbPath);
            db.Delete<Contact>(id);
        }

        public void DeleteAllData()
        {
            var db = new SQLiteConnection(dbPath);
            db.DeleteAll<Contact>();
        }

        //TODO:Test Aktulisieren eine von Daten
        public void UpdateData(Contact contact)
        {
            var db = new SQLiteConnection(dbPath);
            db.Update(contact);
        }
    }
}