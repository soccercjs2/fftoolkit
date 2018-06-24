using fftoolkit.DB.Model;
using fftoolkit.Logic.Scrapers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fftoolkit.Logic.Managers
{
    public class ScraperManager
    {
        public ScraperManager() { }

        public List<Player> ScrapeFantasyPros(int week)
        {
            FantasyProsScraper fantasyProsScraper = new FantasyProsScraper();
            List<Player> projections = new List<Player>();

            projections.AddRange(fantasyProsScraper.Scrape("QB", week));
            projections.AddRange(fantasyProsScraper.Scrape("RB", week));
            projections.AddRange(fantasyProsScraper.Scrape("WR", week));
            projections.AddRange(fantasyProsScraper.Scrape("TE", week));

            return projections;
        }

        public List<Player> ScrapeNfl(int year, int week)
        {
            NflScraper nflScraper = new NflScraper();
            return nflScraper.Scrape(year, week);
        }
    }
}
