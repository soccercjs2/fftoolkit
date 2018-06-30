using fftoolkit.DB.Model;
using fftoolkit.Logic.Classes;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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