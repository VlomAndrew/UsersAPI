using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UsersAPI
{
    public class API : IDisposable
    {
        private UserContext dataSet;

        private string localPath;

        public string ServerPath
        {
            get { return localPath; }
            set { localPath = value; }
        }

        public string LoadPath { get; set; }

        private User currentUser = null;

        public API()
        {
            dataSet = new UserContext();
            
        }

        public void Register(UserInfo info)
        {
            try
            {
                var user = dataSet.users.FirstOrDefault(u => u.Email == info.Email);
                if (user != null)
                {
                    throw new RegistrExc("Пользователь с таким email уже существует");
                }

                user = dataSet.users.FirstOrDefault(u => u.Phone == info.Phone);
                if (user != null)
                {
                    throw new RegistrExc("Пользователь с таким phone уже существует");
                }
                user = new User(info);
                dataSet.users.Add(user);
                dataSet.SaveChanges();
            }
            catch (RegistrExc e)
            {
                throw;
            }
            catch (Exception e)
            {
               
                throw;
            }
        }

        public void Login(string login, string pass)
        {
            try
            {
                var user = dataSet.users.FirstOrDefault(u => u.Email == login || u.Phone == login);
                if (user == null)
                {
                    throw new RegistrExc("Не удалось найти пользователя с таким логином");
                }

                if (User.UnHash(user.PasswordHash) == pass)
                {
                    currentUser = user;
                }
                else
                {
                    throw new RegistrExc("Пароль неверный.");
                }
            }
            catch (RegistrExc e)
            {
                throw;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public string ShowCurrentUserInfo()
        {
            return currentUser?.ToString();
        }

        public void LoadFileToServer(string fileName)
        {

            if (currentUser == null)
            {
                throw new RegistrExc("Пользователь не залогинен");
            }
            var folder = localPath + "\\" + currentUser.GivenName + currentUser.Surname;

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            FileInfo info = new FileInfo(fileName);
            if (info.Exists)
            {
                info.CopyTo(folder + fileName.Substring(fileName.LastIndexOf('\\')), true);
            }
        }

        public List<string> GetAllUsersFiles()
        {
            if (currentUser == null)
            {
                throw new RegistrExc("Пользователь не залогинен");
            }
            var folder = localPath + "\\" + currentUser.GivenName + currentUser.Surname;
            if (!Directory.Exists(folder))
            {
                throw new FolderExc("У пользователя не загружено ни одного файла");
            }

            var dInfo = new DirectoryInfo(folder);

            var resList = new List<string>();

            foreach (var fInfo in dInfo.GetFiles())
            {
                resList.Add(fInfo.Name);
            }
            return resList;
        }

        public string GetServerFilePath(string fileName)
        {
            if (currentUser == null)
            {
                throw new RegistrExc("Пользователь не залогинен");
            }
            var folder = localPath + "\\" + currentUser.GivenName + currentUser.Surname;
            if (!Directory.Exists(folder))
            {
                throw new FolderExc("У пользователя не загружено ни одного файла");
            }

            var fPath = folder + "\\" + fileName;
            var fInfo = new FileInfo(fPath);
            if (fInfo.Exists)
            {
                return fPath;
            }
            else
            {
                throw new FolderExc("Такого файла не существует");
            }
        }

        public void LoadFileToPC(string fileName)
        {

            if (currentUser == null)
            {
                throw new RegistrExc("Пользователь не залогинен");
            }
            var folder = localPath + "\\" + currentUser.GivenName + currentUser.Surname;
            if (!Directory.Exists(folder))
            {
                throw new FolderExc("У пользователя не загружено ни одного файла");
            }
            var fPath = folder + "\\" + fileName;
            var fInfo = new FileInfo(fPath);
            if (fInfo.Exists)
            {
                try
                {
                    fInfo.CopyTo(ServerPath+"\\" + fileName, true);
                }
                catch (Exception e)
                {
                    throw;
                }
                
            }
            else
            {
                throw new FolderExc("Файл не существует");
            }

        }

        public void Dispose()
        {
            currentUser = null;
            dataSet?.Dispose();
        }
    }
}