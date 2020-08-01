using BlogSystem.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.IBLL
{
    public interface IUserBLL
    {
        bool Register(string email, string password);

        bool Login(string email, string password, out Guid userId);

        bool ChangePassword(string email, string oldPassword, string newPassword);

        bool ChangeUserInfomation(string email, string blogName, string imagePath);

        UserInformationDTO GetUserInformation(string email);
    }
}
