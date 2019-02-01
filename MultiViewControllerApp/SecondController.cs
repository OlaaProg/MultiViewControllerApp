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
                 CurrentContact.SecondName = txtSecondname.Text;
                 CurrentContact.Street = txtStreet.Text;
                 CurrentContact.HausNumber = txtHouseNumber.Text;
                 CurrentContact.Place = txtPlace.Text;
                 CurrentContact.Postcode = txtPostcode.Text;
                 CurrentContact.Phone = txtTel.Text;
                 CurrentContact.MobileNumber = txtMobil.Text;
                 CurrentContact.Email = txtEmail.Text;

                 HideKeyboard();
                 Delegate.SaveContact(CurrentContact);
             };

            btnDel.TouchUpInside += (sender, e) =>
            {
                HideKeyboard();
                Delegate.DeleteContact(CurrentContact);
            };
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            txtFirstName.Text = CurrentContact.FirstName;
            txtSecondname.Text = CurrentContact.SecondName;
            txtHouseNumber.Text = CurrentContact.HausNumber;
            txtStreet.Text = CurrentContact.Street;
            txtPlace.Text = CurrentContact.Place;
            txtPostcode.Text = CurrentContact.Postcode;
            txtTel.Text = CurrentContact.Phone;
            txtMobil.Text = CurrentContact.MobileNumber;

        }

        internal void SetItem(FirstController firstController, Contact item)
        {
            Delegate=firstController;
            CurrentContact = item;
        }

        #region Hilfe Methoden und Func

        public void HideKeyboard()
        {
            txtFirstName.ResignFirstResponder();
            txtSecondname.ResignFirstResponder();
            txtStreet.ResignFirstResponder();
            txtPlace.ResignFirstResponder();
            txtPostcode.ResignFirstResponder();
            txtHouseNumber.ResignFirstResponder();
            txtMobil.ResignFirstResponder();
            txtTel.ResignFirstResponder();
            txtEmail.ResignFirstResponder();


        }

        #endregion
    }
}