# InTimePro Backend API

This backend is a .NET Framework 4.8 API application for the Flutter InTimePro client.

## Features
- Auth endpoints: login, refresh, logout
- Auth providers: email, google, microsoft
- In-memory employee store for demo authentication
- Swagger UI

## Run
1. Open `backend/InTimePro/InTimePro.sln` in Visual Studio.
2. Restore NuGet packages.
3. Start `InTimePro.Api`.
4. API base URL defaults to `http://localhost:5055`.
5. Swagger UI is available at `http://localhost:5055/swagger`.

## Demo credentials
- Email: `employee@intimepro.com`
- Password: `Password@123`
