<?php

/*
 * The base class used a the result of a controller action.
 */
class ActionResult {
  public $Content;
  public $ContentType = 'text/plain';
  public $ContentCharset = 'UTF-8';
  public $HttpCode = 200;
  public $Location;

  public function PreRender() { }
  public function PostRender() { }
  public function Render() {
    return $this->Content;
  }
}
