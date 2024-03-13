using AFI.Feature.QuoteForm.Areas.AFIWEB.ModelBuilders;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Services;
using System;
using System.Text.RegularExpressions;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Processors
{
	public interface IContactUsFormProcessor
	{
		ProcessorResponseModel Process(ContactUsPostModel model);
	}

	public class ContactUsFormProcessor : IContactUsFormProcessor
	{
		private readonly IEmailService _emailService;
		private readonly IReCaptchaService _reCaptchaService;
		private readonly IContactUsEmailBuilder _emailBuilder;

		const string PHONE_VALIDATION_PATTERN = @"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}";

		public ContactUsFormProcessor(IEmailService emailService, IReCaptchaService reCaptchaService, IContactUsEmailBuilder emailBuilder)
		{
			_emailService = emailService;
			_reCaptchaService = reCaptchaService;
			_emailBuilder = emailBuilder;
		}

		public ProcessorResponseModel Process(ContactUsPostModel model)
		{
			var output = new ProcessorResponseModel();

			if (model == null)
			{
				output.WasSuccessful = false;
				output.FailureMessage = "Please try again";
				return output;
			}

			if (!_reCaptchaService.VerifyResponse(model.token).Success)
			{
				output.WasSuccessful = false;
				output.FailureMessage = "Please try again";
				return output;
			}

			if (model.name.Length > 100 || model.message.Length > 500 || model.email.Length > 100)
			{
				output.WasSuccessful = false;
				output.FailureMessage = "The maximum character limit has been exceeded. Please correct the error and re-submit the form";
				return output;
			}

			Regex regex = new Regex(PHONE_VALIDATION_PATTERN, RegexOptions.IgnoreCase);
			if (!string.IsNullOrEmpty(model.phone) && !IsValidEmail(model.email) || !regex.IsMatch(model.phone))
			{
				output.WasSuccessful = false;
				output.FailureMessage = "Invalid Email address or phone number";
				return output;
			}

			SendConsumerContactUsEmail(model.name, model.email, model.phone, model.subject, model.message);
			SendInternalContactUsFormEmail(model.name, model.email, model.phone, model.subject, model.message);

			output.WasSuccessful = true;
			return output;
		}

		private void SendConsumerContactUsEmail(string name, string email, string phone, string subject, string body)
		{
			try
			{
				var message = _emailBuilder.BuildConsumerConfirmationEmail(name, email, phone, subject, body);
				_emailService.Send(message);
			}
			catch (Exception ex)
			{
				Sitecore.Diagnostics.Log.Error($"{nameof(ContactUsFormProcessor)}: Error sending client contact us confirmation email to {email} for subject {subject}.", ex, this);
			}
		}

		private void SendInternalContactUsFormEmail(string name, string consumerEmail, string phone, string subject, string message)
		{
			try
			{
				var email = _emailBuilder.BuildInternalEmail(name, consumerEmail, phone, subject, message);
				_emailService.Send(email);
			}
			catch (Exception ex)
			{
				Sitecore.Diagnostics.Log.Error($"{nameof(ContactUsFormProcessor)}: Error sending internal contact us email for subject {subject}.", ex, this);
			}
		}

		private bool IsValidEmail(string email)
		{
			try
			{
				var addr = new System.Net.Mail.MailAddress(email);
				return addr.Address == email;
			}
			catch
			{
				return false;
			}
		}		
	}
}