﻿using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace YourApp.Services
{
    public interface IApiClient
    {
        Task<HttpResponseMessage> GetAsync(string path);
        Task<HttpResponseMessage> PostAsync(string path, object payload);
    }

    public class ApiClient : IApiClient
    {
        private readonly HttpClient client;
        private readonly IConfigurationSection appConfig;
        private readonly DiscoveryCache discoveryCache;
        private readonly ILogger logger;

        public ApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<ApiClient> log)
        {
            appConfig = configuration.GetSection("Challenges");
            discoveryCache = new DiscoveryCache(appConfig.GetValue<string>("GatekeeperUrl"));
            client = httpClientFactory.CreateClient("challengesHttpClient");
            logger = log;
        }

        private async Task<string> GetTokenAsync()
        {
            var discovery = await discoveryCache.GetAsync();
            if (discovery.IsError)
            {
                logger.LogError(discovery.Error);
                throw new ApiClientException("Couldn't read discovery document.");
            }

            var tokenRequest = new ClientCredentialsTokenRequest
            {
                Address = discovery.TokenEndpoint,
                ClientId = appConfig.GetValue<string>("ClientId"),
                ClientSecret = appConfig.GetValue<string>("ClientSecret"),
                
                // The ApiResourceName of the resources you want to access.
                // Other valid values might be `comms`, `health_data_repository`, etc.
                // Ask in #dev-gatekeeper for help
                Scope = "user_groups gatekeeper health_data_repository"

            };

            var response = await client.RequestClientCredentialsTokenAsync(tokenRequest);
            if (response.IsError)
            {
                logger.LogError(response.Error);
                throw new ApiClientException("Couldn't retrieve access token.");
            }
            return response.AccessToken;
        }

        public async Task<HttpResponseMessage> GetAsync(string uri)
        {
            client.SetBearerToken(await GetTokenAsync());
            return await client.GetAsync(uri);
        }

        public async Task<HttpResponseMessage> PostAsync(string uri, object payload)
        {
            client.SetBearerToken(await GetTokenAsync());
            return await client.PostAsJsonAsync(uri, payload);
        }
    }

    public class ApiClientException : Exception
    {
        public ApiClientException(string message) : base(message)
        {
        }
    }
}
