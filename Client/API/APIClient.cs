using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Text;

namespace BlazorWasm.Client.API
{
    public class APIClient
    {
        private string _Server = "";

        //private readonly OidcClient _client = default!;
        private string? _currentAccessToken;

        public APIClient(string server)
        {
            _Server = server; //https://demo.duendesoftware.com/api/test

            //_Client = new HttpClient();
        }

        public async Task<string> GetRootContent()
        {
            var client = new HttpClient();
            var exchangeResponse = await client.RequestTokenExchangeTokenAsync(new TokenExchangeTokenRequest
            {
                Address = "https://localhost:5001/connect/token",
                GrantType = OidcConstants.GrantTypes.TokenExchange,

                ClientId = "spa",
                ClientSecret = "secret",

                SubjectToken = "", //bearerToken.AccessToken,
                SubjectTokenType = OidcConstants.TokenTypeIdentifiers.AccessToken
            });

            return "";
        }

        //private async Task<bool> AccessTokenActive()
        //{
        //    try
        //    {
        //        if (
        //               (_AccessToken == null) ||
        //               ((_AccessToken != null) && (!_AccessToken.IsValid()))
        //           )
        //        {
        //            //Access token either doesn't exist, or it expired -> try to create a new one
        //            _AccessToken = await GetAccessToken();
        //        }
        //    }
        //    catch
        //    {

        //    }

        //    return ((_AccessToken != null) && _AccessToken.IsValid());
        //}

        //private async Task<IAccessTokenProvider> GetAccessToken()
        //{
        //    IAccessTokenProvider atp = new AccessTokenProvider();

        //    try
        //    {
        //        if (_Client == null) _Client = new HttpClient();

        //        // Encode client_id and client_secret in base64
        //        string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_ClientId}:{_ClientSecret}"));

        //        // Prepare form data
        //        var formData = new FormUrlEncodedContent(new[]
        //        {
        //                new KeyValuePair<string, string>("grant_type", "client_credentials"),
        //                new KeyValuePair<string, string>("scope", "global")
        //            });

        //        // Add authorization header
        //        _Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

        //        // Perform POST request to obtain token
        //        HttpResponseMessage response = await _Client.PostAsync($"{_ServerName}/token", formData);

        //        // Read response content
        //        string responseContent = await response.Content.ReadAsStringAsync();

        //        // Check if request was successful
        //        if (response.IsSuccessStatusCode)
        //        {
        //            // Parse JSON response to get access token
        //            dynamic? jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);
        //            if (jsonResponse != null)
        //            {
        //                string sAccess_token = jsonResponse.access_token;
        //                double dExpires_In = jsonResponse.expires_in;
        //                if (dExpires_In > 0) atp = new AccessTokenProvider(sAccess_token, dExpires_In);
        //            }
        //        }
        //        else
        //        {
        //            // Request failed, throw exception or handle appropriately
        //            throw new Exception($"Failed to retrieve access token: {response.StatusCode} - {responseContent}");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exception
        //        Console.WriteLine($"An error occurred: {ex.Message}");
        //    }

        //    return atp;
        //}

        //#region V4 endpoints

        ///// <summary>
        ///// Returns VCard UUID, server name and IsSecureCard which will be used to download virtual cards.
        ///// IsSecureCard value 1 indicates, virtual card is secured with security code.
        ///// This function requires site name, access token, configuration name and email address of the vcard user
        ///// </summary>
        ///// <param name="confName"></param>
        ///// <param name="siteName"></param>
        ///// <param name="email"></param>
        ///// <returns></returns>
        //public async Task<cResponseMessage_UUID?> GetUUID(string confName, string siteName, string email)
        //{
        //    cResponseMessage_UUID? uuid = null;

        //    #region Mandatory fields

        //    bool bMandatoryFieldsOK = true;
        //    try
        //    {
        //        if (confName.Length < 1) throw new Exception($"confName is a mandatory parameter");
        //        if (siteName.Length < 1) throw new Exception($"siteName is a mandatory parameter");
        //        if (email.Length < 1) throw new Exception($"email is a mandatory parameter");
        //    }
        //    catch (Exception ex)
        //    {
        //        bMandatoryFieldsOK = false;
        //        Console.WriteLine($"GetUUID: An error occurred: {ex.Message}");
        //    }

        //    #endregion Mandatory fields
        //    if (bMandatoryFieldsOK)
        //    {
        //        string responseContent = await HttpResponseMessage_GET("GetUUIDV4", $"confName={confName}&siteName={siteName}&email={email}");
        //        if (responseContent.Length > 0)
        //        {
        //            // Parse JSON response to get values
        //            uuid = Newtonsoft.Json.JsonConvert.DeserializeObject<cResponseMessage_UUID>(responseContent);
        //        }
        //    }

        //    return uuid;
        //}

        //#endregion V4 endpoints
        //#region V3 endpoints

        ///// <summary>
        ///// Returns a status to check whether VCard excel file(s) has been imported successfully or not.
        ///// 'yes' indicates file imported successfully and 'no' indicates file not imported yet and still in progress.
        ///// </summary>
        ///// <returns></returns>
        //public async Task<string> GetFileImportedStatus()
        //{
        //    string sStatus = "";
        //    string responseContent = await HttpResponseMessage_GET("GetFileImportedStatusV3");
        //    if (responseContent.Length > 0)
        //    {
        //        // Parse JSON response to get values
        //        dynamic? jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);
        //        if (jsonResponse != null) sStatus = jsonResponse.isFileImported;
        //    }

        //    return sStatus;
        //}

        ///// <summary>
        ///// Returns a list of imported VCard history records associated to the current user.
        ///// Import VCard history records object has ImportStatus, SiteName, Email and ImportDate.
        ///// </summary>
        ///// <param name="siteID"></param>
        ///// <returns></returns>
        //public async Task<List<cResponseMessage_ImportHistory>?> GetImportHistory(Int64 siteID)
        //{
        //    List<cResponseMessage_ImportHistory>? responses = null;

        //    #region Mandatory fields

        //    bool bMandatoryFieldsOK = true;
        //    try
        //    {
        //        if (siteID < 1) throw new Exception($"siteID is a mandatory parameter");
        //    }
        //    catch (Exception ex)
        //    {
        //        bMandatoryFieldsOK = false;
        //        Console.WriteLine($"UploadPhotos: An error occurred: {ex.Message}");
        //    }

        //    #endregion Mandatory fields
        //    if (bMandatoryFieldsOK)
        //    {
        //        string responseContent = await HttpResponseMessage_GET("GetImportHistoryV3", $"siteId={siteID.ToString()}");
        //        if (responseContent.Length > 0)
        //        {
        //            // Parse JSON response to get values
        //            cResponseStatusMessage_ImportHistory? response = Newtonsoft.Json.JsonConvert.DeserializeObject<cResponseStatusMessage_ImportHistory>(responseContent);

