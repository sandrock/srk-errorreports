-- MySQL dump 10.11
--
-- Host: localhost    Database: srkerrorreports
-- ------------------------------------------------------
-- Server version	5.0.51a-24+lenny5-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `AnnouncementTexts`
--

DROP TABLE IF EXISTS `AnnouncementTexts`;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
CREATE TABLE `AnnouncementTexts` (
  `AnnouncementId` int(10) unsigned NOT NULL,
  `Language` varchar(5) NOT NULL,
  `Content` text NOT NULL,
  PRIMARY KEY  (`AnnouncementId`,`Language`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `Announcements`
--

DROP TABLE IF EXISTS `Announcements`;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
CREATE TABLE `Announcements` (
  `AnnouncementId` int(10) unsigned NOT NULL auto_increment,
  `Source` varchar(80) NOT NULL,
  `Date` datetime NOT NULL,
  `Status` tinyint(4) NOT NULL default '0',
  `Author` varchar(80) NOT NULL,
  `Section` varchar(32) NOT NULL,
  PRIMARY KEY  (`AnnouncementId`),
  KEY `Section` (`Section`)
) ENGINE=MyISAM AUTO_INCREMENT=13 DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `ErrorReports`
--

DROP TABLE IF EXISTS `ErrorReports`;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
CREATE TABLE `ErrorReports` (
  `ReportId` bigint(20) unsigned NOT NULL auto_increment,
  `Date` datetime NOT NULL,
  `AppStartTime` datetime default NULL,
  `AppErrorTime` datetime NOT NULL,
  `AppExitTime` datetime default NULL,
  `AssemblyName` varchar(255) NOT NULL,
  `AssemblyVersion` varchar(32) NOT NULL,
  `Culture` varchar(24) NOT NULL,
  `DeploymentKind` varchar(32) NOT NULL,
  `DeploymentComment` varchar(64) default NULL,
  `OSPlatform` varchar(64) NOT NULL,
  `OSVersion` varchar(64) NOT NULL,
  `DeviceManufacturer` varchar(128) default NULL,
  `DeviceName` varchar(128) default NULL,
  `DeviceId` varchar(128) default NULL,
  `UserId` varchar(255) default NULL,
  `DeviceTotalMemory` bigint(20) default NULL,
  `AppCurrentMemoryUsage` bigint(20) default NULL,
  `AppPeakMemoryUsage` bigint(20) default NULL,
  `Comment` varchar(255) default NULL,
  `ExceptionType` varchar(255) NOT NULL,
  `ExceptionMessage` varchar(255) NOT NULL,
  `ExceptionTrace` text,
  `FullException` text,
  `UserAgent` varchar(255) NOT NULL,
  `ClientIPAddress` varchar(40) NOT NULL,
  `Handling` tinyint(4) NOT NULL default '0',
  `HttpRequest` text,
  `HttpReferer` text,
  `HttpMethod` varchar(32) default NULL,
  `HttpHost` varchar(255) default NULL,
  PRIMARY KEY  (`ReportId`)
) ENGINE=MyISAM AUTO_INCREMENT=447 DEFAULT CHARSET=utf8;
SET character_set_client = @saved_cs_client;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2012-01-07 22:03:38
