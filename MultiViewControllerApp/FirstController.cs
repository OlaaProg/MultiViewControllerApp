using Foundation;
using System;
using System.Collections.Generic;
using UIKit;

namespace MultiViewControllerApp
{
    public partial class FirstController : UITableViewController
    {
        List<Contact> contacts;

        public FirstController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            contacts = new List<Contact>
            {
                new Contact{ FirstName ="Ola"},
                new Contact{FirstName="Isam"}
            };
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

        public void SaveContact(Contact contact)
        {
            var oldTask = contacts.Find(t => t.Id == contact.Id);
            NavigationController.PopViewController(true);
        }
        public void DeleteContact(Contact contact)
        {
            var oldContact = contacts.Find(t => t.Id == contact.Id);
            contacts.Remove(oldContact);
            NavigationController.PopViewController(true);
        }
        public void CreateContact()
        {
            var newId = contacts[contacts.Count - 1].Id + 1;
            var newContact = new Contact { Id = newId };
            contacts.Add(newContact);


            var detail = Storyboard.InstantiateViewController("secondStoryboardId") as SecondController;
            detail.SetItem(this, newContact);
            NavigationController.PushViewController(detail, true);
        }
    }
}