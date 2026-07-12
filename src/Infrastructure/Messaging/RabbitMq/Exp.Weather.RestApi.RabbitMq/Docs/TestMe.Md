# RabbitMQ Weather API test notes

## RabbitMq
    1. Install RabbitMq if it is not already installed
    2. Make sure RabbitMq is working, use http://localhost:15672 if installed locally to access the admin portal

## Uses of this package
    1. Make sure app using this library has following settings in appsettings 
      "RabbitMqSettings": {
        "Host": "localhost", //default value
        "Port": 5672, ////default value, 15672 is used for http://localhost:15672
        "AuthEnabled": true,
        "UserName": "ENV:EXP_RABBITMQ_USERNAME", //default value
        "Password": "ENV:EXP_RABBITMQ_PASSWORD", //default value
        "VirtualHost": "/", //default value
        "Exchange": "test_exchange", //default value "", if defined create exchange type as Topic - highly flexible and only this one supported in this package 
        "RoutingKey": "test" //default value "" 
      }

  Note & Limitations: 
      a. If Authentication is required, you must provide username and password
      b. Queue must be created with feature : durable: false, exclusive: false, autoDelete: false
      c. Exchange must be created with exchange type as Topic - highly flexible 


    2. Add RabbitMq queuing service in program.cs
       using Vishwam.RabbitMqLibrary.Queuing.Services;

       builder.Services.AddRabbitMqService(builder.Configuration);

    
    3. To Send Message into Q and Consume from Q,
       using Vishwam.RabbitMqLibrary.Queuing.Services;
       Inject IRabbitMqService rabbitMqService,

       //To Send in Q
       await rabbitMqService.PublishAsync(msgAsString);

       //To Receive from Q
       var message = await rabbitMqService.ConsumeAsync();

    4. Run this app and get weather for a city, 
        Application will create weather forecast and send it to RabbitMQ
        then it will retrieve the message from RabbitMQ, 
        the reponse object will contain both information