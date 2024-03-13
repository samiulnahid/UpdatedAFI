using AFI.Feature.Identifiers;
using AFI.Foundation.QuoteForm.Models;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.ModelBuilders
{
	public interface IContactUsFormViewModelBuilder
	{
		ContactUsFormViewModel Build();
	}


	public class ContactUsFormViewModelBuilder : IContactUsFormViewModelBuilder
	{
		public ContactUsFormViewModel Build()
		{
			var model = new ContactUsFormViewModel();
			model.Subjects = BuildSubjectList();
			model.PhoneHelpText = RenderingContext.Current.Rendering.Item[Templates.Contact_Us_Form.FieldNames.PhoneHelpText];
			model.RecaptchaKey = ConfigurationManager.AppSettings["ReCaptcha.SiteKey"];
			return model;
		}

		private IEnumerable<SelectListItem> BuildSubjectList()
		{
			var db = Sitecore.Context.Database;
			var subjectItemIds = RenderingContext.Current.Rendering.Item[Templates.Contact_Us_Form.FieldNames.Subjects].Split('|');
			var output = new List<SelectListItem>();
			output.Add(new SelectListItem() { Value = "", Text = RenderingContext.Current.Rendering.Item[Templates.Contact_Us_Form.FieldNames.DefaultSubjectText] });
			Item item;

			foreach (var id in subjectItemIds)
			{
				if (!string.IsNullOrWhiteSpace(id))
				{
					item = db.GetItem(id);
					output.Add(new SelectListItem()
					{
						Value = id,
						Text = item[Templates.Subject_Option.FieldNames.Title]
					});
				}
			}

			return output;
		}
	}
}