-- SQL Server Instance:  smg-sql01
USE [Utilities];
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('vhan.qy_GetVhanConfig'))
   DROP PROC [vhan].[qy_GetVhanConfig]
GO
CREATE PROCEDURE [vhan].[qy_GetVhanConfig]
/* -----------------------------------------------------------------------------------------------------------
   Procedure Name  :  qy_GetVhanConfig
   Business Analyis:
   Project/Process :   
   Description     :  Get configuration settings for the 
                      'VHAN' application.
	  
   Author          :  Philip Morrison 
   Create Date     :  8/13/2025

   ***********************************************************************************************************
   **         Change History                                                                                **
   ***********************************************************************************************************

   Date       Version    Author             Description
   --------   --------   -----------        ------------
   8/13/2025  1.01.001   Philip Morrison    Created

*/ -----------------------------------------------------------------------------------------------------------                                   

AS
BEGIN

  -- KeyValueTable
  DECLARE @KeyValueTable Table
  (
    [Name] [nvarchar] (1000)
	,[Value] [nvarchar] (1000)
  );
  
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
}
*/  
  DECLARE @ReadDirectory [nvarchar] (1000) = '';
  DECLARE @ArchiveInputFileThisRunDirectory [nvarchar] (1000) = '';
  DECLARE @ArchiveInputFileDirectory [nvarchar] (1000) = '';
  DECLARE @ArchiveOutputFileDirectory [nvarchar] (1000) = '';
  DECLARE @ToArcadiaSftpDirectory [nvarchar] (1000) = '';
  DECLARE @MyBaseWebApiUrl [nvarchar] (1000) = '';
  DECLARE @EmailBaseWebApiUrl [nvarchar] (1000) = '';
  DECLARE @EmailSubject [nvarchar] (1000) = '';
  DECLARE @EmailFromAddress [nvarchar] (1000) = '';
  DECLARE @EmaileesString [nvarchar] (1000) = '';

-- Template Declarations
DECLARE @Application            varchar(128) = 'Vhan' 
DECLARE @Version                varchar(25)  = '1.00.001'

DECLARE @ProcessID              int          = 0
DECLARE @Process                varchar(128) = 'Arcadia'

DECLARE @BatchOutID             int
DECLARE @BatchDescription       varchar(1000) = @@ServerName + '  - ' + @Version
DECLARE @BatchDetailDescription varchar(1000)
DECLARE @BatchMessage           varchar(MAX)
DECLARE @User                   varchar(128) = SUSER_NAME()

DECLARE @AnticipatedRecordCount int 
DECLARE @ActualRecordCount      int

SET NOCOUNT ON

BEGIN TRY

--  Initialize Batch
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  NULL, 'BatchStart', @BatchDescription, @ProcessID, @Process
----------------------------------------------------------------------------------------------------------------------------------------------------

    SET @BatchDetailDescription = '010/120:  Populate KeyValueTable with call to [administration].[qy_GetApplicationSettings]'
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailStart', @BatchDetailDescription
	
	  SELECT @AnticipatedRecordCount = COUNT(*)
	                                   FROM @KeyValueTable;
	  
      -- Populate KeyValueTable with call to [administration].[qy_GetApplicationSettings]
      INSERT INTO @KeyValueTable
      (
        [Name]
	    ,[Value]
      )
      EXEC [Admin].[Utilities].[administration].[qy_GetApplicationSettings] 'Vhan', 'Default', 'Arcadia', NULL, 'AppUser';
	
    SET @ActualRecordCount = @@ROWCOUNT
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailEnd', NULL, NULL, NULL, @AnticipatedRecordCount, @ActualRecordCount

----------------------------------------------------------------------------------------------------------------------------------------------------
/*	
    [ArchiveInputFileThisRunDirectory]
*/
    SET @BatchDetailDescription = '020/120:  Populate @ArchiveInputFileThisRunDirectory'
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailStart', @BatchDetailDescription
	
	  SELECT @AnticipatedRecordCount = COUNT(*)
      FROM @KeyValueTable
      WHERE [Name] = 'ArchiveInputFileThisRunDirectory';	
	  
      -- Populate @ArchiveInputFileThisRunDirectory
      SELECT @ArchiveInputFileThisRunDirectory = [Value]
      FROM @KeyValueTable
      WHERE [Name] = 'ArchiveInputFileThisRunDirectory';	
	
    SET @ActualRecordCount = @@ROWCOUNT
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailEnd', NULL, NULL, NULL, @AnticipatedRecordCount, @ActualRecordCount

----------------------------------------------------------------------------------------------------------------------------------------------------
/*	
    [ReadDirectory]
*/
    SET @BatchDetailDescription = '030/120:  Populate @ReadDirectory'
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailStart', @BatchDetailDescription
	
	  SELECT @AnticipatedRecordCount = COUNT(*)
      FROM @KeyValueTable
      WHERE [Name] = 'ReadDirectory';	
	  
      -- Populate @ReadDirectory
      SELECT @ReadDirectory = [Value]
      FROM @KeyValueTable
      WHERE [Name] = 'ReadDirectory';	
	
    SET @ActualRecordCount = @@ROWCOUNT
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailEnd', NULL, NULL, NULL, @AnticipatedRecordCount, @ActualRecordCount

