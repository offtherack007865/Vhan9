-- SQL Service Instance:  smg-sql01
/*	
  "ReadDirectory": "\\\\ps-nas\\NAS\\SSS\\IT\\Decision Support\\ETL\\Export\\VHAN",
  "ArchiveInputFileThisRunDirectory": "\\\\ps-nas\\NAS\\SSS\\IT\\Decision Support\\ETL\\Export\\VHAN\ArchiveInputFilesThisRun",
  "ArchiveInputFileDirectory": "\\\\ps-nas\\NAS\\SSS\\IT\\Decision Support\\ETL\\Export\\VHAN\\ArchiveInputFiles",
  "ArchiveOutputFileDirectory": "\\\\ps-nas\\NAS\\SSS\\IT\\Decision Support\\ETL\\Export\\VHAN\\ArchiveOutputFiles",
  "ToArcadiaSftpDirectory": "\\\\ps-nas\\NAS\\SSS\\IT\\Decision Support\\ETL\\Export\\VHAN\\ToArcadiaSftpDirectory",
  "MyBaseWebApiUrl": "http://webservices:4222",
  "EmailBaseWebApiUrl": "http://webservices:4001/api/EmailWebApi/SendEmailWithHtmlStringInput",
  "EmailSubject": "VHAN zip file was transferred successfully to SFTP Site",
  "EmailFromAddress": "smgapplications@summithealthcare.com",
  "EmaileesString": "pwmorrison@summithealthcare.com"
*/
USE [Administration];


DECLARE @MyApplicationID [int] = 0;

-- If Application ID exists, delete both the Process ID and its associated Settings.
SELECT @MyApplicationID = [ApplicationID]
  FROM [admin].[Application]
  WHERE [ApplicationName] = 'Vhan';
  
IF (@MyApplicationID IS NULL OR @MyApplicationID = 0) BEGIN
  print ('Insert Application');
  INSERT INTO [admin].[Application]
           ([ApplicationName]
           ,[Description]
           ,[NotificationOnError]
           ,[NotificationGroupID]
           ,[Notes]
           ,[Visible]
           ,[ListOrder]
           ,[Active]
           ,[InsertedBy]
           ,[InsertedDate]
           ,[UpdatedBy]
           ,[UpdatedDate])
     VALUES
           ('Vhan'
           ,'Vhan'
           ,0
           ,null
           ,null
           ,1
           ,550
           ,1
           ,'pwmorrison'
           ,getdate()
           ,'pwmorrison'
           ,getdate());
		   
SELECT @MyApplicationID = [ApplicationID]
  FROM [admin].[Application]
  WHERE [ApplicationName] = 'Vhan';
		   
END

DECLARE @MyProcessID [int] = 0;

-- If Process ID exists, delete both the Process ID and its associated Settings.
SELECT @MyProcessID = [ProcessID]
  FROM [support].[Process]
  WHERE [Name] = 'Arcadia'
    AND [ApplicationID] = @MyApplicationID;  
  
IF (@MyProcessID IS NOT NULL) BEGIN
  DELETE [support].[Setting]
   WHERE [ApplicationID] = @MyApplicationID
     AND [ProcessID] = @MyProcessID;
	 
  DELETE [support].[Process]
   WHERE [ApplicationID] = @MyApplicationID
     AND [ProcessID] = @MyProcessID;
	 
END
  
-- Insert Process 
INSERT INTO [support].[Process]
           ([ApplicationID]
           ,[Name]
           ,[Type]
           ,[Description]
           ,[NotificationOnError]
           ,[NotificationGroupID]
           ,[Visible]
           ,[ListOrder]
           ,[Active])
     VALUES
           (@MyApplicationID
           ,'Arcadia'
           ,'Reporting' 
           ,'Arcadia'
           ,0
           ,NULL
           ,1
           ,0 
           ,1);		   
  print 'Set process'

-- Find newly inserted process ID
SET @MyProcessID = NULL;
SELECT @MyProcessID = [ProcessID]
  FROM [support].[Process]
  WHERE [Name] = 'Arcadia'
    AND [ApplicationID] = @MyApplicationID;  
IF (@MyProcessID IS NULL) BEGIN
  print 'Process ID not there.'
  RETURN;
END 

