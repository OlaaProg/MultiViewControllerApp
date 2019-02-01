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

            btn_CallTel.TouchUpInside += (sender, e) =>
             {
                 HideKeyboard();
                 Call(txtTel.Text);
             };

            btnCall.TouchUpInside += (sender, e) =>
             {
                 HideKeyboard();
                 Call(txtMobil.Text);
             };

            btnSMS.TouchUpInside += (sender, e) =>
            {
                HideKeyboard();
                SendSMS(txtMobil.Text);
            };

            btnEmail.TouchUpInside += (sender, e) =>
            {
                HideKeyboard();
                SendEmail(txtEmail.Text);
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
            Delegate = firstController;
            CurrentContact = item;
        }

        #region Hilfe Methoden und Func

        private void HideKeyboard()
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

        private void Call(string callNumber)
        {
            NSUrl url = new NSUrl("tel:+123456789012");

            if (callNumber.Length == 12)
            { url = new NSUrl("tel:+" + callNumber); }

            if (UIApplication.SharedApplication.CanOpenUrl(url))
            {
                UIApplication.SharedApplication.OpenUrl(url);
            }
            else
            {
                Console.WriteLine("Cannot opern url: {0}", url.AbsoluteString);
                var alert = UIAlertController.Create("Not supported! \n:(", "Scheme 'tel:' is not supported on the device", UIAlertControllerStyle.Alert);
                alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                PresentViewController(alert, true, null);
            }
        }

        private void SendSMS(string callNumber)
        {
            NSUrl url = new NSUrl("sms:+123456789012");

            if (callNumber.Length == 12)
            { url = new NSUrl("sms:+" + callNumber); }

            if (UIApplication.SharedApplication.CanOpenUrl(url))
            {
                UIApplication.SharedApplication.OpenUrl(url);
            }
            else
            {
                Console.WriteLine("Cannot opern url: {0}", url.AbsoluteString);
                var alert = UIAlertController.Create("Not supported! \n:(", "Scheme 'sms:' is not supported on the device", UIAlertControllerStyle.Alert);
                alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                PresentViewController(alert, true, null);
            }
        }

        private void SendEmail(string email)
        {
            NSUrl url = new NSUrl("mailto:mail@example.com");

            if (email.Length == 12)
            { url = new NSUrl("mailto:" + email); }

            if (UIApplication.SharedApplication.CanOpenUrl(url))
            {
                UIApplication.SharedApplication.OpenUrl(url);
            }
            else
            {
                Console.WriteLine("Cannot opern url: {0}", url.AbsoluteString);
                var alert = UIAlertController.Create("Not supported! \n:(", "Scheme 'mailto:' is not supported on the device", UIAlertControllerStyle.Alert);
                alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                PresentViewController(alert, true, null);
            }
        }
        #endregion
    }
}