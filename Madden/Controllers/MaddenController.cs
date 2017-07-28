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
            var teams = db.Teams.Where(m => m.LeagueID == leagueId).ToList();
            return View(teams);
        }

        // GET Madden/Players/{teamId}
        public ActionResult Players(int? teamId)
        {
            var players = db.Players.Where(m => m.TeamId == teamId && m.ContractYearsLeft > 0).ToList();
            return View(players);
        }

        // GET Madden/Players2/{teamId}
        public ActionResult Players2(int? teamId)
        {
            ViewBag.TeamName = db.Teams.Where(m => m.TeamId == teamId).Select(m => m.Name).FirstOrDefault();
            var players = db.Players.Where(m => m.TeamId == teamId && m.ContractYearsLeft > 0).ToList();
            return View(players);
        }
    }
}