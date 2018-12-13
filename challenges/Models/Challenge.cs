using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace challenges.Models
{
    public class Challenge
    {
        public virtual int ChallengeId { get; set; }

        [Display(Name = "Start Date Time")]
        public virtual DateTime StartDateTime { get; set; }

        [Display(Name = "End Date Time")]
        public virtual DateTime EndDateTime { get; set; }

        [Range(0.001, double.MaxValue, ErrorMessage = "The goal value must be greater than 0")]
        public virtual double Goal { get; set; }

        public virtual GoalMetric GoalMetric { get; set; }

        [Display(Name = "Goal Metric")]
        public virtual int GoalMetricId { get; set; }

        public virtual Activity Activity {get; set;}

        [Display(Name = "Activity")]
        public virtual int ActivityId { get; set; }

        [Display(Name = "Is Group Challenge?")]
        public virtual bool IsGroupChallenge { get; set; }

        [Display(Name = "Group")]
        public virtual string Groupid { get; set; }
    }
}
