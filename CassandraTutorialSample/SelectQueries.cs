using Cassandra;
using Cassandra.Mapping;
using System;

namespace CassandraTutorialSample
{
	public class SelectQueries
	{
		private ISession Session;
		private IMapper Mapper;
		private int recordCount;

		public SelectQueries(ISession session)
		{
			Session = session;
			Mapper = new Mapper(Session);
		}

		public void ShowRecords()
		{
			ShowAllRecords();
			ShowRecordsWithEqualityClause();
			ShowRecordsWithGreaterThanLessThanClause();
			ShowRecordsUsingLimitClause();
			ShowRecordsUsingOrderByDescClause();
		}

		private void ShowAllRecords()
		{
			// Showing All Records
			try
			{
				Console.WriteLine("Showing All records:");
				Console.WriteLine("-------------------------------");
				recordCount = 0;
				foreach (Race race in Mapper.Fetch<Race>("Select * from race"))
				{
					recordCount++;
					Console.WriteLine(race);
				}
				if (recordCount != 40)
				{
					throw new Exception("We got only " + recordCount + " Records. Expected records count: 40");
				}

				Console.WriteLine("All records shown Successfully");
			}
			catch (Exception e)
			{
				Console.WriteLine("Select all records failed Stack Trace: " + e.ToString());
			}
		}

		private void ShowRecordsWithEqualityClause()
		{
			// Selecting with Where Clause '=' and No Partition Key Specified (without ALLOW FILTERING)

			try
			{
				Console.WriteLine("Selecting with Where Clause '=' and No Partition Keys Specified (without ALLOW FILTERING): Trying");
				recordCount = 0;
				Mapper.Fetch<Race>("Select * from race where rank > 3");

				throw new Exception("Selecting with Where Clause '=' and No Partition Keys Specified (without ALLOW FILTERING): Failed");

			}
			catch (Exception e)
			{
				if (e.ToString().Contains("use ALLOW FILTERING"))
				{
					Console.WriteLine("Selecting with Where Clause '=' and No Partition Keys Specified(without ALLOW FILTERING): Worked Successfully as 'ALLOW FILTERING' was expected");
				}
				else
				{
					Console.WriteLine("Selecting with Where Clause '=' and No Partition Keys Specified(without ALLOW FILTERING): Failed" + e.ToString());
				}
			}


			// Selecting with Where Clause '=' and No Partition Key Specified

			try
			{
				Console.WriteLine("Selecting with Where Clause '=' and No Partition Keys Specified: Trying");
				recordCount = 0;
				foreach (Race race in Mapper.Fetch<Race>("Select * from race where rank > 3 ALLOW FILTERING"))
				{
					recordCount++;
					Console.WriteLine(race);
				}

				if (recordCount != 28)
				{
					throw new Exception("We got only " + recordCount + " Records. Expected records count: 28");
				}

				Console.WriteLine("Selecting with Where Clause '=' and No Partition Keys Specified: Worked Successfully");
			}
			catch (Exception e)
			{
				Console.WriteLine("Selecting with Where Clause '=' and No Partition Keys Specified: Failed " + e.ToString());
			}

			// Selecting with Where Clause '=' and Partial Partition Key Specified

			try
			{
				Console.WriteLine("Selecting with Where Clause '=' and Partial Partition Keys Specified: Trying");
				recordCount = 0;
				foreach (Race race in Mapper.Fetch<Race>("Select * from race where race_year = 2012 ALLOW FILTERING"))
				{
					recordCount++;
					Console.WriteLine(race);
				}
				if (recordCount != 20)
				{
					throw new Exception("We got only " + recordCount + " Records. Expected records count: 20");
				}
				Console.WriteLine("Selecting with Where Clause '=' and Partial Partition Keys Specified: Worked Successfully");
			}
			catch (Exception e)
			{
				Console.WriteLine("Selecting with Where Clause '=' and Partial Partition Keys Specified: Failed " + e.ToString());
			}


			// Selecting with Where Clause '=' and All Partition Key Specified  (AND Clause)

			try
			{
				Console.WriteLine("Selecting with Where Clause '=' and All Partition Keys Specified (AND Clause): Trying");
				recordCount = 0;
				foreach (Race race in Mapper.Fetch<Race>("Select * from race where race_year = 2012 and race_name  = 'GPX'"))
				{
					recordCount++;
					Console.WriteLine(race);
				}
				if (recordCount != 10)
				{
					throw new Exception("We got only " + recordCount + " Records. Expected records count: 20");
				}

				Console.WriteLine("Selecting with Where Clause '='and All Partition Keys Specified (AND Clause): Worked Successfully");
			}
			catch (Exception e)
			{
				Console.WriteLine("Selecting with Where Clause '=' and All Partition Keys Specified (AND Clause): Failed " + e.ToString());
			}
		}

