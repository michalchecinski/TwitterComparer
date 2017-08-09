using System;
using System.Collections.Generic;
using TwitterComparerLibrary.Model;

namespace TwitterComparerLibrary
{
    public class Cache
    {
        private static List<CompareUsersResult> results = new List<CompareUsersResult>();

        public static void Add(CompareUsersResult result)
        {
            if (results.Contains(result))
            {
                var foundResult = results.Find(x => SameUserNames(x.FirstUser.ScreenName, x.SecondUser.ScreenName, result));
                foundResult.Update(result);
            }
            else
            {
                results.Add(result);
            }
        }

        public static CompareUsersResult Get(string firstUser, string secondUser)
        {
            foreach (var result in results)
            {
                if (SameUserNames(firstUser, secondUser, result) &&
                    result.LastUpdate >= DateTime.Now.AddMinutes(-16))
                {
                    return result;
                }
                    
            }

            return null;
        }

        private static bool SameUserNames(string firstUserName, string secondUserName, CompareUsersResult result)
        {
            if (result.FirstUser.ScreenName == firstUserName && result.SecondUser.ScreenName == secondUserName)
            {
                return true;
            }
            if (result.FirstUser.ScreenName == secondUserName && result.SecondUser.ScreenName == firstUserName)
            {
                return true;
            }
            return false;
        }
    }
}
