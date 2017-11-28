---
services: cosmos-db
platforms: dotnet
author: mojind
---

# Developing a Dotnet app with Cassandra API using Azure Cosmos DB
Azure Cosmos DB is Microsoft's globally distributed multi-model database service. You can quickly create and query document, table, key-value, and graph databases, all of which benefit from the global distribution and horizontal scale capabilities at the core of Azure Cosmos DB. 
This tutorial demonstrates how to create an Azure Cosmos DB account for the Cassandra API by using the Azure portal. 

## Running this sample
* Before you can run this sample, you must have the following perquisites:
	* An active Azure Cassandra API account - If you don't have an account, refer to the [Create Cassandra API account](https://github.com/mimig1/azure-docs-pr/blob/cassandra/includes/cosmos-db-create-dbaccount-cassandra.md). 
	* [Microsoft Visual Studio](https://www.visualstudio.com).
	* [Git](http://git-scm.com/).

1. Clone this repository using `git clone https://github.com/mojind/azure-cosmos-db-cassandra-dotnet-tutorial.git`

2. Open the CassandraTutorialSample.sln solution and install the Cassandra .NET driver. Use the .NET Driver's NuGet package. From the Package Manager Console window in Visual Studio:

```bash
PM> Install-Package CassandraCSharpDriver
```

3. Next, configure the endpoints in **Program.cs**

```
private const string UserName = "<FILLME>"; 
private const string Password = "<FILLME>";
private const string CassandraContactPoint = "<FILLME>"; //  DnsName
```
4. Compile and Run the project.

## About the code
1. The code included in this sample is intended to get educate with a C# application using Cassandra C# driver with Cassandra API on top of Azure Cosmos DB. It mainly performs CRUD operations on Cassandra.
2. Cassandra Table used is **Race**. This is modeled as Race Class. This has **race_name**, **race_year** and **rank** which models the primary key for the race table. This has 2 partition Keys (race_year, race_name) and one clustering key (rank). Combination of Partition Key and Clustering key makes a primary key on table. Race Class has other parameters like cyclist_name, location, audiance_capacity, any_accident, shop_ip, transaction_id  spanning  multiple data types. 
  The code is facilitated by Utilities.cs class which helps in Creating Keyspace and table and some basic declarations related to data.
  
> **Create**: This involves creation on keyspace and  table. Also the insertion of new records in InsertQueries where we insert data into the race table. This leverages Mapper Class of C# Driver. Sample Usage with Mapper:
```
// Keyspace creation
session.Execute("CREATE KEYSPACE raceprofile WITH REPLICATION = { 'class' : 'NetworkTopologyStrategy', 'datacenter1' : 1 };");
// Table creation
session.Execute("CREATE TABLE race (race_year int, race_name text, rank int, any_accident boolean, audiance_capacity bigint, cyclist_name text, location text, shop_ip inet, transaction_id uuid, PRIMARY KEY((race_year, race_name), rank))");
// Inserting a record
IMapper mapper = new Mapper(Session);
mapper.Insert<Race>(race);
```

> **Read**: The reading of records is through SelectQueries. Here various type of operation is performed spanning various combinations of where clause, limit clause,  order by clause, logical operators and partition key like Not given, partially given and completely specified. Sample Usage with Mapper:
```
Mapper.Fetch<Race>("Select * from race")
Mapper.First<Race>("Select * from race where race_name = ? and race_year = ? and rank = ?", "GPX", 2012, 3)
```

> **Update**: The update of records is through UpdateQueries which updates a record only if complete primary key is given, else proper error message is thrown. Sample Usage with Mapper:
```
Mapper.Update<Race>("set cyclist_name = ? where race_name = ? and race_year = ? and rank = ?", Utilities.CyclistName.Zahira.ToString(), "GPX", 2012, 3);
```

> **Delete**: The deletion of records is through DeleteQueries which deletes a record only if complete partition key is given, else proper error message is thrown. Sample Usage with Mapper: 
```
Mapper.Delete<Race>("where race_name=? and race_year=?", "GPX", 2013);
```

> **Paging**: The paging of record is testing out using with and without mapper. This is done in PagingQueries. Sample Usage with Mapper:

```
IPage<Race> raceFirstPage = Mapper.FetchPage<Race>(Cql.New("Select * from race").WithOptions(opt => opt.SetPageSize(4)));
IPage<Race> raceSecondPage = Mapper.FetchPage<Race>(pageSize: 4, pagingState: raceFirstPage.PagingState, query: "Select * from race", args: null);
```

## More information

- [Azure Cosmos DB](https://docs.microsoft.com/azure/cosmos-db/introduction)
- [Cassandra DB](http://cassandra.apache.org/)
