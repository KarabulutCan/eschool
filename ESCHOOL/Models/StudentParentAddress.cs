using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESCHOOL.Models
{
    public class StudentParentAddress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentParentAddressID { get; set; }
        public int StudentID { get; set; }

        [Display(Name = "GenderType", ResourceType = typeof(Resources.Resource))]
        public int ParentGenderTypeCategoryID { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string ParentPicture { get; set; }

        [Display(Name = "Kinship", ResourceType = typeof(Resources.Resource))]
        public int KinshipCategoryID { get; set; }

        [Display(Name = "Profession", ResourceType = typeof(Resources.Resource))]
        public int ProfessionCategoryID { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "IdNumber", ResourceType = typeof(Resources.Resource))]
        public string IdNumber { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "HomePhone", ResourceType = typeof(Resources.Resource))]
        public string HomePhone { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "WorkPhone", ResourceType = typeof(Resources.Resource))]
        public string WorkPhone { get; set; }

        public bool IsSMS { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "MobilePhone", ResourceType = typeof(Resources.Resource))]
        public string MobilePhone { get; set; }

        public bool IsEmail { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "EMail", ResourceType = typeof(Resources.Resource))]
        public string EMail { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Address", ResourceType = typeof(Resources.Resource))]
        public string HomeAddress { get; set; }

        [Display(Name = "City", ResourceType = typeof(Resources.Resource))]
        public int HomeCityParameterID { get; set; }

        [Display(Name = "Town", ResourceType = typeof(Resources.Resource))]
        public int HomeTownParameterID { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        [Display(Name = "ZipCode", ResourceType = typeof(Resources.Resource))]
        public string HomeZipCode { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Address", ResourceType = typeof(Resources.Resource))]
        public string WorkAddress { get; set; }

        [Display(Name = "City", ResourceType = typeof(Resources.Resource))]
        public int WorkCityParameterID { get; set; }

        [Display(Name = "Town", ResourceType = typeof(Resources.Resource))]
        public int WorkTownParameterID { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        [Display(Name = "ZipCode", ResourceType = typeof(Resources.Resource))]
        public string WorkZipCode { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "Name", ResourceType = typeof(Resources.Resource))]
        public string Name3 { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "IdNumber", ResourceType = typeof(Resources.Resource))]
        public string IdNumber3 { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "HomePhone", ResourceType = typeof(Resources.Resource))]
        public string HomePhone3 { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "WorkPhone", ResourceType = typeof(Resources.Resource))]
        public string WorkPhone3 { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        [Display(Name = "MobilePhone", ResourceType = typeof(Resources.Resource))]
        public string MobilePhone3 { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Address", ResourceType = typeof(Resources.Resource))]
        public string Address3 { get; set; }

        [Display(Name = "City", ResourceType = typeof(Resources.Resource))]
        public int CityParameterID3 { get; set; }

        [Display(Name = "Town", ResourceType = typeof(Resources.Resource))]
        public int TownParameterID3 { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        [Display(Name = "ZipCode", ResourceType = typeof(Resources.Resource))]
        public string ZipCode3 { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        [Display(Name = "Note", ResourceType = typeof(Resources.Resource))]
        public string Note { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        public string GuarantorId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string GuarantorName { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        public string GuarantorPhone { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string GuarantorAddress { get; set; }

        public int GuarantorCityParameterID { get; set; }
        public int GuarantorTownParameterID { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string GuarantorZipCode { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string GuarantorOther { get; set; }



        [Column(TypeName = "nvarchar(50)")]
        public string DebtorName { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string DebtorPlaceOfBirth { get; set; }

        [Column(TypeName = "date")]
        public Nullable<DateTime> DebtorDOB { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string DebtorFatherName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string DebtorAddress { get; set; }

        public int DebtorCityID { get; set; }
        public int DebtorTownID { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string DebtorZipCode { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        public string DebtorMobilePhone { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        public string DebtorHomePhone { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        public string DebtorWorkPhone { get; set; }



        [Column(TypeName = "nvarchar(30)")]
        public string CardNameOnCard1 { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        public string CardBankName1 { get; set; }
        public bool CardType1 { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string CardNumber1 { get; set; }

        [Column(TypeName = "nvarchar(02)")]
        public string CardExpiryMonth1 { get; set; }

        [Column(TypeName = "nvarchar(04)")]
        public string CardExpiryYear1 { get; set; }

        [Column(TypeName = "nvarchar(04)")]
        public string CardExpiryCVC1 { get; set; }

        [Column(TypeName = "nvarchar(02)")]
        public string CardAccountCuttingDay1 { get; set; }


        [Column(TypeName = "nvarchar(30)")]
        public string CardNameOnCard2 { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        public string CardBankName2 { get; set; }
        public bool CardType2 { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string CardNumber2 { get; set; }

        [Column(TypeName = "nvarchar(02)")]
        public string CardExpiryMonth2 { get; set; }

        [Column(TypeName = "nvarchar(04)")]
        public string CardExpiryYear2 { get; set; }

        [Column(TypeName = "nvarchar(04)")]
        public string CardExpiryCVC2 { get; set; }

        [Column(TypeName = "nvarchar(02)")]
        public string CardAccountCuttingDay2 { get; set; }


        public bool isEmpty
        {
            get
            {
                return (
                        (string.IsNullOrWhiteSpace(ParentPicture) || ParentPicture == "default.png") &&
                        string.IsNullOrWhiteSpace(IdNumber) &&
                        string.IsNullOrWhiteSpace(HomePhone) &&
                        string.IsNullOrWhiteSpace(WorkPhone) &&
                        string.IsNullOrWhiteSpace(MobilePhone) &&
                        string.IsNullOrWhiteSpace(EMail) &&
                        string.IsNullOrWhiteSpace(HomeAddress) &&
                        string.IsNullOrWhiteSpace(HomeZipCode) &&
                        string.IsNullOrWhiteSpace(WorkAddress) &&
                        string.IsNullOrWhiteSpace(WorkZipCode) &&

                        string.IsNullOrWhiteSpace(Name3) &&
                        string.IsNullOrWhiteSpace(IdNumber3) &&
                        string.IsNullOrWhiteSpace(IdNumber3) &&
                        string.IsNullOrWhiteSpace(WorkPhone3) &&
                        string.IsNullOrWhiteSpace(MobilePhone3) &&
                        string.IsNullOrWhiteSpace(Address3) &&
                        string.IsNullOrWhiteSpace(ZipCode3) &&
                        string.IsNullOrWhiteSpace(Note) &&

                        string.IsNullOrWhiteSpace(DebtorName) &&
                        string.IsNullOrWhiteSpace(DebtorPlaceOfBirth) &&
                        string.IsNullOrWhiteSpace(DebtorFatherName) &&
                        string.IsNullOrWhiteSpace(DebtorAddress) &&
                        string.IsNullOrWhiteSpace(DebtorZipCode) &&
                        string.IsNullOrWhiteSpace(DebtorMobilePhone) &&
                        string.IsNullOrWhiteSpace(DebtorHomePhone) &&
                        string.IsNullOrWhiteSpace(DebtorWorkPhone) &&
                        DebtorCityID == 0 &&
                        DebtorTownID == 0 &&

                        string.IsNullOrWhiteSpace(CardNameOnCard1) &&
                        string.IsNullOrWhiteSpace(CardBankName1) &&
                        string.IsNullOrWhiteSpace(CardNumber1) &&
                        string.IsNullOrWhiteSpace(CardExpiryMonth1) &&
                        string.IsNullOrWhiteSpace(CardExpiryYear1) &&
                        string.IsNullOrWhiteSpace(CardNameOnCard1) &&
                        string.IsNullOrWhiteSpace(CardExpiryCVC1) &&
                             string.IsNullOrWhiteSpace(CardAccountCuttingDay1) &&
                        CardType1 == true && CardType1 == false &&

                        string.IsNullOrWhiteSpace(CardNameOnCard2) &&
                        string.IsNullOrWhiteSpace(CardBankName2) &&
                        string.IsNullOrWhiteSpace(CardNumber2) &&
                        string.IsNullOrWhiteSpace(CardExpiryMonth2) &&
                        string.IsNullOrWhiteSpace(CardExpiryYear2) &&
                        string.IsNullOrWhiteSpace(CardNameOnCard2) &&
                        string.IsNullOrWhiteSpace(CardExpiryCVC2) &&
                        string.IsNullOrWhiteSpace(CardAccountCuttingDay2) &&
                        CardType2 == true && CardType2 == false &&

                        KinshipCategoryID == 0 &&
                        ProfessionCategoryID == 0 &&
                        HomeCityParameterID == 0 &&
                        HomeTownParameterID == 0 &&
                        WorkCityParameterID == 0 &&
                        WorkTownParameterID == 0 &&
                        CityParameterID3 == 0 &&
                        TownParameterID3 == 0);
            }
        }
    }
}
