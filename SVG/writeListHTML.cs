using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System;
using System.Globalization;

/**/

namespace TestContainer
{
    public class commentLinesSVG //: MonoBehaviour
    {
        public List<string> splitFile = new List<string>();

        //DateTime dt1 = new DateTime(2015, 12, 31); 
        DateTime dt1 = DateTime.Now;

        public static string fileName = "rip testVG.svg";
        public static string EditedLine = "";

        //public static string xmlFolder = "/Users/dylanzhu/Desktop/TestVG";
        public static string xmlFolder = "/Users/dylanzhu/Desktop/TextFiles";
        public static int strokeNum;
        public static int originalFileLineNumber;

        //Defining Tabs Stuff
        public static string tabs;
        public static int lines;

        //public static string leftInsertLocation = "─ [";
        public static string leftInsertLocation = "[";
        public static int identifierLength = leftInsertLocation.Length;

        public List<int> KeyList = new List<int>();
        public Dictionary<int, string> locationAndCommentedLine = new Dictionary<int, string>();

        public static string test = "123456789";

        public static void originalFile()
        {

            var readAll = File.ReadAllText(fileName);

            //Updating for VSCode
            //Console.WriteLine(readAll);
            Console.WriteLine(readAll);
        }

        // Start is called before the first frame update
        public void Start()
        {
            //Updating for VSCode
            //Console.WriteLine("Initiated");
            Console.WriteLine("Initiated");
            IterateThroughSVGs();
        }

        void FileToList()
        {
            int counter = 0;
            string line;

            // First read of the file to determine array size
            System.IO.StreamReader file = new System.IO.StreamReader(fileName);
            while ((line = file.ReadLine()) != null)
            {
                splitFile.Add(line); //Making a longass list appended with every code line
                counter++;
            }

            file.Close();
        }

        public void StrokeDetection()
        {   
            //This method adds the new code to the previously created string array

            //Create a string array from the list
            string[] sepLines = splitFile.ToArray();
            //Console.WriteLine("The Array is " + sepLines.Length + " long");

            //Start from line 0, iterate through the file by adding to the line number. End for loop when at end of file.
            for (originalFileLineNumber = 0; originalFileLineNumber < sepLines.Length; originalFileLineNumber++)
            {
                int firstChar = 0;
                //As long as not at the end of the file line. Testing a couple characters in front of the "cursor"
                for (int charNum = 0; (charNum + identifierLength - 1) < sepLines[originalFileLineNumber].Length; charNum++)
                {
                    //Test the isolated section against the identifier.
                    string section = sepLines[originalFileLineNumber].Substring(charNum, identifierLength);

                    if (section == leftInsertLocation)
                    {
                        //Only record the character location of the first instance of the flag marker 
                        //(Actually unnecessary with the break at the end lol)
                        if (firstChar == 0)
                            firstChar = charNum;
                        
                        //Full Line Rn
                        string sectionedLine = sepLines[originalFileLineNumber].Substring(firstChar, sepLines[originalFileLineNumber].Length-firstChar);

                        //Console.WriteLine("A path instance occured at line: " + (originalFileLineNumber + 1));
                        //Writing the new line

                        /*
                        Console.WriteLine("originalFileLineNumber: " + (originalFileLineNumber));
                        Console.WriteLine("charNum: " + (charNum));
                        Console.WriteLine("identifierLength: " + (identifierLength));
                        Console.WriteLine("identifierLength: " + (sepLines[originalFileLineNumber].Length-1));
                        /**/

                        tabs = new String('\t', 3*lines);

                        EditedLine = tabs + "<li>" + sectionedLine + "</li>";
                        
                        Console.WriteLine("Charnum: " + charNum + ", "  + EditedLine);
                        
                        //locationAndCommentedLine.Add((originalFileLineNumber + 1), ("<!-- " + sepLines[originalFileLineNumber] + "</l>"));
                        locationAndCommentedLine.Add((originalFileLineNumber + 1), (EditedLine));
                        KeyList.Add(originalFileLineNumber + 1);
                        break;
                    }
                }
            }
        }

