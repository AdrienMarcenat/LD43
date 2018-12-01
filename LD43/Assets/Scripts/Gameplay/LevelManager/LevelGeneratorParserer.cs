using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static class LevelGeneratorParserer
{
    public static void ParseFile (this LevelGenerator levelGenerator, string file)
    {
        string[] lines = File.ReadAllLines (file);
        List<LevelSequence> sequences = levelGenerator.m_OrderSequences = new List<LevelSequence> ();
        LevelSequence currentSequence = new LevelSequence ();
        foreach (string line in lines)
        {
            try
            {
                string[] words = line.Split (' ');
                if (words.Length == 0)
                {
                    continue;
                }
                LevelSequence.ELevelSequenceStart type = LevelSequence.Parse (words[0]);
                //New sequence
                if (type != LevelSequence.ELevelSequenceStart.ERROR)
                {
                    if (currentSequence.m_Orders.Count > 0 && currentSequence.m_Start != LevelSequence.ELevelSequenceStart.ERROR)
                    {
                        sequences.Add (currentSequence);
                    }
                    currentSequence = new LevelSequence (type);
                    continue;
                }
                //New order
                LevelOrder order = MakeLevelOrder (words);
                currentSequence.m_Orders.Add (order);

            }
            catch (Exception e)
            {
                Debug.LogWarning ("Couldn't read line \"" + line + "\" of file \"" + file + "\"");
                Debug.LogWarning (e.Message);
            }
        }
        if (currentSequence.m_Orders.Count > 0 && currentSequence.m_Start != LevelSequence.ELevelSequenceStart.ERROR)
        {
            sequences.Add (currentSequence);
        }
    }

    static private LevelOrder MakeLevelOrder (string[] args)
    {
        System.DateTime time = System.DateTime.ParseExact (args[0], "mm:ss.fff", null);
        float floatTime = time.Minute * 60 + time.Second + 0.001f * time.Millisecond;
        switch (args[1].ToLower ())
        {
            case ("spawn"):
                return new SpawnLevelOrder (args.SubArray (2), floatTime);

            case ("talk"):
                return new TalkLevelOrder (args[2], floatTime);

            case ("music"):
                //TODO
                break;

            case ("endgame"):
                return new EndLevelOrder (floatTime);
        }
        return null;
    }
}
