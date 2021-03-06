
/*
This RPG data streaming assignment was created by Fernando Restituto.
Pixel RPG characters created by Sean Browning.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

#region Assignment Instructions

/*  Hello!  Welcome to your first lab :)

Wax on, wax off.

    The development of saving and loading systems shares much in common with that of networked gameplay development.  
    Both involve developing around data which is packaged and passed into (or gotten from) a stream.  
    Thus, prior to attacking the problems of development for networked games, you will strengthen your abilities to develop solutions using the easier to work with HD saving/loading frameworks.

    Try to understand not just the framework tools, but also, 
    seek to familiarize yourself with how we are able to break data down, pass it into a stream and then rebuild it from another stream.


Lab Part 1

    Begin by exploring the UI elements that you are presented with upon hitting play.
    You can roll a new party, view party stats and hit a save and load button, both of which do nothing.
    You are challenged to create the functions that will save and load the party data which is being displayed on screen for you.

    Below, a SavePartyButtonPressed and a LoadPartyButtonPressed function are provided for you.
    Both are being called by the internal systems when the respective button is hit.
    You must code the save/load functionality.
    Access to Party Character data is provided via demo usage in the save and load functions.

    The PartyCharacter class members are defined as follows.  */

public partial class PartyCharacter
{
    public int classID;

    public int health;
    public int mana;

    public int strength;
    public int agility;
    public int wisdom;

    public LinkedList<int> equipment;

}


/*
    Access to the on screen party data can be achieved via …..

    Once you have loaded party data from the HD, you can have it loaded on screen via …...

    These are the stream reader/writer that I want you to use.
    https://docs.microsoft.com/en-us/dotnet/api/system.io.streamwriter
    https://docs.microsoft.com/en-us/dotnet/api/system.io.streamreader

    Alright, that’s all you need to get started on the first part of this assignment, here are your functions, good luck and journey well!
*/


#endregion


#region Assignment Part 1

static public class AssignmentPart1
{
  
    static public void SavePartyButtonPressed()
    {

        StreamWriter sw = new StreamWriter(Application.dataPath + Path.DirectorySeparatorChar + "SaveFileClass01.txt");
        foreach (PartyCharacter pc in GameContent.partyCharacters)
        {
            //Debug.Log("PC class id == " + pc.classID);
            sw.WriteLine(pc.classID + "," + pc.health + "," + pc.mana + "," + pc.strength + "," + pc.agility + "," + pc.wisdom);

            foreach (int e in pc.equipment)
            {
                //Debug.Log("EQUIPAMENT = " + e);
                sw.WriteLine(e);
            }

            sw.WriteLine("");
        }

        
        sw.Close();
    }

    static public void LoadPartyButtonPressed()
    {

        GameContent.partyCharacters.Clear();

        if(File.Exists(Application.dataPath + Path.DirectorySeparatorChar + "SaveFileClass01.txt"))
        {
            string line = "";

            StreamReader sr = new StreamReader(Application.dataPath + Path.DirectorySeparatorChar + "SaveFileClass01.txt");
            
            while((line = sr.ReadLine()) != null)
            {
                string[] savedLine = line.Split(',');

                PartyCharacter pc = new PartyCharacter(int.Parse(savedLine[0]), int.Parse(savedLine[1]), int.Parse(savedLine[2]), int.Parse(savedLine[3]), int.Parse(savedLine[4]), int.Parse(savedLine[5]));

                while((line = sr.ReadLine()) != "")
                {
                    pc.equipment.AddLast(int.Parse(line));
                }

                GameContent.partyCharacters.AddLast(pc);
            }
            
          
        }
        //GameContent.partyCharacters.Clear();

        GameContent.RefreshUI();

    }

}


#endregion


#region Assignment Part 2

//  Before Proceeding!
//  To inform the internal systems that you are proceeding onto the second part of this assignment,
//  change the below value of AssignmentConfiguration.PartOfAssignmentInDevelopment from 1 to 2.
//  This will enable the needed UI/function calls for your to proceed with your assignment.
static public class AssignmentConfiguration
{
    public const int PartOfAssignmentThatIsInDevelopment = 2;
}

/*

In this part of the assignment you are challenged to expand on the functionality that you have already created.  
    You are being challenged to save, load and manage multiple parties.
    You are being challenged to identify each party via a string name (a member of the Party class).

To aid you in this challenge, the UI has been altered.  

    The load button has been replaced with a drop down list.  
    When this load party drop down list is changed, LoadPartyDropDownChanged(string selectedName) will be called.  
    When this drop down is created, it will be populated with the return value of GetListOfPartyNames().

    GameStart() is called when the program starts.

    For quality of life, a new SavePartyButtonPressed() has been provided to you below.

    An new/delete button has been added, you will also find below NewPartyButtonPressed() and DeletePartyButtonPressed()

Again, you are being challenged to develop the ability to save and load multiple parties.
    This challenge is different from the previous.
    In the above challenge, what you had to develop was much more directly named.
    With this challenge however, there is a much more predicate process required.
    Let me ask you,
        What do you need to program to produce the saving, loading and management of multiple parties?
        What are the variables that you will need to declare?
        What are the things that you will need to do?  
    So much of development is just breaking problems down into smaller parts.
    Take the time to name each part of what you will create and then, do it.

Good luck, journey well.

*/

