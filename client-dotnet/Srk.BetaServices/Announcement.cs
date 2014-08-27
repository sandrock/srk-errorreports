
namespace Srk.BetaServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.ComponentModel;

    /// <summary>
    /// Represent an announcement.
    /// </summary>
    public class Announcement
    {
        /// <summary>
        /// Data storage ID.
        /// Supposed to identify items.
        /// </summary>
        public int AnnouncementId { get; set; }

        /// <summary>
        /// Informative field.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Author of the message.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Date of publication.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Publication status.
        /// </summary>
        public short Status { get; set; }

        /// <summary>
        /// Message content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Section is used for targetting applications.
        /// </summary>
        public string Section { get; set; }

        /// <summary>
        /// Mark an announcement as read.
        /// </summary>
        public bool IsRead { get; set; }
    }
}
