CREATE DATABASE `CrimeCluster` 
CHARACTER SET 'utf8' 
COLLATE 'utf8_general_ci'; 

-- Table for Logs
-- Index on the Date Created for quick retrieval
CREATE TABLE IF NOT EXISTS `Log`
(
	`ID` CHAR(36) PRIMARY KEY NOT NULL, 
	`Message` VARCHAR(255) NOT NULL, 
	`Level` VARCHAR(5),
	`DateCreated` DATETIME NOT NULL,
	`Exception` VARCHAR(1023),
	INDEX `IX_DateCreated` (`DateCreated`)
);

-- Table for Grades of an Inciden Report
CREATE TABLE IF NOT EXISTS `IncidentGrading`
(
	`ID` CHAR(36) PRIMARY KEY NOT NULL, 
    `GradeValue` INT(2), 
    `Description` VARCHAR(255),
    `IsDeleted` TINYINT(1) NOT NULL
);

-- Table for Locations 
CREATE TABLE IF NOT EXISTS `Location`
(
	`ID` CHAR(36) PRIMARY KEY NOT NULL, 
    `Latitude` DECIMAL(10, 8) NOT NULL, 
    `Longitude` DECIMAL(10, 8) NOT NULL,
    `DateLogged` DATETIME NOT NULL,
    `IsDeleted` TINYINT(1) NOT NULL
);

-- Table for Officers and Dispatchers
-- FK_Officer_Incident associates with the incident the person is currently assigned too
-- FK_Officer_Location associates with the location the person is currently associated with 
CREATE TABLE IF NOT EXISTS `Person`
(
	`ID` CHAR(36) PRIMARY KEY NOT NULL, 
    `Title` VARCHAR(3) NOT NULL, 
    `FirstName` VARCHAR(255) NOT NULL, 
    `LastName` VARCHAR(255) NOT NULL, 
    `DOB` DATETIME NOT NULL, 
    `DateRegistered` DATETIME NOT NULL, 
    `Status` VARCHAR(32) NOT NULL,
    `BadgeNumber` VARCHAR(255) NOT NULL,
    `IsDeleted` TINYINT(1) NOT NULL,
    `IncidentID` CHAR(36) NULL, 
    `LocationID` CHAR(36) NULL
);

-- Table for an Indicent 
-- FK_Incident_Location associates a location with the incident 
-- FK_Incident_IncidentGrading associates the incident with a incident grading
CREATE TABLE `Incident`
(
	`ID` CHAR(36) PRIMARY KEY NOT NULL, 
    `Summary` VARCHAR(1023) NOT NULL, 
    `DateCreated` DATETIME NOT NULL,
    `LocationID` CHAR(36) NOT NULL, 
    `IncidentGradingID` CHAR(36) NOT NULL,
    `IsDeleted` TINYINT(1) NOT NULL,
    FOREIGN KEY `FK_Incident_Location` (`LocationID`) REFERENCES `Location`(`ID`),
    FOREIGN KEY `FK_Incident_IncidentGrading` (`IncidentGradingID`) REFERENCES `IncidentGrading`(`ID`)
);

-- Table for the Incident Outcome 
-- FK_IncidentOutcome_Incident associates an IncidentOutcome report to an Incident 
-- FK_IncidentOutcome_Officer associates an IncidentOutcome to the officer who reported it 
CREATE TABLE `IncidentOutcome`
(
	`ID` CHAR(36) PRIMARY KEY NOT NULL, 
    `DateCreated` DATETIME NOT NULL, 
    `Outcome` VARCHAR(1023) NOT NULL, 
    `IncidentID` CHAR(36) NOT NULL, 
    `OfficerID` CHAR(36) NOT NULL, 
    `IsDeleted` TINYINT(1) NOT NULL, 
    FOREIGN KEY `FK_IncidentOutcome_Incident` (`IncidentID`) REFERENCES `Incident`(`ID`), 
    FOREIGN KEY `FK_IncidentOutcome_Officer` (`OfficerID`) REFERENCES `Person`(`ID`)
);

-- Foreign Keys added after all Entities Created

-- Officer -> Location
ALTER TABLE `Person`
ADD CONSTRAINT `FK_Officer_Location` 
FOREIGN KEY (`LocationID`)
REFERENCES `Location`(`ID`);

-- Officer -> Incident
ALTER TABLE `Person`
ADD CONSTRAINT `FK_Officer_Incident` 
FOREIGN KEY (`IncidentID`) 
REFERENCES `Incident`(`ID`)
