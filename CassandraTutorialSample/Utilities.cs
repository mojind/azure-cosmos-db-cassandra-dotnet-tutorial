using Cassandra;
using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace CassandraTutorialSample
{
    public static class Utilities
    {
        public enum Location
        {
            Delhi, Mumbai, Bangalore, Calcutta
        }

        public enum CyclistName
        {
            LyubovK, JiriK, IvanH, LiliyaB
        }
         
        public static T RandomEnum<T>()
		{
			T[] values = (T[])Enum.GetValues(typeof(T));
			return values[new Random().Next(0, values.Length)];
		}

		public static void CreateKeyspace(ISession session)
		{
            session.Execute("DROP KEYSPACE IF EXISTS raceprofile");
            session.Execute("CREATE KEYSPACE raceprofile WITH REPLICATION = { 'class' : 'NetworkTopologyStrategy', 'datacenter1' : 1 };");
            Console.WriteLine("Created keyspace raceprofile");
		}

        public static void CreateTable(ISession session)
        {
            // create race table (NO TTL (time to live specified)
            session.Execute("drop table IF EXISTS race");
            session.Execute("CREATE TABLE race (race_year int, race_name text, rank int, any_accident boolean, audiance_capacity bigint, cyclist_name text, location text, shop_ip inet, transaction_id uuid, PRIMARY KEY((race_year, race_name), rank))");
            Console.WriteLine("Created table race");
        }

        public static bool ValidateServerCertificate(
            object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            Console.WriteLine("Certificate error: {0}", sslPolicyErrors);
            // Do not allow this client to communicate with unauthenticated servers.
            return false;
        }

        public static IPAddress GetRandomIPAddress()
        {
            int rand = new Random().Next(1, 8);
            switch (rand)
            {
                case 1:
                    return new IPAddress(new byte[] { 192, 168, 20, 15 });
                case 2:
                    return IPAddress.Parse("2001:cdba:0000:0000:0000:0000:3257:9652");
                case 3:
                    return new IPAddress(new byte[] { 192, 168, 20, 11 });
                case 4:
                    return new IPAddress(new byte[] { 121, 112, 64, 99 });
                default:
                    return new IPAddress(new byte[] { 192, 168, 20, 19 });
            }
        }
    }
}

