using System;
using System.Collections.Generic;

namespace Srk.BetaServices {
    public interface IBetaServices : IDisposable {

        string Language { get; set; }

        void GetAnnouncementSectionsAsync();

        event AsyncResponseHandler<string[]> GetAnnouncementSectionsEnded;

        void GetAnnouncementsAsync(string section);
        void GetAnnouncementsAsync(string section, uint limit);

        event AsyncResponseHandler<Announcement[]> GetAnnouncementsEnded;

        void GetLatestAnnouncementAsync();
        void GetLatestAnnouncementAsync(string section);

        event AsyncResponseHandler<Announcement> GetLatestAnnouncementEnded;

        void ReportCrashAsync(ErrorReport report);

        event AsyncResponseHandler ReportCrashEnded;
    }
}
