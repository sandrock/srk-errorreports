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

        void ReportUsageAsync(string username, DateTime day, Dictionary<string, uint> data);

        event AsyncResponseHandler ReportUsageEnded;

        void ContactAsync(string username, string email, string message);

        event AsyncResponseHandler ContactEnded;

        void ReportVariousAsync(string message);

        event AsyncResponseHandler ReportVariousEnded;

        void ReportShowAsync(string showUrl, string category, string username);

        event AsyncResponseHandler ReportShowEnded;

        void GetShowsByCategoryAsync(string category);

        event AsyncResponseHandler<string[]> GetShowsByCategoryEnded;

    }
}
