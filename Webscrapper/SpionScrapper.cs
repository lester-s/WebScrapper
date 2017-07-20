using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Webscrapper
{
	public class SpionScrapper:IScrapper
	{
		public string Name { get; }
		const string address = "https://www.spi0n.com/";
		public static long LastArticleIndex = 0;

		public SpionScrapper()
		{
			Name = "Spion.com";
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

			var matches = Regex.Matches(page, @"post-[0-9]+").Cast<Match>().Select(m => m.Value).ToList();

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
