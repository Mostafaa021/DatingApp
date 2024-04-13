This project is a modern dating application built with Angular 16 for the frontend and .NET 6 for the backend. 
It incorporates essential features such as clean architecture, identity role management, and real-time chat functionality using SignalR.

Archeticture
Clean Architecture: The application is structured following the principles of clean architecture, promoting separation of concerns and maintainability.
Unit of Work Pattern 

Identity Role Management: User authentication and authorization are implemented using .NET Identity, allowing for role-based access control and user management.

SignalR Chat: Real-time messaging functionality enables users to engage in seamless conversations with each other, enhancing the interactive experience of the application.

Technologies Used
The project utilizes the following technologies:

Frontend: Angular 16
Backend: .NET 6
Authentication & Authorization: .NET Identity
Real-time Communication: SignalR

Getting Started
Follow these steps to set up the project locally:

Prerequisites
Node.js and npm installed on your machine
.NET 6 SDK installed
Angular CLI installed globally (npm install -g @angular/cli)
Installation
Clone the repository: git clone https://github.com/Mostafaa021/DatingApp.git
Navigate to the backend directory: cd DatingApp/backend
Restore .NET dependencies: dotnet restore
Run the backend server: dotnet run
Navigate to the frontend directory: cd ../frontend
Install frontend dependencies: npm install
Start the Angular development server: ng serve
Usage
Once the project is set up and running, you can access the application through your web browser.
Register as a new user or log in with existing credentials to explore the dating platform. 
Use the chat feature to communicate with other users in real-time.

Contributing
Contributions to the project are welcome! If you find any bugs or have suggestions for new features, please open an issue or submit a pull request.
