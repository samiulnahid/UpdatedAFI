using System.Collections.Generic;
using System.Linq;
using AFI.Feature.VinLookup;
using AFI.Feature.VinLookup.Models;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.AutoForm.VinLookup;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Handlers
{
    public interface IAutoFormHandler
    {
        VehicleVinInformation GetByVin(AutoByVinPostModel vinNumberPostModel);
        VehicleMakeInformation GetMakesByYear(AutoMakeByYearPostModel yearNumberPostModel);
        VehicleModelInformation GetModelsByMake(AutoModelByMakePostModel modelByMakePostModel);
        VehicleBodyTypeInformation GetBodyTypesByModel(AutoBodyTypeByModelPostModel byModelPostModel);
    }
    public class AutoFormHandler : IAutoFormHandler
    {
        private readonly IVinLookupService _service;

        public AutoFormHandler(IVinLookupService service)
        {
            _service = service;
        }

        public VehicleVinInformation GetByVin(AutoByVinPostModel vinNumberPostModel)
        {
            if(vinNumberPostModel == null) return new VehicleVinInformation();
            VehicleInformation vehicleInfo = _service.GetVehicleInformationByVin(vinNumberPostModel.Vin);
            return new VehicleVinInformation
            {
                Model = new Options
                {
                    label = vehicleInfo.Model,
                    value = vehicleInfo.Model
                },
                Year = new Options
                {
                    label = vehicleInfo.Year.ToString(),
                    value = vehicleInfo.Year.ToString()
                },
                Make = new Options
                {
                    label = vehicleInfo.Make,
                    value = vehicleInfo.Make
                },
                BodyType = new Options
                {
                    label = vehicleInfo.BodyStyle,
                    value = vehicleInfo.BodyStyle
                }
            };
        }

        public VehicleMakeInformation GetMakesByYear(AutoMakeByYearPostModel yearNumberPostModel)
        {
            int year;
            if (yearNumberPostModel == null || !int.TryParse(yearNumberPostModel.Year, out year)) return new VehicleMakeInformation();
            IDictionary<int, string> makes = _service.GetMakes(year);
            return new VehicleMakeInformation
            {
                MakeList = makes.Select(m=> new Options
                {
                    value = m.Key.ToString(),
                    label = m.Value
                }).ToList()
            };
        }

        public VehicleModelInformation GetModelsByMake(AutoModelByMakePostModel modelByMakePostModel)
        {
            int year;
            int makeId;
            if(modelByMakePostModel == null || !int.TryParse(modelByMakePostModel.Year, out year) || !int.TryParse(modelByMakePostModel.MakeId, out makeId)) return new VehicleModelInformation();
            var models = _service.GetModels(year, makeId);
            return new VehicleModelInformation
            {
                ModelList = models.Select(m=> new Options
                {
                    value = m.Key.ToString(),
                    label = m.Value
                }).ToList()
            };
        }

        public VehicleBodyTypeInformation GetBodyTypesByModel(AutoBodyTypeByModelPostModel byModelPostModel)
        {
            int modelId;
            if(byModelPostModel == null || !int.TryParse(byModelPostModel.ModelId, out modelId)) return new VehicleBodyTypeInformation();
            var bodyTypes = _service.GetBodyStyles(modelId);
            return new VehicleBodyTypeInformation
            {
                BodyTypes = bodyTypes.Select(bt=> new Options
                {
                    value = bt.Key.ToString(),
                    label = bt.Value
                }).ToList()
            };
        }
    }
}