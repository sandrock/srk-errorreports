<?php

//require_once 'MiniController.php';

/**
 * ActionResult serving a data object as JSON.
 *
 * @author sandrock
 */
class ServiceActionResult extends ActionResult {

  private $errors = array();
  private $data;

  public function  __construct($data) {
    $this->ContentType = 'application/json';
    if ($data instanceof Exception) {
      $e = new stdClass();
      $e->Code = $data->getCode();
      $e->Message = $data->getMessage();
      $this->errors[] = $e;
    } else {
      $this->data = $data;
    }
  }


  public function Render() {
    $o = new stdClass();
    $o->Errors = $this->errors;
    $o->Data = $this->data;
    return Zend_Json::encode($o);
    return json_encode($o);
  }

}
