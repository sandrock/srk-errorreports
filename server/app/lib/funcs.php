<?php

function GetTimestampFromJsonDate($jsonDate) {
  $date = substr($jsonDate, strpos($jsonDate, '(') + 1);
  return substr($date, 0, strpos($date, ')') - 5 - 3);
}

function GetDateFromJsonDate($jsonDate) {
  $time = GetTimestampFromJsonDate($jsonDate);
  $d = new Zend_Date($time, Zend_Date::TIMESTAMP);
  return $d->get(Zend_Date::ISO_8601);
}

