using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace Webscrapper
{
	public class KoreusScrapper: IScrapper
	{
		public string Name { get; }

		const string address = "https://www.koreus.com/modules/news/";

		public static long LastArticleIndex = 0;

		public KoreusScrapper()
		{
			Name = "Koreus.com";
		}
		public bool Check()
		{

			long tempMax = 0;
			var page = string.Empty;

			using (var client = new WebClient())
			{
				page = client.DownloadString(address);
			}

			if (string.IsNullOrWhiteSpace(page))
			{
				return false;
			}

			var matches = Regex.Matches(page, @"article[0-9]+\.html").Cast<Match>().Select(m => m.Value).ToList();

			if (matches.Count <= 0)
			{
				return false;
			}

			var articles = matches.Select(article => Regex.Match(article, @"\d+").Value).Select(index => Convert.ToInt64(index)).ToList();

			articles.Sort();
			tempMax = articles.Last();
			if (tempMax > LastArticleIndex)
			{
				LastArticleIndex = tempMax;
				return true;
			}

			return false;
		}
	}
}
