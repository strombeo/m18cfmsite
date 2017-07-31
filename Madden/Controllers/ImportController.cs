using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Madden.Models.ImportModels;
using Newtonsoft.Json;

namespace Madden.Controllers
{
    public class ImportController : Controller
    {
        // create a db context
        private MaddenDataContext db = new MaddenDataContext();

        // POST: ImportTeams
        [HttpPost]
        public ActionResult ImportTeams(int leagueId, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    // Read the file into a string then deserialize
                    string plainString = ReadUploadedFile(file);
                    var res = JsonConvert.DeserializeObject<dynamic>(plainString);

                    // Extract Teams into List of TeamImport objects
                    List<ImportTeam> importTeams = new List<ImportTeam>();
                    foreach (var item in res)
                    {
                        foreach (var sub in item)
                        {
                            //string subString = sub.ToString();
                            importTeams.Add(JsonConvert.DeserializeObject<ImportTeam>(sub.ToString()));
                        }
                    }
                    // there should be more elegant history done here...  
                    // but for now just remove all teams from this league and re-add them

                    // Remove all of the teams from the DB for this League
                    db.Teams.DeleteAllOnSubmit(db.Teams.Where(m => m.LeagueID == leagueId));
                    db.SubmitChanges();

                    // now iterate over the teams, importing each one
                    foreach (var importTeam in importTeams)
                    {
                        ImportTeam(leagueId, importTeam);
                    }

                    ViewBag.Message = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            return RedirectToAction("Leagues", "Madden");
        }


        // POST: ImportPlayers
        [HttpPost]
        public ActionResult ImportPlayers(int leagueId, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    // Read the file into a string then deserialize
                    string plainString = ReadUploadedFile(file);
                    var res = JsonConvert.DeserializeObject<dynamic>(plainString);

                    // Extract Players into List of ImportPlayer objects
                    List<ImportPlayer> importPlayers = new List<ImportPlayer>();
                    foreach (var item in res)
                    {
                        foreach (var leag in item)
                        {
                            foreach(var outerTeam in leag)
                            {
                                foreach(var innerTeam in outerTeam)
                                {
                                    foreach(var player in innerTeam)
                                    {
                                        string playerString = player.ToString();
                                        playerString = playerString.Substring(playerString.IndexOf("{"));
                                        ImportPlayer importPlayer = JsonConvert.DeserializeObject<ImportPlayer>(playerString);
                                        importPlayers.Add(importPlayer);
                                    }
                                }
                            }
                        }
                    }

                    // there should be more elegant history done here...  
                    // but for now just remove all players from this league and re-add them

                    // Remove all of the players from the DB for this League
                    db.Players.DeleteAllOnSubmit(db.Players.Where(m => m.LeagueID == leagueId));
                    db.SubmitChanges();
                    
                    // now iterate over the teams, importing each one
                    foreach (var importPlayer in importPlayers)
                    {
                        ImportPlayer(leagueId, importPlayer);
                    }

                    ViewBag.Message = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            return RedirectToAction("Leagues", "Madden");
        }

        
        private string ReadUploadedFile(HttpPostedFileBase file)
        {
            string result = string.Empty;

            using (BinaryReader b = new BinaryReader(file.InputStream))
            {
                byte[] binData = b.ReadBytes(file.ContentLength);
                result = System.Text.Encoding.UTF8.GetString(binData);
            }

            return result;
        }


