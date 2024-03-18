/**
 * used to validate that inputs are not empty
 * @param {Object} event	JavaScript event object	The input event reference object related to the form input field. This event data can be helpful to perform actions like active validation on an input field while the user is typing.
 * @param {Object} form	HTML reference	A jquery reference to the form wrapper element.
 * @param {Object} input	HTML reference	A jquery reference to the input element being validated.
 * @param {Object} label	HTML reference	A jquery reference to the label for the input being validated.
 * @param {Object} $	jquery instance	Widget’s internal jquery instance. Use this to help you write your validation logic, if needed.
 * @param {Object} CXBus	CXBus instance	Widget’s internal CXBus reference. Use this to call commands on the bus, if needed.
 * @param {Object} Common	Function Library	Widget’s internal Common library of functions and utilities. Use if needed.
 */
const isNotEmptyTwo = (event, form, input, label, $, CXBus, Common) => {
  return input && !!input.val().trim();
};

window._genesys = {
  widgets: {
    main: {
      preload: ["webchat"],
      i18n: {
        en: {
          calendar: {
            CalendarDayLabels: ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"],
            CalendarMonthLabels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept", "Oct", "Nov", "Dec"],
            CalendarLabelToday: "Today",
            CalendarLabelTomorrow: "Tomorrow",
            CalendarTitle: "Schedule a Call",
            CalendarOkButtonText: "Okay",
            CalendarError: "Unable to fetch availability details.",
            CalendarClose: "Cancel",
            AriaWindowTitle: "Calendar Window",
            AriaCalendarClose: "Cancel",
            AriaYouHaveChosen: "You have chosen",
            AriaNoTimeSlotsFound: "No time slots found for selected date",
          },
          callus: {
            CallUsTitle: "Call Us",
            ContactsHeader: "You can reach us at any of the following numbers...",
            CancelButtonText: "Cancel",
            CoBrowseText:
              "<span class='cx-cobrowse-offer'>Already on a call? Let us <a role='link' tabindex='0' class='cx-cobrowse-link'>browse with you</a></span>",
            CoBrowse: "Start Co-browse",
            CoBrowseWarning:
              "Co-browse allows your agent to see and control your desktop, to help guide you. When on a live call with an Agent, request a code to start Co-browse and enter it below. Not yet on a call? just cancel out of this screen to return to Call Us page.",
            SubTitle: "You can reach us at any of the following numbers...",
            AriaWindowLabel: "Call Us Window",
            AriaCallUsClose: "Call Us Close",
            AriaBusinessHours: "Business Hours",
            AriaCallUsPhoneApp: "Opens the phone application",
            AriaCobrowseLink: "Opens the Co-browse Session",
            AriaCancelButtonText: "Cancel",
          },
          callback: {
            CallbackTitle: "Receive a Call",
            CancelButtonText: "Cancel",
            AriaCancelButtonText: "Cancel",
            ConfirmButtonText: "Confirm",
            AriaConfirmButtonText: "Confirm",
            CallbackFirstName: "First Name",
            CallbackPlaceholderRequired: "Required",
            CallbackPlaceholderOptional: "Optional",
            CallbackLastName: "Last Name",
            CallbackPhoneNumber: "Phone",
            CallbackQuestion: "When should we call you?",
            CallbackDayLabels: ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"],
            CallbackMonthLabels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
            CallbackConfirmDescription: "You're booked in!",
            CallbackNumberDescription: "We will call you at the number provided:",
            CallbackNotes: "Notes",
            CallbackDone: "Close",
            AriaCallbackDone: "Close",
            CallbackOk: "Okay",
            AriaCallbackOk: "Okay",
            CallbackCloseConfirm: "Are you sure you want to cancel arranging this callback?",
            CallbackNoButtonText: "No",
            AriaCallbackNoButtonText: "No",
            CallbackYesButtonText: "Yes",
            AriaCallbackYesButtonText: "Yes",
            CallbackWaitTime: "Wait Time",
            CallbackWaitTimeText: "min wait",
            CallbackOptionASAP: "As soon as possible",
            CallbackOptionPickDateTime: "Pick date & time",
            AriaCallbackOptionPickDateTime: "Opens a date picker",
            CallbackPlaceholderCalendar: "Select Date & Time",
            AriaMinimize: "Callback Minimize",
            AriaWindowLabel: "Callback Window",
            AriaMaximize: "Callback Maximize",
            AriaClose: "Callback Close",
            AriaCalendarClosedStatus: "Calendar is closed",
            Errors: {
              501: "Invalid parameters cannot be accepted, please check the supporting server API documentation for valid parameters.",
              503: "Missing apikey, please ensure it is configured properly.",
              1103: "Missing apikey, please ensure it is configured properly.",
              7030: "Please enter a valid phone number.",
              7036: "Callback to this number is not possible. Please retry with another phone number.",
              7037: "Callback to this number is not allowed. Please retry with another phone number.",
              7040: "Please configure a valid service name.",
              7041: "Too many requests at this time.",
              7042: "Office closed. Please try scheduling within the office hours.",
              unknownError:
                "Something went wrong, we apologize for the inconvenience. Please check your connection settings and try again.",
              phoneNumberRequired: "Phone number is required.",
              CallbackDateTimeRequired: "Select Date & Time",
            },
          },
          channelselector: {
            Title: "Live Assistance",
            SubTitle: "How would you like to get in touch?",
            WaitTimeTitle: "Wait Time",
            AvailableTitle: "Available",
            AriaAvailableTitle: "Available",
            UnavailableTitle: "Unavailable",
            CobrowseButtonText: "CobrowseSubTitle",
            CallbackTitle: "Receive a Call",
            CobrowseSubTitle: "Agent connection is required for this.",
            AriaClose: "Live Assistance Close",
            AriaWarning: "Warning",
            AriaAlert: "Alert",
            minute: "min",
            minutes: "mins",
            AriaWindowLabel: "Live Assistance Window",
          },
          clicktocall: {
            Title: "ClickToCall",
            FirstName: "First Name",
            PlaceholderRequired: "Required",
            PlaceholderOptional: "Optional",
            LastName: "Last Name",
            PhoneNumber: "Phone",
            WaitTime: "Wait Time",
            FormCancel: "Cancel",
            AriaFormCancel: "Cancel",
            FormSubmit: "Request a number",
            AriaFormSubmit: "Request a number",
            PhoneLabel: "Dial in now",
            AriaPhoneTitle: "Opens the phone application",
            AccessLabel: "Access Code",
            ExpireLabel: "Number Expires in",
            AriaExpireLabel: "Number Expires in Timer",
            DisplayClose: "Close",
            AriaDisplayClose: "Close",
            NetworkFail:
              "Something went wrong, we apologize for the inconvenience. Please check your connection settings and try again.",
            NetworkRetry: "OK",
            AriaNetworkRetry: "OK",
            InvalidAccept: "OK",
            AriaInvalidAccept: "OK",
            PhoneExpired: "Phone number has expired!",
            PhoneReRequest: "Request a new number",
            AriaPhoneReRequest: "Request a new number",
            LocalFormValidationEmptyPhoneNumber: "Please enter a phone number",
            ConfirmCloseWindow: "You have unsubmitted form data. Are you sure you want to quit?",
            AriaConfirmCloseCancel: "No",
            ConfirmCloseCancel: "No",
            AriaConfirmCloseConfirm: "Yes",
            ConfirmCloseConfirm: "Yes",
            AriaWindowLabel: "Click To Call Window",
            AriaMaximize: "Click To Call Maximize",
            AriaMinimize: "Click To Call Minimize",
            AriaClose: "Click To Call Close",
          },
          cobrowse: {
            agentJoined: "Representative has joined the session.",
            youLeft: "You have left the session. Co-browse is now terminated.",
            sessionTimedOut: "Session timed out. Co-browse is now terminated.",
            sessionInactiveTimedOut: "Session timed out. Co-browse is now terminated.",
            agentLeft: "Representative has left the session. Co-browse is now terminated.",
            sessionError: "Unexpected error occured. Co-browse is now terminated.",
            sessionStarted: "Co-browse session started. Waiting for representative to connect to the session…",
            navRefresh: "Representative has refreshed the page. Reloading.",
            navBack: 'Representative has pressed the "Back" button. Reloading page.',
            navForward: 'Representative has pressed the "Forward" button. Reloading page.',
            navUrl: "Representative has requested navigation. Reloading page.",
            navFailed: "Navigation request by representative has failed.",
            toolbarContent: "Session ID: {sessionId}",
            contentMasked: "Content is hidden from representative.",
            contentMaskedPartially: "Some content is hidden from representative.",
            exitBtnTitle: "Exit Co-browse session",
            areYouOnPhone: "Are you on the phone with our representative?",
            areYouOnPhoneOrChat: "Are you on the phone or chat with our representative?",
            connectBeforeCobrowse:
              "You need to be connected with our representative to continue with co-browsing. Please call us or start a live chat with us, and then start Co-browse again.",
            sessionStartedAutoConnect: "Co-browse session started. Waiting for representative to connect to the session…",
            browserUnsupported:
              "Unfortunately, your browser is not currently supported.<br><br> Supported browsers are: <ul><li><a target='_blank' href='http://www.google.com/chrome'>Google Chrome</a></li><li><a target='_blank' href='http://www.firefox.com/'>Mozilla Firefox</a></li><li><a target='_blank' href='http://microsoft.com/ie'>Internet Explorer 9 and above</a></li><li><a target='_blank' href='https://www.apple.com/safari'>Safari 6 and above</a></li></ul>",
            chatIsAlreadyRunning: "Chat is already running on another page.",
            modalYes: "Yes",
            modalNo: "No",
          },
          knowledgecenter: {
            SidebarButton: "Search",
            SearchButton: "Search",
            Title: "Ask a Question",
            Ask: "Ask",
            AriaAsk: "Ask",
            Close: "Close",
            AriaClose: "Close",
            Categories: "Categories",
            NoResults: "No Results",
            NoResultsTextUnder: "We're sorry but we could not find a suitable answer for you.",
            NoResultsTextRephrase: "Could you please try rephrasing the question?",
            WasThisHelpful: "Was this helpful?",
            Yes: "Yes",
            No: "No",
            AriaYes: "Yes",
            AriaNo: "No",
            ArticleHelpfulnessYes: "Article Helpfulness - 'Yes'",
            ArticleHelpfulnessYesDesc:
              "Great! We're very pleased to hear that the article assisted you in your search. Have a great day!",
            ArticleHelpfulnessNo: "Article Helpfulness - 'No'",
            ArticleHelpfulnessNoDesc:
              "We're sorry that the article wasn't a good match for your search. We thank you for your feedback!",
            TypeYourQuestion: "Type your question",
            Back: "Back",
            AriaBack: "Back",
            More: "More",
            Error: "Error!",
            GKCIsUnavailable: "Knowledge Center Server is currently not available.",
            AriaClear: "Clear",
            AriaSearch: "Search",
            AriaWindowLabel: "Search Window",
            AriaSearchDropdown: "Suggested results",
            AriaSearchMore: "Read more about",
            AriaResultsCount: "Total number of results",
            KnowledgeAgentName: "Knowledge Center",
            WelcomeMessage:
              "Hello and welcome! A Live agent will be with you shortly. In the meantime, can I assist you with any questions you may have? Please type a question into the input field below.",
            SearchResult: "While waiting for an Agent to connect, here are the most relevant answers to your query:",
            NoDocumentsFound: "I'm sorry. No articles matched your question. Would you like to ask another question?",
            SuggestedMessage: "The following knowledge item has been suggested:",
            OpenDocumentHint: "Click on it to view its content.",
            SuggestedDocumentMessage: "The document '<%DocTitle%>' has been suggested.",
            FeedbackQuestion: "Was this helpful?",
            FeedbackAccept: "Yes",
            FeedbackDecline: "No",
            TranscriptMarker: "KnowledgeCenter: ",
            SearchMessage: "Search with query '<%SearchQuery%>'↲",
            VisitMessage: "Visit for document '<%VisitQuery%>'",
            OpenMessage: "Viewed '<%OpenQuery%>'",
            AnsweredMessage: "Results for query '<%AnsweredQuery%>' have been marked as relevant.",
            UnansweredMessage: "Results for query '<%UnansweredQuery%>' have been marked as unanswered.",
            PositiveVoteMessage: "Positive voting for document '<%VoteQuery%>'.",
            NegativeVoteMessage: "Negative voting for document '<%VoteQuery%>'.",
            SuggestedMarker: "The document[^\\0]*?has been suggested.",
          },
          sendmessage: {
            SendMessageButton: "Send Message",
            OK: "OK",
            Title: "Send Message",
            PlaceholderFirstName: "Required",
            PlaceholderLastName: "Required",
            PlaceholderEmail: "Required",
            PlaceholderSubject: "Required",
            PlaceholderTypetexthere: "Type your message here...",
            FirstName: "First Name",
            LastName: "Last Name",
            Email: "Email",
            Subject: "Subject",
            Attachfiles: "Attach files",
            AriaAttachfiles: "Attach files link. Open a file upload dialog.",
            Send: "Send",
            AriaSend: "Send",
            Sent: "Your message has been sent...",
            Close: "Close",
            ConfirmCloseWindow: "Are you sure you want to close the Send Message widget?",
            Cancel: "Cancel",
            AriaMinimize: "Send Message Minimize",
            AriaMaximize: "Send Message Maximize",
            AriaWindowLabel: "Send Message Window",
            AriaClose: "Close",
            AriaCloseAlert: "Alert box is closed",
            AriaEndConfirm: "Yes",
            AriaEndCancel: "Cancel",
            AriaOK: "OK",
            AriaRemoveFile: "Remove file",
            AriaFileIcon: "File",
            AriaFileSize: "File Size",
            Errors: {
              102: "First Name required.",
              103: "Last Name required.",
              104: "Subject required.",
              181: "Email address required.",
              182: "Message text content required.",
              connectionError: "Unable to reach server. Please try again.",
              unknownError:
                "Something went wrong, we apologize for the inconvenience. Please check your connection settings and try again.",
              attachmentsLimit: "Total number of attachments exceeds limit: ",
              attachmentsSize: "Total size of attachments exceeds limit: ",
              invalidFileType: "Unsupported file type. Please upload images, PDFs, text files and ZIPs.",
              invalidFromEmail: "Invalid email - From address.",
              invalidMailbox: "Invalid email - To address.",
            },
          },
          sidebar: {
            SidebarTitle: "Need help?",
            ChannelSelectorName: "Live Assistance",
            ChannelSelectorTitle: "Get assistance from one of our agents right away",
            SearchName: "Search",
            SearchTitle: "Search",
            CallUsName: "Call Us",
            CallUsTitle: "Call Us details",
            CallbackName: "Callback",
            CallbackTitle: "Receive a Call",
            ClickToCallName: "ClickToCall",
            ClickToCallTitle: "Request a customer service phone number",
            SendMessageName: "Send Message",
            SendMessageTitle: "Send Message",
            WebChatName: "Live Chat",
            WebChatTitle: "Live Chat",
            AriaClose: "Close the menu Need help",
          },
          webchat: {
            ChatButton: "Chat",
            ChatStarted: "Chat Started.\n\n Hi there!  An agent will be with you shortly.",
            ChatEnded:
              "Chat Ended.\n\n Thank you for contacting our chat support. I am now closing this chat. If you have any more issues, please don’t hesitate to let us know. Have a great day!",
            AgentNameDefault: "Agent",
            AgentConnected: "<%Agent%> Connected",
            AgentDisconnected: "<%Agent%> Disconnected",
            BotNameDefault: "Bot",
            BotConnected: "<%Bot%> Connected",
            BotDisconnected: "<%Bot%> Disconnected",
            SupervisorNameDefault: "Supervisor",
            SupervisorConnected: "<%Agent%> Connected",
            SupervisorDisconnected: "<%Agent%> Disconnected",
            AgentTyping: "...",
            AriaAgentTyping: "Agent is typing",
            AgentUnavailable: "Sorry. There are no agents available. Please try later.",
            ChatTitle: "Live Chat",
            ChatEnd: "X",
            ChatClose: "X",
            ChatMinimize: "Min",
            ChatFormFirstName: "First Name",
            ChatFormLastName: "Last Name",
            ChatFormNickname: "Nickname",
            ChatFormEmail: "Email",
            ChatFormSubject: "Subject",
            ChatFormPlaceholderFirstName: "Required",
            ChatFormPlaceholderLastName: "Required",
            ChatFormPlaceholderNickname: "Optional",
            ChatFormPlaceholderEmail: "Optional",
            ChatFormPlaceholderSubject: "Optional",
            ChatFormSubmit: "Start Chat",
            AriaChatFormSubmit: "Start Chat",
            ChatFormCancel: "Cancel",
            AriaChatFormCancel: "Cancel",
            ChatFormClose: "Close",
            ChatInputPlaceholder: "Type your message here",
            ChatInputSend: "Send",
            AriaChatInputSend: "Send",
            ChatEndQuestion: "Are you sure you want to end this chat session?",
            ChatEndCancel: "Cancel",
            ChatEndConfirm: "End chat",
            AriaChatEndCancel: "Cancel",
            AriaChatEndConfirm: "End chat",
            ConfirmCloseWindow: "Are you sure you want to close chat?",
            ConfirmCloseCancel: "Cancel",
            ConfirmCloseConfirm: "Close",
            AriaConfirmCloseCancel: "Cancel",
            AriaConfirmCloseConfirm: "Close",
            ActionsDownload: "Download transcript",
            ActionsEmail: "Email transcript",
            ActionsPrint: "Print transcript",
            ActionsCobrowseStart: "Start Co-browse",
            AriaActionsCobrowseStartTitle: "Opens the Co-browse session",
            ActionsSendFile: "Attach Files",
            AriaActionsSendFileTitle: "Opens a file upload dialog",
            ActionsEmoji: "Send Emoji",
            ActionsCobrowseStop: "Exit Co-browse",
            ActionsVideo: "Invite to Video Chat",
            ActionsTransfer: "Transfer",
            ActionsInvite: "Invite",
            InstructionsTransfer: "Open this link on another device to transfer your chat session</br></br><%link%>",
            InstructionsInvite: "Share this link with another person to add them to this chat session</br></br><%link%>",
            InviteTitle: "Need help?",
            InviteBody: "Let us know if we can help out.",
            InviteReject: "No thanks",
            InviteAccept: "Start chat",
            AriaInviteAccept: "Start chat",
            AriaInviteReject: "No thanks",
            ChatError: "There was a problem starting the chat session. Please retry.",
            ChatErrorButton: "OK",
            AriaChatErrorButton: "OK",
            ChatErrorPrimaryButton: "Yes",
            ChatErrorDefaultButton: "No",
            AriaChatErrorPrimaryButton: "Yes",
            AriaChatErrorDefaultButton: "No",
            DownloadButton: "Download",
            AriaDownloadButton: "Download",
            FileSent: "has sent:",
            FileTransferRetry: "Retry",
            AriaFileTransferRetry: "Retry",
            FileTransferError: "OK",
            AriaFileTransferError: "OK",
            FileTransferCancel: "Cancel",
            AriaFileTransferCancel: "Cancel",
            RestoreTimeoutTitle: "Chat ended",
            RestoreTimeoutBody: "Your previous chat session has timed out. Would you like to start a new one?",
            RestoreTimeoutReject: "No thanks",
            RestoreTimeoutAccept: "Start chat",
            AriaRestoreTimeoutAccept: "Start chat",
            AriaRestoreTimeoutReject: "No thanks",
            EndConfirmBody: "Would you really like to end your chat session?",
            EndConfirmAccept: "End chat",
            EndConfirmReject: "Cancel",
            AriaEndConfirmAccept: "End chat",
            AriaEndConfirmReject: "Cancel",
            SurveyOfferQuestion: "Would you like to participate in a survey?",
            ShowSurveyAccept: "Yes",
            ShowSurveyReject: "No",
            AriaShowSurveyAccept: "Yes",
            AriaShowSurveyReject: "No",
            UnreadMessagesTitle: "unread messages",
            AriaYouSaid: "You said",
            AriaSaid: "said",
            AriaSystemSaid: "System said",
            AriaWindowLabel: "Live Chat Window",
            AriaMinimize: "Live Chat Minimize",
            AriaMaximize: "Live Chat Maximize",
            AriaClose: "Live Chat Close",
            AriaEmojiStatusOpen: "Emoji picker dialog is opened",
            AriaEmojiStatusClose: "Emoji picker dialog is closed",
            AriaEmoji: "emoji",
            AriaCharRemaining: "Characters remaining",
            AriaMessageInput: "Message box",
            AsyncChatEnd: "End Chat",
            AsyncChatClose: "Close Window",
            AriaAsyncChatEnd: "End Chat",
            AriaAsyncChatClose: "Close Window",
            DayLabels: ["Sun", "Mon", "Tue", "Wed", "Thur", "Fri", "Sat"],
            MonthLabels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sept", "Oct", "Nov", "Dec"],
            todayLabel: "Today",
            Errors: {
              102: "First name is required.",
              103: "Last name is required.",
              161: "Please enter your name.",
              201: "The file could not be sent.<br/><strong><p class='filename' title='<%FilenameFull%>'>'<%FilenameTruncated%>'</p></strong><p class='cx-advice'>The maximum number of attached files would be exceeded (<%MaxFilesAllowed%>).</p>",
              202: "The file could not be sent.<br/><strong><p class='filename' title='<%FilenameFull%>'>'<%FilenameTruncated%>'</p></strong><p class='cx-advice'>Upload limit and/or maximum number of attachments would be exceeded (<%MaxAttachmentsSize%>).</p>",
              203: "The file could not be sent.<br/><strong><p class='filename' title='<%FilenameFull%>'>'<%FilenameTruncated%>'</p></strong><p class='cx-advice'>File type is not allowed.</p>",
              204: "We're sorry but your message is too long. Please write a shorter message.",
              240: "We're sorry but we cannot start a new chat at this time. Please try again later.",
              364: "Invalid email address.",
              401: "We're sorry but we are not able to authorize the chat session. Would you like to start a new chat?",
              404: "We're sorry but we cannot find your previous chat session. Would you like to start a new chat?",
              500: "We're sorry, an unexpected error occurred with the service. Would you like to close and start a new Chat?",
              503: "We're sorry, the service is currently unavailable or busy. Would you like to close and start a new Chat again?",
              ChatUnavailable: "We're sorry but we cannot start a new chat at this time. Please try again later.",
              CriticalFault:
                "Your chat session has ended unexpectedly due to an unknown issue. We apologize for the inconvenience.",
              StartFailed:
                "There was an issue starting your chat session. Please verify your connection and that you submitted all required information properly, then try again.",
              MessageFailed: "Your message was not received successfully. Please try again.",
              RestoreFailed: "We're sorry but we were unable to restore your chat session due to an unknown error.",
              TransferFailed: "Unable to transfer chat at this time. Please try again later.",
              FileTransferSizeError:
                "The file could not be sent.<br/><strong><p class='filename' title='<%FilenameFull%>'>'<%FilenameTruncated%>'</p></strong><p class='cx-advice'>File size is larger than the allowed size (<%MaxSizePerFile%>).</p>",
              InviteFailed: "Unable to generate invite at this time. Please try again later.",
              ChatServerWentOffline:
                "Messages are currently taking longer than normal to get through. We're sorry for the delay.",
              RestoredOffline: "Messages are currently taking longer than normal to get through. We're sorry for the delay.",
              Disconnected: "<div style='text-align:center'>Connection lost</div>",
              Reconnected: "<div style='text-align:center'>Connection restored</div>",
              FileSendFailed:
                "The file could not be sent.<br/><strong><p class='filename' title='<%FilenameFull%>'><%FilenameTruncated%></p></strong><p class='cx-advice'>There was an unexpected disconnection. Try again?</p>",
              Generic: "<div style='text-align:center'>An unexpected error occurred.</div>",
              "pureengage-v3-rest-INVALID_FILE_TYPE": "Invalid file type. Only Images are allowed.",
              "pureengage-v3-rest-LIMIT_FILE_SIZE": "File size is larger than the allowed size.",
              "pureengage-v3-rest-LIMIT_FILE_COUNT": "The maximum number of attached files exceeded the limit.",
              "pureengage-v3-rest-INVALID_CONTACT_CENTER": "Invalid x-api-key transport configuration.",
              "pureengage-v3-rest-INVALID_ENDPOINT": "Invalid endpoint transport configuration.",
              "pureengage-v3-rest-INVALID_NICKNAME": "First Name is required.",
              "pureengage-v3-rest-AUTHENTICATION_REQUIRED": "We're sorry but we are not able to authorize the chat session.",
              "purecloud-v2-sockets-400":
                "Sorry, something went wrong. Please verify your connection and that you submitted all required information properly, then try again.",
              "purecloud-v2-sockets-500": "We're are sorry, an unexpected error occurred with the service.",
              "purecloud-v2-sockets-503": "We're sorry, the service is currently unavailable.",
              "pureconnect-v4-rest-400":
                "Sorry, something went wrong. Please verify your connection and that you submitted all required information properly, then try again.",
              "pureconnect-v4-rest-500": "We're sorry, an unexpected error occurred with the service.",
              "pureconnect-v4-rest-503": "We're sorry, the service is currently unavailable.",
            },
          },
        },
      },
    },
  },
};

