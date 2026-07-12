In order to bring Methods of service, following things are needed
1. Make sure following packages are installed
	 <PackageReference Include="Microsoft.AspNetCore.Grpc.JsonTranscoding" Version="8.0.6" />
	 <PackageReference Include="Microsoft.AspNetCore.Grpc.Swagger" Version="0.8.6" />

 2. google folder is included, this is manually downloaded from google

 3. When you create a Protos for service, make sure that is added in project file, here is an example
	<Protobuf Include="Protos\todo.proto" GrpcServices="Server" />
