(function () {
  // Initialize some variables to hold simple feature state
  let chatStarted = false;
  let hasShownCallInMessage = 0;
  let inactiveCustomerState = -1;
  let isEndingChat = false;
  let hasBeenTransferred = false;
  let chatSettings = {
    debug: false,
    agentTransferMessage: "{AGENT_NAME} has transferred your chat. Another agent will be with you shortly.",
    genesysWebChatConfig: "https://afi.org/js/webchat.config.js",
    inactiveMessages: [
      {
        minutes: 2,
        message: "Hello, you've been inactive for a few minutes may I still assist you?",
      },
      {
        minutes: 4,
        message: "Hello, it's been a while since your last response do you still need assistance?",
      },
      {
        minutes: 5,
        message:
          "As it has been {MINUTES} minutes since your last response, I'll unfortunately have to end this chat session. If you still need further assistance, feel free to reach back out to us via chat or call us at {PHONE_NUMBER}",
      },
    ],
    minutesTillCallInMessage: "3",
    timezone: "America/Chicago",
    hours: {
      sunday: {
        startOfDay: "00:00",
        endOfDay: "00:00",
      },
      monday: {
        startOfDay: "08:00",
        endOfDay: "18:00",
      },
      tuesday: {
        startOfDay: "08:00",
        endOfDay: "18:00",
      },
      wednesday: {
        startOfDay: "08:00",
        endOfDay: "18:00",
      },
      thursday: {
        startOfDay: "08:00",
        endOfDay: "18:00",
      },
      friday: {
        startOfDay: "08:00",
        endOfDay: "18:00",
      },
      saturday: {
        startOfDay: "00:00",
        endOfDay: "00:00",
      },
    },
    queues: {
      CHAT_Billing: {
        phoneNumber: "1-855-593-5556",
      },
      CHAT_FNOL: {
        phoneNumber: "1-855-810-2473",
      },
      "CHAT_Member Services": {
        phoneNumber: "1-855-307-7843",
      },
      "CHAT_Payment Assistance": {
        phoneNumber: "1-855-425-0258",
      },
      CHAT_Sales: {
        phoneNumber: "1-855-567-3681",
      },
      "CHAT_Specialty Lines": {
        phoneNumber: "1-855-699-3027",
      },
    },
    callInMessage:
      "We appreciate your patience while holding. We are currently experiencing a high chat volume and apologize for the inconvenience.  Please stay on the chat for the next available agent or you can call us at {PHONE_NUMBER}.",
    chatMessageHtml:
      '<div class="cx-message-group"><div class="cx-message cx-system i18n cx-NewTextBubble" tabindex="0" data-message="{CHAT_MESSAGE}" data-message-type="transcript" id="" style=""><div class="cx-bubble"><span aria-hidden="true" class="cx-name"></span><span class="aria-name" data-message="AriaTheySaid" role="text" aria-label="System said"></span><div class="cx-message-text">{CHAT_MESSAGE}</div><div class="cx-common-screen-reader cx-space" tabindex="-1" aria-hidden="true">&nbsp;&nbsp;&nbsp;</div><span class="cx-time">{CHAT_TIME}</span></div></div></div>',
  };

  CXBus.configure({ debug: false, pluginsPath: "https://apps.mypurecloud.com/widgets/9.0/plugins/" });
  CXBus.loadFile(chatSettings.genesysWebChatConfig).done(function () {
    CXBus.loadPlugin("widgets-core");
  });

  function updateEndChatButton() {
    $(".cx-button-close").text("End Chat").css("width", "auto");
  }

  setTimeout(function () {
    const AFIWebChat = window._genesys.widgets.bus?.registerPlugin("AFIWebChat");
    AFIWebChat?.subscribe("WebChatService.started", function (e) {
      updateEndChatButton();
    });

    AFIWebChat?.subscribe("WebChat.closed", function (e) {
      resetState();
    });
  }, 2000);

  function getNow() {
    return moment.tz(chatSettings.timezone);
  }

  function getToday() {
    return getNow().format("dddd").toLowerCase();
  }

  function getTodaysDate() {
    return getNow().format("YYYY-MM-DD");
  }

  function getTimeOfDay(start = true) {
    let key = start ? "startOfDay" : "endOfDay";
    let date = getTodaysDate();
    let time = getTodaysSchedule()[key];
    return moment.tz(`${date} ${time}`, "YYYY-MM-DD hh:mm", chatSettings.timezone);
  }

  function getTodaysSchedule() {
    let today = getToday();
    return chatSettings.hours[today];
  }

  function isNowInSchedule() {
    let start = getTimeOfDay(true);
    let end = getTimeOfDay(false);
    let now = getNow();
    if (chatSettings.debug) {
      $("#start-time").text(start.format("hh:mm A"));
      $("#end-time").text(end.format("hh:mm A"));
    }
    return now.isAfter(start) && now.isBefore(end);
  }

  function showOrHideChatButton() {
    let now = getNow();
    if (isNowInSchedule()) {
      $("#schedule-decision").text(now.format("hh:mm A") + " is in schedule");
      $(".cx-widget.cx-webchat-chat-button").show();
    } else {
      $("#schedule-decision").text(now.format("hh:mm A") + " is NOT in schedule");
      $(".cx-widget.cx-webchat-chat-button").hide();
    }
    logTarget();
  }

  function logTarget() {
    if (chatSettings.debug) {
      $("#chat-target").text(window._genesys.widgets.webchat.transport.interactionData.routing.targetAddress);
    }
  }

  function detectChatStarted() {
    let $transcript = getChatTranscriptElement();
    if ($transcript) {
      return $transcript.text().includes("Chat Started");
    }
    return false;
  }

  function detectChatEnded() {
    let $transcript = getChatTranscriptElement();
    if ($transcript) {
      return $transcript.text().includes("Chat Ended");
    }
    return false;
  }

  function detectAgentDisconnected() {
    let $transcript = getChatTranscriptElement();
    if ($transcript) {
      return $transcript.text().includes("Disconnected");
    }
    return false;
  }

  function detectAgentConnected() {
    let $transcript = getChatTranscriptElement();
    if ($transcript) {
      return $transcript.text().includes("Connected");
    }
    return false;
  }

  function getChatTranscriptElement() {
    let $transcript = $(".cx-transcript");
    if ($transcript && $transcript.length > 0) {
      return $transcript;
    }
    return null;
  }

  function getChatGroupElements() {
    let $transcript = getChatTranscriptElement();
    if ($transcript) {
      return $transcript.find(".cx-message-group");
    }
    return null;
  }

  function getChatStartedTime() {
    let startTime = null;
    let $chatGroups = getChatGroupElements();
    if ($chatGroups && $chatGroups.length > 0) {
      $chatGroups.each(function (i, chatGroup) {
        if ($(chatGroup).text().includes("Chat Started")) {
          startTime = $(chatGroup).find(".cx-time").text();
        }
      });
    }
    return startTime;
  }

  function log(message) {
    if (chatSettings.debug) {
      console.table(message);
    }
  }

  function getAgentConnectedTime() {
    let connectedTime = null;
    let $chatGroups = getChatGroupElements();
    if ($chatGroups && $chatGroups.length > 0) {
      $chatGroups.each(function (i, chatGroup) {
        if ($(chatGroup).text().includes("Connected")) {
          connectedTime = $(chatGroup).find(".cx-time").text();
        }
      });
    }
    return connectedTime;
  }

  function getLastCustomerChatTime() {
    return $(".cx-message.cx-participant.cx-you:last").find(".cx-time").text();
  }

  function resetState() {
    chatStarted = false;
    isEndingChat = false;
    hasShownCallInMessage = false;
  }

  function endChat() {
    console.log("Chat Ended");
    isEndingChat = true;
    setTimeout(function () {
      $(".cx-webchat .cx-button-close").click();
      setTimeout(function () {
        $(".cx-webchat .cx-end-confirm ").click();
        resetState();
      }, 500);
    }, 30000);
    inactiveCustomerState = -1;
  }

  function handleInQueueMessaging() {
    chatStarted = detectChatStarted() && !detectChatEnded() && !detectAgentConnected();
    if (chatStarted) {
      let startTime = getChatStartedTime();
      let callInMessageTime = moment(startTime, "hh:mm A").add(chatSettings.minutesTillCallInMessage, "minutes");
      let now = getNow();
      if (now.isAfter(callInMessageTime)) {
        if (!hasShownCallInMessage) {
          addCallInMessage();
        }
      }
    }
  }
  function handleIdleCustomer() {
    let inactiveMessages = chatSettings.inactiveMessages;
    const isValidState =
      !hasBeenTransferred &&
      !isEndingChat &&
      inactiveMessages &&
      detectChatStarted() &&
      !detectChatEnded() &&
      detectAgentConnected() &&
      inactiveCustomerState < inactiveMessages.length;
    if (isValidState) {
      let customerLastChatTime = getLastCustomerChatTime();
      log({ method: "handleIdleCustomer", customerLastChatTime, source: "getAgentConnectedTime" });
      // if the customer hasn't responded at all, use the agent connected time
      if (!customerLastChatTime) {
        customerLastChatTime = getAgentConnectedTime();
        log({ method: "handleIdleCustomer", customerLastChatTime, source: "getAgentConnectedTime" });
      }
      if (customerLastChatTime) {
        log({ method: "handleIdleCustomer", customerLastChatTime, inactiveCustomerState });
        let now = getNow();
        inactiveMessages.forEach(function (inactiveMessage, index) {
          let compareTime = moment(customerLastChatTime, "hh:mm A").add(inactiveMessage.minutes, "minutes");
          if (index > inactiveCustomerState) {
            if (now.isAfter(compareTime)) {
              log({ method: "handleIdleCustomer", customerLastChatTime, inactiveCustomerState, now, compareTime });
              inactiveCustomerState++;
              addInactiveMessage();
              if (inactiveCustomerState === inactiveMessages.length - 1) {
                endChat();
              }
            }
          }
          if (index === 0 && now.isBefore(compareTime)) {
            inactiveCustomerState = -1;
          }
        });
      }
    } else {
      inactiveCustomerState = -1;
    }
  }

  function handleAgentTransfer() {
    let mightHaveBeenTransferred = detectAgentDisconnected() && !detectChatEnded();
    if (mightHaveBeenTransferred) {
      // give it .5 seconds and make sure that the "Chat ended" still hasn't been added
      // so we know for sure that this is a transfer
      setTimeout(() => {
        if (!detectChatEnded()) {
          hasBeenTransferred = true;
          updateAgentTransferMessage();
        }
      }, 500);
    }
  }

  function updateAgentTransferMessage() {
    const systemMessages = $(".cx-message.cx-system .cx-message-text");
    if (systemMessages.length > 0) {
      systemMessages.each((i, e) => {
        const $e = $(e);
        const currentText = $e.text();
        console.log(currentText);
        if (currentText.includes("Disconnected")) {
          console.log("includes Disconnected");
          const regex = /(.+)\s* Disconnected/m;
          const agentName = currentText.match(regex)[1] || "Agent";
          let newText = "{AGENT_NAME} has transferred your chat. Another agent will be with you shortly.";
          //let newText = chatSettings.agentTransferMessage;
          newText = newText.replace("{AGENT_NAME}", agentName);
          $e.text(newText);
        }
      });
    }
  }

  function addCallInMessage() {
    let selectedQueue = window._genesys.widgets.webchat.transport.interactionData.routing.targetAddress;
    let queueSettings = chatSettings.queues[selectedQueue];
    addChatMessage(queueSettings, chatSettings.callInMessage);
    hasShownCallInMessage = true;
  }

  function addInactiveMessage() {
    let selectedQueue = window._genesys.widgets.webchat.transport.interactionData.routing.targetAddress;
    let queueSettings = chatSettings.queues[selectedQueue];
    let inactiveMessage = chatSettings.inactiveMessages?.[inactiveCustomerState]?.message;
    let minutes = chatSettings.inactiveMessages?.[inactiveCustomerState]?.minutes;
    inactiveMessage = inactiveMessage.replaceAll("{MINUTES}", minutes);
    addChatMessage(queueSettings, inactiveMessage);
  }

  function addChatMessage(queueSettings, message) {
    let $transcript = getChatTranscriptElement();
    if ($transcript && queueSettings && message) {
      let chatTime = getNow().format("hh:mm A");
      let callInNumber = queueSettings.phoneNumber;
      let callInMessage = message.replaceAll("{PHONE_NUMBER}", callInNumber);
      let callInHtml = chatSettings.chatMessageHtml
        .replaceAll("{CHAT_MESSAGE}", callInMessage)
        .replaceAll("{CHAT_TIME}", chatTime);
      $transcript.append(callInHtml);
      scrollChat();
    }
  }

  function scrollChat() {
    let $transcript = getChatTranscriptElement();
    if ($transcript) {
      $transcript.scrollTop($transcript[0].scrollHeight);
    }
  }

  // Set up some timers to poll the dom and react to chat state
  // this is done on the front end because Genesys doesn't support
  // in queue messaging for the inbound chat flow
  setInterval(handleIdleCustomer, 1000);

  setInterval(handleInQueueMessaging, 1000);

  setInterval(showOrHideChatButton, 1000);

  setInterval(handleAgentTransfer, 1000);

  // Attach handlers to the chat form elements to update the
  $(document).ready(function () {
    $("#cx_webchat_form_member_type").parent().parent().removeClass("hidden");
  });

  $(document).on("click", ".cx-webchat-chat-button", function () {
    setTimeout(updateEndChatButton, 300);
  });

  $(document).on("change", "#cx_webchat_form_member_type", function () {
    if ($(this).val() === "yes") {
      $("#cx_webchat_form_membernumber").parent().parent().removeClass("hidden");
      $("#cx_webchat_form_need_type").parent().parent().removeClass("hidden");
      $("#cx_webchat_form_lineofbusiness").parent().parent().addClass("hidden");
      window._genesys.widgets.webchat.transport.interactionData.routing.targetAddress = "CHAT_Member Services";
    } else {
      $("#cx_webchat_form_lineofbusiness").parent().parent().removeClass("hidden");
      $("#cx_webchat_form_membernumber").val("").parent().parent().addClass("hidden");
      $("#cx_webchat_form_need_type").val(0).parent().parent().addClass("hidden");
      $("#cx_webchat_form_billing_type").val(0).parent().parent().addClass("hidden");
      window._genesys.widgets.webchat.transport.interactionData.routing.targetAddress = "CHAT_Sales";
    }
    logTarget();
  });

  $(document).on("change", "#cx_webchat_form_need_type", function () {
    let value = $(this).val();
    if (value === "CHAT_Billing") {
      $("#cx_webchat_form_billing_type").parent().parent().removeClass("hidden");
    } else {
      $("#cx_webchat_form_billing_type").val(0).parent().parent().addClass("hidden");
    }
    window._genesys.widgets.webchat.transport.interactionData.routing.targetAddress = value;
    logTarget();
  });

  $(document).on("change", "#cx_webchat_form_billing_type", function () {
    window._genesys.widgets.webchat.transport.interactionData.routing.targetAddress = $(this).val();
    logTarget();
  });
})();
