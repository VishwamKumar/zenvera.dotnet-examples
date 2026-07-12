# WeatherApp OAuth2 using google Firebase in GraphQL 

## How To Test

1. You need to create an account in google firebase for this project
     a. Once account is setup, download the config json file by going into 
        Project Settings=>Sevice Accounts==>Generate new private key
     b. Save the json file as firebasesettings.json in the root of the project 
     c. In this project firebasesettings.json has been excluded for security purpose  
Note: This firebase settings file is used for passing Credentials using "GOOGLE_APPLICATION_CREDENTIALS" in LaunchSettings.json

2. Run the project
3. Use PostMan to create following request to get a token for a user using google firebase
 a. To Create User and Token

 https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=AIzaSyB_fCvMDnNFgBEBuF3WLDlVkaWWx7U0yeE
 {
    "email":"demo@demo.com",
    "password":"test1234",
    "returnSecureToken":true
}

To refresh the token, if user already exists or token expired, use following
https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=AIzaSyB_fCvMDnNFgBEBuF3WLDlVkaWWx7U0yeE
{
    "email":"demo@demo.com",
    "password":"test1234",
    "returnSecureToken":true
}


4. Now, Create a new POST request to get weather through GraphQL using this token, e.g.
   https://localhost:8303/graphql

  Post the following in the Body of GraphQL
  {
    weatherForecast {
        date
        temperatureC
        summary
    }
}


 Note: In Authorization tab, Selected Bearer Token, and copy the Token value you had received
 For a valid Token you will get a valid reponse, else you will get an auth error

This an example of of very basic implementation of OAuth2 Using Firebase in GraphQL API   