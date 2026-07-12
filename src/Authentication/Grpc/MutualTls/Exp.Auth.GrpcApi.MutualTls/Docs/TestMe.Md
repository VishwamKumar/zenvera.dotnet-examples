# WeatherApp MTLS Auth TestMe

## How To Test
1. Use PostMan
2. Add Client Certificates:
    a. Go to the Settings (the gear icon) in the upper-right corner of Postman.
    b. Navigate to the Certificates tab.
       Click on Add Certificate.
       Enter the host (localhost in this example) and port (8203).
       Upload your client certificate and private key files (e.g., client.crt and client.key) 
          or Personal Information Exchange file and passowrd (e.g. client.cert.pfx, test1234)
 
3. Create a gRPC request, name it e.g. GetWeatherForecast
   a. In URL put : localhost:8203 [ensure it is secure, means TLS is enabled, lock is showing up]
   b. In Select a method, select : Weatherforecaster/GetWeatherForecast
   c. In message section: Add the following 
    {
      "days": 2
    }

3. Invoke the call, you should receive a reponse in JSON format
 
## Tips on Generating a self-signed Certificates
Use bash or Powershell command prompt

1.Generate a Root Certificate:
openssl genrsa -out rootCA.key 2048
openssl req -x509 -new -nodes -key rootCA.key -sha256 -days 1024 -out rootCA.pem -subj "/CN=localhost"

2. Generate a Server Certificate:
openssl genrsa -out server.key 2048
openssl req -new -key server.key -out server.csr -subj "/CN=localhost"
openssl x509 -req -in server.csr -CA rootCA.pem -CAkey rootCA.key -CAcreateserial -out server.crt -days 500 -sha256

3. Generate a Client Certificate:
openssl genrsa -out client.key 2048
openssl req -new -key client.key -out client.csr -subj "/CN=localhost"
openssl x509 -req -in client.csr -CA rootCA.pem -CAkey rootCA.key -CAcreateserial -out client.crt -days 500 -sha256

4. Create Server Certificate 
openssl pkcs12 -export -out server.cert.pfx -inkey server.key -in server.crt

5. Create Client Certificate
 openssl pkcs12 -export -out client.cert.pfx -inkey client.key -in client.crt



    