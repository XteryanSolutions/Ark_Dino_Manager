﻿using Microsoft.Maui.ApplicationModel;
using System.Data;
using System.Data.Common;
using System.Globalization;

namespace Ark_Dino_Manager
{
    class DataManager
    {
        // how many different dino classes do we have
        public static int classSize = 0;


        public static DataSet ImportsDataSet = new DataSet("importsDataSet");
        public static DataTable ImportsTable = new DataTable("importsList");

        public static DataSet FemaleDataSet = new DataSet("dinoDataSet");
        public static DataTable FemaleTable = new DataTable("dinoList");

        public static DataSet MaleDataSet = new DataSet("dinoDataSet");
        public static DataTable MaleTable = new DataTable("dinoList");

        public static DataSet BottomDataSet = new DataSet("dinoDataSet");
        public static DataTable BottomTable = new DataTable("dinoList");

        public static DataSet StatDataSet = new DataSet("dinoDataSet");
        public static DataTable StatTable = new DataTable("dinoList");

        public static DataSet ArchiveDataSet = new DataSet("dinoDataSet");
        public static DataTable ArchiveTable = new DataTable("dinoList");

        public static int ModC = 0;  // keep track of updated dinos
        public static int AddC = 0;  // keep track of added dinos

        public static string DecimalSeparator = "";
        private static string ThousandsSeparator = "";

        private static readonly CultureInfo Culture = Thread.CurrentThread.CurrentCulture;

        // dino stats
        public static double LevelMax = 0;
        public static double HpMax = 0;
        public static double StaminaMax = 0;
        public static double O2Max = 0;
        public static double FoodMax = 0;
        public static double WeightMax = 0;
        public static double DamageMax = 0;
        public static double SpeedMax = 0;
        public static double CraftMax = 0;



