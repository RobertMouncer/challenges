using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace challenges.Models
{
    public class Challenge
    {
        [Key]
        public virtual int ChallengeId { get; set; }
        [Required]
        public virtual DateTime StartDateTime { get; set; }
        [Required]
        public virtual DateTime EndDateTime { get; set; }
        [Required]
        public virtual int Goal { get; set; }
        [Required]
        public virtual bool Repeat { get; set; }
        
        public virtual Activity Activity {get; set;}
        [Required]
        public virtual int ActivityId { get; set; }
        [Required]
        public virtual bool IsGroupChallenge { get; set; }
        public virtual int Groupid { get; set; }
    }
}
