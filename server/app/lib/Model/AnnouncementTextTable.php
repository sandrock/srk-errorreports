<?php

/**
 * Description of AnnouncementTable
 *
 * @author sandrock
 */
class Model_AnnouncementTextTable extends Zend_Db_Table_Abstract {

  protected $_name = 'AnnouncementTexts';
  protected $_primary = array('AnnouncementId', 'Language');

  public function Get($announId, $lang) {
    $select = $this->select()
      ->where('AnnouncementId = ?', $announId, Zend_Db::INT_TYPE)
      ->where('Language = ?', $lang);
    return $this->fetchRow($select);
  }

}
