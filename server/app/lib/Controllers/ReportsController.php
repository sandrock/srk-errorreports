<?php

//require_once 'ServiceController.php';
//require_once 'Model/AnnouncementTable.php';


/*
 * Error reporting controller.
 */
class Controllers_ReportsController extends ServiceController {

  public function __construct() {
    parent::__construct();
  }

  protected function IndexAction() {
    return $this->ActionResultText('Nothing here but ducks.');
  }

  /*
   * POST method that receives an error report.
   * data={json}
   */
  protected function PostErrorReportAction() {
    $data = $_REQUEST['data'];
    if (empty($data))
      return $this->ActionResult(new Exception('No data', 2003));

    // decode posted json
    $d = json_decode($data, true);
    if ($d == null || !is_array($d) || count($d) == 0)
      return $this->ActionResult(new Exception('No data (after json_decode)', 2004));

    // insert error report in db
    $table = new Model_ReportTable();
    $row = $table->createRow($d);
    $row->Date = Zend_Date::now()->get(Zend_Date::ISO_8601);
    if (!empty($row->AppStartTime))
      $row->AppStartTime = GetDateFromJsonDate($row->AppStartTime);
    if (!empty($row->AppExitTime))
      $row->AppExitTime = GetDateFromJsonDate($row->AppExitTime);
    if (!empty($row->AppErrorTime))
      $row->AppErrorTime = GetDateFromJsonDate($row->AppErrorTime);

    $row->UserAgent = $_SERVER['HTTP_USER_AGENT'];
    $row->ClientIPAddress = $_SERVER['REMOTE_ADDR'];

    try {
      $row->save();
    } catch (Exception $ex) {
      return $this->ActionResult($ex);
    }
    
    // email notifications
    try {
      $conf = Zend_Registry::get('conf');
      if ($conf->modules->reports->emailNotitication) {
        $report = $row;
        $subject = $conf->modules->reports->emailSubject;
        $subject = eval("return<<<EOD\n{$subject}\nEOD;\n");
        $content = $conf->modules->reports->emailContent;
        $content = eval("return<<<EOD\n{$content}\nEOD;\n");
        $to = $conf->modules->reports->emailRecipient;

        // here I would like to manage a list of subscriber by project (AssemblyName)
        // to notify specific people for specific projects
        // 
        #if ($row->AssemblyName == 'ProjectX')
        #  $to = $to . ', someonex@test.com';

        if (mail($to, $subject, $content)) {
          // email sent
        } else {
          // email error, too bad :-/
        }
      }
    } catch(Exception $ex) {
      // email or templating error, too bad :-/
    }
    
    return $this->ActionResult($row->ReportId);
  }

  /*
   * GET method to list error reports.
   * [order=asc|desc][count={number}][id={id}][AssemblyName={name}]
   */
  protected function ListAction() {
    if (!$this->CheckAccess())
      return $this->ForbiddenResponse();

    $order = $_REQUEST['order'] == 'desc' ? 'desc' : 'asc';
    $id = $_REQUEST['id'] > 0 ? (int)$_REQUEST['id'] : 0;
    $count = 10;
    if ($_REQUEST['count'] > 0 && $_REQUEST['count'] < 100)
      $count = (int)$_REQUEST['count'];
    
    $table = new Model_ReportTable();
    $select = $table->select();
    if ($order == 'asc') {
      $select->order('ReportId ASC');
      if ($id > 0)
        $select->where('ReportId > ?', $id);
    } else {
      if ($id > 0)
        $select->where('ReportId < ?', $id);
      $select->order('ReportId DESC');    
    }

    $assemblyName = $_REQUEST['AssemblyName'];
    if (!empty($assemblyName))
      $select->where('AssemblyName = ?', $assemblyName);

    $select->limit($count);

    $all = $table->fetchAll($select)->toArray();

    return $this->ActionResult($all);
  }
  
  /*
   * GET method to list assembly names.
   */
  protected function AssemblyNamesAction(){
    if (!$this->CheckAccess())
      return $this->ForbiddenResponse();

    $table = new Model_ReportTable();
    $select = $table->select();
    $select->from($table, array('AssemblyName'));
    $select->group('AssemblyName');
    $all = $table->fetchAll($select);
    $content = array();
    foreach ($all as $row)
      $content[] = $row->AssemblyName;
    return $this->ActionResult($content);
  }

  /*
   * GET method to list assemblies with reports count.
   */
  protected function AssembliesAction(){
    if (!$this->CheckAccess())
      return $this->ForbiddenResponse();

    $table = new Model_ReportTable();
    $select = $table->select();
    $select->from($table, array('AssemblyName', 'Handling', 'COUNT(ReportId) Reports'));
    $select->group(array('AssemblyName', 'Handling'));
    $all = $table->fetchAll($select)->toArray();
    //return $this->ActionResult($select->__toString());
    return $this->ActionResult($all);
  }

  /*
   * GET method return the apache_request_headers.
   */
  protected function HeadersAction() {
    return $this->ActionResult(apache_request_headers());
  }

  /*
   * POST method to update the Handling status of a report.
   */
  protected function ChangeHandlingAction() {
    $id = (int)$_REQUEST['ReportId'];
    $status = (int)$_REQUEST['Handling'];
    $table = new Model_ReportTable();
    $rows = $table->find($id);
    $row = $rows->current();
    $row->Handling = $status;
    $row->save();
    $result = new stdClass();
    $result->ReportId = $row->ReportId;
    $result->Handling = $row->Handling;
    return $this->ActionResult($result);
  }

  private function CheckAccess() {
    $headers = apache_request_headers();
    if (!array_key_exists(self::AuthHeaderName, $headers)) 
      return false;
    $auth = $headers[self::AuthHeaderName];
    return $auth == self::AuthHeaderValue;
  }

  private function ForbiddenResponse() {
    $headers = apache_request_headers();
    if (!array_key_exists(self::AuthHeaderName, $headers))
      return $this->ActionResult(new Exception('Forbidden: auth header is missing'));
    $auth = $headers[self::AuthHeaderName];
    if ($auth != self::AuthHeaderValue)
      return $this->ActionResult(new Exception('Forbidden: invalid credentials'));

    return $this->ActionResult(new Exception('Forbidden'));
  }
  
  const AuthHeaderName = 'X-SrkErrorReportAuth';
  const AuthHeaderValue = '567C9844-C92F-48C7-9D2C-A6A0234E0F27'; // this should move to config
}
