using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using challenges.Migrations;
using challenges.Models;
using challenges.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using YourApp.Services;

namespace challenges.Controllers.shared
{
    public class SharedFunctionality
    {
        private static IUserChallengeRepository _userChallengeRepository;
        private static IApiClient _apiClient;
        
        public static void Init(IUserChallengeRepository userChallengeRepository, IApiClient apiClient)
        {
            _userChallengeRepository = userChallengeRepository;
            _apiClient = apiClient;
        }

        public static async void UpdatePercentageListAsync(List<UserChallenge> userChallenges)
        {
            foreach(var c in userChallenges)
            {
                var userData = await _apiClient.GetAsync("https://docker2.aberfitness.biz/health-data-repository/api/Activities/ByUser/" 
                                                      + c.UserId + "?from=" + c.Challenge.StartDateTime.Date + "&to=" + DateTime.Now.Date);

                var userDataResult = userData.Content.ReadAsStringAsync().Result;

                await UpdatePercentageCompleteAsync(c, userDataResult);

            }
        }
        
        //this is also awful, please change
        public static async Task<UserChallenge> UpdatePercentageCompleteAsync(UserChallenge userChallenge, string userDataString)
        {
            if(userDataString == "[]" || userDataString == null)
            {
                return userChallenge;
            }
            dynamic dataString = JsonConvert.DeserializeObject(userDataString);
            var progress = 0;

            foreach (var d in dataString)
            {
                if (d.activityTypeId == userChallenge.Challenge.Activity.DbActivityId)
                {
                    switch (userChallenge.Challenge.GoalMetric.GoalMetricDbName)
                    {
                        case "caloriesBurnt":
                            progress += d.caloriesBurnt;
                            break;
                        case "averageHeartRate":
                            progress += d.averageHeartRate;
                            break;
                        case "stepsTaken":
                            progress += d.stepsTaken;
                            break;
                        case "metresTravelled":
                            progress += d.metresTravelled;
                            break;
                        case "metresElevationGained":
                            progress += d.metresElevationGained;
                            break;
                        default:
                            progress = 0;
                            break;
                    }
                }
            }

            var percentageComplete = (progress / userChallenge.Challenge.Goal)*100;
            
            userChallenge.PercentageComplete = Math.Min(100, percentageComplete);

            await _userChallengeRepository.UpdateAsync(userChallenge);

            return userChallenge;
        }
    }
}