window._genesys.widgets.webchat = {
  apikey: "n3eNkgLLgLKXREBMYjGm6lygOHHOK8VA",
  dataURL: "https://api.genesyscloud.com/gms-chat/2/chat",
  userData: {},
  emojis: true,
  uploadsEnabled: false,
  confirmFormCloseEnabled: true,
  actionsMenu: true,
  maxMessageLength: 140,
  transport: {
    type: "purecloud-v2-sockets",
    dataURL: "https://api.usw2.pure.cloud", // replace with API URL matching your region
    deploymentKey: "6280e87e-9736-46bb-8bf5-f9a9709d945d", // replace with your Deployment ID
    orgGuid: "f1f87606-c4c5-4e7f-9fc7-ccf01c51e2c4", // replace with your Organization ID
    interactionData: {
      routing: {
        targetType: "QUEUE",
        targetAddress: "CHAT_Sales", //C1_DEMO queue
        priority: 2,
      },
    },
  },
  autoInvite: {
    enabled: false,
    timeToInviteSeconds: 10,
    inviteTimeoutSeconds: 30,
  },

  chatButton: {
    enabled: true,
    template:
      '<div class="cx-widget cx-webchat-chat-button cx-side-button" role="button" tabindex="0" data- message="ChatButton" data - gcb - service - node="true"><span class="cx-icon" data-icon="chat"></span><span class="i18n cx-chat-button-label" data-message="ChatButton"></span></div>',
    effect: "fade",
    openDelay: 1000,
    effectDuration: 300,
    hideDuringInvite: true,
  },
  form: {
    wrapper:
      "<div id='web-form'>" +
      "<div id='welcome-message'>" +
      "Hi there! <br><br> Thank you for contacting Armed Forces Insurance. We have a few questions to help us serve you better. </div>" +
      "</div>",
    inputs: [
      {
        id: "cx_webchat_form_lastName",
        name: "fullName",
        maxlength: "100",
        placeholder: "Required",
        label: "Full Name",
        required: true,
        wrapper: "<div class='form-group'><div class='label'>{label}</div><div class='chat-input'>{input}</div></div>",
        validate: isNotEmptyTwo,
      },
      {
        id: "cx_webchat_form_email",
        name: "email",
        maxlength: "100",
        placeholder: "Required",
        label: "Email",
        required: true,
        value: "",
        wrapper: "<div class='form-group'><div class='label'>{label}</div><div class='chat-input'>{input}</div></div>",
        validate: isNotEmptyTwo,
      },
      {
        id: "cx_webchat_form_phone",
        name: "phone",
        maxlength: "100",
        placeholder: "Optional",
        label: "Phone",
        value: "",
        wrapper: "<div class='form-group'><div class='label'>{label}</div><div class='chat-input'>{input}</div></div>",
      },
      {
        name: "cx_webchat_form_pagedata",
        label: "Page",
        type: "hidden",
        value: document.title,
      },
      {
        id: "cx_webchat_form_member_type",
        name: "memberType",
        placeholder: "Required",
        label: "Are you a current member?",
        type: "select",
        options: [
          {
            disabled: "disabled",
            selected: "selected",
            hidden: "hidden",
          },
          {
            text: "Yes",
            value: "yes",
          },
          {
            text: "No",
            value: "no",
          },
        ],
        wrapper: "<div class='form-group mt-30'><div class='label'>{label}</div><div class='chat-input'>{input}</div></div>",
      },
      {
        id: "cx_webchat_form_lineofbusiness",
        name: "lineOfBusiness",
        maxlength: "250",
        placeholder: "Optional",
        label: "What type of policy can we assist you with today?",
        value: "",
        wrapper: "<div class='form-group hidden'><div class='label'>{label}</div><div class='chat-input'>{input}</div></div>",
      },
      {
        id: "cx_webchat_form_membernumber",
        name: "memberNumber",

        maxlength: "100",
        placeholder: "Optional",
        label: "Member Number",
        value: "",
        wrapper:
          "<div class='form-group hidden ml-30'><div class='label'>{label}</div><div class='chat-input'>{input}</div></div>",
      },
      {
        id: "cx_webchat_form_need_type",
        name: "needType",
        placeholder: "Required",
        label: "Choose a Department:",
        type: "select",
        options: [
          {
            disabled: "disabled",
            selected: "selected",
            hidden: "hidden",
          },
          {
            text: "Billing",
            value: "CHAT_Billing",
          },
          {
            text: "Member Services",
            value: "CHAT_Member Services",
          },
          {
            text: "Claims",
            value: "CHAT_FNOL",
          },
          {
            text: "Specialty Lines",
            value: "CHAT_Specialty Lines",
          },
        ],
        wrapper:
          "<div class='form-group hidden mt-30'><div class='label'>{label}</div><div class='chat-input'>{input}</div></div>",
      },
      {
        id: "cx_webchat_form_billing_type",
        name: "billingType",
        placeholder: "Required",
        label: "Billing Questions",
        type: "select",
        options: [
          {
            disabled: "disabled",
            selected: "selected",
            hidden: "hidden",
          },
          {
            text: "Invoice",
            value: "CHAT_Billing",
          },
          {
            text: "Payment",
            value: "CHAT_Billing",
          },
          {
            text: "Late Payment",
            value: "CHAT_Payment Assistance",
          },
          {
            text: "Refund",
            value: "CHAT_Billing",
          },
        ],
        wrapper:
          "<div class='form-group hidden ml-30'><div class='label'>{label}</div><div class='chat-input'>{input}</div></div>",
      },
    ],
  },

  minimizeOnMobileRestore: false,

  ariaIdleAlertIntervals: [50, 25, 10],

  ariaCharRemainingIntervals: [75, 25, 10],
};