        //            if ((response != null) && (response.ResponseMessages != null))
        //            {
        //                responses = response.ResponseMessages;
        //            }
        //        }
        //    }

        //    return responses;
        //}

        ///// <summary>
        ///// Update transferable status & CardLayout of multiple existing VCards in the given site.
        ///// Note* CSN cannot be transferable / CardLayout cannot be assigned to the CSN/Visitor card is non transferable.
        ///// </summary>
        ///// <param name="siteID"></param>
        ///// <param name="virtualCardIds"></param>
        ///// <param name="transferable"></param>
        ///// <param name="cardlayoutName"></param>
        ///// <returns></returns>
        //public async Task<List<cResponseMessage_VirtualCard>?> EditMultipleVirtualCards(Int64 siteID, Int64[] virtualCardIds, string transferable, string? cardlayoutName)
        //{
        //    List<cResponseMessage_VirtualCard>? responseMessages = null;

        //    #region Mandatory fields

        //    bool bMandatoryFieldsOK = true;
        //    try
        //    {
        //        if (siteID < 1) throw new Exception($"siteID is a mandatory parameter");
        //        if (virtualCardIds.Length < 1) throw new Exception($"virtualCardIds is a mandatory parameter");
        //        if ((transferable.Length < 1) || ((transferable.Length > 0) && (transferable.ToLower() != "no") && (transferable.ToLower() != "yes"))) throw new Exception($"transferable parameter has to be 'Yes/yes/YES/No/NO/no'");
        //    }
        //    catch (Exception ex)
        //    {
        //        bMandatoryFieldsOK = false;
        //        Console.WriteLine($"EditMultipleVirtualCards: An error occurred: {ex.Message}");
        //    }

        //    #endregion Mandatory fields
        //    if (bMandatoryFieldsOK)
        //    {
        //        //string sParameters = "{" + $"\"ids\":[\"{string.Join("\",\"", virtualCardIds)}\"], \"siteId\":{siteID.ToString()}, \"accessToken\":{_AccessToken}, \"transferable\":{transferable.ToLower()}, \"cardlayoutName\":{(cardlayoutName == null ? "" : cardlayoutName)}" + "}";
        //        string sParameters = "{" + $"\"ids\":[\"{string.Join("\",\"", virtualCardIds)}\"], \"siteId\":{siteID.ToString()}, \"transferable\":\"{transferable.ToLower()}\", \"cardlayoutName\":\"{(cardlayoutName == null ? "" : cardlayoutName)}\"" + "}";

        //        string responseContent = await HttpResponseMessage_POST("EditMultipleVirtualCardsV3", new StringContent(sParameters, Encoding.UTF8, "application/json"));
        //        if (responseContent.Length > 0)
        //        {
        //            // Parse JSON response to get values
        //            cResponseStatusMessage_VirtualCard? statusMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<cResponseStatusMessage_VirtualCard>(responseContent);
        //            if ((statusMessage != null) && (statusMessage.StatusCode == 200) && (statusMessage.ResponseMessages != null))
        //            {
        //                responseMessages = statusMessage.ResponseMessages;
        //            }
        //        }
        //    }

        //    return responseMessages;
        //}

        ///// <summary>
        ///// Returns message having statuscode and response message containing imort result.
        ///// Import file formats allowed: .xlsx and .csv
        ///// </summary>
        ///// <param name="siteID"></param>
        ///// <param name="importFile"></param>
        ///// <returns></returns>
        //public async Task<List<cResponseMessage_Import>?> ImportVirtualCard(Int64 siteID, string importFile)
        //{
        //    List<cResponseMessage_Import>? responseMessages = null;

        //    #region Mandatory fields
        //    string sFileExt = "";
        //    bool bMandatoryFieldsOK = true;
        //    try
        //    {
        //        if (siteID < 1) throw new Exception($"siteID is a mandatory parameter");
        //        if (importFile.Length < 1) throw new Exception($"excelFile is a mandatory parameter");
        //        sFileExt = Path.GetExtension(importFile).ToLower();
        //        if ((sFileExt != ".xlsx") && (sFileExt != ".csv")) throw new Exception($"Only file with '.xlsx' or '.csv' extension is allowed");
        //        if (!File.Exists(importFile)) throw new Exception($"Excel file does not exist at the specified location");
        //    }
        //    catch (Exception ex)
        //    {
        //        bMandatoryFieldsOK = false;
        //        Console.WriteLine($"ImportVirtualCard: An error occurred: {ex.Message}");
        //    }

        //    #endregion Mandatory fields
        //    if (bMandatoryFieldsOK)
        //    {
        //        MultipartFormDataContent content = new MultipartFormDataContent();
        //        content.Add(new StringContent(siteID.ToString(), Encoding.UTF8, "application/json"), "siteId");

        //        byte[] fileByteArray = File.ReadAllBytes(importFile);
        //        HttpContent fileContent = new StreamContent(new MemoryStream(fileByteArray));
        //        //StreamContent fileContent = new StreamContent(File.OpenRead(importFile));

        //        switch (sFileExt)
        //        {
        //            case ".xlsx":
        //                {
        //                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        //                    //fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/vnd.ms-excel");
        //                }
        //                break;
        //            case ".csv":
        //                {
        //                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("text/csv");
        //                }
        //                break;
        //        }

        //        content.Add(fileContent, "Excel File", Path.GetFileName(importFile));

        //        string responseContent = await HttpResponseMessage_POST("ImportVirtualCardV3", content);
        //        if (responseContent.Length > 0)
        //        {
        //            // Parse JSON response to get values
        //            cResponseStatusMessage_Import? response = Newtonsoft.Json.JsonConvert.DeserializeObject<cResponseStatusMessage_Import>(responseContent);
        //            if ((response != null) && (response.StatusCode == 200) && (response.ResponseMessages != null))
        //            {
        //                responseMessages = response.ResponseMessages;
        //            }
        //        }
        //    }

        //    return responseMessages;
        //}

        //#endregion V3 endpoints
        //#region V2 enpoints

        ///// <summary>
        ///// Returns a list of photo object(s) associated with the site id.
        ///// A photo object has an PhotoId of integer type and PhotoName of string type.
        ///// </summary>
        ///// <param name="siteID"></param>
        ///// <returns></returns>
        //public async Task<List<cPhoto>?> GetPhotoList(double siteID)
        //{
        //    List<cPhoto>? photos = new List<cPhoto>();
        //    string responseContent = await HttpResponseMessage_GET("GetPhotoListV2", $"siteId={siteID.ToString()}");
        //    if (responseContent.Length > 0)
        //    {
        //        // Parse JSON response to get values
        //        photos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<cPhoto>>(responseContent);
        //    }

        //    return photos;
        //}

