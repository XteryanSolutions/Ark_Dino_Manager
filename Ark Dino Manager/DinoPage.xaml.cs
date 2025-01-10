﻿using System.Data;
using System.Diagnostics;
using static Ark_Dino_Manager.Shared;
using static Ark_Dino_Manager.DataManager;

namespace Ark_Dino_Manager;

public partial class DinoPage : ContentPage
{
    ////////////////////    View Toggles    ////////////////////
    public static int ToggleExcluded = Shared.DefaultToggle;
    public static bool CurrentStats = Shared.DefaultStat;

    ////////////////////    Selecting       ////////////////////
    public static string selectedID = "";
    public static bool isSelected = false;
    public static bool canDouble = false;
    public static bool isDouble = false;
    public static bool showTree = false;


    // keep track of boxviews for recoloring
    private Dictionary<int, BoxView> boxViews = new Dictionary<int, BoxView>();
    private int boxID = 0;
    private int boxRowID = 0;

    Button ExcludeBtn = new Button { };
    Button ArchiveBtn = new Button { };

    ////////////////////    Table Sorting   ////////////////////
    public static string sortM = Shared.DefaultSortM;
    public static string sortF = Shared.DefaultSortF;


    private string levelText = "";
    private string hpText = "";
    private string staminaText = "";
    private string O2Text = "";
    private string foodText = "";
    private string weightText = "";
    private string damageText = "";
    private string notesText = "";
    private string speedText = "";
    private string craftText = "";

    ////////////////////    Data   ////////////////////
    private bool editStats = false;
    private bool dataValid = false;
    private int dinoCount = 0;

    private Stopwatch timer1 = Stopwatch.StartNew();


    public DinoPage()
    {
        InitializeComponent();

        dataValid = false;
        CreateContent();
    }

    private void FromHere()
    {
        timer1 = Stopwatch.StartNew();
    }

    private void ToHere(string text)
    {
        timer1.Stop();
        var elapsedMilliseconds = timer1.Elapsed.TotalMilliseconds;
        Shared.loadCount++; double outAVG = 0;
        if (Shared.loadCount < 2) { Shared.loadAvg = elapsedMilliseconds; outAVG = Shared.loadAvg; }
        else { Shared.loadAvg += elapsedMilliseconds; outAVG = Shared.loadAvg / Shared.loadCount; }
        FileManager.Log($"{text}: {elapsedMilliseconds}ms Avg: {outAVG}", 0);
    }


    public void CreateContent()
    {
        FromHere(); // Start benchmark timer here
        FileManager.Log("Updating GUI -> " + Shared.setPage, 0);
        if (!isSelected) { this.Title = $"{Shared.setPage.Replace("_", " ")}"; }

        if (Monitor.TryEnter(Shared._dbLock, TimeSpan.FromSeconds(5)))
        {
            try
            {
                if (!string.IsNullOrEmpty(Shared.selectedClass))
                {
                    if (!dataValid)
                    {
                        FileManager.Log("Loading All Data", 0);
                        // sort data based on column clicked
                        DataManager.GetDinoData(Shared.selectedClass, sortM, sortF, ToggleExcluded, CurrentStats);

                        // load this data only when showing all and included
                        if (ToggleExcluded == 0 || ToggleExcluded == 1 || ToggleExcluded == 2)
                        {
                            DataManager.EvaluateDinos();
                            if (!CurrentStats && !showTree)
                            {
                                DataManager.GetBestPartner();
                            }
                        }
                        dinoCount = DataManager.DinoCount(Shared.selectedClass, ToggleExcluded);
                        dataValid = true;
                    }
                }

                DinoView();
            }
            catch
            {
                FileManager.Log("Failed updating dinos", 2);
                DefaultView("Dinos exploded :O");
            }
            finally
            {
                Monitor.Exit(Shared._dbLock);
            }
        }
        else
        {
            FileManager.Log("DinoPage Failed to acquire database lock", 1);
            DefaultView("Dinos walked away :(");
        }
        ToHere("Time1"); // Stop timer and show results
    }