static public class AssignmentPart2
{
    private static LinkedList<PartySaveData> parties;
    public const string PartyMetaFiles = "PartyIndexesAndNames.txt";
    private static int lastUseIndex;
    static public void GameStart()
    {
        GameContent.RefreshUI();
        LoadPartyMetaFiles();

    }

    static public List<string> GetListOfPartyNames()
    {

        if (parties == null)
            return new List<string>();

        LinkedList<string> pNames = new LinkedList<string>();

        foreach(PartySaveData p in parties)
        {
            pNames.AddLast(p.name);
        }


        return new List<string>(pNames);
    }

    static public void LoadPartyDropDownChanged(string selectedName)
    {
        foreach (PartySaveData p in parties)
        {
            if (p.name == selectedName)
                p.load();
        }

        GameContent.RefreshUI();
    }

    static public void SavePartyButtonPressed()
    {
        lastUseIndex++;
        PartySaveData e = new PartySaveData(lastUseIndex, GameContent.GetPartyNameFromInput());
        parties.AddLast(e);

        SavePartyMetaFiles();

        e.save();

        GameContent.RefreshUI();
       
    }

    static public void NewPartyButtonPressed()
    {

    }

    static public void DeletePartyButtonPressed()
    {

    }


    static public void SavePartyMetaFiles()
    {
        StreamWriter sw = new StreamWriter(Application.dataPath + Path.DirectorySeparatorChar + PartyMetaFiles);

        sw.WriteLine("1," + lastUseIndex);

        foreach(PartySaveData pData in parties)
        {
            sw.WriteLine("2," + pData.index + "," + pData.name);
        }

        sw.Close();

    }

    static public void LoadPartyMetaFiles()
    {
        parties = new LinkedList<PartySaveData>();
        string path = Application.dataPath + Path.DirectorySeparatorChar + PartyMetaFiles;
        

        if (File.Exists(path))
        {
            string line = "";
            StreamReader sr = new StreamReader(path); ;

            while ((line = sr.ReadLine()) != null)
            {
                string[] savedLine = line.Split(',');

                int SafeDataSignifier = int.Parse(savedLine[0]);

                if(SafeDataSignifier == 1)
                {
                    lastUseIndex = int.Parse(savedLine[1]);
                }
                else if (SafeDataSignifier == 2)
                {
                    parties.AddLast(new PartySaveData(int.Parse(savedLine[1]), savedLine[2]));
                }
            }

            sr.Close();

        }


    }
}

class PartySaveData
{
    
    public int index;
    public string name;
    public PartySaveData(int index, string name)
    {
        this.index = index;
        this.name = name;
    }

    public void save()
    {
        StreamWriter sw = new StreamWriter(Application.dataPath + Path.DirectorySeparatorChar + index + ".txt");
        foreach (PartyCharacter pc in GameContent.partyCharacters)
        {
            //Debug.Log("PC class id == " + pc.classID);
            sw.WriteLine(pc.classID + "," + pc.health + "," + pc.mana + "," + pc.strength + "," + pc.agility + "," + pc.wisdom);

            foreach (int e in pc.equipment)
            {
                //Debug.Log("EQUIPAMENT = " + e);
                sw.WriteLine(e);
            }

            sw.WriteLine("");
        }


        sw.Close();
    }

    public void load()
    {
        GameContent.partyCharacters.Clear();

        if (File.Exists(Application.dataPath + Path.DirectorySeparatorChar + index + ".txt"))
        {
            string line = "";

            StreamReader sr = new StreamReader(Application.dataPath + Path.DirectorySeparatorChar + index + ".txt");

            while ((line = sr.ReadLine()) != null)
            {
                string[] savedLine = line.Split(',');

                PartyCharacter pc = new PartyCharacter(int.Parse(savedLine[0]), int.Parse(savedLine[1]), int.Parse(savedLine[2]), int.Parse(savedLine[3]), int.Parse(savedLine[4]), int.Parse(savedLine[5]));

                while ((line = sr.ReadLine()) != "")
                {
                    pc.equipment.AddLast(int.Parse(line));
                }

                GameContent.partyCharacters.AddLast(pc);

            }

            sr.Close();

        }
        //GameContent.partyCharacters.Clear();

        GameContent.RefreshUI();
    }
}
#endregion


