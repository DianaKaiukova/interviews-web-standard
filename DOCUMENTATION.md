Project Documentation: Task Management System
1. Prerequisites
.NET 8 SDK: Download

Node.js 18+: Download

SQLite: Comes pre-installed with .NET

Git: Download

2. Project Structure
task-management/
├── api/          # .NET 8 API backend
└── client/       # Nuxt 3 frontend
3. Setup & Run API
bash
# Navigate to API directory
cd api

# Restore dependencies
dotnet restore

# Apply database migrations
dotnet ef database update

# Run the API (default port: 5221)
dotnet run
API Endpoints:

Tasks: GET/POST/PUT/DELETE http://localhost:5221/api/tasks

Tags: GET/POST/PUT/DELETE http://localhost:5221/api/tags

Swagger UI: http://localhost:5221/swagger

4. Setup & Run Client
bash
# Navigate to client directory
cd client

# Install dependencies
npm install

# Run the client (default port: 3000)
npm run dev
Client Routes:

Tasks: http://localhost:3000/

Tags: http://localhost:3000/tags

5. Environment Configuration
API Port:

Change in api/Properties/launchSettings.json:

json
"applicationUrl": "http://0.0.0.0:5221"
Client API Base URL:

Create .env file in client/:

env
API_BASE=http://localhost:5221
6. Key Features
Task Management:

Create, edit, delete tasks

Add descriptions

Tag System:

Create and manage tags

Assign tags to tasks

Relationship:

Many-to-many task-tag relationships

Responsive UI:

Works on mobile and desktop

7. Troubleshooting
Common Issues:

Port Conflicts:

Change ports in launchSettings.json (API) or nuxt.config.ts (client)

Database Issues:

bash
cd api
dotnet ef database drop --force
dotnet ef database update
CORS Errors:

Ensure app.UseCors("AllowAll") is in Program.cs

Verify client's .env matches API port

Firewall Blocking:

powershell
# Run as Administrator:
New-NetFirewallRule -DisplayName "Allow API Port" -Direction Inbound -LocalPort 5221 -Protocol TCP -Action Allow
8. Testing
API Tests:

bash
cd api.Tests
dotnet test
Client Manual Testing:

Create tasks with different titles

Add/remove tags

Assign tags to tasks

Verify updates persist after refresh

9. Deployment
API:

Publish to Azure App Service or Docker container

Update database connection string in production

Client:

Static hosting (Netlify, Vercel, Azure Static Web Apps):

bash
cd client
npm run generate
