namespace Exp.Auth.GrpcApi.JwtBearer.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class AllowAnonymousAttribute : Attribute
{
    //This Attribute is needed to bypass AuthController method to generate token
}
