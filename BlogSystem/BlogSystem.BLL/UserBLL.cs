using BlogSystem.DAL;
using BlogSystem.DTO;
using BlogSystem.IBLL;
using BlogSystem.IDAL;
using BlogSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.BLL
{
    public class UserBLL: IUserBLL
    {
        private IUserDAL _userDal = new UserDAL();
        
        public bool ChangePassword(string email, string oldPassword, string newPassword)
        {
            User user = _userDal.GetUserByEmail(email);
            if(user != null && user.UserPassword == MD5Encryption64(oldPassword))
            {
                user.UserPassword = MD5Encryption64(newPassword);
                return _userDal.EditEntity(user);
            }
            else
            {
                return false;
            }
        }

        public bool ChangeUserInfomation(string email, string blogName, string imagePath)
        {
            User user = _userDal.GetUserByEmail(email);
            if (user != null)
            {
                user.UserBlogName = blogName;
                user.UserImagePath = imagePath;
                return _userDal.EditEntity(user);
            }
            else
            {
                return false;
            }
        }

        public UserInformationDTO GetUserInformation(string email)
        {
            User user = _userDal.GetUserByEmail(email);
            return new UserInformationDTO() {
                Email = user.UserEmail,
                ImagePath = user.UserImagePath,
                BlogName = user.UserBlogName,
                FollowerCount = user.UserFollowerCount,
                FollowingCount = user.UserFollowingCount
            };
        }

        public bool Login(string email, string password, out Guid userId)
        {
            User user = _userDal.GetUserByEmail(email);
            if(user != null)
            {
                if (user.UserPassword == MD5Encryption64(password))
                {
                    userId = user.UserId;
                    return true;
                }
                else
                {
                    // wrong password
                    userId = new Guid();
                    return false;
                }
            }
            else
            {
                // user does not exist
                userId = new Guid();
                return false;
            }
            
        }

        public bool Register(string email, string password)
        {
            return _userDal.CreateEntity(new User()
            {
                UserId = Guid.NewGuid(),
                UserCreateDateTime = DateTime.Now,
                // 0: no, 1: yes
                UserIsRemoved = 0,
                UserEmail = email,
                // MD5 encrypt user's password
                UserPassword = MD5Encryption64(password),
                UserImagePath = "default.jpg",
                UserFollowerCount = 0,
                UserFollowingCount = 0,
                UserBlogName = email.Split(new char[] { '@' })[0],
            }) ;
        }

        /// <summary>
        /// encrypt user's password, after that, store encrypted password in database
        /// </summary>
        /// <param name="password"> raw password </param>
        /// <returns> encrypted password </returns>
        private string MD5Encryption64(string password)
        {
            MD5 mD5 = MD5.Create();
            byte[] s = mD5.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(s);
        }
    }
}
