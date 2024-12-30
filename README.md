# Sckoopy Web API

## ✨ **Overview**

The **Sckoopy Web API** is a robust and scalable API designed to handle modern web application needs with an efficient architecture. This project utilizes **.NET 8**, **Entity Framework Core**, and a multi-layered architecture to ensure maintainability, flexibility, and performance. The API was developed with best practices in mind, making it easy to scale and integrate with various platforms.

---

## 🌍 **Features**

- **RESTful Endpoints**: Offers clean and predictable RESTful APIs for easy integration.
- **Multi-layered Architecture**: Separation of concerns with distinct layers:
  - **Controller Layer**: Handles HTTP requests and responses.
  - **Service Layer**: Encapsulates business logic.
  - **Data Access Layer**: Manages database interactions with **Entity Framework Core**.
- **Cloud Integration**: Hosted on **Azure** with database and API running on Azure services for high availability.
- **Authentication-Ready**: Uses a token-based authentication mechanism for secure API consumption.
- **CORS Enabled**: Configured to allow cross-origin requests, making it flexible for frontend integration.
- **Cloudinary Integration**: Handles media uploads seamlessly using the Cloudinary service.

---

## 📊 **Technologies Used**

- **.NET 8**: The backbone of the API for building scalable and high-performance web applications.
- **Entity Framework Core**: Simplifies database interactions while supporting migrations and complex queries.
- **Cloudinary SDK**: For media management and upload functionality.
- **Azure**: For hosting both the database and the API.
- **SQL Server**: Robust database solution integrated with **Entity Framework**.

---

## 🛠️ **Technical Details**

### 🎨 Architecture

This project implements a **multi-layered architecture** to ensure a clean separation of concerns:

1. **Controller Layer**:

   - Receives and processes HTTP requests.
   - Handles route definitions and delegates operations to the service layer.

2. **Service Layer**:

   - Contains all the business logic.
   - Acts as a bridge between the controller and the data access layer.

3. **Data Access Layer**:

   - Built using **Entity Framework Core**.
   - Manages database operations such as CRUD functionality.
   - Abstracted into a repository-like structure to make the code testable and maintainable.

---

### 🏛️ Database Integration

- **Entity Framework Core** was utilized to connect to a **SQL Server** database hosted on **Azure**.
- The database connection string is managed through **environment variables** for enhanced security and flexibility.
- Migrations were used to manage database schema changes efficiently.

Example of the connection string in the project:

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING")));
```

---

### ✨ API Highlights

- **Endpoints**:
  - `POST /users`: Create a new user.
  - `GET /posts`: Retrieve all posts.
  - `POST /comments`: Add a comment to a post.
  - `POST /upload`: Upload a file to Cloudinary.
- **Secure Access**:
  - The API supports token-based authentication .
  - Swagger was configured for testing but disabled in production for security purposes.
- **Cloudinary Integration**:
  - Configured to store and retrieve media files.
  - Cloudinary credentials are securely stored in environment variables.

Example of Cloudinary configuration:

```csharp
builder.Services.AddSingleton(c => {
    var cloudinary = new Cloudinary(new Account(
        Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME"),
        Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY"),
        Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET")
    ));
    return cloudinary;
});
```

---

## 💡 **Key Advantages**

- **Scalability**: Hosted on Azure, making it capable of handling growing demands.
- **Maintainability**: The multi-layered architecture ensures clean code and easy debugging.
- **Security**: Utilizes environment variables for sensitive data and supports secure token authentication.
- **Flexibility**: Supports integration with multiple front-end platforms and third-party services.

---

## 🚀 **Deployment**

The project is deployed on **Azure**, leveraging:

- **Azure App Service** for hosting the API.
- **Azure SQL Database** for persistent data storage.

Steps to deploy:

1. Clone the repository to your local machine.
2. Add the required **environment variables** for database and Cloudinary integration.
3. Use the following command to publish:
   ```bash
   dotnet publish -o ./publish
   ```
4. Push the published files to Azure App Service.

---

## 🔗 **Getting Started Locally**

1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/SckoopyWebAPI.git
   ```
2. Restore dependencies:
   ```bash
   dotnet restore
   ```
3. Run the project:
   ```bash
   dotnet run
   ```
4. Access the API at `https://localhost:5001`.

---

## 🎮 **Future Enhancements**

- **Enhanced Security**: Implement OAuth2 for external app authentication.
- **Logging**: Add a robust logging mechanism using **Serilog**.
- **Unit Tests**: Expand unit and integration test coverage.

---

## 👤 **Contributing**

Contributions are welcome! Feel free to fork this repository and submit pull requests with your changes.

---

## 🔒 **Environment Variables**

Make sure to set the following environment variables for local development:

- `SQL_CONNECTION_STRING`: Connection string for Azure SQL Database.
- `CLOUDINARY_CLOUD_NAME`: Cloudinary cloud name.
- `CLOUDINARY_API_KEY`: Cloudinary API key.
- `CLOUDINARY_API_SECRET`: Cloudinary API secret.



---

## 🌐 **Contact**

Feel free to reach out for any queries or support:

- **GitHub**: https\://github.com/Amro-Aladghem
- **Email**: [ameraladghem@gmail.com](mailto\:ameraladghem@gmail.com)

