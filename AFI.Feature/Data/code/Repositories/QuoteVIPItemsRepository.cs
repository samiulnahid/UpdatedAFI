using AFI.Feature.Data.Providers;
using Dapper;
using AFI.Feature.Data.DataModels;
using Sitecore.Diagnostics;
using System.Collections.Generic;

namespace AFI.Feature.Data.Repositories
{
    public interface IQuoteVIPItemsRepository
    {
        IEnumerable<QuoteVIPItems> GetAllForQuote(int quoteKey);
        void Create(QuoteVIPItems entity);
    }

    public class QuoteVIPItemsRepository : IQuoteVIPItemsRepository
    {
		private readonly IDbConnectionProvider _dbConnectionProvider;

		public QuoteVIPItemsRepository(IDbConnectionProvider dbConnectionProvider)
		{
			_dbConnectionProvider = dbConnectionProvider;
		}

		public IEnumerable<QuoteVIPItems> GetAllForQuote(int quoteKey)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select * from dbo.[QuoteVIPItems] where [QuoteKey] = @quoteKey";
				return db.Query<QuoteVIPItems>(sql, new { quoteKey });
			}
		}

		public void Create(QuoteVIPItems entity)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				db.Open();
				using (var transaction = db.BeginTransaction())
				{
					try
					{
						var sql = "insert into dbo.[QuoteVIPItems] ([Key], [QuoteKey], [PropertyType], [ItemTitle], [StampCoinNumberItems], [CameraItemDescription], [CameraBrand], [CameraSerialNumber], [CameraAccessories], [SilverwareVasesNumberPlaceSetting], [SilverwareVasesNumberPiecesPerPlaceSetting], [SilverwareVasesManufacturer], [SilverwareVasesNumberAdditionalPieces], [FineArtsAntiquesAge], [FineArtsAntiquesSize], [WeaponMake], [WeaponModel], [WeaponSerialNumber], [JewelryColor], [JewelryCut], [JewelryClarity], [JewelryCaratWeight], [JewelryMountingMetal], [JewelryWatchManufacturer], [FurType], [FurArticle], [InstrumentType], [ClassRingStoneType], [ClassRingStoneWeight], [ClassRingMountingMetal], [Description], [EstimatedValue], [CoverageCost], [Changeset], [CreateDate]) values (@Key, @QuoteKey, @PropertyType, @ItemTitle, @StampCoinNumberItems, @CameraItemDescription, @CameraBrand, @CameraSerialNumber, @CameraAccessories, @SilverwareVasesNumberPlaceSetting, @SilverwareVasesNumberPiecesPerPlaceSetting, @SilverwareVasesManufacturer, @SilverwareVasesNumberAdditionalPieces, @FineArtsAntiquesAge, @FineArtsAntiquesSize, @WeaponMake, @WeaponModel, @WeaponSerialNumber, @JewelryColor, @JewelryCut, @JewelryClarity, @JewelryCaratWeight, @JewelryMountingMetal, @JewelryWatchManufacturer, @FurType, @FurArticle, @InstrumentType, @ClassRingStoneType, @ClassRingStoneWeight, @ClassRingMountingMetal, @Description, @EstimatedValue, @CoverageCost, @Changeset, @CreateDate)";
						db.Execute(sql, entity, transaction);
						transaction.Commit();
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(QuoteVIPItemsRepository)}: Error while attempting to insert QuoteVIPItems.", ex, this);
						transaction.Rollback();
					}
				}
			}
		}
	}
}