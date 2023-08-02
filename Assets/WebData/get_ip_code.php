<?php
header('Access-Control-Allow-Origin: *'); 
          echo var_export(unserialize(file_get_contents('http://www.geoplugin.net/php.gp?ip='.$_SERVER['REMOTE_ADDR'])));
 ?>