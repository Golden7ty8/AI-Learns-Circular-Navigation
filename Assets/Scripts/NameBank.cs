using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameBank : MonoBehaviour
{

    //static int namesUsedCount = 0;
    static string[] names = {
        "Alastair", "Bronte", "Jason Chipp", "Evan", "Soka", "Cotter", "Gabriel", "Amber", "Blaze",
		"MatPat", "Stephanie", "Chris", "Jason",
        "Bob", "Ms. Applegate",
        "Reg", "Riko", "Nanachi", "Ozen", "Lyza",
        "Ainz", "Albeto", "Shal Tier", "Aura", "Marey", "Cokitus", "Demiurge", "Sebis", "Gasef", "Brain", "Envy", "Enry",
        "Elias", "Chise", "Zaniba", "Ubaba", "Chihiro",
        "Shita", "Potsu", "Muska",
        "Violet",
        "Edward", "Alfonse", "Hoenheim",
        "Eragon", "Saphira", "Roran", "Naswada", "Arya",
        "Harry", "Ron", "Hermionie", "Albus", "Minerva", "Filch", "Snape", "Qwirl", "Gilderoy", "Loopin", "Sprout", "Moody", "Umbridge", "Slughorn", "Hagrid", "Vernin", "Petunia", "Dugley", "Wood", "Flitwick", "Serious", "Peter", "Tom", "Jinny", "George", "Fred", "Molly", "Aurther", "Percy", "Lucius", "Draco", "Crab", "Goyal", "Reeterskeeter", "Dobey",
        "Katniss", "Peeta", "President Snow",
		"Jimmy", "Conan", "Racheal", "Detective Moore",
		"Calvin", "Hobbes", "Susie",
		"FireStar",
		"Garfield", "Odie", "John",
        "Remy", "Linguini", "Colette"
    };
    static int namesLeftCount = names.Length;
    static bool[] pickedNames = new bool[names.Length];

    void Start()
    {
        for(int i = 0; i < pickedNames.Length; i++)
        {
            pickedNames[i] = false;
        }

        Debug.Log("Name Count: " + names.Length);
    }

    public static string GetRandomName()
    {
        //int namesLeftCount = names.Length - (namesUsedCount % names.Length);

        string res = "";

        //If we've run out of names in the names array...
        if(namesLeftCount <= 0)
        {
            res = "Robot #" + ((namesLeftCount * -1) + 1).ToString();
        } else
        {
            int pickedAvailableIndex = Random.Range(0, namesLeftCount);
            //Go through names array, not counting those already picked until you reach the pickedAvailableIndex'th
            //non-picked one.
            for(int i = 0; i < pickedNames.Length; i++)
            {
                if(!pickedNames[i])
                {
                    if(pickedAvailableIndex == 0)
                    {
                        pickedNames[i] = true;
                        res = names[i];
                        break;
                    } else
                    {
                        pickedAvailableIndex--;
                    }
                }
            }
        }

        //Just in case ;).
        if (res == "")
            Debug.LogWarning("GetRandomName returned \"\"!");

        namesLeftCount--;
        return res;
    }

    /*public static string GetName(string[] requestedNames)
    {
        //int generation = (namesUsedCount / names.Length) + 1;

        //Start by trying to get the requested name, starting with index 0 of requested names.
        //For every requested name...
        for (int i)

        

    }*/

    public static string GetRomanNumeral(int num)
    {
        string res = "";

        int numLeft = num;
        while (numLeft > 0)
        {
            if (numLeft >= 1000)
            {
                res += "M";
                numLeft -= 1000;
            }
            else if (numLeft >= 900)
            {
                res += "CM";
                numLeft -= 900;
            }
            else if (numLeft >= 500)
            {
                res += "D";
                numLeft -= 500;
            }
            else if (numLeft >= 400)
            {
                res += "CD";
                numLeft -= 400;
            }
            else if (numLeft >= 100)
            {
                res += "C";
                numLeft -= 100;
            }
            else if (numLeft >= 90)
            {
                res += "XC";
                numLeft -= 90;
            }
            else if (numLeft >= 50)
            {
                res += "L";
                numLeft -= 50;
            }
            else if (numLeft >= 40)
            {
                res += "XL";
                numLeft -= 40;
            }
            else if (numLeft >= 10)
            {
                res += "X";
                numLeft -= 10;
            }
            else if (numLeft >= 9)
            {
                res += "IX";
                numLeft -= 9;
            }
            else if (numLeft >= 5)
            {
                res += "V";
                numLeft -= 5;
            }
            else if (numLeft >= 4)
            {
                res += "IV";
                numLeft -= 4;
            }
            else if (numLeft >= 1)
            {
                res += "I";
                numLeft -= 1;
            }
        }

        return res;

    }

}
