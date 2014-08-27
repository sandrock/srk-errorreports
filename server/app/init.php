<?php

require_once 'preinit.php';

# Safe initialization of PHP variables
ini_set('track_errors', true);
ini_set('display_errors', true);
error_reporting(E_ALL | E_NOTICE | E_STRICT);

# Configure include_path for your own classes
set_include_path(get_include_path().PATH_SEPARATOR.'./lib');

# Loads Zend_Loader and register class auto-loading
require_once 'Zend/Version.php';
if (Zend_Version::compareVersion('1.8.0') === 1) {
    require_once 'Zend/Loader.php';
    Zend_Loader::registerAutoload();
} else {
    require_once 'Zend/Loader/Autoloader.php';
    Zend_Loader_Autoloader::getInstance()->setFallbackAutoloader(true);
}

# Application main configuration
if ($conf->debug) {
require_once 'conf.php';
  ini_set('display_errors', true);
}
Zend_Registry::set('conf', $conf);
date_default_timezone_set($conf->date->timezone);
Zend_Registry::set('Zend_Locale', new Zend_Locale($conf->date->locale));
Zend_Locale::setDefault($conf->date->locale);


# Database link
if ((int) $conf->db->enabled) {
    $db = Zend_Db::factory($conf->db->adapter, $conf->db->params);
    Zend_Db_Table_Abstract::setDefaultAdapter($db);
}

require_once 'lib/funcs.php';

