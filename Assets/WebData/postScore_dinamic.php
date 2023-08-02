<?php 
header('Access-Control-Allow-Origin: *'); 
include("common.php");
	$link=dbConnect();

	$name = safe($_POST['name']);
	$score = safe($_POST['score']);
	$country = safe($_POST['country']);
	$mytable = safe($_POST['mytable']);

		$query = "INSERT INTO $dbName .`$mytable` (`id`, `name`, `score`, `country`) VALUES (NULL, '$name', '$score', '$country');"; 
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
		
		echo "done";
?>