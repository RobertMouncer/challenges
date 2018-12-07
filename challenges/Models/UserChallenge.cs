using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace challenges.Models
{
    public class UserChallenge
    {
        [Key]
        public virtual int UserChallengeId { get; set; }
        [Required]
        public virtual string UserId { get; set; }
        
        public virtual Challenge Challenge { get; set; }
        [Required]
        public virtual int ChallengeId { get; set; }
        [Required]
        public virtual int PercentageComplete { get; set; }
    }
}
