# Goal

I am trying to create a .NetCore 2.1 WebApi application, which is authenticated against Azure Active Directory V2 using bearer tokens, and host a swagger UI for the application.

# Steps Taken

- I created a new ASP.Net Core web application, selected the API option and changed authentication to "Work or School accounts".
- This auto generated a solution with AAD bearer token integration auto configured and also provisioned the AAD application.
- I then added the Swashbuckle.AspNetCore nuget package and provided some basic configuration.
- I added a security filter so that swashbuckle would add the security definitions to the API methods I add the Authoirze attribute to
- On my AAD Application I enabled implict flow and the redirect url so that my login attempts would succeed.


# Problem
I am able to login to AAD using the swagger UI with the current configuration, and it returns a bearer token, which swagger is sending along on any of the UI invoked calls but authentication on the WebAPI level is failing.

The error I receive is `Bearer error="invalid_token", error_description="The signature is invalid" `. I have tried to investigate this issue and pin point the cause and solution but am currently stumpt.

I am able to get the swagger UI working using AAD's V1 endpoints by passing the resource field in as the WebAPI's AAD ClientId. This then results in a JWT token with the Audience set to the AAD ClientId, this seems to work. With the V2 endpoints the Jwt token has Graph API as its audience and I think this is my problem. I have not yet been able to solve the issue.

