<?php

//require_once 'ServiceActionResult.php';
//require_once 'MiniController.php';
/*
 * A controller built for API calls
 */
class ServiceController extends MiniController {

  public function ActionResult($object) {
    return new ServiceActionResult($object);
  }

  protected function ErrorAction($object) {
    return $this->ActionResult($object);
  }

}
