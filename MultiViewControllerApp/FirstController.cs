using Foundation;
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using UIKit;

namespace MultiViewControllerApp
{
    public partial class FirstController : UITableViewController
    {
        List<Contact> contacts;
        string sqlitePath;

        public FirstController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            //contacts = new List<Contact>
            // {
            //     new Contact{ FirstName ="Ola"},
            //     new Contact{FirstName="Isam"}
            // };

            contacts = new List<Contact>();
            
            DataInit();
            btnAdd.Clicked += (sender, e) => CreateContact();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
           TableView.Source = new RootTableSource(contacts.ToArray());
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            if(segue.Identifier== "ContactSegueId")
            {
                var secondController = segue.DestinationViewController as SecondController;
                if(secondController!=null)
                {
                    var source = TableView.Source as RootTableSource; ;
                    var rowPath = TableView.IndexPathForSelectedRow;
                    var item = source.GetItem(rowPath.Row);
                    secondController.SetItem(this, item);
                }
            }
        }

        #region save add delete Buttons
        public void SaveContact(Contact contact)
        {
            var oldContact = contacts.Find(t => t.Id == contact.Id);

            UpdateData(sqlitePath, contact);
            NavigationController.PopViewController(true);
        }
        
		public void DeleteContact(Contact contact)
        {
            var oldContact = contacts.Find(t => t.Id == contact.Id);
            contacts.Remove(oldContact);

            if (File.Exists(sqlitePath) == true)
            {                
                File.Delete(sqlitePath);
                Console.WriteLine("\nDatenbankdatei gelöscht...");
            }
            else
            {
                Console.WriteLine("\nDatenbankdatei existiert nicht!");
            }


            NavigationController.PopViewController(true);
        }
       
	    public void CreateContact()
        {
            var newId = contacts[contacts.Count - 1].Id + 1;
            var newContact = new Contact { Id = newId };
            contacts.Add(newContact);

            var detail = Storyboard.InstantiateViewController("secondStoryboardId") as SecondController;
            detail.SetItem(this, newContact);

            InsertData(sqlitePath, newContact);
            NavigationController.PushViewController(detail, true);
        }

        #endregion


        //todo: test it firstly
        #region Sqlite
        private void DataInit()
        {
            sqlitePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "MyTestContactsDB.db3");
            CreateSQLLiteDatabase(sqlitePath);
            
