using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmScore
{
    class Vote
    {
        public enum VoteIssuer
        {
            Jury, Public
        }
        public VoteIssuer Issuer { get; set; }
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public int Points { get; set; }
    }
}
