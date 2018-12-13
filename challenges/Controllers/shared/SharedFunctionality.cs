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
        private static string _hdrUrl;
        
        public void Init(IUserChallengeRepository userChallengeRepository, IApiClient apiClient, string hdrUrl)
        {
            _userChallengeRepository = userChallengeRepository;
            _apiClient = apiClient;
            _hdrUrl = hdrUrl;
        }

        public async Task<List<UserChallenge>> UpdatePercentageListAsync(List<UserChallenge> userChallenges)
        {

            
            var todayDate = DateTime.Now;
            foreach (var c in userChallenges)
            {
                var challengeStartDate = c.Challenge.StartDateTime;
                var challengeEndDate = c.Challenge.EndDateTime;
                var dateSelected = todayDate;

                if(DateTime.Compare(challengeEndDate, todayDate) <= 0)
                {
                    dateSelected = challengeEndDate;
                }


                if (DateTime.Compare(challengeStartDate, todayDate) <= 0)
                {
                    var userData = await _apiClient.GetAsync(_hdrUrl+ "api/Activities/ByUser/"
                                                          + c.UserId + "?from=" + challengeStartDate.ToString("yyyy-MM-dd") + "&to=" + dateSelected.Date.ToString("yyyy-MM-dd"));
                    if (userData.IsSuccessStatusCode)
                    {
                        var userDataResult = userData.Content.ReadAsStringAsync().Result;

                        await UpdatePercentageCompleteAsync(c, userDataResult);
                    }

                }

            }
            return userChallenges;
        }
        
        //this is also awful, please change
        public async Task<UserChallenge> UpdatePercentageCompleteAsync(UserChallenge userChallenge, string userDataString)
        {
            if(userDataString == "[]" || userDataString == null)
            {
                userChallenge.PercentageComplete = 0;

                await _userChallengeRepository.UpdateAsync(userChallenge);
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
                            progress += (int)d.caloriesBurnt;
                            break;
                        case "averageHeartRate":
                            progress += (int)d.averageHeartRate;
                            break;
                        case "stepsTaken":
                            progress += (int)d.stepsTaken;
                            break;
                        case "metresTravelled":
                            progress += (int)d.metresTravelled;
                            break;
                        case "metresElevationGained":
                            progress += (int)d.metresElevationGained;
                            break;
                        default:
                            break;
                    }
                }
            }

            double percentageComplete = ((double)progress / (double)userChallenge.Challenge.Goal)*100;
            
            userChallenge.PercentageComplete = Math.Min(100, (int)percentageComplete);

            await _userChallengeRepository.UpdateAsync(userChallenge);

            return userChallenge;
        }
    }
}