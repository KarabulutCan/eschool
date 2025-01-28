using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    public class Customers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerID { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Address { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        public string Town { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        public string City { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        public string Country { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string ZipCode { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        public string Phone { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Email { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Website { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string ContactName { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        public string ContactMobile { get; set; }

        [Column(TypeName = "date")]
        public Nullable<DateTime> RegisterDate { get; set; }

        [Column(TypeName = "date")]
        public Nullable<DateTime> ExpireDate { get; set; }

        public Nullable<bool> IsActive { get; set; }
    }

}
