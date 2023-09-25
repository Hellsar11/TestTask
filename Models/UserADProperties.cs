using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace TestTask.Models
{
    internal class UserADProperties
    {
        public string? Email { get; set; }
        public string? StaffNumber { get; set; }
        public string? Position { get; set; }
        public string? Department { get; set; }
        public string? NameLast { get; set; }
        public string? NameFirst { get; set; }
        public string? NameMiddle { get; set; }

        internal static UserADProperties? GetProperties(string login)
        {
            var oPC = new PrincipalContext(ContextType.Domain);
            var user = UserPrincipal.FindByIdentity(oPC, IdentityType.SamAccountName, login);
            var entry = (DirectoryEntry)user.GetUnderlyingObject();
            if (entry != null) {
                string[]? names = entry?.Properties["name"]?.Value?.ToString().Split(" ");
                var properties = new UserADProperties
                {
                    Email = entry?.Properties["mail"]?.Value?.ToString(),
                    StaffNumber = entry?.Properties["extensionAttribute9"]?.Value?.ToString(),
                    Position = entry?.Properties["department"]?.Value?.ToString(),
                    Department = entry?.Properties["description"]?.Value?.ToString(),
                    NameLast = names?.Length > 0 ? names[0] : "",
                    NameFirst = names?.Length > 1 ? names[1] : "",
                    NameMiddle = names?.Length > 2 ? names[2] : ""
                };
                return properties;
            }
            return null;
        }
    }
}
