using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System;
/**/

public class commentLinesSVG : MonoBehaviour
{
    public List<string> splitFile = new List<string>();

    public static string fileName = "rip testVG.svg";

    public static string xmlFolder = "/Users/dylanzhu/Desktop/SVG/TestVG";
    //public static string xmlFolder = "/Users/dylanzhu/Desktop/ /Unity Projects/characterDrawingTest/Assets/Resources/xmlFiles";
    //public static string xmlFolder = "/Users/dylanzhu/Desktop/ /Unity Projects/characterDrawingTest/Assets/Scenes/xmlFiles";
    //public static string xmlFolder = "/Users/dylanzhu/Desktop/testFolder";

    public static int strokeNum;
    public static int originalFileLineNumber;
    public static string strokeIdentifier = "<path id=";
    public static int identifierLength = strokeIdentifier.Length;

    public List<int> KeyList = new List<int>();
    public Dictionary<int, string> locationAndCommentedLine = new Dictionary<int, string>();

    public static string test = "123456789";

    public static void originalFile()
    {

        var readAll = File.ReadAllText(fileName);

        //Updating for VSCode
        //Debug.Log(readAll);
        Console.WriteLine(readAll);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Updating for VSCode
        //Debug.Log("Initiated");
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
            splitFile.Add(line);
            counter++;
        }

        file.Close();
    }

    public void StrokeDetection()
    {   
        string[] sepLines = splitFile.ToArray();
        //Debug.Log("The Array is " + sepLines.Length + " long");
        for (originalFileLineNumber = 0; originalFileLineNumber < sepLines.Length; originalFileLineNumber++)
        {
            for (int charNum = 0; (charNum + identifierLength - 1) < sepLines[originalFileLineNumber].Length; charNum++)
            {
                string section = sepLines[originalFileLineNumber].Substring(charNum, identifierLength);
                if (section == strokeIdentifier)
                {
                    //Debug.Log("A path instance occured at line: " + (originalFileLineNumber + 1));
                    locationAndCommentedLine.Add((originalFileLineNumber + 1), ("<!-- " + sepLines[originalFileLineNumber] + " -->"));
                    KeyList.Add(originalFileLineNumber + 1);
                }
            }
        }

        //Resets sepLines array
        //Array.Clear(sepLines, 0, sepLines.Length);
    }

    public void NewPictureCreation()
    {
        //Debug.Log("There are " + locationAndCommentedLine.Count + " strokes");
        int[] locationKey = KeyList.ToArray();

        //Updating for VSCode
        //Debug.Log("There are " + locationAndCommentedLine.Count + " strokes in this character.");


        ArrayList lines = new ArrayList();
        StreamReader rdr = new StreamReader(fileName);
        string newFolderName = fileName.Substring(0, fileName.Length - 4);
        if (!Directory.Exists(newFolderName))  // if it doesn't exist, create
            Directory.CreateDirectory(newFolderName);
        //Debug.Log("Folder should be created by now.");

        string line;

        while ((line = rdr.ReadLine()) != null)
        {
            lines.Add(line);
        }
        rdr.Close();

        StreamWriter reName = new StreamWriter(newFolderName + "/" + "Original.svg");

        foreach (string strNewLine in lines)
        {
            reName.WriteLine(strNewLine);
        }
        reName.Close();


        for (int instances = locationAndCommentedLine.Count - 1; instances > -1; instances--)
        {
            lines.RemoveAt(locationKey[instances]-1);
            lines.Insert(locationKey[instances]-1, locationAndCommentedLine[locationKey[instances]]);


            //Creating new files with different successive strokes commented out.
            StreamWriter wrtr = new StreamWriter(newFolderName + "/" + instances + ".svg");

            foreach (string strNewLine in lines)
            {
                wrtr.WriteLine(strNewLine);
            }
            wrtr.Close();

            var checkFiles = File.ReadAllText(newFolderName + "/" + instances + ".svg");
            //Debug.Log("File " + (locationAndCommentedLine.Count - instances) + " = " + checkFiles);
        }

        //Finally removes the original file.
        if (File.Exists(fileName))
        {
            try
            {
                File.Delete(fileName);
            }
            catch
            {
                //Updating for VSCode
                //Debug.Log("This file couldn't be removed for God knows why :(");
                Console.WriteLine("This file couldn't be removed for God knows why :(");
                return;
            }
        }
        lines.Clear();
    }

    public void IterativeCleanUp()
    {
        KeyList.Clear();
        splitFile.Clear();
        locationAndCommentedLine.Clear();
        //Debug.Log("Everything should be reset.");

    }

    //Iterates through the entire xmlFiles folder until every ".svg" file has been changed into folders
    public void IterateThroughSVGs()
    {
        foreach (string file in Directory.EnumerateFiles(xmlFolder, "*.svg"))
        {
            //rip testVG

            //Updating for VSCode
            //Debug.Log("The current file's name is: " + file);
            Console.WriteLine("The current file's name is: " + file);

            fileName = file;

            //Debug.Log("Displaying Original File");
            //originalFile();
            //Debug.Log("Turning file into list");
            FileToList();
            //Debug.Log("Seeing where there are strokes in the code");
            StrokeDetection();
            //Debug.Log("Adding xml comments at the found locations during the specified times");
            NewPictureCreation();
            IterativeCleanUp();
        }
    }
}