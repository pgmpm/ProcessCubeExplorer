# ************************************************************
# Example Data for the Process Cube Explorer
# https://github.com/pgmpm/ProcessCubeExplorer
#
# Sequel Pro SQL dump
# Version 4096
#
# http://www.sequelpro.com/
# http://code.google.com/p/sequel-pro/
#
# Generation Time: 2014-03-17 18:51:47 +0000
# ************************************************************


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


# Dump of table CASE
# ------------------------------------------------------------

DROP TABLE IF EXISTS `CASE`;

CREATE TABLE `CASE` (
  `case_id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `fact_id` int(11) unsigned DEFAULT NULL,
  PRIMARY KEY (`case_id`),
  KEY `fact` (`fact_id`),
  CONSTRAINT `CASE_ibfk_1` FOREIGN KEY (`fact_id`) REFERENCES `FACT` (`fact_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

LOCK TABLES `CASE` WRITE;
/*!40000 ALTER TABLE `CASE` DISABLE KEYS */;

INSERT INTO `CASE` (`case_id`, `fact_id`)
VALUES
	(1,1),
	(21,1),
	(2,2),
	(22,2),
	(3,3),
	(23,3),
	(4,4),
	(24,4),
	(5,5),
	(25,5),
	(6,6),
	(26,6),
	(7,7),
	(27,7),
	(8,8),
	(28,8),
	(9,9),
	(29,9),
	(10,10),
	(30,10),
	(11,11),
	(31,11),
	(12,12),
	(32,12),
	(13,13),
	(33,13),
	(14,14),
	(34,14),
	(101,101),
	(121,101),
	(102,102),
	(122,102),
	(103,103),
	(123,103),
	(104,104),
	(124,104),
	(105,105),
	(125,105),
	(106,106),
	(126,106),
	(107,107),
	(127,107),
	(108,108),
	(128,108),
	(109,109),
	(129,109),
	(110,110),
	(130,110),
	(111,111),
	(131,111),
	(112,112),
	(132,112),
	(113,113),
	(133,113),
	(114,114),
	(134,114);

/*!40000 ALTER TABLE `CASE` ENABLE KEYS */;
UNLOCK TABLES;


# Dump of table DIM_AGE
# ------------------------------------------------------------

DROP TABLE IF EXISTS `DIM_AGE`;

CREATE TABLE `DIM_AGE` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `content` varchar(255) DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  `group_id` int(11) unsigned DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `age_group` (`group_id`),
  CONSTRAINT `age_group` FOREIGN KEY (`group_id`) REFERENCES `DIM_AGE_GROUP` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

LOCK TABLES `DIM_AGE` WRITE;
/*!40000 ALTER TABLE `DIM_AGE` DISABLE KEYS */;

INSERT INTO `DIM_AGE` (`id`, `content`, `description`, `group_id`)
VALUES
	(2,'0','0 Jahre',1),
	(3,'1','1 Jahre',1),
	(4,'2','2 Jahre',1),
	(5,'3','3 Jahre',1),
	(6,'4','4 Jahre',1),
	(7,'5','5 Jahre',2),
	(8,'6','6 Jahre',2),
	(9,'7','7 Jahre',2),
	(10,'8','8 Jahre',2),
	(11,'9','9 Jahre',2),
	(12,'10','10 Jahre',2),
	(13,'11','11 Jahre',3),
	(14,'12','12 Jahre',3),
	(15,'13','13 Jahre',3),
	(16,'14','14 Jahre',3),
	(17,'15','15 Jahre',3),
	(18,'16','16 Jahre',4),
	(19,'17','17 Jahre',4),
	(20,'18','18 Jahre',4),
	(21,'19','19 Jahre',4),
	(22,'20','20 Jahre',4),
	(23,'21','21 Jahre',5),
	(24,'22','22 Jahre',5),
	(25,'23','23 Jahre',5),
	(26,'24','24 Jahre',5),
	(27,'25','25 Jahre',6),
	(28,'26','26 Jahre',6),
	(29,'27','27 Jahre',6),
	(30,'28','28 Jahre',6),
	(31,'29','29 Jahre',6),
	(32,'30','30 Jahre',6),
	(33,'31','31 Jahre',7),
	(34,'32','32 Jahre',7),
	(35,'33','33 Jahre',7),
	(36,'34','34 Jahre',7),
	(37,'35','35 Jahre',7),
	(38,'36','36 Jahre',8),
	(39,'37','37 Jahre',8),
	(40,'38','38 Jahre',8),
	(41,'39','39 Jahre',8),
	(42,'40','40 Jahre',8),
	(43,'41','41 Jahre',9),
	(44,'42','42 Jahre',9),
	(45,'43','43 Jahre',9),
	(46,'44','44 Jahre',9),
	(47,'45','45 Jahre',9),
	(48,'46','46 Jahre',10),
	(49,'47','47 Jahre',10),
	(50,'48','48 Jahre',10),
	(51,'49','49 Jahre',10),
	(52,'50','50 Jahre',10),
	(53,'51','51 Jahre',11),
	(54,'52','52 Jahre',11),
	(55,'53','53 Jahre',11),
	(56,'54','54 Jahre',11),
	(57,'55','55 Jahre',11),
	(58,'56','56 Jahre',12),
	(59,'57','57 Jahre',12),
	(60,'58','58 Jahre',12),
	(61,'59','59 Jahre',12),
	(62,'60','60 Jahre',12),
	(63,'61','61 Jahre',13),
	(64,'62','62 Jahre',13),
	(65,'63','63 Jahre',13),
	(66,'64','64 Jahre',13),
	(67,'65','65 Jahre',13),
	(68,'66','66 Jahre',14),
	(69,'67','67 Jahre',14),
	(70,'68','68 Jahre',14),
	(71,'69','69 Jahre',14),
	(72,'70','70 Jahre',14),
	(73,'71','71 Jahre',15),
	(74,'72','72 Jahre',15),
	(75,'73','73 Jahre',15),
	(76,'74','74 Jahre',15),
	(77,'75','75 Jahre',15),
	(78,'76','76 Jahre',16),
	(79,'77','77 Jahre',16),
	(80,'78','78 Jahre',16),
	(81,'79','79 Jahre',16),
	(82,'80','80 Jahre',16),
	(83,'81','81 Jahre',17),
	(84,'82','82 Jahre',17),
	(85,'83','83 Jahre',17),
	(86,'84','84 Jahre',17),
	(87,'85','85 Jahre',17),
	(88,'86','86 Jahre',18),
	(89,'87','87 Jahre',18),
	(90,'88','88 Jahre',18),
	(91,'89','89 Jahre',18),
	(92,'90','90 Jahre',18),
	(93,'91','91 Jahre',19),
	(94,'92','92 Jahre',19),
	(95,'93','93 Jahre',19),
	(96,'94','94 Jahre',19),
	(97,'95','95 Jahre',19),
	(98,'96','96 Jahre',20),
	(99,'97','97 Jahre',20),
	(100,'98','98 Jahre',20),
	(101,'99','99 Jahre',20),
	(102,'100','100 Jahre',20);

/*!40000 ALTER TABLE `DIM_AGE` ENABLE KEYS */;
UNLOCK TABLES;


# Dump of table DIM_AGE_GROUP
# ------------------------------------------------------------

DROP TABLE IF EXISTS `DIM_AGE_GROUP`;

CREATE TABLE `DIM_AGE_GROUP` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `content` varchar(255) DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  `group_coarse_id` int(11) unsigned DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `age_group_coarse` (`group_coarse_id`),
  CONSTRAINT `age_group_coarse` FOREIGN KEY (`group_coarse_id`) REFERENCES `DIM_AGE_GROUP_COARSE` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

LOCK TABLES `DIM_AGE_GROUP` WRITE;
/*!40000 ALTER TABLE `DIM_AGE_GROUP` DISABLE KEYS */;

INSERT INTO `DIM_AGE_GROUP` (`id`, `content`, `description`, `group_coarse_id`)
VALUES
	(1,'0-5','0–5 Jahre',1),
	(2,'6–10','6–10 Jahre',1),
	(3,'11–15','11–15 Jahre',1),
	(4,'16–20','16–20 Jahre',1),
	(5,'21–25','21–25 Jahre',2),
	(6,'26–30','26–30 Jahre',2),
	(7,'31–35','31–35 Jahre',2),
	(8,'36–40','36–40 Jahre',2),
	(9,'41–45','41–45 Jahre',2),
	(10,'46–50','46–50 Jahre',2),
	(11,'51–55','51–55 Jahre',2),
	(12,'56–60','56–60 Jahre',2),
	(13,'61–65','61–65 Jahre',3),
	(14,'66–70','66–70 Jahre',3),
	(15,'71–75','71–75 Jahre',3),
	(16,'76–80','76–80 Jahre',3),
	(17,'81–85','81–85 Jahre',3),
	(18,'86–90','86–90 Jahre',3),
	(19,'91–95','91–95 Jahre',3),
	(20,'96–100','96–100 Jahre',3);

/*!40000 ALTER TABLE `DIM_AGE_GROUP` ENABLE KEYS */;
UNLOCK TABLES;


# Dump of table DIM_AGE_GROUP_COARSE
# ------------------------------------------------------------

DROP TABLE IF EXISTS `DIM_AGE_GROUP_COARSE`;

CREATE TABLE `DIM_AGE_GROUP_COARSE` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `content` varchar(255) DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

LOCK TABLES `DIM_AGE_GROUP_COARSE` WRITE;
/*!40000 ALTER TABLE `DIM_AGE_GROUP_COARSE` DISABLE KEYS */;

INSERT INTO `DIM_AGE_GROUP_COARSE` (`id`, `content`, `description`)
VALUES
	(1,'0–20','0–20 Jahre'),
	(2,'21–60','21–60 Jahre'),
	(3,'61-100','>60 Jahre');

/*!40000 ALTER TABLE `DIM_AGE_GROUP_COARSE` ENABLE KEYS */;
UNLOCK TABLES;


# Dump of table DIM_BLOODTYPE
# ------------------------------------------------------------

DROP TABLE IF EXISTS `DIM_BLOODTYPE`;

CREATE TABLE `DIM_BLOODTYPE` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `content` varchar(255) DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

LOCK TABLES `DIM_BLOODTYPE` WRITE;
/*!40000 ALTER TABLE `DIM_BLOODTYPE` DISABLE KEYS */;

INSERT INTO `DIM_BLOODTYPE` (`id`, `content`, `description`)
VALUES
	(1,'A','A'),
	(2,'B','B'),
	(3,'AB','AB'),
	(4,'0','0');

/*!40000 ALTER TABLE `DIM_BLOODTYPE` ENABLE KEYS */;
UNLOCK TABLES;


# Dump of table DIM_ICD10_Krankheitsgruppen
# ------------------------------------------------------------

DROP TABLE IF EXISTS `DIM_ICD10_Krankheitsgruppen`;

CREATE TABLE `DIM_ICD10_Krankheitsgruppen` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `content` varchar(255) DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  `kapitel` int(11) unsigned DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `kapitel` (`kapitel`),
  CONSTRAINT `dim_icd10_krankheitsgruppen_ibfk_1` FOREIGN KEY (`kapitel`) REFERENCES `DIM_ICD10_Krankheitskapitel` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

LOCK TABLES `DIM_ICD10_Krankheitsgruppen` WRITE;
/*!40000 ALTER TABLE `DIM_ICD10_Krankheitsgruppen` DISABLE KEYS */;

INSERT INTO `DIM_ICD10_Krankheitsgruppen` (`id`, `content`, `description`, `kapitel`)
VALUES
	(3,'S00-S09','Verletzungen des Kopfes',22),
	(4,'S10-S19','Verletzungen des Halses',22),
	(5,'S20-S29','Verletzungen des Thorax',22),
	(6,'S30-S39','Verletzungen des Abdomens, der Lumbosakralgegend, der Lendenwirbelsäule und des Beckens',22),
	(7,'S40-S49','Verletzungen der Schulter und des Oberarmes',22),
	(8,'S50-S59','Verletzungen des Ellenbogens und des Unterarmes',22),
	(9,'S60-S69','Verletzungen des Handgelenkes und der Hand',22),
	(10,'S70-S79','Verletzungen der Hüfte und des Oberschenkels',22),
	(11,'S80-S89','Verletzungen des Knies und des Unterschenkels',22),
	(12,'S90-S99','Verletzungen der Knöchelregion und des Fußes',22),
	(13,'T00-T07','Verletzungen mit Beteiligung mehrerer Körperregionen',22),
	(14,'T08-T14','Verletzungen nicht näher bezeichneter Teile des Rumpfes, der Extremitäten oder anderer Körperregionen',22),
	(15,'T15-T19','Folgen des Eindringens eines Fremdkörpers durch eine natürliche Körperöffnung',22),
	(16,'T20-T25','Verbrennungen oder Verätzungen der äußeren Körperoberfläche, Lokalisation bezeichnet',22),
	(17,'T26-T28','Verbrennungen oder Verätzungen, die auf das Auge und auf innere Organe begrenzt sind',22),
	(18,'T29-T32','Verbrennungen oder Verätzungen mehrerer und nicht näher bezeichneter Körperregionen',22),
	(19,'T33-T35','Erfrierungen',22),
	(20,'T36-T50','Vergiftungen durch Arzneimittel, Drogen und biologisch aktive Substanzen',22),
	(21,'T51-T65','Toxische Wirkungen von vorwiegend nicht medizinisch verwendeten Substanzen',22),
	(22,'T66-T78','Sonstige und nicht näher bezeichnete Schäden durch äußere Ursachen',22),
	(23,'T79-T79','Bestimmte Frühkomplikationen eines Traumas',22),
	(24,'T80-T88','Komplikationen bei chirurgischen Eingriffen und medizinischer Behandlung, anderenorts nicht klassifiziert',22),
	(25,'T89-T89','Sonstige Komplikationen eines Traumas, anderenorts nicht klassifiziert',22);

/*!40000 ALTER TABLE `DIM_ICD10_Krankheitsgruppen` ENABLE KEYS */;
UNLOCK TABLES;


# Dump of table DIM_ICD10_Krankheitskapitel
# ------------------------------------------------------------

DROP TABLE IF EXISTS `DIM_ICD10_Krankheitskapitel`;

CREATE TABLE `DIM_ICD10_Krankheitskapitel` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `content` varchar(255) DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

LOCK TABLES `DIM_ICD10_Krankheitskapitel` WRITE;
/*!40000 ALTER TABLE `DIM_ICD10_Krankheitskapitel` DISABLE KEYS */;

INSERT INTO `DIM_ICD10_Krankheitskapitel` (`id`, `content`, `description`)
VALUES
	(4,'A00–B99','Bestimmte infektiöse und parasitäre Krankheiten'),
	(5,'C00–D48','Neubildungen (beispielsweise Tumore u. Ä.)'),
	(6,'D50–D89','Krankheiten des Blutes und der blutbildenden Organe sowie bestimmte Störungen mit Beteiligung des Immunsystems'),
	(7,'E00–E90','Endokrine, Ernährungs- und Stoffwechselkrankheiten'),
	(8,'F00–F99','Psychische und Verhaltensstörungen'),
	(9,'G00–G99','Krankheiten des Nervensystems'),
	(10,'H00–H59','Krankheiten des Auges und der Augenanhangsgebilde'),
	(11,'H60–H95','Krankheiten des Ohres und des Warzenfortsatzes'),
	(12,'I00–I99','Krankheiten des Kreislaufsystems'),
	(13,'J00–J99','Krankheiten des Atmungssystems'),
	(14,'K00–K93','Krankheiten des Verdauungssystems'),
	(15,'L00–L99','Krankheiten der Haut und der Unterhaut'),
	(16,'M00–M99','Krankheiten des Muskel-Skelett-Systems und des Bindegewebes'),
	(17,'N00–N99','Krankheiten des Urogenitalsystems'),
	(18,'O00–O99','Schwangerschaft, Geburt und Wochenbett'),
	(19,'P00–P96','Bestimmte Zustände, die ihren Ursprung in der Perinatalperiode haben'),
	(20,'Q00–Q99','Angeborene Fehlbildungen, Deformitäten und Chromosomenanomalien'),
	(21,'R00–R99','Symptome und abnorme klinische und Laborbefunde, die anderenorts nicht klassifiziert sind'),
	(22,'S00–T98','Verletzungen, Vergiftungen und bestimmte andere Folgen äußerer Ursachen'),
	(23,'V01–Y98','Äußere Ursachen von Morbidität und Mortalität'),
	(24,'Z00–Z99','Faktoren, die den Gesundheitszustand beeinflussen und zur Inanspruchnahme des Gesundheitswesens führen'),
	(25,'U00–U89','Schlüsselnummern für besondere Zwecke');

/*!40000 ALTER TABLE `DIM_ICD10_Krankheitskapitel` ENABLE KEYS */;
UNLOCK TABLES;


# Dump of table DIM_ICD10_Krankheitsklassen
# ------------------------------------------------------------

DROP TABLE IF EXISTS `DIM_ICD10_Krankheitsklassen`;

CREATE TABLE `DIM_ICD10_Krankheitsklassen` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `content` varchar(255) DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  `gruppe` int(11) unsigned DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `gruppe` (`gruppe`),
  CONSTRAINT `dim_icd10_krankheitsklassen_ibfk_1` FOREIGN KEY (`gruppe`) REFERENCES `DIM_ICD10_Krankheitsgruppen` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

LOCK TABLES `DIM_ICD10_Krankheitsklassen` WRITE;
/*!40000 ALTER TABLE `DIM_ICD10_Krankheitsklassen` DISABLE KEYS */;

INSERT INTO `DIM_ICD10_Krankheitsklassen` (`id`, `content`, `description`, `gruppe`)
VALUES
	(1,'T14','Verletzung an einer nicht näher bezeichneten Körperregion',14),
	(2,'T13','Sonstige Verletzungen der unteren Extremität, Höhe nicht näher bezeichnet',14),
	(3,'T12','Fraktur der unteren Extremität, Höhe nicht näher bezeichnet',14),
	(4,'T11','Sonstige Verletzungen der oberen Extremität, Höhe nicht näher bezeichnet',14),
	(5,'T10','Fraktur der oberen Extremität, Höhe nicht näher bezeichnet',14),
	(6,'T09','Sonstige Verletzungen der Wirbelsäule und des Rumpfes, Höhe nicht näher bezeichnet',14),
	(7,'T08','Fraktur der Wirbelsäule, Höhe nicht näher bezeichnet',14);

/*!40000 ALTER TABLE `DIM_ICD10_Krankheitsklassen` ENABLE KEYS */;
UNLOCK TABLES;


# Dump of table DIM_ICD10_Subkategorien
# ------------------------------------------------------------

DROP TABLE IF EXISTS `DIM_ICD10_Subkategorien`;

CREATE TABLE `DIM_ICD10_Subkategorien` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `content` varchar(255) DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  `klasse` int(11) unsigned DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `klasse` (`klasse`),
  CONSTRAINT `dim_icd10_subkategorien_ibfk_1` FOREIGN KEY (`klasse`) REFERENCES `DIM_ICD10_Krankheitsklassen` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

LOCK TABLES `DIM_ICD10_Subkategorien` WRITE;
/*!40000 ALTER TABLE `DIM_ICD10_Subkategorien` DISABLE KEYS */;

INSERT INTO `DIM_ICD10_Subkategorien` (`id`, `content`, `description`, `klasse`)
VALUES
	(1,'T14.20','Fraktur an einer nicht näher bezeichneten Körperregion (geschlossen)',1),
	(2,'T14.21','Fraktur an einer nicht näher bezeichneten Körperregion (offen)',1),
	(3,'T14.0','Oberflächliche Verletzung an einer nicht näher bezeichneten Körperregion',1),
	(4,'T14.1','Offene Wunde an einer nicht näher bezeichneten Körperregion',1),
	(5,'T14.3','Luxation, Verstauchung und Zerrung an einer nicht näher bezeichneten Körperregion',1),
	(6,'T14.4','Verletzung eines oder mehrerer Nerven an einer nicht näher bezeichneten Körperregion',1),
	(7,'T14.5','Verletzung eines oder mehrerer Blutgefäße an einer nicht näher bezeichneten Körperregion',1),
	(8,'T14.6','Verletzung von Muskeln und Sehnen an einer nicht näher bezeichneten Körperregion',1),
	(9,'T14.7','Zerquetschung und traumatische Amputation einer nicht näher bezeichneten Körperregion',1),
	(10,'T14.8','Sonstige Verletzungen einer nicht näher bezeichneten Körperregion',1),
	(11,'T14.9','Verletzung, nicht näher bezeichnet',1);

/*!40000 ALTER TABLE `DIM_ICD10_Subkategorien` ENABLE KEYS */;
UNLOCK TABLES;


# Dump of table DIM_INSURANCE
# ------------------------------------------------------------

DROP TABLE IF EXISTS `DIM_INSURANCE`;

CREATE TABLE `DIM_INSURANCE` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `content` varchar(255) DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

LOCK TABLES `DIM_INSURANCE` WRITE;
/*!40000 ALTER TABLE `DIM_INSURANCE` DISABLE KEYS */;

INSERT INTO `DIM_INSURANCE` (`id`, `content`, `description`)
VALUES
	(1,'Privat','Private versichert'),
	(2,'Kasse','Gesetzlich versichert');

/*!40000 ALTER TABLE `DIM_INSURANCE` ENABLE KEYS */;
UNLOCK TABLES;


# Dump of table DIM_PLACE
# ------------------------------------------------------------

DROP TABLE IF EXISTS `DIM_PLACE`;

CREATE TABLE `DIM_PLACE` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `content` varchar(255) DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  `state` int(11) unsigned DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `state` (`state`),
  CONSTRAINT `dim_place_ibfk_1` FOREIGN KEY (`state`) REFERENCES `DIM_PLACE_STATE` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

LOCK TABLES `DIM_PLACE` WRITE;
/*!40000 ALTER TABLE `DIM_PLACE` DISABLE KEYS */;

INSERT INTO `DIM_PLACE` (`id`, `content`, `description`, `state`)
VALUES
	(1,'Kiel','Kiel',1),
	(2,'Lübeck','Lübeck',1),
	(3,'Flensburg','Flensburg',1),
	(4,'Neumünster','Neumünster',1),
	(5,'Oldenburg','Oldenburg',2),
	(6,'Osnabrück','Osnabrück',2),
	(7,'Hannover','Hannover',2),
	(8,'Göttingen','Göttingen',2),
	(9,'Lüneburg','Lüneburg',2),
	(10,'Hamburg','Hamburg',3),
	(11,'Bremen','Bremen',4),
	(12,'Bremerhaven','Bremerhaven',4),
	(13,'Rostock','Rostock',5),
	(14,'Schwerin','Schwerin',5);

/*!40000 ALTER TABLE `DIM_PLACE` ENABLE KEYS */;
UNLOCK TABLES;


# Dump of table DIM_PLACE_COUNTRY
# ------------------------------------------------------------

DROP TABLE IF EXISTS `DIM_PLACE_COUNTRY`;

CREATE TABLE `DIM_PLACE_COUNTRY` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `content` varchar(255) DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

LOCK TABLES `DIM_PLACE_COUNTRY` WRITE;
/*!40000 ALTER TABLE `DIM_PLACE_COUNTRY` DISABLE KEYS */;

INSERT INTO `DIM_PLACE_COUNTRY` (`id`, `content`, `description`)
VALUES
	(1,'Deutschland','Bundesrepublik Deutschland');

/*!40000 ALTER TABLE `DIM_PLACE_COUNTRY` ENABLE KEYS */;
UNLOCK TABLES;


# Dump of table DIM_PLACE_STATE
# ------------------------------------------------------------

DROP TABLE IF EXISTS `DIM_PLACE_STATE`;

CREATE TABLE `DIM_PLACE_STATE` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `content` varchar(255) DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  `country` int(11) unsigned DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `country` (`country`),
  CONSTRAINT `dim_place_state_ibfk_1` FOREIGN KEY (`country`) REFERENCES `DIM_PLACE_COUNTRY` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

LOCK TABLES `DIM_PLACE_STATE` WRITE;
/*!40000 ALTER TABLE `DIM_PLACE_STATE` DISABLE KEYS */;

INSERT INTO `DIM_PLACE_STATE` (`id`, `content`, `description`, `country`)
VALUES
	(1,'SH','Schleswig-Holstein',1),
	(2,'NDS','Niedersachsen',1),
	(3,'HH','Hamburg',1),
	(4,'HB','Bremen',1),
	(5,'MV','Mecklenburg-Vorpommern',1);

/*!40000 ALTER TABLE `DIM_PLACE_STATE` ENABLE KEYS */;
UNLOCK TABLES;


# Dump of table E_DIM_CARETAKER
# ------------------------------------------------------------

DROP TABLE IF EXISTS `E_DIM_CARETAKER`;

CREATE TABLE `E_DIM_CARETAKER` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `content` varchar(255) DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  `group` int(11) unsigned DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `group` (`group`),
  CONSTRAINT `e_dim_caretaker_ibfk_1` FOREIGN KEY (`group`) REFERENCES `E_DIM_CARETAKER_GROUP` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

LOCK TABLES `E_DIM_CARETAKER` WRITE;
/*!40000 ALTER TABLE `E_DIM_CARETAKER` DISABLE KEYS */;

INSERT INTO `E_DIM_CARETAKER` (`id`, `content`, `description`, `group`)
VALUES
	(11,'Dr. Zügig','Dr. Zügig',1),
	(12,'Dr. Maler','Dr. Maler',1),
	(13,'Dr. Müller','Dr. Müller',1),
	(21,'Herr Schmidt','Herr Schmidt',2),
	(22,'Frau Peters','Frau Peters',2),
	(23,'Frau Meier','Frau Meier',2),
	(41,'Prof. Dr. Oberst','Prof. Dr. Oberst',4),
	(42,'Prof. Dr. Best','Prof. Dr. Best',4),
	(43,'Prof. Dr. Super','Prof. Dr. Super',4),
	(51,'Frau Meier','Frau Meier',5),
	(52,'Frau Schneider','Frau Schneider',5),
	(53,'Frau Bauer','Frau Bauer',5);

/*!40000 ALTER TABLE `E_DIM_CARETAKER` ENABLE KEYS */;
UNLOCK TABLES;


# Dump of table E_DIM_CARETAKER_GROUP
# ------------------------------------------------------------

DROP TABLE IF EXISTS `E_DIM_CARETAKER_GROUP`;

CREATE TABLE `E_DIM_CARETAKER_GROUP` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `content` varchar(255) DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

LOCK TABLES `E_DIM_CARETAKER_GROUP` WRITE;
/*!40000 ALTER TABLE `E_DIM_CARETAKER_GROUP` DISABLE KEYS */;

INSERT INTO `E_DIM_CARETAKER_GROUP` (`id`, `content`, `description`)
VALUES
	(1,'Arzt','Arzt'),
	(2,'Helfer','Helfer'),
	(3,'Putzkraft','Putzkraft'),
	(4,'Chefarzt','Chefarzt'),
	(5,'Personal','Personal');

/*!40000 ALTER TABLE `E_DIM_CARETAKER_GROUP` ENABLE KEYS */;
UNLOCK TABLES;


# Dump of table E_DIM_DEPARTMENT
# ------------------------------------------------------------

DROP TABLE IF EXISTS `E_DIM_DEPARTMENT`;

CREATE TABLE `E_DIM_DEPARTMENT` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `content` varchar(255) DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

LOCK TABLES `E_DIM_DEPARTMENT` WRITE;
/*!40000 ALTER TABLE `E_DIM_DEPARTMENT` DISABLE KEYS */;

INSERT INTO `E_DIM_DEPARTMENT` (`id`, `content`, `description`)
VALUES
	(1,'Empfang','Empfang'),
	(2,'Chirurgie','Chirurgie'),
	(3,'Labor','Labor'),
	(4,'Notaufnahme','Notaufnahme'),
	(5,'Frauenheilkunde','Frauenheilkunde'),
	(6,'Geburtshilfe','Geburtshilfe'),
	(7,'Pädiatrie','Pädiatrie'),
	(8,'Radiologie','Radiologie'),
	(9,'Notaufnahme','Notaufnahme'),
	(10,'Medizinische Klinik','Medizinische Klinik');

/*!40000 ALTER TABLE `E_DIM_DEPARTMENT` ENABLE KEYS */;
UNLOCK TABLES;


# Dump of table EVENT
# ------------------------------------------------------------

DROP TABLE IF EXISTS `EVENT`;

CREATE TABLE `EVENT` (
  `event_id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `case_id` int(11) unsigned DEFAULT NULL,
  `department_id` int(11) unsigned DEFAULT NULL,
  `caretaker_id` int(11) unsigned DEFAULT NULL,
  `activity` varchar(255) DEFAULT NULL,
  `timestamp` int(11) DEFAULT NULL,
  PRIMARY KEY (`event_id`),
  KEY `case` (`case_id`),
  KEY `caretaker` (`caretaker_id`),
  KEY `department` (`department_id`),
  CONSTRAINT `EVENT_ibfk_1` FOREIGN KEY (`case_id`) REFERENCES `CASE` (`case_id`),
  CONSTRAINT `caretaker` FOREIGN KEY (`caretaker_id`) REFERENCES `E_DIM_CARETAKER` (`id`),
  CONSTRAINT `department` FOREIGN KEY (`department_id`) REFERENCES `E_DIM_DEPARTMENT` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

LOCK TABLES `EVENT` WRITE;
/*!40000 ALTER TABLE `EVENT` DISABLE KEYS */;

INSERT INTO `EVENT` (`event_id`, `case_id`, `department_id`, `caretaker_id`, `activity`, `timestamp`)
VALUES
	(1,1,1,51,'Aufnahme',1),
	(2,1,10,11,'Anamnese',2),
	(3,1,10,21,'Blutentnahme',3),
	(4,1,8,21,'Röntgen',4),
	(5,1,3,11,'Befundung',5),
	(6,1,2,41,'Operation',6),
	(7,1,10,21,'Verband',7),
	(8,1,10,21,'Verband',8),
	(9,1,1,51,'Entlassen',9),
	(10,2,1,52,'Aufnahme',1),
	(11,2,10,12,'Anamnese',2),
	(12,2,8,22,'Röntgen',3),
	(13,2,10,22,'Blutentnahme',4),
	(14,2,3,12,'Befundung',5),
	(15,2,2,42,'Operation',6),
	(16,2,10,22,'Verband',7),
	(17,2,10,22,'Verband',8),
	(18,2,10,22,'Verband',9),
	(19,2,10,22,'Verband',10),
	(20,2,1,52,'Entlassen',11),
	(21,3,1,53,'Aufnahme',1),
	(22,3,10,13,'Anamnese',2),
	(23,3,10,23,'Blutentnahme',3),
	(24,3,8,23,'MRT',4),
	(25,3,3,13,'Befundung',5),
	(26,3,2,43,'Operation',6),
	(27,3,10,23,'Verband',7),
	(28,3,10,23,'Verband',8),
	(29,3,1,53,'Entlassen',9),
	(30,4,1,51,'Aufnahme',1),
	(31,4,10,11,'Anamnese',2),
	(32,4,8,21,'MRT',3),
	(33,4,10,21,'Blutentnahme',4),
	(34,4,3,11,'Befundung',5),
	(35,4,2,41,'Operation',6),
	(36,4,10,21,'Verband',7),
	(37,4,10,21,'Verband',8),
	(38,4,10,21,'Verband',9),
	(39,4,10,21,'Verband',10),
	(40,4,1,51,'Entlassen',11),
	(41,5,1,51,'Aufnahme',1),
	(42,5,10,11,'Anamnese',2),
	(43,5,10,21,'Blutentnahme',3),
	(44,5,8,21,'Röntgen',4),
	(45,5,3,11,'Befundung',5),
	(46,5,2,41,'Operation',6),
	(47,5,10,21,'Verband',7),
	(48,5,10,21,'Verband',8),
	(49,5,1,51,'Entlassen',9),
	(50,6,1,52,'Aufnahme',1),
	(51,6,10,12,'Anamnese',2),
	(52,6,8,22,'Röntgen',3),
	(53,6,10,22,'Blutentnahme',4),
	(54,6,3,12,'Befundung',5),
	(55,6,2,42,'Operation',6),
	(56,6,10,22,'Verband',7),
	(57,6,10,22,'Verband',8),
	(58,6,10,22,'Verband',9),
	(59,6,10,22,'Verband',10),
	(60,6,1,52,'Entlassen',11),
	(61,7,1,53,'Aufnahme',1),
	(62,7,10,13,'Anamnese',2),
	(63,7,10,23,'Blutentnahme',3),
	(64,7,8,23,'MRT',4),
	(65,7,3,13,'Befundung',5),
	(66,7,2,43,'Operation',6),
	(67,7,10,23,'Verband',7),
	(68,7,10,23,'Verband',8),
	(69,7,1,53,'Entlassen',9),
	(70,8,1,52,'Aufnahme',1),
	(71,8,10,12,'Anamnese',2),
	(72,8,8,22,'MRT',3),
	(73,8,10,22,'Blutentnahme',4),
	(74,8,3,12,'Befundung',5),
	(75,8,2,42,'Operation',6),
	(76,8,10,22,'Verband',7),
	(77,8,10,22,'Verband',8),
	(78,8,10,22,'Verband',9),
	(79,8,10,22,'Verband',10),
	(80,8,1,52,'Entlassen',11),
	(81,9,1,51,'Aufnahme',1),
	(82,9,10,11,'Anamnese',2),
	(83,9,10,21,'Blutentnahme',3),
	(84,9,8,21,'Röntgen',4),
	(85,9,3,11,'Befundung',5),
	(86,9,2,41,'Operation',6),
	(87,9,10,21,'Verband',7),
	(88,9,10,21,'Verband',8),
	(89,9,1,51,'Entlassen',9),
	(90,10,1,52,'Aufnahme',1),
	(91,10,10,12,'Anamnese',2),
	(92,10,8,22,'Röntgen',3),
	(93,10,10,22,'Blutentnahme',4),
	(94,10,3,12,'Befundung',5),
	(95,10,2,42,'Operation',6),
	(96,10,10,22,'Verband',7),
	(97,10,10,22,'Verband',8),
	(98,10,10,22,'Verband',9),
	(99,10,10,22,'Verband',10),
	(100,10,1,52,'Entlassen',11),
	(101,11,1,53,'Aufnahme',1),
	(102,11,10,13,'Anamnese',2),
	(103,11,10,23,'Blutentnahme',3),
	(104,11,8,23,'MRT',4),
	(105,11,3,13,'Befundung',5),
	(106,11,2,43,'Operation',6),
	(107,11,10,23,'Verband',7),
	(108,11,10,23,'Verband',8),
	(109,11,1,53,'Entlassen',9),
	(110,12,1,53,'Aufnahme',1),
	(111,12,10,13,'Anamnese',2),
	(112,12,8,23,'MRT',3),
	(113,12,10,23,'Blutentnahme',4),
	(114,12,3,13,'Befundung',5),
	(115,12,2,43,'Operation',6),
	(116,12,10,23,'Verband',7),
	(117,12,10,23,'Verband',8),
	(118,12,10,23,'Verband',9),
	(119,12,10,23,'Verband',10),
	(120,12,1,53,'Entlassen',11),
	(121,13,1,52,'Aufnahme',1),
	(122,13,10,12,'Anamnese',2),
	(123,13,8,22,'Röntgen',3),
	(124,13,10,22,'Blutentnahme',4),
	(125,13,3,12,'Befundung',5),
	(126,13,2,42,'Operation',6),
	(127,13,10,22,'Verband',7),
	(128,13,10,22,'Verband',8),
	(129,13,10,22,'Verband',9),
	(130,13,10,22,'Verband',10),
	(131,13,1,52,'Entlassen',11),
	(132,14,1,53,'Aufnahme',1),
	(133,14,10,13,'Anamnese',2),
	(134,14,10,23,'Blutentnahme',3),
	(135,14,8,23,'MRT',4),
	(136,14,3,13,'Befundung',5),
	(137,14,2,43,'Operation',6),
	(138,14,10,23,'Verband',7),
	(139,14,10,23,'Verband',8),
	(140,14,1,53,'Entlassen',9),
	(141,21,1,51,'Aufnahme',1),
	(142,21,10,11,'Anamnese',2),
	(143,21,10,21,'Blutentnahme',3),
	(144,21,8,21,'Röntgen',4),
	(145,21,3,11,'Befundung',5),
	(146,21,2,41,'Operation',6),
	(147,21,10,21,'Verband',7),
	(148,21,10,21,'Verband',8),
	(149,21,1,51,'Entlassen',9),
	(150,22,1,52,'Aufnahme',1),
	(151,22,10,12,'Anamnese',2),
	(152,22,8,22,'Röntgen',3),
	(153,22,10,22,'Blutentnahme',4),
	(154,22,3,12,'Befundung',5),
	(155,22,2,42,'Operation',6),
	(156,22,10,22,'Verband',7),
	(157,22,10,22,'Verband',8),
	(158,22,10,22,'Verband',9),
	(159,22,10,22,'Verband',10),
	(160,22,1,52,'Entlassen',11),
	(161,23,1,53,'Aufnahme',1),
	(162,23,10,13,'Anamnese',2),
	(163,23,10,23,'Blutentnahme',3),
	(164,23,8,23,'MRT',4),
	(165,23,3,13,'Befundung',5),
	(166,23,2,43,'Operation',6),
	(167,23,10,23,'Verband',7),
	(168,23,10,23,'Verband',8),
	(169,23,1,53,'Entlassen',9),
	(170,24,1,51,'Aufnahme',1),
	(171,24,10,11,'Anamnese',2),
	(172,24,8,21,'MRT',3),
	(173,24,10,21,'Blutentnahme',4),
	(174,24,3,11,'Befundung',5),
	(175,24,2,41,'Operation',6),
	(176,24,10,21,'Verband',7),
	(177,24,10,21,'Verband',8),
	(178,24,10,21,'Verband',9),
	(179,24,10,21,'Verband',10),
	(180,24,1,51,'Entlassen',11),
	(181,25,1,51,'Aufnahme',1),
	(182,25,10,11,'Anamnese',2),
	(183,25,10,21,'Blutentnahme',3),
	(184,25,8,21,'Röntgen',4),
	(185,25,3,11,'Befundung',5),
	(186,25,2,41,'Operation',6),
	(187,25,10,21,'Verband',7),
	(188,25,10,21,'Verband',8),
	(189,25,1,51,'Entlassen',9),
	(190,26,1,52,'Aufnahme',1),
	(191,26,10,12,'Anamnese',2),
	(192,26,8,22,'Röntgen',3),
	(193,26,10,22,'Blutentnahme',4),
	(194,26,3,12,'Befundung',5),
	(195,26,2,42,'Operation',6),
	(196,26,10,22,'Verband',7),
	(197,26,10,22,'Verband',8),
	(198,26,10,22,'Verband',9),
	(199,26,10,22,'Verband',10),
	(200,26,1,52,'Entlassen',11),
	(201,27,1,53,'Aufnahme',1),
	(202,27,10,13,'Anamnese',2),
	(203,27,10,23,'Blutentnahme',3),
	(204,27,8,23,'MRT',4),
	(205,27,3,13,'Befundung',5),
	(206,27,2,43,'Operation',6),
	(207,27,10,23,'Verband',7),
	(208,27,10,23,'Verband',8),
	(209,27,1,53,'Entlassen',9),
	(210,28,1,52,'Aufnahme',1),
	(211,28,10,12,'Anamnese',2),
	(212,28,8,22,'MRT',3),
	(213,28,10,22,'Blutentnahme',4),
	(214,28,3,12,'Befundung',5),
	(215,28,2,42,'Operation',6),
	(216,28,10,22,'Verband',7),
	(217,28,10,22,'Verband',8),
	(218,28,10,22,'Verband',9),
	(219,28,10,22,'Verband',10),
	(220,28,1,52,'Entlassen',11),
	(221,29,1,51,'Aufnahme',1),
	(222,29,10,11,'Anamnese',2),
	(223,29,10,21,'Blutentnahme',3),
	(224,29,8,21,'Röntgen',4),
	(225,29,3,11,'Befundung',5),
	(226,29,2,41,'Operation',6),
	(227,29,10,21,'Verband',7),
	(228,29,10,21,'Verband',8),
	(229,29,1,51,'Entlassen',9),
	(230,30,1,52,'Aufnahme',1),
	(231,30,10,12,'Anamnese',2),
	(232,30,8,22,'Röntgen',3),
	(233,30,10,22,'Blutentnahme',4),
	(234,30,3,12,'Befundung',5),
	(235,30,2,42,'Operation',6),
	(236,30,10,22,'Verband',7),
	(237,30,10,22,'Verband',8),
	(238,30,10,22,'Verband',9),
	(239,30,10,22,'Verband',10),
	(240,30,1,52,'Entlassen',11),
	(241,31,1,53,'Aufnahme',1),
	(242,31,10,13,'Anamnese',2),
	(243,31,10,23,'Blutentnahme',3),
	(244,31,8,23,'MRT',4),
	(245,31,3,13,'Befundung',5),
	(246,31,2,43,'Operation',6),
	(247,31,10,23,'Verband',7),
	(248,31,10,23,'Verband',8),
	(249,31,1,53,'Entlassen',9),
	(250,32,1,53,'Aufnahme',1),
	(251,32,10,13,'Anamnese',2),
	(252,32,8,23,'MRT',3),
	(253,32,10,23,'Blutentnahme',4),
	(254,32,3,13,'Befundung',5),
	(255,32,2,43,'Operation',6),
	(256,32,10,23,'Verband',7),
	(257,32,10,23,'Verband',8),
	(258,32,10,23,'Verband',9),
	(259,32,10,23,'Verband',10),
	(260,32,1,53,'Entlassen',11),
	(261,33,1,52,'Aufnahme',1),
	(262,33,10,12,'Anamnese',2),
	(263,33,8,22,'Röntgen',3),
	(264,33,10,22,'Blutentnahme',4),
	(265,33,3,12,'Befundung',5),
	(266,33,2,42,'Operation',6),
	(267,33,10,22,'Verband',7),
	(268,33,10,22,'Verband',8),
	(269,33,10,22,'Verband',9),
	(270,33,10,22,'Verband',10),
	(271,33,1,52,'Entlassen',11),
	(272,34,1,53,'Aufnahme',1),
	(273,34,10,13,'Anamnese',2),
	(274,34,10,23,'Blutentnahme',3),
	(275,34,8,23,'MRT',4),
	(276,34,3,13,'Befundung',5),
	(277,34,2,43,'Operation',6),
	(278,34,10,23,'Verband',7),
	(279,34,10,23,'Verband',8),
	(280,34,1,53,'Entlassen',9),
	(281,101,4,51,'Aufnahme',1),
	(282,101,4,11,'Offene Wunden versorgen',2),
	(283,101,10,11,'Anamnese',3),
	(284,101,10,21,'Blutentnahme',4),
	(285,101,8,21,'Röntgen',5),
	(286,101,3,11,'Befundung',6),
	(287,101,2,41,'Operation',7),
	(288,101,10,21,'Verband',8),
	(289,101,10,21,'Verband',9),
	(290,101,1,51,'Entlassen',10),
	(291,102,4,52,'Aufnahme',1),
	(292,102,4,12,'Offene Wunden versorgen',2),
	(293,102,10,12,'Anamnese',3),
	(294,102,10,22,'Blutentnahme',4),
	(295,102,8,22,'MRT',5),
	(296,102,3,12,'Befundung',6),
	(297,102,2,42,'Operation',7),
	(298,102,10,22,'Verband',8),
	(299,102,10,22,'Verband',9),
	(300,102,10,22,'Verband',10),
	(301,102,10,22,'Verband',11),
	(302,102,1,52,'Entlassen',12),
	(303,103,4,53,'Aufnahme',1),
	(304,103,4,13,'Offene Wunden versorgen',2),
	(305,103,10,13,'Anamnese',3),
	(306,103,8,23,'MRT',5),
	(307,103,10,23,'Blutentnahme',4),
	(308,103,3,13,'Befundung',6),
	(309,103,2,43,'Operation',7),
	(310,103,10,23,'Verband',8),
	(311,103,10,23,'Verband',9),
	(312,103,1,53,'Entlassen',10),
	(313,104,4,51,'Aufnahme',1),
	(314,104,4,12,'Offene Wunden versorgen',2),
	(315,104,10,13,'Anamnese',3),
	(316,104,8,21,'Röntgen',4),
	(317,104,10,22,'Blutentnahme',5),
	(318,104,3,13,'Befundung',6),
	(319,104,2,41,'Operation',7),
	(320,104,10,22,'Verband',8),
	(321,104,10,23,'Verband',9),
	(322,104,10,21,'Verband',10),
	(323,104,10,22,'Verband',11),
	(324,104,1,53,'Entlassen',12),
	(325,105,4,51,'Aufnahme',1),
	(326,105,4,11,'Offene Wunden versorgen',2),
	(327,105,10,11,'Anamnese',3),
	(328,105,10,21,'Blutentnahme',4),
	(329,105,8,21,'Röntgen',5),
	(330,105,3,11,'Befundung',6),
	(331,105,2,41,'Operation',7),
	(332,105,10,21,'Verband',8),
	(333,105,10,21,'Verband',9),
	(334,105,1,51,'Entlassen',10),
	(335,106,4,52,'Aufnahme',1),
	(336,106,4,12,'Offene Wunden versorgen',2),
	(337,106,10,12,'Anamnese',3),
	(338,106,10,22,'Blutentnahme',4),
	(339,106,8,22,'MRT',5),
	(340,106,3,12,'Befundung',6),
	(341,106,2,42,'Operation',7),
	(342,106,10,22,'Verband',8),
	(343,106,10,22,'Verband',9),
	(344,106,10,22,'Verband',10),
	(345,106,10,22,'Verband',11),
	(346,106,1,52,'Entlassen',12),
	(347,107,4,53,'Aufnahme',1),
	(348,107,4,13,'Offene Wunden versorgen',2),
	(349,107,10,13,'Anamnese',3),
	(350,107,8,23,'MRT',4),
	(351,107,10,23,'Blutentnahme',5),
	(352,107,3,13,'Befundung',6),
	(353,107,2,43,'Operation',7),
	(354,107,10,23,'Verband',8),
	(355,107,10,23,'Verband',9),
	(356,107,1,53,'Entlassen',10),
	(357,108,4,51,'Aufnahme',1),
	(358,108,4,12,'Offene Wunden versorgen',2),
	(359,108,10,13,'Anamnese',3),
	(360,108,8,21,'Röntgen',4),
	(361,108,10,22,'Blutentnahme',5),
	(362,108,3,13,'Befundung',6),
	(363,108,2,41,'Operation',7),
	(364,108,10,22,'Verband',8),
	(365,108,10,23,'Verband',9),
	(366,108,10,21,'Verband',10),
	(367,108,10,22,'Verband',11),
	(368,108,1,53,'Entlassen',12),
	(369,109,4,51,'Aufnahme',1),
	(370,109,4,11,'Offene Wunden versorgen',2),
	(371,109,10,11,'Anamnese',3),
	(372,109,10,21,'Blutentnahme',4),
	(373,109,8,21,'Röntgen',5),
	(374,109,3,11,'Befundung',6),
	(375,109,2,41,'Operation',7),
	(376,109,10,21,'Verband',8),
	(377,109,10,21,'Verband',9),
	(378,109,1,51,'Entlassen',10),
	(379,110,4,52,'Aufnahme',1),
	(380,110,4,12,'Offene Wunden versorgen',2),
	(381,110,10,12,'Anamnese',3),
	(382,110,10,22,'Blutentnahme',4),
	(383,110,8,22,'MRT',5),
	(384,110,3,12,'Befundung',6),
	(385,110,2,42,'Operation',7),
	(386,110,10,22,'Verband',8),
	(387,110,10,22,'Verband',9),
	(388,110,10,22,'Verband',10),
	(389,110,10,22,'Verband',11),
	(390,110,1,52,'Entlassen',12),
	(391,111,4,53,'Aufnahme',1),
	(392,111,4,13,'Offene Wunden versorgen',2),
	(393,111,10,13,'Anamnese',3),
	(394,111,8,23,'MRT',4),
	(395,111,10,23,'Blutentnahme',5),
	(396,111,3,13,'Befundung',6),
	(397,111,2,43,'Operation',7),
	(398,111,10,23,'Verband',8),
	(399,111,10,23,'Verband',9),
	(400,111,1,53,'Entlassen',10),
	(401,112,4,51,'Aufnahme',1),
	(402,112,4,12,'Offene Wunden versorgen',2),
	(403,112,10,13,'Anamnese',3),
	(404,112,8,21,'Röntgen',4),
	(405,112,10,22,'Blutentnahme',5),
	(406,112,3,13,'Befundung',6),
	(407,112,2,41,'Operation',7),
	(408,112,10,22,'Verband',8),
	(409,112,10,23,'Verband',9),
	(410,112,10,21,'Verband',10),
	(411,112,10,22,'Verband',11),
	(412,112,1,53,'Entlassen',12),
	(413,113,4,51,'Aufnahme',1),
	(414,113,4,11,'Offene Wunden versorgen',2),
	(415,113,10,11,'Anamnese',3),
	(416,113,10,21,'Blutentnahme',4),
	(417,113,8,21,'Röntgen',5),
	(418,113,3,11,'Befundung',6),
	(419,113,2,41,'Operation',7),
	(420,113,10,21,'Verband',8),
	(421,113,10,21,'Verband',9),
	(422,113,1,51,'Entlassen',10),
	(423,114,4,52,'Aufnahme',1),
	(424,114,4,12,'Offene Wunden versorgen',2),
	(425,114,10,12,'Anamnese',3),
	(426,114,10,22,'Blutentnahme',4),
	(427,114,8,22,'MRT',5),
	(428,114,3,12,'Befundung',6),
	(429,114,2,42,'Operation',7),
	(430,114,10,22,'Verband',8),
	(431,114,10,22,'Verband',9),
	(432,114,10,22,'Verband',10),
	(433,114,10,22,'Verband',11),
	(434,114,1,52,'Entlassen',12),
	(435,121,4,51,'Aufnahme',1),
	(436,121,4,11,'Offene Wunden versorgen',2),
	(437,121,10,11,'Anamnese',3),
	(438,121,10,21,'Blutentnahme',4),
	(439,121,8,21,'Röntgen',5),
	(440,121,3,11,'Befundung',6),
	(441,121,2,41,'Operation',7),
	(442,121,10,21,'Verband',8),
	(443,121,10,21,'Verband',9),
	(444,121,1,51,'Entlassen',10),
	(445,122,4,52,'Aufnahme',1),
	(446,122,4,12,'Offene Wunden versorgen',2),
	(447,122,10,12,'Anamnese',3),
	(448,122,10,22,'Blutentnahme',4),
	(449,122,8,22,'MRT',5),
	(450,122,3,12,'Befundung',6),
	(451,122,2,42,'Operation',7),
	(452,122,10,22,'Verband',8),
	(453,122,10,22,'Verband',9),
	(454,122,10,22,'Verband',10),
	(455,122,10,22,'Verband',11),
	(456,122,1,52,'Entlassen',12),
	(457,123,4,53,'Aufnahme',1),
	(458,123,4,13,'Offene Wunden versorgen',2),
	(459,123,10,13,'Anamnese',3),
	(460,123,8,23,'MRT',4),
	(461,123,10,23,'Blutentnahme',5),
	(462,123,3,13,'Befundung',6),
	(463,123,2,43,'Operation',7),
	(464,123,10,23,'Verband',8),
	(465,123,10,23,'Verband',9),
	(466,123,1,53,'Entlassen',10),
	(467,124,4,51,'Aufnahme',1),
	(468,124,4,12,'Offene Wunden versorgen',2),
	(469,124,10,13,'Anamnese',3),
	(470,124,8,21,'Röntgen',4),
	(471,124,10,22,'Blutentnahme',5),
	(472,124,3,13,'Befundung',6),
	(473,124,2,41,'Operation',7),
	(474,124,10,22,'Verband',8),
	(475,124,10,23,'Verband',9),
	(476,124,10,21,'Verband',10),
	(477,124,10,22,'Verband',11),
	(478,124,1,53,'Entlassen',12),
	(479,125,4,51,'Aufnahme',1),
	(480,125,4,11,'Offene Wunden versorgen',2),
	(481,125,10,11,'Anamnese',3),
	(482,125,10,21,'Blutentnahme',4),
	(483,125,8,21,'Röntgen',5),
	(484,125,3,11,'Befundung',6),
	(485,125,2,41,'Operation',7),
	(486,125,10,21,'Verband',8),
	(487,125,10,21,'Verband',9),
	(488,125,1,51,'Entlassen',10),
	(489,126,4,52,'Aufnahme',1),
	(490,126,4,12,'Offene Wunden versorgen',2),
	(491,126,10,12,'Anamnese',3),
	(492,126,10,22,'Blutentnahme',4),
	(493,126,8,22,'MRT',5),
	(494,126,3,12,'Befundung',6),
	(495,126,2,42,'Operation',7),
	(496,126,10,22,'Verband',8),
	(497,126,10,22,'Verband',9),
	(498,126,10,22,'Verband',10),
	(499,126,10,22,'Verband',11),
	(500,126,1,52,'Entlassen',12),
	(501,127,4,53,'Aufnahme',1),
	(502,127,4,13,'Offene Wunden versorgen',2),
	(503,127,10,13,'Anamnese',3),
	(504,127,8,23,'MRT',4),
	(505,127,10,23,'Blutentnahme',5),
	(506,127,3,13,'Befundung',6),
	(507,127,2,43,'Operation',7),
	(508,127,10,23,'Verband',8),
	(509,127,10,23,'Verband',9),
	(510,127,1,53,'Entlassen',10),
	(511,128,4,51,'Aufnahme',1),
	(512,128,4,12,'Offene Wunden versorgen',2),
	(513,128,10,13,'Anamnese',3),
	(514,128,8,21,'Röntgen',4),
	(515,128,10,22,'Blutentnahme',5),
	(516,128,3,13,'Befundung',6),
	(517,128,2,41,'Operation',7),
	(518,128,10,22,'Verband',8),
	(519,128,10,23,'Verband',9),
	(520,128,10,21,'Verband',10),
	(521,128,10,22,'Verband',11),
	(522,128,1,53,'Entlassen',12),
	(523,129,4,51,'Aufnahme',1),
	(524,129,4,11,'Offene Wunden versorgen',2),
	(525,129,10,11,'Anamnese',3),
	(526,129,10,21,'Blutentnahme',4),
	(527,129,8,21,'Röntgen',5),
	(528,129,3,11,'Befundung',6),
	(529,129,2,41,'Operation',7),
	(530,129,10,21,'Verband',8),
	(531,129,10,21,'Verband',9),
	(532,129,1,51,'Entlassen',10),
	(533,130,4,52,'Aufnahme',1),
	(534,130,4,12,'Offene Wunden versorgen',2),
	(535,130,10,12,'Anamnese',3),
	(536,130,10,22,'Blutentnahme',4),
	(537,130,8,22,'MRT',5),
	(538,130,3,12,'Befundung',6),
	(539,130,2,42,'Operation',7),
	(540,130,10,22,'Verband',8),
	(541,130,10,22,'Verband',9),
	(542,130,10,22,'Verband',10),
	(543,130,10,22,'Verband',11),
	(544,130,1,52,'Entlassen',12),
	(545,131,4,53,'Aufnahme',1),
	(546,131,4,13,'Offene Wunden versorgen',2),
	(547,131,10,13,'Anamnese',3),
	(548,131,8,23,'MRT',4),
	(549,131,10,23,'Blutentnahme',5),
	(550,131,3,13,'Befundung',6),
	(551,131,2,43,'Operation',7),
	(552,131,10,23,'Verband',8),
	(553,131,10,23,'Verband',9),
	(554,131,1,53,'Entlassen',10),
	(555,132,4,51,'Aufnahme',1),
	(556,132,4,12,'Offene Wunden versorgen',2),
	(557,132,10,13,'Anamnese',3),
	(558,132,8,21,'Röntgen',4),
	(559,132,10,22,'Blutentnahme',5),
	(560,132,3,13,'Befundung',6),
	(561,132,2,41,'Operation',7),
	(562,132,10,22,'Verband',8),
	(563,132,10,23,'Verband',9),
	(564,132,10,21,'Verband',10),
	(565,132,10,22,'Verband',11),
	(566,132,1,53,'Entlassen',12),
	(567,133,4,51,'Aufnahme',1),
	(568,133,4,11,'Offene Wunden versorgen',2),
	(569,133,10,11,'Anamnese',3),
	(570,133,10,21,'Blutentnahme',4),
	(571,133,8,21,'Röntgen',5),
	(572,133,3,11,'Befundung',6),
	(573,133,2,41,'Operation',7),
	(574,133,10,21,'Verband',8),
	(575,133,10,21,'Verband',9),
	(576,133,1,51,'Entlassen',10),
	(577,134,4,52,'Aufnahme',1),
	(578,134,4,12,'Offene Wunden versorgen',2),
	(579,134,10,12,'Anamnese',3),
	(580,134,10,22,'Blutentnahme',4),
	(581,134,8,22,'MRT',5),
	(582,134,3,12,'Befundung',6),
	(583,134,2,42,'Operation',7),
	(584,134,10,22,'Verband',8),
	(585,134,10,22,'Verband',9),
	(586,134,10,22,'Verband',10),
	(587,134,10,22,'Verband',11),
	(588,134,1,52,'Entlassen',12);

/*!40000 ALTER TABLE `EVENT` ENABLE KEYS */;
UNLOCK TABLES;


# Dump of table FACT
# ------------------------------------------------------------

DROP TABLE IF EXISTS `FACT`;

CREATE TABLE `FACT` (
  `fact_id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `age_id` int(11) unsigned DEFAULT NULL,
  `bloodtype_id` int(11) unsigned DEFAULT NULL,
  `icd10_id` int(11) unsigned DEFAULT NULL,
  `insurance_id` int(11) unsigned DEFAULT NULL,
  `place_id` int(11) unsigned DEFAULT NULL,
  PRIMARY KEY (`fact_id`),
  KEY `age` (`age_id`),
  KEY `bloodtype` (`bloodtype_id`),
  KEY `insurance` (`insurance_id`),
  KEY `place` (`place_id`),
  KEY `icd` (`icd10_id`),
  CONSTRAINT `age` FOREIGN KEY (`age_id`) REFERENCES `DIM_AGE` (`id`),
  CONSTRAINT `bloodtype` FOREIGN KEY (`bloodtype_id`) REFERENCES `DIM_BLOODTYPE` (`id`),
  CONSTRAINT `icd` FOREIGN KEY (`icd10_id`) REFERENCES `DIM_ICD10_Subkategorien` (`id`),
  CONSTRAINT `insurance` FOREIGN KEY (`insurance_id`) REFERENCES `DIM_INSURANCE` (`id`),
  CONSTRAINT `place` FOREIGN KEY (`place_id`) REFERENCES `DIM_PLACE` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

LOCK TABLES `FACT` WRITE;
/*!40000 ALTER TABLE `FACT` DISABLE KEYS */;

INSERT INTO `FACT` (`fact_id`, `age_id`, `bloodtype_id`, `icd10_id`, `insurance_id`, `place_id`)
VALUES
	(1,18,2,1,1,1),
	(2,20,1,1,2,2),
	(3,25,4,1,2,3),
	(4,29,2,1,1,4),
	(5,33,3,1,1,5),
	(6,37,1,1,2,6),
	(7,39,2,1,2,7),
	(8,42,2,1,2,8),
	(9,44,3,1,2,9),
	(10,48,1,1,1,10),
	(11,50,2,1,2,11),
	(12,51,2,1,2,12),
	(13,56,1,1,2,13),
	(14,60,4,1,2,14),
	(101,15,1,2,1,1),
	(102,23,2,2,2,2),
	(103,25,1,2,2,3),
	(104,28,4,2,1,4),
	(105,35,1,2,1,5),
	(106,34,2,2,2,6),
	(107,33,3,2,2,7),
	(108,46,2,2,2,8),
	(109,48,1,2,2,9),
	(110,49,4,2,1,10),
	(111,55,3,2,2,11),
	(112,53,2,2,2,12),
	(113,52,1,2,2,13),
	(114,61,1,2,2,14);

/*!40000 ALTER TABLE `FACT` ENABLE KEYS */;
UNLOCK TABLES;



/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
