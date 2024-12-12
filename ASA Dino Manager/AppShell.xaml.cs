﻿//using Android.Nfc;
using Microsoft.Maui.Controls;

namespace ASA_Dino_Manager
{
    public partial class AppShell : Shell
    {


        // IMPORTING
        public static bool Importing = false;
        public static bool ImportEnabled = false;
        public static int Delay = 5;
        public static int DefaultDelay = 30; // default import delay in seconds

        public static int tagSize = 0;

        // benchmark stuff
        private static int ImportCount = 0;
        private static double ImportAvg = 0; // keep track of average import time

        private bool _isTimerRunning = false; // Timer control flag

        public AppShell()
        {
            InitializeComponent();

            if (!FileManager.InitFileManager())
            {
                // Exit app here
                Application.Current.Quit();
            }
            if (!DataManager.InitDataManager())
            {
                // Exit app here
                Application.Current.Quit();
            }


            PopulateShellContents();

            StartTimer();
        }


        private void ClearShell()
        {
            Items.Clear();
            var shellContent = new ShellContent
            {
                Title = "Dino Archive",
                ContentTemplate = new DataTemplate(typeof(MainPage)), // Replace with the appropriate page
                Route = "Dino Archive"
            };

            // Add the ShellContent to the Shell
            Items.Add(shellContent);
        }


        private void PopulateShellContents()
        {
            string[] classList = DataManager.GetAllClasses();
            string[] tagList = DataManager.GetAllDistinctColumnData("Tag");

            if (tagList.Length > tagSize)
            {
                tagSize = tagList.Length;
                //ClearShell();
                Items.Clear();

                // Retrieve the tag list from DataManager and sort alphabetically
                var sortedTagList = classList.OrderBy(tag => tag).ToArray();

                if (sortedTagList.Length < 1)
                {
                    var shellContent = new ShellContent
                    {
                        Title = "Dino Species",
                        ContentTemplate = new DataTemplate(typeof(MainPage)), // Replace with the appropriate page
                        Route = "Dino Species"
                    };

                    // Add the ShellContent to the Shell
                    Items.Add(shellContent);
                    return; // exit early if the tagList is empty
                }

                // Loop through the sorted tags and create ShellContent dynamically
                foreach (var tag in sortedTagList)
                {
                    var shellContent = new ShellContent
                    {
                        Title = tag,
                        ContentTemplate = new DataTemplate(typeof(MainPage)), // Replace with the appropriate page
                        Route = tag
                    };

                    // Add the ShellContent to the Shell
                    Items.Add(shellContent);
                }
            }
            else
            {
               // StartImport();
            }

        }

        public static void StartImport()
        {
            if (ImportEnabled)
            {
                if (FileManager.CheckPath(FileManager.GamePath))
                {
                    if (!Importing)
                    {
                        FileManager.Log("Starting Import thread");
                        Thread thread = new Thread(delegate () { // start new thread
                            try
                            {
                                Importing = true;
                                var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                                DataManager.Import();
                                stopwatch.Stop();
                                var elapsedMilliseconds = stopwatch.Elapsed.TotalMilliseconds;
                                Importing = false;
                                FileManager.SaveFiles();
                                ImportCount++;
                                double outAVG = 0;
                                if (ImportCount < 2) { ImportAvg = elapsedMilliseconds; outAVG = ImportAvg; }
                                else { ImportAvg += elapsedMilliseconds; outAVG = ImportAvg / ImportCount; }

                                FileManager.Log("Imported files in " + elapsedMilliseconds + "ms" + " Avg: " + outAVG);
                                Delay = DefaultDelay; // only start up timer after scanning is done 
                            }
                            catch
                            {
                                FileManager.Log("Import thread failure");
                                Delay = DefaultDelay; // infinite retries????????
                            }
                        });
                        thread.Start();
                    }
                    else
                    {
                        FileManager.Log("DataBase locked");
                        // restart timer if database is locked
                        Delay = DefaultDelay;
                    }
                }
                else
                {
                    // scan for a new path and save it
                    FileManager.ScanPath();
                    Delay = DefaultDelay;
                }
            }
        }


        private void StartTimer()
        {
            _isTimerRunning = true; // Flag to control the timer

            Device.StartTimer(TimeSpan.FromSeconds(5), () =>
            {
                if (!_isTimerRunning)
                    return false; // Stop the timer

                TriggerFunction();
                return true; // Continue running the timer
            });
        }

        private void TriggerFunction()
        {
            //StartImport();
            // Logic to be executed every 5 seconds
            Console.WriteLine($"Function triggered at {DateTime.Now}");
            //PopulateShellContents();
            string[] tagList = DataManager.GetAllDistinctColumnData("Tag");


            


            if (FileManager.GamePath != "")
            {
                ImportEnabled = true;
                StartImport();
            }
            else
            {
                ImportEnabled = false;
            }

            PopulateShellContents();
        }

        public void StopTimer()
        {
            _isTimerRunning = false; // Call this to stop the timer if needed
        }


    }
}
