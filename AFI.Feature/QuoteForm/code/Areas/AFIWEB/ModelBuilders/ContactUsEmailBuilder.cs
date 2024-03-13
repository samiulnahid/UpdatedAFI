using AFI.Feature.QuoteForm.Areas.AFIWEB.Models;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Repository;
using Identifiers = AFI.Feature.Identifiers;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.ModelBuilders
{
	public interface IContactUsEmailBuilder
	{
		Email BuildConsumerConfirmationEmail(string consumerName, string consumerEmail, string consumerPhone, string consumerSubjectId, string consumerMessageBody);
		Email BuildInternalEmail(string consumerName, string consumerEmail, string consumerPhone, string consumerSubjectId, string consumerMessageBody);
	}

	public class ContactUsEmailBuilder : IContactUsEmailBuilder
	{
		private readonly IGlobalSettingsRepository _globalSettingsRepository;

		public ContactUsEmailBuilder(IGlobalSettingsRepository globalSettingsRepository)
		{
			_globalSettingsRepository = globalSettingsRepository;
		}

		public Email BuildConsumerConfirmationEmail(string consumerName, string consumerEmail, string consumerPhone, string consumerSubjectId, string consumerMessageBody)
		{
			var subjectTitle = Sitecore.Context.Database.GetItem(consumerSubjectId)[Identifiers.Templates.Subject_Option.FieldNames.Title];
			var fromEmail = _globalSettingsRepository.GetSetting(Identifiers.Templates.Global_Settings.FieldNames.Contact_Us_Client_From_List);
			var subjectTemplate = _globalSettingsRepository.GetSetting(Identifiers.Templates.Global_Settings.FieldNames.Contact_Us_Client_Subject);
			var bodyTemplate = _globalSettingsRepository.GetSetting(Identifiers.Templates.Global_Settings.FieldNames.Contact_Us_Client_Body);

			var email = new Email();
			email.IsBodyHtml = true;
			email.To = new[] { consumerEmail };
			email.FromEmail = fromEmail;
			email.Subject = ReplaceEmailStringTokens(subjectTemplate, consumerName, consumerEmail, consumerPhone, subjectTitle, consumerMessageBody);
			email.Body = ReplaceEmailStringTokens(bodyTemplate, consumerName, consumerEmail, consumerPhone, subjectTitle, consumerMessageBody);
			
			return email;
		}

		public Email BuildInternalEmail(string consumerName, string consumerEmail, string consumerPhone, string consumerSubjectId, string consumerMessageBody)
		{
			var subjectTitle = Sitecore.Context.Database.GetItem(consumerSubjectId)[Identifiers.Templates.Subject_Option.FieldNames.Title];
			var fromEmail = _globalSettingsRepository.GetSetting(Identifiers.Templates.Global_Settings.FieldNames.Contact_Us_Internal_From);
			var subjectTemplate = _globalSettingsRepository.GetSetting(Identifiers.Templates.Global_Settings.FieldNames.Contact_Us_Internal_Subject);
			var bodyTemplate = _globalSettingsRepository.GetSetting(Identifiers.Templates.Global_Settings.FieldNames.Contact_Us_Internal_Body);
			var emailAddresses = GetContactUsInternalEmailAddress(consumerSubjectId);

			var email = new Email();
			email.IsBodyHtml = true;
			email.To = emailAddresses.ToList;
			email.Bcc = emailAddresses.BCCList;
			email.Cc = emailAddresses.CCList;
			email.Subject = ReplaceEmailStringTokens(subjectTemplate, consumerName, consumerEmail,consumerPhone, subjectTitle, consumerMessageBody);
			email.Body = ReplaceEmailStringTokens(bodyTemplate, consumerName, consumerEmail, consumerPhone, subjectTitle, consumerMessageBody);
			email.FromEmail = fromEmail;

			return email;
		}

		private AddressBook GetContactUsInternalEmailAddress(string guid)
		{
			var subject = Sitecore.Context.Database.GetItem(guid);
			if (subject != null)
			{
				return new AddressBook
				{
					ToList = subject[Identifiers.Templates.Subject_Option.FieldNames.Email_List_To].Split('|'),
					BCCList = subject[Identifiers.Templates.Subject_Option.FieldNames.Email_List_Bcc].Split('|'),
					CCList = subject[Identifiers.Templates.Subject_Option.FieldNames.Email_List_CC].Split('|'),
					FromList = _globalSettingsRepository.GetSetting(Identifiers.Templates.Global_Settings.FieldNames.Contact_Us_Internal_From).Split('|')
				};
			}
			else
			{
				return new AddressBook();
			}
		}

		private string ReplaceEmailStringTokens(string stringToEdit, string consumerName, string consumerEmail, string consumerPhone, string consumerSubjectTitle, string consumerMessageBody)
		{
			const string NAME_TOKEN = "{{NAME}}";
			const string EMAIL_TOKEN = "{{EMAIL}}";
			const string PHONE_TOKEN = "{{PHONE}}";
			const string SUBJECT_TOKEN = "{{SUBJECT}}";
			const string MESSAGE_TOKEN = "{{MESSAGE}}";

			stringToEdit = stringToEdit.Replace(NAME_TOKEN, consumerName);
			stringToEdit = stringToEdit.Replace(EMAIL_TOKEN, consumerEmail);
			stringToEdit = stringToEdit.Replace(PHONE_TOKEN, consumerPhone);
			stringToEdit = stringToEdit.Replace(SUBJECT_TOKEN, consumerSubjectTitle);
			stringToEdit = stringToEdit.Replace(MESSAGE_TOKEN, consumerMessageBody);

			return stringToEdit;
		}
	}
}