----------------------------------------------------------------------------------------------------------------------------------------------------

/*	
      ,[ArchiveInputFileDirectory]
*/
    SET @BatchDetailDescription = '040/120:  Populate @ArchiveInputFileDirectory'
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailStart', @BatchDetailDescription
	
	  SELECT @AnticipatedRecordCount = COUNT(*)
      FROM @KeyValueTable
      WHERE [Name] = 'ArchiveInputFileDirectory';	
	  
      -- Populate @ArchiveInputFileDirectory
      SELECT @ArchiveInputFileDirectory = [Value]
      FROM @KeyValueTable
      WHERE [Name] = 'ArchiveInputFileDirectory';	
	
    SET @ActualRecordCount = @@ROWCOUNT
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailEnd', NULL, NULL, NULL, @AnticipatedRecordCount, @ActualRecordCount

----------------------------------------------------------------------------------------------------------------------------------------------------
/*	
      ,[ArchiveOutputFileDirectory]
*/
    SET @BatchDetailDescription = '050/120:  Populate @ArchiveOutputFileDirectory'
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailStart', @BatchDetailDescription
	
	  SELECT @AnticipatedRecordCount = COUNT(*)
      FROM @KeyValueTable
      WHERE [Name] = 'ArchiveOutputFileDirectory';	
	  
      -- Populate @ArchiveOutputFileDirectory
      SELECT @ArchiveOutputFileDirectory = [Value]
      FROM @KeyValueTable
      WHERE [Name] = 'ArchiveOutputFileDirectory';	
	
    SET @ActualRecordCount = @@ROWCOUNT
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailEnd', NULL, NULL, NULL, @AnticipatedRecordCount, @ActualRecordCount

----------------------------------------------------------------------------------------------------------------------------------------------------
/*	
      ,[ToArcadiaSftpDirectory]
*/
    SET @BatchDetailDescription = '060/120:  Populate @ToArcadiaSftpDirectory'
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailStart', @BatchDetailDescription
	
	  SELECT @AnticipatedRecordCount = COUNT(*)
      FROM @KeyValueTable
      WHERE [Name] = 'ToArcadiaSftpDirectory';	
	  
      -- Populate @ToArcadiaSftpDirectory
      SELECT @ToArcadiaSftpDirectory = [Value]
      FROM @KeyValueTable
      WHERE [Name] = 'ToArcadiaSftpDirectory';	
	
    SET @ActualRecordCount = @@ROWCOUNT
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailEnd', NULL, NULL, NULL, @AnticipatedRecordCount, @ActualRecordCount

----------------------------------------------------------------------------------------------------------------------------------------------------

/*	
      ,[MyBaseWebApiUrl]
*/
    SET @BatchDetailDescription = '070/120:  Populate @MyBaseWebApiUrl'
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailStart', @BatchDetailDescription
	
	  SELECT @AnticipatedRecordCount = COUNT(*)
      FROM @KeyValueTable
      WHERE [Name] = 'MyBaseWebApiUrl';	
	  
      -- Populate @MyBaseWebApiUrl
      SELECT @MyBaseWebApiUrl = [Value]
      FROM @KeyValueTable
      WHERE [Name] = 'MyBaseWebApiUrl';	
	
    SET @ActualRecordCount = @@ROWCOUNT
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailEnd', NULL, NULL, NULL, @AnticipatedRecordCount, @ActualRecordCount

----------------------------------------------------------------------------------------------------------------------------------------------------
/*	
      ,[EmailBaseWebApiUrl]
*/
    SET @BatchDetailDescription = '080/120:  Populate @EmailBaseWebApiUrl'
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailStart', @BatchDetailDescription
	
	  SELECT @AnticipatedRecordCount = COUNT(*)
      FROM @KeyValueTable
      WHERE [Name] = 'EmailBaseWebApiUrl';	
	  
      -- Populate @EmailBaseWebApiUrl
      SELECT @EmailBaseWebApiUrl = [Value]
      FROM @KeyValueTable
      WHERE [Name] = 'EmailBaseWebApiUrl';	
	
    SET @ActualRecordCount = @@ROWCOUNT
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailEnd', NULL, NULL, NULL, @AnticipatedRecordCount, @ActualRecordCount