        ///// <summary>
        ///// Returns a list of cardlayout object(s) associated with the site id.
        ///// A cardlayout object has a CardLayoutId of integer type and CardLayoutName of string type.
        ///// </summary>
        ///// <param name="siteID"></param>
        ///// <returns></returns>
        //public async Task<List<cCardLayout>?> GetCardLayoutList(double siteID)
        //{
        //    List<cCardLayout>? cardLayouts = new List<cCardLayout>();
        //    string responseContent = await HttpResponseMessage_GET("GetCardLayoutListV2", $"siteId={siteID.ToString()}");
        //    if (responseContent.Length > 0)
        //    {
        //        // Parse JSON response to get values
        //        cardLayouts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<cCardLayout>>(responseContent);
        //    }

        //    return cardLayouts;
        //}

        ///// <summary>
        ///// Returns a VCard preview object associated with the VCard id and given access token.
        ///// A VCard preview object has an FrontVirtualCardPreview and BackVirtualCardPreview of string type.
        ///// Incase of CSN / Private ID default Card layout VCard preview object has only FrontVirtualCardPreview of string type and BackVirtualCardPreview has empty or null value
        ///// </summary>
        ///// <param name="vcardId"></param>
        ///// <param name="siteID"></param>
        ///// <returns></returns>
        //public async Task<cVirtualCardPreview?> GetVirtualCardPreview(Int64 vcardId, Int64 siteID)
        //{
        //    cVirtualCardPreview? cardPreview = null;
        //    string responseContent = await HttpResponseMessage_GET("GetVirtualCardPreviewV2", $"vcardId={vcardId.ToString()}&siteId={siteID.ToString()}");
        //    if (responseContent.Length > 0)
        //    {
        //        // Parse JSON response to get values
        //        cardPreview = Newtonsoft.Json.JsonConvert.DeserializeObject<cVirtualCardPreview>(responseContent);
        //    }

        //    return cardPreview;
        //}

        ///// <summary>
        ///// Returns a list of VCard object(s) associated with the site id and given access token.
        ///// A VCard object has a VirtualCardId, ConfigurationId, PhotoId and CardLayoutId of integer type
        /////                      first name, last name, configuration name, Status, Email, PhotoName, CardLayoutName,
        /////                         Transferable (Optional field, default value = no), CreationDate, ActivationDate, RevokeDate, StartDateTime, EndDateTime,
        /////                         VisitorTimeZone, IsVisitorCard, Fields (1-5), SecurityCode and private id of string type.
        ///// </summary>
        ///// <param name="siteID"></param>
        ///// <returns></returns>
        //public async Task<List<cVirtualCard>?> GetVirtualCardList(Int64 siteID)
        //{
        //    List<cVirtualCard>? cards = new List<cVirtualCard>();
        //    string responseContent = await HttpResponseMessage_GET("GetVirtualCardListV2", $"siteId={siteID.ToString()}");
        //    if (responseContent.Length > 0)
        //    {
        //        // Parse JSON response to get values
        //        cards = Newtonsoft.Json.JsonConvert.DeserializeObject<List<cVirtualCard>>(responseContent);
        //    }

        //    return cards;
        //}

        ///// <summary>
        ///// Returns VCard detail object associated with the VCard id and given access token.
        ///// A VCard detail object has an id of integer type, first name, last name, Status, StartDateTime, EndDateTime, VisitorTimeZone,
        /////                                     IsVisitorCard,CreationDate, ActivationDate, RevokeDate, Fields(1-5), SecurityCode, Email,
        /////                                     Phone ,private Id, CardLayoutName, Transferable (Default value = no) and PhotoName of string type and
        /////                                     configuration id, PhotoId and CardLayoutId of integer type.
        ///// </summary>
        ///// <param name="vcardId"></param>
        ///// <param name="siteID"></param>
        ///// <returns></returns>
        //public async Task<cVirtualCard?> GetVirtualCardDetail(Int64 vcardId, Int64 siteID)
        //{
        //    cVirtualCard? card = null;
        //    string responseContent = await HttpResponseMessage_GET("GetVirtualCardDetailV2", $"vcardId={vcardId.ToString()}&siteId={siteID.ToString()}");
        //    if (responseContent.Length > 0)
        //    {
        //        // Parse JSON response to get values
        //        card = Newtonsoft.Json.JsonConvert.DeserializeObject<cVirtualCard>(responseContent);
        //    }

        //    return card;
        //}

        ///// <summary>
        ///// Returns updated virtual card id if virtual card was successfully added, 0 if not successful.
        ///// </summary>
        ///// <param name="virtualCard"></param>
        ///// <returns></returns>
        //public async Task<Int64> AddVirtualCard(cVirtualCardAdd virtualCard)
        //{
        //    Int64 iVirtualCardId = 0;

        //    #region Mandatory fields

        //    bool bMandatoryFieldsOK = true;
        //    try
        //    {
        //        if (virtualCard == null)
        //        {
        //            throw new Exception($"virtualCard is a mandatory parameter");
        //        }
        //        else
        //        {
        //            if ((virtualCard.siteId == null) || (virtualCard.siteId < 1)) throw new Exception($"SiteID is a mandatory field");
        //            if ((virtualCard.FirstName == null) || (virtualCard.FirstName.Length < 1)) throw new Exception($"FirstName is a mandatory field");
        //            if ((virtualCard.LastName is null) || (virtualCard.LastName.Length < 1)) throw new Exception($"LastName is a mandatory field");
        //            if ((virtualCard.Email == null) || (virtualCard.Email.Length < 1)) throw new Exception($"Email is a mandatory field");
        //            if ((virtualCard.PhoneNumber == null) || (virtualCard.PhoneNumber.Length < 10) || (virtualCard.PhoneNumber.Length > 20)) throw new Exception($"PhoneNumber is a mandatory field with minimum 10 and maximum 20 characters");
        //            if ((virtualCard.ConfigurationId > 0) && ((virtualCard.PrivateorStidId != null) && (virtualCard.PrivateorStidId.Length < 1))) throw new Exception($"PrivateorStidId is a mandatory field if configurationId is specified");

        //            if ((virtualCard.Field1 != null) && (virtualCard.Field1.Length > 0) && (virtualCard.Field1.Any(ch => !char.IsLetterOrDigit(ch)))) throw new Exception($"Field1 field can't contain any special characters");
        //            if ((virtualCard.Field2 != null) && (virtualCard.Field2.Length > 0) && (virtualCard.Field2.Any(ch => !char.IsLetterOrDigit(ch)))) throw new Exception($"Field2 field can't contain any special characters");
        //            if ((virtualCard.Field3 != null) && (virtualCard.Field3.Length > 0) && (virtualCard.Field3.Any(ch => !char.IsLetterOrDigit(ch)))) throw new Exception($"Field3 field can't contain any special characters");
        //            if ((virtualCard.Field4 != null) && (virtualCard.Field4.Length > 0) && (virtualCard.Field4.Any(ch => !char.IsLetterOrDigit(ch)))) throw new Exception($"Field4 field can't contain any special characters");
        //            if ((virtualCard.Field5 != null) && (virtualCard.Field5.Length > 0) && (virtualCard.Field5.Any(ch => !char.IsLetterOrDigit(ch)))) throw new Exception($"Field5 field can't contain any special characters");

