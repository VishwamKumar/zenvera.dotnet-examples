# WeatherApp OAuth2 TestMe

## How To Test
1. Use PostMan
2. Create a request with POST, e.g. URL like
  https://localhost:8105/connect/token
    a. In Headers section, add following key and value
    Content-Type : application/x-www-form-urlencoded

    b.  In the body, choose x-www-form-urlencoded and set the following params as Key and Value
    grant_type : client_credentials
    client_id : WeatherAppClient,
    client_secret : development-only-client-secret
    scope : weather_read  
  
  Make sure app is running, then Send, 
  You will get a token info, e.g. like below
  {
    "access_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6IjdjOWIyYThlNGY2ZDVhMWIzZTljN2Q1ZjhhMmIxZTNmIiwidHlwIjoiYXQrand0In0.eyJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MTAwIiwibmJmIjoxNzE5MjgzNzk5LCJpYXQiOjE3MTkyODM3OTksImV4cCI6MTcxOTI4NDA5OSwiYXVkIjoiV2VhdGhlckFwcCIsInNjb3BlIjpbIndlYXRoZXJfcmVhZCJdLCJjbGllbnRfaWQiOiJXZWF0aGVyQXBwQ2xpZW50IiwianRpIjoiRjhCNUM1NUQ0Njc0QUJCRDRBRTBBMzc5QzA4MkUzMjIifQ.FU6oMKFB1EMo1oFIbWtPnIg88W1a_7lLo90swu0OCetvWxMLc9FHoJpqSjZRE2YrgdYwKH_SqLvPZAPQ4WXX5PNIkK7OHqkJ-8zhG0QjCWnBasv68vJUZbC9_v2IxjSlFNmOdm-sF8zX9JSK921GF_waUGEg9u11pT4SkUrYEfJ7i7slXUJni8gpM9W_yOvxveUejLeH4mulTlQ1oW1NzmrWbkv6Fk3ixYJ-HHnBRa0oKGqZ8xnQDlCf_I-sXonpHCu46i99i1mA2-Mt-8z_PhvRXU0fUSfgnzj4aJtdlGT4dLpOjlV8TvmLY7lWfje1Rkq74Csaj3zVbksYMN3Cfw",
    "expires_in": 300,
    "token_type": "Bearer",
    "scope": "weather_read"
}
 
 Note: connect/token is part of Identity Server, there is no seperate controller 

3. Now, Create a new request to get weather, e.g.
   https://localhost:8105/api/WeatherForecast

 In Authorization tab, Selected Bearer Token, and copy the access_token value for Token.
 
This an example of of very basic implementation of OAuth2 Implementation

