using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Foundation;
using Mono.Data.Sqlite;
using UIKit;

namespace MultiViewControllerApp.Database
{
    public class SqlitManager
    {
        string sqlitePath;
        //todo: test it firstly
        #region Sqlite
        public void DataInit()
        {
            sqlitePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "MyContactsDB.db3");
            CreateSQLLiteDatabase(sqlitePath);
        }

        public void CreateSQLLiteDatabase(string sqlitePath)
        {
            try
            {
                if (!File.Exists(sqlitePath))
                {
                    SqliteConnection.CreateFile(sqlitePath);
                    using (SqliteConnection sqlcon = new SqliteConnection(String.Format("Data Source = {0}", sqlitePath)))
                    {
                        sqlcon.Open();
                        using (SqliteCommand sqlCom = new SqliteCommand(sqlcon))
                        {
                            sqlCom.CommandText = "CREATE TABLE TestContacts (ID INTEGER PRIMARY KEY, FirstName VARCHAR(20), SecondName VARCHAR(20), Street VARCHAR(20), Place VARCHAR(20), Phone VARCHAR(20), MobileNumber VARCHAR(20), Email VARCHAR(20))";
                            sqlCom.ExecuteNonQuery();

                            //TODO: Test Insert Initial Contacts
                            sqlCom.CommandText = "INSERT INTO TestContacts (FirstName, Phone) VALUES ('Polizei','110')";
                            sqlCom.ExecuteNonQuery();
                            sqlCom.CommandText = "INSERT INTO TestContacts (FirstName, Phone) VALUES ('Feuerwehr','112')";
                            sqlCom.ExecuteNonQuery();
                            sqlCom.CommandText = "INSERT INTO TestContacts (FirstName, Phone) VALUES ('Rettungsdienst','112')";
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
                Console.WriteLine(String.Format("\nSqlite error Great SQLite Database: {0}", ex.Message));
            }
        }
        // TODO:Test Insert data in SQLite
        public void InsertData(Contact contact)
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
                if (File.Exists(sqlitePath))
                {
                    using (SqliteConnection sqlcon = new SqliteConnection(String.Format("Data Source = {0}", sqlitePath)))
                    {
                        //sqlcon.ConnectionTimeout = 1000;
                        sqlcon.Open();
                        using (SqliteCommand sqlCom = new SqliteCommand(sqlcon))
                        {
                            sqlCom.CommandText = String.Format("INSERT INTO TestContacts (FirstName, SecondName,Street,Place, Phone, MobileNumber,Email)" +
                                " VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}') ", contact.FirstName, contact.SecondName, contact.Street, contact.Place, contact.Phone, contact.MobileNumber, contact.Email);

                            sqlCom.ExecuteNonQuery();
                        }
                        sqlcon.Close();
                    }
                    Console.WriteLine("\nEine Zeile hinzugefügt...");

                }
                else
                {
                    CreateSQLLiteDatabase(sqlitePath);
                    Console.WriteLine("\nDatenbankdatei existiert nicht!");

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("\nSqlite error Insert a Data: {0}", ex.Message));
            }

        }
        //
        public List<Contact> QueryDate()
        {
            List<Contact> contacts = new List<Contact>();
            try
            {
                if (File.Exists(sqlitePath))
                {
                    using (SqliteConnection sqlcon = new SqliteConnection(String.Format("Data Source = {0}", sqlitePath)))
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
                                        Id = int.Parse(dbReader["Id"].ToString()),
                                        FirstName = dbReader["FirstName"].ToString(),
                                        SecondName = dbReader["SecondName"].ToString(),
                                        Street = dbReader["Street"].ToString(),
                                        Place = dbReader["Place"].ToString(),
                                        Phone = dbReader["Phone"].ToString(),
                                        MobileNumber = dbReader["MobileNumber"].ToString(),
                                        Email = dbReader["Email"].ToString()
                                    });
                                    Console.WriteLine(String.Format("\n============================== Id: {0}, Vorname: {1}", dbReader["Id"], dbReader["FirstName"]));
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
                Console.WriteLine(String.Format("\nSqlite error Query the Data: {0}", ex.Message));
            }
            return contacts;
        }

