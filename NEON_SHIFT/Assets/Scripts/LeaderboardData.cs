using System;
using System.Collections.Generic;

// Esto nos permite guardarlo como JSON
[Serializable]
public class ScoreEntry
{
    public string playerName;
    public int score;
}

[Serializable]
public class Leaderboard
{
    // Una lista de las 10 mejores entradas
    public List<ScoreEntry> entries = new List<ScoreEntry>();
}