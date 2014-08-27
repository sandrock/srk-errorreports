<?php

/*
 * A simple controller for the needs of this app.
 */
class MiniController {

  protected $action;
  protected $actionFunc;
  protected $actionResult;
  

  public function __construct() {
  }
  
  public function Execute($action = null) {
    try {
      $this->SubExecute($action);
    } catch (Exception $ex) {
      ob_end_clean();
      $this->actionResult = $this->ErrorAction($ex);
      try {
        $this->Render();
      } catch (Exception $exc) {
        echo $exc->getMessage();
        echo $exc->getTraceAsString();
      }

    }
  }

  private function SubExecute($action = null) {
    ob_start();
  
    // ProcessRequest
    $this->PreProcessRequest();
    
    if ($action == null) {
      $action = isset($_REQUEST['action']) ? $_REQUEST['action'] : 'Index';
      if (strlen($action) > 32)
        throw new Exception('Action name too long', 2000);
    }
    $this->action = ucfirst(strtolower($action));
    $this->actionFunc = $this->action.'Action';
    
    $this->PostProcessRequest();
    
    // ActionInvoke
    $this->PreActionInvoke();
    
    //try {
      if (method_exists($this, $this->actionFunc)) {
        $func = $this->actionFunc;
        $this->actionResult = $this->$func();
      } else {
        throw new Exception('Action not found', 2001);
      }
    //} catch (Exception $ex) {
    //  $this->actionResult = $this->ActionResultError($ex);
    //}
    
    $this->PostActionInvoke();
    
    if (Zend_Registry::get('conf')->debug) {
      ob_end_flush();
    } else {
      ob_end_clean();
    }
    
    // Render
    $this->PreRender();
    if ($this->actionResult != null) {
      $this->actionResult->PreRender();
    }
    $this->Render();
    if ($this->actionResult != null) {
      $this->actionResult->PostRender();
    }
    $this->PostRender();
  }
  
  protected function PreProcessRequest() {
    
  }
  
  protected function PostProcessRequest() {
    
  }
  
  protected function PreActionInvoke() {
    
  }
  
  protected function PostActionInvoke() {
    
  }
  
  protected function PreRender() {
    
  }
  
  private function Render() {
    if (!Zend_Registry::get('conf')->debug) {
      if (headers_sent()) {
        throw new Exception('HTTP headers already sent.');
      }
    }
    if ($this->actionResult == null) {
      $r = new ActionResult();
      $r->Content = 'no content';
    } else {
      $r = $this->actionResult;
    }
    
    // HTTP code
    if ($r->HttpCode != 200) {
      header('HTTP/1.0 '.$r->HttpCode);
    }
    
    // Content type
    header('Content-Type: '.$r->ContentType.'; charset='.$r->ContentCharset);
    
    // Redirections
    if (!empty($r->Location)) {
      header('Location: '.$r->Location);
    }
    
    echo $r->Render();
  }
  
  protected function PostRender() {
    
  }
  
  protected function ActionResultError(Exception $ex) {
    $r = new ActionResult();
    
    $r->ContentType = 'text/plain';
    if (Zend_Registry::get('conf')->debug) {
      $r->Content = (string)$ex;
    } else {
      $r->Content = $ex->getMessage();
    }
    
    return $r;
  }
  
  protected function ActionResultText($content) {
    if ($this->rendering) {
      $content= $this->EndRender();
    }
  
    $r = new ActionResult();
    
    $r->ContentType = 'text/plain';
    $r->Content = $content;
    
    return $r;
  }
  
  protected function ActionResultHtml($content) {
    if ($this->rendering) {
      $content= $this->EndRender();
    }
  
    $r = new ActionResult();
    
    $r->ContentType = 'text/html';
    $r->Content = $content;
    
    return $r;
  }
  
  protected function ActionResultXhtml($content) {
    if ($this->rendering) {
      $content= $this->EndRender();
    }
  
    $r = new ActionResult();
    
    $r->ContentType = 'application/xml+xhtml';
    $r->Content = $content;
    
    return $r;
  }
  
  protected function ActionResultJson($content) {
    if ($this->rendering) {
      $content= $this->EndRender();
    }
  
    $r = new ActionResult();
    
    $r->ContentType = 'application/json';
    $r->Content = $content;
    
    return $r;
  }
  
  protected function ErrorAction($object) {
    if ($object instanceof Exception)
      return $this->ActionResultText($object->getCode().' '.$object->getMessage());
    else
      return $object;
  }
  
  protected function StartRender() {
    if (!$this->rendering) {
      $this->rendering = true;
      ob_start();
    }
  }
  
  protected function EndRender() {
    return ob_get_clean();
  }
  
  private $rendering = false;
  
}