print 'ApplicationId = ' + convert([nvarchar] (20), @MyApplicationID)
print 'ProcessId = ' + convert([nvarchar] (20), @MyProcessID)

/*	
  "ReadDirectory": "\\\\ps-nas\\NAS\\SSS\\IT\\Decision Support\\ETL\\Export\\VHAN",
  "ArchiveInputFileThisRunDirectory": "\\\\ps-nas\\NAS\\SSS\\IT\\Decision Support\\ETL\\Export\\VHAN\ArchiveInputFilesThisRun",
  "ArchiveInputFileDirectory": "\\\\ps-nas\\NAS\\SSS\\IT\\Decision Support\\ETL\\Export\\VHAN\\ArchiveInputFiles",
  "ArchiveOutputFileDirectory": "\\\\ps-nas\\NAS\\SSS\\IT\\Decision Support\\ETL\\Export\\VHAN\\ArchiveOutputFiles",
  "ToArcadiaSftpDirectory": "\\\\ps-nas\\NAS\\SSS\\IT\\Decision Support\\ETL\\Export\\VHAN\\ToArcadiaSftpDirectory",
  "MyBaseWebApiUrl": "http://webservices:4222",
  "EmailBaseWebApiUrl": "http://webservices:4001/api/EmailWebApi/SendEmailWithHtmlStringInput",
  "EmailSubject": "VHAN zip file was transferred successfully to SFTP Site",
  "EmailFromAddress": "smgapplications@summithealthcare.com",
  "EmaileesString": "pwmorrison@summithealthcare.com"
*/
--BEGIN TRAN

INSERT INTO [support].[Setting]

           ([ApplicationID]
            ,[ProcessID]
            ,[Name]
            ,[Type]
            ,[Description]
            ,[Value]
            ,[Active])

VALUES (@MyApplicationID
           ,@MyProcessID
           ,'ReadDirectory'
           ,'Default'
           ,''
           ,'\\ps-nas\NAS\SSS\IT\Decision Support\ETL\Export\VHAN'
           ,1)
      ,(@MyApplicationID
           ,@MyProcessID
           ,'ArchiveInputFileThisRunDirectory'
           ,'Default'
           ,''
           ,'\\ps-nas\NAS\SSS\IT\Decision Support\ETL\Export\VHAN\ArchiveInputFileThisRun'
           ,1)
      ,(@MyApplicationID
           ,@MyProcessID
           ,'ArchiveInputFileDirectory'
           ,'Default'
           ,''
           ,'\\ps-nas\NAS\SSS\IT\Decision Support\ETL\Export\VHAN\ArchiveInputFiles'
           ,1)
      ,(@MyApplicationID
           ,@MyProcessID
           ,'ArchiveOutputFileDirectory'
           ,'Default'
           ,''
           ,'\\ps-nas\NAS\SSS\IT\Decision Support\ETL\Export\VHAN\ArchiveOutputFiles'
           ,1)           
      ,(@MyApplicationID
           ,@MyProcessID
           ,'ToArcadiaSftpDirectory'
           ,'Default'
           ,''
           ,'\\ps-nas\NAS\SSS\IT\Decision Support\ETL\Export\VHAN\ToArcadiaSftpDirectory'
           ,1)
      ,(@MyApplicationID
           ,@MyProcessID
           ,'MyBaseWebApiUrl'
           ,'Default'
           ,''
           ,'http://webservices:8114'
           ,1)           
      ,(@MyApplicationID
           ,@MyProcessID
           ,'EmailBaseWebApiUrl'
           ,'Default'
           ,''
           ,'http://webservices:8081/api/EmailWebApi/SendEmailWithHtmlStringInput'
           ,1)
      ,(@MyApplicationID
           ,@MyProcessID
           ,'EmailSubject'
           ,'Default'
           ,''
           ,'VHAN zip file was transferred successfully to SFTP Site'
           ,1)
      ,(@MyApplicationID
           ,@MyProcessID
           ,'EmailFromAddress'
           ,'Default'
           ,''
           ,'smgapplications@summithealthcare.com'
           ,1)
      ,(@MyApplicationID
           ,@MyProcessID
           ,'EmaileesString'
           ,'Default'
           ,''
           ,'pwmorrison@summithealthcare.com'
           ,1);


-- COMMIT TRAN

-- ROLLBACK TRAN