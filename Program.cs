using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;


var builder = WebApplication.CreateBuilder(args);

//This method is going to ask for Microsoft EntraID for OIDC Authentication using Access Tokens and ID Tokens
//using Microsoft.AspNetCore.Authentication.OpenIdConnect; the sso logic is wrapped within this package there is no aditional logic required for sso.
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
  .AddMicrosoftIdentityWebApp(builder.Configuration);

builder.Services.AddControllersWithViews(options => 
{
    var policy = new AuthorizationPolicyBuilder()
         .RequireAuthenticatedUser()
         .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
      
});

// This method requires only Authenticted users to view the restricted URL
builder.Services.AddRazorPages()
    .AddMvcOptions(options => 
    {
        var policy = new AuthorizationPolicyBuilder()
         .RequireAuthenticatedUser()
         .Build();
        options.Filters.Add(new AuthorizeFilter(policy));


    })
    .AddMicrosoftIdentityUI();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


//This Function asks for OIDC Authentication before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
