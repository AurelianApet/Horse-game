<?php

$dbName = 'casino_horserace';
$username =  'as_cwc_user';
$pwd = 'as_cwc_user';
$host = 'localhost';

function dbConnect()
{
	global  $dbName;
	global  $username;
	global  $pwd;
	global  $host;

	$link = mysql_connect($host, $username, $pwd);
	
	if(!$link)
	{
		fail("Couldn´t connect to database server");
	}
	
	if(!@mysql_select_db($dbName))
	{
		fail("Couldn´t find database $dbName");
	}
	
	return $link;
	}
	
function safe($variable)
{
	$variable = addslashes(trim($variable));
	return $variable;
}

function fail($errorMsg)
{
	print $errorMsg;
	exit;
}

?>