--
-- Database: `DWH`
--

SET AUTOCOMMIT=0;

START TRANSACTION;
-- --------------------------------------------------------

--
-- Table structure for table `CASE`
--

CREATE TABLE IF NOT EXISTS `CASE` (
  `case_id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `fact_id` int(11) unsigned DEFAULT NULL,
  PRIMARY KEY (`case_id`),
  KEY `fact_id` (`fact_id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=0;

-- --------------------------------------------------------


--
-- Table structure for table `DIM_DATETIME`
--

CREATE TABLE IF NOT EXISTS `DIM_DATETIME` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `content` varchar(255) DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=0;

-- --------------------------------------------------------

--
-- Table structure for table `DIM_TIME_DAY`
--

CREATE TABLE IF NOT EXISTS `DIM_TIME_DAY` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `content` varchar(255) DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  `month` int(10) unsigned DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `month` (`month`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=0;

-- --------------------------------------------------------

--
-- Table structure for table `DIM_TIME_MONTH`
--

CREATE TABLE IF NOT EXISTS `DIM_TIME_MONTH` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `content` varchar(255) DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  `year` int(10) unsigned DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `year` (`year`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=0;

-- --------------------------------------------------------

--
-- Table structure for table `DIM_TIME_YEAR`
--

CREATE TABLE IF NOT EXISTS `DIM_TIME_YEAR` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `content` varchar(255) DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=0;

-- --------------------------------------------------------

--
-- Table structure for table `EVENT`
--

CREATE TABLE IF NOT EXISTS `EVENT` (
  `event_id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `case_id` int(11) unsigned DEFAULT NULL,
  `event` varchar(255) DEFAULT NULL,
  `activity` varchar(255) DEFAULT NULL,
  `sequence` int(11) DEFAULT NULL,
  `timestamp` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`event_id`),
  KEY `case_id` (`case_id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=0;

-- --------------------------------------------------------

--
-- Table structure for table `DIM_PROCESS`
--

CREATE TABLE IF NOT EXISTS `DIM_PROCESS` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `content` varchar(255) DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=0;

-- --------------------------------------------------------

--
-- Table structure for table `FACT`
--

CREATE TABLE IF NOT EXISTS `FACT` (
  `fact_id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `process` int(10) unsigned DEFAULT NULL,
  `time` int(10) unsigned DEFAULT NULL,
  PRIMARY KEY (`fact_id`),
  KEY `process` (`process`),
  KEY `time` (`time`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=0;

-- --------------------------------------------------------

--
-- Constraints for table `EVENT`
--
ALTER TABLE `EVENT`
  ADD CONSTRAINT `EVENT_ibfk_1` FOREIGN KEY (`case_id`) REFERENCES `CASE` (`case_id`) ON DELETE CASCADE;

--
-- Constraints for table `CASE`
--
ALTER TABLE `CASE`
  ADD CONSTRAINT `CASE_ibfk_1` FOREIGN KEY (`fact_id`) REFERENCES `FACT` (`fact_id`) ON DELETE CASCADE;

--
-- Constraints for table `DIM_TIME_DAY`
--
ALTER TABLE `DIM_TIME_DAY`
  ADD CONSTRAINT `dim_time_day_ibfk_1` FOREIGN KEY (`month`) REFERENCES `DIM_TIME_MONTH` (`id`);

--
-- Constraints for table `DIM_TIME_MONTH`
--
ALTER TABLE `DIM_TIME_MONTH`
  ADD CONSTRAINT `dim_time_month_ibfk_1` FOREIGN KEY (`year`) REFERENCES `DIM_TIME_YEAR` (`id`);

--
-- Constraints for table `FACT`
--
ALTER TABLE `FACT`
  ADD CONSTRAINT `fact_ibfk_1` FOREIGN KEY (`process`) REFERENCES `DIM_PROCESS` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `fact_ibfk_2` FOREIGN KEY (`time`) REFERENCES `DIM_DATETIME` (`id`) ON DELETE CASCADE
  ;
  
-- --------------------------------------------------------
COMMIT;
