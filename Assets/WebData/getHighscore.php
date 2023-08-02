<?php
header('Access-Control-Allow-Origin: *'); 
include("common.php");
	$link=dbConnect();
/*
	$limit = safe($_POST['limit']);
*/	
    $mytable = safe($_POST['mytable']);
	$ascordesc = safe($_POST['ascordesc']);
    
    if($ascordesc === 'A') {
       $query = "SELECT `name`, `country`, max(score) as maxscore FROM $dbName .`$mytable` group by `name` order by maxscore ASC LIMIT 300";
    }
    
    if($ascordesc === 'D') {
       $query = "SELECT `name`, `country`, max(score) as maxscore FROM $dbName .`$mytable` group by `name` order by maxscore DESC LIMIT 300";
    }
 
 
    $result = mysql_query($query);    
    $my_err = mysql_error();
    if($result === false || $my_err != '')
    {
        echo "
        <pre>
            $my_err <br />
            $query <br />
        </pre>";
        die();
    }

    $num_results = mysql_num_rows($result);
    
    for($i = 0; $i < $num_results; $i++)
    {
         $row = mysql_fetch_array($result);
                
         echo $i+1 . " - " . $row['name'] . "\t - \t " . $row['maxscore'] . "\t - \t :" . $row["country"] . "\n";
          
    }
?>