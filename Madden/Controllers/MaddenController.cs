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
        public ActionResult LeaguePlayers(int? leagueId, string position)
        {
            ViewBag.LeagueId = leagueId;
            var players = db.Players.Where(m => m.LeagueID == leagueId && m.Position == position).ToList();
            return View(players);
        }
    }
}