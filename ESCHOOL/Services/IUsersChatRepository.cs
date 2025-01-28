using ESCHOOL.Models;
using System.Collections.Generic;

namespace ESCHOOL.Services
{
    public interface IUsersChatRepository
    {
        IEnumerable<UsersChat> GetUsersChatAll(int schoolCode, bool isArchive);
        IEnumerable<UsersChat> GetUsersMessages(int schoolCode, int userID, int chatUserID, bool isArchive);
        IEnumerable<UsersChat> GetUsersMessagesReceive(int schoolCode, int userID, bool isArchive);
        UsersChat GetUsersChat(int schoolCode, int chatID);
        bool CreateUsersChat(UsersChat usersChat);
        bool UpdateUsersChat(UsersChat usersChat);
        bool DeleteUsersChat(UsersChat usersChat);
        bool ExistUsersChat(int schoolCode, int chatID);

        bool Save();
    }
}