		private void ShowRecordsWithGreaterThanLessThanClause()
		{
			// Selecting with Where Clause '><' and All Partition Key Specified with '=' clause)
			try
			{
				Console.WriteLine("Selecting with Where Clause '><' and All Partition Keys Specified with '=' clause: Trying");
				recordCount = 0;
				foreach (Race race in Mapper.Fetch<Race>("Select * from race where race_year = 2012 and race_name  = 'GPX' and  audiance_capacity >200 and audiance_capacity < 800"))
				{
					recordCount++;
					Console.WriteLine(race);
				}
				if (recordCount != 6)
				{
					throw new Exception("We got only " + recordCount + " Records. Expected records count: 6");
				}
				Console.WriteLine("Selecting with Where Clause '><' and All Partition Keys Specified with '=' clause: Worked Successfully");
			}
			catch (Exception e)
			{

				Console.WriteLine("Selecting with Where Clause '><' and All Partition Keys Specified: Failed " + e.ToString());
			}

		}

		private void ShowRecordsUsingLimitClause()
		{
			// Selecting with Where Clause '=' and All Partition Key Specified  (AND Clause) and LIMIT Clause

			try
			{
				Console.WriteLine("Selecting with Where Clause '=' and All Partition Keys Specified (AND Clause) and LIMIT Clause: Trying");
				recordCount = 0;
				foreach (Race race in Mapper.Fetch<Race>("Select * from race where race_year = 2012 and race_name  = 'GPX' limit 5"))
				{
					recordCount++;
					Console.WriteLine(race);
				}
				if (recordCount != 5)
				{
					throw new Exception("We got only " + recordCount + " Records. Expected records count: 5");
				}

				Console.WriteLine("Selecting with Where Clause '='and All Partition Keys Specified (AND Clause) and LIMIT Clause: Worked Successfully");
			}
			catch (Exception e)
			{
				Console.WriteLine("Selecting with Where Clause '=' and All Partition Keys Specified (AND Clause) and LIMIT Clause: Failed " + e.ToString());
			}
		}

		private void ShowRecordsUsingOrderByDescClause()
		{
			try
			{
				// Selecting with Where Clause '=' and All Partition Key Specified  (AND Clause) + Ordering on Clustering key ASC
				Console.WriteLine("Selecting with Where Clause '=' and All Partition Keys Specified (AND Clause) + Ordering on Clustering key DESC: Trying");

				long previousValue = long.MaxValue;
				foreach (Race race in Mapper.Fetch<Race>("Select * from race where race_year = 2012 and race_name = 'GPX' order by rank desc "))
				{
					long currentValue = race.audiance_capacity;
					if (currentValue > previousValue)
					{
						throw new Exception(String.Format("Ordering on Rank Desc did not work. Previous Value: {0} , Current Value: {1}", previousValue, currentValue));
					}

					previousValue = currentValue;
					Console.WriteLine(race);
				}
				Console.WriteLine("Selecting with Where Clause '='and All Partition Keys Specified (AND Clause) + Ordering on Clustering key DESC: Worked Successfully");
			}
			catch (Exception e)
			{
				Console.WriteLine("Selecting with Where Clause '=' and All Partition Keys Specified (OR Clause)  + Ordering on Clustering key DESC: Failed" +
						". Stack Trace: " + e.ToString());
			}
		}
	}
}

