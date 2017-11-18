using Cassandra;
using Cassandra.Mapping;
using System;
using System.Net;

namespace CassandraTutorialSample
{
    public class InsertQueries
	{
		private ISession Session;

		public InsertQueries(ISession session)
		{
			this.Session = session;
		}

		public void InsertRecords()
		{
			// Inserting Records
			try
			{
				Console.WriteLine("Create Race Records");
				Console.WriteLine("-----------------");
				InsertData();
				Console.WriteLine("Records have been Successfully created");
			}
			catch (Exception e)
			{
				Console.WriteLine("Insertion failed. Stack Trace:" + e.ToString());
			}
		}

		private void InsertData()
		{
			InsertData(2012, "GPX", 10);
			InsertData(2012, "GPY", 10);
			InsertData(2013, "GPX", 10);
			InsertData(2013, "GPY", 10);
		}

		private  void InsertData(int race_year, String race_name, int count_insert)
		{
			Console.WriteLine("Inserting Records for race_year: " + race_year + " race_name: " + race_name);
			int rank_start = 1;
			Random rand = new Random();


			int inserted;
			for (inserted = 0; inserted < count_insert; inserted++)
			{
				Race race = new Race(race_year, race_name, rank_start++,
					    false,
						(long)((inserted * 100) + (rand.Next(1, 100) + 1)) ,
						Utilities.RandomEnum<Utilities.CyclistName>().ToString(),
						Utilities.RandomEnum<Utilities.Location>().ToString(),
                        Utilities.GetRandomIPAddress(),
                        new Guid());

				InsertData(race);
			}

			Console.WriteLine("Insertion for Records for race_year: " + race_year + " race_name: " + race_name + " done");
		}

		private void InsertData(Race race)
		{
			IMapper mapper = new Mapper(Session);

			mapper.Insert<Race>(race);

			/* Alternatively One can directly execute insert queries via Named parameters example
			string insertCQLQuery = "insert into race(race_year, race_name, rank, any_accident, audiance_capacity, cyclist_name, location) values (:a, :b, :c, :d, :e, :f, :g)";
			var statement = Session.Prepare(insertCQLQuery);
			Session.Execute(statement.Bind(new { a = race.race_year, b = race.race_name, c = race.rank, d = race.any_accident, e = race.audiance_capacity, f = race.cyclist_name, g = race.location })); */
		}

        
	}
}
