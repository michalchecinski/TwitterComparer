using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterComparerLibrary
{
    public class Cache
    {
        public string FirstUser { get; private set; }
        public string SecondUser { get; private set; }
        public DateTime UpdateDateTime { get; private set; }
        public List<User> UsersList { get; private set; }

        public void Update(string firstUser, string secondUser, List<User> usersList, DateTime updateDateTime)
        {
            FirstUser = firstUser;
            SecondUser = secondUser;
            UpdateDateTime = updateDateTime;
            UsersList = usersList;
        }

        public void Update(string firstUser, string secondUser, List<User> usersList)
        {
            UpdateDateTime = DateTime.Now;
            FirstUser = firstUser;
            SecondUser = secondUser;
            UsersList = usersList;
        }
    }
}
