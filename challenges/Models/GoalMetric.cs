using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace challenges.Models
{
    public class GoalMetric
    {
        public virtual int GoalMetricId { get; set; }

        [Display(Name = "Goal Metric Display Name")]
        public virtual string GoalMetricDisplay { get; set; }

        [Display(Name = "Goal Metric Database Key")]
        public virtual string GoalMetricDbName { get; set; }
    }
}
