<?php

require_once 'init.php';
//require_once 'Controllers/AnnouncementsController.php';

$ctrl = new Controllers_AnnouncementsController();

try {
  $ctrl->Execute();
} catch (Exception $ex) {
  echo '<pre>';
  if ($conf->debug)
    echo $ex;
  else
    echo $ex->getMessage();
  echo '</pre>';
}
