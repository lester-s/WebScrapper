using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using Windows.UI.Notifications;

namespace Webscrapper
{
	class Program
	{
		private static readonly List<IScrapper> Scrappers = new List<IScrapper>();
		
		static void Main(string[] args)
		{
			Init();
			foreach (var scrapper in Program.Scrappers)
			{
				if (scrapper.Check())
				{
					SendToastNotification(scrapper.Name);

				}
			}

			Thread.Sleep(5000);
			Main(args);
		}

		private static void Init()
		{
			if (IsInitialized)
			{
				return;
			}

			Scrappers.Add(new KoreusScrapper());
			Scrappers.Add(new SpionScrapper());
			Scrappers.Add(new VdmScrapper());
			IsInitialized = true;
		}

		public static bool IsInitialized = false;

		private static void SendToastNotification(string siteName)
		{
			var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);

			var stringElements = toastXml.GetElementsByTagName("text");

			stringElements[0].AppendChild(toastXml.CreateTextNode($"There is a new article in {siteName}"));


			var toast = new ToastNotification(toastXml);

			ToastNotificationManager.CreateToastNotifier("webSccrapper").Show(toast);

			Console.WriteLine($"{DateTime.Now} New article in {siteName}");
		}
	}
}
