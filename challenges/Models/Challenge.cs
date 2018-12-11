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
        public virtual DateTime StartDateTime { get; set; }
        public virtual DateTime EndDateTime { get; set; }
        public virtual int Goal { get; set; }
        public virtual GoalMetric GoalMetric { get; set; }
        public virtual int GoalMetricId { get; set; }
        public virtual bool Repeat { get; set; }
        public virtual Activity Activity {get; set;}
        public virtual int ActivityId { get; set; }
        public virtual bool IsGroupChallenge { get; set; }
        public virtual string Groupid { get; set; }
    }
}
