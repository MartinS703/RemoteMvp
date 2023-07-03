using CsvHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteMvpApp
{
    public enum UserListActionResult
    {
        UserNotExisting,
        UserAlreadyExists,
        UserOkPasswordWrong,
        AccessGranted,
        AccessGrantedAsAdmin,
        RegistrationOk
    }

    public class Userlist
    {
        public record User(string UserName, string Password, bool admin);
        public readonly List<User> _users;
        private string _filePath;
        private string _loginUserName;

        /// <summary>
        /// just accept csvfile paths
        /// </summary>
        /// <param name="path"></param>
        public Userlist(string? path)
        {
            _users = new List<User>();
            // Check if filePath is a valid csv filepath
            if (!string.IsNullOrEmpty(path) && path.EndsWith(".csv"))
            {
                _filePath = path;
            }

            LoadUserData();
        }

        public void Delete(string username)
        {
            foreach (var user in _users)
            {
                if (user.UserName == username)
                {
                    _users.Remove(user);
                    break;
                }
            }
            StoreUserData();
            
        }

        public UserListActionResult LoginUser(string username, string password)
        {
            _loginUserName = username;
            foreach (var user in _users.Where(user => user.UserName.Equals(_loginUserName)))
            {
                if (user.Password.Equals(password))
                {

                    //
                    if (user.admin == true) {
                        return UserListActionResult.AccessGrantedAsAdmin;
                    }
                    else
                    {
                        return UserListActionResult.AccessGranted;
                    }
                    //

                }
                else
                {
                    return UserListActionResult.UserOkPasswordWrong;
                }
            }

            return UserListActionResult.UserNotExisting;
        }

        public UserListActionResult RegisterUser(string username, string password, bool admin = false)
        {
            if (_users.Any(user => user.UserName.Equals(username)))
            {
                return UserListActionResult.UserAlreadyExists;
            }

            User newUser = new(username, password, admin);
            _users.Add(newUser);

            StoreUserData();
            return UserListActionResult.RegistrationOk;
        }

        //public void RemoveUser(string username)
        //{
        //    _users.RemoveAll(user => user.UserName.Equals(username));
        //}

        //public void RemoveAllUsers()
        //{
        //    _users.Clear();
        //}

        public User? GetByUsername(string username)
        {
            foreach(var user in _users)
            {
                if(user.UserName == username)
                {
                    return user;
                }
            }
            return null;
        }

        private void StoreUserData()
        {
            using (var writer = new StreamWriter(_filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(_users); // List to store
            }

        }

        private void LoadUserData()
        {
            using (var reader = new StreamReader(_filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var record = csv.GetRecord<User>();
                    if (record != null)
                    {
                        _users.Add(record);
                    }
                }
            }
        }
    }
}
