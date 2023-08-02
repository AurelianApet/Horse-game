<?php
header('Access-Control-Allow-Origin: *'); 
include("common.php");

	$link=dbConnect();

	   $mytable = safe($_POST['mytable']);
	   
	   $query = "SELECT score, name, country FROM $dbName .`$mytable` WHERE score = (SELECT MAX(score) FROM $dbName .`$mytable`)";
	   
       $result = mysql_query($query) or die('Query failed: ' . mysql_error());    
   
       $row = mysql_fetch_array($result);
                
       echo $row['score'] . ':' . $row['name'] . ':' . $row['country'] ;
          
  
 ?>