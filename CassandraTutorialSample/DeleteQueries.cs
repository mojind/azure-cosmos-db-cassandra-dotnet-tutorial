using Cassandra;
using Cassandra.Mapping;
using System;

namespace CassandraTutorialSample
{
	public class DeleteQueries
	{
		private ISession Session;
		private IMapper Mapper;

		public DeleteQueries(ISession session)
		{
			Session = session;
			Mapper = new Mapper(Session);
		}

		public void DeleteRecords()
		{
			DeleteWithPartitionParitionKey();
			DeleteWithCompletePartitionKey();
		}

		private void DeleteWithPartitionParitionKey()
		{
			// Deleting Records with only partition key Specified (No Mapper)
			try
			{
				Console.WriteLine("Deleting Records With Partial Partition Key (No Mapper): Trying");

				Session.Execute("Delete from race where race_name = 'GPX'");

				throw new Exception("Deleting Records on Partial primary Key  (No Mapper) shoud have thrown Exception. Deleting Records on Partial Partition Key: Failed");
			}

			catch (Exception e)
			{
				if (e.ToString().Contains("Some partition key parts are missing: race_year"))
				{
					Console.WriteLine("Deleting Records on Partial Partition Key: Worked Successfully as complete Partition key was expected.");
				}
				else
				{

					Console.WriteLine(e.ToString());
				}
			}
		}

		private void DeleteWithCompletePartitionKey()
		{
			// Deleting Records with Complete partition key (No Mapper) Specified
			try
			{
				Console.WriteLine("Deleting Records with Complete partition key Specified (No Mapper): Trying");

				Session.Execute("Delete from race where race_name = 'GPX' and race_year = 2012");


				int recordCount = 0;
				foreach (Race race in Mapper.Fetch<Race>("Select * from race where race_name=? and race_year=?", "GPX", 2012))
				{
					recordCount++;
				}

				if (recordCount != 0)
				{
					throw new Exception("We got " + recordCount + " Records. Expected records count: 0");
				}

				Console.WriteLine("Deleting Records with Complete partition key Specified (No Mapper): Worked Successfully");
			}

			catch (Exception e)
			{

				Console.WriteLine("Deleting Records with Complete partition key (No Mapper) Failed:" + e.ToString());
			}


			// Deleting Records with Complete partition key (Mapper) Specified
			try
			{
				Console.WriteLine("Deleting Records with Complete partition key Specified (Mapper): Trying");

				Mapper.Delete<Race>("where race_name=? and race_year=?", "GPX", 2013);

				int recordCount = 0;
				foreach (Race race in Mapper.Fetch<Race>("Select * from race where race_name=? and race_year=?", "GPX", 2013))
				{
					recordCount++;
				}

				if (recordCount != 0)
				{
					throw new Exception("We got " + recordCount + " Records. Expected records count: 0");
				}

				Console.WriteLine("Deleting Records with Complete partition key Specified (Mapper): Worked Successfully");
			}

			catch (Exception e)
			{

				Console.WriteLine("Deleting Records with Complete partition key (Mapper) Failed:" + e.ToString());
			}
		}
	}
}
