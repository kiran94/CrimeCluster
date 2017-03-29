CREATE DATABASE  IF NOT EXISTS `crimecluster` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `CrimeCluster`;
-- MySQL dump 10.13  Distrib 5.6.24, for osx10.8 (x86_64)
--
-- Host: localhost    Database: CrimeCluster
-- ------------------------------------------------------
-- Server version	5.5.42

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
-- Table structure for table `Incident`
--

DROP TABLE IF EXISTS `Incident`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Incident` (
  `ID` char(36) NOT NULL,
  `CrimeID` char(64) DEFAULT NULL,
  `DateCreated` datetime NOT NULL,
  `ReportedBy` varchar(255) NOT NULL,
  `FallsWithin` varchar(255) DEFAULT NULL,
  `LocationID` char(36) NOT NULL,
  `LocationDesc` varchar(255) NOT NULL,
  `LSOACode` varchar(9) NOT NULL,
  `LSOAName` varchar(31) NOT NULL,
  `CrimeType` varchar(31) NOT NULL,
  `LastOutcomeCategory` varchar(63) NOT NULL,
  `Context` varchar(255) DEFAULT NULL,
  `IncidentGradingID` char(36) DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `FK_Incident_Location` (`LocationID`),
  KEY `FK_Incident_IncidentGrading` (`IncidentGradingID`),
  KEY `IX_DateCreated` (`DateCreated`),
  CONSTRAINT `incident_ibfk_1` FOREIGN KEY (`LocationID`) REFERENCES `Location` (`ID`),
  CONSTRAINT `incident_ibfk_2` FOREIGN KEY (`IncidentGradingID`) REFERENCES `IncidentGrading` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `IncidentGrading`
--

DROP TABLE IF EXISTS `IncidentGrading`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `IncidentGrading` (
  `ID` char(36) NOT NULL,
  `GradeValue` int(2) DEFAULT NULL,
  `Description` varchar(255) DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `IncidentOutcome`
--

DROP TABLE IF EXISTS `IncidentOutcome`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `IncidentOutcome` (
  `ID` char(36) NOT NULL,
  `DateCreated` datetime NOT NULL,
  `Outcome` varchar(1023) NOT NULL,
  `IncidentID` char(36) NOT NULL,
  `OfficerID` char(36) NOT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `FK_IncidentOutcome_Incident` (`IncidentID`),
  KEY `FK_IncidentOutcome_Officer` (`OfficerID`),
  CONSTRAINT `incidentoutcome_ibfk_1` FOREIGN KEY (`IncidentID`) REFERENCES `Incident` (`ID`),
  CONSTRAINT `incidentoutcome_ibfk_2` FOREIGN KEY (`OfficerID`) REFERENCES `Person` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Location`
--

DROP TABLE IF EXISTS `Location`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Location` (
  `ID` char(36) NOT NULL,
  `Latitude` decimal(10,8) NOT NULL,
  `Longitude` decimal(10,8) NOT NULL,
  `DateLogged` datetime NOT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Person`
--

DROP TABLE IF EXISTS `Person`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Person` (
  `ID` char(36) NOT NULL,
  `Title` varchar(3) NOT NULL,
  `FirstName` varchar(255) NOT NULL,
  `LastName` varchar(255) NOT NULL,
  `DOB` datetime NOT NULL,
  `DateRegistered` datetime NOT NULL,
  `Status` varchar(32) NOT NULL,
  `BadgeNumber` varchar(255) NOT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  `IncidentID` char(36) DEFAULT NULL,
  `LocationID` char(36) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `FK_Officer_Location` (`LocationID`),
  KEY `FK_Officer_Incident` (`IncidentID`),
  CONSTRAINT `FK_Officer_Incident` FOREIGN KEY (`IncidentID`) REFERENCES `Incident` (`ID`),
  CONSTRAINT `FK_Officer_Location` FOREIGN KEY (`LocationID`) REFERENCES `Location` (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2017-03-29 11:00:04
