using TPA_Desktop.Core.Models;

namespace TPA_Desktop.Core.Facades
{
    public static class Authentication
    {
        public static Employee Employee { get; private set; }

        public static void Login(Employee user)
        {
            Employee = user;
        }
    }
}