using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenges.Models
{
    public class GoalMetric
    {
        public virtual int GoalMetricId { get; set; }
        public virtual string GoalMetricDisplay { get; set; }
        public virtual string GoalMetricDbName { get; set; }
    }
}
