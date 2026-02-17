# Microsoft Entra App Registration Setup (Demo)

1. Register a new app in Microsoft Entra ID.
2. Expose an API scope named `access_as_user`.
3. Define app roles: `Demo.Read` and `Demo.Admin`.
4. Configure token claims for roles/scopes as needed.
5. Set local environment variables:

```bash
export Entra__TenantId="<tenant-id>"
export Entra__ClientId="<client-id>"
export Entra__Audience="api://<client-id>"
export Entra__Instance="https://login.microsoftonline.com/"
```

6. Run the API and test endpoints with an access token from your Entra tenant.
