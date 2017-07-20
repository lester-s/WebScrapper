using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using Windows.UI.Notifications;

namespace Webscrapper
{
	class Program
	{
		public static long LastArticleIndex = 0;
		static void Main(string[] args)
		{
			const string address = "https://www.koreus.com/modules/news/";

			long tempMax = 0;
			var page = string.Empty;

			using (var client = new WebClient())
			{
				page = client.DownloadString(address);
			}

			if (string.IsNullOrWhiteSpace(page))
			{
				return;
			}

			var matches = Regex.Matches(page, @"article[0-9]+\.html").Cast<Match>().Select(m => m.Value).ToList();

			if (matches.Count <= 0)
			{
				return;
			}

			var articles = matches.Select(article => Regex.Match(article, @"\d+").Value).Select(index => Convert.ToInt64(index)).ToList();

			articles.Sort();
			tempMax = articles.Last();

			if (tempMax > LastArticleIndex)
			{
				LastArticleIndex = tempMax;
				var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);

				var stringElements = toastXml.GetElementsByTagName("text");

				stringElements[0].AppendChild(toastXml.CreateTextNode($"There is a new article in {address}"));


				var toast = new ToastNotification(toastXml);

				ToastNotificationManager.CreateToastNotifier("webSccrapper").Show(toast);

				Console.WriteLine($"New article in {address}");
			}

			Thread.Sleep(5000);
			Main(args);
		}
	}
}
