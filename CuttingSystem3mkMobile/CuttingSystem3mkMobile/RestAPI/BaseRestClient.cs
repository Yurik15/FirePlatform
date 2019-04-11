using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CuttingSystem3mkMobile.RestAPI
{
    public abstract class BaseRestClient
    {

        #region Members
        private string _baseUrl = "http://yurik15-001-site1.atempurl.com/api";
        #endregion

        #region Constructors
        protected BaseRestClient()
        {
        }
        #endregion Constructors

        #region Public Methods

        #endregion Public Methods

        #region Protected Methods

        #region Get Methods
        protected async Task<ServiceStatusMessage<TEntity>> MakeGetRequestReturnObject<TEntity>(string endpointRelativePath, bool authorise) where TEntity : class
        {
            var responseMessage = new ServiceStatusMessage<TEntity>();
            try
            {
                var url = await GetPathForServiceCall(endpointRelativePath);
                using (var client = new HttpClient())
                {
                    await SetupHttpClient(client, url, authorise);
                    var response = await client.GetAsync(url);
                    responseMessage.StatusCode = response.StatusCode.ToString();
                    responseMessage.DidSucceed = response.IsSuccessStatusCode;
                    responseMessage.ResponseMessage = response.ReasonPhrase;

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        responseMessage.Entity = JsonConvert.DeserializeObject<TEntity>(content);
                    }
                }
            }
            catch (Exception ex)
            {

                responseMessage.RaisedException = ex;
                responseMessage.DidSucceed = false;
            }
            return responseMessage;
        }

        protected async Task<ServiceStatusMessage<bool>> MakeGetRequestReturnBool(string endpointRelativePath, bool authorise)
        {
            var responseMessage = new ServiceStatusMessage<bool>();
            try
            {
                var url = await GetPathForServiceCall(endpointRelativePath);
                using (var client = new HttpClient())
                {
                    await SetupHttpClient(client, url, authorise);
                    var response = await client.GetAsync(url);

                    responseMessage.StatusCode = response.StatusCode.ToString();
                    responseMessage.DidSucceed = response.IsSuccessStatusCode;
                    responseMessage.ResponseMessage = response.ReasonPhrase;

                    if (response.IsSuccessStatusCode)
                    {
                        responseMessage.Entity = bool.Parse(await response.Content.ReadAsStringAsync());
                    }
                }
            }
            catch (Exception ex)
            {
                responseMessage.RaisedException = ex;
                responseMessage.DidSucceed = false;
            }
            return responseMessage;
        }

        protected async Task<ServiceStatusMessage<string>> MakeGetRequestReturnString(string endpointRelativePath, bool authorise)
        {
            var responseMessage = new ServiceStatusMessage<string>();
            try
            {
                var url = await GetPathForServiceCall(endpointRelativePath);
                using (var client = new HttpClient())
                {
                    await SetupHttpClient(client, url, authorise);
                    var response = await client.GetAsync(url);

                    responseMessage.StatusCode = response.StatusCode.ToString();
                    responseMessage.DidSucceed = response.IsSuccessStatusCode;
                    responseMessage.ResponseMessage = response.ReasonPhrase;

                    if (response.IsSuccessStatusCode)
                    {
                        responseMessage.Entity = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                responseMessage.RaisedException = ex;
                responseMessage.DidSucceed = false;
            }
            return responseMessage;
        }


        protected async Task<ServiceStatusMessage<byte[]>> MakeGetRequestReturnByteArray(string endpointRelativePath, bool authorise)
        {
            var responseMessage = new ServiceStatusMessage<byte[]>();
            try
            {
                var url = await GetPathForServiceCall(endpointRelativePath);
                using (var client = new HttpClient())
                {
                    await SetupHttpClient(client, url, authorise);
                    var response = await client.GetAsync(url);

                    responseMessage.StatusCode = response.StatusCode.ToString();
                    responseMessage.DidSucceed = response.IsSuccessStatusCode;
                    responseMessage.ResponseMessage = response.ReasonPhrase;

                    if (response.IsSuccessStatusCode)
                    {
                        var ms = await response.Content.ReadAsStreamAsync();
                        if (ms != null && ms.Length > 0)
                        {
                            responseMessage.Entity = new byte[ms.Length];
                            int read = await ms.ReadAsync(responseMessage.Entity, 0, (int)ms.Length);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                responseMessage.RaisedException = ex;
                responseMessage.DidSucceed = false;
            }
            return responseMessage;
        }
        #endregion Get Methods

        #region Post Methods
        protected async Task<ServiceStatusMessage> MakePostRequest(string endpointRelativePath, bool authorise, string jsonContent)
        {
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            return await PostRequestAsync(endpointRelativePath, authorise, httpContent);
        }

        protected async Task<ServiceStatusMessage<TEntity>> MakePostRequestReturnObject<TEntity>(string endpointRelativePath, bool authorise, string jsonContent) where TEntity : class
        {
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            return await PostRequestReturnObjectAsync<TEntity>(endpointRelativePath, authorise, httpContent);
        }

        protected async Task<ServiceStatusOperationSuccessMessage> MakePostRequestReturnBool(string endpointRelativePath, bool authorise, string jsonContent)
        {
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            return await PostRequestReturnBoolAsync(endpointRelativePath, authorise, httpContent);
        }

        protected async Task<ServiceStatusMessage> MakePostRequestReturnByteArray(string endpointRelativePath, bool authorise, string jsonContent)
        {
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            return await PostRequestReturnByteArrayAsync(endpointRelativePath, authorise, httpContent);
        }

        protected async Task<ServiceStatusMessage> MakePostRequestReturnStream(string endpointRelativePath, bool authorise, string jsonContent)
        {
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            return await PostRequestReturnStreamAsync(endpointRelativePath, authorise, httpContent);
        }
        #endregion Post Methods

        #region Put Methods
        protected async Task<ServiceStatusMessage> MakePutRequest(string endpointRelativePath, bool authorise, string jsonContent)
        {
            var responseMessage = new ServiceStatusMessage();
            try
            {
                var url = await GetPathForServiceCall(endpointRelativePath);
                using (var client = new HttpClient())
                {
                    await SetupHttpClient(client, url, authorise);
                    var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    var response = await client.PutAsync(url, httpContent);
                    responseMessage.StatusCode = response.StatusCode.ToString();
                    responseMessage.DidSucceed = response.IsSuccessStatusCode;
                    responseMessage.ResponseMessage = response.ReasonPhrase;
                }
            }
            catch (Exception ex)
            {
                responseMessage.RaisedException = ex;
                responseMessage.DidSucceed = false;
            }
            return responseMessage;
        }

        protected async Task<ServiceStatusMessage> MakePutRequestReturnObject<TEntity>(string endpointRelativePath, bool authorise, string jsonContent) where TEntity : class
        {
            var responseMessage = new ServiceStatusMessage<TEntity>();
            try
            {
                var url = await GetPathForServiceCall(endpointRelativePath);
                using (var client = new HttpClient())
                {
                    await SetupHttpClient(client, url, authorise);
                    var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    var response = await client.PutAsync(url, httpContent);
                    responseMessage.StatusCode = response.StatusCode.ToString();
                    responseMessage.DidSucceed = response.IsSuccessStatusCode;
                    responseMessage.ResponseMessage = response.ReasonPhrase;
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        if (!string.IsNullOrWhiteSpace(content))
                        {
                            responseMessage.Entity = JsonConvert.DeserializeObject<TEntity>(content);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                responseMessage.RaisedException = ex;
                responseMessage.DidSucceed = false;
            }
            return responseMessage;
        }

        protected async Task<ServiceStatusMessage> MakePutRequestReturnByteArray(string endpointRelativePath, bool authorise, string jsonContent)
        {
            var responseMessage = new ServiceStatusMessage<byte[]>();
            try
            {
                var url = await GetPathForServiceCall(endpointRelativePath);
                using (var client = new HttpClient())
                {
                    await SetupHttpClient(client, url, authorise);
                    var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(url, httpContent);
                    responseMessage.StatusCode = response.StatusCode.ToString();
                    responseMessage.DidSucceed = response.IsSuccessStatusCode;
                    responseMessage.ResponseMessage = response.ReasonPhrase;
                    if (response.IsSuccessStatusCode)
                    {
                        responseMessage.Entity = await response.Content.ReadAsByteArrayAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                responseMessage.RaisedException = ex;
                responseMessage.DidSucceed = false;
            }
            return responseMessage;
        }

        protected async Task<ServiceStatusMessage> MakePutRequestReturnStream(string endpointRelativePath, bool authorise, string jsonContent)
        {
            var responseMessage = new ServiceStatusMessage<Stream>();
            try
            {
                var url = await GetPathForServiceCall(endpointRelativePath);
                using (var client = new HttpClient())
                {
                    await SetupHttpClient(client, url, authorise);
                    var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(url, httpContent);
                    responseMessage.StatusCode = response.StatusCode.ToString();
                    responseMessage.DidSucceed = response.IsSuccessStatusCode;
                    responseMessage.ResponseMessage = response.ReasonPhrase;
                    if (response.IsSuccessStatusCode)
                    {
                        responseMessage.Entity = await response.Content.ReadAsStreamAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                responseMessage.RaisedException = ex;
                responseMessage.DidSucceed = false;
            }
            return responseMessage;
        }
        #endregion Put Methods

        #region Patch Methods
        protected async Task<ServiceStatusMessage> MakePatchRequest(string endpointRelativePath, bool authorise, string jsonContent)
        {
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json-patch+json");
            return await PostRequestAsync(endpointRelativePath, authorise, httpContent);
        }

        protected async Task<ServiceStatusMessage> MakePatchRequestReturnObject<TEntity>(string endpointRelativePath, bool authorise, string jsonContent) where TEntity : class
        {
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json-patch+json");
            return await PostRequestReturnObjectAsync<TEntity>(endpointRelativePath, authorise, httpContent);
        }

        protected async Task<ServiceStatusMessage> MakePatchRequestReturnByteArray(string endpointRelativePath, bool authorise, string jsonContent)
        {
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json-patch+json");
            return await PostRequestReturnByteArrayAsync(endpointRelativePath, authorise, httpContent);
        }

        protected async Task<ServiceStatusMessage> MakePatchRequestReturnStream(string endpointRelativePath, bool authorise, string jsonContent)
        {
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json-patch+json");
            return await PostRequestReturnStreamAsync(endpointRelativePath, authorise, httpContent);
        }
        #endregion Patch Methods

        #region Delete Methods

        #endregion Delete Methods

        #region General Methods
        protected async Task SetupHttpClient(HttpClient client, string url, bool authorise)
        {
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (authorise)
            {
                client.DefaultRequestHeaders.Authorization = await GetAuthenticationHeader();
            }
        }

        protected async Task<AuthenticationHeaderValue> GetAuthenticationHeader()
        {
            var token = ApplicationContext.ApplicationContext.AppToken;
            return new AuthenticationHeaderValue("Bearer", token);
        }

        protected virtual async Task<string> GetPathForServiceCall(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(_baseUrl))
            {
                _baseUrl = ApplicationContext.ApplicationContext.BaseUrl;

                if (_baseUrl.EndsWith("/", StringComparison.OrdinalIgnoreCase))
                {
                    _baseUrl = _baseUrl.TrimEnd('/');
                }
            }

            if (relativePath.StartsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                relativePath = relativePath.TrimStart('/');
            }

            var url = $"{_baseUrl}/{relativePath}";

            return url;
        }
        #endregion General Methods
        #endregion Protected Methods

        #region Private Methods

        private async Task<ServiceStatusMessage> PostRequestAsync(string endpointRelativePath, bool authorise, HttpContent bodyContent)
        {
            var responseMessage = new ServiceStatusMessage();
            try
            {
                var url = await GetPathForServiceCall(endpointRelativePath);
                using (var client = new HttpClient())
                {
                    await SetupHttpClient(client, url, authorise);
                    var response = await client.PostAsync(url, bodyContent);
                    responseMessage.StatusCode = response.StatusCode.ToString();
                    responseMessage.DidSucceed = response.IsSuccessStatusCode;
                    responseMessage.ResponseMessage = response.ReasonPhrase;
                }
            }
            catch (Exception ex)
            {
                responseMessage.RaisedException = ex;
                responseMessage.DidSucceed = false;
            }
            return responseMessage;
        }

        private async Task<ServiceStatusMessage<TEntity>> PostRequestReturnObjectAsync<TEntity>(string endpointRelativePath, bool authorise, HttpContent bodyContent) where TEntity : class
        {
            var responseMessage = new ServiceStatusMessage<TEntity>();
            try
            {
                var url = await GetPathForServiceCall(endpointRelativePath);

                using (var client = new HttpClient())
                {
                    await SetupHttpClient(client, url, authorise);
                   
                    var response = await client.PostAsync(url, bodyContent);

                    responseMessage.StatusCode = ((int)response.StatusCode).ToString();
                    responseMessage.DidSucceed = response.IsSuccessStatusCode;
                    responseMessage.ResponseMessage = response.ReasonPhrase;
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        if (!string.IsNullOrWhiteSpace(content))
                        {
                            responseMessage.Entity = JsonConvert.DeserializeObject<TEntity>(content);
                        }
                    }
                }
            }
            catch (Exception ex)
            {;
                responseMessage.RaisedException = ex;
                responseMessage.DidSucceed = false;
            }
            return responseMessage;
        }

        private async Task<ServiceStatusOperationSuccessMessage> PostRequestReturnBoolAsync(string endpointRelativePath, bool authorise, HttpContent bodyContent)
        {
            var responseMessage = new ServiceStatusOperationSuccessMessage();
            try
            {
                var url = await GetPathForServiceCall(endpointRelativePath);

                using (var client = new HttpClient())
                {
                    await SetupHttpClient(client, url, authorise);

                    var response = await client.PostAsync(url, bodyContent);
                   
                    responseMessage.StatusCode = ((int)response.StatusCode).ToString();
                    responseMessage.DidSucceed = response.IsSuccessStatusCode;
                    responseMessage.ResponseMessage = response.ReasonPhrase;
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        if (!string.IsNullOrWhiteSpace(content))
                        {
                            var singleReturnedBool = JsonConvert.DeserializeObject<string>(content);
                            var responsValue = false;
                            if (bool.TryParse(singleReturnedBool, out responsValue))
                            {
                                responseMessage.Entity = responsValue;
                            }
                            else
                            {
                                //throw new InvalidServiceCallResponseException();
                            }
                        }
                        else
                        {
                            //throw new InvalidServiceCallResponseException();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                responseMessage.RaisedException = ex;
                responseMessage.DidSucceed = false;
            }
            return responseMessage;
        }

        private async Task<ServiceStatusMessage> PostRequestReturnByteArrayAsync(string endpointRelativePath, bool authorise, HttpContent bodyContent)
        {
            var responseMessage = new ServiceStatusMessage<byte[]>();
            try
            {
                var url = await GetPathForServiceCall(endpointRelativePath);

                using (var client = new HttpClient())
                {
                    await SetupHttpClient(client, url, authorise);

                    var response = await client.PostAsync(url, bodyContent);

                    responseMessage.StatusCode = response.StatusCode.ToString();
                    responseMessage.DidSucceed = response.IsSuccessStatusCode;
                    responseMessage.ResponseMessage = response.ReasonPhrase;

                    if (response.IsSuccessStatusCode)
                    {
                        responseMessage.Entity = await response.Content.ReadAsByteArrayAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                responseMessage.RaisedException = ex;
                responseMessage.DidSucceed = false;
            }
            return responseMessage;
        }

        private async Task<ServiceStatusMessage> PostRequestReturnStreamAsync(string endpointRelativePath, bool authorise, HttpContent bodyContent)
        {
            var responseMessage = new ServiceStatusMessage<Stream>();
            try
            {
                var url = await GetPathForServiceCall(endpointRelativePath);

                using (var client = new HttpClient())
                {
                    await SetupHttpClient(client, url, authorise);

                    var response = await client.PostAsync(url, bodyContent);

                    responseMessage.StatusCode = response.StatusCode.ToString();
                    responseMessage.DidSucceed = response.IsSuccessStatusCode;
                    responseMessage.ResponseMessage = response.ReasonPhrase;
                    {
                        responseMessage.Entity = await response.Content.ReadAsStreamAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                responseMessage.RaisedException = ex;
                responseMessage.DidSucceed = false;
            }
            return responseMessage;
        }
        #endregion Private Methods


    }
}
