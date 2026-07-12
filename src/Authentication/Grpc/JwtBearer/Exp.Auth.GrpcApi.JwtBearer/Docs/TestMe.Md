# WeatherApp Jwt Auth TestMe

## How To Test
1. Use PostMan
2. Create a request with POST, e.g. URL like
  https://localhost:8202/api/Auth/Login
  In the body, send the following 
 {
    "UserName":"TestUser1",
    "Password":"development-only-password"
  }

 You will get a token value if authentication is successful.

3. Create a gRPC request, name it e.g. GetWeatherForecast
   a. In URL put : localhost:8202 [ensure it is secure, means TLS is enabled, lock is showing up]
   b. In Select a method, select : Weatherforecaster/GetWeatherForecast
   c. In message section: Add the following 
    {
      "days": 4
    }

3. In Authorization tab, choose Bearer Token, and put the token value 
     
4. Invoke the call, you should receive a reponse in JSON format if token is valid else error
 



    