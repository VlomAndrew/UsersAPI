using System;

namespace UsersAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            API api = new API();
            //try
            //{
            //    api.Register(new UserInfo { Email = "abc@mail.ru", GivenName = "Andrew", MiddleName = "Karpov", Surname = "B", Phone = "1111-22", Password = "123" });
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //    throw;
            //}

            try
            {
                api.Login("1111-22", "123");
                System.Console.WriteLine(api.ShowCurrentUserInfo());
                //api.LoadFileToServer(@"C:\Users\andre\OneDrive\Рабочий стол\11.txt");
                var li = api.GetAllUsersFiles();
                foreach (var f in li)
                {
                 Console.WriteLine(f);   
                }
                api.LoadPath = string.Format($"c:\\users\\{Environment.UserName}\\Downloads");
                Console.WriteLine(api.GetServerFilePath("11.txt"));
                api.LoadFileToPC("11.txt");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}
