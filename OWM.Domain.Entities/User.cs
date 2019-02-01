namespace OWM.Domain.Entities
{
    public class User : BaseAuditClass
    {
        public string FirstName { get; private set; }
        public string SurName { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }

        public User(string firstName, string surName, string username, string password)
        {
            FirstName = firstName;
            SurName = surName;
            Username = username;
            Password = password;
        }
    }
}
