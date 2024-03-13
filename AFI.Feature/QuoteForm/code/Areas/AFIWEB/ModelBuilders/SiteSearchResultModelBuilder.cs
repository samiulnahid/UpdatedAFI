using System.Web;
using Elision.Feature.Library.Search;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Data;
using AFI.Foundation.Quote.Areas.AFIWEB.Models;
using AFITemplates = AFI.Feature.Identifiers.Templates;

namespace AFI.Foundation.Quote.Areas.AFIWEB.ModelBuilders
{
	public interface ISiteSearchResultModelBuilder
	{
		SiteSearchResultModel Build(string q, Item pageContextItem);
	}

	public class SiteSearchResultModelBuilder : ISiteSearchResultModelBuilder
	{
		private readonly ISiteSearcher _siteSearcher;

		public SiteSearchResultModelBuilder(ISiteSearcher siteSearcher)
		{
			_siteSearcher = siteSearcher;
		}

		public SiteSearchResultModel Build(string q, Item pageContextItem)
		{
			var model = new SiteSearchResultModel
			{
				Query = q,
				SearchUrl = LinkManager.GetItemUrl(Sitecore.Context.Item)
			};

			var	allowedTemplateIds = new[]
				{
					AFITemplates._ContentPage.TemplateId,
					//AFITemplates._RichTextEditor.TemplateId,
					//AFITemplates.RTE_Block.TemplateId,
					//AFITemplates.RTE_Pod.TemplateId,
					///AFITemplates.Content_Page.TemplateId,
					//AFITemplates._HomePage.TemplateId
				};

			var searchOptions = new SiteSearchOptions(null, pageContextItem, q, allowedTemplateIds);

			model.Results = _siteSearcher.Search(searchOptions);

			return model;
		}
	}
}