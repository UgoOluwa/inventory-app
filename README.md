# inventory-app
This application manages the inventory of an organization

#### inventory microservice which includes; 
* ASP.NET Core Web API application 
* REST API principles, CRUD operations 
* **Mongo DB NoSQL database** connection on docker
* N-Layer implementation with Repository Pattern
* Swagger Open API implementation
* Dockerfile implementation


#### User Interface inventory microservice which includes;
* Asp.net Core Web Application with Razor template
* Aspnet core razor tools - View Components, partial Views, Tag Helpers, Model Bindings and Validations, Razor Sections etc.. 

#### Docker Compose establishment with all microservices on docker;
* Dockerization of microservices
* **Dockerization** of database
* Override Environment variables

## Run The Project
You will need the following tools:

* [Visual Studio 2019](https://visualstudio.microsoft.com/downloads/) if viewing and editing the project
* [.Net Core 5.0 or later](https://dotnet.microsoft.com/download/dotnet-core/5.0)
* [Docker Desktop](https://www.docker.com/products/docker-desktop)

### Installing
Follow these steps to get your development environment set up: (Before Run, Start the Docker Desktop)
1. Clone the repository
2. At the root directory which include **docker-compose.yml** files, run below command on your CMD:
```csharp
docker-compose -f docker-compose.yml -f docker-compose.override.yml up –d
```
3. Wait for docker compose all microservices. That’s it!

4. You can **launch microservices** as below urls:
* **Inventory API -> http://localhost:8000/swagger/index.html**
* **Web UI -> http://localhost:8003**

5. Launch http://localhost:8003/ in your browser to view the Web UI.

