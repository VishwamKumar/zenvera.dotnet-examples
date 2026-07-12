# WeatherApp BasicAuth TestMe

## How To Test
1. Use PostMan
2. Create a request with GET, e.g. URL like
  https://localhost:8102/api/WeatherForecast
3. In Authorization tab, choose Basic Authorization, eneter username and password

Note: This username and password gets converted to base64 string, so if you are calling it from an API, the Authorization value must be in base 64 string.
    