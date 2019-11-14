# SantanderCodingTest
SANTANDER SENIOR BACKEND DEVELOPER CODING TEST 



1. Tutorial to publishing an API 

1.1 Tutorial experience on publishing an API to an IIS server

	https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/iis/?view=aspnetcore-2.2
	
1.2 Tutorial experience on publishing an API on Linux with Apache

	https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-apache?view=aspnetcore-2.2
	

2. Configuring the application

	On the file [appsettings.json] you can configure the settings :
  
  "ApiVersion": API Version 

  For Limit Rate more information :
  https://github.com/stefanprodan/AspNetCoreRateLimit/wiki

  "IpRateLimiting": {
    "EnableEndpointRateLimiting": true = Enable Rate Limit
    "StackBlockedRequests": false = Stack Blocked Requests
    "RealIPHeader": "X-Real-IP" = IP avaluation mode
    "ClientIdHeader": "X-ClientId"
    "HttpStatusCode": 429 = Result Status Code Error
    "GeneralRules": [
      {
        "Endpoint": "*:/api/*",
        "Period": "1s" = Period of avaluation
        "Limit": 2 = Limit per Period
      }

  "AmountItens": Maximum itens on response of method "BestStories"
  "UrlBestStoriesList": The IDs for the "best stories" can be retrieved from this URL
  "UrlStoryDetail": The details for an individual story ID can be retrieved from this URL
  "TimeFormat": Time format on time field result
  "BestStoriesCacheSeconds": Time in seconds to release data from cache
  
3. How to run the application

	After publishing, call the service :
	
3.1 On Local Host
	https://localhost:44376/api/v1/BestStories
	

3.2 On Published Server	
	
	[URL DOMAIN]/v1/BestStories

4. Proposed enhancements

4.1 Swagger tool
4.2 Logger 
4.3 Native Cache
4.4 Database to History
4.5 Code Review