        //            if ((virtualCard.Field1 != null) && (virtualCard.Field1.Length > 30)) throw new Exception($"Field1 field has maximum 30 characters allowed");
        //            if ((virtualCard.Field2 != null) && (virtualCard.Field2.Length > 30)) throw new Exception($"Field2 field has maximum 30 characters allowed");
        //            if ((virtualCard.Field3 != null) && (virtualCard.Field3.Length > 30)) throw new Exception($"Field3 field has maximum 30 characters allowed");
        //            if ((virtualCard.Field4 != null) && (virtualCard.Field4.Length > 30)) throw new Exception($"Field4 field has maximum 30 characters allowed");
        //            if ((virtualCard.Field5 != null) && (virtualCard.Field5.Length > 30)) throw new Exception($"Field5 field has maximum 30 characters allowed");

        //            if ((virtualCard.Transferable != null) && (virtualCard.Transferable.Length > 0) && (virtualCard.Transferable.ToLower() != "no") && (virtualCard.Transferable.ToLower() != "yes")) throw new Exception($"Transferable field has to be 'Yes/yes/YES/No/NO/no'");
        //            if ((virtualCard.SecurityCode != null) && (virtualCard.SecurityCode.Length != 6)) throw new Exception($"SecurityCode field has to be 6 characters");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        bMandatoryFieldsOK = false;
        //        Console.WriteLine($"AddVirtualCard: An error occurred: {ex.Message}");
        //    }

        //    #endregion Mandatory fields
        //    if (bMandatoryFieldsOK)
        //    {
        //        string sParameters = Newtonsoft.Json.JsonConvert.SerializeObject(virtualCard);

        //        string responseContent = await HttpResponseMessage_POST("AddVirtualCardV2", new StringContent(sParameters, Encoding.UTF8, "application/json"));
        //        if (responseContent.Length > 0)
        //        {
        //            // Parse JSON response to get values
        //            dynamic? jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);
        //            if (jsonResponse != null)
        //            {
        //                iVirtualCardId = jsonResponse.VirtualCardId;
        //                //virtualCard.VirtualCardId = iVirtualCardId;
        //            }
        //        }
        //    }

        //    return iVirtualCardId;
        //}

        ///// <summary>
        ///// Returns true if virtual card was successfully updated, false if not successful.
        ///// </summary>
        ///// <param name="virtualCard"></param>
        ///// <returns></returns>
        //public async Task<bool> EditVirtualCard(cVirtualCardEdit virtualCard)
        //{
        //    bool bCardEdited = false;

        //    #region Mandatory fields

        //    bool bMandatoryFieldsOK = true;
        //    try
        //    {
        //        if (virtualCard == null)
        //        {
        //            throw new Exception($"virtualCard is a mandatory parameter");
        //        }
        //        else
        //        {
        //            if ((virtualCard.siteId == null) || (virtualCard.siteId < 1)) throw new Exception($"siteID is a mandatory field");
        //            if ((virtualCard.FirstName == null) || (virtualCard.FirstName.Length < 1)) throw new Exception($"FirstName is a mandatory field");
        //            if ((virtualCard.LastName is null) || (virtualCard.LastName.Length < 1)) throw new Exception($"LastName is a mandatory field");
        //            if ((virtualCard.Email == null) || (virtualCard.Email.Length < 1)) throw new Exception($"Email is a mandatory field");
        //            if (virtualCard.VirtualCardId < 1) throw new Exception($"VirtualCardId is a mandatory field");
        //            if ((virtualCard.PhoneNumber == null) || (virtualCard.PhoneNumber.Length < 10) || (virtualCard.PhoneNumber.Length > 20)) throw new Exception($"PhoneNumber is a mandatory field with maximum 20 and minimum 10 characters");
        //            if ((virtualCard.ConfigurationId > 0) && ((virtualCard.PrivateorStidId != null) && (virtualCard.PrivateorStidId.Length < 1))) throw new Exception($"PrivateorStidId is a mandatory field if configurationId is specified");

        //            if ((virtualCard.Field1 != null) && (virtualCard.Field1.Length > 0) && (virtualCard.Field1.Any(ch => !char.IsLetterOrDigit(ch)))) throw new Exception($"Field1 field can't contain any special characters");
        //            if ((virtualCard.Field2 != null) && (virtualCard.Field2.Length > 0) && (virtualCard.Field2.Any(ch => !char.IsLetterOrDigit(ch)))) throw new Exception($"Field2 field can't contain any special characters");
        //            if ((virtualCard.Field3 != null) && (virtualCard.Field3.Length > 0) && (virtualCard.Field3.Any(ch => !char.IsLetterOrDigit(ch)))) throw new Exception($"Field3 field can't contain any special characters");
        //            if ((virtualCard.Field4 != null) && (virtualCard.Field4.Length > 0) && (virtualCard.Field4.Any(ch => !char.IsLetterOrDigit(ch)))) throw new Exception($"Field4 field can't contain any special characters");
        //            if ((virtualCard.Field5 != null) && (virtualCard.Field5.Length > 0) && (virtualCard.Field5.Any(ch => !char.IsLetterOrDigit(ch)))) throw new Exception($"Field5 field can't contain any special characters");

        //            if ((virtualCard.Field1 != null) && (virtualCard.Field1.Length > 30)) throw new Exception($"Field1 field has maximum 30 characters allowed");
        //            if ((virtualCard.Field2 != null) && (virtualCard.Field2.Length > 30)) throw new Exception($"Field2 field has maximum 30 characters allowed");
        //            if ((virtualCard.Field3 != null) && (virtualCard.Field3.Length > 30)) throw new Exception($"Field3 field has maximum 30 characters allowed");
        //            if ((virtualCard.Field4 != null) && (virtualCard.Field4.Length > 30)) throw new Exception($"Field4 field has maximum 30 characters allowed");
        //            if ((virtualCard.Field5 != null) && (virtualCard.Field5.Length > 30)) throw new Exception($"Field5 field has maximum 30 characters allowed");

        //            if ((virtualCard.Transferable != null) && (virtualCard.Transferable.Length > 0) && (virtualCard.Transferable.ToLower() != "no") && (virtualCard.Transferable.ToLower() != "yes")) throw new Exception($"Transferable field has to be 'Yes/yes/YES/No/NO/no'");
        //            if ((virtualCard.SecurityCode != null) && (virtualCard.SecurityCode.Length != 6)) throw new Exception($"SecurityCode field has to be 6 characters");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        bMandatoryFieldsOK = false;
        //        Console.WriteLine($"EditVirtualCard: An error occurred: {ex.Message}");
        //    }

        //    #endregion Mandatory fields
        //    if (bMandatoryFieldsOK)
        //    {
        //        string sParameters = Newtonsoft.Json.JsonConvert.SerializeObject(virtualCard);

