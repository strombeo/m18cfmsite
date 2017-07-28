using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Madden.Models.ImportModels
{
    public class ImportTeam
    {
        public int tODiff { get; set; } = 0;
        public int primaryColor { get; set; } = 0;
        public int offRushYds { get; set; } = 0;
        public int divLosses { get; set; } = 0;
        public int homeTies { get; set; } = 0;
        public int awayWins { get; set; } = 0;
        public double winPct { get; set; } = 0.0;
        public int logoId { get; set; } = 0;
        public int ptsForRank { get; set; } = 0;
        public string conferenceName { get; set; } = "";
        public int confTies { get; set; } = 0;
        public int totalTies { get; set; } = 0;
        public int divWins { get; set; } = 0;
        public int offPassYds { get; set; } = 0;
        public int ovrRating { get; set; } = 0;
        public string teamName { get; set; } = "";
        public int divisionId { get; set; } = 0;
        public int defTotalYds { get; set; } = 0;
        public int defScheme { get; set; } = 0;
        public int defRushYds { get; set; } = 0;
        public int rank { get; set; } = 0;
        public int playoffStatus { get; set; } = 0;
        public string displayName { get; set; } = "";
        public int offRushYdsRank { get; set; } = 0;
        public long capRoom { get; set; } = 0;
        public string nickName { get; set; } = "";
        public int teamOvr { get; set; } = 0;
        public int secondaryColor { get; set; } = 0;
        public int seasonIndex { get; set; } = 0;
        public int homeLosses { get; set; } = 0;
        public int awayTies { get; set; } = 0;
        public int seed { get; set; } = 0;
        public int netPts { get; set; } = 0;
        public int defPassYds { get; set; } = 0;
        public int ptsAgainst { get; set; } = 0;
        public int offTotalYds { get; set; } = 0;
        public int stageIndex { get; set; } = 0;
        public int offPassYdsRank { get; set; } = 0;
        public int divTies { get; set; } = 0;
        public int offScheme { get; set; } = 0;
        public int confLosses { get; set; } = 0;
        public int totalLosses { get; set; } = 0;
        public int defTotalYdsRank { get; set; } = 0;
        public string userName { get; set; } = "";
        public int homeWins { get; set; } = 0;
        public string cityName { get; set; } = "";
        public int defRushYdsRank { get; set; } = 0;
        public long capAvailable { get; set; } = 0;
        public int conferenceId { get; set; } = 0;
        public int teamId { get; set; } = 0;
        public string divisionName { get; set; } = "";
        public int injuryCount { get; set; } = 0;
        public int ptsFor { get; set; } = 0;
        public int calendarYear { get; set; } = 0;
        public int confWins { get; set; } = 0;
        public int totalWins { get; set; } = 0;
        public long capSpent { get; set; } = 0;
        public string abbrName { get; set; } = "";
        public string divName { get; set; } = "";
        public int winLossStreak { get; set; } = 0;
        public int defPassYdsRank { get; set; } = 0;
        public int weekIndex { get; set; } = 0;
        public int ptsAgainstRank { get; set; } = 0;
        public int prevRank { get; set; } = 0;
        public int offTotalYdsRank { get; set; } = 0;
        public int awayLosses { get; set; } = 0;

        public ImportTeam()
        {
            // do nothing, just here so I can deserialize json
        }
    }
}