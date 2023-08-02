-- phpMyAdmin SQL Dump
-- version 3.4.5
-- http://www.phpmyadmin.net
--
-- Host: localhost
-- Generation Time: Nov 30, 2015 at 11:39 AM
-- Server version: 5.5.16
-- PHP Version: 5.3.8

SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Database: `casino_horserace`
--

-- --------------------------------------------------------

--
-- Table structure for table `player_nick`
--

CREATE TABLE IF NOT EXISTS `newdino_player_nick` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(15) NOT NULL DEFAULT 'anonymous',
  `country` varchar(4) NOT NULL DEFAULT 'XX',
  PRIMARY KEY (`id`),
  UNIQUE KEY `name` (`name`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=53 ;

-- --------------------------------------------------------

--
-- Table structure for table `scores_match_easy`
--

CREATE TABLE IF NOT EXISTS `newdino_scores_level_0_mission_0` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(15) NOT NULL DEFAULT 'anonymous',
  `score` float unsigned NOT NULL DEFAULT '0',
  `country` varchar(4) NOT NULL DEFAULT 'XX',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=3 ;

-- --------------------------------------------------------
--
-- Table structure for table `scores_match_easy`
--

CREATE TABLE IF NOT EXISTS `newdino_scores_level_0_mission_1` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(15) NOT NULL DEFAULT 'anonymous',
  `score` float unsigned NOT NULL DEFAULT '0',
  `country` varchar(4) NOT NULL DEFAULT 'XX',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=3 ;

-- --------------------------------------------------------
--
-- Table structure for table `scores_match_easy`
--

CREATE TABLE IF NOT EXISTS `newdino_scores_level_0_mission_2` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(15) NOT NULL DEFAULT 'anonymous',
  `score` float unsigned NOT NULL DEFAULT '0',
  `country` varchar(4) NOT NULL DEFAULT 'XX',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=3 ;

-- --------------------------------------------------------
--
-- Table structure for table `scores_match_easy`
--

CREATE TABLE IF NOT EXISTS `newdino_scores_level_0_mission_3` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(15) NOT NULL DEFAULT 'anonymous',
  `score` float unsigned NOT NULL DEFAULT '0',
  `country` varchar(4) NOT NULL DEFAULT 'XX',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=3 ;

-- --------------------------------------------------------
--
-- Table structure for table `scores_match_easy`
--

CREATE TABLE IF NOT EXISTS `newdino_scores_level_0_mission_4` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(15) NOT NULL DEFAULT 'anonymous',
  `score` float unsigned NOT NULL DEFAULT '0',
  `country` varchar(4) NOT NULL DEFAULT 'XX',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=3 ;

-- --------------------------------------------------------
--
-- Table structure for table `scores_match_easy`
--

CREATE TABLE IF NOT EXISTS `newdino_scores_level_0_mission_5` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(15) NOT NULL DEFAULT 'anonymous',
  `score` float unsigned NOT NULL DEFAULT '0',
  `country` varchar(4) NOT NULL DEFAULT 'XX',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=3 ;

-- --------------------------------------------------------


/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
