﻿
namespace Srk.BetaServices
{
    /// <summary>
    /// Basic class to keep track of the current version.
    /// </summary>
    public static class Version
    {
        /// <summary>
        /// Internal name of this library.
        /// </summary>
        public const string LibraryName = "Srk.BetaServices";

        /// <summary>
        /// Current version of this library.
        /// </summary>
        //VERSION: version number is hard-coded
        public const string LibraryVersion = "0.5.0.0";

        /// <summary>
        /// Computed user-agent for this library.
        /// </summary>
        internal const string LibraryUserAgent = LibraryName + "/" + LibraryVersion;
    }
}
