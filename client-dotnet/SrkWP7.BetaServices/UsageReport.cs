using System;
using System.Collections.Generic;

namespace Srk.BetaServices {

    public class UsageReport {

        public UsageReport() {

        }

        public List<PerUserReport> PerUser { get { return _perUser; } }
        private readonly List<PerUserReport> _perUser = new List<PerUserReport>();
        



    }
}
