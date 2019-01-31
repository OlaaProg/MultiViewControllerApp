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
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "MyTestSQLiteContactsDB.db3");
        public DbManager()
        {
            CreateDatabase();
           
        }
       
        public void CreateDatabase()
        {
            //TODO:Test Erstellen einer leeren Datenbank
            var db = new SQLiteConnection(dbPath);
            if (db.Table<Contact>().Count() == 0)
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
                    FirstName = "Feuerwehr",
                    SecondName = "Rettungsdienst",
                    Phone = "112"
                };
                db.Insert(initContact);
            }
        }
        
        //TODO:Test Abrufen von Daten
        public Contact GetContact(int id)
        {
            var db = new SQLiteConnection(dbPath);
            return db.Get<Contact>(id);
        }
        
        //TODO:Test Abrufen Alle Daten
        public List<Contact> GetAllContact()
        {
            var db = new SQLiteConnection(dbPath);
            var contacts= db.Table<Contact>().ToList() ;
            return contacts ;
        }

        //TODO:TestLöschen von Daten
        public void DeleteContact(int id)
        {
            var db = new SQLiteConnection(dbPath);
            db.Delete<Contact>(id);
        }

        //TODO:Test Aktulisieren eine von Daten
        public void UpdateContact(Contact contact)
        {
            var db = new SQLiteConnection(dbPath);
             db.Update(contact);
        }
    }
}