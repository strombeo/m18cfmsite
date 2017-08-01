using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Madden.Controllers
{
    public class MaddenController : Controller
    {
        // create a db context
        private MaddenDataContext db = new MaddenDataContext();

        // GET: Madden
        public ActionResult Index()
        {
            return View();
        }

        // GET: Madden/Leagues
        public ActionResult Leagues()
        {
            var leagues = db.Leagues.ToList();
            return View(leagues);
        }

        // GET: Madden/Teams/{leagueId}
        public ActionResult Teams(int? leagueId)
        {
            ViewBag.LeagueId = leagueId;
            var teams = db.Teams.Where(m => m.LeagueID == leagueId).ToList();
            return View(teams);
        }

        // GET Madden/TeamPlayers/{leagueId, teamId}
        public ActionResult TeamPlayers(int? leagueId, int? teamId)
        {
            ViewBag.LeagueId = leagueId;
            var players = db.Players.Where(m => m.LeagueID == leagueId && m.TeamId == teamId && m.ContractYearsLeft > 0).ToList();
            return View(players);
        }

        // GET Madden/LeaguePlayers/{leagueId}
        public ActionResult LeaguePlayers(int? leagueId, string position, string sortBy, string currentSortDirection)
        {
            //reverse the sort direction (default to desc)
            if(currentSortDirection == "DESC")
            {
                ViewBag.SortDirection = "ASC";
            }
            else
            {
                ViewBag.SortDirection = "DESC";
            }

            
            ViewBag.LeagueId = leagueId;
            ViewBag.Position = position;
            var players = db.Players.Where(m => m.LeagueID == leagueId && m.Position == position);

            switch(sortBy)
            {

                #region General
                case "FirstName":
                    if (ViewBag.SortDirection == "ASC")
                    { players = players.OrderBy(m => m.FirstName); }
                    else
                    { players = players.OrderByDescending(m => m.FirstName); }
                    break;
                case "LastName":
                    if (ViewBag.SortDirection == "ASC")
                    { players = players.OrderBy(m => m.LastName); }
                    else
                    { players = players.OrderByDescending(m => m.LastName); }
                    break;
                case "Team":
                    if (ViewBag.SortDirection == "ASC")
                    { players = players.OrderBy(m => m.TeamId); }
                    else
                    { players = players.OrderByDescending(m => m.TeamId); }
                    break;
                case "Age":
                    if (ViewBag.SortDirection == "ASC")
                    { players = players.OrderBy(m => m.Age); }
                    else
                    { players = players.OrderByDescending(m => m.Age); }
                    break;
                case "Overall":
                    if(ViewBag.SortDirection == "ASC")
                    { players = players.OrderBy(m => m.Overall); } else
                    { players = players.OrderByDescending(m => m.Overall); }
                    break;
                case "Development":
                    if (ViewBag.SortDirection == "ASC")
                    { players = players.OrderBy(m => m.Development); }
                    else
                    { players = players.OrderByDescending(m => m.Development); }
                    break;
                case "ContractYearsLeft":
                    if (ViewBag.SortDirection == "ASC")
                    { players = players.OrderBy(m => m.ContractYearsLeft); }
                    else
                    { players = players.OrderByDescending(m => m.ContractYearsLeft); }
                    break;
                case "Strength":
                    if (ViewBag.SortDirection == "ASC")
                    { players = players.OrderBy(m => m.Strength); }
                    else
                    { players = players.OrderByDescending(m => m.Strength); }
                    break;
                case "Agility":
                    if (ViewBag.SortDirection == "ASC")
                    { players = players.OrderBy(m => m.Agility); }
                    else
                    { players = players.OrderByDescending(m => m.Agility); }
                    break;
                case "Speed":
                    if (ViewBag.SortDirection == "ASC")
                    { players = players.OrderBy(m => m.Speed); }
                    else
                    { players = players.OrderByDescending(m => m.Speed); }
                    break;
                case "Acceleration":
                    if (ViewBag.SortDirection == "ASC")
                    { players = players.OrderBy(m => m.Acceleration); }
                    else
                    { players = players.OrderByDescending(m => m.Acceleration); }
                    break;
                case "Awareness":
                    if (ViewBag.SortDirection == "ASC")
                    { players = players.OrderBy(m => m.Awareness); }
                    else
                    { players = players.OrderByDescending(m => m.Awareness); }
                    break;
                case "Catch":
                    if (ViewBag.SortDirection == "ASC")
                    { players = players.OrderBy(m => m.Catch); }
                    else
                    { players = players.OrderByDescending(m => m.Catch); }
                    break;
                #endregion

                #region Offense
                case "Carry":
                    if (ViewBag.SortDirection == "ASC")
                    { players = players.OrderBy(m => m.Carry); }
                    else
                    { players = players.OrderByDescending(m => m.Carry); }
                    break;
                case "ThrowPower":
                    if (ViewBag.SortDirection == "ASC")
                    { players = players.OrderBy(m => m.ThrowPower); }
                    else
                    { players = players.OrderByDescending(m => m.ThrowPower); }
                    break;
                case "PassBlock":
                    if (ViewBag.SortDirection == "ASC")
                    { players = players.OrderBy(m => m.PassBlock); }
                    else
                    { players = players.OrderByDescending(m => m.PassBlock); }
                    break;
                case "RunBlock":
                    if (ViewBag.SortDirection == "ASC")
                    { players = players.OrderBy(m => m.RunBlock); }
                    else
                    { players = players.OrderByDescending(m => m.RunBlock); }
                    break;
                case "Jump":
                    if (ViewBag.SortDirection == "ASC")
                    { players = players.OrderBy(m => m.Jump); }
                    else
                    { players = players.OrderByDescending(m => m.Jump); }
                    break;
                case "Trucking":
                    if (ViewBag.SortDirection == "ASC")
                    { players = players.OrderBy(m => m.Trucking); }
                    else
                    { players = players.OrderByDescending(m => m.Trucking); }
                    break;
                case "Elusiveness":
                    if (ViewBag.SortDirection == "ASC")
                    { players = players.OrderBy(m => m.Elusiveness); }
                    else
                    { players = players.OrderByDescending(m => m.Elusiveness); }
                    break;
                case "BallCarrierVision":
                    if (ViewBag.SortDirection == "ASC")
                    { players = players.OrderBy(m => m.BallCarrierVision); }
                    else
                    { players = players.OrderByDescending(m => m.BallCarrierVision); }
                    break;
                case "StiffArm":
                    if (ViewBag.SortDirection == "ASC")
                    { players = players.OrderBy(m => m.StiffArm); }
                    else
                    { players = players.OrderByDescending(m => m.StiffArm); }
                    break;
                case "SpinMove":
                    if (ViewBag.SortDirection == "ASC")
                    { players = players.OrderBy(m => m.SpinMove); }
                    else
                    { players = players.OrderByDescending(m => m.SpinMove); }
                    break;
                case "JukeMove":
                    if (ViewBag.SortDirection == "ASC")
                    { players = players.OrderBy(m => m.JukeMove); }
                    else
                    { players = players.OrderByDescending(m => m.JukeMove); }
                    break;
                case "ImpactBlock":
                    if (ViewBag.SortDirection == "ASC")
                    { players = players.OrderBy(m => m.ImpactBlock); }
                    else
                    { players = players.OrderByDescending(m => m.ImpactBlock); }
                    break;
                case "SpectacularCatch":
                    if (ViewBag.SortDirection == "ASC")
                    { players = players.OrderBy(m => m.SpectacularCatch); }
                    else
                    { players = players.OrderByDescending(m => m.SpectacularCatch); }
                    break;
                case "CatchInTraffic":
                    if (ViewBag.SortDirection == "ASC")
                    { players = players.OrderBy(m => m.CatchInTraffic); }
                    else
                    { players = players.OrderByDescending(m => m.CatchInTraffic); }
                    break;
                case "RouteRunning":
                    if (ViewBag.SortDirection == "ASC")
                    { players = players.OrderBy(m => m.RouteRunning); }
                    else
                    { players = players.OrderByDescending(m => m.RouteRunning); }
                    break;
                case "Release":
                    if (ViewBag.SortDirection == "ASC")
                    { players = players.OrderBy(m => m.Release); }
                    else
                    { players = players.OrderByDescending(m => m.Release); }
                    break;
                case "ThrowAccuracyShort":
                    if (ViewBag.SortDirection == "ASC")
                    { players = players.OrderBy(m => m.ThrowAccuracyShort); }
                    else
                    { players = players.OrderByDescending(m => m.ThrowAccuracyShort); }
                    break;
                case "ThrowAccuracyMid":
                    if (ViewBag.SortDirection == "ASC")
                    { players = players.OrderBy(m => m.ThrowAccuracyMid); }
                    else
                    { players = players.OrderByDescending(m => m.ThrowAccuracyMid); }
                    break;
                case "ThrowAccuracyDeep":
                    if (ViewBag.SortDirection == "ASC")
                    { players = players.OrderBy(m => m.ThrowAccuracyDeep); }
                    else
                    { players = players.OrderByDescending(m => m.ThrowAccuracyDeep); }
                    break;
                case "ThrowOnRun":
                    if (ViewBag.SortDirection == "ASC")
                    { players = players.OrderBy(m => m.ThrowOnRun); }
                    else
                    { players = players.OrderByDescending(m => m.ThrowOnRun); }
                    break;
                case "PlayAction":
                    if (ViewBag.SortDirection == "ASC")
                    { players = players.OrderBy(m => m.PlayAction); }
                    else
                    { players = players.OrderByDescending(m => m.PlayAction); }
                    break;
                    #endregion

                #region Defense

                    #endregion

            }

            List<Madden.Player> playerList = players.ToList();
            return View(playerList);
        }
    }
}