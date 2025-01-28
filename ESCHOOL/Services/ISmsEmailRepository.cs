using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface ISmsEmailRepository
    {
        IEnumerable<SmsEmail> GetSmsEmailAll(int schoolID);
        SmsEmail GetSmsEmail(int studentID);

        bool CreateSmsEmail(SmsEmail smsEmail);
        bool UpdateSmsEmail(SmsEmail smsEmail);
        bool DeleteSmsEmail(SmsEmail smsEmail);

        bool Save();
    }
}