        //        string responseContent = await HttpResponseMessage_PUT("EditVirtualCardV2", new StringContent(sParameters, Encoding.UTF8, "application/json"));
        //        if (responseContent.Length > 0)
        //        {
        //            // Parse JSON response to get values
        //            dynamic? jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);
        //            bCardEdited = (jsonResponse != null);
        //            //bCardEdited = ((jsonResponse != null) && (jsonResponse.Message == "VCard updated successfully"));
        //        }
        //    }

        //    return bCardEdited;
        //}

        ///// <summary>
        ///// Returns count of photos imported successfully.
        ///// To get results for each photo individually, use UploadPhotos
        ///// </summary>
        ///// <param name="siteID"></param>
        ///// <param name="imageFiles"></param>
        ///// <returns></returns>
        //public async Task<int> UploadPhotosReturnCount(Int64 siteID, List<cPhotoImage> imageFiles)
        //{
        //    List<cResponseMessage_Photo>? responses = await UploadPhotos(siteID, imageFiles);
        //    return responses.Where(rm => rm.StatusCode == 85).ToList().Count();
        //}

        ///// <summary>
        ///// Returns message containing result for each photo imported.
        ///// Having PhotoName,StatusCode and response message.
        ///// </summary>
        ///// <param name="siteID"></param>
        ///// <param name="imageFiles"></param>
        ///// <returns></returns>
        //public async Task<List<cResponseMessage_Photo>?> UploadPhotos(Int64 siteID, List<cPhotoImage> imageFiles)
        //{
        //    List<cResponseMessage_Photo>? responses = null;

        //    #region Mandatory fields

        //    bool bMandatoryFieldsOK = true;
        //    try
        //    {
        //        if (siteID < 1) throw new Exception($"siteID is a mandatory parameter");
        //        if ((imageFiles == null) || ((imageFiles != null) && (imageFiles.Count < 1))) throw new Exception($"imageFiles is a mandatory parameter");
        //    }
        //    catch (Exception ex)
        //    {
        //        bMandatoryFieldsOK = false;
        //        Console.WriteLine($"UploadPhotos: An error occurred: {ex.Message}");
        //    }

        //    #endregion Mandatory fields
        //    if (bMandatoryFieldsOK)
        //    {
        //        MultipartFormDataContent content = new MultipartFormDataContent();
        //        content.Add(new StringContent(siteID.ToString(), Encoding.UTF8, "application/json"), "siteId");

        //        bool bAllPhotosValid = true;
        //        foreach (cPhotoImage photoImage in imageFiles)
        //        {
        //            bool bValidPhoto = (photoImage.PhotoImage != null);
        //            if (bValidPhoto)
        //            {
        //                using (MemoryStream ms = new MemoryStream())
        //                {
        //                    photoImage.PhotoImage.Save(ms, photoImage.PhotoImage.RawFormat);
        //                    //photoImage.PhotoImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
        //                    bValidPhoto = (ms.Length <= (_MaxImageKB * 1024));
        //                    if (bValidPhoto)
        //                    {
        //                        byte[] imageBytes = ms.ToArray();

        //                        //bValidPhoto = IsValidSTidImage(imageBytes);
        //                        if (bValidPhoto)
        //                        {
        //                            HttpContent imageContent = new StreamContent(new MemoryStream(imageBytes));

        //                            eImageFormat imageFormat = GetImageFormat(imageBytes);
        //                            switch (imageFormat)
        //                            {
        //                                case eImageFormat.jpeg:
        //                                case eImageFormat.jpg: imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg"); break;
        //                                case eImageFormat.bmp: imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/bmp"); break;
        //                                case eImageFormat.gif: imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/gif"); break;
        //                                case eImageFormat.png: imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/png"); break;
        //                                case eImageFormat.svg_small:
        //                                case eImageFormat.svg_capital: imageFormat = eImageFormat.UNKNOWN; break; //Not supported currently
        //                                case eImageFormat.intel_tiff:
        //                                case eImageFormat.motorola_tiff: imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/tiff"); break;
        //                                case eImageFormat.svg_xml_small:
        //                                case eImageFormat.svg_xml_capital: imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/svg+xml"); break;
        //                                case eImageFormat.webp: imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/webp"); break;
        //                            }

        //                            if (imageFormat != eImageFormat.UNKNOWN)
        //                            {
        //                                content.Add(imageContent, "Photos", photoImage.PhotoName);
        //                            }
        //                        }
        //                    }
        //                }
        //            }

        //            if (bAllPhotosValid)
        //            {
        //                bAllPhotosValid = bValidPhoto;
        //            }
        //        }
        //        string responseContent = await HttpResponseMessage_POST("UploadPhotosV2", content);
        //        if (responseContent.Length > 0)
        //        {
        //            // Parse JSON response to get values
        //            cResponseStatusMessage_Photo? response = Newtonsoft.Json.JsonConvert.DeserializeObject<cResponseStatusMessage_Photo>(responseContent);

        //            if ((response != null) && (response.StatusCode == 200) && (response.ResponseMessages != null))
        //            {
        //                responses = response.ResponseMessages;
        //            }
        //        }
        //    }

        //    return responses;
        //}

        ///// <summary>
        ///// Returns a byte array of excel file of entire user VCard(s) that exist in a given Site.
        ///// Following columns will be exported in excel file:
        /////   First Name
        /////   Last Name
        /////   Email
        /////   Phone Number
        /////   Blue Mobile ID Configuration
        /////   Private ID/CSN
        /////   Card Layout
        /////   Photo
        /////   Transferable
        /////   Start Date Time(If exists otherwise show the empty cell)
        /////   End Date Time(If exists otherwise show the empty cell)
        /////   Time Zone(If exists otherwise show the empty cell)
        /////   Security Code(If exists otherwise show the empty cell)
        ///// </summary>
        ///// <param name="siteID"></param>
        ///// <returns></returns>
        //public async Task<byte[]?> ExportVirtualCard(Int64 siteID)
        //{
        //    byte[]? fileBytes = null;

        //    #region Mandatory fields

        //    bool bMandatoryFieldsOK = true;
        //    try
        //    {
        //        if (siteID < 1) throw new Exception($"siteID is a mandatory parameter");
        //    }
        //    catch (Exception ex)
        //    {
        //        bMandatoryFieldsOK = false;
        //        Console.WriteLine($"ExportVirtualCard: An error occurred: {ex.Message}");
        //    }

        //    #endregion Mandatory fields
        //    if (bMandatoryFieldsOK)
        //    {
        //        fileBytes = await HttpResponseMessage_GET_ByteArray("ExportVirtualCardV2", $"siteId={siteID.ToString()}");
        //    }

        //    return fileBytes;
        //}