----------------------------------------------------------------------------------------------------------------------------------------------------
/*	
      ,[EmailSubject]
*/
    SET @BatchDetailDescription = '090/120:  Populate @EmailSubject'
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailStart', @BatchDetailDescription
	
	  SELECT @AnticipatedRecordCount = COUNT(*)
      FROM @KeyValueTable
      WHERE [Name] = 'EmailSubject';	
	  
      -- Populate @EmailSubject
      SELECT @EmailSubject = [Value]
      FROM @KeyValueTable
      WHERE [Name] = 'EmailSubject';	
	
    SET @ActualRecordCount = @@ROWCOUNT
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailEnd', NULL, NULL, NULL, @AnticipatedRecordCount, @ActualRecordCount

----------------------------------------------------------------------------------------------------------------------------------------------------
/*	
      ,[EmailFromAddress]
*/
    SET @BatchDetailDescription = '100/120:  Populate @EmailFromAddress'
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailStart', @BatchDetailDescription
	
	  SELECT @AnticipatedRecordCount = COUNT(*)
      FROM @KeyValueTable
      WHERE [Name] = 'EmailFromAddress';	
	  
      -- Populate @EmailFromAddress
      SELECT @EmailFromAddress = [Value]
      FROM @KeyValueTable
      WHERE [Name] = 'EmailFromAddress';	
	
    SET @ActualRecordCount = @@ROWCOUNT
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailEnd', NULL, NULL, NULL, @AnticipatedRecordCount, @ActualRecordCount

----------------------------------------------------------------------------------------------------------------------------------------------------
/*	
      ,[EmaileesString]
*/
    SET @BatchDetailDescription = '110/120:  Populate @EmaileesString'
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailStart', @BatchDetailDescription
	
	  SELECT @AnticipatedRecordCount = COUNT(*)
      FROM @KeyValueTable
      WHERE [Name] = 'EmaileesString';	
	  
      -- Populate @EmaileesString
      SELECT @EmaileesString = [Value]
      FROM @KeyValueTable
      WHERE [Name] = 'EmaileesString';	
	
    SET @ActualRecordCount = @@ROWCOUNT
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailEnd', NULL, NULL, NULL, @AnticipatedRecordCount, @ActualRecordCount

----------------------------------------------------------------------------------------------------------------------------------------------------
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
}
*/  
    SET @BatchDetailDescription = '120/120:  Populate @ReadDirectory'
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailStart', @BatchDetailDescription
	
	  SET @AnticipatedRecordCount = 1;
	  
      -- Output 
      SELECT 
        @ReadDirectory AS [ReadDirectory]
        ,@ArchiveInputFileThisRunDirectory AS [ArchiveInputFileThisRunDirectory]
        ,@ArchiveInputFileDirectory AS [ArchiveInputFileDirectory]
        ,@ArchiveOutputFileDirectory AS [ArchiveOutputFileDirectory]
        ,@ToArcadiaSftpDirectory AS [ToArcadiaSftpDirectory]
        ,@MyBaseWebApiUrl AS [MyBaseWebApiUrl]
        ,@EmailBaseWebApiUrl AS [EmailBaseWebApiUrl]
        ,@EmailSubject AS [EmailSubject]
        ,@EmailFromAddress AS [EmailFromAddress]
        ,@EmaileesString AS [EmaileesString];
	
    SET @ActualRecordCount = @@ROWCOUNT
    EXEC Admin.Utilities.logs.di_Batch @BatchOutID OUTPUT,  @BatchOutID, 'DetailEnd', NULL, NULL, NULL, @AnticipatedRecordCount, @ActualRecordCount

----------------------------------------------------------------------------------------------------------------------------------------------------

--  Close batch
    EXEC Admin.Utilities.logs.di_batch @BatchOutID OUTPUT, @BatchOutID, 'BatchEnd'

END TRY


BEGIN CATCH
DECLARE @Err              int
     ,  @ErrorMessage     varchar(Max)
     ,  @ErrorLine        varchar(128)
     ,  @Workstation      varchar(128) = @Application
     ,  @Procedure        VARCHAR(500)

    IF ERROR_NUMBER() IS NULL 
      SET @Err =0;
    ELSE
      SET @Err = ERROR_NUMBER();

    SET @ErrorMessage = ERROR_MESSAGE()
    SET @ErrorLine    = 'SP Line Number: ' + CAST(ERROR_LINE() as varchar(10)) 
    
	SET @Workstation  = HOST_NAME()
	
    SET @Procedure    = @@SERVERNAME + '.' + DB_NAME() + '.' + OBJECT_SCHEMA_NAME(@@ProcID) + '.' + OBJECT_NAME(@@ProcID) + ' - ' + @ErrorLine + ' - ' + LEFT(@BatchDetailDescription, 7)
    EXEC Admin.Utilities.administration.di_ErrorLog  @Application ,@Process, @Version ,0, @ErrorMessage, @Procedure,  @User , @Workstation

    SET @BatchMessage = 'Process Failed:  ' +  @ErrorMessage
    EXEC Admin.Utilities.logs.di_batch @BatchOutID OUTPUT, @BatchOutID, 'BatchEnd', @BatchMessage
	
    RAISERROR(@ErrorMessage, 16,1)

END CATCH


END