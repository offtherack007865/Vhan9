using log4net;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vhan9.data.Models;

namespace Vhan9.ConsoleApp
{
    public class MainOps
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MainOps));
        public MainOps
                (
                    qy_GetVhanConfigOutputColumns inputConfigOps
                    , List<string> inputSourceFullFilenameList
                )
        {
            MyConfigOptions = inputConfigOps;
            MySourceFullFilenameList = inputSourceFullFilenameList;
        }
        public qy_GetVhanConfigOutputColumns MyConfigOptions { get; set; }
        public string MyFullArchiveFilename { get; set; }
        public List<string> MySourceFullFilenameList { get; set; }
        public List<string> MyFilenameLisForThisRun { get; set; }

        public MainOpsOutput MyMain()
        {
            MainOpsOutput returnOutput = new MainOpsOutput();

            // Build the output zip full filename.
            MyFullArchiveFilename =
                BuildOutputZipFullFilename();

            // Delete all old source files from the last run before copying
            // the files for this run.
            ClearSoureFilesForThisRunDirectory();

            // Copy Input Files to Input Files Archive For This Run
            CopyInputFilesToInputFileArchiveForThisRun();

            // Create Zip File from the files in the new input file
            // archive directory.
            CreateZipFile();

            // Copy the archived output zip file to the 
            // To Arcadia SFTP send queue folder
            CopyOutputZipFileToArcadiaDirectory();

            // Copy Input Files for this run to the
            // Master InputArchive
            CopyInputFilesForThisRunToMasterArchiveDirectory();

            // Delete all old source files from the last run before copying
            // the files for this run.
            ClearSoureFilesForThisRunDirectory();

            // Email notification
            returnOutput.MailBodyLineList.Add($"New Zip file created {MyFullArchiveFilename.Replace("\\", " ")}");

            return returnOutput;
        }

        // Build Zip full filename
        public string BuildOutputZipFullFilename()
        {
            string returnOutput = string.Empty;

            DateTime currentDate = DateTime.Now;

            string myCurrentTimestampString =
                DateTime.Now.ToString($"{currentDate.Year.ToString()}_{currentDate.Month.ToString().PadLeft(2,'0')}_{currentDate.Day.ToString().PadLeft(2,'0')}_{currentDate.Hour.ToString().PadLeft(2, '0')}_{currentDate.Minute.ToString().PadLeft(2, '0')}_{currentDate.Second.ToString().PadLeft(2, '0')}_{currentDate.Microsecond.ToString()}");

            string datedFilenameForZip =
                $"VhanDataFromSummitHealthcare_{myCurrentTimestampString}.zip";
            returnOutput =
                Path.Combine
                (
                    MyConfigOptions.ArchiveOutputFileDirectory
                    , datedFilenameForZip
                );
            if (File.Exists(returnOutput))
            {
                File.Delete(returnOutput);
            }
            return returnOutput;
        }

        // Read Directory Files ==========>>> Input Files For This Run
        public void CopyInputFilesToInputFileArchiveForThisRun()
        {
            int oneBasedFileCounter = 0;
            foreach (string loopInputFilename in MySourceFullFilenameList)
            {
                oneBasedFileCounter++;
                Console.WriteLine
                (
                    $"CopyInputFilesToInputFileArchiveForThisRun {oneBasedFileCounter} of {MySourceFullFilenameList.Count} files copied to this run source file directory"
                );

                FileInfo loopInputFi =
                    new FileInfo(loopInputFilename);
                string loopArchiveFilename =
                    Path.Combine
                    (
                        MyConfigOptions.ArchiveInputFileThisRunDirectory
                        , loopInputFi.Name
                    );

                // Copy the input file to the input file archive folder
                if (File.Exists(loopArchiveFilename))
                {
                    File.Delete(loopArchiveFilename);
                }
                File.Copy(loopInputFilename, loopArchiveFilename);

                // Delete file from Read directory.
                if (File.Exists(loopArchiveFilename))
                {
                    File.Delete(loopInputFilename);
                }
            }

            // After the files are all copied to the "This run archive",
            // get the File list
            MyFilenameLisForThisRun = Directory.GetFiles(MyConfigOptions.ArchiveInputFileThisRunDirectory).ToList();

        }

        // Copy Output Zip File ==========>>> "SFTP to Arcadia" directory
        public void CopyOutputZipFileToArcadiaDirectory()
        {
            FileInfo archiveZipFi =
                new FileInfo(MyFullArchiveFilename);
            string toArcadiaZipFullFilename =
                Path.Combine
                (
                    MyConfigOptions.ToArcadiaSftpDirectory
                    , archiveZipFi.Name
                );
            if (File.Exists(toArcadiaZipFullFilename))
            {
                File.Delete (toArcadiaZipFullFilename);
            }
            File.Copy(MyFullArchiveFilename, toArcadiaZipFullFilename);
        }

        // Input Files for this run ==========>>> Master Archive directory
        public void CopyInputFilesForThisRunToMasterArchiveDirectory()
        {
            int oneBasedFileCounter = 0;
            foreach (string loopInputFilename in MyFilenameLisForThisRun)
            {
                oneBasedFileCounter++;
                Console.WriteLine
                (
                    $"CopyInputFilesForThisRunToMasterArchiveDirector {oneBasedFileCounter} of {MyFilenameLisForThisRun.Count} files copied to this run source file directory"
                );

                FileInfo loopInputFi =
                    new FileInfo(loopInputFilename);
                string loopArchiveFilename =
                    Path.Combine
                    (
                        MyConfigOptions.ArchiveInputFileDirectory
                        , loopInputFi.Name
                    );

                // Copy the input file to the input file archive folder
                if (File.Exists(loopArchiveFilename))
                {
                    File.Delete(loopArchiveFilename);
                }
                File.Copy(loopInputFilename, loopArchiveFilename);
            }
        }

        public void ClearSoureFilesForThisRunDirectory()
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(MyConfigOptions.ArchiveInputFileThisRunDirectory);

            int oneBasedFileCounter = 0;

            foreach (FileInfo file in di.GetFiles())
            {
                oneBasedFileCounter++;
                Console.WriteLine
                (
                    $"{oneBasedFileCounter} files deleted."
                );

                file.Delete();
            }
        }

        public void CreateZipFile()
        {

            // Zip up the input files and place them into the 
            // Output Archive folder.
            using (var fileStream = new FileStream(MyFullArchiveFilename, FileMode.CreateNew))
            {
                using (var archive = new ZipArchive(fileStream, ZipArchiveMode.Create, true))
                {
                    int oneBasedFileCounter = 0;
                    foreach (string loopInputFilename in MyFilenameLisForThisRun)
                    {
                        oneBasedFileCounter++;
                        Console.WriteLine
                        (
                            $"CreateZipFile {oneBasedFileCounter} of {MyFilenameLisForThisRun.Count} files incorporated into Zip File."
                        );

                        FileInfo loopInputFi =
                            new FileInfo(loopInputFilename);
                        using (FileStream fsSource = new FileStream(loopInputFilename, FileMode.Open, FileAccess.Read))
                        {
                            string fileName = loopInputFi.Name;
                            var zipArchiveEntry = archive.CreateEntry(fileName, CompressionLevel.Fastest);
                            using (var zipStream = zipArchiveEntry.Open())
                            {
                                using (StreamReader sr = File.OpenText(loopInputFilename))
                                {
                                    string s = String.Empty;
                                    while ((s = sr.ReadLine()) != null)
                                    {
                                        s = $"{s}\r\n";
                                        byte[] bytes = Encoding.ASCII.GetBytes(s);
                                        zipStream.Write(bytes, 0, bytes.Length);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