        ///// <summary>
        ///// Returns a byte array of excel file of entire user VCard(s) (and status) that exist in a given Site.
        ///// Following columns will be exported in excel file:
        /////   First Name
        /////   Last Name
        /////   Email
        /////   Blue Mobile ID Configuration
        /////   Private ID/CSN
        /////   VCard Creation Date
        /////   VCard Activation Date (If exists otherwise show the empty cell)
        /////   VCard Revoked Date (If exists otherwise show the empty cell)
        /////   Status
        /////   Card Layout
        /////   Photo
        /////   Fields 1-5 (with customized name) – if field are empty report will show the empty cell.
        /////   Transferable
        /////   Start Date Time(If exists otherwise show the empty cell)
        /////   End Date Time(If exists otherwise show the empty cell)
        /////   Time Zone(If exists otherwise show the empty cell)
        /////   Security Code(If exists otherwise show the empty cell)
        ///// </summary>
        ///// <param name="siteID"></param>
        ///// <returns></returns>
        //public async Task<byte[]?> ReportVirtualCard(Int64 siteID)
        //{
        //    byte[]? fileBytes = null;

        //    #region Mandatory fields

        //    bool bMandatoryFieldsOK = true;
        //    try
        //    {
        //        if (siteID < 1) throw new Exception($"siteID is a mandatory parameter");
        //    }
        //    catch (Exception ex)
        //    {
        //        bMandatoryFieldsOK = false;
        //        Console.WriteLine($"ReportVirtualCard: An error occurred: {ex.Message}");
        //    }

        //    #endregion Mandatory fields
        //    if (bMandatoryFieldsOK)
        //    {
        //        fileBytes = await HttpResponseMessage_GET_ByteArray("ReportVirtualCardV2", $"siteId={siteID.ToString()}");
        //    }

        //    return fileBytes;
        //}

        ///// <summary>
        ///// Returns count of reserved credits of the company of current user, which user can use to download virtual cards.
        ///// </summary>
        ///// <returns></returns>
        //public async Task<int> GetReservedCredits()
        //{
        //    int iCredits = 0;
        //    string responseContent = await HttpResponseMessage_GET("GetReservedCreditsV2");
        //    if (responseContent.Length > 0)
        //    {
        //        // Parse JSON response to get values
        //        dynamic? jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);
        //        if (jsonResponse != null) iCredits = jsonResponse.credits;
        //    }

        //    return iCredits;
        //}

        //#endregion V2 enpoints
        //#region V1 enpoints

        ///// <summary>
        ///// Returns a list of customer site object of the current user.
        ///// A customer site object has an id of integer type and site name of string type.
        ///// </summary>
        ///// <returns></returns>
        //public async Task<List<cSite>?> GetSiteList()
        //{
        //    List<cSite>? sites = new List<cSite>();
        //    string responseContent = await HttpResponseMessage_GET("GetSiteListV1");
        //    if (responseContent.Length > 0)
        //    {
        //        // Parse JSON response to get values
        //        sites = Newtonsoft.Json.JsonConvert.DeserializeObject<List<cSite>>(responseContent);
        //    }

        //    return sites;
        //}

        ///// <summary>
        ///// Returns a list of reader configuration object(s) associated with the site id.
        ///// A reader configuration object has an id of integer type and configuration name of string type.
        ///// </summary>
        ///// <param name="siteID"></param>
        ///// <returns></returns>
        //public async Task<List<cReaderConfig>?> GetReaderConfigurationList(Int64 siteID)
        //{
        //    List<cReaderConfig>? readerConfigs = new List<cReaderConfig>();
        //    string responseContent = await HttpResponseMessage_GET("GetReaderConfigurationListV1", $"siteId={siteID.ToString()}");
        //    if (responseContent.Length > 0)
        //    {
        //        // Parse JSON response to get values
        //        readerConfigs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<cReaderConfig>>(responseContent);
        //    }

        //    return readerConfigs;
        //}

        ///// <summary>
        ///// Returns list of message object(s) with results of all virtual card ids that was passed to be deleted.
        ///// </summary>
        ///// <param name="siteID"></param>
        ///// <param name="virtualCardIds"></param>
        ///// <returns></returns>
        //public async Task<List<cResponseMessage_VirtualCard>?> DeleteVirtualCard(Int64 siteID, Int64[] virtualCardIds)
        //{
        //    List<cResponseMessage_VirtualCard>? responseMessages = null;

        //    #region Mandatory fields

        //    bool bMandatoryFieldsOK = true;
        //    try
        //    {
        //        if (siteID < 1) throw new Exception($"siteID is a mandatory parameter");
        //        if (virtualCardIds.Length < 1) throw new Exception($"virtualCardIds is a mandatory parameter");
        //    }
        //    catch (Exception ex)
        //    {
        //        bMandatoryFieldsOK = false;
        //        Console.WriteLine($"DeleteVirtualCard: An error occurred: {ex.Message}");
        //    }

        //    #endregion Mandatory fields
        //    if (bMandatoryFieldsOK)
        //    {
        //        string sParameters = "{" + $"\"ids\":[\"{string.Join("\",\"", virtualCardIds)}\"], \"siteId\":{siteID.ToString()}" + "}";

        //        string responseContent = await HttpResponseMessage_POST("DeleteVirtualCardV1", new StringContent(sParameters, Encoding.UTF8, "application/json"));
        //        if (responseContent.Length > 0)
        //        {
        //            // Parse JSON response to get values
        //            cResponseStatusMessage_VirtualCard? responseMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<cResponseStatusMessage_VirtualCard>(responseContent);
        //            if ((responseMessage != null) && (responseMessage.StatusCode == 200) && (responseMessage.ResponseMessages != null))
        //            {
        //                responseMessages = responseMessage.ResponseMessages;
        //            }
        //        }
        //    }

        //    return responseMessages;
        //}

        ///// <summary>
        ///// Returns list of message object(s) with results of all virtual card ids that was passed to be sent to end users.
        ///// </summary>
        ///// <param name="siteID"></param>
        ///// <param name="virtualCardIds"></param>
        ///// <returns></returns>
        //public async Task<List<cResponseMessage_VirtualCard>?> SendVirtualCard(Int64 siteID, Int64[] virtualCardIds)
        //{
        //    List<cResponseMessage_VirtualCard>? responseMessages = null;

        //    #region Mandatory fields

        //    bool bMandatoryFieldsOK = true;
        //    try
        //    {
        //        if (siteID < 1) throw new Exception($"siteID is a mandatory parameter");
        //        if (virtualCardIds.Length < 1) throw new Exception($"virtualCardIds is a mandatory parameter");
        //    }
        //    catch (Exception ex)
        //    {
        //        bMandatoryFieldsOK = false;
        //        Console.WriteLine($"SendVirtualCard: An error occurred: {ex.Message}");
        //    }

        //    #endregion Mandatory fields
        //    if (bMandatoryFieldsOK)
        //    {
        //        string sParameters = "{" + $"\"ids\":[\"{string.Join("\",\"", virtualCardIds)}\"], \"siteId\":{siteID.ToString()}" + "}";

