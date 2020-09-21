using System;

namespace UsersAPI
{
    public class RegistrExc : Exception
    {
        public RegistrExc(string msg) : base(msg)
        {

        }
    }

    public class FolderExc : Exception
    {
        public FolderExc(string msg) : base(msg)
        {

        }
    }
}