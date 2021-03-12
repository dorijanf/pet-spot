using System;

namespace PetSpot.DATA.Exceptions
{
    public class NotAuthorizedException : Exception
    {
        public NotAuthorizedException(string message) : base(message)
        {

        }
    }
}
