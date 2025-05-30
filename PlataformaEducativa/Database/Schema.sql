<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.0" />
    <PackageReference Include="Google.Apis.Drive.v3" Version="1.68.0.3411" />
    <PackageReference Include="Google.Apis.Auth" Version="1.68.0" />
    <PackageReference Include="Microsoft.AspNetCore.Authentication.Cookies" Version="2.2.0" />
  </ItemGroup>

</Project>