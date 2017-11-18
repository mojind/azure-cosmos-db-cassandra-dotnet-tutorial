using Cassandra;
using Cassandra.Mapping;
using System;

namespace CassandraTutorialSample
{
	public class UpdateQueries
	{
		private ISession Session;
		private IMapper Mapper;
		private int recordCount;

		public UpdateQueries(ISession session)
		{
			Session = session;
			Mapper = new Mapper(Session);
		}

		public void Update()
		{
			UpdateWithoutEntirePrimaryKey();
			UpdateWithEntirePrimaryKey();
		}

		private void UpdateWithoutEntirePrimaryKey()
		{
			// Update with Partial Primary Key Specified (No Mapper)
			try
			{
				Console.WriteLine("Update With Partial PrimaryKey (No Mapper): Trying");

				Session.Execute("Update race set cyclist_name = 'Kanna' where race_name = 'GPX' and race_year = 2012");

				throw new Exception("Update on Partial primary Key shoud have thrown Exception. Update With Partial PrimaryKey: Failed");
			}
			catch (Exception e)
			{
				if (e.ToString().Contains("Some cluster key parts are missing: rank"))
				{
					Console.WriteLine("Update With Partial PrimaryKey: Worked Successfully as Clustering Key was also expected.");
				}
				else
				{
					Console.WriteLine(e.ToString());
				}
			}
		}

		private void UpdateWithEntirePrimaryKey()
		{
			// Update with Entire Primary Key Specified  (No Mapper)

			try
			{
				Console.WriteLine("Update With Entire PrimaryKey (No Mapper) : Trying");

				Session.Execute("Update race set cyclist_name = 'IvanH' where race_name = 'GPX' and race_year = 2012 and rank = 2");

				Race race = Mapper.First<Race>("Select * from race where race_name = ? and race_year = ? and rank = ?", "GPX", 2012, 2);

				if (race.cyclist_name.Equals(Utilities.CyclistName.IvanH.ToString()))
				{
					Console.WriteLine("Update With Entire PrimaryKey: Worked SuccessFully");
				}
				else
				{
					throw new Exception("Update on Entire primary Key (No Mapper) Failed as data recieved on select query is different");
				}			
			}
			catch (Exception e)
			{
				Console.WriteLine("Update With Entire PrimaryKey (No Mapper): Failed: " + e.ToString());
			}

			// Update with Entire Primary Key Specified ( Mapper)

			try
			{
				Console.WriteLine("Update With Entire PrimaryKey (Mapper) : Trying");

				Mapper.Update<Race>("set cyclist_name = ? where race_name = ? and race_year = ? and rank = ?", Utilities.CyclistName.IvanH.ToString(), "GPX", 2012, 3);

				Race race = Mapper.First<Race>("Select * from race where race_name = ? and race_year = ? and rank = ?", "GPX", 2012, 3);

				if (race.cyclist_name.Equals(Utilities.CyclistName.IvanH.ToString()))
				{
					Console.WriteLine("Update With Entire PrimaryKey: Worked SuccessFully");
				}
				else
				{
					throw new Exception("Update on Entire primary Key Failed as data recieved on select query is different");
				}				
			}
			catch (Exception e)
			{
				Console.WriteLine("Update With Entire PrimaryKey ( Mapper): Failed: " + e.ToString());
			}
		}
	}
}
