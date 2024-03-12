using LocalEyesTipApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LocalEyesTipApp.DataServices
{
	class RSSFeedDataService
	{
		private readonly HttpClient _httpClient;
		private static readonly string _baseAddress = "https://localeyes.dk/feed/";

		public RSSFeedDataService()
		{
			_httpClient = new HttpClient()
			{
				BaseAddress = new Uri(_baseAddress)
			};
		}

		public List<RSSFeedNewsModel> GetFeedDataAsync()
		{
			List<RSSFeedNewsModel> newsList = new ();

			if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
			{
				try
				{
					using var reader = XmlReader.Create(_baseAddress);
					var feed = SyndicationFeed.Load(reader);

					foreach (var item in feed.Items)
					{
						var article = new RSSFeedNewsModel()
						{
							Title = item.Title.ToString(),
							Description = item.Content.ToString(),
							Published = item.PublishDate.DateTime,
						};
					}

					return newsList;
				}
				catch (Exception ex)
				{
					return newsList;
				}
			}

			return newsList;
		}
	}
}
