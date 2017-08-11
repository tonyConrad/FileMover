using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data;
using System.Xaml;



/*To Dos:
    
    v.1
   Error handling
        if user closes program during middle of file move
    search / find combinations working
    backup folders working
    make executable
    show log of file moves in program
    put on GitHub

    v. 2
    save session
    after both folders selected, then can press button to start moving
    Check box so don't have to authorize each move
    Multiple windows - popouts etc
    beautification
    button to choose which folder(s) are not allowed to move from/to
    show previous move file logs
    undo button
    allow inputs of words/phrases of folders that can't be moved to/from - or have selection box


*/

namespace FileMover
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 


    public partial class MainWindow : Window
    {
 


        public MainWindow()
        {
            InitializeComponent();
            //moveFiles_Button.IsEnabled = false;

            //hide UI related to the file moving process 




            /*doesn't work
            if (!string.IsNullOrEmpty(originFolder) && !string.IsNullOrEmpty(destinationFolder)) ;
            {
                moveFiles_Button.IsEnabled = true;
                test_TextBox.Text = "they are not empty";
            } 
            */

            //set fields to read only
            folderOrigin_TextBox.IsReadOnly = true;
            destinationFolder_TextBox.IsReadOnly = true;
            filePath_TextBox.IsReadOnly = true;
            fileName_TextBox.IsReadOnly = true;





        }




        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = System.IO.Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = System.IO.Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        ////add keybinding to allow 'enter' to move file
        //private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        //{
        //    if (e.Key == Key.Return)
        //    {

        //        System.Windows.MessageBox.Show("Hi");
        //        //folderWithFiles_Button_Click(sender, e);
        //        e.Handled = true;
        //    }
        //}

        //make variables global

        string originFolder { get; set; } //makes originFolder global
        
        string destinationFolder { get; set; } //makes global
        string backupFolder { get; set; } //later get from .txt file 
        string[] sourceFilesAndDirs { get; set; }
        string[][] toMoveArray { get; set; }
        string[][] searchOneArray { get; set; }
        string[][] searchTwoArray { get; set; }
        string[][] searchThreeArray { get; set; }
        string[][] searchFourArray { get; set; }
        string[][] combinedSearchArrays { get; set; }
        int ArrOneLength { get; set; }
        int ArrTwoLength { get; set; }
        int ArrThreeLength { get; set; }
        int ArrFourLength { get; set; }
        string curDateTime { get; set; }

        string logFilesPath = @"C:\Users\tferrin\Dropbox (Fullscreen)\Vendor Files\.FileMover move Logs\";
//        string curDateTime = DateTime.Now.ToString("yyyyMMdd - h mm tt  ss" + "Sec");
        string curDate = DateTime.Now.ToString("yyyyMMdd");
        string fileMovementTracker { get; set; }

        int sourceFilesAndDirsCount { get; set; }
        int currentArrayIndex { get; set; }































        //BUTTON---for testing --display List item
        public void GetFilename_Button_Click(object sender, RoutedEventArgs e)
        {
            /*
            ////ARRAY join testing --- works
            //int[] x = new int[] { 1, 2, 3 };
            //int[] y = new int[] { 4, 5 };
            //var z = new int[x.Length + y.Length];

            //x.CopyTo(z, 0);
            //y.CopyTo(z, x.Length);
            //System.Windows.MessageBox.Show(z[2].ToString());
            */


            string[][] arrayOne = new string[5][];
            for (int xx = 0; xx < 5; xx++)
            {
                arrayOne[xx] = new string[2] { xx.ToString() + "   ***Logic 1", xx.ToString() + "dir path" };
            }

            //System.Windows.MessageBox.Show(arrayOne[2][0].ToString() + ", " + arrayOne[2][1].ToString());

            string[][] arrayTwo = new string[5][];
            for (int xx = 0; xx < 5; xx++)
            {
                arrayTwo[xx] = new string[2] { xx.ToString() + "   ***Logic 2", xx.ToString() + "dir path" };
            }

            System.Windows.MessageBox.Show(arrayTwo[2][0].ToString() + ", " + arrayTwo[2][1].ToString());

            string[][] arrayThree = new string[5][];
            for (int xx = 0; xx < 5; xx++)
            {
                arrayThree[xx] = new string[2] { xx.ToString() + "   ***Logic 3", xx.ToString() + "dir path" };
            }

            System.Windows.MessageBox.Show(arrayThree[2][0].ToString() + ", " + arrayThree[2][1].ToString());


            string[][] combinedArrays = arrayOne.Concat(arrayTwo).ToArray();
            combinedArrays = combinedArrays.Concat(arrayThree).ToArray();
            System.Windows.MessageBox.Show(combinedArrays[14][0].ToString() + ", " + combinedArrays[14][1].ToString());
        }

        //FORTESTING --- This button tells the file path of the selected item
        public void SecondWord_Click(object sender, RoutedEventArgs e)
        {
            //String selectedFilePath = moveTo_dataGrid.SelectedItem.ToString(); //this displays the value of both columns with unusable characters too
            //String selectedFilePath = moveTo_dataGrid.SelectedItem.ToString();

            //DataRowView dataRow = (DataRowView)moveTo_dataGrid.SelectedItem;
            //int index = moveTo_dataGrid.CurrentCell.Column.DisplayIndex;
            //string cellValue = dataRow.Row.ItemArray[index].ToString();

            //get the value of the selected row's second column 
            object item = moveTo_dataGrid.SelectedItem;
            string selectedFolderPath = (moveTo_dataGrid.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;
            System.Windows.MessageBox.Show(selectedFolderPath);
        }



  

        //BUTTON---After this button is clicked a folder selection box pops up and the user selects the folder that contains the files to be moved
        public void folderWithFiles_Button_Click(object sender, RoutedEventArgs e)
        {
            //clear all List Items
            folderOrigin_TextBox.Text = "";

            //select folder that has the files
            var sourceDialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = sourceDialog.ShowDialog();

            //check to make sure off limits folder isn't chosen as source file
            string offLimitsFolder = "Vendor Files";
            int offLimitsSearch = 0;
            offLimitsSearch = System.IO.Path.GetFileName(sourceDialog.SelectedPath).IndexOf(offLimitsFolder);

            //make sure folder chosen and offlimits folder not chosen
            if (!string.IsNullOrEmpty(sourceDialog.SelectedPath))
            {
                originFolder = sourceDialog.SelectedPath; //global variable - get/set
                folderOrigin_TextBox.Text = originFolder;

                //after this button is clicked all folders or files are grouped and displayed
                sourceFilesAndDirs = Directory.GetFileSystemEntries(originFolder, "*"); //define array
                currentArrayIndex = 0; //set the array index to 0 so the first file to be moved is the 1st
                sourceFilesAndDirsCount = Directory.GetFileSystemEntries(originFolder, "*").Length;

                //if (sourceFilesAndDirsCount > 0)
                //{
                //    // Loop through and add all folders to the ListBox.
                //    for (int x = 0; x < sourceFilesAndDirsCount; x++)
                //    {
                //        //originFolderFiles_ListBox.Items.Add(System.IO.Path.GetFileName(sourceFilesAndDirs[x]).ToString());
                //        //FooBar.Items.Add(System.IO.Path.GetFileName(sourceFilesAndDirs[x]).ToString());
                //        //dataGridView1.ItemsSource = sourceFilesAndDirs;
                //    }
                //}

                //List<string> sourceFilesAndDirs_List = sourceFilesAndDirs.ToList();
                //DataContext = this;


                if (offLimitsSearch != -1)
                {
                    originFolder = ""; //reset origin folder
                    folderOrigin_TextBox.Text = ""; //and reset textbox
                    System.Windows.MessageBox.Show("Files & Folders can't be moved FROM the selected folder, because it contains the phrase:  " + offLimitsFolder + ".  Please select another.");

                }
            }
            else
            {
            }

            //System.Windows.MessageBox.Show(toMoveArray[0][1]);
            //toMoveArray[0][1] = "";  // how to remove from array
        }




        //BUTTON---After this button is clicked a folder selection box pops up and the user selects the parent folder they want their invoices moved to
        public void MoveToFolder_Button_Click(object sender, RoutedEventArgs e) //working
        {
            //clear all List Items
            destinationFolder_TextBox.Text = "";
            
            //select folder you want files moved to
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (!string.IsNullOrEmpty(dialog.SelectedPath))
            {
                destinationFolder = dialog.SelectedPath; //global through get/set
                destinationFolder_TextBox.Text = destinationFolder;

                //make an array of all folders in selected directory and count how many there are
                var destDirResults = Directory.GetDirectories(destinationFolder, "*");
                int destDirResultsCount = Directory.GetDirectories(destinationFolder, "*").Length;


                /*COMMENT OUT UNTIL CLOSER TO PROD
                //activate move files button after both directories selected and only then
                if (originFolder.Length != 0 && destinationFolder.Length != 0)
                {
                    moveFiles_Button.IsEnabled = true;
                }
                */


                string[][] moveToFolderArray = new string[destDirResultsCount][];
                for (int kk = 0; kk < destDirResultsCount; kk++)
                {
                    moveToFolderArray[kk] = new string[2] { System.IO.Path.GetFileName(destDirResults[kk]).ToString(), destDirResults[kk].ToString() };
                }
                var data = (from arr in moveToFolderArray select new { FolderName = arr[0], FolderPath = arr[1] }); 
                moveTo_dataGrid.ItemsSource = data.ToList();
            }
            else
            {
            }

        }





        //BUTTON -- clicked to start the program after the source and destination directories are selected
        public void moveFiles_Click(object sender, RoutedEventArgs e)
        {
            fileName_TextBox.Text = System.IO.Path.GetFileName(sourceFilesAndDirs[currentArrayIndex]).ToString();
            filePath_TextBox.Text = sourceFilesAndDirs[currentArrayIndex].ToString();

            search_Button_Click(e, e);
        }





        //BUTTON---click on this button to move file to selected folder - will create backup, move, and log the move
        public void MoveFile_Button_Click(object sender, RoutedEventArgs e)
        {
            if (moveTo_dataGrid.SelectedCells.Count > 0)
            {
                curDateTime = DateTime.Now.ToString("yyyyMMdd - h mm tt  ss" + "Sec");
                //get the value of the selected row's second column  (where user wants file/folder moved to)
                object item = moveTo_dataGrid.SelectedItem;
                string selectedFolderPath = (moveTo_dataGrid.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;

                //move the file in the current array index to the selected 
                string origPath = filePath_TextBox.Text; //full path needed 
                string destPath = System.IO.Path.Combine(selectedFolderPath,fileName_TextBox.Text);
                //System.Windows.MessageBox.Show("Length of Destination Path :  " + destPath.Length);
                string backupPath = System.IO.Path.Combine(@"C:\Users\tferrin\Dropbox (Fullscreen)\Vendor Files\.FileMover backup files\", fileName_TextBox.Text); //easy - full path will be static - add folder YYYYMMDD for each move
                //System.Windows.MessageBox.Show("Length of Destination Path :  " + backupPath.Length);

                /* if file then use this */
                //make sure file is in origin directory
                if (File.Exists(origPath))
                {
                    //copy the file and move to the backup directory
                    File.Copy(origPath, backupPath, true);

                    //if file successfully backed up then move it
                    if (File.Exists(backupPath))
                    {
                        //move original to destination directory
                        File.Move(origPath, destPath);
                    }

                    //check to make sure that file successfully moved, then record in log and remove from array
                    if (File.Exists(destPath))
                    {
                        //record transfer to log text file - one created per day (if file is confirmed to have been moved)
                        fileMovementTracker = curDateTime + ", " + filePath_TextBox.Text + " > Moved to:  " + destPath;
                        System.IO.File.AppendAllText(logFilesPath + curDate + ".txt", Environment.NewLine + fileMovementTracker);
                    
                        //remove from Array
                        sourceFilesAndDirs[currentArrayIndex] = "";

                        //go to next item that needs to be moved
                        NextToMove();

                        //after move run a search on the next item
                        search_Button_Click(e, e);
                    }
                }


                /* if directory then use this */
                if (Directory.Exists(origPath))
                {
                    
                    //copy the folders and contents into the backup folder (just in case)
                    try
                    {
                        DirectoryCopy(origPath, backupPath, true);
                    }
                    catch (Exception Ex)
                    {
                        System.Windows.MessageBox.Show("{0} exception caught.", Ex.ToString());
                    }



                //if the folder was backed up successfully, then move the folder
                if (Directory.Exists(backupPath))
                    {
                        try
                        {
                            //move the folder
                            Directory.Move(origPath, destPath);
                        }
                        catch (Exception Ex)
                        {
                            System.Windows.MessageBox.Show("{0} exception caught.", Ex.ToString());
                        }

                    }


                    //if folder moved successfully, then log, remove from array, go to next item, etc
                    if (Directory.Exists(destPath))
                    {
                        fileMovementTracker = curDateTime + ", " + filePath_TextBox.Text + " > Moved to:  " + destPath;
                        System.IO.File.AppendAllText(logFilesPath + curDate + ".txt", Environment.NewLine + fileMovementTracker);

                        //remove from Array
                        sourceFilesAndDirs[currentArrayIndex] = "";

                        //go to next item that needs to be moved
                        NextToMove();

                        //after move run a search on the next item
                        search_Button_Click(e, e);
                    }
                }

            }
        }




        //BUTTON---if this button is clicked then the file or folder is not moved and it is left in the origination folder untouched
        public void SkipFile_Button_Click(object sender, RoutedEventArgs e)
        {
            //go to next item in list
            NextToMove();
            //run search for new file that was just moved to
            search_Button_Click(e, e);
        }




        //BUTTON---Click on this button to cancel moving the current file and the whole process
        public void StopMoving_Button_Click(object sender, RoutedEventArgs e)
        {
            fileName_TextBox.Text = "";
            filePath_TextBox.Text = "";
            folderOrigin_TextBox.Text = "";
            destinationFolder_TextBox.Text = "";
            originFolder = "";
            destinationFolder = "";
            createFolder_TextBox.Text = "";
            moveTo_dataGrid.Columns.Clear();
        }




        //BUTTON --Click shows all folders in folder that was selected to move to
        private void ShowAllFiles_Button_Click(object sender, RoutedEventArgs e)
        {
            //add new folder to available folders to move file/folder to
            var destDirResults = Directory.GetDirectories(destinationFolder, "*");
            int destDirResultsCount = Directory.GetDirectories(destinationFolder, "*").Length;
            string[][] moveToFolderArray = new string[destDirResultsCount][];
            for (int kk = 0; kk < destDirResultsCount; kk++)
            {
                moveToFolderArray[kk] = new string[2] { System.IO.Path.GetFileName(destDirResults[kk]).ToString(), destDirResults[kk].ToString() };
            }
            var data = (from arr in moveToFolderArray select new { FolderName = arr[0], FolderPath = arr[1] });
            moveTo_dataGrid.ItemsSource = data.ToList();
        }




        //BUTTON - create folder to add file to
        private void createFolderConfirm_Button_Click(object sender, RoutedEventArgs e)
        {

            String newFolder = createFolder_TextBox.Text;
            String newFolderPath = System.IO.Path.Combine(destinationFolder, newFolder);
        
            if (string.IsNullOrEmpty(newFolder))
            {
                System.Windows.MessageBox.Show("Nothing Entered");
            }

            if (!string.IsNullOrEmpty(newFolder))
            {
                try
                {
                    if (Directory.Exists(newFolderPath))
                    {
                        System.Windows.MessageBox.Show("That folder exists already.");
                        return;
                    }

                    Directory.CreateDirectory(newFolderPath);

                    //reset new folder variable and field for next time
                    createFolder_TextBox.Text = "";
                    newFolder = "";

                    //add new folder to available folders to move file/folder to
                    var destDirResults = Directory.GetDirectories(destinationFolder, "*");
                    int destDirResultsCount = Directory.GetDirectories(destinationFolder, "*").Length;
                    string[][] moveToFolderArray = new string[destDirResultsCount][];
                    for (int kk = 0; kk < destDirResultsCount; kk++)
                    {
                        moveToFolderArray[kk] = new string[2] { System.IO.Path.GetFileName(destDirResults[kk]).ToString(), destDirResults[kk].ToString() };
                    }
                    var data = (from arr in moveToFolderArray select new { FolderName = arr[0], FolderPath = arr[1] });
                    moveTo_dataGrid.ItemsSource = data.ToList();

                    //run search for whatever is the current file
                    search_Button_Click(e, e);


                }
                catch (Exception Ex)
                {
                    System.Windows.MessageBox.Show("{0} exception caught.", Ex.ToString());
                }
            }



        }




        //BUTTON - SEARCH LOGIC - for now is a button - used as method elsewhere
        public void search_Button_Click(object sender, RoutedEventArgs e)
        {


            searchOneArray = new string[0][];
            searchTwoArray = new string[0][];
            searchThreeArray = new string[0][];
            combinedSearchArrays = new string[0][];
            //combinedSearchArrays[0] = new string[] { "0", "0" };


            //Regex to remove special characters from file name
            string FileNameTextBoxValue = System.IO.Path.GetFileNameWithoutExtension(fileName_TextBox.Text);  //toMoveArray[currentArrayIndex][0];
            string pattern = "[^a-zA-Z0-9 ]"; //find all characters except numbers and letters
            string replacement = " "; //replace with this
            Regex rgx = new Regex(pattern);
            string cleanedUpValue = rgx.Replace(FileNameTextBoxValue, replacement);

            //turn string into an array of words
            var searchWordsArray = cleanedUpValue.Split();
            //System.Windows.MessageBox.Show(searchWordsArray[0] + " - 0,  " + searchWordsArray[1] + " - 1");

            //SEARCH Logic 1 - find first 2 words of file in first 2 words of folder (1st 2ndword *)
            if (searchWordsArray.Length >= 2)
            {
                var searchDirsFirstTwoBeginning = Directory.GetDirectories(destinationFolder, searchWordsArray[0] + " " + searchWordsArray[1] + "*");
                int searchDirMatchesFirstTwoBeginning = Directory.GetDirectories(destinationFolder, searchWordsArray[0] + " " + searchWordsArray[1] + "*").Length;
                //if the first word of the file matches the first word of the folder, then it will display
                if (searchDirMatchesFirstTwoBeginning > 0)
                {

                    searchOneArray = new string[searchDirMatchesFirstTwoBeginning][];
                    for (int xx = 0; xx < searchDirMatchesFirstTwoBeginning; xx++)
                    {
                        searchOneArray[xx] = new string[2] { System.IO.Path.GetFileName(searchDirsFirstTwoBeginning[xx]).ToString() + "   ***Logic 1 (first two in first two*)", searchDirsFirstTwoBeginning[xx].ToString() };
                    }
                    ArrOneLength = searchOneArray.Length;
                }
            }

            //SEARCH Logic 2 - Find First word of file in First word of folder (1st word*)
            if (searchWordsArray.Length >= 1)
            {
                var searchDirsFirstFirst = Directory.GetDirectories(destinationFolder, searchWordsArray[0] + "*");
                int searchDirMatchesFirstFirst = Directory.GetDirectories(destinationFolder, searchWordsArray[0] + "*").Length;

                //if matches, then it will display
                if (searchDirMatchesFirstFirst > 0)
                {
                    searchTwoArray = new string[searchDirMatchesFirstFirst][];
                    for (int yy = 0; yy < searchDirMatchesFirstFirst; yy++)
                    {
                        searchTwoArray[yy] = new string[2] { System.IO.Path.GetFileName(searchDirsFirstFirst[yy]).ToString() + "   ***Logic 2 (1st word in 1st word*)", searchDirsFirstFirst[yy].ToString() };
                    }
                   // var firstWordBegdata = (from arr in searchTwoArray select new { FolderName = arr[0], FolderPath = arr[1] });
                    //moveTo_dataGrid.ItemsSource = firstWordBegdata.ToList();

                    ArrTwoLength = searchTwoArray.Length;
                    //System.Windows.MessageBox.Show(searchTwoArray[0][0]);
                }
            }

            //Search Logic 3 - Find 2nd word of file in first part of folder (2ndword*)
            if (searchWordsArray.Length >= 2)
            {
                var searchDirsSecondFirst = Directory.GetDirectories(destinationFolder, "*" + searchWordsArray[0] + "*" + searchWordsArray[1] + "*");
                int searchDirMatchesSecondFirst = Directory.GetDirectories(destinationFolder, "*" + searchWordsArray[0] + "*" + searchWordsArray[1] + "*").Length;
                //if matches, then it will display
                if (searchDirMatchesSecondFirst > 0)
                {
                    searchThreeArray = new string[searchDirMatchesSecondFirst][];
                    for (int b = 0; b < searchDirMatchesSecondFirst; b++)
                    {
                        searchThreeArray[b] = new string[2] { System.IO.Path.GetFileName(searchDirsSecondFirst[b]).ToString() + "   ***Logic 3 (first second as *first* second*)", searchDirsSecondFirst[b].ToString() };
                    }
                    ArrThreeLength = searchThreeArray.Length;
                   // System.Windows.MessageBox.Show(searchWordsArray[0] + searchWordsArray[1] + "*");
                }
            }

            //SEARCH logic 4 - Find first word anywhere in string (*1st word*)
            if (searchWordsArray.Length >= 2)
            {
                var searchDirsLogicFour = Directory.GetDirectories(destinationFolder, "*" + searchWordsArray[0] + "*");
                int searchDirMatchesLogicFour = Directory.GetDirectories(destinationFolder, "*" + searchWordsArray[0] + "*").Length;
                //if matches, then it will display
                if (searchDirMatchesLogicFour > 0)
                {
                    searchFourArray = new string[searchDirMatchesLogicFour][];
                    for (int w = 0; w < searchDirMatchesLogicFour; w++)
                    {
                        searchFourArray[w] = new string[2] { System.IO.Path.GetFileName(searchDirsLogicFour[w]).ToString() + "   ***Logic 4 (1st word search in *1stword*)", searchDirsLogicFour[w].ToString() };
                    }
                    ArrFourLength = searchFourArray.Length;
                }
            }


            //SEARCH logic 5 - Combine then find first 2 words anywhere in foldername (*firstsecond*)
            //SEARCH logic 6 - Use first 6 characters to search beginning of foldername (left(6)*)
            //SEARCH logic 7 - Use first word * second word from filename to find folder (*first*second*)
            //SEARCH logic 8 - Use last word from filename to find folder (last*)
            //SEARCH logic 9 - 12 - replace last letter (9), last 2 letters(10), 2nd letter(11), first letter(12) of first word with *
            //SEARCH logic 13 - 16 - do same for 2nd word

            int totalArrLength = ArrOneLength + ArrTwoLength + ArrThreeLength;
           // System.Windows.MessageBox.Show(totalArrLength.ToString());
            if (totalArrLength > 0)
            {





                //    var combinedSearchData = (from arr in combinedSearchArray select new { FolderName = arr[0], FolderPath = arr[1] });
                //    moveTo_dataGrid.ItemsSource = combinedSearchData.ToList();
                //System.Windows.MessageBox.Show(totalArrLength.ToString());
                //combinedSearchArrays = new string[totalArrLength][];
                if (ArrOneLength > 0){combinedSearchArrays = combinedSearchArrays.Concat(searchOneArray).ToArray();}
                if (ArrTwoLength > 0)
                {
                    //System.Windows.MessageBox.Show(searchTwoArray[0][0]);
                    combinedSearchArrays = combinedSearchArrays.Concat(searchTwoArray).ToArray();
                    //System.Windows.MessageBox.Show(combinedSearchArrays[0][0]);
                }
                if (ArrThreeLength > 0) { combinedSearchArrays = combinedSearchArrays.Concat(searchThreeArray).ToArray(); }
                if (ArrFourLength > 0) { combinedSearchArrays = combinedSearchArrays.Concat(searchFourArray).ToArray(); }

                var combinedSearchData = (from arr in combinedSearchArrays select new { FolderName = arr[0], FolderPath = arr[1] });
                moveTo_dataGrid.ItemsSource = combinedSearchData.ToList();
                //if (combinedSearchArrays.Length > 0) { System.Windows.MessageBox.Show(combinedSearchArrays[1][0] + ", " + combinedSearchArrays[1][0]); }
                //System.Windows.MessageBox.Show(combinedArrays[14][0].ToString() + ", " + combinedArrays[14][1].ToString());

            }






            //string text = System.IO.File.ReadAllText(backupFileTxtPath);
            //test_TextBox.Text = text;

        }


        //Function that moves on to next item in array - for skipping items that have been moved or just to skip
        public void NextToMove()
        {
            if (sourceFilesAndDirsCount > currentArrayIndex + 1) //make sure that we haven't gotten to the end of the array
            {
                currentArrayIndex++;
                fileName_TextBox.Text = System.IO.Path.GetFileName(sourceFilesAndDirs[currentArrayIndex]).ToString();
                filePath_TextBox.Text = (sourceFilesAndDirs[currentArrayIndex]).ToString();

                if (String.IsNullOrEmpty(sourceFilesAndDirs[currentArrayIndex]) && sourceFilesAndDirsCount > currentArrayIndex + 1)
                {
                    currentArrayIndex++;
                    fileName_TextBox.Text = System.IO.Path.GetFileName(sourceFilesAndDirs[currentArrayIndex]).ToString();
                    filePath_TextBox.Text = (sourceFilesAndDirs[currentArrayIndex]).ToString();
                }
            }
            else //if we did get to end of array then start at beginning again
            {
                currentArrayIndex = 0;
                fileName_TextBox.Text = System.IO.Path.GetFileName(sourceFilesAndDirs[currentArrayIndex]).ToString();
                filePath_TextBox.Text = (sourceFilesAndDirs[currentArrayIndex]).ToString();

                if (String.IsNullOrEmpty(sourceFilesAndDirs[currentArrayIndex]))
                {
                    currentArrayIndex++;
                    fileName_TextBox.Text = System.IO.Path.GetFileName(sourceFilesAndDirs[currentArrayIndex]).ToString();
                    filePath_TextBox.Text = (sourceFilesAndDirs[currentArrayIndex]).ToString();
                }
            }
        }


    }
}
