<?php

//require_once 'ServiceController.php';
//require_once 'Model/AnnouncementTable.php';

class Controllers_AnnouncementsController extends ServiceController {


  public function __construct() {
    parent::__construct();
  }

  protected function IndexAction() {
    return $this->ActionResultText('Nothing here but aliens.');
  }

  protected function GetLatestAction() {
    $lang = $_REQUEST['lang'];
    if (empty($lang))
      $lang= 'en';
    if (strlen($lang) > 5)
      return $this->ActionResult(new Exception('Invalid lang.', 2000));

    $section = isset($_REQUEST['section']) ? $_REQUEST['section'] : null;
    if (strlen($section) > 32)
      return $this->ActionResult(new Exception('Invalid section.', 2002));

    $table = new Model_AnnouncementTable();
    $last = $table->GetLatestTranslated($lang, $section);

    if ($last == null)
      return $this->ActionResult(null);

    $date = new Zend_Date($last->Date);
    $last->Date = '/Date(' . $date->get(Zend_Date::TIMESTAMP)*1000 . ')/';
    $a = $last->toArray();

    return $this->ActionResult($a);
  }

  protected function GetAllAction() {
    $lang = isset($_REQUEST['lang']) ? $_REQUEST['lang'] : null;
    if (empty($lang))
      $lang= 'en';
    if (strlen($lang) > 5)
      return $this->ActionResult(new Exception('Invalid lang.', 2000));

    $section = isset($_REQUEST['section']) ? $_REQUEST['section'] : null;
    if (strlen($section) > 32)
      return $this->ActionResult(new Exception('Invalid section.', 2002));

    $limit = (int)$_REQUEST['limit'];

    $table = new Model_AnnouncementTable();
    $all = $table->GetAllTranslated($lang, $section, $limit);

    if ($all == null)
      return $this->ActionResult(null);

    $list = array();
    foreach ($all as $item) {
      $date = new Zend_Date($item->Date);
      $item->Date = '/Date(' . $date->get(Zend_Date::TIMESTAMP)*1000 . ')/';
      $list[] = $item->toArray();
      #$item->Content = utf8_encode($item->Content);
    }

    return $this->ActionResult($list);
  }

  protected function GetSectionsAction() {
    $table = new Model_AnnouncementTable();
    $sections = $table->GetSections();

    $list = array();
    foreach ($sections as $item) {
      $list[] = $item->Section;
    }

    return $this->ActionResult($list);
  }

}
