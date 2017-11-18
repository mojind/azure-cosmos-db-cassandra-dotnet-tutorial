using Cassandra;
using System;
using System.Security.Authentication;

namespace CassandraTutorialSample
{
    public class Program
    {
        // Cassandra Cluster Configs
        private const string UserName = "<FILL ME>";
        private const string Password = "<FILL ME>";
        private const string CassandraContactPoint = "<FILL ME>"; //  dnsname
        private static int CassandraPort = 10350;

        public static void Main(string[] args)
        {
            // Connect to cassandra cluster  (Cassandra API on Azure Cosmos DB supports only TLSv1.2)
            var options = new Cassandra.SSLOptions(SslProtocols.Tls12, true, Utilities.ValidateServerCertificate);
            options.SetHostNameResolver((ipAddress) => CassandraContactPoint);
            Cluster cluster = Cluster.Builder().WithCredentials(UserName, Password).WithPort(CassandraPort).AddContactPoint(CassandraContactPoint).WithSSL(options).Build();
            ISession session = cluster.Connect();

            Utilities.CreateKeyspace(session);
            session = cluster.Connect("raceprofile");
            Utilities.CreateTable(session);

            InsertQueries insertData = new InsertQueries(session);
            insertData.InsertRecords();

            SelectQueries selectData = new SelectQueries(session);
            selectData.ShowRecords();

            PagingQueries pagingData = new PagingQueries(session);
            pagingData.Paging();

            UpdateQueries updateQueries = new UpdateQueries(session);
            updateQueries.Update();

            DeleteQueries deleteQueries = new DeleteQueries(session);
            deleteQueries.DeleteRecords();

            // Wait for enter key before exiting  
            Console.ReadLine();
        }
    }
}
