# SimpleE2ETester

## Get started

**Instal package via nuget package manager console** 

```
Install-Package SimpleE2ETesterLibrary -Version 1.0.4
```
## Purpose
For example we want to test the workflow where user registers an account in our application.Typical scenario would look like:
  >1.User registers an account by providing necessary data like email,password,nickname etc.<br/>
  >2.User completes registration process by providing valid registration token.<br/>
  >3.User is logging into his newly created account by passing an email and password.<br/>
  >4.User performs some actions that are available in our application, for example adds a review to recenly watched movie.

Testing more complex scenario can quickly translate into code that can be unmaintainable,complex and might look like:
```csharp
    private static HttpClient _httpClient = new HttpClient();
    
    var registrationDto = new {Nickname="nickname",Password="password"}.ToString();
    var registrationUri = new Uri("Register_uri");
    var registrationRequest = new HttpRequestMessage(HttpMethod.Post, uri)
    {
        Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json")
    };
            
    var registrationResponse = await _httpClient.SendAsync(registrationRequest);
    var registrationResponseContent = registrationResponse.Content.ReadAsStringAsync();
    
    var completeRegistrationDto = new {Token="registration_token",Email="email"}.ToString();
    var completeRegistrationUri = new Uri("CompleteRegistration_uri");
    var completeRegistrationRequest = new HttpRequestMessage(HttpMethod.Put, completeRegistrationUri)
    {
        Content = new StringContent(JsonConvert.SerializeObject(completeRegistrationDto), Encoding.UTF8, "application/json")
    };
            
    var completeRegistrationResponse = await _httpClient.SendAsync(completeRegistrationRequest);
    var completeRegistrationContent = response.Content.ReadAsStringAsync()
```
And so on and so on... Somewhere in the middle you may have to check if the response status code is correct,<br />
maybe get some more complex data structure from the response.So concluding it is definitely not easy to read or modify given code.

So the purpose of this simple library is to ease up the E2E testing process by introducing more redable way to perform same sort of operations.

## Example
  - Initialization
  ```csharp
    private readonly ISimpleE2ETester _tester;
    private static HttpClient _configuredHttpClient;
    
    _tester = new SimpleE2ETester(new AspNetHttpClient(_configuredHttpClient));
  ```
  - Create http requests
  ```csharp
  public class RegisterAccountHttpRequest : ISimpleHttpRequest
  {
        private readonly string _email;
        private readonly string _password;

        public RegisterAccountHttpRequest(string email,string password)
        {
            _email = email;
            _password = password;
        }

        public HttpRequestMessage ToHttpRequest()
        {
            var uri = new Uri("Register uri");
            var dto = new { Email= _email,Password=_password }.ToString();
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json")
            };

            return httpRequest;
        }
    }
  ```
  ```csharp
  public class GetRegistrationSummaryHttpRequest : ISimpleHttpRequest
  {
        private readonly string _email;

        public GetRegistrationSummaryHttpRequest(string email)
        {
            _email = email;
        }

        public HttpRequestMessage ToHttpRequest()
        {
            return new HttpRequestMessage(HttpMethod.Get, new Uri(@"registration_url/" + _email));
        }
    }
  
  ```
  ```csharp
  public class CompleteRegistrationHttpRequest : ISimpleHttpRequest
  {
        private readonly string _email;
        private readonly string _registrationToken;

        public CompleteRegistrationHttpRequest(string email,string registrationToken)
        {
            _email = email;
            _registrationToken = registrationToken;
        }

        public HttpRequestMessage ToHttpRequest()
        {
            var uri = new Uri("CompleteRegistration uri");
            var dto = new { Email= _email,RegistrationToken= _registrationToken }.ToString();
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json")
            };

            return httpRequest;
        }
    }
  ```
  ```csharp
  public class LoginHttpRequest : ISimpleHttpRequest
  {
        private readonly string _email;
        private readonly string _password;

        public LoginHttpRequest(string email,string password)
        {
            _email = email;
            _password = password;
        }

        public HttpRequestMessage ToHttpRequest()
        {
            var uri = new Uri("Login uri");
            var dto = new { Email = _email, Password = _password }.ToString();
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json")
            };
            
            return httpRequest;
        }
    }
  ```
  
  ```csharp
  public class AddReviewHttpRequest : ISimpleHttpRequest
  {
      private readonly Guid _userGuid;
      private readonly string _review;
      private readonly string _token;

      public AddReviewHttpRequest(Guid userGuid,string review,strin token)
      {
          _email = email;
          _review = review;
          _token = token;
      }

      public HttpRequestMessage ToHttpRequest()
      {
          var uri = new Uri("Add review uri");
          var dto = new { UserGuid = _userGuid, Review = _review }.ToString();
          var httpRequest = new HttpRequestMessage(HttpMethod.Post, uri)
          {
              Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json")
          };
          
          httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer",_token);
          
          return httpRequest;
      }
  }
  ```
  - Test the workflow
  ```csharp
  
        [Test]
        public async Task AddReview_WhenCalled_ShouldReturnResponseWithCreatedStatusCode()
        {
            string email = "testemail@gmail.com";
            string password = "password";
            string registrationToken = string.Empty;
            string token = string.Empty;
        
            new SimpleE2ETester()
                .Act(()=>Logger.Information("RegisterUser"))
                .SendAsync(()=>new RegisterAccountHttpRequest(email,password))
                .SendSynchronouslyAndMapToResult(()=>new GetRegistrationSummaryHttpRequest(email))
                .Tap(async(response)=>registrationToken=await response.GetResponseContentAsync<string>())
                .Back()
                .SendAsync(()=>new CompleteRegistrationHttpRequest(email,registrationToken))
                .Delay().Seconds(10)
                .SendSynchronouslyAndMapToResult(()=>new LoginHttpRequest(email,registrationToken))
                .Tap(async(response)=>token=await response.GetResponseContentAsync<string>())
                .Back()
                .ArePrecedingRequestsSuccessful((flag) => flag.Should().BeTrue())
                .SendAsyncAndMapToResult(()=>new AddReviewHttpRequest(Guid.NewGuid(),"Review",token))
                .Tap((result) => result.IsCreated().Should().BeTrue())
                .Back()
                .AddTask(async () => await DoSomeAdditionalWorkAsync())
                .DoAsync();
        }
    
  ```
  ## Used libraries
  <ul>
  <li>FluentValidation (https://fluentvalidation.net/)</li>
  <li>Newtonsoft.Json (https://www.newtonsoft.com/json)</li>
  <li>.NET Core 3.1 (https://dotnet.microsoft.com/download)</li>
</ul>

      
  
