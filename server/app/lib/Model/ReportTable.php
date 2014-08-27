<?php

require_once 'Zend/Db/Table/Abstract.php';

class Model_ReportTable extends Zend_Db_Table_Abstract {
  protected $_name = 'ErrorReports';
  protected $_primary = 'ReportId';  
}