        //        string responseContent = await HttpResponseMessage_POST("SendVirtualCardV1", new StringContent(sParameters, Encoding.UTF8, "application/json"));
        //        if (responseContent.Length > 0)
        //        {
        //            // Parse JSON response to get values
        //            cResponseStatusMessage_VirtualCard? responseMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<cResponseStatusMessage_VirtualCard>(responseContent);
        //            if ((responseMessage != null) && (responseMessage.StatusCode == 200) && (responseMessage.ResponseMessages != null))
        //            {
        //                responseMessages = responseMessage.ResponseMessages;
        //            }
        //        }
        //    }

        //    return responseMessages;
        //}

        ///// <summary>
        ///// Returns list of message object(s) with results of all virtual card ids that was passed to be revoked.
        ///// </summary>
        ///// <param name="siteID"></param>
        ///// <param name="virtualCardIds"></param>
        ///// <returns></returns>
        //public async Task<List<cResponseMessage_VirtualCard>?> RevokeVirtualCard(Int64 siteID, Int64[] virtualCardIds)
        //{
        //    List<cResponseMessage_VirtualCard>? responses = null;

        //    #region Mandatory fields

        //    bool bMandatoryFieldsOK = true;
        //    try
        //    {
        //        if (siteID < 1) throw new Exception($"siteID is a mandatory parameter");
        //        if (virtualCardIds.Length < 1) throw new Exception($"virtualCardIds is a mandatory parameter");
        //    }
        //    catch (Exception ex)
        //    {
        //        bMandatoryFieldsOK = false;
        //        Console.WriteLine($"RevokeVirtualCard: An error occurred: {ex.Message}");
        //    }

        //    #endregion Mandatory fields
        //    if (bMandatoryFieldsOK)
        //    {
        //        string sParameters = "{" + $"\"ids\":[\"{string.Join("\",\"", virtualCardIds)}\"], \"siteId\":{siteID.ToString()}" + "}";

        //        string responseContent = await HttpResponseMessage_POST("RevokeVirtualCardV1", new StringContent(sParameters, Encoding.UTF8, "application/json"));
        //        if (responseContent.Length > 0)
        //        {
        //            // Parse JSON response to get values
        //            cResponseStatusMessage_VirtualCard? responseMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<cResponseStatusMessage_VirtualCard>(responseContent);
        //            if ((responseMessage != null) && (responseMessage.StatusCode == 200) && (responseMessage.ResponseMessages != null))
        //            {
        //                responses = responseMessage.ResponseMessages;
        //            }
        //        }
        //    }

        //    return responses;
        //}

        ///// <summary>
        ///// Returns version object containing:
        /////   API Version of type string
        /////   Online Portal Version of type string
        ///// </summary>
        ///// <param name="siteID"></param>
        ///// <param name="virtualCardIds"></param>
        ///// <returns></returns>
        //public async Task<cVersion?> GetVersion()
        //{
        //    cVersion? version = null;
        //    string responseContent = await HttpResponseMessage_GET("GetVersionV1");
        //    if (responseContent.Length > 0)
        //    {
        //        // Parse JSON response to get values
        //        version = Newtonsoft.Json.JsonConvert.DeserializeObject<cVersion>(responseContent);
        //    }

        //    return version;
        //}

        ///// <summary>
        ///// Returns string containing language. As used by API version 4, can only contain 'fr' or 'en'
        //public async Task<string> GetLanguage()
        //{
        //    string sLanguage = "";
        //    string responseContent = await HttpResponseMessage_GET("GetLanguageV1");
        //    if (responseContent.Length > 0)
        //    {
        //        // Parse JSON response to get values
        //        try
        //        {
        //            dynamic? jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);
        //            if (jsonResponse != null) sLanguage = jsonResponse.Message;
        //        }
        //        catch
        //        {

        //        }
        //    }

        //    return sLanguage;
        //}

        ///// <summary>
        ///// Returns true if language was changed successfully, false otherwise.
        ///// As used by API version 4, can only be 'fr' or 'en'
        //public async Task<bool> SetLanguage(string language)
        //{
        //    //Should only be 'fr' or 'en'
        //    bool bLanguageSet = false;

        //    #region Mandatory fields

        //    bool bMandatoryFieldsOK = true;
        //    try
        //    {
        //        language = language.ToLower();
        //        if ((language != "fr") && (language != "en")) throw new Exception($"language can only be 'fr' or 'en'");
        //    }
        //    catch (Exception ex)
        //    {
        //        bMandatoryFieldsOK = false;
        //        Console.WriteLine($"SetLanguage: An error occurred: {ex.Message}");
        //    }

        //    #endregion Mandatory fields
        //    if (bMandatoryFieldsOK)
        //    {
        //        string sParameters = "{" + $"\"language\":\"{language.ToString()}\"" + "}";
        //        string responseContent = await HttpResponseMessage_PUT("SetLanguageV1", new StringContent(sParameters, Encoding.UTF8, "application/json")); //text/plain
        //        if (responseContent.Length > 0)
        //        {
        //            // Parse JSON response to get values
        //            dynamic? jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);
        //            bLanguageSet = (jsonResponse != null);
        //        }
        //    }

        //    return bLanguageSet;
        //}

        ///// <summary>
        ///// Returns object containing fields parameters object associated with site id.
        ///// </summary>
        ///// <param name="siteID"></param>
        ///// <returns></returns>
        //public async Task<cVCardFields?> GetFieldNames(double siteID)
        //{
        //    cVCardFields? fields = null;
        //    string responseContent = await HttpResponseMessage_GET("GetFieldNamesV1", $"siteId={siteID.ToString()}");
        //    if (responseContent.Length > 0)
        //    {
        //        // Parse JSON response to get values
        //        fields = Newtonsoft.Json.JsonConvert.DeserializeObject<cVCardFields>(responseContent);
        //    }

        //    return fields;
        //}

        ///// <summary>
        ///// Returns the number of remaining credits of the company associated with the current user.
        ///// </summary>
        ///// <returns></returns>
        //public async Task<int> GetAvailableCredits()
        //{
        //    int iCredits = 0;
        //    string responseContent = await HttpResponseMessage_GET("GetAvailableCreditsV1");
        //    if (responseContent.Length > 0)
        //    {
        //        // Parse JSON response to get values
        //        dynamic? jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);
        //        if (jsonResponse != null) iCredits = jsonResponse.credits;
        //    }

        //    return iCredits;
        //}

        //#endregion V1 enpoints

        //private async Task<string> HttpResponseMessage_GET(string sAPIEndpoint)
        //{
        //    return await HttpResponseMessage_GET(sAPIEndpoint, null);
        //}
        //private async Task<string> HttpResponseMessage_GET(string sAPIEndpoint, string? sParameters)
        //{
        //    string responseContent = "";

        //    try
        //    {
        //        bool bAccessTokenActive = await AccessTokenActive();
        //        if (bAccessTokenActive)
        //        {
        //            // Add authorization header
        //            _Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", (_AccessToken != null ? _AccessToken.AccessToken : ""));