            QueryDate(sqlitePath);
        }

        private void CreateSQLLiteDatabase(string sDBFileName)
        {           
            try
            {
                if (!File.Exists(sDBFileName))
                {
                    SqliteConnection.CreateFile(sDBFileName);
                    using (SqliteConnection sqlcon = new SqliteConnection(String.Format("Data Source = {0}", sDBFileName)))
                    {
                        sqlcon.Open();
                        using (SqliteCommand sqlCom = new SqliteCommand(sqlcon))
                        {                            
                            sqlCom.CommandText = "CREATE TABLE TestContacts (ID INTEGER PRIMARY KEY, FirstName VARCHAR(20), SecondName VARCHAR(20), Street VARCHAR(20), Place VARCHAR(20), Phone VARCHAR(20), MobileNumber VARCHAR(20), Email)";                            
                            sqlCom.ExecuteNonQuery();
                            
                            //TODO: Test Insert Initial Contacts
                            sqlCom.CommandText = "INSERT INTO TestContacts (FirstName, Phone) VALUES ('Polizei','110')";
                            sqlCom.CommandText = "INSERT INTO TestContacts (FirstName, Phone) VALUES ('Feuerwehr und Rettungsdienst','112')";
                            sqlCom.ExecuteNonQuery();
                        }
                        sqlcon.Close();
                    }
                    Console.WriteLine("\nDatenbanktabelle [Contacts] erstellt...");

                }
                else
                {
                    Console.WriteLine("\nDatenbank existiert bereits!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("\nSqlite error: {0}", ex.Message));
            }
        }
        //
        private void InsertData(string sDBFileName, Contact contact)
        {
            int id = contact.Id;
            string sFirstName = contact.FirstName;
            string sLastName = contact.SecondName;
            string sStreet = contact.Street + ", " + contact.HausNumber;
            string sPlace = contact.Place + ", " + contact.Postcode;
            string sPhone = contact.Phone;
            string sMobileNumber = contact.MobileNumber;
            string sEmail = contact.Email;

            try
            {
                if (File.Exists(sDBFileName))
                {
                    using (SqliteConnection sqlcon = new SqliteConnection(String.Format("Data Source = {0}", sDBFileName)))
                    {
                        //sqlcon.ConnectionTimeout = 1000;
                        sqlcon.Open();
                        using (SqliteCommand sqlCom = new SqliteCommand(sqlcon))
                        {
                            // sqlCom.CommandText = String.Format("INSERT INTO TestContacts (FirstName ) VALUES ('{0}','{1}')", contact.FirstName, contact.SecondName );
                            //TODO: Test Insert a new Contact
                            sqlCom.CommandText = "INSERT INTO  TestContacts  (FirstName, SecondName,Street,Place, Phone, MobileNumber,Email) VALUES ('sFirstName', 'sLastName', 'sStreet', 'sPlace', 'sPhone', 'sMobileNumber', 'sEmail') WHERE Id= id";

                            sqlCom.ExecuteNonQuery();
                        }
                        sqlcon.Close();
                    }
                    Console.WriteLine("\nEine Zeile hinzugefügt...");

                }
                else
                {
                    Console.WriteLine("\nDatenbankdatei existiert nicht!");

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("\nSqlite error: {0}", ex.Message));
            }

        }
        //
        private void QueryDate(string sDBFileName)
        {
            try
            {
                if (File.Exists(sDBFileName))
                {
                    using (SqliteConnection sqlcon = new SqliteConnection(String.Format("Data Source = {0}", sDBFileName)))
                    {
                        sqlcon.Open();
                        using (SqliteCommand sqlCom = new SqliteCommand(sqlcon))
                        {
                            Console.WriteLine("");
                            Console.WriteLine("\nVorhandene Datenbankwerte der Tabelle: Contacts\n");
                            //
                            sqlCom.CommandText = "SELECT * FROM TestContacts";
                            using (SqliteDataReader dbReader = sqlCom.ExecuteReader())
                            {
                                while (dbReader.Read())
                                {
                                    contacts.Add(new Contact()
                                    {
                                        FirstName = string.Format("Firstname: {0}", dbReader["FirstName"]) ,
                                        SecondName = dbReader["SecondName"].ToString(),
                                        Street = dbReader["Street"].ToString(),
                                        Place = dbReader["Place"].ToString(),
                                        Phone = dbReader["Phone"].ToString(),
                                        MobileNumber = dbReader["MobileNumber"].ToString(),
                                        Email=dbReader["Email"].ToString()
                                    });
                                    Console.WriteLine(String.Format("\nVorname: {0}", dbReader["FirstName"]));
                                }
                            }
                        }
                        sqlcon.Close();
                    }

                }
                else
                {
                    Console.WriteLine("\nDatenbankdatei existiert nicht!");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("\nSqlite error: {0}", ex.Message));
            }

        }

        //TODO: Update data in SQLite
        private void UpdateData(string sDBFileName, Contact contact)
        {
            int id = contact.Id;
            string sFirstName = contact.FirstName;
            string sLastName = contact.SecondName;
            string sStreet = contact.Street + ", " + contact.HausNumber;
            string sPlace = contact.Place + ", " + contact.Postcode;
            string sPhone = contact.Phone;
            string sMobileNumber = contact.MobileNumber;
            string sEmail = contact.Email;
            if (sFirstName.Length == 0)
            {
                Console.WriteLine("\nAngabe Vorname fehlt.");
                return;
            }
            if (sLastName.Length == 0)
            {
                Console.WriteLine("\nAngabe Nachname fehlt.");
                return;
            }
            //todo: update a contact 

            try
            {
                if (File.Exists(sDBFileName))
                {
                    using (SqliteConnection sqlcon = new SqliteConnection(String.Format("Data Source = {0}", sDBFileName)))
                    {
                        sqlcon.Open();
                        using (SqliteCommand sqlCom = new SqliteCommand(sqlcon))
                        {
                            Console.WriteLine("");
                            Console.WriteLine("\nVorhandene Datenbankwerte der Tabelle: Contacts\n");
                            //
                            sqlCom.CommandText = "UPDATE TestContacts SET (FirstName, SecondName,Street,Place, Phone, MobileNumber,Email) VALUES ('sFirstName', 'sLastName', 'sStreet', 'sPlace', 'sPhone', 'sMobileNumber', 'sEmail') WHERE Id= id";
                            sqlCom.ExecuteNonQuery();
                        }
                        sqlcon.Close();
                    }

                }
                else
                {
                    Console.WriteLine("\nDatenbankdatei existiert nicht!");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("\nSqlite error: {0}", ex.Message));
            }
        }
        #endregion

    }
}