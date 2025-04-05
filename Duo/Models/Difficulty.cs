using System.Collections.Generic;

namespace Duo.Models
{
    public enum Difficulty
    {
        Easy = 1,
        Normal = 2,
        Hard = 3
    }

    // list with difficulties as strings
    public static class DifficultyList
    {
        public static readonly List<string> DIFFICULTIES = new ()
        {
            "Easy",
            "Normal",
            "Hard"
        };
    }
}