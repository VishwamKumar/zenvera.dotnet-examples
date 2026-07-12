# WeatherApp JWT Auth TestMe

## How To Test
1. Use PostMan
2. Create a request with POST, e.g. URL like
  https://localhost:8103/api/Auth/Login
  In the body, send the following 
 {
    "UserName":"TestUser1",
    "Password":"development-only-password"
  }

 You will get a token value if authentication is successful.

3. Now, Create a new request to get weather, e.g.
   https://localhost:8103/api/WeatherForecast

 In Authorization tab, Selected Bearer Token, and copy the Token value you had received
 
This an example of of very basic implementation of JWT Token    