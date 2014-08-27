<?php

/**
 * Description of AnnouncementTable
 *
 * @author sandrock
 */
class Model_AnnouncementTable extends Zend_Db_Table_Abstract {

  protected $_name = 'Announcements';
  protected $_primary = 'AnnouncementId';

  public function GetLatestOne($section = null) {
    $select = $this->select()
      ->where('Announcements.Status > 0')
      ->order(array('desc' => 'Date'))
      ->limit(1);
    if (!empty($$section))
      $select->where('Section = ?', $section);
    return $this->fetchRow($select);
  }

  public function GetLatestTranslated($lang, $section = null) {
    $select = $this->select()
      ->setIntegrityCheck(false)
      ->from('Announcements')
      ->joinInner('AnnouncementTexts', 'AnnouncementTexts.AnnouncementId = Announcements.AnnouncementId', array('Content'))
      ->where('AnnouncementTexts.Language = ?', $lang)
      ->where('Announcements.Status > 0')
      ->order('Announcements.Date DESC')
      ->limit(1);
    if (!empty($section))
      $select->where('Announcements.Section = ?', $section);
    return $this->fetchRow($select);
  }

  public function GetAllTranslated($lang, $section = null, $limit = 0) {
    $select = $this->select()
      ->setIntegrityCheck(false)
      ->from('Announcements')
      ->join('AnnouncementTexts', 'AnnouncementTexts.AnnouncementId = Announcements.AnnouncementId', array('Content'))
      ->where('AnnouncementTexts.Language = ?', $lang)
      ->where('Announcements.Status > 0')
      ->order('Announcements.Date DESC');
    if ($limit != 0)
      $select->limit((int)$limit);
    if (!empty($section))
      $select->where('Announcements.Section = ?', $section);
    return $this->fetchAll($select);
  }

  public function GetSections() {
    $select = $this->select()
      ->from('Announcements', new Zend_Db_Expr('DISTINCT Section'));
    return $this->fetchAll($select);
  }

}
