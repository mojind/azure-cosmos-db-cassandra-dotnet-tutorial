using Cassandra;
using Cassandra.Mapping;
using System;

namespace CassandraTutorialSample
{
	public class PagingQueries
	{
		private ISession Session;
		private IMapper Mapper;

		public PagingQueries(ISession session)
		{
			Session = session;
			Mapper = new Mapper(Session);
		}

		public void Paging()
		{
			PagingWithoutMapper();
			PagingWithMapper();
		}

		private void PagingWithoutMapper()
		{
			try
			{
				Console.WriteLine("Paging Without Mapper with page size of 4: Trying");
				var ps = new SimpleStatement("SELECT * from race");
				// Disable automatic paging. Set page size of 4
				var statement = ps.SetAutoPage(false).SetPageSize(4);
				var rs = Session.Execute(statement);
				// Store the paging state
				var pagingState = rs.PagingState;

				// Later in time ...
				// Retrieve the following page of results.
				var statement2 = ps
				   .SetAutoPage(false)
				   .SetPagingState(pagingState);

				var rs2 = Session.Execute(statement2);

				// now we should get 4 results in rs2 with values of rank as 5,6,7,8

				int[] expectedRank = new int[4] { 5, 6, 7, 8 };
				int i = 0;
				foreach (var race in rs2)
				{
					if ((int)race["rank"] != expectedRank[i++])
					{
						throw new Exception("Value of Current Rank not as expected.");
					}

					//The enumerator will yield all the rows from Cassandra
					//Retrieving them in the back in blocks of 4.
				}

				Console.WriteLine("Paging Without Mapper with page size of 4: Worked Succesfully");
			}
			catch (Exception e)
			{
				Console.WriteLine("Paging Without Mapper with page size of 4: Failed" + e.ToString());
			}
		}

		private void PagingWithMapper()
		{
			try
			{
				Console.WriteLine("Paging With Mapper with page size of 4: Trying");

				IPage<Race> raceFirstPage = Mapper.FetchPage<Race>(Cql.New("Select * from race").WithOptions(opt => opt.SetPageSize(4)));

				IPage<Race> raceSecondPage = Mapper.FetchPage<Race>(pageSize: 4, pagingState: raceFirstPage.PagingState, query: "Select * from race", args: null);

				int[] expectedRank = new int[4] { 5, 6, 7, 8 };
				int i = 0;
				foreach (Race race in raceSecondPage)
				{
					if (race.rank != expectedRank[i++])
					{
						throw new Exception("Value of Current Rank not as expected.");
					}

					//The enumerator will yield all the rows from Cassandra
					//Retrieving them in the back in blocks of 4.
				}

				Console.WriteLine("Paging With Mapper with page size of 4: Worked Succesfully");
			}
			catch (Exception e)
			{
				Console.WriteLine("Paging With Mapper with page size of 4: Failed" + e.ToString());
			}
		}

	}
}
