using ESCHOOL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ESCHOOL.Services
{

    public class UsersChatRepository : IUsersChatRepository
    {
        private CustomersDbContext _usersChatContext;
        public UsersChatRepository(CustomersDbContext usersChatContext)
        {
            _usersChatContext = usersChatContext;
        }

        IEnumerable<UsersChat> IUsersChatRepository.GetUsersChatAll(int schoolCode, bool isArchive)
        {
            return _usersChatContext.UsersChat.Where(c=> c.SchoolCode == schoolCode && c.IsArchive == isArchive).OrderByDescending(b=> b.ChatDate).ToList();
        }

        IEnumerable<UsersChat> IUsersChatRepository.GetUsersMessages(int schoolCode, int userID, int chatUserID, bool isArchive)
        {
            return _usersChatContext.UsersChat.Where(b => b.SchoolCode == schoolCode && b.ChatUserID1 == userID && b.ChatUserID2 == chatUserID && b.IsArchive == isArchive).ToList(); 
        }

        IEnumerable<UsersChat> IUsersChatRepository.GetUsersMessagesReceive(int schoolCode, int userID, bool isArchive)
        {
            return _usersChatContext.UsersChat.Where(b => b.SchoolCode == schoolCode && b.ChatUserID2 == userID && b.IsArchive == isArchive).ToList();
        }


        public UsersChat GetUsersChat(int schoolCode, int chatID)
        {
            return _usersChatContext.UsersChat.Where(b => b.SchoolCode == schoolCode && b.ChatID == chatID).FirstOrDefault();
        }
        public bool CreateUsersChat(UsersChat usersChat)
        {
            _usersChatContext.Add(usersChat);
            return Save();
        }
        public bool UpdateUsersChat(UsersChat usersChat)
        {
            _usersChatContext.Update(usersChat);
            return Save();
        }
        public bool DeleteUsersChat(UsersChat usersChat)
        {
            _usersChatContext.Remove(usersChat);
            return Save();
        }

        public bool ExistUsersChat(int schoolCode, int chatID)
        {
            return _usersChatContext.UsersChat.Any(c => c.SchoolCode == schoolCode && c.ChatID == chatID);
        }
        public bool Save()
        {
            var saved = _usersChatContext.SaveChanges();
            return saved >= 0 ? true : false;
        }

    }
}
