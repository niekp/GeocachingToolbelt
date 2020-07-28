using GeocachingToolbelt.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GeocachingToolbelt.Utils {
	public class Geocoder {
		private readonly IConfiguration configuration;

		public Geocoder(IConfiguration configuration) {
			this.configuration = configuration;
		}

		public async Task<GeocodeResult> Search(string address) {
			string apikey = configuration.GetValue<string>("bingApiKey");
			
			string baseURL = $"http://dev.virtualearth.net/REST/v1/Locations/{address}?maxResults=10&key={apikey}";
			
			try {

				using HttpClient client = new HttpClient();
				using HttpResponseMessage res = await client.GetAsync(baseURL);
				using HttpContent content = res.Content;

				string data = await content.ReadAsStringAsync();
				if (data != null) {
					var dataObj = JObject.Parse(data);

					var result = dataObj["resourceSets"]?.FirstOrDefault()?["resources"]?.FirstOrDefault();
					if (result != null) {

						var coords = result["point"]["coordinates"];
						Coordinate coordinate = null;
						if (coords != null) {
							if (double.TryParse(coords[0].ToString(), out double lat) && double.TryParse(coords[1].ToString(), out double lon)) {
								coordinate = new Coordinate(lat, lon);
							}
						}

						return new GeocodeResult() {
							Coordinate = coordinate,
							Name = result["name"].ToString()
						};
					}
				} else {
					Console.WriteLine("Data is null!");
				}
			} catch (Exception exception) {
				Console.WriteLine(exception);
			}

			return null;

		}
	}
}
