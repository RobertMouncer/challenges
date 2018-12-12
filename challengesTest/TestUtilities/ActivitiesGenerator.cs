using System.Collections.Generic;
using challenges.Models;

namespace challengesTest.TestUtilities
{
    public class ActivitiesGenerator
    {
        public static List<Activity> Create(int quantity)
        {
            List<Activity> list = new List<Activity>();
            for (var i = 0; i < quantity; i++)
            {
                CreateActivity(i + 1);
            }

            return list;
        }


        public static Activity CreateActivity(int id)
        {
            return new Activity
            {
                ActivityName = "TestActivity" + id,
                ActivityId = id
            };
        }
    }
}