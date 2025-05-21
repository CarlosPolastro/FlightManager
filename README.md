# FlightManager

Project Structure

- **0 - Presentation**
  - FlightManager.Web
- **1 - Application**
  - FlightManager.API
- **2 - Domain**
  - FlightManager.Business
  - FlightManager.Domain
- **3 - Data**
  - FlightManager.Data


How to Configure and Run

- **Configure**

  -  Go to path 'FlightManager/FlightManager.Data/DBScripts'
  -  Execute script 'CREATE DATABASE.sql' in a SQL Server Database
  -  Go to file 'FlightManager/FlightManager.API/appsetting.json'
  -  Change section 'ConnectionStrings/DefaultConnection' 

- **Run**
  - Run FlightManager.API
  - Run FlightManager.Web