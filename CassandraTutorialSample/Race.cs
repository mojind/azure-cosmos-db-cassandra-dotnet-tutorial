using System;
using System.Net;

namespace CassandraTutorialSample
{
	public class Race
	{
		public int race_year { get; set; }

		public String race_name { get; set; }

		public int rank { get; set; }

		public bool any_accident { get; set; }

		public long audiance_capacity { get; set; }

		public String cyclist_name { get; set; }

		public String location { get; set; }

        public IPAddress shop_ip { get; set; }

        public Guid transaction_id { get; set; }

        public Race(int race_year, String race_name, int rank, bool anyAccident, long audianceCapacity, String cyclistName, String location, IPAddress shopIp, Guid transactionId)
		{
			this.race_year = race_year;
			this.race_name = race_name;			
			this.rank = rank;
			this.any_accident = anyAccident;
			this.audiance_capacity = audianceCapacity;
			this.cyclist_name = cyclistName;
			this.location = location;
            this.shop_ip = shopIp;
            this.transaction_id = transaction_id;
		}

		public override string ToString()
		{
			return String.Format("Race [race_year='{0}', race_name = '{1}', rank = '{2}', any_accident = '{3}', " +
						"audiance_capacity = '{4}', cyclist_name = '{5}', location = '{6}', shop_ip = '{7}', transaction_id = '{8}' ]",
				race_year, race_name, rank, any_accident, audiance_capacity, cyclist_name, location, shop_ip, transaction_id);
		}
	}
}
