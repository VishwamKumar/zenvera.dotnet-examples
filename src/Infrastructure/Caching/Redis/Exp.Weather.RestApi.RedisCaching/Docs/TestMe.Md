# WeatherApp Using Redis Cache TestMe

## Installing Redis
1. Redis Install  
     On Linux - install redis, 
     On Windows - you can also install Memurai which uses redis
     Make sure Redis/Memurai service is running

2. Make sure app has following settings in appsettings
 
"RedisCacheSettings": {
    "Host": "localhost",
    "Port": 6379,
    "AuthEnabled": false,
    "Password": "your_redis_password",
    "Channel": "your_channel",
    "DB": 0
  }       
3. Run this app and get weather for a city, 
   first time it generates the weather forecast as it won;t be avialble in cache and 
   then next calls for same city will use the cache