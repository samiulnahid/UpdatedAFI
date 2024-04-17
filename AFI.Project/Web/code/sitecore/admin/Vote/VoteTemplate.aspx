<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VoteTemplate.aspx.cs" %> <%@ Register TagPrefix="sc"
Namespace="Sitecore.Mvc" Assembly="Sitecore.Mvc" %> <%@ Register TagPrefix="scf" Namespace="Sitecore.ExperienceForms.Mvc"
Assembly="Sitecore.ExperienceForms.Mvc" %>

<!DOCTYPE html>

<html>
  <head runat="server">
    <!-- <meta name="viewport" content="width=device-width, initial-scale=1.0" /> -->
    <title>AFI Vote</title>
    <link rel="icon" type="image/png" sizes="32x32" href="/-/media/Project/AFI/afi/FavIcon/favicon-32x32.png" />

    <link rel="stylesheet" href="icon/css/all.min.css" />
    <link rel="stylesheet" href="font/afi-fonts.css" />
    <link rel="stylesheet" href="css/common.css" />
    <link rel="stylesheet" href="css/vote-form.css" />
    <link rel="stylesheet" href="css/vote-panel.css" />
    <link rel="stylesheet" href="css/vote_information.css" />
  </head>
  <body class="configure-page">
    <form id="form1" runat="server">
      <!-- <header>
        <div class="icon-container">
          <a href="/sitecore/shell/sitecore/client/applications/launchpad" title="Back to Lunchpad" class="back"
            ><img src="./image/back.svg" alt="back"
          /></a>
          <a href="/sitecore/admin/vote/configure-vote.aspx?sc_bw=1" title="Vote configure" class="setting">
            <img src="./image/gear.png" alt="configure" style="width: 40px" />
          </a>
        </div>

        <div class="user-info">
          <asp:Button
            ID="btnLogout"
            usesubmitbehavior="false"
            type="button"
            runat="server"
            Text="Logout"
            OnClick="btnLogout_Click"
            class="logout-btn"
          />
          <asp:Label ID="lblUsername" runat="server" Text="" class="username"></asp:Label>

          <img src="./image/avatar.png" alt="User Photo" class="user-photo" />
        </div>
      </header>
      <div class="logo" style="margin: 50px auto;width: 200px;">
        <img src="./image/logo.png" alt="Logo" />
      </div>
      <div class="box configure-box">
    

        <sc:Placeholder runat="server" Key="FormContent" />

     
      </div> -->

      <!-- Download PDF button -->
      <section class="vote-step-two">
        <div class="contbtn pdf_btn">
          <input value="Download PDF" type="button" class="continue button button--large-text" id="downloadPdfButton" />
        </div>
      </section>

      <!-- Vote One  -->
      <section>
        <div class="login-form-container">
          <a class="afi_logo" href="https://afi.org/en/">
            <img src="./image/logo.png" alt="Logo" />
          </a>

          <h2 class="vote_title">Vote For 2024 AFIE Subscribers' Advisory Committee</h2>

          <p class="text_msg" id="help_text_msg">
            Please enter the Member Number. The rest of the fields will populate automatically.
          </p>

          <p class="text_msg" id="required_text_msg" style="display: none">Please fill the required information.</p>

          <section class="form_info_vote">
            <div class="form-group">
              <label for="afi_member_number">AFI Member Number:</label>
              <input type="text" id="afi_member_number" class="txtVoteMemberNumber" name="afi_member_number" required />
            </div>

            <div class="form-group">
              <label for="pin">PIN:</label>
              <input type="text" id="pin" class="txtVotePIN" name="pin" required />
            </div>

            <div class="form-group">
              <label for="email">Email:</label>
              <input type="email" id="email" class="txtVoteEmail" name="email" />
            </div>

            <div class="form-group">
              <label for="name">Name:</label>
              <input type="text" id="name" class="txtVoteName" name="name" required />
            </div>
          </section>
        </div>
      </section>

      <!-- Vote Two -->
      <section>
        <div id="content">
          <div class="vote-step-two">
            <div class="form">
              <div id="vote_step_two">
                <!-- <div id="vote_step_two_logo" style="text-align: center">
                <a title="Image 1" href="https://afi.org/en/"><img src="./image/logo.png" alt="afi" /></a>
              </div> -->

                <div id="vote_step_two_title">
                  <h2>Vote For 2024 AFI Subscribers' Advisory Committee</h2>
                </div>

                <div id="vote_step_two_name">
                  <h4 class="txt-dear">Dear <span id="txtDearMemberName"> </span>,</h4>
                </div>

                <div id="vote_step_two_text_2">
                  <p data-sc-field-key="43BB6842D7104574A37E5EF4B9D6C094" class="">
                    KNOW ALL PERSONS BY THESE PRESENTS, that the undersigned member of Armed Forces Insurance Exchange hereby
                    appoints LT COL Michael J Yaguchi, USAF (Ret) as proxy, with full power of substitution, for and in the name
                    of the undersigned, at the Annual Meeting of the members of the Exchange to be held in a virtual format from
                    the Armed Forces Insurance Headquarters, on the 23rd day of June 2024, commencing at 1700 hours, with respect
                    to the following:
                  </p>
                </div>

                <div id="vote_step_two_text_2">
                  <p data-sc-field-key="8C0F914E6B074DFE8B1C9CFD6185FC34" class="txt-no">
                    1. To receive and consider reports of the Attorney-in-Fact's Board of Directors.
                  </p>
                  <p data-sc-field-key="0F4F7BC47C7146E49735D19A62F614CB" class="txt-no">
                    2. To elect a member to the Exchange's Subscribers' Advisory Committee.
                  </p>
                </div>
              </div>

              <section class="main offscreen__darken" role="main" id="main-content">
                <form method="post" novalidate>
                  <div class="form___wrapper">
                    <div class="txtbody dv-content-vote-bottom">
                      <h3 class="txt-title">Candidates</h3>

                      <p class="txt-candidate white__text">Please vote for or against the candidate below:</p>

                      <div class="person first">
                        <a
                          href="https://afi.org/about-afi/leadership/subscribers-advisory-committee/dr-christina-love"
                          target="_blank"
                          >Dr. Christina D. Love</a
                        >
                        <label class="rdo-css">
                          <input type="radio" name="Dr. Christina D. Love" value="For" /><span></span>
                          For
                        </label>

                        <label class="rdo-css against">
                          <input type="radio" name="Dr. Christina D. Love" value="Against" /><span></span>Against
                        </label>
                      </div>

                      <div class="person">
                        <a
                          href="https://afi.org/about-afi/leadership/subscribers-advisory-committee/colonel-kay-k-wakatake"
                          target="_blank"
                          >Colonel Kay Wakatake, U.S. Army, Retired</a
                        >
                        <label class="rdo-css">
                          <input type="radio" name="Colonel Kay Wakatake, U.S. Army, Retired" value="For" /><span></span>
                          For
                        </label>
                        <label class="rdo-css against">
                          <input type="radio" name="Colonel Kay Wakatake, U.S. Army, Retired" value="Against" /><span></span>
                          Against
                        </label>
                      </div>

                      <div class="person">
                        <a
                          href="https://afi.org/about-afi/leadership/subscribers-advisory-committee/colonel-david-l-musgrave"
                          target="_blank"
                          >Colonel David L. Musgrave, Army, Retired</a
                        >
                        <label class="rdo-css">
                          <input type="radio" name="Colonel David L. Musgrave, Army, Retired" value="For" /><span></span>
                          For
                        </label>
                        <label class="rdo-css against">
                          <input type="radio" name="Colonel David L. Musgrave, Army, Retired" value="Against" /><span></span>
                          Against
                        </label>
                      </div>

                      <div class="error__message__wrapper">
                        <span class="error__message">Please fill all the required fields.</span>
                      </div>
                      <!-- <div class="contbtn"></div> -->
                    </div>
                  </div>
                </form>
              </section>
            </div>
          </div>
        </div>
      </section>

      <!-- ******************* Loader ******************* -->
      <div id="page-loader">
        <img src="./image/loader-logo.png" alt="logo" />
      </div>

      <script>
        function startLoader() {
          const pageLoader = document.getElementById("page-loader");
          if (pageLoader) pageLoader.style.display = "block";
        }

        function stopLoader() {
          const pageLoader = document.getElementById("page-loader");
          if (pageLoader) pageLoader.style.display = "none";
        }

        window.addEventListener("load", stopLoader);
      </script>

      <!-- ************************************************ -->
      <!--                        SCRIPT                    -->
      <!-- ************************************************ -->

      <script type="text/javascript" src="js/jquery.min.js"></script>
      <script type="text/javascript" src="js/html2pdf.bundle.min.js"></script>

      <script>
        document.getElementById("downloadPdfButton").addEventListener("click", function () {
          const memberNumValue = document.getElementById("afi_member_number")?.value;

          if (!memberNumValue) {
            const confirmation = confirm("Are you sure you want to download the PDF with blank fields?");
            if (!confirmation) return;
          }

          const button = document.querySelector(".pdf_btn");
          const container = document.querySelector(".vote-step-two .form");
          const loginFormContainer = document.querySelector(".login-form-container");
          const voteTwoBottom = document.querySelector(".vote-step-two .dv-content-vote-bottom");
          const help_text_msg = document.querySelector("#help_text_msg");
          const required_text_msg = document.querySelector("#required_text_msg");
          const mainContent = document.querySelector("#main-content");

          button.style.display = "none";
          help_text_msg.style.display = "none";
          required_text_msg.style.display = "block";
          loginFormContainer.style.margin = "auto";
          container.style.gridTemplateColumns = "1fr";
          voteTwoBottom.style.padding = "0 40px";
          mainContent.style.display = "block";

          const element = document.body;
          const options = {
            filename: "vote_template_" + memberNumValue + ".pdf",
            margin: [8, 2, 2, 2],
            pagebreak: { mode: "avoid-all" },
            jsPDF: { unit: "mm", format: "a4", orientation: "portrait" },
          };
          html2pdf().set(options).from(element).save();

          setTimeout(function () {
            button.style.display = "";
            help_text_msg.style.display = "";
            required_text_msg.style.display = "none";
            loginFormContainer.style.margin = "50px auto";
            container.style.gridTemplateColumns = "1fr 1fr";
            voteTwoBottom.style.padding = 40;
            mainContent.style.display = "grid";
          }, 1);
        });
      </script>

      <script>
        $(document).ready(function () {
          // on member number change call api function with debounce
          $(".txtVoteMemberNumber").on("input", function () {
            if (window.myDebounce) clearTimeout(window.myDebounce);

            window.myDebounce = setTimeout(getFormField, 1000);
          });

          // call api to get field data
          function getFormField() {
            var afiMemberNumber = $(".txtVoteMemberNumber").val();

            if (afiMemberNumber) {
              const url = `/api/sitecore/AFIReport/GetMemberInfoByMemberNumber?memberNumber=${afiMemberNumber}`;

              $.ajax({
                type: "GET",
                url: url,
                dataType: "json",
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                data: {},
                success: function (data) {
                  // Parse the JSON data
                  var jsonResponse = JSON.parse(data);
                  // Check if response has Success field
                  if ("Success" in jsonResponse) {
                    // Handle case where data not found. Success is False
                    if (!jsonResponse.Success) {
                      $(".txtVoteEmail").val("");
                      $(".txtVotePIN").val("");
                      $(".txtVoteName").val("");
                      $("#txtDearMemberName").html("");
                    }
                  } else {
                    // Response does not have Success field, set values to respective fields
                    var email = jsonResponse.EmailAddress || "";
                    var pin = jsonResponse.PIN || "";
                    var name = jsonResponse.FullName || "";
                    $(".txtVoteEmail").val(email);
                    $(".txtVotePIN").val(pin);
                    $(".txtVoteName").val(name);
                    $("#txtDearMemberName").html(name);
                  }
                },
                error: function (xhr, status, error) {
                  // Handle error
                  console.error("Error occurred while fetching data:", error);
                  $(".txtVoteEmail").val("");
                  $(".txtVotePIN").val("");
                  $(".txtVoteName").val("");
                  $("#txtDearMemberName").html("");
                },
              });
            }
          }
        });
      </script>
    </form>
  </body>
</html>

<script runat="server">
  protected void Page_Load(object sender, EventArgs e)
  {
      if (!IsPostBack)
      {
          // Retrieve the logged-in username
          string username = GetLoggedInUsername();

          // Display the username
          // lblUsername.Text = username;
      }
  }

  protected void btnLogout_Click(object sender, EventArgs e)
  {
      // Log out the user
      Sitecore.Security.Authentication.AuthenticationManager.Logout();

      // Redirect to a logout page or the login page
      Response.Redirect("/sitecore/login?fbc=1");
  }

  private string GetLoggedInUsername()
  {
      // Get the currently logged-in user
      Sitecore.Security.Accounts.User user = Sitecore.Context.User;

      // Get the username
      string username = string.Empty;
      if (user != null)
      {
          if (user.Profile != null && !string.IsNullOrEmpty(user.Profile.FullName))
          {
              username = user.Profile.FullName;
          }
          else
          {
              username = user.Name;
          }
      }
      return username;
  }
</script>
