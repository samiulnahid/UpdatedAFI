using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace AFI.Feature.Prospect.Providers
{
	public interface IDatabaseConnectionProvider
	{
		IDbConnection GetAFIDatabaseConnection();
	}

	public class DatabaseConnectionProvider : IDatabaseConnectionProvider
	{
		private static readonly string ConnectionString;

		static DatabaseConnectionProvider()
		{
			ConnectionString = ConfigurationManager.ConnectionStrings["AFIDB"].ConnectionString;
		}

		public IDbConnection GetAFIDatabaseConnection()
		{
			return new SqlConnection(ConnectionString);
		}
	}
}
