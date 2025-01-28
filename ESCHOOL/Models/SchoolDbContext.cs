using Castle.Core.Configuration;
using ESCHOOL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;

namespace ESCHOOL.Models
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options)
        {

        }

        public DbSet<SchoolInfo> SchoolInfo { get; set; }
        public DbSet<SchoolBusServices> SchoolBusServices { get; set; }
        public DbSet<PSerialNumber> PSerialNumber { get; set; }
        public DbSet<SchoolFee> SchoolFee { get; set; }
        public DbSet<SchoolFeeTable> SchoolFeeTable { get; set; }
        public DbSet<Classroom> Classroom { get; set; }
        public DbSet<DiscountTable> DiscountTable { get; set; }
        public DbSet<StudentDiscount> StudentDiscount { get; set; }
        public DbSet<Parameter> Parameter { get; set; }
        public DbSet<Bank> Bank { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<StudentPeriods> StudentPeriods { get; set; }
        public DbSet<StudentAddress> StudentAddress { get; set; }
        public DbSet<StudentParentAddress> StudentParentAddress { get; set; }
        public DbSet<StudentFamilyAddress> StudentFamilyAddress { get; set; }
        public DbSet<StudentInvoiceAddress> StudentInvoiceAddress { get; set; }
        public DbSet<StudentInvoice> StudentInvoice { get; set; }
        public DbSet<StudentInvoiceDetail> StudentInvoiceDetail { get; set; }
        public DbSet<StudentNote> StudentNote { get; set; }
        public DbSet<StudentDebt> StudentDebt { get; set; }
        public DbSet<StudentDebtDetail> StudentDebtDetail { get; set; }
        public DbSet<StudentDebtDetailTable> StudentDebtDetailTable { get; set; }
        public DbSet<StudentPayment> StudentPayment { get; set; }
        public DbSet<StudentInstallment> StudentInstallment { get; set; }
        public DbSet<StudentInstallmentPayment> StudentInstallmentPayment { get; set; }
        public DbSet<TempData> TempData { get; set; }
        public DbSet<TempPlan> TempPlan { get; set; }
        public DbSet<StudentTemp> StudentTemp { get; set; }

        public DbSet<AccountCodes> AccountCodes { get; set; }
        public DbSet<AccountCodesDetail> AccountCodesDetail { get; set; }
        public DbSet<Accounting> Accounting { get; set; }
        public DbSet<SmsEmail> SmsEmail { get; set; }
        public DbSet<UsersLog> UsersLog { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Absenteeism> Absenteeism { get; set; }
        public DbSet<UsersWorkAreas> UsersWorkAreas { get; set; }
        public DbSet<MultipurposeList> MultipurposeList { get; set; }
        public DbSet<ExcelData> ExcelData { get; set; }

        public DbSet<StudentTaskDataSource> StudentTaskDataSource { get; set; }
        public DbSet<UsersTaskDataSource> UsersTaskDataSource { get; set; }
        public DbSet<SchoolsTaskDataSource> SchoolsTaskDataSource { get; set; }
        public DbSet<TempM101Header> TempM101Header { get; set; }
        public DbSet<TempM101> TempM101 { get; set; }
    }
}
