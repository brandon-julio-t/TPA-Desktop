using TPA_Desktop.Models;
using TPA_Desktop.Models.Abstract;

namespace TPA_Desktop.Facades
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