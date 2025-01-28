using ESCHOOL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{
    public class SmsEmailRepository : ISmsEmailRepository
    {
        private SchoolDbContext _smsEmailContext;
        public SmsEmailRepository(SchoolDbContext smsEmailContext)
        {
            _smsEmailContext = smsEmailContext;
        }
        public SmsEmail GetSmsEmail(int studentID)
        {
            return _smsEmailContext.SmsEmail.Where(b => b.StudentID == studentID).FirstOrDefault();
        }

        public IEnumerable<SmsEmail> GetSmsEmailAll(int schoolID)
        {
            return _smsEmailContext.SmsEmail.Where(b => b.SchoolID == schoolID).ToList();
        }

        public bool CreateSmsEmail(SmsEmail student)
        {
            _smsEmailContext.Add(student);
            return Save();
        }
        public bool UpdateSmsEmail(SmsEmail student)
        {
            _smsEmailContext.Update(student);
            return Save();
        }
        public bool DeleteSmsEmail(SmsEmail student)
        {
            _smsEmailContext.Remove(student);
            return Save();
        }

        public bool Save()
        {
            var saved = _smsEmailContext.SaveChanges();
            return saved >= 0 ? true : false;
        }
    }
}
