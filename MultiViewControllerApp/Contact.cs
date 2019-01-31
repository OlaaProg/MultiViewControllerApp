using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using SQLite;
using UIKit;

namespace MultiViewControllerApp
{
    [Table("Items")]
    public class Contact
    {
        [PrimaryKey, AutoIncrement,Column("_id")]
        public int Id { get; set; }
        [MaxLength(20)]
        public string FirstName { get; set; }
        [MaxLength(20)]
        public string SecondName { get; set; }
        [MaxLength(20)]
        public string Street { get; set; }
        [MaxLength(20)]
        public string HausNumber { get; set; }
        [MaxLength(20)]
        public string Place { get; set; }
        [MaxLength(20)]
        public string Postcode { get; set; }
        [MaxLength(20)]
        public string Phone { get; set; }
        [MaxLength(20)]
        public string MobileNumber { get; set; }
        [MaxLength(20)]
        public string Email { get; set; }
    }
}