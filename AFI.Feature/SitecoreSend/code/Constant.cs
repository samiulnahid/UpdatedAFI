using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.SitecoreSend
{
    public class Constant
    {
        public struct MoosendSetting
        {
            public static readonly ID MoosendSettingTemplateID = new ID("{3C89EA08-B884-49B5-B812-164A65AE7DF5}");
            public static readonly ID MoosendSettingId = new ID("{3CD768BF-22CC-4BC2-8F8B-62C8328B42AC}");
            public struct Fields
            {
                public static readonly ID APIURL = new ID("{E19BED10-C07A-4AEB-B7ED-49A9C561D4DC}");
                public static readonly ID APIKEY = new ID("{56C3357E-FDD7-4E8F-9E1F-C32ABB6A5F15}");
                public static readonly ID AFIKEY = new ID("{0365F217-DE80-4E9B-AE96-873DA81FF6C1}");
                public static readonly ID WelcomeListID = new ID("{1B6749CA-F60C-4A19-85CF-A0AB04D25808}");
                public static readonly ID DedicatedListID = new ID("{F271F6DC-7FB4-435A-B299-FFD503610C75}");
                public static readonly ID OtherListID = new ID("{5DB6BA31-FABC-46A5-9C19-469BA484A9E0}");
            }
        }

        public struct MoosendMessage
        {
            // Success
            public const string Success = "Successfully Submitted";
            public const string SuccessCode = "200";
            public const string ErrorCode = "400";
            public const string SuccessImport = "Thank you for confirming that your data has been imported successfully. We will synchronize it to the Sitecoresend system at midnight.";

            //// Client Errors
            public const int BadRequest = 400;
            public const string BadRequestMessage = "Bad Request! Something went wrong, please try again later.";

            public const int Unauthorized = 401;
            public const string UnauthorizedMessage = "Unauthorized. Please be authenticate first.";

            public const int AccessDenied = 403;
            public const string AccessDeniedMessage = "Access Denied: You are not allowed to access this.";

            public const int NotFound = 404;
            public const string NotFoundMessage = "Content Not Found! Please try again.";

            public const int MethodNotAllowed = 405;
            public const string MethodNotAllowedMessage = "Method Not Allowed: The method specified in the request is not allowed. Please try again.";

            public const int TooManyRequests = 429;
            public const string TooManyRequestsMessage = "Too Many Requests from client.";

            // Server Errors
            public const int InternalServerError = 500;
            public const string InternalServerErrorMessage = "Something went wrong, please try again later.";

            // others
            public const string Update = "Successfully Updated.";

            public const string MissingSecurityCode = "Please enter valid security token.";

            public const string EmptyField = "Required field cannot be empty.";

            public const string DuplicateList = "This name already has been inserted, Please try another.";

            public const string EmptyData = "No data is available.";

            public const string ListNameInvalid = "Please enter correct list name.";

            public const string SecurityCodeMessage = "The provided token is valid for the next 1 hour.";

            public const string InvalidToken = "Token is invalid. Please ensure the correct token or request a new one. ";



        }
    }
}