# WeatherApp JWT Auth TestMe

## How To Test
1. Use PostMan        
2. Make sure you connection string in appsetting to a SQL Server Database is correct
3. Run the following on developer command prompt 
    dotnet ef migrations add InitialCreate
    dotnet ef database update
4. Once you run this app, in Swagger, use register method to register a user
5. Using login method, get a token 
6. Now, paste this token in Authorze option and then Get weather forecast
 
This an example of of very basic implementation of JWT Token using Identity framework of EFC.
It incorportates adding custom field. For more info see 
https://learn.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-7.0

