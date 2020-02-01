using System.Collections.Generic;

public static class Localization
{
    // TODO: This ought to be in files instead of in code
    public static readonly Dictionary<string, string> localizationTable = new Dictionary<string, string>
    {
        { "Stats_Label_Score", "Score:" },
        { "Stats_Label_Time", "Time:" },
        // The english version of this is "Best X", where "X" stands for "multiplier". This should be kept as short as possible.
        { "Stats_Label_BestMultiplier", "Best X:" },
        { "Stats_Label_Kills", "Kills:" },
        { "Stats_Label_NewHighScore", "New High Score!" },

        // This is a title which is displayed at the top of a global leaderboard.
        { "Leaderboard_Title_GlobalLeaderboard", "Global Leaderboard" },

        // This is a title which is displayed at the top of a friends-only leaderboard.
        { "Leaderboard_Title_FriendsLeaderboard", "Friends Leaderboard" },

        // This is a label which precedes a button prompting the player to switch the leaderboard to showing only friends' scores, rather than all players'.
        { "Leaderboard_Label_FriendsModeSwitchPrompt", "Friends:" },
        // This is a label which precedes a button prompting the player to switch the leaderboard to showing all players' scores, rather than only friends'.
        { "Leaderboard_Label_GlobalModeSwitchPrompt", "Global:" },

        // This is a label which precedes a ranking like "11/120", displaying the current player's position on the leaderboard.
        { "Leaderboard_Label_YourRank", "Your Rank:" },

        { "Leaderboard_Error_CannotSubmitScoreMessage", "Error submitting score" },
        { "Leaderboard_Error_CannotLoadLeaderboardMessage", "Error loading leaderboard" },
        { "Leaderboard_Error_ErrorDetailsLabel", "Details" },
        { "Leaderboard_Error_ScoreBufferedMessage", "Your score has been saved offline and will be submitted automatically when possible." },
        { "Leaderboard_Error_CheckConnectionMessage", "Check your internet connection. If you are connected, there might be something wrong on our end." },
        { "Leaderboard_Error_CheatingMessage", "Score rejected. If you believe this is an error, contact the Breakpoint dev team. (reason: {0} | frame: {1})" },
    };

    public static string Localize(string id)
    {
        string value = id;
        if (!localizationTable.TryGetValue(id, out value))
        {
            value = id;
        }

        return value;
    }
}
