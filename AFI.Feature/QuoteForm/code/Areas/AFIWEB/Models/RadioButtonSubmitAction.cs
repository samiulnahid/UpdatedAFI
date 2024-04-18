using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using Sitecore.Diagnostics;
using Sitecore.ExperienceForms.Data;
using Sitecore.ExperienceForms.Data.Entities;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Processing;
using Sitecore.ExperienceForms.Processing.Actions;
using Sitecore.ExperienceForms.Tracking;
using Sitecore.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models;
using static Sitecore.Web.Authentication.DomainAccessGuard;

namespace Sitecore.ExperienceForms.Samples.SubmitActions
{
    public class RadioButtonSubmitAction : SubmitActionBase<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogSubmit"/> class.
        /// </summary>
        /// <param name="submitActionData">The submit action data.</param>
        public RadioButtonSubmitAction(ISubmitActionData submitActionData) : base(submitActionData)
        {
        }
        protected override bool TryParse(string value, out string target)
        {
            target = string.Empty;
            return true;
        }
        protected override bool Execute(string data, FormSubmitContext formSubmitContext)
        {
            if (formSubmitContext.Fields != null)
            {
                foreach (IViewModel postedField in formSubmitContext.Fields)
                {
                    AddFieldData(postedField, new FormEntry { FormItemId = formSubmitContext.FormId, FormEntryId = formSubmitContext.SessionId, Fields = new  List<FieldData>() });
                }
            }

            return true;
        }

        #region Radio Button Action

        //private IFormDataProvider _dataProvider;

        //private IFileStorageProvider _fileStorageProvider;

        //private IContactIdResolver _contactIdResolver;


        //protected virtual IContactIdResolver ContactIdResolver
        //{
        //    get
        //    {
        //        IContactIdResolver contactIdResolver = this._contactIdResolver;
        //        if (contactIdResolver == null)
        //        {
        //            IContactIdResolver service = ServiceProviderServiceExtensions.GetService<IContactIdResolver>(ServiceLocator.ServiceProvider);
        //            IContactIdResolver contactIdResolver1 = service;
        //            this._contactIdResolver = service;
        //            contactIdResolver = contactIdResolver1;
        //        }
        //        return contactIdResolver;
        //    }
        //}

        //public RadioButtonSubmitAction(ISubmitActionData submitActionData) : base(submitActionData) { }
        //protected override bool Execute(string data, FormSubmitContext formSubmitContext)
        //{
        //    FormEntry formEntry = new FormEntry()
        //    {
        //        Created = DateTime.UtcNow,
        //        FormItemId = formSubmitContext.FormId,
        //        FormEntryId = formSubmitContext.SessionId,
        //        Fields = new List<FieldData>(),
        //        ContactId = this.ContactIdResolver.ContactId
        //    };

        //    if (formSubmitContext.Fields != null)
        //    {
        //        foreach (IViewModel postedField in formSubmitContext.Fields)
        //        {
        //            AddFieldData(postedField, formEntry);
        //        }
        //    }

        //    return false;
        //}
        protected static void AddFieldData(IViewModel postedField, FormEntry formEntry)
        {
            List<SelectedValues> fieldValues = GetFieldValue(postedField);
        }
        private static List<SelectedValues> GetFieldValue(IViewModel postedField)
        {
            Assert.ArgumentNotNull(postedField, "postedField");
            return ReflectionUtil.CallMethod(postedField, "GetSelectedStringValue") as List<SelectedValues>;
        }

        #endregion
    }
}