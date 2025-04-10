## 📦 Clone the Project
Clone the entire project repository:
```bash
git clone https://github.com/boldevs/notes_app.git
```

## 📦 Setup backend
### 1. Execute the Database Schema

- Open SQL Server Management Studio (SSMS) or your preferred SQL tool.
- Create a new database named: NotesAppDb
- Run the SQL script to create the necessary tables
- 📄 File location: notes-application/DbSchemaScript/notesapp.sql

### 2. Setup Database Connection in appsettings.json

- After creating the database, you need to configure the backend to connect to it.
- Open the backend project:
  - notes-application/NotesApp.Api
- Locate and open the file:
  - appsettings.json
  
```bash
"ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=NotesAppDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
```
- Please modify the Server value in the connection string to match your SQL Server instance name, where you previously created the NotesAppDb database.

### 3. Setup CORS in Backend

To allow the frontend (Vue app) to communicate with the backend (ASP.NET API), you need to configure CORS (Cross-Origin Resource Sharing).
- Register the CORS Policy the file: Program.cs
```bash
// Add CORS policy service
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // frontend dev server
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
```

### 4. Run backend
To start the ASP.NET Core API:

```bash
cd notes-application\NotesApp.Api
dotnet build
dotnet run or dotnet watch run
```

## 📦 Installation & Setup frondend

```bash
cd notes_app/notesapp-frontend
npm install
npm run dev 
```
### 🔗 API Base URL Configuration

To connect your frontend with the backend, you need to set the correct API base URL.
Open the file:
src/utils/api.ts
```bash
const API = axios.create({
  baseURL: 'http://localhost:5047', // 👈 Update this to match your backend server
  headers: {
    'Content-Type': 'application/json',
  },
});
```

## 📦 Usage
Once the web application is running, please click the link to register as a user in order to gain access to the application.
