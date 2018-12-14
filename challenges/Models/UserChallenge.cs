using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace challenges.Models
{
    public class UserChallenge
    {
        public virtual int UserChallengeId { get; set; }
        public virtual string UserId { get; set; }
        public virtual Challenge Challenge { get; set; }
        public virtual int ChallengeId { get; set; }
        public virtual int PercentageComplete { get; set; }
        public virtual bool EmailSent { get; set; }
    }
}
