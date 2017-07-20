using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Webscrapper
{
	public class VdmScrapper: IScrapper
	{
		public string Name { get; }

		const string address = "http://www.viedemerde.fr/";

		public static long LastArticleIndex = 0;

		public VdmScrapper()
		{
			Name = "VDM.com";
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

			var match = Regex.Match(page, @"card-[0-9]+").Value;

			if (string.IsNullOrWhiteSpace(match))
			{
				return false;
			}

			tempMax = Convert.ToInt64(Regex.Match(match, @"\d+").Value);

			if (tempMax != LastArticleIndex)
			{
				LastArticleIndex = tempMax;
				return true;
			}

			return false;
		}
	}
}