    private void DefaultView(string labelText)
    {
        var mainLayout = new Grid();

        mainLayout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Fixed button row
        mainLayout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star }); // Scrollable content


        var scrollContent = new StackLayout
        {
            Spacing = 20,
            Padding = 3
        };

        var image1 = new Image { Source = "dino.png", HeightRequest = 400, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Start };
        var label1 = new Label { Text = labelText, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Start, FontAttributes = FontAttributes.Bold, TextColor = Shared.PrimaryColor, FontSize = 22 };


        AddToGrid(mainLayout, image1, 0, 0);
        AddToGrid(mainLayout, label1, 1, 0);


        // Wrap the scrollable content in a ScrollView and add it to the second row
        var scrollView = new ScrollView { Content = scrollContent };

        AddToGrid(mainLayout, scrollView, 0, 0);

        // only attach the tapgesture if we have something selected
        // for now its the only way to force refresh a page
        // so we attach it to everything so we can click
        UnSelectDino(mainLayout);

        this.Content = null;
        this.Content = mainLayout;
    }

    public void DinoView()
    {
        // ==============================================================    Create Dino Layout   =====================================================

        // Create the main layout
        var mainLayout = new Grid
        {
            BackgroundColor = Shared.MainPanelColor
        };


        // create main layout with 2 columns

        // Define row definitions
        mainLayout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star }); // 0


        mainLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = 100 }); // 0
        mainLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); // 1

        ////////////////////////////////////////////////////////////////////////////////////////////////////

        // reset boxViews
        boxID = 0; boxRowID = 0;
        boxViews = new Dictionary<int, BoxView>();

        // Add side panel to left column
        AddToGrid(mainLayout, CreateSidePanel(), 0, 0);

        // Add main panel to right column
        AddToGrid(mainLayout, CreateMainPanel(), 0, 1);

        // only attach the tapgesture if we have something selected
        if (!isDouble)
        {
            UnSelectDino(mainLayout);
        }


        this.Content = mainLayout;
    }

    private void AddToGrid(Grid grid, View view, int row, int column, string title = "", bool selected = false, bool isDoubl = false, string id = "")
    {
        // Ensure rows exist up to the specified index
        while (grid.RowDefinitions.Count <= row)
        {
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });

            // Determine the even row color based on the title
            Color evenRowColor = title switch
            {
                "Male" => Shared.MainPanelColor,
                "Bottom" => Shared.BottomPanelColor,
                _ => Shared.MainPanelColor // Default color if title doesn't match
            };

            // Determine the even row color based on the title
            Color oddRowColor = title switch
            {
                "Male" => Shared.OddMPanelColor,
                "Bottom" => Shared.OddBPanelColor,
                _ => Shared.OddMPanelColor // Default color if title doesn't match
            };

            // Choose the color based on the row index
            var rowColor = grid.RowDefinitions.Count % 2 == 0
                ? evenRowColor // Even rows
                : oddRowColor; // Odd rows

            // Override if row is selected
            if (selected) { rowColor = Shared.SelectedColor; }

            // Override if in dino extended view
            if (isDoubl) { rowColor = Shared.MainPanelColor; }

            // Add a background color to the row
            var rowBackground = new BoxView { Color = rowColor };
            Grid.SetRow(rowBackground, grid.RowDefinitions.Count - 1);
            Grid.SetColumnSpan(rowBackground, grid.ColumnDefinitions.Count > 0
                ? grid.ColumnDefinitions.Count
                : 1); // Cover all columns

            // dont recolor the bottom panel since its not selectable
            if (title != "Bottom")
            {
                boxViews[boxID++] = rowBackground;
            }

            // make background on row selectable to increase surface area
            if (title != "Bottom") // not the bottom panel
            {
                if (id != "") // only when an id is passed
                {
                    SelectBG(rowBackground, id);
                }
            }

            grid.Children.Add(rowBackground);
        }

        // Set the row and column for the view
        Grid.SetRow(view, row);
        Grid.SetColumn(view, column);

        // Add the view to the grid
        grid.Children.Add(view);
    }

    private Grid CreateSidePanel()
    {
        var grid = new Grid
        {
            RowSpacing = 5,
            ColumnSpacing = 0,
            Padding = 5,
            BackgroundColor = Shared.SidePanelColor
        };

        // Define columns
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Scrollable content
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Scrollable content

        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Scrollable content
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Scrollable content
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star }); // Scrollable content

        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Scrollable content
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Scrollable content

        var ToggleBtnColor = Shared.DefaultBColor;
        var StatsBtnColor = Shared.PrimaryColor;

        if (ToggleExcluded == 0)
        {
            ToggleBtnColor = Shared.DefaultBColor;
        }
        else if (ToggleExcluded == 1)
        {
            ToggleBtnColor = Shared.PrimaryColor;
        }
        else if (ToggleExcluded == 2)
        {
            ToggleBtnColor = Shared.SecondaryColor;
        }
        else if (ToggleExcluded == 3)
        {
            ToggleBtnColor = Shared.TrinaryColor;
        }

        string ToggleBtnText = "Toggle"; string StatsBtnText = "Breeding";
        if (ToggleExcluded == 0) { ToggleBtnText = "All"; }
        else if (ToggleExcluded == 1) { ToggleBtnText = "Included"; }
        else if (ToggleExcluded == 2) { ToggleBtnText = "Excluded"; }
        else if (ToggleExcluded == 3) { ToggleBtnText = "Archived"; }

        if (CurrentStats) { StatsBtnText = "Current"; StatsBtnColor = Shared.SecondaryColor; }


        if (isDouble)
        {
            var SaveBtn = new Button { Text = "Save", BackgroundColor = Shared.TrinaryColor };
            SaveBtn.Clicked += SaveBtnClicked;
            AddToGrid(grid, SaveBtn, 0, 0);


            var BackBtn = new Button { Text = "Back", BackgroundColor = Shared.PrimaryColor };
            BackBtn.Clicked += BackBtnClicked;
            AddToGrid(grid, BackBtn, 1, 0);
        }
        else if (showTree)
        {
            var BackBtn = new Button { Text = "Back", BackgroundColor = Shared.PrimaryColor };
            BackBtn.Clicked += BackBtnClicked;
            AddToGrid(grid, BackBtn, 0, 0);
        }
        else
        {
            var ToggleBtn = new Button { Text = ToggleBtnText, BackgroundColor = ToggleBtnColor };
            ToggleBtn.Clicked += ToggleBtnClicked;
            AddToGrid(grid, ToggleBtn, 0, 0);


            var StatsBtn = new Button { Text = StatsBtnText, BackgroundColor = StatsBtnColor };
            StatsBtn.Clicked += StatsBtnClicked;
            AddToGrid(grid, StatsBtn, 1, 0);
        }

        if (!showTree && !isDouble)
        {
            var TreeBtn = new Button { Text = "Heritage", BackgroundColor = DefaultBColor };
            TreeBtn.Clicked += TreeBtnClicked;
            AddToGrid(grid, TreeBtn, 3, 0);
        }

        // ExcludeBtn.Text = "Include";

        ExcludeBtn = new Button { Text = "" };
        ExcludeBtn.Clicked += ExcludeBtnClicked;
        AddToGrid(grid, ExcludeBtn, 5, 0);

        ArchiveBtn = new Button { Text = "" };
        ArchiveBtn.Clicked += ArchiveBtnClicked;
        AddToGrid(grid, ArchiveBtn, 6, 0);

        string group = DataManager.GetGroup(selectedID);
        if (group == "Exclude") { ExcludeBtn.Text = "Include"; ExcludeBtn.BackgroundColor = Shared.PrimaryColor; }
        else { ExcludeBtn.Text = "Exclude"; ExcludeBtn.BackgroundColor = Shared.SecondaryColor; }

        if (group == "Archived") { ArchiveBtn.Text = "Include"; ArchiveBtn.BackgroundColor = Shared.PrimaryColor; }
        else { ArchiveBtn.Text = "Archive"; ArchiveBtn.BackgroundColor = Shared.TrinaryColor; }

        if (!isSelected)
        {
            ExcludeBtn.IsVisible = false;
            ArchiveBtn.IsVisible = false;
        }
        else
        {
            ExcludeBtn.IsVisible = true;
            ArchiveBtn.IsVisible = true;
        }

        return grid;
    }

    private Grid CreateMainPanel()
    {
        editStats = false;
        var maingrid = new Grid
        {
            RowSpacing = 0,
            ColumnSpacing = 5,
            Padding = 0,
        };

        // Define columns
        maingrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); // 0

        // Dynamically adjust the bottom bar height
        int rowCount = DataManager.BottomTable.Rows.Count;
        int maxVisibleRows = 5; int barH;
        int buffer = Shared.sizeOffset; // Extra buffer to prevent scrolling

        if (rowCount > 0)
        {
            // Adjust based on row count
            int offset = 13 - Math.Min(rowCount, maxVisibleRows) * 4;
            barH = (Math.Min(rowCount, maxVisibleRows) * Shared.rowHeight) + Shared.headerSize + offset + buffer;
            if (rowCount > 5) { barH = 127; } // prevent showing the top of the 6th row
        }
        else
        {
            barH = 0; // No rows, no bar height
        }

        // FileManager.Log($"barH: {barH} = {rowCount} * {Shared.rowHeight} + {Shared.headerSize} + {ofs}", 0);

        if (showTree || CurrentStats || DataManager.BottomTable.Rows.Count < 1) { barH = 0; }

        // Define row definitions
        maingrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star }); // Scrollable content




        ////////////////////////////////////////////////////////////////////////////////////////////////////

        if (dinoCount > 0 && !isDouble && !showTree) // more than 0 dinos and not double clicked and not in showTree
        {
            // create the row for bottompanel if not in dinoEview
            maingrid.RowDefinitions.Add(new RowDefinition { Height = barH }); // Scrollable content

            // Create scrollable content
            var scrollContent = new StackLayout
            {
                Spacing = 20,
                Padding = 3
            };

            // Add male and female tables
            scrollContent.Children.Add(CreateDinoGrid(DataManager.MaleTable, "Male"));



            scrollContent.Children.Add(CreateDinoGrid(DataManager.FemaleTable, "Female"));



            // Wrap the scrollable content in a ScrollView and add it to the second row
            // changed to include horizontal scrolling
            var scrollView = new ScrollView { Content = scrollContent, Orientation = ScrollOrientation.Horizontal };

            AddToGrid(maingrid, scrollView, 0, 1);

            ////////////////////////////////////////////////////////////////////////////////////////////////////

            // Create scrollable content
            var bottomContent = new StackLayout
            {
                Spacing = 0,
                Padding = 3,
                BackgroundColor = Shared.BottomPanelColor
            };


            bottomContent.Children.Add(CreateDinoGrid(DataManager.BottomTable, "Bottom"));


            // Wrap the scrollable content in a ScrollView and add it to the third row
            var bottomPanel = new ScrollView { Content = bottomContent };

            AddToGrid(maingrid, bottomPanel, 1, 1);

            ////////////////////////////////////////////////////////////////////////////////////////////////////
        }
        else if (dinoCount > 0 && isDouble && !showTree)
        {
            ////////////////////////////////////////////////////////////////////////////////////////////////////
            // make dino info box

            // Create scrollable content
            var scrollContent = new StackLayout { Spacing = 20, Padding = 3, };

            // Create Grid for stats
            var statGrid = new Grid { RowSpacing = 0, ColumnSpacing = 20, Padding = 3 };

            // Define columns
            statGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // 0
            statGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // 0
            statGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // 0
            statGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // 0

            // Get info about selected dino
            string currentID = selectedID;
            string sex = DataManager.GetLastColumnData("ID", currentID, "Sex");


            Color DefaultColor = Shared.maleColor;

            DefaultColor = Shared.maleColor;
            if (sex == "Male") { DefaultColor = Shared.maleColor; }
            else if (sex == "Female") { DefaultColor = Shared.femaleColor; }

            var cellColor0 = DefaultColor;
            var cellColor1 = DefaultColor;
            var cellColor2 = DefaultColor;
            var cellColor3 = DefaultColor;
            var cellColor4 = DefaultColor;
            var cellColor5 = DefaultColor;
            var cellColor6 = DefaultColor;
            var cellColor7 = DefaultColor;
            var cellColor8 = DefaultColor;
            var cellColor9 = DefaultColor;

            // get the current breed stats of selected dino
            string sep = DataManager.DecimalSeparator;

            string level = DataManager.GetFirstColumnData("ID", currentID, "Level").Replace(".", sep);
            string hp = DataManager.GetFirstColumnData("ID", currentID, "Hp").Replace(".", sep);
            string stamina = DataManager.GetFirstColumnData("ID", currentID, "Stamina").Replace(".", sep);
            string O2 = DataManager.GetFirstColumnData("ID", currentID, "Oxygen").Replace(".", sep);
            string food = DataManager.GetFirstColumnData("ID", currentID, "Food").Replace(".", sep);
            string weight = DataManager.GetFirstColumnData("ID", currentID, "Weight").Replace(".", sep);
            string damage = DataManager.GetFirstColumnData("ID", currentID, "Damage").Replace(".", sep);

            string speed = DataManager.GetFirstColumnData("ID", currentID, "Speed").Replace(".", sep);
            string craft = DataManager.GetFirstColumnData("ID", currentID, "CraftSkill").Replace(".", sep);

            //set the temp variables
            levelText = level;
            hpText = hp;
            staminaText = stamina;
            O2Text = O2;
            foodText = food;
            weightText = weight;
            damageText = damage;
            speedText = speed;
            craftText = craft;


            //recolor stats (use -0.1 to account for rounding)
            if (DataManager.ToDouble(level) >= (DataManager.LevelMax - 0.1)) { cellColor1 = Shared.goodColor; }
            if (DataManager.ToDouble(hp) >= DataManager.HpMax - 0.1) { cellColor2 = Shared.goodColor; }
            if (DataManager.ToDouble(stamina) >= DataManager.StaminaMax - 0.1) { cellColor3 = Shared.goodColor; }
            if (DataManager.ToDouble(O2) >= DataManager.O2Max - 0.1) { cellColor4 = Shared.goodColor; }
            if (DataManager.ToDouble(food) >= DataManager.FoodMax - 0.1) { cellColor5 = Shared.goodColor; }
            if (DataManager.ToDouble(weight) >= DataManager.WeightMax - 0.1) { cellColor6 = Shared.goodColor; }
            if ((DataManager.ToDouble(damage) + 1) * 100 >= DataManager.DamageMax - 0.1) { cellColor7 = Shared.goodColor; }
            if ((DataManager.ToDouble(speed) + 1) * 100 >= DataManager.SpeedMax - 0.1) { cellColor8 = Shared.goodColor; }
            if ((DataManager.ToDouble(craft) + 1) * 100 >= DataManager.CraftMax - 0.1) { cellColor9 = Shared.goodColor; }


            // mutation detection overrides normal coloring -> mutaColor
            string mutes = DataManager.GetMutes(currentID);
            if (mutes.Length >= 7 && !CurrentStats) // dont show mutations on current statview
            {
                string aC = mutes.Substring(0, 1); string bC = mutes.Substring(1, 1); string cC = mutes.Substring(2, 1);
                string dC = mutes.Substring(3, 1); string eC = mutes.Substring(4, 1); string fC = mutes.Substring(5, 1);
                string gC = mutes.Substring(6, 1);

                if (aC == "1") { cellColor2 = Shared.mutaColor; }
                if (bC == "1") { cellColor3 = Shared.mutaColor; }
                if (cC == "1") { cellColor4 = Shared.mutaColor; }
                if (dC == "1") { cellColor5 = Shared.mutaColor; }
                if (eC == "1") { cellColor6 = Shared.mutaColor; }
                if (fC == "1") { cellColor7 = Shared.mutaColor; }
                if (gC == "1") { cellColor8 = Shared.mutaColor; }

            }



            // add stat text
            var t0 = new Label { Text = "", Style = (Style)Application.Current.Resources["Headline"], TextColor = Shared.maleColor, FontSize = Shared.fontHSize, FontAttributes = FontAttributes.Bold };
            var t1 = new Label { Text = "Level", TextColor = cellColor1, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
            var t2 = new Label { Text = "Hp", TextColor = cellColor2, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
            var t3 = new Label { Text = "Stamina", TextColor = cellColor3, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
            var t4 = new Label { Text = "O2", TextColor = cellColor4, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
            var t5 = new Label { Text = "Food", TextColor = cellColor5, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
            var t6 = new Label { Text = "Weight", TextColor = cellColor6, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
            var t7 = new Label { Text = "Damage", TextColor = cellColor7, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
            var t8 = new Label { Text = "Speed", TextColor = cellColor8, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
            var t9 = new Label { Text = "Crafting", TextColor = cellColor9, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };



            int rowid = 0;
            int colid = 0;
            AddToGrid(statGrid, t0, rowid++, colid, "", false, true);
            AddToGrid(statGrid, t1, rowid++, colid, "", false, true);
            AddToGrid(statGrid, t2, rowid++, colid, "", false, true);
            AddToGrid(statGrid, t3, rowid++, colid, "", false, true);
            AddToGrid(statGrid, t4, rowid++, colid, "", false, true);
            AddToGrid(statGrid, t5, rowid++, colid, "", false, true);
            AddToGrid(statGrid, t6, rowid++, colid, "", false, true);
            AddToGrid(statGrid, t7, rowid++, colid, "", false, true);
            AddToGrid(statGrid, t8, rowid++, colid, "", false, true);
            AddToGrid(statGrid, t9, rowid++, colid, "", false, true);




            var editLabel = new Label
            {
                Text = "Breeding Stats",
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start,
                Style = (Style)Application.Current.Resources["Headline"],
                TextColor = cellColor0,
                FontSize = Shared.fontHSize,
                FontAttributes = FontAttributes.Bold
            };

            var textBox1 = new Entry { Text = level, Placeholder = "Level", WidthRequest = 200, HeightRequest = 10, TextColor = cellColor1, BackgroundColor = Shared.OddMPanelColor, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start };
            var textBox2 = new Entry { Text = hp, Placeholder = "Hp", WidthRequest = 200, HeightRequest = 10, TextColor = cellColor2, BackgroundColor = Shared.OddMPanelColor, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start };
            var textBox3 = new Entry { Text = stamina, Placeholder = "Stamina", WidthRequest = 200, HeightRequest = 10, TextColor = cellColor3, BackgroundColor = Shared.OddMPanelColor, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start };
            var textBox4 = new Entry { Text = O2, Placeholder = "O2", WidthRequest = 200, HeightRequest = 10, TextColor = cellColor4, BackgroundColor = Shared.OddMPanelColor, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start };
            var textBox5 = new Entry { Text = food, Placeholder = "Food", WidthRequest = 200, HeightRequest = 10, TextColor = cellColor5, BackgroundColor = Shared.OddMPanelColor, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start };
            var textBox6 = new Entry { Text = weight, Placeholder = "Weight", WidthRequest = 200, HeightRequest = 10, TextColor = cellColor6, BackgroundColor = Shared.OddMPanelColor, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start };
            var textBox7 = new Entry { Text = damage, Placeholder = "Damage", WidthRequest = 200, HeightRequest = 10, TextColor = cellColor7, BackgroundColor = Shared.OddMPanelColor, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start };
            var textBox8 = new Entry { Text = speed, Placeholder = "Speed", WidthRequest = 200, HeightRequest = 10, TextColor = cellColor8, BackgroundColor = Shared.OddMPanelColor, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start };
            var textBox9 = new Entry { Text = craft, Placeholder = "Crafting", WidthRequest = 200, HeightRequest = 10, TextColor = cellColor9, BackgroundColor = Shared.OddMPanelColor, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start };



            textBox1.TextChanged += (sender, e) =>
            {
                if (!IsValidDouble(e.NewTextValue)) { ((Entry)sender).Text = e.OldTextValue; }
                else { levelText = e.NewTextValue; }
            };
            textBox2.TextChanged += (sender, e) =>
            {
                if (!IsValidDouble(e.NewTextValue)) { ((Entry)sender).Text = e.OldTextValue; }
                else { hpText = e.NewTextValue; }
            };
            textBox3.TextChanged += (sender, e) =>
            {
                if (!IsValidDouble(e.NewTextValue)) { ((Entry)sender).Text = e.OldTextValue; }
                else { staminaText = e.NewTextValue; }
                staminaText = e.NewTextValue;
            };
            textBox4.TextChanged += (sender, e) =>
            {
                if (!IsValidDouble(e.NewTextValue)) { ((Entry)sender).Text = e.OldTextValue; }
                else { O2Text = e.NewTextValue; }
            };
            textBox5.TextChanged += (sender, e) =>
            {
                if (!IsValidDouble(e.NewTextValue)) { ((Entry)sender).Text = e.OldTextValue; }
                else { foodText = e.NewTextValue; }
            };
            textBox6.TextChanged += (sender, e) =>
            {
                if (!IsValidDouble(e.NewTextValue)) { ((Entry)sender).Text = e.OldTextValue; }
                else { weightText = e.NewTextValue; }
            };
            textBox7.TextChanged += (sender, e) =>
            {
                if (!IsValidDouble(e.NewTextValue)) { ((Entry)sender).Text = e.OldTextValue; }
                else { damageText = e.NewTextValue; }
            };

            textBox8.TextChanged += (sender, e) =>
            {
                if (!IsValidDouble(e.NewTextValue)) { ((Entry)sender).Text = e.OldTextValue; }
                else { speedText = e.NewTextValue; }
            };
            textBox9.TextChanged += (sender, e) =>
            {
                if (!IsValidDouble(e.NewTextValue)) { ((Entry)sender).Text = e.OldTextValue; }
                else { craftText = e.NewTextValue; }
            };


            // AddToGrid(grid1, imageContainer, 0, 1);

            rowid = 0;
            colid = 1;

            AddToGrid(statGrid, editLabel, rowid++, colid, "", false, true);
            AddToGrid(statGrid, textBox1, rowid++, colid, "", false, true);
            AddToGrid(statGrid, textBox2, rowid++, colid, "", false, true);
            AddToGrid(statGrid, textBox3, rowid++, colid, "", false, true);
            AddToGrid(statGrid, textBox4, rowid++, colid, "", false, true);
            AddToGrid(statGrid, textBox5, rowid++, colid, "", false, true);
            AddToGrid(statGrid, textBox6, rowid++, colid, "", false, true);
            AddToGrid(statGrid, textBox7, rowid++, colid, "", false, true);
            AddToGrid(statGrid, textBox8, rowid++, colid, "", false, true);
            AddToGrid(statGrid, textBox9, rowid++, colid, "", false, true);


            // get parents id

            string papaID = DataManager.GetLastColumnData("ID", currentID, "Papa");
            string mamaID = DataManager.GetLastColumnData("ID", currentID, "Mama");

            string papaName = DataManager.GetLastColumnData("ID", papaID, "Name");
            string mamaName = DataManager.GetLastColumnData("ID", mamaID, "Name");

            if (papaName == "") { papaName = "Papa Stats"; }
            if (mamaName == "") { mamaName = "Mama Stats"; }


            string levelP = DataManager.GetFirstColumnData("ID", papaID, "Level").Replace(".", sep);
            string hpP = DataManager.GetFirstColumnData("ID", papaID, "Hp").Replace(".", sep);
            string staminaP = DataManager.GetFirstColumnData("ID", papaID, "Stamina").Replace(".", sep);
            string O2P = DataManager.GetFirstColumnData("ID", papaID, "Oxygen").Replace(".", sep);
            string foodP = DataManager.GetFirstColumnData("ID", papaID, "Food").Replace(".", sep);
            string weightP = DataManager.GetFirstColumnData("ID", papaID, "Weight").Replace(".", sep);
            string damageP = DataManager.GetFirstColumnData("ID", papaID, "Damage").Replace(".", sep);
            string speedP = DataManager.GetFirstColumnData("ID", papaID, "Speed").Replace(".", sep);
            string craftP = DataManager.GetFirstColumnData("ID", papaID, "CraftSkill").Replace(".", sep);


            DefaultColor = Shared.maleColor;


            cellColor1 = DefaultColor;
            cellColor2 = DefaultColor;
            cellColor3 = DefaultColor;
            cellColor4 = DefaultColor;
            cellColor5 = DefaultColor;
            cellColor6 = DefaultColor;
            cellColor7 = DefaultColor;
            cellColor8 = DefaultColor;
            cellColor9 = DefaultColor;



            //recolor stats (use -0.1 to account for rounding)
            if (DataManager.ToDouble(levelP) >= (DataManager.LevelMax - 0.1)) { cellColor1 = Shared.goodColor; }
            if (DataManager.ToDouble(hpP) >= DataManager.HpMax - 0.1) { cellColor2 = Shared.goodColor; }
            if (DataManager.ToDouble(staminaP) >= DataManager.StaminaMax - 0.1) { cellColor3 = Shared.goodColor; }
            if (DataManager.ToDouble(O2P) >= DataManager.O2Max - 0.1) { cellColor4 = Shared.goodColor; }
            if (DataManager.ToDouble(foodP) >= DataManager.FoodMax - 0.1) { cellColor5 = Shared.goodColor; }
            if (DataManager.ToDouble(weightP) >= DataManager.WeightMax - 0.1) { cellColor6 = Shared.goodColor; }
            if ((DataManager.ToDouble(damageP) + 1) * 100 >= DataManager.DamageMax - 0.1) { cellColor7 = Shared.goodColor; }
            if ((DataManager.ToDouble(speedP) + 1) * 100 >= DataManager.SpeedMax - 0.1) { cellColor8 = Shared.goodColor; }
            if ((DataManager.ToDouble(craftP) + 1) * 100 >= DataManager.CraftMax - 0.1) { cellColor9 = Shared.goodColor; }



            // add papa stats
            var papaH = new Label { Text = papaName, Style = (Style)Application.Current.Resources["Headline"], TextColor = Shared.maleColor, FontSize = Shared.fontHSize, FontAttributes = FontAttributes.Bold };
            var labelP1 = new Label { Text = levelP, TextColor = cellColor1, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
            var labelP2 = new Label { Text = hpP, TextColor = cellColor2, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
            var labelP3 = new Label { Text = staminaP, TextColor = cellColor3, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
            var labelP4 = new Label { Text = O2P, TextColor = cellColor4, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
            var labelP5 = new Label { Text = foodP, TextColor = cellColor5, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
            var labelP6 = new Label { Text = weightP, TextColor = cellColor6, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
            var labelP7 = new Label { Text = damageP, TextColor = cellColor7, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
            var labelP8 = new Label { Text = speedP, TextColor = cellColor8, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
            var labelP9 = new Label { Text = craftP, TextColor = cellColor9, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };



            rowid = 0;
            colid = 2;
            AddToGrid(statGrid, papaH, rowid++, colid, "", false, true);
            AddToGrid(statGrid, labelP1, rowid++, colid, "", false, true);
            AddToGrid(statGrid, labelP2, rowid++, colid, "", false, true);
            AddToGrid(statGrid, labelP3, rowid++, colid, "", false, true);
            AddToGrid(statGrid, labelP4, rowid++, colid, "", false, true);
            AddToGrid(statGrid, labelP5, rowid++, colid, "", false, true);
            AddToGrid(statGrid, labelP6, rowid++, colid, "", false, true);
            AddToGrid(statGrid, labelP7, rowid++, colid, "", false, true);
            AddToGrid(statGrid, labelP8, rowid++, colid, "", false, true);
            AddToGrid(statGrid, labelP9, rowid++, colid, "", false, true);



            string levelM = DataManager.GetFirstColumnData("ID", mamaID, "Level").Replace(".", sep);
            string hpM = DataManager.GetFirstColumnData("ID", mamaID, "Hp").Replace(".", sep);
            string staminaM = DataManager.GetFirstColumnData("ID", mamaID, "Stamina").Replace(".", sep);
            string O2M = DataManager.GetFirstColumnData("ID", mamaID, "Oxygen").Replace(".", sep);
            string foodM = DataManager.GetFirstColumnData("ID", mamaID, "Food").Replace(".", sep);
            string weightM = DataManager.GetFirstColumnData("ID", mamaID, "Weight").Replace(".", sep);
            string damageM = DataManager.GetFirstColumnData("ID", mamaID, "Damage").Replace(".", sep);
            string speedM = DataManager.GetFirstColumnData("ID", mamaID, "Speed").Replace(".", sep);
            string craftM = DataManager.GetFirstColumnData("ID", mamaID, "CraftSkill").Replace(".", sep);



            DefaultColor = Shared.femaleColor;

            cellColor1 = DefaultColor;
            cellColor2 = DefaultColor;
            cellColor3 = DefaultColor;
            cellColor4 = DefaultColor;
            cellColor5 = DefaultColor;
            cellColor6 = DefaultColor;
            cellColor7 = DefaultColor;
            cellColor8 = DefaultColor;
            cellColor9 = DefaultColor;

            //recolor stats (use -0.1 to account for rounding)
            if (DataManager.ToDouble(levelM) >= (DataManager.LevelMax - 0.1)) { cellColor1 = Shared.goodColor; }
            if (DataManager.ToDouble(hpM) >= DataManager.HpMax - 0.1) { cellColor2 = Shared.goodColor; }
            if (DataManager.ToDouble(staminaM) >= DataManager.StaminaMax - 0.1) { cellColor3 = Shared.goodColor; }
            if (DataManager.ToDouble(O2M) >= DataManager.O2Max - 0.1) { cellColor4 = Shared.goodColor; }
            if (DataManager.ToDouble(foodM) >= DataManager.FoodMax - 0.1) { cellColor5 = Shared.goodColor; }
            if (DataManager.ToDouble(weightM) >= DataManager.WeightMax - 0.1) { cellColor6 = Shared.goodColor; }
            if ((DataManager.ToDouble(damageM) + 1) * 100 >= DataManager.DamageMax - 0.1) { cellColor7 = Shared.goodColor; }
            if ((DataManager.ToDouble(speedM) + 1) * 100 >= DataManager.SpeedMax - 0.1) { cellColor8 = Shared.goodColor; }
            if ((DataManager.ToDouble(craftM) + 1) * 100 >= DataManager.CraftMax - 0.1) { cellColor9 = Shared.goodColor; }


            // add mama stats
            var mamaH = new Label { Text = mamaName, Style = (Style)Application.Current.Resources["Headline"], TextColor = Shared.femaleColor, FontSize = Shared.fontHSize, FontAttributes = FontAttributes.Bold };

            var labelM1 = new Label { Text = levelM, TextColor = cellColor1, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
            var labelM2 = new Label { Text = hpM, TextColor = cellColor2, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
            var labelM3 = new Label { Text = staminaM, TextColor = cellColor3, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
            var labelM4 = new Label { Text = O2M, TextColor = cellColor4, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
            var labelM5 = new Label { Text = foodM, TextColor = cellColor5, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
            var labelM6 = new Label { Text = weightM, TextColor = cellColor6, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
            var labelM7 = new Label { Text = damageM, TextColor = cellColor7, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
            var labelM8 = new Label { Text = speedM, TextColor = cellColor8, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
            var labelM9 = new Label { Text = craftM, TextColor = cellColor9, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };


            rowid = 0;
            colid = 3;
            AddToGrid(statGrid, mamaH, rowid++, colid, "", false, true);
            AddToGrid(statGrid, labelM1, rowid++, colid, "", false, true);
            AddToGrid(statGrid, labelM2, rowid++, colid, "", false, true);
            AddToGrid(statGrid, labelM3, rowid++, colid, "", false, true);
            AddToGrid(statGrid, labelM4, rowid++, colid, "", false, true);
            AddToGrid(statGrid, labelM5, rowid++, colid, "", false, true);
            AddToGrid(statGrid, labelM6, rowid++, colid, "", false, true);
            AddToGrid(statGrid, labelM7, rowid++, colid, "", false, true);
            AddToGrid(statGrid, labelM8, rowid++, colid, "", false, true);
            AddToGrid(statGrid, labelM9, rowid++, colid, "", false, true);


            scrollContent.Children.Add(statGrid);


            rowid = 0;
            var notesGrid = new Grid
            {
                RowSpacing = 0,
                ColumnSpacing = 20,
                Padding = 3
            };
            // Define columns
            notesGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); // 0


            string notes = DataManager.GetNotes(currentID);


            string ageText = ""; bool validAgeRate = false;
            Color growColor = Shared.PrimaryColor;


            var grown = new Label { Text = ageText, Style = (Style)Application.Current.Resources["Headline"], TextColor = growColor, FontSize = Shared.fontHSize, FontAttributes = FontAttributes.Bold, HorizontalOptions = LayoutOptions.Start };

            // notes textbox defined here
            var textBoxN = new Editor { Text = notes, Placeholder = "Notes", WidthRequest = 600, HeightRequest = 200, TextColor = cellColor0, BackgroundColor = Shared.OddMPanelColor, FontSize = 16, HorizontalOptions = LayoutOptions.Start, Keyboard = Keyboard.Create(KeyboardFlags.None) };

            textBoxN.TextChanged += (sender, e) =>
            {
                editStats = true;
                notesText = e.NewTextValue;
            };

            if (validAgeRate) // only add label if dino is still aging
            {
                AddToGrid(notesGrid, grown, rowid++, 0, "", false, true);
            }

            AddToGrid(notesGrid, textBoxN, rowid++, 0, "", false, true);


            scrollContent.Children.Add(notesGrid);

            var scrollView = new ScrollView { Content = scrollContent };

            AddToGrid(maingrid, scrollView, 0, 0);
            ////////////////////////////////////////////////////////////////////////////////////////////////////
        }
        else if (showTree)
        {
            ////////////////////////////////////////////////////////////////////////////////////////////////////
            // make tree of life grid

            // Create scrollable content
            var scrollContent = new StackLayout { Spacing = 20, Padding = 3, };

            // Create Grid for generations
            var genGrid = new Grid { RowSpacing = 0, ColumnSpacing = 20, Padding = 3 };

            // get amount of generations
            double gen = DataManager.MaxGenerations(selectedClass);


            int i = 0;
            while (i <= gen)
            {
                // Define a column for each generation
                genGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });



                // add stuff to each column for each generation
                int rowid = 0; // starting at row 0
                string bText = $"Generation: {i}";
                if (i == 0) { bText = $"Generation: {Smap["Missing"]}"; }
                var t = new Label { Text = bText, TextColor = Shared.goldColor, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
                AddToGrid(genGrid, t, rowid++, i, "", false, true);


                string[] pairs = DataManager.GetGenParents(selectedClass, i);
                foreach (string pair in pairs)
                {
                    var parts = pair.Split(',');

                    // Check for missing or empty values
                    string papaID = parts.Length > 0 && !string.IsNullOrWhiteSpace(parts[0]) ? parts[0] : "UnknownPapa";
                    string mamaID = parts.Length > 1 && !string.IsNullOrWhiteSpace(parts[1]) ? parts[1] : "UnknownMama";

                    string papaName = DataManager.GetLastColumnData("ID", papaID, "Name");
                    string mamaName = DataManager.GetLastColumnData("ID", mamaID, "Name");


                    if (papaName == "")
                    {
                        if (papaID == "00") { papaName = Shared.Smap["Unknown"]; }
                        else { papaName = Shared.Smap["Missing"]; }
                    }
                    if (mamaName == "") 
                    {
                        if (mamaID == "00") { mamaName = Shared.Smap["Unknown"]; }
                        else { mamaName = Shared.Smap["Missing"]; }
                    }


                    var t0 = new Label { Text = $"{papaName} + {mamaName}", TextColor = Shared.maleColor, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
                    AddToGrid(genGrid, t0, rowid++, i, "", false, true);

                    string[] kidsfrompair = DataManager.GetKidsFromPair(selectedClass, pair);
                    foreach (string kid in kidsfrompair)
                    {
                        string kidName = DataManager.GetLastColumnData("ID", kid, "Name");

                        if (kidName == "") { kidName = kid; }

                        var t1 = new Label { Text = $"{kidName}", TextColor = Shared.bottomColor, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
                        AddToGrid(genGrid, t1, rowid++, i, "", false, true);

                    }


                    var t9 = new Label { Text = $" ", TextColor = Shared.maleColor, FontSize = Shared.fontSize, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
                    AddToGrid(genGrid, t9, rowid++, i, "", false, true);
                }


                i++;
            }



            scrollContent.Children.Add(genGrid);

            var scrollView = new ScrollView { Content = scrollContent };

            AddToGrid(maingrid, scrollView, 0, 0);
        }
        else
        {
            ////////////////////////////////////////////////////////////////////////////////////////////////////
            // create empty table content

            // Create scrollable content
            var scrollContent = new StackLayout
            {
                Spacing = 20,
                Padding = 3
            };

            // Create grid to put data in
            var grid1 = new Grid
            {
                RowSpacing = 0,
                ColumnSpacing = 20,
                Padding = 3
            };
            // Define columns
            maingrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // 0


            var imageContainer = new Grid
            {
                BackgroundColor = Shared.MainPanelColor, // Set the background color here
                Padding = 0
            };

            var image = new Image
            {
                Source = "dino.png",
                HeightRequest = 400,
                Aspect = Aspect.AspectFit
            };

            // Add the image to the container
            imageContainer.Children.Add(image);

            var label1 = new Label
            {
                Text = "No dinos in here :/",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Style = (Style)Application.Current.Resources["Headline"],
                TextColor = Shared.goodColor,
                FontSize = 22,
                FontAttributes = FontAttributes.Bold
            };


            AddToGrid(grid1, imageContainer, 0, 0);
            AddToGrid(grid1, label1, 1, 0);



            scrollContent.Children.Add(grid1);

            var scrollView = new ScrollView { Content = scrollContent };

            AddToGrid(maingrid, scrollView, 0, 0);

            ////////////////////////////////////////////////////////////////////////////////////////////////////

        }




        return maingrid;
    }

    private Grid CreateDinoGrid(DataTable table, string title)
    {
        var grid = new Grid { RowSpacing = 0, ColumnSpacing = 20, Padding = 3 };

        // Define columns
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // 0
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // 1
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // 2
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // 3
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // 4
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // 5
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // 6
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // 7
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // 8
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // 9
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // 10
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // 11
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // 12
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // 13
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // 14
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // 15
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // 16
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // 17

        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star }); // 18


        Color DefaultColor = Shared.maleColor;
        Color headerColor = DefaultColor;

        if (title == "Male") { DefaultColor = Shared.maleColor; headerColor = Shared.maleHeaderColor; }
        else if (title == "Female") { DefaultColor = Shared.femaleColor; headerColor = Shared.femaleHeaderColor; }
        else { DefaultColor = Shared.bottomColor; headerColor = Shared.bottomHeaderColor; }

        // check for sats we dont need
        bool hasO2 = true; bool hasSpeed = false; bool hasCraft = true;
        if (DataManager.O2Max == 150) { hasO2 = false; }
        if (DataManager.CraftMax == 100) { hasCraft = false; }

        if (title != "Bottom") { hasSpeed = true; } // dont activate for offspring since speed doesnt breed

        int fSize = Shared.headerSize;  // header fontsize

        // add sorting symbol to the sorted column
        string sortChar = "";

        // find out wich table we are sorting
        string tableSort = "";
        if (title == "Male") { tableSort = sortM; }
        else if (title == "Female") { tableSort = sortF; }


        string newTest = "";
        if (tableSort.Contains("ASC")) { newTest = tableSort.Substring(0, tableSort.Length - 4); }
        if (tableSort.Contains("DESC")) { newTest = tableSort.Substring(0, tableSort.Length - 5); }

        string upChar = Smap["SortUp"];
        string downChar = Smap["SortDown"];


        // maybe some width adjustment for headers to line up the tables
        int colID = 0;
        int[] cellW = { 110, 100 }; // maybe figure out the max width of any cells
        // , WidthRequest = cellW[colID++]

        sortChar = ""; if (newTest == $"{Smap["Name"]}Name") { if (tableSort.Contains("ASC")) { sortChar = " " + upChar; } if (tableSort.Contains("DESC")) { sortChar = " " + downChar; } }
        var nameH = new Label { Text = $"{Smap["Name"]}Name{sortChar}", FontAttributes = FontAttributes.Bold, TextColor = headerColor, FontSize = fSize, WidthRequest = cellW[colID++] };
        sortChar = ""; if (newTest == $"{Smap["Level"]}Level") { if (tableSort.Contains("ASC")) { sortChar = " " + upChar; } if (tableSort.Contains("DESC")) { sortChar = " " + downChar; } }
        var levelH = new Label { Text = $"{Smap["Level"]}Level{sortChar}", FontAttributes = FontAttributes.Bold, TextColor = headerColor, FontSize = fSize };

        sortChar = ""; if (newTest == $"{Smap["Hp"]}Hp") { if (tableSort.Contains("ASC")) { sortChar = " " + upChar; } if (tableSort.Contains("DESC")) { sortChar = " " + downChar; } }
        var hpH = new Label { Text = $"{Smap["Hp"]}Hp{sortChar}", FontAttributes = FontAttributes.Bold, TextColor = headerColor, FontSize = fSize };
        sortChar = ""; if (newTest == $"{Smap["Stamina"]}Stamina") { if (tableSort.Contains("ASC")) { sortChar = " " + upChar; } if (tableSort.Contains("DESC")) { sortChar = " " + downChar; } }
        var staminaH = new Label { Text = $"{Smap["Stamina"]}Stamina{sortChar}", FontAttributes = FontAttributes.Bold, TextColor = headerColor, FontSize = fSize };
        sortChar = ""; if (newTest == $"{Smap["O2"]}O2") { if (tableSort.Contains("ASC")) { sortChar = " " + upChar; } if (tableSort.Contains("DESC")) { sortChar = " " + downChar; } }
        var O2H = new Label { Text = $"{Smap["O2"]}O2{sortChar}", FontAttributes = FontAttributes.Bold, TextColor = headerColor, FontSize = fSize };
        sortChar = ""; if (newTest == $"{Smap["Food"]}Food") { if (tableSort.Contains("ASC")) { sortChar = " " + upChar; } if (tableSort.Contains("DESC")) { sortChar = " " + downChar; } }
        var foodH = new Label { Text = $"{Smap["Food"]}Food{sortChar}", FontAttributes = FontAttributes.Bold, TextColor = headerColor, FontSize = fSize };
        sortChar = ""; if (newTest == $"{Smap["Weight"]}Weight") { if (tableSort.Contains("ASC")) { sortChar = " " + upChar; } if (tableSort.Contains("DESC")) { sortChar = " " + downChar; } }
        var weightH = new Label { Text = $"{Smap["Weight"]}Weight{sortChar}", FontAttributes = FontAttributes.Bold, TextColor = headerColor, FontSize = fSize };
        sortChar = ""; if (newTest == $"{Smap["Damage"]}Dmg") { if (tableSort.Contains("ASC")) { sortChar = " " + upChar; } if (tableSort.Contains("DESC")) { sortChar = " " + downChar; } }
        var damageH = new Label { Text = $"{Smap["Damage"]}Dmg{sortChar}", FontAttributes = FontAttributes.Bold, TextColor = headerColor, FontSize = fSize };
        sortChar = ""; if (newTest == $"{Smap["Crafting"]}Crafting") { if (tableSort.Contains("ASC")) { sortChar = " " + upChar; } if (tableSort.Contains("DESC")) { sortChar = " " + downChar; } }
        var craftH = new Label { Text = $"{Smap["Crafting"]}Crafting{sortChar}", FontAttributes = FontAttributes.Bold, TextColor = headerColor, FontSize = fSize };

        sortChar = ""; if (newTest == $"{Smap["Speed"]}Speed") { if (tableSort.Contains("ASC")) { sortChar = " " + upChar; } if (tableSort.Contains("DESC")) { sortChar = " " + downChar; } }
        var speedH = new Label { Text = $"{Smap["Speed"]}Speed{sortChar}", FontAttributes = FontAttributes.Bold, TextColor = headerColor, FontSize = fSize };
        sortChar = ""; if (newTest == $"{Smap["Status"]}Status") { if (tableSort.Contains("ASC")) { sortChar = " " + upChar; } if (tableSort.Contains("DESC")) { sortChar = " " + downChar; } }
        var header8 = new Label { Text = $"{Smap["Status"]}Status{sortChar}", FontAttributes = FontAttributes.Bold, TextColor = headerColor, FontSize = fSize };
        sortChar = ""; if (newTest == $"{Smap["Gen"]}Gen") { if (tableSort.Contains("ASC")) { sortChar = " " + upChar; } if (tableSort.Contains("DESC")) { sortChar = " " + downChar; } }
        var header9 = new Label { Text = $"{Smap["Gen"]}Gen{sortChar}", FontAttributes = FontAttributes.Bold, TextColor = headerColor, FontSize = fSize };
        sortChar = ""; if (newTest == $"{Smap["Papa"]}Papa") { if (tableSort.Contains("ASC")) { sortChar = " " + upChar; } if (tableSort.Contains("DESC")) { sortChar = " " + downChar; } }
        var header10 = new Label { Text = $"{Smap["Papa"]}Papa{sortChar}", FontAttributes = FontAttributes.Bold, TextColor = Shared.maleHeaderColor, FontSize = fSize };
        sortChar = ""; if (newTest == $"{Smap["Mama"]}Mama") { if (tableSort.Contains("ASC")) { sortChar = " " + upChar; } if (tableSort.Contains("DESC")) { sortChar = " " + downChar; } }
        var header11 = new Label { Text = $"{Smap["Mama"]}Mama{sortChar}", FontAttributes = FontAttributes.Bold, TextColor = Shared.femaleHeaderColor, FontSize = fSize };
        sortChar = ""; if (newTest == $"{Smap["Mutation"]}pM") { if (tableSort.Contains("ASC")) { sortChar = " " + upChar; } if (tableSort.Contains("DESC")) { sortChar = " " + downChar; } }
        var header12 = new Label { Text = $"{Smap["Mutation"]}pM{sortChar}", FontAttributes = FontAttributes.Bold, TextColor = Shared.maleHeaderColor, FontSize = fSize };
        sortChar = ""; if (newTest == $"{Smap["Mutation"]}mM") { if (tableSort.Contains("ASC")) { sortChar = " " + upChar; } if (tableSort.Contains("DESC")) { sortChar = " " + downChar; } }
        var header13 = new Label { Text = $"{Smap["Mutation"]}mM{sortChar}", FontAttributes = FontAttributes.Bold, TextColor = Shared.femaleHeaderColor, FontSize = fSize };
        sortChar = ""; if (newTest == $"{Smap["Imprint"]}Imprint") { if (tableSort.Contains("ASC")) { sortChar = " " + upChar; } if (tableSort.Contains("DESC")) { sortChar = " " + downChar; } }
        var header14 = new Label { Text = $"{Smap["Imprint"]}Imprint{sortChar}", FontAttributes = FontAttributes.Bold, TextColor = headerColor, FontSize = fSize };
        sortChar = ""; if (newTest == $"{Smap["Imprinter"]}Imprinter") { if (tableSort.Contains("ASC")) { sortChar = " " + upChar; } if (tableSort.Contains("DESC")) { sortChar = " " + downChar; } }
        var header15 = new Label { Text = $"{Smap["Imprinter"]}Imprinter{sortChar}", FontAttributes = FontAttributes.Bold, TextColor = headerColor, FontSize = fSize };


        if (title != "Bottom") // not sortable bottom row
        {
            SortColumn(nameH, title);
            SortColumn(levelH, title);
            //---------------------
            SortColumn(hpH, title);
            SortColumn(staminaH, title);
            if (hasO2) { SortColumn(O2H, title); }
            SortColumn(foodH, title);
            SortColumn(weightH, title);
            SortColumn(damageH, title);
            if (hasSpeed) { SortColumn(speedH, title); }
            //---------------------
            if (hasCraft) { SortColumn(craftH, title); }
            SortColumn(header8, title);
            SortColumn(header9, title);
            SortColumn(header10, title);
            SortColumn(header11, title);
            SortColumn(header12, title);
            SortColumn(header13, title);
            SortColumn(header14, title);
            SortColumn(header15, title);
        }

        int startID = 0;

        // Add base header row
        AddToGrid(grid, nameH, 0, startID++, title);
        AddToGrid(grid, levelH, 0, startID++, title);
        //---------------------
        AddToGrid(grid, hpH, 0, startID++, title);
        AddToGrid(grid, staminaH, 0, startID++, title);
        if (hasO2) { AddToGrid(grid, O2H, 0, startID++, title); }
        AddToGrid(grid, foodH, 0, startID++, title);
        AddToGrid(grid, weightH, 0, startID++, title);
        AddToGrid(grid, damageH, 0, startID++, title);
        if (hasCraft) { AddToGrid(grid, craftH, 0, startID++, title); }
        //---------------------
        if (hasSpeed) { AddToGrid(grid, speedH, 0, startID++, title); }
        AddToGrid(grid, header8, 0, startID++, title);
        AddToGrid(grid, header9, 0, startID++, title);
        AddToGrid(grid, header10, 0, startID++, title);
        AddToGrid(grid, header11, 0, startID++, title);

        // Add last column headers if conditions are met
        if (title != "Bottom")
        {
            AddToGrid(grid, header12, 0, startID++, title);
            AddToGrid(grid, header13, 0, startID++, title);
            AddToGrid(grid, header14, 0, startID++, title);
            AddToGrid(grid, header15, 0, startID++, title);
        }

        // add one xtra id for female header row
        if (title == "Female") { boxRowID++; }
        
        int rowIndex = 1; // Start adding rows below the header
        foreach (DataRow row in table.Rows)
        {
            boxRowID++;

            string id = row["ID"].ToString();
            string name = row["Name"].ToString();
            string level = row["Level"].ToString();
            //////////////
            string hp = row["Hp"].ToString();
            string stamina = row["Stamina"].ToString();
            string O2 = row["O2"].ToString();
            string food = row["Food"].ToString();
            string weight = row["Weight"].ToString();
            string damage = row["Damage"].ToString();
            string craft = row["Crafting"].ToString();
            //////////////
            string speed = row["Speed"].ToString();
            string status = row["Status"].ToString();
            string gen = row["Gen"].ToString();
            string papa = row["Papa"].ToString();
            string mama = row["Mama"].ToString();
            string papaM = row["PapaMute"].ToString();
            string mamaM = row["MamaMute"].ToString();
            string imprint = row["Imprint"].ToString();
            string imprinter = row["Imprinter"].ToString();
            string mutes = row["Mutes"].ToString();



            if (name == "") { name = "Name me"; }

            var nameC = DefaultColor;
            var levelC = DefaultColor;
            ////////////
            var hpC = DefaultColor;
            var staminaC = DefaultColor;
            var O2C = DefaultColor;
            var foodC = DefaultColor;
            var weightC = DefaultColor;
            var damageC = DefaultColor;
            var craftC = DefaultColor;
            ////////////
            var speedC = DefaultColor;
            var defaultC = DefaultColor;


            //recolor breeding stats
            if (DataManager.ToDouble(hp) + statOffset >= DataManager.HpMax) { hpC = Shared.goodColor; }
            if (DataManager.ToDouble(stamina) + statOffset >= DataManager.StaminaMax) { staminaC = Shared.goodColor; }
            if (DataManager.ToDouble(O2) + statOffset >= DataManager.O2Max) { O2C = Shared.goodColor; }
            if (DataManager.ToDouble(food) + statOffset >= DataManager.FoodMax) { foodC = Shared.goodColor; }
            if (DataManager.ToDouble(weight) + statOffset >= DataManager.WeightMax) { weightC = Shared.goodColor; }
            if (DataManager.ToDouble(damage) + statOffset >= DataManager.DamageMax) { damageC = Shared.goodColor; }
            if (DataManager.ToDouble(craft) + statOffset >= DataManager.CraftMax) { craftC = Shared.goodColor; }




            // mutation detection overrides normal coloring -> mutaColor
            if (mutes.Length >= 7 && !CurrentStats) // dont show mutations on current statview
            {
                string aC = mutes.Substring(0, 1); string bC = mutes.Substring(1, 1); string cC = mutes.Substring(2, 1);
                string dC = mutes.Substring(3, 1); string eC = mutes.Substring(4, 1); string fC = mutes.Substring(5, 1);
                string gC = mutes.Substring(6, 1);

                if (aC == "1" && ToDouble(hp) + statOffset >= HpMax) { hpC = mutaColor; } else if (aC == "1" && ToDouble(hp) - statOffset < HpMax) { hpC = mutaBadColor; }
                if (bC == "1" && ToDouble(stamina) + statOffset >= StaminaMax) { staminaC = mutaColor; } else if (bC == "1" && ToDouble(stamina) - statOffset < StaminaMax) { staminaC = mutaBadColor; }
                if (cC == "1" && ToDouble(O2) + statOffset >= O2Max) { O2C = mutaColor; } else if (cC == "1" && ToDouble(O2) - statOffset < O2Max) { O2C = mutaBadColor; }
                if (dC == "1" && ToDouble(food) + statOffset >= FoodMax) { foodC = mutaColor; } else if (dC == "1" && ToDouble(food) - statOffset < FoodMax) { foodC = mutaBadColor; }
                if (eC == "1" && ToDouble(weight) + statOffset >= WeightMax) { weightC = mutaColor; } else if (eC == "1" && ToDouble(weight) - statOffset < WeightMax) { weightC = mutaBadColor; }
                if (fC == "1" && ToDouble(damage) + statOffset >= DamageMax) { damageC = mutaColor; } else if (fC == "1" && ToDouble(damage) - statOffset < DamageMax) { damageC = mutaBadColor; }
                if (gC == "1" && ToDouble(craft) + statOffset >= CraftMax) { craftC = mutaColor; } else if (gC == "1" && ToDouble(craft) - statOffset < CraftMax) { craftC = mutaBadColor; }

            }

            // override offspring colors based on breed points
            if (title == "Bottom")
            {
                string IDC = row["Res"].ToString(); // this column only exist in bottom table
                string aC = IDC.Substring(0, 1); string bC = IDC.Substring(1, 1); string cC = IDC.Substring(2, 1);
                string dC = IDC.Substring(3, 1); string eC = IDC.Substring(4, 1); string fC = IDC.Substring(5, 1);
                string gC = IDC.Substring(6, 1);

                if (aC == "2") { hpC = Shared.bestColor; }
                if (bC == "2") { staminaC = Shared.bestColor; }
                if (cC == "2") { O2C = Shared.bestColor; }
                if (dC == "2") { foodC = Shared.bestColor; }
                if (eC == "2") { weightC = Shared.bestColor; }
                if (fC == "2") { damageC = Shared.bestColor; }
                if (gC == "2") { craftC = Shared.bestColor; }

                if (!hasO2) { cC = "2"; }
                if (!hasCraft) { gC = "2"; }
                if ((aC + bC + cC + dC + eC + fC + gC) == "2222222")
                {
                    // here is a golden offspring with all the best stats
                    hpC = Shared.goldColor;
                    staminaC = Shared.goldColor;
                    O2C = Shared.goldColor;
                    foodC = Shared.goldColor;
                    weightC = Shared.goldColor;
                    damageC = Shared.goldColor;
                    craftC = Shared.goldColor;
                }
            }

            if (title != "Bottom")
            {
                // Add notes symbol if notes are set
                string notes = DataManager.GetNotes(id);
                if (notes != "") { status += Smap["Notes"]; }


                // replace placeholders with symbols
                status = status.Replace("#", $"{Smap["Identical"]}");
                status = status.Replace("<", $"{Smap["LessThan"]}");
            }

            bool hasMama = false; bool hasPapa = false;
            // if not missing and not unknown = something with a name
            if (!mama.Contains(Smap["Warning"]) && !mama.Contains(Smap["Unknown"])) { hasMama = true; }
            if (!papa.Contains(Smap["Warning"]) && !papa.Contains(Smap["Unknown"])) { hasPapa = true; }

            // check for stats that someone with a parent should or should not have
            if (hasMama || hasPapa)
            {
                bool warn = false;
                // if we have a parent generation cant be 0
                if (ToDouble(gen) < 1) { warn = true; }
                // also if we have a mutation generation cant be 0
                if (ToDouble(mamaM) > 0 && ToDouble(gen) < 1) { warn = true; }
                if (ToDouble(papaM) > 0 && ToDouble(gen) < 1) { warn = true; }
                if (warn) { gen = Smap["Warning"]; }
            }


            // mark all generation dependant data as invalid
            if (mama == Shared.Smap["Warning"] && papa == Shared.Smap["Warning"])
            {
                papaM = Shared.Smap["Warning"];
                mamaM = Shared.Smap["Warning"];
                gen = Shared.Smap["Warning"];
            }


            // replace empty imprinter string with tribe
            if (mama == "" && papa == "" && imprinter == "")
            {
                // get the tamer string instead of imprinter
                imprinter = DataManager.GetFirstColumnData("ID", id, "Tribe");
            }


            // Create a Labels
            var nameL = new Label { Text = name, TextColor = nameC };
            var levelL = new Label { Text = level, TextColor = levelC };
            //////////////
            var hpL = new Label { Text = hp, TextColor = hpC };
            var staminaL = new Label { Text = stamina, TextColor = staminaC };
            var O2L = new Label { Text = O2, TextColor = O2C };
            var foodL = new Label { Text = food, TextColor = foodC };
            var weightL = new Label { Text = weight, TextColor = weightC };
            var damageL = new Label { Text = damage, TextColor = damageC };
            var craftL = new Label { Text = craft, TextColor = craftC };
            //////////////
            var speedL = new Label { Text = speed, TextColor = speedC };
            var statusL = new Label { Text = status, TextColor = defaultC };
            var genL = new Label { Text = gen, TextColor = defaultC };
            var papaL = new Label { Text = papa, TextColor = Shared.maleColor };
            var mamaL = new Label { Text = mama, TextColor = Shared.femaleColor };
            var papaML = new Label { Text = papaM, TextColor = Shared.maleColor };
            var mamaML = new Label { Text = mamaM, TextColor = Shared.femaleColor };
            var imprintL = new Label { Text = imprint, TextColor = defaultC };
            var imprinterL = new Label { Text = imprinter, TextColor = defaultC };

            bool selected = false;
            if (title != "Bottom") // dont make bottom panel selectable
            {
                // figure out if we have this dino selected for row coloring purposes
                if (id == selectedID) { selected = true; }

                // Attach TapGesture to all labels
                SelectDino(nameL, id, boxRowID);
                SelectDino(levelL, id, boxRowID);
                //------------------------------------------
                SelectDino(hpL, id, boxRowID);
                SelectDino(staminaL, id, boxRowID);
                if (hasO2) { SelectDino(O2L, id, boxRowID); }
                SelectDino(foodL, id, boxRowID);
                SelectDino(weightL, id, boxRowID);
                SelectDino(damageL, id, boxRowID);
                if (hasCraft) { SelectDino(craftL, id, boxRowID); }
                //------------------------------------------
                if (hasSpeed) { SelectDino(speedL, id, boxRowID); }
                SelectDino(statusL, id, boxRowID);
                SelectDino(genL, id, boxRowID);
                SelectDino(papaL, id, boxRowID);
                SelectDino(mamaL, id, boxRowID);
                SelectDino(papaML, id, boxRowID);
                SelectDino(mamaML, id, boxRowID);
                SelectDino(imprintL, id, boxRowID);
                SelectDino(imprinterL, id, boxRowID);
            }

            // Reset startID for new row
            startID = 0;

            // Add base items to the grid
            AddToGrid(grid, nameL, rowIndex, startID++, title, selected, false, id);
            AddToGrid(grid, levelL, rowIndex, startID++, title, selected, false, id);
            //-------------------
            AddToGrid(grid, hpL, rowIndex, startID++, title, selected, false, id);
            AddToGrid(grid, staminaL, rowIndex, startID++, title, selected, false, id);
            if (hasO2) { AddToGrid(grid, O2L, rowIndex, startID++, title, selected, false, id); }
            AddToGrid(grid, foodL, rowIndex, startID++, title, selected, false, id);
            AddToGrid(grid, weightL, rowIndex, startID++, title, selected, false, id);
            AddToGrid(grid, damageL, rowIndex, startID++, title, selected, false, id);
            if (hasCraft) { AddToGrid(grid, craftL, rowIndex, startID++, title, selected, false, id); }
            //-------------------
            if (hasSpeed) { AddToGrid(grid, speedL, rowIndex, startID++, title, selected, false, id); }
            AddToGrid(grid, statusL, rowIndex, startID++, title, selected, false, id);
            AddToGrid(grid, genL, rowIndex, startID++, title, selected, false, id);
            AddToGrid(grid, papaL, rowIndex, startID++, title, selected, false, id);
            AddToGrid(grid, mamaL, rowIndex, startID++, title, selected, false, id);
            AddToGrid(grid, papaML, rowIndex, startID++, title, selected, false, id);
            AddToGrid(grid, mamaML, rowIndex, startID++, title, selected, false, id);
            AddToGrid(grid, imprintL, rowIndex, startID++, title, selected, false, id);
            AddToGrid(grid, imprinterL, rowIndex, startID++, title, selected, false, id);

            rowIndex++;
        }

        return grid;
    }

    private void ClearSelection()
    {
        if (selectedID != "" && !isDouble)
        {
            //  FileManager.Log($"Unselected {selectedID}", 0);
            selectedID = ""; isSelected = false; this.Title = $"{Shared.setPage.Replace("_", " ")}";
            canDouble = false; editStats = false;

            ExcludeBtn.IsVisible = false;
            ArchiveBtn.IsVisible = false;

            // recolor all rows to default
            DefaultRowColors();
        }
    }

    private bool IsValidDouble(string input)
    {
        editStats = true;
        return double.TryParse(input, out _); // Returns true if the input is a valid double
    }

    private void DefaultRowColors()
    {
        if (Monitor.TryEnter(Shared._dbLock, TimeSpan.FromSeconds(5)))
        {
            try
            {
                if (boxViews.Count > 0)
                {
                    int rowsM = DataManager.MaleTable.Rows.Count;
                    int rowsT = boxViews.Count;
                    int z = 0;

                    for (int i = 0; i < rowsT; i++) // color all male rows
                    {
                        // start coloring the rows with Solid color
                        if (i <= rowsM)
                        {
                            boxViews[i].Color = i % 2 == 0 ? OddMPanelColor : MainPanelColor;
                        }
                        else // use z instead of i to reset odd & even at new table
                        {
                            boxViews[i].Color = z % 2 == 0 ? OddMPanelColor : MainPanelColor;
                            z++;
                        }
                    }
                }
            }
            catch { }
            finally
            {
                Monitor.Exit(Shared._dbLock);
            }
        }
        else
        {
            FileManager.Log("Recoloring failure", 1);
        }
    }

    private void ButtonGroup()
    {
        string group = DataManager.GetGroup(selectedID);
        if (group == "Exclude") { ExcludeBtn.Text = "Include"; ExcludeBtn.BackgroundColor = Shared.PrimaryColor; }
        else { ExcludeBtn.Text = "Exclude"; ExcludeBtn.BackgroundColor = Shared.SecondaryColor; }

        if (group == "Archived") { ArchiveBtn.Text = "Include"; ArchiveBtn.BackgroundColor = Shared.PrimaryColor; }
        else { ArchiveBtn.Text = "Archive"; ArchiveBtn.BackgroundColor = Shared.TrinaryColor; }

        ExcludeBtn.IsVisible = true;
        ArchiveBtn.IsVisible = true;
    }


    // Button event handlers
    void SortColumn(Label label, string sex)
    {
        label.GestureRecognizers.Clear();
        // Create a TapGestureRecognizer
        var tapGesture1 = new TapGestureRecognizer();
        tapGesture1.Tapped += (s, e) =>
        {
            // Handle the click event and pass additional data
            string column = label.Text;

            if (column.Contains(Smap["SortUp"]) || column.Contains(Smap["SortDown"]))
            {
                column = column.Substring(0, column.Length - 2);
            }


            var splitM = sortM.Split(new[] { @" " }, StringSplitOptions.RemoveEmptyEntries);
            var splitF = sortF.Split(new[] { @" " }, StringSplitOptions.RemoveEmptyEntries);

            string outM = "";
            string outF = "";

            if (splitM.Length > 0)
            {
                outM = splitM[0];
            }
            if (splitF.Length > 0)
            {
                outF = splitF[0];
            }

            if (sex == "Male")
            {
                // are we clicking the same column then toggle sorting
                if (outM == column)
                {
                    if (sortM.Contains("ASC"))
                    {
                        sortM = column + " DESC";
                    }
                    else if (sortM.Contains("DESC"))
                    {
                        sortM = "";
                    }
                }
                else
                {
                    sortM = column + " ASC";
                }
            }
            else if (sex == "Female")
            {
                // are we clicking the same column then toggle sorting
                if (outF == column)
                {
                    if (sortF.Contains("ASC")) // then switch to descending
                    {
                        sortF = column + " DESC";
                    }
                    else if (sortF.Contains("DESC")) // finally turn it off
                    {
                        sortF = "";
                    }
                }
                else // first sort ascending
                {
                    sortF = column + " ASC";
                }
            }

            FileManager.Log($"Sorted: {sortM} : {sortF}", 0);

            dataValid = false;
            ClearSelection();
            CreateContent();
        };

        // Attach the TapGestureRecognizer to the label
        label.GestureRecognizers.Add(tapGesture1);
    }

    void SelectDino(Label label, string id, int boxid = 0)
    {
        label.GestureRecognizers.Clear();
        // Create a TapGestureRecognizer
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += (s, e) =>
        {
            if (selectedID != id) // select a new dino
            {
                selectedID = id; isSelected = true;

                // recolor all rows to default
                DefaultRowColors();

                boxViews[boxid].Color = SelectedColor;

                // make buttons visible
                ButtonGroup();

                // set title to dino name
                this.Title = $"{DataManager.GetLastColumnData("ID", selectedID, "Name")} - {selectedID}";

                // activate double clicking
                canDouble = true;
                DisableDoubleClick();
            }
            else if (selectedID == id && canDouble) // select same dino within time
            {
                // double click  // open the dino extended info window
                isDouble = true; canDouble = false;
                CreateContent();
            }
            else if (selectedID == id && !canDouble) // select same dino over time
            {
                // re activate double clicking
                canDouble = true;
                DisableDoubleClick();
            }
        };

        // Attach the TapGestureRecognizer to the label
        label.GestureRecognizers.Add(tapGesture);
    }

    void SelectBG(BoxView inp, string id)
    {
        inp.GestureRecognizers.Clear();
        // Create a TapGestureRecognizer
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += (s, e) =>
        {
            if (selectedID != id) // select a new dino
            {
                selectedID = id; isSelected = true;

                // recolor all rows to default
                DefaultRowColors();

                inp.Color = SelectedColor;

                // make buttons visible
                ButtonGroup();

                // set title to dino name
                this.Title = $"{DataManager.GetLastColumnData("ID", selectedID, "Name")} - {selectedID}";

                // activate double clicking
                canDouble = true;
                DisableDoubleClick();
            }
            else if (selectedID == id && canDouble) // select same dino within time
            {
                // double click  // open the dino extended info window
                isDouble = true; canDouble = false;
                CreateContent();
            }
            else if (selectedID == id && !canDouble) // select same dino over time
            {
                // re activate double clicking
                canDouble = true;
                DisableDoubleClick();
            }
        };

        // Attach the TapGestureRecognizer to the label
        inp.GestureRecognizers.Add(tapGesture);
    }

    private async Task DisableDoubleClick()
    {
        await Task.Delay(Shared.doubleClick);
        canDouble = false;
    }

    void UnSelectDino(Grid grid)
    {
        grid.GestureRecognizers.Clear();
        // Create a TapGestureRecognizer
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += (s, e) =>
        {
            ClearSelection();
            //CreateContent();
        };

        // Attach the TapGestureRecognizer to the label
        grid.GestureRecognizers.Add(tapGesture);
    }

    private void ToggleBtnClicked(object? sender, EventArgs e)
    {
        ToggleExcluded++;
        if (ToggleExcluded == 3)
        {
            ToggleExcluded = 0;
        }
        dataValid = false;
        FileManager.Log($"Toggle Exclude {ToggleExcluded}", 0);

        ClearSelection();
        CreateContent();
    }

    private void StatsBtnClicked(object? sender, EventArgs e)
    {
        if (CurrentStats)
        {
            CurrentStats = false;
        }
        else
        {
            CurrentStats = true;
        }


        FileManager.Log($"Toggle Stats {CurrentStats}", 0);

        dataValid = false;
        ClearSelection();
        CreateContent();
    }

    private void ExcludeBtnClicked(object? sender, EventArgs e)
    {
        if (selectedID != "")
        {
            string group = DataManager.GetGroup(selectedID);
            if (group == "Exclude") { group = ""; }
            else if (group == "") { group = "Exclude"; FileManager.Log($"Excluded ID: {selectedID}", 0); }
            DataManager.SetGroup(selectedID, group);

            dataValid = false;
            ClearSelection();
            CreateContent();
        }
    }

    private void ArchiveBtnClicked(object? sender, EventArgs e)
    {
        if (selectedID != "")
        {
            // Handle the click event
            string status = DataManager.GetGroup(selectedID);
            if (status == "Archived") { status = ""; FileManager.Log($"Restored ID: {selectedID}", 0); }
            else if (status == "") { status = "Archived"; FileManager.Log($"Archived ID: {selectedID}", 0); }
            else if (status == "Exclude") { status = "Archived"; FileManager.Log($"Archived ID: {selectedID}", 0); }
            DataManager.SetGroup(selectedID, status);

            dataValid = false;
            ClearSelection();
            CreateContent();
        }

    }

    private void BackBtnClicked(object? sender, EventArgs e)
    {
        // reset toggles etc.
        levelText = ""; hpText = ""; staminaText = ""; O2Text = "";
        foodText = ""; weightText = ""; damageText = ""; notesText = "";
        speedText = ""; craftText = ""; dataValid = false;
        isDouble = false; showTree = false;
        ClearSelection();
        CreateContent();
    }

    private void SaveBtnClicked(object? sender, EventArgs e)
    {
        // save data here

        // find the breed stats of
        // selectedID
        // and edit them

        if (editStats)
        {
            DataManager.EditBreedStats(selectedID, levelText, hpText, staminaText, O2Text, foodText, weightText, damageText, notesText, speedText, craftText);
            FileManager.needSave = true;
            dataValid = false;
        }

        // reset toggles etc.
        levelText = ""; hpText = ""; staminaText = ""; O2Text = "";
        foodText = ""; weightText = ""; damageText = ""; notesText = "";
        speedText = ""; craftText = "";
        isDouble = false;

        ClearSelection();
        CreateContent();
    }

    private void TreeBtnClicked(object? sender, EventArgs e)
    {
        showTree = true;

        dataValid = false;
        ClearSelection();
        CreateContent();

    }


}