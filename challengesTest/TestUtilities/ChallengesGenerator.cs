using System;
using System.Collections.Generic;
using challenges.Models;

namespace challengesTest.TestUtilities
{
    public static class ChallengesGenerator
    {
        public static List<UserChallenge> Create(int quantity)
        {
            List<UserChallenge> list = new List<UserChallenge>();
            for (var i = 0; i < quantity; i++)
            {
                list.Add(CreateChallenge(i + 1));
            }

            return list;
        }

        public static UserChallenge CreateChallenge(int id)
        {
            var aId = 10 + id;
            var cId = 20 + id;
            var uId = 30 + id;
            return new UserChallenge
            {
                UserChallengeId = uId,
                UserId = "TestUid" + uId,
                Challenge = new Challenge
                {
                    ChallengeId = cId,
                    StartDateTime = new DateTime().Add(TimeSpan.FromDays(1)),
                    EndDateTime = new DateTime().Add(TimeSpan.FromDays(2)),
                    Goal = 1000 + cId,
                    GoalMetric = "TestGoalMetric",
                    Repeat = false,
                    Activity = new Activity
                    {
                        ActivityId = aId,
                        ActivityName = "TestActivityName" + aId
                    },
                    ActivityId = aId,
                    IsGroupChallenge = false,
                    Groupid = "1"
                },
                PercentageComplete = 0,
                ChallengeId = cId
            };
        }
    }
}