        public static bool InitDataManager()
        {
            try
            {
                DecimalSeparator = Culture.NumberFormat.CurrencyDecimalSeparator;
                ThousandsSeparator = Culture.NumberFormat.CurrencyGroupSeparator;


                ImportsDataSet.Tables.Add(ImportsTable);
                FemaleDataSet.Tables.Add(FemaleTable);
                MaleDataSet.Tables.Add(MaleTable);
                BottomDataSet.Tables.Add(BottomTable);
                StatDataSet.Tables.Add(StatTable);
                ArchiveDataSet.Tables.Add(ArchiveTable);


                ImportsTable.Clear();
                ImportsTable.Columns.Add("ID", typeof(string));
                ImportsTable.Columns.Add("Tag", typeof(string));
                ImportsTable.Columns.Add("Sex", typeof(string));
                ImportsTable.Columns.Add("Name", typeof(string));
                ImportsTable.Columns.Add("Level", typeof(string));
                ImportsTable.Columns.Add("HP", typeof(string));
                ImportsTable.Columns.Add("Stamina", typeof(string));
                ImportsTable.Columns.Add("Oxygen", typeof(string));
                ImportsTable.Columns.Add("Food", typeof(string));
                ImportsTable.Columns.Add("Weight", typeof(string));
                ImportsTable.Columns.Add("Damage", typeof(string));
                ImportsTable.Columns.Add("Speed", typeof(string));
                ImportsTable.Columns.Add("Mama", typeof(string));
                ImportsTable.Columns.Add("Papa", typeof(string));
                ImportsTable.Columns.Add("MamaMute", typeof(string));
                ImportsTable.Columns.Add("PapaMute", typeof(string));
                ImportsTable.Columns.Add("Gen", typeof(string));
                ImportsTable.Columns.Add("GenM", typeof(string));
                ImportsTable.Columns.Add("BabyAge", typeof(string));
                ImportsTable.Columns.Add("Neutered", typeof(string));
                ImportsTable.Columns.Add("Imprint", typeof(string));
                ImportsTable.Columns.Add("Imprinter", typeof(string));
                ImportsTable.Columns.Add("Tribe", typeof(string));
                ImportsTable.Columns.Add("Class", typeof(string));
                ImportsTable.Columns.Add("Time", typeof(string));
                ImportsTable.Columns.Add("Colors", typeof(string));
                ImportsTable.Columns.Add("CraftSkill", typeof(string));



                FemaleTable.Clear();
                FemaleTable.Columns.Add("Status", typeof(string));
                FemaleTable.Columns.Add("Name", typeof(string));
                FemaleTable.Columns.Add("Level", typeof(double));
                // ==============
                FemaleTable.Columns.Add("HP", typeof(double));
                FemaleTable.Columns.Add("Stamina", typeof(double));
                FemaleTable.Columns.Add("O2", typeof(double));
                FemaleTable.Columns.Add("Food", typeof(double));
                FemaleTable.Columns.Add("Weight", typeof(double));
                FemaleTable.Columns.Add("Damage", typeof(double));
                FemaleTable.Columns.Add("Crafting", typeof(double));
                // ==============
                FemaleTable.Columns.Add("Speed", typeof(double));
                FemaleTable.Columns.Add("Gen", typeof(double));
                FemaleTable.Columns.Add("Papa", typeof(string));
                FemaleTable.Columns.Add("Mama", typeof(string));
                FemaleTable.Columns.Add("MamaMute", typeof(double));
                FemaleTable.Columns.Add("PapaMute", typeof(double));
                FemaleTable.Columns.Add("Age", typeof(double));
                FemaleTable.Columns.Add("Imprint", typeof(double));
                FemaleTable.Columns.Add("Imprinter", typeof(string));
                FemaleTable.Columns.Add("ID", typeof(string));
                FemaleTable.Columns.Add("Tag", typeof(string));
                FemaleTable.Columns.Add("Mutes", typeof(string));
                FemaleTable.Columns.Add("Group", typeof(string));
                FemaleTable.Columns.Add("Res", typeof(string));
                // babytab stuff
                FemaleTable.Columns.Add("DateT", typeof(string));
                FemaleTable.Columns.Add("Time", typeof(double));
                FemaleTable.Columns.Add("Rate", typeof(double));



                MaleTable.Clear();
                MaleTable.Columns.Add("Status", typeof(string));
                MaleTable.Columns.Add("Name", typeof(string));
                MaleTable.Columns.Add("Level", typeof(double));
                // ==============
                MaleTable.Columns.Add("HP", typeof(double));
                MaleTable.Columns.Add("Stamina", typeof(double));
                MaleTable.Columns.Add("O2", typeof(double));
                MaleTable.Columns.Add("Food", typeof(double));
                MaleTable.Columns.Add("Weight", typeof(double));
                MaleTable.Columns.Add("Damage", typeof(double));
                MaleTable.Columns.Add("Crafting", typeof(double));
                // ==============
                MaleTable.Columns.Add("Speed", typeof(double));
                MaleTable.Columns.Add("Gen", typeof(double));
                MaleTable.Columns.Add("Papa", typeof(string));
                MaleTable.Columns.Add("Mama", typeof(string));
                MaleTable.Columns.Add("MamaMute", typeof(double));
                MaleTable.Columns.Add("PapaMute", typeof(double));
                MaleTable.Columns.Add("Age", typeof(double));
                MaleTable.Columns.Add("Imprint", typeof(double));
                MaleTable.Columns.Add("Imprinter", typeof(string));
                MaleTable.Columns.Add("ID", typeof(string));
                MaleTable.Columns.Add("Tag", typeof(string));
                MaleTable.Columns.Add("Mutes", typeof(string));
                MaleTable.Columns.Add("Group", typeof(string));
                MaleTable.Columns.Add("Res", typeof(string));
                // babytab stuff
                MaleTable.Columns.Add("DateT", typeof(string));
                MaleTable.Columns.Add("Time", typeof(double));
                MaleTable.Columns.Add("Rate", typeof(double));


                BottomTable.Clear();
                BottomTable.Columns.Add("Status", typeof(string));
                BottomTable.Columns.Add("Name", typeof(string));
                BottomTable.Columns.Add("Level", typeof(double));
                // ==============
                BottomTable.Columns.Add("HP", typeof(double));
                BottomTable.Columns.Add("Stamina", typeof(double));
                BottomTable.Columns.Add("O2", typeof(double));
                BottomTable.Columns.Add("Food", typeof(double));
                BottomTable.Columns.Add("Weight", typeof(double));
                BottomTable.Columns.Add("Damage", typeof(double));
                // ==============
                BottomTable.Columns.Add("Speed", typeof(double));
                BottomTable.Columns.Add("Gen", typeof(double));
                BottomTable.Columns.Add("Papa", typeof(string));
                BottomTable.Columns.Add("Mama", typeof(string));
                BottomTable.Columns.Add("MamaMute", typeof(double));
                BottomTable.Columns.Add("PapaMute", typeof(double));
                BottomTable.Columns.Add("Age", typeof(double));
                BottomTable.Columns.Add("Imprint", typeof(double));
                BottomTable.Columns.Add("Imprinter", typeof(string));
                BottomTable.Columns.Add("ID", typeof(string));
                BottomTable.Columns.Add("Crafting", typeof(double));
                BottomTable.Columns.Add("Mutes", typeof(string));
                BottomTable.Columns.Add("Group", typeof(string));
                BottomTable.Columns.Add("Res", typeof(string));


                StatTable.Clear();
                StatTable.Columns.Add("ID", typeof(string));
                StatTable.Columns.Add("Status", typeof(string));
                StatTable.Columns.Add("Notes", typeof(string));
                StatTable.Columns.Add("Mutes", typeof(string));
                StatTable.Columns.Add("Rate", typeof(string));



                ArchiveTable.Clear();
                ArchiveTable.Columns.Add("ID", typeof(string));
                ArchiveTable.Columns.Add("Tag", typeof(string));
                ArchiveTable.Columns.Add("Name", typeof(string));
                ArchiveTable.Columns.Add("Level", typeof(double));
                ArchiveTable.Columns.Add("Class", typeof(string));


                FileManager.LoadFiles();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void MergeDatabases(string dataLocation)
        {
            // Load the second table from XML
            DataTable table2 = new DataTable();
            table2.ReadXml(dataLocation);

            // Clone structure from the ImportsTable
            DataTable mergedTable = ImportsTable.Clone();

            // Combine all rows from ImportsTable and table2
            DataTable allRows = ImportsTable.Copy();
            allRows.Merge(table2);

            // Group by Dino ID and select the most recent entry (ordered by oldest first)
            var groupedRows = allRows.AsEnumerable()
                .GroupBy(row => row["ID"].ToString())
                .Select(group =>
                {
                    // Parse "Time" as DateTime (assume all timestamps are in UTC)
                    return group.OrderBy(row =>
                        DateTime.ParseExact(row["Time"].ToString(), "dd/MM/yyyy HH:mm:ss", Culture, DateTimeStyles.AssumeUniversal))
                        .First();
                });

            // Clear ImportsTable and add the most recent rows from merged data
            ImportsTable.Clear();
            foreach (var row in groupedRows)
            {
                ImportsTable.ImportRow(row);
            }
        }

        public static void DeepCleanDatabase()
        {
            // Get all distinct IDs from the database.
            string[] idList = GetAllDistinctColumnData("ID");
            int totalClean = 0;
            foreach (string id in idList)
            {
                // mutation detection here to only do it when data changes
                MutationDetection(id);

                //var rows = ImportsTable.Select($"ID = '{id}'", "Time ASC");

                var rows = ImportsTable.AsEnumerable()
    .Where(row => row.Field<string>("ID") == id)
    .OrderBy(row => DateTime.ParseExact(row.Field<string>("Time"), "dd/MM/yyyy HH:mm:ss", Culture))
    .ToArray();


                // figure out if its still a baby and we only need to save firsty and last rows
                double firstAge = ToDouble(GetFirstColumnData("ID", id, "BabyAge")) * 100;
                double lastAge = ToDouble(GetLastColumnData("ID", id, "BabyAge")) * 100;
                bool isBaby = false; bool beenBaby = false;
                if (lastAge < 100) { isBaby = true; }
                if (firstAge < 100) { beenBaby = true; }


                string str1 = ""; string str2 = ""; string str3 = ""; string str4 = "";
                if (isBaby) // 2 // is baby means only 2 rows need to be saved
                {
                    str1 = GetFirstColumnData("ID", id, "Time");
                    str4 = GetLastColumnData("ID", id, "Time");
                    if (str1 == "" || str4 == "") // need both
                    {
                        str1 = ""; str2 = ""; str3 = ""; str4 = "";
                    }
                }
                else if (!isBaby && beenBaby) // 4
                {
                    str1 = GetFirstColumnData("ID", id, "Time");

                    foreach (var row in rows)
                    {
                        string timeValue = row["Time"].ToString();
                        double age = ToDouble(row["BabyAge"].ToString()) * 100;

                        if (age < 100)
                        {
                            str2 = timeValue; // last baby time
                        }
                        if (age == 100)
                        {
                            str3 = timeValue; // first adult time
                            break;
                        }
                    }

                    str4 = GetLastColumnData("ID", id, "Time");
                    if (str1 == "" || str2 == "" || str3 == "" || str4 == "") // need all 4
                    {
                        str1 = ""; str2 = ""; str3 = ""; str4 = "";
                    }
                }
                else if (!isBaby && !beenBaby) // 2 // not a baby and never been = new tame
                {
                    str1 = GetFirstColumnData("ID", id, "Time");
                    str4 = GetLastColumnData("ID", id, "Time");
                    if (str1 == "" || str4 == "") // need both
                    {
                        str1 = ""; str2 = ""; str3 = ""; str4 = "";
                    }
                }


                int rowID = 0; int savedRows = 0; int delRows = 0;
                foreach (var row in rows)
                {
                    string timeValue = row["Time"].ToString();
                    string ageValue = row["BabyAge"].ToString();

                    if (timeValue == str1 || timeValue == str2 || timeValue == str3 || timeValue == str4)
                    {
                        // save theese rows
                        savedRows++;
                        double age = ToDouble(ageValue) * 100;
                        //  FileManager.Log($"Saved row: {age} - {timeValue}", 1);
                    }
                    else
                    {
                        delRows++; totalClean++;
                        double age = ToDouble(ageValue) * 100;
                        //  FileManager.Log($"Deleted row: {age} - {timeValue}", 1);
                        ImportsTable.Rows.Remove(row);
                    }
                    rowID++;
                }
                if (delRows > 0)
                {
                    string name = GetLastColumnData("ID", id, "Name");
                    FileManager.Log($"==================================", 1);
                    FileManager.Log($"Dino: {name}", 1);
                    if (isBaby) { FileManager.Log($"is Baby", 1); }
                    else if (beenBaby) { FileManager.Log($"been Baby", 1); }
                    else { FileManager.Log($"only adult", 1); }
                    FileManager.Log($"Saved rows: {savedRows} - Deleted rows: {delRows}", 1);
                }
            }
            ImportsTable.AcceptChanges();
            if (totalClean > 0)
            {
                FileManager.Log($"====================================================", 1);
                FileManager.Log($"Total rows deleted: {totalClean}", 1);
                FileManager.Log($"================= Database Cleaned =================", 1);
                FileManager.needSave = true;
            }
        }

        public static string LongClassToShort(string longClass)
        {
            string dinoClass = "";
            if (longClass.ToUpper().Contains("MAGIC"))
            {
                // magicland stuff
                // <Class>/Forglar/Forglar_All/Dinos/MagicLand/Therizino_Character_BP_Magic.Therizino_Character_BP_Magic_C</Class>

                var split = longClass.Split(new[] { @"_Character_BP" }, StringSplitOptions.RemoveEmptyEntries);

                // <Class>/Forglar/Forglar_All/Dinos/MagicLand/Therizino    _Magic.Therizino_       _Magic_C</Class>

                dinoClass = split[1].Replace("_", "").Trim();// Magic.Therizino
                dinoClass = dinoClass.Replace(".", "_");// Magic_Therizino

            }
            else if (longClass.ToUpper().Contains("BIONIC"))
            {
                // bionic stuff (TEK)
                // <Class>/Game/PrimalEarth/Dinos/Stego/BionicStego_Character_BP.BionicStego_Character_BP_C</Class>
                var classSplit = longClass.Split(new[] { @"/" }, StringSplitOptions.RemoveEmptyEntries);

                // <Class>  Game    PrimalEarth     Dinos   Stego   BionicStego_Character_BP.BionicStego_Character_BP_C<    Class>
                if (classSplit.Length > 2)
                {
                    dinoClass = $"TEK_{classSplit[3].Trim()}";
                }
            }
            else if (longClass.ToUpper().Contains("ABERRANT"))
            {
                // aberrant stuff
                // <Class>/Game/PrimalEarth/Dinos/Lystrosaurus/Lystro_Character_BP_Aberrant.Lystro_Character_BP_Aberrant_C</Class>

                var classSplit = longClass.Split(new[] { @"/" }, StringSplitOptions.RemoveEmptyEntries);

                // <Class>  Game    PrimalEarth     Dinos   Lystrosaurus    Lystro_Character_BP_Aberrant.Lystro_Character_BP_Aberrant_C<    Class>

                if (classSplit.Length > 2)
                {
                    dinoClass = $"Aberrant_{classSplit[3].Trim()}";
                }
            }
            else
            {
                // normal stuff
                // <Class>/Game/PrimalEarth/Dinos/Doedicurus/Doed_Character_BP.Doed_Character_BP_C</Class>
                // <Class>/Game/PrimalEarth/Dinos/Lystrosaurus/Lystro_Character_BP.Lystro_Character_BP_C</Class>
                // <Class>/Game/PrimalEarth/Dinos/Ankylo/Ankylo_Character_BP.Ankylo_Character_BP_C</Class>

                var classSplit = longClass.Split(new[] { @"/" }, StringSplitOptions.RemoveEmptyEntries);

                // <Class>  Game    PrimalEarth     Dinos   Doedicurus      Doed_Character_BP.Doed_Character_BP_C<      Class>
                // <Class>  Game    PrimalEarth     Dinos   Lystrosaurus    Lystro_Character_BP.Lystro_Character_BP_C<  Class>
                // <Class>  Game    PrimalEarth     Dinos   Ankylo          Ankylo_Character_BP.Ankylo_Character_BP_C<  Class>

                if (classSplit.Length > 2)
                {
                    dinoClass = classSplit[3].Trim();
                }
            }
            return dinoClass;
        }

        public static string[] GetAllClassesShort(string exclude = "")
        {
            // get the raw unprocessed classlist
            string[] classList = DataManager.GetAllDistinctColumnData("Class");

            // Define exclusions
            string[] excludes = { "N/A", "#", exclude };

            // Use a HashSet to store distinct values
            HashSet<string> resultSet = new HashSet<string>();

            // Iterate through the rows of the list
            foreach (string rawClass in classList)
            {
                string dinoClass = LongClassToShort(rawClass);

                // Check if the value is not excluded and is not null/empty
                if (!excludes.Contains(dinoClass) && !string.IsNullOrWhiteSpace(dinoClass))
                {
                    // Add the value to the result set (distinct)
                    resultSet.Add(dinoClass);
                }
            }

            // Return the distinct values as an array
            return resultSet.ToArray();
        }

        public static string ShortClassToLong(string shortClass)
        {
            string result = "";

            foreach (DataRow row in ImportsTable.Rows)
            {
                string longClass = row["Class"].ToString();


                if (shortClass.ToUpper().Contains("MAGIC"))
                {
                    // <Class>/Forglar/Forglar_All/Dinos/MagicLand/Therizino_Character_BP_Magic.Therizino_Character_BP_Magic_C</Class>
                    if (longClass.Contains(shortClass.Replace("_", ".")))
                    {
                        return row["Class"].ToString();
                    }
                }
                else if (shortClass.ToUpper().Contains("TEK"))
                {
                    // <Class>/Game/PrimalEarth/Dinos/Stego/BionicStego_Character_BP.BionicStego_Character_BP_C</Class>
                    if (longClass.Contains(shortClass.Replace("TEK_", "Bionic")))
                    {
                        return row["Class"].ToString();
                    }
                }
                else if (shortClass.ToUpper().Contains("ABERRANT"))
                {
                    // <Class>/Game/PrimalEarth/Dinos/Lystrosaurus/Lystro_Character_BP_Aberrant.Lystro_Character_BP_Aberrant_C</Class>
                    if (longClass.Contains("Aberrant"))
                    {
                        if (longClass.Contains(shortClass.Replace("Aberrant_", "")))
                        {
                            return row["Class"].ToString();
                        }
                    }
                }
                else
                {
                    // <Class>/Game/PrimalEarth/Dinos/Doedicurus/Doed_Character_BP.Doed_Character_BP_C</Class>
                    if (longClass.Contains(shortClass))
                    {
                        return row["Class"].ToString();
                    }
                }
            }
            return result;
        }

        public static string[] GetDistinctFilteredColumnData(string inColumn1, string inData, string inColumn2, string inData2, string outData, string exclude = "")
        {
            // Define exclusions
            string[] excludes = { "N/A", "#", exclude };

            // Use a HashSet to store distinct values
            HashSet<string> resultSet = new HashSet<string>();

            // Iterate through the rows of the table
            foreach (DataRow row in ImportsTable.Rows)
            {
                // Retrieve the values from the columns
                string value1 = row[inColumn1]?.ToString();
                string value2 = row[inColumn2]?.ToString();
                string outputValue = row[outData]?.ToString();

                // Apply the filtering conditions
                if (value1 == inData &&
                    value2 == inData2 &&
                    !excludes.Contains(value1) &&
                    !excludes.Contains(value2) &&
                    !string.IsNullOrWhiteSpace(outputValue))  // Ensure output is not empty or null
                {
                    // Add the output value to the result set (distinct)
                    resultSet.Add(outputValue);
                }
            }

            // Return the distinct values as an array
            return resultSet.ToArray();
        }

        public static string[] GetDistinctFilteredColumnDataFull(string outData, string exclude = "")
        {
            // Define exclusions
            string[] excludes = { "N/A", "#", exclude };

            // Use a HashSet to store distinct values
            HashSet<string> resultSet = new HashSet<string>();

            // Iterate through the rows of the table
            foreach (DataRow row in ImportsTable.Rows)
            {
                // Retrieve the values from the columns
                string value1 = row["BabyAge"]?.ToString();
                string outputValue = row[outData]?.ToString();

                // Apply the filtering conditions
                if (ToDouble(value1) == 1) // get the fullgrowns
                {
                    if (!string.IsNullOrWhiteSpace(outputValue))  // Ensure output is not empty or null
                    {
                        // Add the output value to the result set (distinct)
                        resultSet.Add(outputValue);
                    }
                }
            }

            // Return the distinct values as an array
            return resultSet.ToArray();
        }

        public static string[] GetDistinctFilteredColumnDataB(string inColumn2, string inData2, string outData, string exclude = "")
        {
            // Define exclusions
            string[] excludes = { "N/A", "#", exclude };

            // Use a HashSet to store distinct values
            HashSet<string> resultSet = new HashSet<string>();


            // make a list of id's to exclude that has fullgrown
            string[] grownUps = DataManager.GetDistinctFilteredColumnDataFull("ID");


            // Iterate through the rows of the table
            foreach (DataRow row in ImportsTable.Rows)
            {
                // Retrieve the values from the columns
                string value1 = row["BabyAge"]?.ToString();
                string value2 = row[inColumn2]?.ToString();
                string outputValue = row[outData]?.ToString();

                // Apply the filtering conditions
                if (ToDouble(value1) < 1) // get only the babies
                {
                    if (value2 == inData2 &&
                        !excludes.Contains(value2) &&
                        !grownUps.Contains(outputValue) && // make sure we exclude grownUps
                        !string.IsNullOrWhiteSpace(outputValue))  // Ensure output is not empty or null
                    {
                        // Add the output value to the result set (distinct)
                        resultSet.Add(outputValue);
                    }
                }
            }

            // Return the distinct values as an array
            return resultSet.ToArray();
        }

        public static string[] GetAllDistinctColumnData(string inColumn, string exclude = "")
        {
            // Define exclusions
            string[] excludes = { "N/A", "#", exclude };

            // Use a HashSet to store distinct values
            HashSet<string> resultSet = new HashSet<string>();

            // Iterate through the rows of the table
            foreach (DataRow row in ImportsTable.Rows)
            {
                // Retrieve the value from the specified column
                string columnValue = row[inColumn]?.ToString();

                // Check if the value is not excluded and is not null/empty
                if (!excludes.Contains(columnValue) && !string.IsNullOrWhiteSpace(columnValue))
                {
                    // Add the value to the result set (distinct)
                    resultSet.Add(columnValue);
                }
            }

            // Return the distinct values as an array
            return resultSet.ToArray();
        }

        public static string GetFirstColumnData(string inColumn, string inData, string outColumn, string exclude = "")
        {
            // get first non empty field
            string[] excludes = { "N/A", "#", exclude };
            string result = "";

            // Iterate through the rows of the table
            foreach (DataRow row in ImportsTable.Rows)
            {
                // Match the input column value
                if (row[inColumn]?.ToString() == inData && !excludes.Contains(row[inColumn]?.ToString()))
                {
                    // Get result if the output column value is not excluded
                    if (!excludes.Contains(row[outColumn]?.ToString()))
                    {
                        return row[outColumn]?.ToString();
                    }
                }
            }
            return result;
        }

        public static string GetLastColumnData(string inColumn, string inData, string outColumn, string exclude = "")
        {
            // Define excluded values
            string[] excludes = { "N/A", "#", exclude };
            string result = "";

            // Iterate through the rows of the table
            foreach (DataRow row in ImportsTable.Rows)
            {
                // Match the input column value
                if (row[inColumn]?.ToString() == inData && !excludes.Contains(row[inColumn]?.ToString()))
                {
                    // Update result if the output column value is not excluded
                    if (!excludes.Contains(row[outColumn]?.ToString()) &&
                        !string.IsNullOrWhiteSpace(row[outColumn]?.ToString())) // Avoid empty/whitespace values
                    {
                        result = row[outColumn]?.ToString();
                    }
                }
            }
            return result;
        }

        public static List<string[]> GetFirstStats(string[] dinos)
        {
            var results = new List<string[]>();
            foreach (string id in dinos)
            {
                string[] columns = {
                    "Name", "Level", "HP", "Stamina", "Oxygen", "Food", "Weight", "Damage", "Speed"
                    , "Mama", "Papa", "MamaMute", "PapaMute", "Gen", "GenM", "BabyAge", "Neutered", "Imprint"
                    , "Imprinter", "Tribe", "CraftSkill"
                };
                string[] stats = new string[columns.Count()];
                int i = 0;
                while (i < columns.Count())
                {
                    stats[i] = DataManager.GetFirstColumnData("ID", id, columns[i]);
                    i++;
                }
                results.Add(stats);
            }
            return results;
        }

        public static List<string[]> GetLastStats(string[] dinos)
        {
            var results = new List<string[]>();
            foreach (string id in dinos)
            {
                string[] columns = {
                    "Name", "Level", "HP", "Stamina", "Oxygen", "Food", "Weight", "Damage", "Speed"
                    , "Mama", "Papa", "MamaMute", "PapaMute", "Gen", "GenM", "BabyAge", "Neutered", "Imprint"
                    , "Imprinter", "Tribe", "CraftSkill"
                };
                string[] stats = new string[columns.Count()];
                int i = 0;
                while (i < columns.Count())
                {
                    stats[i] = DataManager.GetLastColumnData("ID", id, columns[i], "0");
                    i++;
                }
                results.Add(stats);
            }
            return results;
        }

        public static double MaxGenerations(string DinoClass)
        {
            // Retrieve female data
            string[] females = GetDistinctFilteredColumnData("Class", DinoClass, "Sex", "Female", "ID");

            // Retrieve male data
            string[] males = GetDistinctFilteredColumnData("Class", DinoClass, "Sex", "Male", "ID");

            double maxGen = 0;
            foreach (var dino in males)
            {
                string group = GetGroup(dino);
                if (group != "Archived")
                {
                    string gen = GetFirstColumnData("ID", dino, "Gen");
                    string genM = GetFirstColumnData("ID", dino, "GenM");
                    if (ToDouble(genM) > ToDouble(gen)) { gen = genM; }

                    if (ToDouble(gen) >= maxGen) { maxGen = ToDouble(gen); }
                }
            }

            foreach (var dino in females)
            {
                string group = GetGroup(dino);
                if (group != "Archived")
                {
                    string gen = GetFirstColumnData("ID", dino, "Gen");
                    string genM = GetFirstColumnData("ID", dino, "GenM");
                    if (ToDouble(genM) > ToDouble(gen)) { gen = genM; }
                    if (ToDouble(gen) >= maxGen) { maxGen = ToDouble(gen); }
                }
            }

            return maxGen;
        }

        public static string[] GetGenParents(string DinoClass, double gen)
        {
            // Retrieve female data
            string[] females = GetDistinctFilteredColumnData("Class", DinoClass, "Sex", "Female", "ID");

            // Retrieve male data
            string[] males = GetDistinctFilteredColumnData("Class", DinoClass, "Sex", "Male", "ID");

            // Use a HashSet to store distinct values
            HashSet<string> resultSet = new HashSet<string>();

            foreach (var dino in males)
            {
                string group = GetGroup(dino);
                if (group != "Archived")
                {
                    string papaID = GetFirstColumnData("ID", dino, "Papa");
                    string mamaID = GetFirstColumnData("ID", dino, "Mama");
                    double genD = ToDouble(GetFirstColumnData("ID", dino, "Gen"));
                    double genDM = ToDouble(GetFirstColumnData("ID", dino, "GenM"));
                    if (genDM > genD) { genD = genDM; }

                    if (genD == gen)
                    {
                        // Combine papaID and mamaID into a single string with a delimiter
                        string combined = $"{papaID},{mamaID}";

                        if (mamaID != "" && papaID != "")
                        {
                            // Add the combined value to the result set (distinct pairs)
                            resultSet.Add(combined);
                        }
                    }
                }
            }

            foreach (var dino in females)
            {
                string group = GetGroup(dino);
                if (group != "Archived")
                {
                    string papaID = GetFirstColumnData("ID", dino, "Papa");
                    string mamaID = GetFirstColumnData("ID", dino, "Mama");
                    double genD = ToDouble(GetFirstColumnData("ID", dino, "Gen"));
                    double genDM = ToDouble(GetFirstColumnData("ID", dino, "GenM"));
                    if (genDM > genD) { genD = genDM; }

                    if (genD == gen)
                    {
                        // Combine papaID and mamaID into a single string with a delimiter
                        string combined = $"{papaID},{mamaID}";

                        if (mamaID != "" && papaID != "")
                        {
                            // Add the combined value to the result set (distinct pairs)
                            resultSet.Add(combined);
                        }
                    }
                }
            }

            // Convert to an array of strings
            return resultSet.ToArray();
        }

        public static string[] GetKidsFromPair(string DinoClass, string pair)
        {
            var parts = pair.Split(',');

            // Check for missing or empty values
            string papaID = parts.Length > 0 && !string.IsNullOrWhiteSpace(parts[0]) ? parts[0] : "";
            string mamaID = parts.Length > 1 && !string.IsNullOrWhiteSpace(parts[1]) ? parts[1] : "";


            // Retrieve female data
            string[] females = GetDistinctFilteredColumnData("Class", DinoClass, "Sex", "Female", "ID");

            // Retrieve male data
            string[] males = GetDistinctFilteredColumnData("Class", DinoClass, "Sex", "Male", "ID");

            // Use a HashSet to store distinct values
            HashSet<string> resultSet = new HashSet<string>();

            // ignore empty and invalid results
            if (papaID != "" && mamaID != "")
            {
                // get each parents boys here
                foreach (var dino in males)
                {
                    string group = GetGroup(dino);
                    if (group != "Archived")
                    {
                        string dinoPapa = GetFirstColumnData("ID", dino, "Papa");
                        string dinoMama = GetFirstColumnData("ID", dino, "Mama");

                        if (papaID == dinoPapa && mamaID == dinoMama)
                        {
                            // add dino id to list
                            resultSet.Add(dino);
                        }
                    }
                }
                // get each parents girls here
                foreach (var dino in females)
                {
                    string group = GetGroup(dino);
                    if (group != "Archived")
                    {
                        string dinoPapa = GetFirstColumnData("ID", dino, "Papa");
                        string dinoMama = GetFirstColumnData("ID", dino, "Mama");

                        if (papaID == dinoPapa && mamaID == dinoMama)
                        {
                            // add dino id to list
                            resultSet.Add(dino);
                        }
                    }
                }
            }
            return resultSet.ToArray();
        }

        public static string GrowUpTime(string id)
        {
            string result = "N/A";
            foreach (DataRow row in ImportsTable.Rows)
            {
                if (row["ID"]?.ToString() == id)
                {
                    string timeValue = row["Time"].ToString();
                    string ageValue = row["BabyAge"].ToString();
                    // make sure both fields have data
                    if (timeValue != "" && ToDouble(ageValue) > 0)
                    {
                        if (ToDouble(ageValue) == 1)
                        {
                            // time when dino reached fullgrown
                            result = ConvertUtcToLocal(timeValue);
                            break;
                        }
                    }
                }
            }
            return result;
        }

        public static void EditBreedStats(string id, string level, string hp, string st, string ox, string fo, string we, string da, string notes, string speed, string craft)
        {
            if (id != "")
            {
                SetNotes(id, notes);
                int rowID = 0;
                foreach (DataRow row in ImportsTable.Rows)
                {
                    if (id == row["ID"].ToString()) // found dino to edit
                    {
                        ImportsTable.Rows[rowID].SetField("Level", level);
                        ImportsTable.Rows[rowID].SetField("Hp", hp);
                        ImportsTable.Rows[rowID].SetField("Stamina", st);
                        ImportsTable.Rows[rowID].SetField("Oxygen", ox);
                        ImportsTable.Rows[rowID].SetField("Food", fo);
                        ImportsTable.Rows[rowID].SetField("Weight", we);
                        ImportsTable.Rows[rowID].SetField("Damage", da);
                        ImportsTable.Rows[rowID].SetField("Speed", speed);
                        ImportsTable.Rows[rowID].SetField("CraftSkill", craft);

                        break;
                    }
                    rowID++;
                }
            }
        }

        public static int DinoCount(string dinoTag, int toggle = 0)
        {
            // count dinos and only include based on their status and what view is toggled 0 to count all
            int count = 0;
            try
            {
                // Retrieve female data
                string[] females = DataManager.GetDistinctFilteredColumnData("Class", dinoTag, "Sex", "Female", "ID");
                foreach (string dino in females)
                {
                    string group = GetGroup(dino);
                    if (toggle == 0) { count++; }
                    else if (toggle == 1) { if (group != "Exclude" && group != "Archived") { count++; } }
                    else if (toggle == 2) { if (group != "" && group != "Archived") { count++; } }
                    else if (toggle == 3) { if (group != "Exclude" && group != "") { count++; } }
                }
                // Retrieve male data
                string[] males = DataManager.GetDistinctFilteredColumnData("Class", dinoTag, "Sex", "Male", "ID");
                foreach (string dino in males)
                {
                    string group = GetGroup(dino);
                    if (toggle == 0) { count++; }
                    else if (toggle == 1) { if (group != "Exclude" && group != "Archived") { count++; } }
                    else if (toggle == 2) { if (group != "" && group != "Archived") { count++; } }
                    else if (toggle == 3) { if (group != "Exclude" && group != "") { count++; } }
                }
            }
            catch { }

            return count;
        }

        public static string GetMutes(string id)
        {
            foreach (DataRow row in DataManager.StatTable.Rows)
            {
                if (id == row["ID"].ToString()) // did we find our dino in dinoData file
                {
                    return row["Mutes"].ToString();
                }
            }
            return "";
        }

        public static string GetGroup(string id)
        {
            foreach (DataRow row in DataManager.StatTable.Rows)
            {
                if (id == row["ID"].ToString()) // did we find our dino in dinoData file
                {
                    return row["Status"].ToString();
                }
            }
            return "";
        }

        public static string GetNotes(string id)
        {
            foreach (DataRow row in DataManager.StatTable.Rows)
            {
                if (id == row["ID"].ToString()) // did we find our dino in dinoData file
                {
                    return row["Notes"].ToString();
                }
            }
            return "";
        }

        public static string GetRate(string shortClass)
        {
            foreach (DataRow row in DataManager.StatTable.Rows)
            {
                if (shortClass == row["ID"].ToString()) // did we find our dino in dinoData file
                {
                    return row["Rate"].ToString();
                }
            }
            return "";
        }

        public static void SetRate(string shortClass, string rate)
        {
            if (shortClass != "")
            {
                int rowid = 0; bool found = false;
                foreach (DataRow row in DataManager.StatTable.Rows)
                {
                    if (shortClass == row["ID"].ToString()) // did we find our dino in dinoData file
                    {
                        StatTable.Rows[rowid].SetField("Rate", rate);
                        FileManager.Log($"Set rate for id: {shortClass} to: {rate}", 0);
                        found = true; break;
                    }
                    rowid++;
                }
                if (!found)
                {
                    DataRow dr = DataManager.StatTable.NewRow();
                    dr["ID"] = shortClass;
                    dr["Status"] = "";
                    dr["Mutes"] = "";
                    dr["Notes"] = "";
                    dr["Rate"] = rate;
                    DataManager.StatTable.Rows.Add(dr);
                }
                // request a save after modifying data
                FileManager.needSave = true;
            }
        }

        public static void SetGroup(string id, string group)
        {
            if (id != "")
            {
                int rowid = 0; bool found = false;
                foreach (DataRow row in DataManager.StatTable.Rows)
                {
                    if (id == row["ID"].ToString()) // did we find our dino in dinoData file
                    {
                        StatTable.Rows[rowid].SetField("Status", group);
                        FileManager.Log($"Set group for id: {id} to: {group}", 0);
                        found = true; break;
                    }
                    rowid++;
                }
                if (!found)
                {
                    DataRow dr = DataManager.StatTable.NewRow();
                    dr["ID"] = id;
                    dr["Status"] = group;
                    dr["Mutes"] = "";
                    dr["Notes"] = "";
                    DataManager.StatTable.Rows.Add(dr);
                }
                // request a save after modifying data
                FileManager.needSave = true;
            }
        }

        public static void SetNotes(string id, string notes)
        {
            int rowid = 0; bool found = false;
            foreach (DataRow row in DataManager.StatTable.Rows)
            {
                if (id == row["ID"].ToString()) // did we find our dino in dinoData file
                {
                    StatTable.Rows[rowid].SetField("Notes", notes);
                    found = true; break;
                }
                rowid++;
            }
            if (!found)
            {
                DataRow dr = DataManager.StatTable.NewRow();
                dr["ID"] = id;
                dr["Notes"] = notes;
                dr["Status"] = "";
                dr["Mutes"] = "";
                DataManager.StatTable.Rows.Add(dr);
            }
            // request a save after modifying data
            FileManager.needSave = true;
        }

        public static void SetMutes(string id, string mutes)
        {
            int rowid = 0; bool found = false;
            foreach (DataRow row in DataManager.StatTable.Rows)
            {
                if (id == row["ID"].ToString()) // did we find our dino in dinoData file
                {
                    StatTable.Rows[rowid].SetField("Mutes", mutes);
                    found = true; break;
                }
                rowid++;
            }
            if (!found)
            {
                DataRow dr = DataManager.StatTable.NewRow();
                dr["ID"] = id;
                dr["Mutes"] = mutes;
                dr["Status"] = "";
                dr["Notes"] = "";
                DataManager.StatTable.Rows.Add(dr);
            }
        }

        private static void ProcessDinos(string[] dinos, List<string[]> FirstStats, List<string[]> LastStats, DataTable table, int toggle , bool baby = false)
        {
            int rowID = 0;
            foreach (var dino in dinos)
            {
                // recheck mutations incase any stats were edited
                MutationDetection(dino);

                string group = GetGroup(dino);
                string mutes = GetMutes(dino);



                bool addIT = false;
                if (toggle == 0)
                {
                    if (group != "Archived")
                    {
                        addIT = true;
                    }
                }
                else if (toggle == 1)
                {
                    if (group != "Archived" && group != "Exclude")
                    {
                        addIT = true;
                    }
                }
                else if (toggle == 2)
                {
                    if (group != "Archived" && group == "Exclude")
                    {
                        addIT = true;
                    }
                }
                else if (toggle == 3)
                {
                    if (group == "Archived")
                    {
                        addIT = true;
                    }
                }

                if (addIT)
                {
                    // at the same time we set the max stats
                    string name = LastStats[rowID][0].ToString();
                    double LevelM = ToDouble(FirstStats[rowID][1].ToString());

                    double HpM = Math.Round(ToDouble(FirstStats[rowID][2].ToString()), 1);
                    double StaminaM = Math.Round(ToDouble(FirstStats[rowID][3].ToString()), 1);
                    double O2M = Math.Round(ToDouble(FirstStats[rowID][4].ToString()), 1);
                    double FoodM = Math.Round(ToDouble(FirstStats[rowID][5].ToString()), 1);
                    double WeightM = Math.Round(ToDouble(FirstStats[rowID][6].ToString()), 1);
                    double DamageM = Math.Round((ToDouble(FirstStats[rowID][7].ToString()) + 1) * 100, 1);
                    double CraftM = Math.Round((ToDouble(FirstStats[rowID][20].ToString()) + 1) * 100, 1);

                    double SpeedM = Math.Round((ToDouble(FirstStats[rowID][8].ToString()) + 1) * 100);
                    string status = "";
                    double gen = ToDouble(FirstStats[rowID][13].ToString());
                    double genM = ToDouble(FirstStats[rowID][14].ToString());
                    if (genM > gen) { gen = genM; }
                    string mama = FirstStats[rowID][9].ToString();
                    string papa = FirstStats[rowID][10].ToString();
                    double mamaMute = ToDouble(FirstStats[rowID][11].ToString());
                    double papaMute = ToDouble(FirstStats[rowID][12].ToString());
                    double age = Math.Round(ToDouble(LastStats[rowID][15].ToString()) * 100);
                    double imprint = Math.Round(ToDouble(LastStats[rowID][17].ToString()) * 100);
                    string imprinter = FirstStats[rowID][18].ToString();

                    string id = dino;

                    // get first known age and time
                    string firstTime = DataManager.GetFirstColumnData("ID", dino, "Time");
                    double firstAge = DataManager.ToDouble(DataManager.GetFirstColumnData("ID", dino, "BabyAge")) * 100;

                    // get last known age and time
                    double lastAge = DataManager.ToDouble(DataManager.GetLastColumnData("ID", dino, "BabyAge")) * 100;
                    string lastTime = DataManager.GetLastColumnData("ID", dino, "Time");

                    // get the most recent parent names
                    string papaName = DataManager.GetLastColumnData("ID", papa, "Name");
                    string mamaName = DataManager.GetLastColumnData("ID", mama, "Name");


                    double estTimeLeft = 0;
                    double ageRate = 0;
                    DateTime dateT = DateTime.UtcNow;

                    string dinoClass = DataManager.GetLastColumnData("ID", id, "Class");
                    string shortClass = DataManager.LongClassToShort(dinoClass);

                    if (baby)
                    {
                        double knownAgeLeft = 100 - lastAge;

                        // convert units
                        DateTime firstTimeD = DateTime.ParseExact(firstTime, "dd/MM/yyyy HH:mm:ss", null);
                        DateTime lastTimeD = DateTime.ParseExact(lastTime, "dd/MM/yyyy HH:mm:ss", null);

                        // Calculate differences
                        double timeDiffMinutes = (lastTimeD - firstTimeD).TotalMinutes;
                        double ageDiff = lastAge - firstAge;

                        // check if manual aging rate is set for this species
                        string rate = DataManager.GetRate(shortClass);
                        double rateD = ToDouble(rate);

                       
                        if (rateD > 0)
                        {
                            ageRate = rateD / 60;
                        }
                        else
                        {
                            // Calculate rates age % per minute
                            ageRate = ageDiff / timeDiffMinutes;
                        }
                        // Estimate data
                        DateTime nowTime = DateTime.UtcNow;

                        double timePassed = (nowTime - lastTimeD).TotalMinutes;
                        double agePassed = timePassed * ageRate;

                        // Convert minutes to TimeSpan
                        TimeSpan timePassedD = TimeSpan.FromMinutes(timePassed);

                        double estimatedAge = lastAge + agePassed;
                        double estAgeLeft = 100 - estimatedAge;
                        estTimeLeft = estAgeLeft / ageRate;
                        double LastTimeLeft = knownAgeLeft / ageRate; // time left at last data

                        
                        if (ageRate > 0)
                        {
                            if (!double.IsNaN(LastTimeLeft))
                            {
                                TimeSpan timePa = TimeSpan.FromMinutes(LastTimeLeft);
                                dateT = lastTimeD + timePa;
                            }
                        }

                        age = estimatedAge;
                    }
                    
                    if (imprinter != "") // we haz imprinter = baby dino
                    {
                        if (lastAge < 100)
                        {
                            DateTime firstTimeD = DateTime.ParseExact(firstTime, "dd/MM/yyyy HH:mm:ss", null);
                            status = $"{Shared.Smap["Age"]}" + ConvertUtcToLocal(firstTimeD.ToString("dd/MM/yyyy HH:mm:ss"));
                        }
                        else
                        {
                            status = $"{Shared.Smap["Grown"]}" + GrowUpTime(dino);
                        }

                        // warn if dont know any of the parent id's
                        if ((mama == "" || mama == "N/A") && (papa == "" || papa == "N/A"))
                        {
                            mamaName = Shared.Smap["Warning"];
                            papaName = Shared.Smap["Warning"];
                        }
                        else // we has one of the parents id
                        {
                            if (mamaName == "") { mamaName = Shared.Smap["Missing"]; }
                            if (papaName == "") { papaName = Shared.Smap["Missing"]; }
                        }

                        if (mama == "00") { mamaName = Shared.Smap["Unknown"]; }
                        if (papa == "00") { papaName = Shared.Smap["Unknown"]; }
                    }
                    else // Fresh adult tame
                    {
                        DateTime firstTimeD = DateTime.ParseExact(firstTime, "dd/MM/yyyy HH:mm:ss", null);
                        status = $"{Shared.Smap["NewTame"]}" + ConvertUtcToLocal(firstTimeD.ToString("dd/MM/yyyy HH:mm:ss"));

                        mamaName = Shared.Smap["Unknown"];
                        papaName = Shared.Smap["Unknown"];

                        imprinter = DataManager.GetLastColumnData("ID", dino, "Tribe");
                    }


                    if (LevelM >= LevelMax) { LevelMax = LevelM; }
                    if (HpM >= HpMax) { HpMax = HpM; }
                    if (StaminaM >= StaminaMax) { StaminaMax = StaminaM; }
                    if (O2M >= O2Max) { O2Max = O2M; }
                    if (FoodM >= FoodMax) { FoodMax = FoodM; }
                    if (WeightM >= WeightMax) { WeightMax = WeightM; }
                    if (DamageM >= DamageMax) { DamageMax = DamageM; }
                    if (SpeedM >= SpeedMax) { SpeedMax = SpeedM; }
                    if (CraftM >= CraftMax) { CraftMax = CraftM; }

                    // Fill the DataRow
                    DataRow dr = table.NewRow();
                    dr["ID"] = dino;
                    dr["Name"] = name;
                    dr["Level"] = LevelM;
                    //------breeding stats-------
                    dr["Hp"] = HpM;
                    dr["Stamina"] = StaminaM;
                    dr["O2"] = O2M;
                    dr["Food"] = FoodM;
                    dr["Weight"] = WeightM;
                    dr["Damage"] = DamageM;
                    dr["Crafting"] = CraftM;
                    //---------------------------
                    dr["Speed"] = SpeedM;
                    dr["Gen"] = gen;
                    dr["Mama"] = mamaName;
                    dr["Papa"] = papaName;
                    dr["MamaMute"] = mamaMute;
                    dr["PapaMute"] = papaMute;
                    
                    dr["Age"] = age;

                    dr["Imprint"] = imprint;
                    dr["Imprinter"] = imprinter;

                    dr["Status"] = status;
                    dr["Tag"] = shortClass; // maybe use this at some point just make sure its not null for now

                    dr["Mutes"] = mutes;
                    dr["Group"] = group;

                    // baby tab stuff
                    dr["Time"] = estTimeLeft;
                    dr["Rate"] = ageRate;
                    dr["DateT"] = ConvertUtcToLocal(dateT.ToString("dd/MM/yyyy HH:mm:ss"));


                    table.Rows.Add(dr);
                }

                rowID++;
            }
        }

        public static void GetDinoData(string DinoClass, string sortiM = "", string sortiF = "", int toggle = 0, bool CurrentStats = false, bool baby = false)
        {
            // Clear the tables before populating them
            MaleTable.Clear();
            FemaleTable.Clear();

            string[] males = Array.Empty<string>();
            string[] females = Array.Empty<string>();

            if (baby)
            {
                // Retrieve female babies data
                females = GetDistinctFilteredColumnDataB("Sex", "Female", "ID");

                // Retrieve male babies data
                males = GetDistinctFilteredColumnDataB("Sex", "Male", "ID");
            }
            else
            {
                if (string.IsNullOrEmpty(DinoClass))
                {
                    return; // Exit early if tag is empty
                }

                // Retrieve female data
                females = GetDistinctFilteredColumnData("Class", DinoClass, "Sex", "Female", "ID");

                // Retrieve male data
                males = GetDistinctFilteredColumnData("Class", DinoClass, "Sex", "Male", "ID");
            }


            LevelMax = 0; HpMax = 0; StaminaMax = 0; O2Max = 0;
            FoodMax = 0; WeightMax = 0; DamageMax = 0; SpeedMax = 0;
            CraftMax = 0;

            if (CurrentStats)
            {
                // Process females
                List<string[]> FirstStatsF = GetLastStats(females);
                List<string[]> LastStatsF = GetLastStats(females);
                ProcessDinos(females, FirstStatsF, LastStatsF, FemaleTable, toggle, baby);

                // Process males
                List<string[]> FirstStatsM = GetLastStats(males);
                List<string[]> LastStatsM = GetLastStats(males);
                ProcessDinos(males, FirstStatsM, LastStatsM, MaleTable, toggle, baby);
            }
            else
            {
                // Process females
                List<string[]> FirstStatsF = GetFirstStats(females);
                List<string[]> LastStatsF = GetLastStats(females);
                ProcessDinos(females, FirstStatsF, LastStatsF, FemaleTable, toggle, baby);

                // Process males
                List<string[]> FirstStatsM = GetFirstStats(males);
                List<string[]> LastStatsM = GetLastStats(males);
                ProcessDinos(males, FirstStatsM, LastStatsM, MaleTable, toggle, baby);
            }


            // remove symbols to use correct name in sorting
            sortiM = ReplaceSymbols(sortiM, Shared.Smap);
            sortiF = ReplaceSymbols(sortiF, Shared.Smap);

            if (baby)
            {
                sortiM = sortiM.Replace("Class", "Tag");
                sortiF = sortiF.Replace("Class", "Tag");
            }

            sortiM = sortiM.Replace("pM", "PapaMute");
            sortiF = sortiF.Replace("pM", "PapaMute");
            sortiM = sortiM.Replace("mM", "MamaMute");
            sortiF = sortiF.Replace("mM", "MamaMute");

            sortiM = sortiM.Replace("Dmg", "Damage");
            sortiF = sortiF.Replace("Dmg", "Damage");


            // Sort the MaleTable based on the desired column
            DataView view1 = new DataView(MaleTable);
            view1.Sort = sortiM;
            MaleTable = view1.ToTable();


            // Sort the FemaleTable based on the desired column
            DataView view2 = new DataView(FemaleTable);
            view2.Sort = sortiF;
            FemaleTable = view2.ToTable();


            //  FileManager.Log("updated data");
        }

        public static string ReplaceSymbols(string input, Dictionary<string, string> symbolMap)
        {
            foreach (var symbol in symbolMap.Values)
            {
                if (!string.IsNullOrEmpty(symbol))
                {
                    input = input.Replace(symbol, string.Empty);
                }
            }
            return input;
        }

        public static void CompileDinoArchive(string sortC = "")
        {
            // Retrieve distinct IDs
            string[] idList = DataManager.GetAllDistinctColumnData("ID");

            ArchiveTable.Clear();

            // now check the status of each id and add them to ArchiveTable if status = Archived
            foreach (string dino in idList)
            {
                string status = GetGroup(dino);
                if (status == "Archived")
                {
                    // Translate the long class to a short readable class
                    string shortClass = DataManager.LongClassToShort(GetLastColumnData("ID", dino, "Class"));

                    DataRow dr = ArchiveTable.NewRow();
                    dr["ID"] = dino;
                    dr["Tag"] = GetLastColumnData("ID", dino, "Tag");
                    dr["Name"] = GetLastColumnData("ID", dino, "Name");
                    dr["Level"] = GetLastColumnData("ID", dino, "Level");
                    dr["Class"] = shortClass;

                    ArchiveTable.Rows.Add(dr);
                }
            }

            // remove symbols to use correct name in sorting
            sortC = ReplaceSymbols(sortC, Shared.Smap);


            sortC = sortC.Replace("Dmg", "Damage");


            // Sort the MaleTable based on the desired column
            DataView view1 = new DataView(DataManager.ArchiveTable);
            view1.Sort = sortC;
            DataManager.ArchiveTable = view1.ToTable();
        }

        public static void EvaluateDinos()
        {
            bool hasO2 = true; bool hasCraft = true;
            if (O2Max == 150) { hasO2 = false; }
            if (CraftMax == 100) { hasCraft = false; }

            int rowIDC = 0; // Male dinos
            foreach (DataRow rowC in MaleTable.Rows)
            {
                string compare = rowC["ID"].ToString();
                string compareStatus = rowC["Status"].ToString();

                string aC = "0"; string bC = "0"; string cC = "0";
                string dC = "0"; string eC = "0"; string fC = "0";
                string gC = "0";
                string binaryC = "0000000";

                double HpC = ToDouble(rowC["HP"].ToString());
                double StaminaC = ToDouble(rowC["Stamina"].ToString());
                double O2C = ToDouble(rowC["O2"].ToString());
                double FoodC = ToDouble(rowC["Food"].ToString());
                double WeightC = ToDouble(rowC["Weight"].ToString());
                double DamageC = ToDouble(rowC["Damage"].ToString());
                double CraftC = ToDouble(rowC["Crafting"].ToString());

                if (HpC + Shared.statOffset >= HpMax) { aC = "1"; }
                if (StaminaC + Shared.statOffset >= StaminaMax) { bC = "1"; }
                if (O2C + Shared.statOffset >= O2Max && hasO2) { cC = "1"; }
                if (FoodC + Shared.statOffset >= FoodMax) { dC = "1"; }
                if (WeightC + Shared.statOffset >= WeightMax) { eC = "1"; }
                if (DamageC + Shared.statOffset >= DamageMax) { fC = "1"; }
                if (CraftC + Shared.statOffset >= CraftMax && hasCraft) { gC = "1"; }

                binaryC = aC + bC + cC + dC + eC + fC + gC;

                string outStatus = "";

                // if not garbage
                // compare males against others to get the best male to breed
                if (binaryC.Contains("1"))
                {
                    foreach (DataRow rowW in MaleTable.Rows)
                    {
                        string with = rowW["ID"].ToString();

                        if (compare != with) // not with eachother
                        {
                            string withStatus = rowW["Status"].ToString();
                            string aW = "0"; string bW = "0"; string cW = "0";
                            string dW = "0"; string eW = "0"; string fW = "0";
                            string gW = "0";

                            double HpW = ToDouble(rowW["HP"].ToString());
                            double StaminaW = ToDouble(rowW["Stamina"].ToString());
                            double O2W = ToDouble(rowW["O2"].ToString());
                            double FoodW = ToDouble(rowW["Food"].ToString());
                            double WeightW = ToDouble(rowW["Weight"].ToString());
                            double DamageW = ToDouble(rowW["Damage"].ToString());
                            double CraftW = ToDouble(rowW["Crafting"].ToString());

                            if (HpW + Shared.statOffset >= DataManager.HpMax) { aW = "1"; }
                            if (StaminaW + Shared.statOffset >= DataManager.StaminaMax) { bW = "1"; }
                            if (O2W + Shared.statOffset >= DataManager.O2Max && hasO2) { cW = "1"; }
                            if (FoodW + Shared.statOffset >= DataManager.FoodMax) { dW = "1"; }
                            if (WeightW + Shared.statOffset >= DataManager.WeightMax) { eW = "1"; }
                            if (DamageW + Shared.statOffset >= DataManager.DamageMax) { fW = "1"; }
                            if (CraftW + Shared.statOffset >= DataManager.CraftMax && hasCraft) { gW = "1"; }

                            string binaryW = aW + bW + cW + dW + eW + fW + gW;

                            // now that we have both binary strings compare them to figure out if the compare is superceeded or not
                            string aA = "0"; string bA = "0"; string cA = "0";
                            string dA = "0"; string eA = "0"; string fA = "0";
                            string gA = "0";

                            // add up the binary shiz with magical ways known only to the gods of blubs
                            if (aC == "0" && aW == "0") { aA = "0"; } else if (aC == "0" && aW == "1") { aA = "1"; } else if (aC == "1" && aW == "0") { aA = "2"; } else if (aC == "1" && aW == "1") { aA = "3"; }
                            if (bC == "0" && bW == "0") { bA = "0"; } else if (bC == "0" && bW == "1") { bA = "1"; } else if (bC == "1" && bW == "0") { bA = "2"; } else if (bC == "1" && bW == "1") { bA = "3"; }
                            if (cC == "0" && cW == "0") { cA = "0"; } else if (cC == "0" && cW == "1") { cA = "1"; } else if (cC == "1" && cW == "0") { cA = "2"; } else if (cC == "1" && cW == "1") { cA = "3"; }
                            if (dC == "0" && dW == "0") { dA = "0"; } else if (dC == "0" && dW == "1") { dA = "1"; } else if (dC == "1" && dW == "0") { dA = "2"; } else if (dC == "1" && dW == "1") { dA = "3"; }
                            if (eC == "0" && eW == "0") { eA = "0"; } else if (eC == "0" && eW == "1") { eA = "1"; } else if (eC == "1" && eW == "0") { eA = "2"; } else if (eC == "1" && eW == "1") { eA = "3"; }
                            if (fC == "0" && fW == "0") { fA = "0"; } else if (fC == "0" && fW == "1") { fA = "1"; } else if (fC == "1" && fW == "0") { fA = "2"; } else if (fC == "1" && fW == "1") { fA = "3"; }
                            if (gC == "0" && gW == "0") { gA = "0"; } else if (gC == "0" && gW == "1") { gA = "1"; } else if (gC == "1" && gW == "0") { gA = "2"; } else if (gC == "1" && gW == "1") { gA = "3"; }

                            string binaryA = aA + bA + cA + dA + eA + fA + gA;

                            if (binaryC == binaryW && !withStatus.Contains("<") && !withStatus.Contains("#"))
                            {
                                // both have same stats   MARK IT
                                outStatus = "# " + rowW["Name"].ToString();  // identical   #with
                            }

                            if (binaryA.Contains("1") && binaryA.Contains("3") && !binaryA.Contains("2")) // has 1 and 3 but not 2
                            {
                                // both have same stats   MARK IT
                                outStatus = "< " + rowW["Name"].ToString();  // superceeded
                            }
                        }
                    }
                }





                // edit the row that we show
                if (outStatus.Contains("<") || outStatus.Contains("#")) { compareStatus = outStatus; }
                if (!binaryC.Contains("1")) { compareStatus = $"{compareStatus}{Shared.Smap["Garbage"]}"; }
                MaleTable.Rows[rowIDC].SetField("Status", compareStatus);
                MaleTable.Rows[rowIDC].SetField("Res", binaryC);
                rowIDC++;
            }
            //==================================================================================================
            rowIDC = 0; // Female dinos
            foreach (DataRow rowC in FemaleTable.Rows)
            {
                string compare = rowC["ID"].ToString();
                string compareStatus = rowC["Status"].ToString();

                string aC = "0"; string bC = "0"; string cC = "0";
                string dC = "0"; string eC = "0"; string fC = "0";
                string gC = "0";
                string binaryC = "0000000";

                double HpC = ToDouble(rowC["HP"].ToString());
                double StaminaC = ToDouble(rowC["Stamina"].ToString());
                double O2C = ToDouble(rowC["O2"].ToString());
                double FoodC = ToDouble(rowC["Food"].ToString());
                double WeightC = ToDouble(rowC["Weight"].ToString());
                double DamageC = ToDouble(rowC["Damage"].ToString());
                double CraftC = ToDouble(rowC["Crafting"].ToString());

                if (HpC + Shared.statOffset >= HpMax) { aC = "1"; }
                if (StaminaC + Shared.statOffset >= StaminaMax) { bC = "1"; }
                if (O2C + Shared.statOffset >= O2Max && hasO2) { cC = "1"; }
                if (FoodC + Shared.statOffset >= FoodMax) { dC = "1"; }
                if (WeightC + Shared.statOffset >= WeightMax) { eC = "1"; }
                if (DamageC + Shared.statOffset >= DamageMax) { fC = "1"; }
                if (CraftC + Shared.statOffset >= CraftMax && hasCraft) { gC = "1"; }

                binaryC = aC + bC + cC + dC + eC + fC + gC;


                // edit the row we show
                if (!binaryC.Contains("1")) { compareStatus = $"{compareStatus}{Shared.Smap["Garbage"]}"; }
                FemaleTable.Rows[rowIDC].SetField("Status", compareStatus);
                FemaleTable.Rows[rowIDC].SetField("Res", binaryC);
                rowIDC++;
            }
        }

        public static void GetBestPartner()
        {
            int check = 7; int maxGP = 5; // max stat points we can have = hp,st,o2,fo,we,da,cr
            if (DataManager.O2Max != 150) { maxGP++; } // have o2
            if (DataManager.CraftMax != 100) { maxGP++; } // have craft

            BottomTable.Clear();
            // ==================================================================================================

            int nr = 1;
            int p0 = check;
            while (p0 >= 0)
            {
                int p1 = check;
                while (p1 >= 0)
                {
                    int p2 = check;
                    while (p2 >= 0)
                    {
                        int roMID = 0;
                        foreach (DataRow rowM in DataManager.MaleTable.Rows)
                        {
                            string IDM1 = rowM["Res"].ToString();
                            string papaID = rowM["ID"].ToString();
                            string statusM = rowM["Status"].ToString();


                            if (!statusM.Contains("<") && !statusM.Contains("#")) // dont include theese males
                            {
                                int roFID = 0;
                                foreach (DataRow rowF in DataManager.FemaleTable.Rows)
                                {
                                    string aB = "0"; string bB = "0"; string cB = "0";
                                    string dB = "0"; string eB = "0"; string fB = "0";
                                    string gB = "0";

                                    string IDF1 = rowF["Res"].ToString();
                                    string mamaID = rowF["ID"].ToString();
                                    string statusF = rowF["Status"].ToString();


                                    string aM = IDM1.Substring(0, 1); string bM = IDM1.Substring(1, 1); string cM = IDM1.Substring(2, 1);
                                    string dM = IDM1.Substring(3, 1); string eM = IDM1.Substring(4, 1); string fM = IDM1.Substring(5, 1);
                                    string gM = IDM1.Substring(6, 1);

                                    string aF = IDF1.Substring(0, 1); string bF = IDF1.Substring(1, 1); string cF = IDF1.Substring(2, 1);
                                    string dF = IDF1.Substring(3, 1); string eF = IDF1.Substring(4, 1); string fF = IDF1.Substring(5, 1);
                                    string gF = IDF1.Substring(6, 1);

                                    int gPoints = 0;
                                    int aPoints = 0;
                                    int nPoints = 0;

                                    string binC = "0000000";
                                    if (aM == "1" && aF == "1") { gPoints++; aB = "2"; } else if (aM == "1" || aF == "1") { aPoints++; aB = "1"; } else { aB = "0"; nPoints++; }
                                    if (bM == "1" && bF == "1") { gPoints++; bB = "2"; } else if (bM == "1" || bF == "1") { aPoints++; bB = "1"; } else { bB = "0"; nPoints++; }
                                    if (cM == "1" && cF == "1") { gPoints++; cB = "2"; } else if (cM == "1" || cF == "1") { aPoints++; cB = "1"; } else { cB = "0"; nPoints++; }
                                    if (dM == "1" && dF == "1") { gPoints++; dB = "2"; } else if (dM == "1" || dF == "1") { aPoints++; dB = "1"; } else { dB = "0"; nPoints++; }
                                    if (eM == "1" && eF == "1") { gPoints++; eB = "2"; } else if (eM == "1" || eF == "1") { aPoints++; eB = "1"; } else { eB = "0"; nPoints++; }
                                    if (fM == "1" && fF == "1") { gPoints++; fB = "2"; } else if (fM == "1" || fF == "1") { aPoints++; fB = "1"; } else { fB = "0"; nPoints++; }
                                    if (gM == "1" && gF == "1") { gPoints++; gB = "2"; } else if (gM == "1" || gF == "1") { aPoints++; gB = "1"; } else { gB = "0"; nPoints++; }

                                    binC = aB + bB + cB + dB + eB + fB + gB;
                                    int agPoints = gPoints + aPoints;

                                    if (p0 == agPoints)
                                    {
                                        if (p1 == gPoints)
                                        {
                                            if (p2 == aPoints)
                                            {
                                                if (aPoints > 0 || agPoints >= maxGP)
                                                {
                                                    MakeOffspring(papaID, mamaID, "Breed #" + nr++, binC, maxGP, aPoints, gPoints);
                                                }
                                            }
                                        }
                                    }
                                    roFID++;
                                }
                            }
                            roMID++;
                        }
                        p2--;
                    }
                    p1--;
                }
                p0--;
            }

            // FileManager.Log("Updated BreedPairs",0);
        }

        public static void MakeOffspring(string male, string female, string offspring, string res, int maxGP, int aPoints, int gPoints)
        {

            if (male != "" && female != "")
            {
                double Hp = 0; double Stamina = 0; double O2 = 0;
                double Food = 0; double Weight = 0; double Damage = 0;
                double Craft = 0;
                double Gen = 0; double LevelB = 0;
                string mama = ""; string papa = "";
                int MrowID = 0;

                foreach (DataRow rowM in DataManager.MaleTable.Rows)
                {
                    string idM = rowM["ID"].ToString();
                    string namM = rowM["Name"].ToString();
                    if (idM == male)
                    {
                        papa = namM;
                        double HpM = ToDouble(rowM["HP"].ToString());
                        double StaminaM = ToDouble(rowM["Stamina"].ToString());
                        double O2M = ToDouble(rowM["O2"].ToString());
                        double FoodM = ToDouble(rowM["Food"].ToString());
                        double WeightM = ToDouble(rowM["Weight"].ToString());
                        double DamageM = ToDouble(rowM["Damage"].ToString());
                        double CraftM = ToDouble(rowM["Crafting"].ToString());
                        double GenM = ToDouble(rowM["Gen"].ToString());
                        double levelM = ToDouble(rowM["Level"].ToString());
                        int FrowID = 0;
                        foreach (DataRow rowF in DataManager.FemaleTable.Rows)
                        {
                            string nameF = rowF["ID"].ToString();
                            string namF = rowF["Name"].ToString();
                            if (nameF == female)
                            {
                                mama = namF;
                                double HpF = ToDouble(rowF["HP"].ToString());
                                double StaminaF = ToDouble(rowF["Stamina"].ToString());
                                double O2F = ToDouble(rowF["O2"].ToString());
                                double FoodF = ToDouble(rowF["Food"].ToString());
                                double WeightF = ToDouble(rowF["Weight"].ToString());
                                double DamageF = ToDouble(rowF["Damage"].ToString());
                                double CraftF = ToDouble(rowF["Crafting"].ToString());
                                double GenF = ToDouble(rowF["Gen"].ToString());
                                double levelF = ToDouble(rowF["Level"].ToString());

                                if (GenM >= GenF)
                                {
                                    Gen = GenM + 1; // add up generations
                                }
                                else
                                {
                                    Gen = GenF + 1; // add up generations
                                }

                                if (HpM > HpF) { Hp = HpM; } else { Hp = HpF; }
                                if (StaminaM > StaminaF) { Stamina = StaminaM; } else { Stamina = StaminaF; }
                                if (O2M > O2F) { O2 = O2M; } else { O2 = O2F; }
                                if (FoodM > FoodF) { Food = FoodM; } else { Food = FoodF; }
                                if (WeightM > WeightF) { Weight = WeightM; } else { Weight = WeightF; }
                                if (DamageM > DamageF) { Damage = DamageM; } else { Damage = DamageF; }
                                if (CraftM > CraftF) { Craft = CraftM; } else { Craft = CraftF; }

                                // average out the level for now
                                if ((levelM + levelF) > 0) { LevelB = Math.Round((levelM + levelF) / 2); }

                            }
                            FrowID++;
                        }
                    }
                    MrowID++;
                }


                DataRow dr = DataManager.BottomTable.NewRow();
                dr["Name"] = offspring;
                dr["Level"] = LevelB;
                dr["Hp"] = Hp;
                dr["Stamina"] = Stamina;
                dr["O2"] = O2;
                dr["Food"] = Food;
                dr["Weight"] = Weight;
                dr["Damage"] = Damage;
                dr["Crafting"] = Craft;
                dr["Gen"] = Gen;
                dr["Mama"] = mama;
                dr["Papa"] = papa;

                double agP = aPoints + gPoints;

                dr["Status"] = $"{gPoints} + {aPoints} = {agP}";

                dr["Res"] = res;


                DataManager.BottomTable.Rows.Add(dr);
            }
        }

        public static void DeleteRowsByID(string id)
        {
            // Select rows matching the ID.
            var rowsToDelete = ImportsTable.Select($"ID = '{id}'");

            if (rowsToDelete.Length == 0)
            {
                FileManager.Log($"No rows found for ID: {id}", 1);
                return;
            }

            // Track how many rows are deleted.
            int deletedCount = 0;

            foreach (var row in rowsToDelete)
            {
                //Console.WriteLine($"Deleting row: ID={row["ID"]}, Name={row["Name"]}");
                row.Delete();
                deletedCount++;
            }

            // Commit the deletions to the DataTable.
            ImportsTable.AcceptChanges();

            if (deletedCount > 0)
            {
                //Console.WriteLine($"Deleted {deletedCount} rows for ID: {id}");

                DataManager.SetGroup(id, "");
                // maybe delete the file too to prevent reimport

                // delete associated ini file to prevent reimport
                // otherwise we have to mark it for exclusion during import
                // maybe as a failsafe?!?!
                FileManager.DeleteFile(id);


                FileManager.Log($"Purged ID: {id}", 0);
                // request a save after modifying data
                FileManager.needSave = true;
            }
        }

        public static void PurgeAll()
        {
            string[] idList = DataManager.GetAllDistinctColumnData("ID");

            foreach (string id in idList)
            {
                string group = GetGroup(id);
                if (group == "Archived")
                {
                    DeleteRowsByID(id);
                }
            }
            FileManager.needSave = true;
        }

        private static void MutationDetection(string id)
        {
            string a = "0"; string b = "0"; string c = "0";
            string d = "0"; string e = "0"; string f = "0";
            string g = "0";

            // better than mutation detection is detecting incorrect stats
            // wich its doing without checking for mutation increase

            string mamaID = GetFirstColumnData("ID", id, "Mama");
            string papaID = GetFirstColumnData("ID", id, "Papa");

            if (mamaID != "" && papaID != "")
            {
                double dinoHP = Math.Round(ToDouble(GetFirstColumnData("ID", id, "HP")));
                double mamaHP = Math.Round(ToDouble(GetFirstColumnData("ID", mamaID, "HP")));
                double papaHP = Math.Round(ToDouble(GetFirstColumnData("ID", papaID, "HP")));

                if ((mamaHP != 0 && papaHP != 0) && (dinoHP != papaHP && dinoHP != mamaHP))
                {
                    if (dinoHP > (papaHP + Shared.muteOffset) || dinoHP < (papaHP - Shared.muteOffset))
                    {
                        if (dinoHP > (mamaHP + Shared.muteOffset) || dinoHP < (mamaHP - Shared.muteOffset))
                        {
                            a = "1";
                        }
                    }
                }

                double dinoStamina = Math.Round(ToDouble(GetFirstColumnData("ID", id, "Stamina")));
                double mamaStamina = Math.Round(ToDouble(GetFirstColumnData("ID", mamaID, "Stamina")));
                double papaStamina = Math.Round(ToDouble(GetFirstColumnData("ID", papaID, "Stamina")));

                if ((mamaStamina != 0 && papaStamina != 0) && (dinoStamina != papaStamina && dinoStamina != mamaStamina))
                {
                    if (dinoStamina > (papaStamina + Shared.muteOffset) || dinoStamina < (papaStamina - Shared.muteOffset))
                    {
                        if (dinoStamina > (mamaStamina + Shared.muteOffset) || dinoStamina < (mamaStamina - Shared.muteOffset))
                        {
                            b = "1";
                        }
                    }
                }

                double dinoOxygen = Math.Round(ToDouble(GetFirstColumnData("ID", id, "Oxygen")));
                double mamaOxygen = Math.Round(ToDouble(GetFirstColumnData("ID", mamaID, "Oxygen")));
                double papaOxygen = Math.Round(ToDouble(GetFirstColumnData("ID", papaID, "Oxygen")));

                if ((mamaOxygen != 0 && papaOxygen != 0) && (dinoOxygen != papaOxygen && dinoOxygen != mamaOxygen))
                {
                    if (dinoOxygen > (papaOxygen + Shared.muteOffset) || dinoOxygen < (papaOxygen - Shared.muteOffset))
                    {
                        if (dinoOxygen > (mamaOxygen + Shared.muteOffset) || dinoOxygen < (mamaOxygen - Shared.muteOffset))
                        {
                            c = "1";
                        }
                    }
                }

                double dinoFood = Math.Round(ToDouble(GetFirstColumnData("ID", id, "Food")));
                double mamaFood = Math.Round(ToDouble(GetFirstColumnData("ID", mamaID, "Food")));
                double papaFood = Math.Round(ToDouble(GetFirstColumnData("ID", papaID, "Food")));

                if ((mamaFood != 0 && papaFood != 0) && (dinoFood != papaFood && dinoFood != mamaFood))
                {
                    if (dinoFood > (papaFood + Shared.muteOffset) || dinoFood < (papaFood - Shared.muteOffset))
                    {
                        if (dinoFood > (mamaFood + Shared.muteOffset) || dinoFood < (mamaFood - Shared.muteOffset))
                        {
                            d = "1";
                        }
                    }
                }

                double dinoWeight = Math.Round(ToDouble(GetFirstColumnData("ID", id, "Weight")));
                double mamaWeight = Math.Round(ToDouble(GetFirstColumnData("ID", mamaID, "Weight")));
                double papaWeight = Math.Round(ToDouble(GetFirstColumnData("ID", papaID, "Weight")));

                if ((mamaWeight != 0 && papaWeight != 0) && (dinoWeight != papaWeight && dinoWeight != mamaWeight))
                {
                    if (dinoWeight > (papaWeight + Shared.muteOffset) || dinoWeight < (papaWeight - Shared.muteOffset))
                    {
                        if (dinoWeight > (mamaWeight + Shared.muteOffset) || dinoWeight < (mamaWeight - Shared.muteOffset))
                        {
                            e = "1";
                        }
                    }
                }

                double dinoDamage = Math.Round(ToDouble(GetFirstColumnData("ID", id, "Damage")), 2);
                double mamaDamage = Math.Round(ToDouble(GetFirstColumnData("ID", mamaID, "Damage")), 2);
                double papaDamage = Math.Round(ToDouble(GetFirstColumnData("ID", papaID, "Damage")), 2);

                if ((mamaDamage != 0 && papaDamage != 0) && (dinoDamage != papaDamage && dinoDamage != mamaDamage))
                {
                    if (dinoDamage > (papaDamage + Shared.muteOffset) || dinoDamage < (papaDamage - Shared.muteOffset))
                    {
                        if (dinoDamage > (mamaDamage + Shared.muteOffset) || dinoDamage < (mamaDamage - Shared.muteOffset))
                        {
                            f = "1";
                        }
                    }
                }

                double dinoCraft = Math.Round(ToDouble(GetFirstColumnData("ID", id, "CraftSkill")), 2);
                double mamaCraft = Math.Round(ToDouble(GetFirstColumnData("ID", mamaID, "CraftSkill")), 2);
                double papaCraft = Math.Round(ToDouble(GetFirstColumnData("ID", papaID, "CraftSkill")), 2);

                if ((mamaCraft != 0 && papaCraft != 0) && (dinoCraft != papaCraft && dinoCraft != mamaCraft))
                {
                    if (dinoCraft > (papaCraft + Shared.muteOffset) || dinoCraft < (papaCraft - Shared.muteOffset))
                    {
                        if (dinoCraft > (mamaCraft + Shared.muteOffset) || dinoCraft < (mamaCraft - Shared.muteOffset))
                        {
                            g = "1";
                        }
                    }
                }
            }

            string mutes = a + b + c + d + e + f + g;
            SetMutes(id, mutes);
        }

        public static void Import()
        {
            // get a string array of exported dino files
            string[] exports = FileManager.GetExportFiles();

            AddC = 0; ModC = 0;
            foreach (string file in exports) // loop trough each file to look for data in all of them
            {
                var split1 = file.Split(new[] { @"DinoExport_" }, StringSplitOptions.RemoveEmptyEntries);
                var split2 = split1[1].Split(new[] { @".ini" }, StringSplitOptions.RemoveEmptyEntries);
                string id = split2[0].ToString();

                string[] importedNew = FilterDinoStats(file);
                // string Time = File.GetLastWriteTime(file).ToString("dd/MM/yyyy HH:mm:ss");
                string Time = File.GetLastWriteTimeUtc(file).ToString("dd/MM/yyyy HH:mm:ss");

                if (importedNew.Count() > 0)
                {

                    bool found = false;
                    foreach (DataRow row in ImportsTable.Rows)
                    {
                        if (row["ID"].ToString() == id)
                        {
                            found = true; break;
                        }
                    }
                    // never added dino before add first entry
                    DataRow dr = ImportsTable.NewRow();
                    dr["ID"] = id;
                    dr["Tag"] = importedNew[0];
                    dr["Sex"] = importedNew[1];
                    dr["Name"] = importedNew[2];
                    dr["Level"] = importedNew[3];
                    dr["Hp"] = importedNew[4];
                    dr["Stamina"] = importedNew[5];
                    dr["Oxygen"] = importedNew[6];
                    dr["Food"] = importedNew[7];
                    dr["Weight"] = importedNew[8];
                    dr["Damage"] = importedNew[9];
                    dr["Speed"] = importedNew[10];
                    dr["Mama"] = importedNew[11];
                    dr["Papa"] = importedNew[12];
                    dr["MamaMute"] = importedNew[13];
                    dr["PapaMute"] = importedNew[14];
                    dr["Gen"] = importedNew[15];
                    dr["GenM"] = importedNew[16];
                    dr["BabyAge"] = importedNew[17];
                    dr["Neutered"] = importedNew[18];
                    dr["Imprint"] = importedNew[19];
                    dr["Imprinter"] = importedNew[20];
                    dr["Tribe"] = importedNew[21];
                    dr["Class"] = importedNew[22];
                    dr["Time"] = Time;
                    dr["Colors"] = importedNew[23];
                    dr["CraftSkill"] = importedNew[24];

                    if (!found)
                    {
                        // add it to mutation list
                        MutationDetection(id);
                        AddC++;
                        //fileManager.log("Added dino: " + id);
                        ImportsTable.Rows.Add(dr);
                    }
                    else // we got a hit on the id
                    {
                        bool change = false; string lastTime = "";
                        lastTime = DataManager.GetLastColumnData("ID", id, "Time", "#");
                        DateTime LastTime = DateTime.ParseExact(lastTime, "dd/MM/yyyy HH:mm:ss", Culture);
                        DateTime NowTime = DateTime.ParseExact(Time, "dd/MM/yyyy HH:mm:ss", Culture);

                        if (NowTime > LastTime) { change = true; }

                        if (change)
                        {
                            bool updated = false;
                            string newMama = importedNew[11];
                            string newPapa = importedNew[12];

                            // check if new data is valid
                            if (newMama != "" && newMama != "N/A" && newMama != "0" && newPapa != "" && newPapa != "N/A" && newPapa != "0")
                            {
                                // replace old data assuming new data is bettah
                                int rowID = 0;
                                foreach (DataRow row in ImportsTable.Rows)
                                {
                                    if (id == row["ID"].ToString())
                                    {
                                        // update all info to correct any missing or invalid data
                                        ImportsTable.Rows[rowID].SetField("Mama", newMama);
                                        ImportsTable.Rows[rowID].SetField("Papa", newPapa);
                                        ImportsTable.Rows[rowID].SetField("MamaMute", importedNew[13]);
                                        ImportsTable.Rows[rowID].SetField("PapaMute", importedNew[14]);
                                        ImportsTable.Rows[rowID].SetField("Gen", importedNew[15]);
                                        ImportsTable.Rows[rowID].SetField("GenM", importedNew[16]);
                                    }
                                    rowID++;
                                }
                            }

                            ModC++;

                            ImportsTable.Rows.Add(dr);
                        }
                    }
                }
            }
            if (AddC > 0)
            {
                FileManager.Log("Added " + AddC + " dinos", 0);
            }
            if (ModC > 0)
            {
                FileManager.Log("Updated " + ModC + " dinos", 0);
            }
        }

        private static string[] FilterDinoStats(string filename, string emptyString = "N/A")
        {
            string[] resultSet = Enumerable.Repeat(emptyString, 25).ToArray();

            try
            {
                var iniData = IniParser.ParseIniFile(filename);

                foreach (var section in iniData)
                {
                    if (section.Key.ToUpper() == "DINOANCESTORS") // look for parents here
                    {
                        try // catch parents separately to leave empty just incase it fails
                        {
                            foreach (var key in section.Value)
                            {
                                var split = key.Value.ToString().Split(new[] { @";" }, StringSplitOptions.RemoveEmptyEntries);

                                var id1P = split[1].Split(new[] { @"=" }, StringSplitOptions.RemoveEmptyEntries);
                                var id2P = split[2].Split(new[] { @"=" }, StringSplitOptions.RemoveEmptyEntries);

                                resultSet[12] = id1P[1] + id2P[1];

                                var id1M = split[4].Split(new[] { @"=" }, StringSplitOptions.RemoveEmptyEntries);
                                var id2M = split[5].Split(new[] { @"=" }, StringSplitOptions.RemoveEmptyEntries);

                                resultSet[11] = id1M[1] + id2M[1];
                            }
                        }
                        catch
                        {
                            resultSet[11] = emptyString;
                            resultSet[12] = emptyString;
                        }
                    }
                    else if (section.Key.ToUpper() == "DINOANCESTORSMALE")
                    {
                        if (resultSet[11] == emptyString)
                        {
                            try // catch parents separately to leave empty just incase it fails
                            {
                                foreach (var key in section.Value)
                                {
                                    var split = key.Value.ToString().Split(new[] { @";" }, StringSplitOptions.RemoveEmptyEntries);

                                    var id1P = split[1].Split(new[] { @"=" }, StringSplitOptions.RemoveEmptyEntries);
                                    var id2P = split[2].Split(new[] { @"=" }, StringSplitOptions.RemoveEmptyEntries);

                                    resultSet[12] = id1P[1] + id2P[1];

                                    var id1M = split[4].Split(new[] { @"=" }, StringSplitOptions.RemoveEmptyEntries);
                                    var id2M = split[5].Split(new[] { @"=" }, StringSplitOptions.RemoveEmptyEntries);

                                    resultSet[11] = id1M[1] + id2M[1];
                                }
                            }
                            catch
                            {
                                resultSet[11] = emptyString;
                                resultSet[12] = emptyString;
                            }
                        }
                    }
                    else if (section.Key.ToUpper() == "COLORIZATION")
                    {
                        try // catch parents separately to leave empty just incase it fails
                        {
                            string[] outst = new string[section.Value.Count];
                            int rid = 0;
                            foreach (var key in section.Value) { outst[rid] = key.Value; rid++; }
                            rid = 0; string output = "";
                            foreach (string t in outst) { output += t + ";"; rid++; }
                            resultSet[23] = output;
                        }
                        catch
                        {
                            resultSet[23] = emptyString;
                        }
                    }
                    else
                    {
                        foreach (var key in section.Value)
                        {
                            if (key.Key.ToUpper() == "DINONAMETAG")
                            {
                                resultSet[0] = key.Value;
                            }
                            else if (key.Key.ToUpper() == "BISFEMALE")
                            {
                                if (key.Value.ToUpper() == "TRUE")
                                {
                                    resultSet[1] = "Female";
                                }
                                else
                                {
                                    resultSet[1] = "Male";
                                }
                            }
                            else if (key.Key.ToUpper() == "TAMEDNAME")
                            {
                                resultSet[2] = key.Value;
                            }
                            else if (key.Key.ToUpper() == "CHARACTERLEVEL")
                            {
                                resultSet[3] = key.Value;
                            }
                            else if (key.Key.ToUpper() == "HEALTH")
                            {
                                resultSet[4] = key.Value;
                            }
                            else if (key.Key.ToUpper() == "STAMINA")
                            {
                                resultSet[5] = key.Value;
                            }
                            else if (key.Key.ToUpper() == "OXYGEN")
                            {
                                resultSet[6] = key.Value;
                            }
                            else if (key.Key.ToUpper() == "FOOD")
                            {
                                resultSet[7] = key.Value;
                            }
                            else if (key.Key.ToUpper() == "WEIGHT")
                            {
                                resultSet[8] = key.Value;
                            }
                            else if (key.Key.ToUpper() == "MELEE DAMAGE")
                            {
                                resultSet[9] = key.Value;
                            }
                            else if (key.Key.ToUpper() == "MOVEMENT SPEED")
                            {
                                resultSet[10] = key.Value;
                            }
                            else if (key.Key.ToUpper() == "RANDOMMUTATIONSFEMALE")
                            {
                                resultSet[13] = key.Value;
                            }
                            else if (key.Key.ToUpper() == "RANDOMMUTATIONSMALE")
                            {
                                resultSet[14] = key.Value;
                            }
                            else if (key.Key.ToUpper() == "DINOANCESTORSCOUNT")
                            {
                                resultSet[15] = key.Value;
                            }
                            else if (key.Key.ToUpper() == "DINOANCESTORSMALE")
                            {
                                resultSet[16] = key.Value;
                            }
                            else if (key.Key.ToUpper() == "BABYAGE")
                            {
                                resultSet[17] = key.Value;
                            }
                            else if (key.Key.ToUpper() == "BNEUTERED")
                            {
                                if (key.Value.ToUpper() == "TRUE")
                                {
                                    resultSet[18] = "Y";
                                }
                                else
                                {
                                    resultSet[18] = "N";
                                }
                            }
                            else if (key.Key.ToUpper() == "DINOIMPRINTINGQUALITY")
                            {
                                resultSet[19] = key.Value;
                            }
                            else if (key.Key.ToUpper() == "IMPRINTERNAME")
                            {
                                resultSet[20] = key.Value;
                            }
                            else if (key.Key.ToUpper() == "TAMERSTRING")
                            {
                                resultSet[21] = key.Value;
                            }
                            else if (key.Key.ToUpper() == "DINOCLASS")
                            {
                                resultSet[22] = key.Value;
                            }
                            else if (key.Key.ToUpper() == "CRAFTING SKILL")
                            {
                                resultSet[24] = key.Value;
                            }
                        }

                    }

                }
            }
            catch
            {
                resultSet = new string[0];
                FileManager.Log("Parse Error!!", 2);
            }
            return resultSet;
        }

        public static double ToDouble(string input, double def = 0)
        {
            if (input == "")
            {
                return def;
            }
            else
            {
                try
                {
                    input = input.Replace(",", DecimalSeparator);
                    input = input.Replace(".", DecimalSeparator);

                    // replace missing field values with 0
                    input = input.Replace("N/A", "0");
                    input = input.Replace("#", "0");


                    return Convert.ToDouble(input);
                }
                catch
                {
                    FileManager.Log($"Failed to convert: {input} - Returned: 0", 1);
                    return def;
                }
            }
        }

        public static string ConvertUtcToLocal(string utcTimestamp)
        {
            // Parse the UTC timestamp into a DateTime object
            DateTime utcTime = DateTime.ParseExact(utcTimestamp, "dd/MM/yyyy HH:mm:ss", Culture, DateTimeStyles.AssumeUniversal);

            // Convert to local time
            DateTime localTime = utcTime.ToLocalTime();

            // Format back to your desired format
            return localTime.ToString("dd/MM/yyyy HH:mm:ss");
        }

    }
}