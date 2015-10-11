
namespace AuditManager.Model.EFModel.AM
{
    public class User : PostFix
    {
        public int Id {get; set;} 
        public string Prefix {get; set;}
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }

    public class Client : CommonProperty
    {
        
    }
}
