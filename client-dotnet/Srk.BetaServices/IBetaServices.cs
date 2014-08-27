
namespace Srk.BetaServices
{
    using System;
    using System.Collections.Generic;

    public interface IBetaServices
    {
        string Language { get; set; }

        string[] GetAnnouncementSections();

        Announcement[] GetAnnouncements(string section);

        Announcement[] GetAnnouncements(string section, int limit);

        Announcement GetLatestAnnouncement();
        Announcement GetLatestAnnouncement(string section);

        void ReportCrash(ErrorReport report);
    }
}
