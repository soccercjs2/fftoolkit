using fftoolkit.DB.Models;
using fftoolkit.Logic.Classes;
using HtmlAgilityPack;
using System.Collections.Generic;

namespace fftoolkit.Logic.HostParsers
{
    public interface IHostParser
    {
        //string GetLoginUrl();

        //string GetPostData(string username, string password);

        List<Team> ParseLeague(HtmlDocument document, League league);

        List<Player> ParseTeam(HtmlDocument document);
    }
}