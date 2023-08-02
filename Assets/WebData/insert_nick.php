<?php 
header('Access-Control-Allow-Origin: *');
include("common.php");
	$link=dbConnect();
    $name = "";
    $country = "";
	$name = $_POST['name'];
	$country = $_POST['country'];

		$query = "INSERT INTO $dbName .`player_nick` (`id`, `name`, `country`) VALUES (NULL, '$name', '$country');"; 
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