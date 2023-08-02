<?php
header('Access-Control-Allow-Origin: *'); 
include("common.php");
	$link=dbConnect();

    $nickname = "";

	$nickname = $_POST['nickname'];
		
	   $query = "SELECT `name` FROM $dbName . `player_nick` WHERE `name` = '$nickname'";
	   
       $result = mysql_query($query) or die('Query failed: ' . mysql_error());

    $result = mysql_query($query);    
  
    $num_results = mysql_num_rows($result);
    
  echo "$num_results";
          
 ?>