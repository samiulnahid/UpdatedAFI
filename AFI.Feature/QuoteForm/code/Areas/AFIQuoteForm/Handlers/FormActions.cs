namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Handlers
{
	public static class FormActions
	{
        //INFO: Send action will be performed on each step of Quote Forms (except submit and save & exit
		public const string Send = "send";
        public const string SaveAndExit = "save";
		public const string Submit = "submit";
		public const string Cancel = "cancel";
        //INFO: Abandon will be hit when the user leaves the form, closes the browser tab or goes to another website
        public const string Abandon = "abandonedform";
    }
}