        //            if (sAPIEndpoint != "")
        //            {
        //                // Perform GET request to obtain token
        //                string requestUri = $"{_ServerName}/Api/{sAPIEndpoint}/";
        //                if ((sParameters != null) && (sParameters.Length > 0)) requestUri = $"{requestUri}?{sParameters}";
        //                HttpResponseMessage response = await _Client.GetAsync(requestUri);

        //                responseContent = await response.Content.ReadAsStringAsync();

        //                // Check if request was successful
        //                if (response.IsSuccessStatusCode)
        //                {
        //                    // Read response content
        //                    return responseContent;

        //                    //// Parse JSON response to get access token
        //                    //dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);
        //                    //return jsonResponse.credits;
        //                }
        //                else
        //                {
        //                    // Request failed, throw exception or handle appropriately
        //                    throw new Exception($"Failed to retrieve response: {response.StatusCode} - {responseContent}");
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exception
        //        Console.WriteLine($"An error occurred: {ex.Message}");
        //        return "";
        //    }

        //    return responseContent;
        //}

        //private async Task<byte[]?> HttpResponseMessage_GET_ByteArray(string sAPIEndpoint)
        //{
        //    return await HttpResponseMessage_GET_ByteArray(sAPIEndpoint, null);
        //}
        //private async Task<byte[]?> HttpResponseMessage_GET_ByteArray(string sAPIEndpoint, string? sParameters)
        //{
        //    byte[]? responseContent = null;

        //    try
        //    {
        //        bool bAccessTokenActive = await AccessTokenActive();
        //        if (bAccessTokenActive)
        //        {
        //            // Add authorization header
        //            _Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", (_AccessToken != null ? _AccessToken.AccessToken : ""));

        //            if (sAPIEndpoint != "")
        //            {
        //                // Perform GET request to obtain token
        //                string requestUri = $"{_ServerName}/Api/{sAPIEndpoint}/";
        //                if ((sParameters != null) && (sParameters.Length > 0)) requestUri = $"{requestUri}?{sParameters}";
        //                HttpResponseMessage response = await _Client.GetAsync(requestUri);

        //                responseContent = await response.Content.ReadAsByteArrayAsync();

        //                // Check if request was successful
        //                if (response.IsSuccessStatusCode)
        //                {
        //                    // Read response content
        //                    return responseContent;

        //                    //// Parse JSON response to get access token
        //                    //dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);
        //                    //return jsonResponse.credits;
        //                }
        //                else
        //                {
        //                    // Request failed, throw exception or handle appropriately
        //                    throw new Exception($"Failed to retrieve response: {response.StatusCode} - {responseContent}");
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exception
        //        Console.WriteLine($"An error occurred: {ex.Message}");
        //        return null;
        //    }

        //    return responseContent;
        //}
        //private async Task<string> HttpResponseMessage_POST(string sAPIEndpoint, HttpContent content)
        //{
        //    string responseContent = "";

        //    try
        //    {
        //        bool bAccessTokenActive = await AccessTokenActive();
        //        if (bAccessTokenActive)
        //        {
        //            // Add authorization header
        //            _Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", (_AccessToken != null ? _AccessToken.AccessToken : ""));

        //            if (sAPIEndpoint != "")
        //            {
        //                // Perform GET request to obtain token
        //                string requestUri = $"{_ServerName}/Api/{sAPIEndpoint}/";
        //                HttpResponseMessage response = await _Client.PostAsync(requestUri, content);

        //                responseContent = await response.Content.ReadAsStringAsync();

        //                // Check if request was successful
        //                if (response.IsSuccessStatusCode)
        //                {
        //                    // Read response content
        //                    return responseContent;

        //                    //// Parse JSON response to get access token
        //                    //dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);
        //                    //return jsonResponse.credits;
        //                }
        //                else
        //                {
        //                    // Request failed, throw exception or handle appropriately
        //                    throw new Exception($"Failed to retrieve response: {response.StatusCode} - {responseContent}");
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exception
        //        Console.WriteLine($"An error occurred: {ex.Message}");
        //        return "";
        //    }

        //    return responseContent;
        //}

        //private async Task<string> HttpResponseMessage_PUT(string sAPIEndpoint, HttpContent content)
        //{
        //    string responseContent = "";

        //    try
        //    {
        //        bool bAccessTokenActive = await AccessTokenActive();
        //        if (bAccessTokenActive)
        //        {
        //            // Add authorization header
        //            _Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", (_AccessToken != null ? _AccessToken.AccessToken : ""));

        //            if (sAPIEndpoint != "")
        //            {
        //                // Perform GET request to obtain token
        //                string requestUri = $"{_ServerName}/Api/{sAPIEndpoint}/";
        //                HttpResponseMessage response = await _Client.PutAsync(requestUri, content);

        //                responseContent = await response.Content.ReadAsStringAsync();

        //                // Check if request was successful
        //                if (response.IsSuccessStatusCode)
        //                {
        //                    // Read response content
        //                    return responseContent;

        //                    //// Parse JSON response to get access token
        //                    //dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);
        //                    //return jsonResponse.credits;
        //                }
        //                else
        //                {
        //                    // Request failed, throw exception or handle appropriately
        //                    throw new Exception($"Failed to retrieve response: {response.StatusCode} - {responseContent}");
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exception
        //        Console.WriteLine($"An error occurred: {ex.Message}");
        //        return "";
        //    }

        //    return responseContent;
        //}

        //private async Task<string> HttpResponseMessage_DELETE(string sAPIEndpoint, string? sParameters)
        //{
        //    string responseContent = "";

        //    try
        //    {
        //        bool bAccessTokenActive = await AccessTokenActive();
        //        if (bAccessTokenActive)
        //        {
        //            // Add authorization header
        //            _Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", (_AccessToken != null ? _AccessToken.AccessToken : ""));

        //            if (sAPIEndpoint != "")
        //            {
        //                // Perform GET request to obtain token
        //                string requestUri = $"{_ServerName}/Api/{sAPIEndpoint}/";
        //                if ((sParameters != null) && (sParameters.Length > 0)) requestUri = $"{requestUri}?{sParameters}";
        //                HttpResponseMessage response = await _Client.DeleteAsync(requestUri);

        //                responseContent = await response.Content.ReadAsStringAsync();

        //                // Check if request was successful
        //                if (response.IsSuccessStatusCode)
        //                {
        //                    // Read response content
        //                    return responseContent;

        //                    //// Parse JSON response to get access token
        //                    //dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);
        //                    //return jsonResponse.credits;
        //                }
        //                else
        //                {
        //                    // Request failed, throw exception or handle appropriately
        //                    throw new Exception($"Failed to retrieve response: {response.StatusCode} - {responseContent}");
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exception
        //        Console.WriteLine($"An error occurred: {ex.Message}");
        //        return "";
        //    }

        //    return responseContent;
        //}
    }
}
