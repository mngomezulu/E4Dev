using System.Collections.Generic;

namespace E4UsersMVCWebApp.Models
{
    public interface IUserRepository
    {
        List<UserModel> GetListOfUsers();
        List<UserModel> GetDataSetListOfUsers();
        UserModel? GetUserByID(int id);
        bool IsExistsNameLastname(string username, string lastname);
        void InsertUserModel(UserModel Student);
        void DeleteUserModel(int id);
        void EditUserModel(UserModel Student);
    }
}