        //TODO:Test Update data in SQLite
        public void UpdateData(Contact contact)
        {

            //todo: update a contact

            try
            {
                if (File.Exists(sqlitePath))
                {
                    using (SqliteConnection sqlcon = new SqliteConnection(String.Format("Data Source = {0}", sqlitePath)))
                    {
                        sqlcon.Open();
                        using (SqliteCommand sqlCom = new SqliteCommand(sqlcon))
                        {
                            Console.WriteLine("");
                            Console.WriteLine("\nVorhandene Datenbankwerte der Tabelle: Contacts\n");

                            sqlCom.CommandText = String.Format("UPDATE TestContacts" +
                                " SET FirstName = '{0}', SecondName = '{1}', Street= '{2}' , Place= '{3}' , Phone= '{4}' , MobileNumber= '{5}' ,Email= '{6}' " +
                                " WHERE Id = {7} ;",
                                contact.FirstName, contact.SecondName, contact.Street, contact.Place, contact.Phone, contact.MobileNumber, contact.Email, contact.Id);
                            sqlCom.ExecuteNonQuery();
                            Console.WriteLine("\n===================Update =======================s\n");

                        }
                        sqlcon.Close();
                    }
                    Console.WriteLine("\n===================Datenbankdatei Update ist geklappt!===============================");
                }
                else
                {
                    Console.WriteLine("\nDatenbankdatei existiert nicht!");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("\nSqlite error Update one data: {0}", ex.Message));
            }
        }

        //TODO:Test Update data in SQLite
        public void DeleteData(int id)
        {
            //todo: delte a contact 
            try
            {
                if (File.Exists(sqlitePath))
                {
                    using (SqliteConnection sqlcon = new SqliteConnection(String.Format("Data Source = {0}", sqlitePath)))
                    {
                        sqlcon.Open();
                        using (SqliteCommand sqlCom = new SqliteCommand(sqlcon))
                        {
                            Console.WriteLine("");
                            Console.WriteLine("\nVorhandene Datenbankwerte der Tabelle: Contacts\n");
                            //
                            sqlCom.CommandText = "DELETE FROM TestContacts  WHERE Id= " + id + "; ";
                            sqlCom.ExecuteNonQuery();
                        }
                        sqlcon.Close();
                    }
                    Console.WriteLine("\n===================Datenbankdatei delete ist geklappt!===============================");
                }
                else
                {
                    Console.WriteLine("\nDatenbankdatei existiert nicht!");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("\nSqlite error Delete one Data: {0}", ex.Message));
            }
        }

        //TODO:Test delete all data in SQLite
        public void DeleteAllData()
        {
            //todo: delte all contacts 

            try
            {
                if (File.Exists(sqlitePath))
                {
                    using (SqliteConnection sqlcon = new SqliteConnection(String.Format("Data Source = {0}", sqlitePath)))
                    {
                        sqlcon.Open();
                        using (SqliteCommand sqlCom = new SqliteCommand(sqlcon))
                        {
                            Console.WriteLine("");
                            Console.WriteLine("\nVorhandene Datenbankwerte der Tabelle: Contacts\n");
                            //
                            sqlCom.CommandText = "DELETE FROM TestContacts";
                            sqlCom.ExecuteNonQuery();
                        }
                        sqlcon.Close();
                    }
                    Console.WriteLine("\n===================Datenbankdatei delete ist geklappt!===============================");
                }
                else
                {
                    Console.WriteLine("\nDatenbankdatei existiert nicht!");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("\nSqlite error  Delete All Data: {0}", ex.Message));
            }
        }

        public void DeleteFileData()
        {
            if (File.Exists(sqlitePath) == true)
            {
                File.Delete(sqlitePath);
                Console.WriteLine("\nDatenbankdatei gelöscht...");
            }
            else
            {
                Console.WriteLine("\nDatenbankdatei existiert nicht!");
            }

        }
        #endregion
    }
}