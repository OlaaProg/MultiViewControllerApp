using Foundation;
using System;
using UIKit;

namespace MultiViewControllerApp
{
    public partial class SecondController : UIViewController
    {
        Contact CurrentContact { get; set; }
        public FirstController Delegate { get; set; }

        public SecondController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            btnSave.TouchUpInside += (sender, e) =>
             {
                 CurrentContact.FirstName = txtFirstName.Text;
                 Delegate.SaveContact(CurrentContact);
             };

            btnDel.TouchUpInside += (sender, e) =>
            {
                Delegate.DeleteContact(CurrentContact);
            };
        }
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            txtFirstName.Text = CurrentContact.FirstName;

        }
        internal void SetItem(FirstController firstController, Contact item)
        {
            Delegate=firstController;
            CurrentContact = item;
        }

    }
}