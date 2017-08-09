using System.Collections.Generic;

namespace TwitterComparerLibrary.Model
{
    public class CompareUsersResult
    {
        public User FirstUser { get; private set; }
        public User SecondUser { get; private set; }
        public IList<User> CommonFollowersList { get; private set; }
        public IList<User> CommonFriendsList { get; private set; }
        public int CommonFollowersNumber { get; private set; }
        public int CommonFriendsNumber { get; private set; }

        public CompareUsersResult(User firstUser, User secondUser, IList<User> commonFollowersList, IList<User> commonFriendsList, int commonFollowersNumber, int commonFriendsNumber)
        {
            FirstUser = firstUser;
            SecondUser = secondUser;
            CommonFollowersList = commonFollowersList;
            CommonFriendsList = commonFriendsList;
            CommonFollowersNumber = commonFollowersNumber;
            CommonFriendsNumber = commonFriendsNumber;
        }

        public CompareUsersResult(User firstUser, User secondUser, IList<User> commonFollowersList, IList<User> commonFriendsList)
        {
            FirstUser = firstUser;
            SecondUser = secondUser;
            CommonFollowersList = commonFollowersList;
            CommonFriendsList = commonFriendsList;
            CommonFollowersNumber = commonFollowersList.Count;
            CommonFriendsNumber = commonFriendsList.Count;
        }
    }
}
