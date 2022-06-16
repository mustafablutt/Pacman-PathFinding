using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class ScoreHolder
{
    public static int score;
    public static int level;

    public static int Score
    {
        get { return score; }
        set { score = value; }
    }

    public static int Level
    {
        get => level;
        set => level = value;
    }
}