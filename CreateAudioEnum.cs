string [] audioClips;



public void CreateAudioConsts()
{
   string copyPath = Application.dataPath + "/Scripts/General/SOUND_CONSTANT.cs";

    Debug.Log("Creating Classfile: " + copyPath);
    {
        using (StreamWriter outfile =
            new StreamWriter(copyPath))
        {
            outfile.WriteLine("using UnityEngine;");
            outfile.WriteLine("using System.Collections;");
            outfile.WriteLine("");
            outfile.WriteLine("public enum SOUND_CONSTANT ");
            outfile.WriteLine("{");
            for (int i = 0; i < oldConsts.Count; i++)
            {
                string temp = audioClips[i];
                if (audioClips.Count > 1 && i < audioClips.Count - 1)
                {
                    temp = audioClips[i]+ ",";
                }
                outfile.WriteLine(temp);
            }
            outfile.WriteLine("}");
        }
    }
}
