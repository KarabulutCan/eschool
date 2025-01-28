using ESCHOOL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ESCHOOL.ViewModels
{
    public class StudentViewModel
    {
        public int StudentInstallmentID { get; set; }

        public int ViewModelID { get; set; }
        public string Period { get; set; }
        public int TaskTypeID { get; set; }
        public int SchoolID { get; set; }
        public string SchoolName { get; set; }
        public int StudentID { get; set; }
        public int UserID { get; set; }

        //Collection Card print
        public int SelectedSchoolCode { get; set; }
        public int RegistrationTypeSubID { get; set; }
        public int StatuCategorySubID { get; set; }
        public int StatuCategoryID { get; set; }
        public int ClassroomID { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public string wwwRootPath { get; set; }
        //

        //BatchInvoice
        public string MaxPrint { get; set; }
        public int TotalInvoice { get; set; }
        public int StartInvoiceSerialNumber { get; set; }
        public int EndInvoiceSerialNumber { get; set; }
        public int NowInvoiceSerialNumber { get; set; }
        public string StudentName { get; set; }

        public bool ViewIsNew { get; set; }
        public bool ViewIsFirst { get; set; }
        public int IsMenu { get; set; }
        public Student Student { get; set; }
        public StudentAddress StudentAddress { get; set; }
        public StudentParentAddress StudentParentAddress { get; set; }
        public StudentFamilyAddress StudentFamilyAddress { get; set; }
        public StudentInvoiceAddress StudentInvoiceAddress { get; set; }
        public StudentNote StudentNote { get; set; }
        public StudentTemp StudentTemp { get; set; }
        public StudentDebt StudentDebt { get; set; }
        public StudentInstallment StudentInstallment { get; set; }
        public IEnumerable<StudentInstallment> StudentInstallment2 { get; set; }
        public StudentPayment StudentPayment { get; set; }

        public SchoolInfo SchoolInfo { get; set; }
        public Bank Bank { get; set; }
        public Classroom Classroom { get; set; }
        public DiscountTable DiscountTable { get; set; }
        public Users Users { get; set; }

        public PSerialNumber PSerialNumber { get; set; }
        public IEnumerable<SchoolFeeTable> SchoolFeeTable { get; set; }
        public IEnumerable<Parameter> Parameter { get; set; }
        public Parameter Parameter2 { get; set; }
        public Parameter Parameter3 { get; set; }

        public string StudentPicture { get; set; }
        public string Gender { get; set; }
        public string Name { get; set; }
        public string StudentClassroom { get; set; }
        public string StatuCategory { get; set; }
        public string RegistrationTypeCategory { get; set; }

        public decimal CutInvoice { get; set; }
        public decimal UncutInvoice { get; set; }

        public string StudentNumber { get; set; }
        public string StudentSerialNumber { get; set; }
        public string IdNumber { get; set; }
        public decimal Total { get; set; }
        public decimal TotalDebt { get; set; }
        public decimal Balance { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public Nullable<DateTime> DateOfRegistration { get; set; }
        public string ParentName { get; set; }

        public bool IsActive { get; set; }

        public string SiblingName { get; set; }
        public int SiblingID { get; set; }
        public bool IsPermission { get; set; }
        public bool IsPermissionDiscount { get; set; }
        public bool IsPermissionCancel { get; set; }
        public bool IsPermissionFee { get; set; }
        public string SelectedCulture { get; set; }
        public string IsPension { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsSuccess2 { get; set; }
        public bool IsSuccess3 { get; set; }
        public bool IsLoop { get; set; }
        public int SortType { get; set; }
        public Nullable<bool> SortOption { get; set; }
        public string SortIcon { get; set; }
        public bool IsExplanationShow { get; set; }

        public string CategoryName1 { get; set; }
        public string CategoryName2 { get; set; }
        public string CategoryName3 { get; set; }
        public string InvoiceTaxNumber { get; set; }
        
    }
}