        public void NewPictureCreation()
        {
            //Create the new file 

            int[] locationKey = KeyList.ToArray();

            ArrayList lines = new ArrayList();
            StreamReader rdr = new StreamReader(fileName);

            /*
            string newFolderName = fileName.Substring(0, fileName.Length - 4);
            if (!Directory.Exists(newFolderName))  // if it doesn't exist, create
                Directory.CreateDirectory(newFolderName);
            //Console.WriteLine("Folder should be created by now.");
            /**/

            string line;

            while ((line = rdr.ReadLine()) != null)
            {
                lines.Add(line);
            }
            rdr.Close();

            //StreamWriter reName = new StreamWriter(newFolderName + "/" + "Fully_Listed.txt");

            StreamWriter reName = new StreamWriter("Fully_Listed.txt");

            foreach (string strNewLine in lines)
            {
                reName.WriteLine(strNewLine);
            }
            reName.Close();

            ///*

            //Cutting and Inserting For Every Found Instance of the Stuff
            for (int instances = locationAndCommentedLine.Count - 1; instances > -1; instances--)
            {
                lines.RemoveAt(locationKey[instances]-1);
                lines.Insert(locationKey[instances]-1, locationAndCommentedLine[locationKey[instances]]);

            }

            //Write the New File
            StreamWriter wrtr = new StreamWriter(xmlFolder + "/Fully_Listed.txt");

            foreach (string strNewLine in lines)
            {
                wrtr.WriteLine(strNewLine);
            }
            wrtr.Close();


            var checkFiles = File.ReadAllText(xmlFolder + "/Fully_Listed.txt");
            //Console.WriteLine("File " + (locationAndCommentedLine.Count - instances) + " = " + checkFiles);
            

            //Original Code for SVG Splitting
            /*
            for (int instances = locationAndCommentedLine.Count - 1; instances > -1; instances--)
            {
                lines.RemoveAt(locationKey[instances]-1);
                lines.Insert(locationKey[instances]-1, locationAndCommentedLine[locationKey[instances]]);

                //Creating new files with different successive strokes commented out.
                StreamWriter wrtr = new StreamWriter(newFolderName + "/" + instances + ".txt");

                foreach (string strNewLine in lines)
                {
                    wrtr.WriteLine(strNewLine);
                }
                wrtr.Close();

                var checkFiles = File.ReadAllText(newFolderName + "/" + instances + ".txt");
                //Console.WriteLine("File " + (locationAndCommentedLine.Count - instances) + " = " + checkFiles);
            }

            /**/

            //Finally removes the original file.
            /*
            if (File.Exists(fileName))
            {
                try
                {
                    File.Delete(fileName);
                }
                catch
                {
                    //Updating for VSCode
                    //Console.WriteLine("This file couldn't be removed for God knows why :(");
                    Console.WriteLine("This file couldn't be removed for God knows why :(");
                    return;
                }
            }
            lines.Clear();
            /**/
        }

        public void IterativeCleanUp()
        {
            KeyList.Clear();
            splitFile.Clear();
            locationAndCommentedLine.Clear();
            Console.WriteLine("Everything should be reset.");

        }

        //Iterates through the entire xmlFiles folder until every ".svg" file has been changed into folders
        public void IterateThroughSVGs()
        {
            foreach (string file in Directory.EnumerateFiles(xmlFolder, "*.txt"))
            {
                //Updating for VSCode
                //Console.WriteLine("The current file's name is: " + file);
                Console.WriteLine("The current file's name is: " + file);

                fileName = file;

                //Console.WriteLine("Displaying Original File");
                //originalFile();
                
                Console.WriteLine("Turning file into list");
                FileToList();
                
                Console.WriteLine("Seeing where there are strokes in the code");
                StrokeDetection();
                
                Console.WriteLine("Adding xml comments at the found locations during the specified times");
                NewPictureCreation();
                IterativeCleanUp();

                //Give job duration
                DateTime dt2 = DateTime.Now;
                TimeSpan span = dt2.Subtract(dt1);//33.00:00:00
                Console.WriteLine("Job took: " + span);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            commentLinesSVG cLS1 = new commentLinesSVG();
            // call directly static method with class name
            cLS1.Start();
        }

    }
}