using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace AFI.Feature.Data.Providers
{
	public interface IDbConnectionProvider
	{
		IDbConnection GetAFIDatabaseConnection();
	}

	public class DbConnectionProvider : IDbConnectionProvider
	{
		private static readonly string ConnectionString;

		static DbConnectionProvider()
		{
			ConnectionString = ConfigurationManager.ConnectionStrings["AFIDB"].ConnectionString;
		}

		public IDbConnection GetAFIDatabaseConnection()
		{
			return new SqlConnection(ConnectionString);
		}
	}
}
