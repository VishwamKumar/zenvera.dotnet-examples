# WeatherApp JWT Auth in GraphQL TestMe

## How To Test
1. Use PostMan
2. Create a request with POST, e.g. URL like
  https://localhost:8301/api/Auth/Login
  In the body, send the following 
 {
    "UserName":"TestUser1",
    "Password":"development-only-password"
  }

 You will get a token value if authentication is successful.

3. Now, Create a new POST request to get weather through GraphQL, e.g.
   https://localhost:8301/graphql

  Post the following in the Body of GraphQL
  {
    weatherForecast {
        date
        temperatureC
        summary
    }
}


 In Authorization tab, Selected Bearer Token, and copy the Token value you had received
 if you a valid Token you will get a valid reponse, else you will get an auth error

This an example of of very basic implementation of JWT Token using GraphQL   