using Foundation;
using System;
using System.Collections.Generic;
using System.IO;
using UIKit;
using MultiViewControllerApp.Database;

namespace MultiViewControllerApp
{
    public partial class FirstController : UITableViewController
    {
        List<Contact> contacts;
        SqlitManager sqlitManager;
        //DbManager sqlitManager;
        public FirstController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            //Test a Contacts list
            //contacts = new List<Contact>
            // {
            //     new Contact{ FirstName ="Ola"},
            //     new Contact{FirstName="Isam"}
            // };

            contacts = new List<Contact>();
            sqlitManager = new SqlitManager();
            //sqlitManager = new DbManager();
            sqlitManager.DataInit();
            contacts = sqlitManager.QueryDate();
            contacts = Sorting();
            btnAdd.Clicked += (sender, e) => CreateContact();
            btnDeleteAll.TouchUpInside += (sender, e) =>
             {
                 DeleteAllContact();
             };
            
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
           TableView.Source = new RootTableSource(contacts.ToArray());
            NavigationController.Title = contacts.Count + " Contacts";
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            if(segue.Identifier== "ContactSegueId")
            {
                var secondController = segue.DestinationViewController as SecondController;
                if(secondController!=null)
                {
                    var source = TableView.Source as RootTableSource; 
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

            sqlitManager.UpdateData(contact);
            contacts = Sorting();
            TableView.Source = new RootTableSource(contacts.ToArray());
            NavigationController.PopViewController(true);
        }
        
		public void DeleteAllContact()
        {                      
            //sqlitManager.DeleteAllData();
            //contacts= sqlitManager.QueryDate();
            //contacts = Sorting();
            //TableView.Source = new RootTableSource(contacts.ToArray());
        }

        public void DeleteContact(Contact contact)
        {
            var oldContact = contacts.Find(t => t.Id == contact.Id);
            contacts.Remove(oldContact);

            sqlitManager.DeleteData(contact.Id);
            NavigationController.PopViewController(true);
        }

        public void CreateContact()
        {
            var newId = contacts[contacts.Count - 1].Id + 1;
            var newContact = new Contact { Id = newId , FirstName="Test"};
            contacts.Add(newContact);

            var detail = Storyboard.InstantiateViewController("secondStoryboardId") as SecondController;
            detail.SetItem(this, newContact);

            sqlitManager.InsertData(newContact);
            contacts = Sorting();

            TableView.Source = new RootTableSource(contacts.ToArray());
            NavigationController.PushViewController(detail, true);
        }

        //Sorting according to the Firstname
        public List<Contact> Sorting()
        {
            contacts.Sort((x, y) => String.Compare(x.FirstName, y.FirstName));
            return contacts;
        }
       
        #endregion
       
      

    }
}