        private void ImportTeam(int leagueId, Models.ImportModels.ImportTeam importTeam)
        {
            // does this team already exist?
            if (db.Teams.Any(m => m.LeagueID == leagueId && m.TeamId == importTeam.teamId))
            {
                // this team already exists, so we need to update it
                var team = db.Teams.Where(m => m.LeagueID == leagueId && m.TeamId == importTeam.teamId).Single();

                #region Assign Team Properties
                // General Info
                team.Username = importTeam.userName;

                // Time Info
                team.CurrentYear = importTeam.calendarYear;
                team.CurrentWeek = importTeam.weekIndex;

                // Team Details
                team.City = importTeam.cityName;
                team.Name = importTeam.teamName;
                team.Conference = importTeam.conferenceName;
                team.Division = importTeam.divisionName;
                team.OverallRating = importTeam.ovrRating;

                // Salary Cap
                team.SalaryCapLimit = importTeam.capRoom;
                team.SalaryCapSpent = importTeam.capSpent;
                team.SalaryCapAvailable = importTeam.capAvailable;

                // Record
                team.Wins = importTeam.totalWins;
                team.Losses = importTeam.totalLosses;
                team.Ties = importTeam.totalTies;

                // Points
                team.PointsFor = importTeam.ptsFor;
                team.PointsAgainst = importTeam.ptsAgainst;
                team.NetPoints = importTeam.netPts;

                // Yardage
                team.OFF_TotalYards = importTeam.offTotalYds;
                team.OFF_RushYards = importTeam.offRushYds;
                team.OFF_PassYards = importTeam.offPassYds;
                team.DEF_TotalYards = importTeam.defTotalYds;
                team.DEF_RushYards = importTeam.defRushYds;
                team.DEF_PashYards = importTeam.defPassYds;

                // Details
                team.TurnoverDifferential = importTeam.tODiff;
                #endregion
                
                // now save all changes
                db.SubmitChanges();
            }
            else
            {
                // this team does not exists, so create a new Team object, then insert it
                Madden.Team team = new Team();

                #region Assign Team Properties
                // IDs
                team.LeagueID = leagueId;
                team.TeamId = importTeam.teamId;

                // General Info
                team.Username = importTeam.userName;

                // Time Info
                team.CurrentYear = importTeam.calendarYear;
                team.CurrentWeek = importTeam.weekIndex;

                // Team Details
                team.City = importTeam.cityName;
                team.Name = importTeam.teamName;
                team.Conference = importTeam.conferenceName;
                team.Division = importTeam.divisionName;
                team.OverallRating = importTeam.ovrRating;

                // Salary Cap
                team.SalaryCapLimit = importTeam.capRoom;
                team.SalaryCapSpent = importTeam.capSpent;
                team.SalaryCapAvailable = importTeam.capAvailable;

                // Record
                team.Wins = importTeam.totalWins;
                team.Losses = importTeam.totalLosses;
                team.Ties = importTeam.totalTies;

                // Points
                team.PointsFor = importTeam.ptsFor;
                team.PointsAgainst = importTeam.ptsAgainst;
                team.NetPoints = importTeam.netPts;

                // Yardage
                team.OFF_TotalYards = importTeam.offTotalYds;
                team.OFF_RushYards = importTeam.offRushYds;
                team.OFF_PassYards = importTeam.offPassYds;
                team.DEF_TotalYards = importTeam.defTotalYds;
                team.DEF_RushYards = importTeam.defRushYds;
                team.DEF_PashYards = importTeam.defPassYds;

                // Details
                team.TurnoverDifferential = importTeam.tODiff;
                #endregion

                db.Teams.InsertOnSubmit(team);
                db.SubmitChanges();
            }
            
        }

        
        private void ImportPlayer(int leagueId, Models.ImportModels.ImportPlayer import)
        {
            // does this player already exist?
            if (db.Players.Any(m => m.LeagueID == leagueId && m.PlayerId == import.rosterId))
            {
                // this player already exists, so we need to update it
                var player = db.Players.Where(m => m.PlayerId == import.rosterId).Single();

                #region Assign Team Properties

                // Personal Info
                player.PlayerId = import.rosterId;
                player.Position = import.position;
                player.FirstName = import.firstName;
                player.LastName = import.lastName;
                player.Age = import.age;
                player.Height = import.height;
                player.Weight = import.weight;
                player.Overall = import.playerBestOvr;
                player.XP = import.experiencePoints;
                player.Number = import.jerseyNum;
                player.YearsPro = import.yearsPro;
                player.TeamId = import.teamId;
                player.Development = import.devTrait;
                player.IsFreeAgent = import.isFreeAgent;
                player.Scheme = import.scheme;
                player.Confidence = import.confRating;

                // Salary Info
                player.ContractSalary = import.contractSalary;
                player.ContractBonus = import.contractBonus;
                player.ContractLength = import.contractLength;
                player.ContractYearsLeft = import.contractYearsLeft;
                player.ContractCapHit = import.capHit;
                player.ContractReleasePenalty = import.capReleasePenalty;
                player.ContractReleaseNetSavings = import.capReleaseNetSavings;

                // General
                player.Awareness = import.awareRating;
                player.Speed = import.speedRating;
                player.Acceleration = import.accelRating;
                player.Strength = import.strengthRating;
                player.Catch = import.catchRating;
                player.Jump = import.jumpRating;
                player.KickReturn = import.kickRetRating;
                player.Stamina = import.staminaRating;
                player.Injury = import.injuryRating;
                player.Toughness = import.toughRating;
                player.Durability = import.durabilityGrade;

                // Offense
                player.Elusiveness = import.elusiveRating;
                player.Carry = import.carryRating;

                // QB
                player.ThrowPower = import.throwPowerRating;
                player.ThrowAccuracy = import.throwAccRating;
                player.ThrowAccuracyShort = import.throwAccShortRating;
                player.ThrowAccuracyMid = import.throwAccMidRating;
                player.ThrowAccuracyDeep = import.throwAccDeepRating;
                player.ThrowOnRun = import.throwOnRunRating;
                player.PlayAction = import.playActionRating;

                // O-Skill
                player.BallCarrierVision = import.bCVRating;
                player.Trucking = import.truckRating;
                player.JukeMove = import.jukeMoveRating;
                player.SpinMove = import.spinMoveRating;
                player.StiffArm = import.stiffArmRating;
                player.Release = import.releaseRating;
                player.RouteRunning = import.routeRunRating;
                player.CatchInTraffic = import.cITRating;
                player.SpectacularCatch = import.specCatchRating;

                // O-Line
                player.RunBlock = import.runBlockRating;
                player.PassBlock = import.passBlockRating;
                player.ImpactBlock = import.impactBlockRating;

                // Defense
                player.Tackle = import.tackleRating;
                player.HitPower = import.hitPowerRating;
                player.PlayRecognition = import.playRecRating;
                player.Pursuit = import.pursuitRating;

                // D-Line
                player.BlockShed = import.blockShedRating;
                player.PowerMoves = import.powerMovesRating;
                player.FinesseMoves = import.finesseMovesRating;

                // D-Skill
                player.ManCover = import.manCoverRating;
                player.ZoneCover = import.zoneCoverRating;
                player.PressCover = import.pressRating;

                // Kick/Punt
                player.KickPower = import.kickPowerRating;
                player.KickAccuracy = import.kickAccRating;

                // Traits ALL
                player.Trait_ALL_Predictable = import.predictTrait;
                player.Trait_ALL_Clutch = import.clutchTrait;
                player.Trait_ALL_Penalty = import.penaltyTrait;

                // Traits O Skill
                player.Trait_OFF_CoverBall = import.coverBallTrait;
                player.Trait_OFF_FightForYards = import.fightForYardsTrait;
                player.Trait_OFF_DropOpenPasses = import.dropOpenPassTrait;
                player.Trait_OFF_FeetInBounds = import.feetInBoundsTrait;
                player.Trait_OFF_PossesionCatch = import.posCatchTrait;
                player.Trait_OFF_AggressiveCatch = import.hPCatchTrait;
                player.Trait_OFF_CatchOnTheRun = import.yACCatchTrait;

                // Trait QB
                player.Trait_QB_ThrowBallAway = import.throwAwayTrait;
                player.Trait_QB_SensePressure = import.sensePressureTrait;
                player.Trait_QB_TightSpiral = import.tightSpiralTrait;
                player.Trait_QB_Style = import.qBStyleTrait;
                player.Trait_QB_ForcePass = import.forcePassTrait;

                // Trait Defense
                player.Trait_DEF_BullRush = import.dLBullRushTrait;
                player.Trait_DEF_SwimMove = import.dLSwimTrait;
                player.Trait_DEF_SpinMove = import.dLSpinTrait;
                player.Trait_DEF_HighMotor = import.highMotorTrait;
                player.Trait_DEF_BigHitter = import.bigHitTrait;
                player.Trait_DEF_StripBall = import.stripBallTrait;
                player.Trait_DEF_PlayBall = import.playBallTrait;

                #endregion

                // now save all changes
                db.SubmitChanges();
            }
            else
            {
                // this team does not exists, so create a new Team object, then insert it
                Madden.Player player = new Player();

                #region Assign Team Properties
                // IDs
                player.LeagueID = leagueId;
                
                // Personal Info
                player.PlayerId = import.rosterId;
                player.Position = import.position;
                player.FirstName = import.firstName;
                player.LastName = import.lastName;
                player.Age = import.age;
                player.Height = import.height;
                player.Weight = import.weight;
                player.Overall = import.playerBestOvr;
                player.XP = import.experiencePoints;
                player.Number = import.jerseyNum;
                player.YearsPro = import.yearsPro;
                player.TeamId = import.teamId;
                player.Development = import.devTrait;
                player.IsFreeAgent = import.isFreeAgent;
                player.Scheme = import.scheme;
                player.Confidence = import.confRating;

                // Salary Info
                player.ContractSalary = import.contractSalary;
                player.ContractBonus = import.contractBonus;
                player.ContractLength = import.contractLength;
                player.ContractYearsLeft = import.contractYearsLeft;
                player.ContractCapHit = import.capHit;
                player.ContractReleasePenalty = import.capReleasePenalty;
                player.ContractReleaseNetSavings = import.capReleaseNetSavings;

                // General
                player.Awareness = import.awareRating;
                player.Speed = import.speedRating;
                player.Acceleration = import.accelRating;
                player.Strength = import.strengthRating;
                player.Catch = import.catchRating;
                player.Jump = import.jumpRating;
                player.KickReturn = import.kickRetRating;
                player.Stamina = import.staminaRating;
                player.Injury = import.injuryRating;
                player.Toughness = import.toughRating;
                player.Durability = import.durabilityGrade;

                // Offense
                player.Elusiveness = import.elusiveRating;
                player.Carry = import.carryRating;

                // QB
                player.ThrowPower = import.throwPowerRating;
                player.ThrowAccuracy = import.throwAccRating;
                player.ThrowAccuracyShort = import.throwAccShortRating;
                player.ThrowAccuracyMid = import.throwAccMidRating;
                player.ThrowAccuracyDeep = import.throwAccDeepRating;
                player.ThrowOnRun = import.throwOnRunRating;
                player.PlayAction = import.playActionRating;

                // O-Skill
                player.BallCarrierVision = import.bCVRating;
                player.Trucking = import.truckRating;
                player.JukeMove = import.jukeMoveRating;
                player.SpinMove = import.spinMoveRating;
                player.StiffArm = import.stiffArmRating;
                player.Release = import.releaseRating;
                player.RouteRunning = import.routeRunRating;
                player.CatchInTraffic = import.cITRating;
                player.SpectacularCatch = import.specCatchRating;

                // O-Line
                player.RunBlock = import.runBlockRating;
                player.PassBlock = import.passBlockRating;
                player.ImpactBlock = import.impactBlockRating;

                // Defense
                player.Tackle = import.tackleRating;
                player.HitPower = import.hitPowerRating;
                player.PlayRecognition = import.playRecRating;
                player.Pursuit = import.pursuitRating;

                // D-Line
                player.BlockShed = import.blockShedRating;
                player.PowerMoves = import.powerMovesRating;
                player.FinesseMoves = import.finesseMovesRating;

                // D-Skill
                player.ManCover = import.manCoverRating;
                player.ZoneCover = import.zoneCoverRating;
                player.PressCover = import.pressRating;

                // Kick/Punt
                player.KickPower = import.kickPowerRating;
                player.KickAccuracy = import.kickAccRating;

                // Traits ALL
                player.Trait_ALL_Predictable = import.predictTrait;
                player.Trait_ALL_Clutch = import.clutchTrait;
                player.Trait_ALL_Penalty = import.penaltyTrait;

                // Traits O Skill
                player.Trait_OFF_CoverBall = import.coverBallTrait;
                player.Trait_OFF_FightForYards = import.fightForYardsTrait;
                player.Trait_OFF_DropOpenPasses = import.dropOpenPassTrait;
                player.Trait_OFF_FeetInBounds = import.feetInBoundsTrait;
                player.Trait_OFF_PossesionCatch = import.posCatchTrait;
                player.Trait_OFF_AggressiveCatch = import.hPCatchTrait;
                player.Trait_OFF_CatchOnTheRun = import.yACCatchTrait;

                // Trait QB
                player.Trait_QB_ThrowBallAway = import.throwAwayTrait;
                player.Trait_QB_SensePressure = import.sensePressureTrait;
                player.Trait_QB_TightSpiral = import.tightSpiralTrait;
                player.Trait_QB_Style = import.qBStyleTrait;
                player.Trait_QB_ForcePass = import.forcePassTrait;

                // Trait Defense
                player.Trait_DEF_BullRush = import.dLBullRushTrait;
                player.Trait_DEF_SwimMove = import.dLSwimTrait;
                player.Trait_DEF_SpinMove = import.dLSpinTrait;
                player.Trait_DEF_HighMotor = import.highMotorTrait;
                player.Trait_DEF_BigHitter = import.bigHitTrait;
                player.Trait_DEF_StripBall = import.stripBallTrait;
                player.Trait_DEF_PlayBall = import.playBallTrait;

                #endregion

                db.Players.InsertOnSubmit(player);
                db.SubmitChanges();
            }

        }


    }
}