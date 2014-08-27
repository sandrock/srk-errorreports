<?php

$conf = new stdClass();
$conf->date = new stdClass();
$conf->db = new stdClass();
$conf->db->params = array();
$conf->modules = new stdClass();
$conf->modules->reports = new stdClass();

$conf->debug = true;

$conf->date->timezone = 'Europe/Paris';
$conf->date->locale = 'en_US';

$conf->db->enabled = 1;
$conf->db->adapter = 'mysqli';
$conf->db->params['host'] = 'localhost';
$conf->db->params['username'] = '';
$conf->db->params['password'] = '';
$conf->db->params['dbname'] = 'errorreports';

$conf->modules->reports->emailNotitication = true;
$conf->modules->reports->emailRecipient = 'someone+logs@test.com';
$conf->modules->reports->emailSubject = '{$report->AssemblyName} - new error report';
$conf->modules->reports->emailContent = 'Hello,

This is an error report notification.

ReportId:    {$report->ReportId}
Date:        {$report->Date}
User-agent:  {$report->UserAgent}
Assembly:    {$report->AssemblyName} {$report->AssemblyVersion} {$report->DeploymentKind}
User:        {$report->UserId} {$report->Culture}
Device:      {$report->DeviceManufacturer} {$report->DeviceName} {$report->DeviceId}
Memory:      {$report->AppCurrentMemoryUsage} / {$report->DeviceTotalMemory} (peak: {$report->AppPeakMemoryUsage})
Platform:    {$report->OSPlatform} {$report->OSVersion}
HttpRequest: {$report->HttpMethod} {$report->HttpRequest}
HttpReferer: {$report->HttpReferer}
HttpHost:    {$report->HttpHost}

Exception:
{$report->FullException}

{$report->ExceptionTrace}

Deployment comment:
{$report->DeploymentComment}

Comment:
{$report->Comment}


-- 
Regards,
The administrator.

administrator@test.com
';

