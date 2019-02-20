using System;
namespace Property.Models
{
    public class UserModel
    {
        int _userId = 0;
        public int UserId { get { return _userId; } set { _userId = value; } }

        private string _firstName = "";
        public string FirstName { get { return _firstName ?? ""; } set { _firstName = value; } }

        private string _lastName = "";
        public string LastName { get { return _lastName ?? ""; } set { _lastName = value; } }

        private string _userName = "";
        public string UserName { get { return _userName; } set { _userName = value; } }

        private string _password = "";
        public string Password { get { return _password; } set { _password = value; } }

        private string _emailId = "";
        public string EmailId { get { return _emailId; } set { _emailId = value; } }

        private DateTime _createdOn = new DateTime(1900, 1, 1);
        public DateTime CreatedOn { get { return _createdOn; } set { _createdOn = value; } }

        private DateTime _lastUpdatedOn = new DateTime(1900, 1, 1);
        public DateTime LastUpdatedOn { get { return _lastUpdatedOn; } set { _lastUpdatedOn = value; } }

        private bool _isActive;
        public bool IsActive { get { return _isActive; } set { _isActive = value; } }
    }
}