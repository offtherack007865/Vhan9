using EmailWebApiLand9.CallEmailWebApiLand;
using EmailWebApiLand9.Data;
using EmailWebApiLand9.Data.Models;

using log4net;

using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Vhan9.CallWebApiLand;
using Vhan9.data.Models;

using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Vhan9.ConsoleApp
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        // Limit Program to run one instance only.
        public static Process PriorProcess()
        // Returns a System.Diagnostics.Process pointing to
        // a pre-existing process with the same name as the
        // current one, if any; or null if the current process
        // is unique.
        {
            Process curr = Process.GetCurrentProcess();
            Process[] procs = Process.GetProcessesByName(curr.ProcessName);
            foreach (Process p in procs)
            {
                if ((p.Id != curr.Id) &&
                    (p.MainModule.FileName == curr.MainModule.FileName))
                    return p;
            }
            return null;
        }
        public static void Main(string[] args)
        {
            if (PriorProcess() != null)
            {

                log.Error("Another instance of the app is already running.");
                return;
            }

            // configure logging via log4net
            string log4netConfigFullFilename =
                Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "log4net.config");
            var fileInfo = new FileInfo(log4netConfigFullFilename);
            if (fileInfo.Exists)
                log4net.Config.XmlConfigurator.Configure(fileInfo);
            else
                throw new InvalidOperationException("No log config file found");


            // Build a config object, using env vars and JSON providers.
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile(Vhan9.data.MyConstants.AppSettingsFile)
                .Build();

            // Read in Configuration Options for this Console Application
            ReadInConfigOptions myReadInConfigOptions = new ReadInConfigOptions(config);
            Vhan9.data.Models.ConfigOptions
                myConfigOptions =
                    myReadInConfigOptions.ReadIn();

            // Get Config Options from the database.
            CallWebApiLandClass
                myCallForGetOptions =
                    new CallWebApiLandClass
                        (
                            myConfigOptions.BaseWebUrl
                        );

            qy_GetVhanConfigOutput
                myqy_GetConfigOutput =
                    myCallForGetOptions
                    .qy_GetVhanConfig();
            if (!myqy_GetConfigOutput.IsOk ||
                myqy_GetConfigOutput.qy_GetVhanConfigOutputColumnsList.Count != 1)
            {
                log.Error($"We had an error in trying to get the configuration file from the database:  {myqy_GetConfigOutput.ErrorMessage}");
                return;
            }

            GetSourceFileList
                myCallToGetSourceFileList =
                    new GetSourceFileList
                    (
                        myqy_GetConfigOutput.qy_GetVhanConfigOutputColumnsList[0]
                    );
            myCallToGetSourceFileList.DoIt();

            if (myCallToGetSourceFileList.MySourceFileList.Count == 0)
            {
                log.Info($"No files to process.");
                return;
            }


            // Main Operations.
            MainOps
                myMainOps =
                    new MainOps
                    (
                        myqy_GetConfigOutput.qy_GetVhanConfigOutputColumnsList[0]
                        , myCallToGetSourceFileList.MySourceFileList
                    );

            MainOpsOutput
                myMainOutput =
                    myMainOps.MyMain();
            if (!myMainOutput.IsOk)
            {
                log.Error(myMainOutput.ErrorMessage);
                return;
            }

            if (myMainOutput.MailBodyLineList.Count > 0)
            {
                string mySubjectLine = "VHAN data file(s) were zipped and sent to the directory to be SFTPed to Arcadia.";
                string myBody = string.Join("<br>", myMainOutput.MailBodyLineList);   //myMainOutput.EmailBody;

                string[] myEmailAddressArray =
                    myqy_GetConfigOutput
                    .qy_GetVhanConfigOutputColumnsList[0]
                    .EmaileesString
                    .Split(";");

                List<string> myEmailAddressList =
                    new List<string>();
                foreach (string loopEmailAddress in myEmailAddressArray)
                {
                    string trimmedEmailAddress = loopEmailAddress.Trim();
                    if (trimmedEmailAddress.Length > 0)
                    {
                        myEmailAddressList.Add(trimmedEmailAddress);
                    }
                }

                string myFromEmailAddress =
                    myqy_GetConfigOutput
                    .qy_GetVhanConfigOutputColumnsList[0]
                    .EmailFromAddress;

                string myEmailWebApiDotNet7BaseUrl =
                    myqy_GetConfigOutput
                    .qy_GetVhanConfigOutputColumnsList[0]
                    .EmailBaseWebApiUrl;

                // Email the notifyees.
                CallEmailWebApiLand myCallEmailWebApi =
                    new CallEmailWebApiLand
                    (
                        mySubjectLine // string inputEemailSubject
                        , myBody // List<string> inputEmailBodyLineList
                        , myEmailAddressList // List<string> inputEmailAddressList
                        , myFromEmailAddress // string inputFromEmailAddress
                        , myEmailWebApiDotNet7BaseUrl // string inputEmailWebApiBaseUrl
                        , new List<string>() //List<string> inputAttachmentList
                    );
                EmailSendWithHtmlStringOutput
                    myEmailSendWithHtmlStringOutput =
                        myCallEmailWebApi.CallIHtmlStringBody();
                if (!myEmailSendWithHtmlStringOutput.IsOk)
                {
                    log.Error($"Error upon trying to invoke the Email Web Api.  Error Message:  {myEmailSendWithHtmlStringOutput}");
                    return;
                }
            }
        }
    }
}