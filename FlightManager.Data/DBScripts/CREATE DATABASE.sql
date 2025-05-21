CREATE DATABASE [FlightManager]
COLLATE SQL_Latin1_General_CP1_CI_AS;
GO

CREATE TABLE [FlightManager].[dbo].[Airports] (
    ID INT IDENTITY(1,1) NOT NULL,
    Name VARCHAR(100) NOT NULL,
    IATACode VARCHAR(3) NOT NULL,
    ICAOCode VARCHAR(4) NOT NULL,
    Latitude DECIMAL(10,7) NOT NULL,
    Longitude DECIMAL(10,7) NOT NULL
    CONSTRAINT [PK_Aiports] PRIMARY KEY CLUSTERED([ID] ASC)
);

CREATE TABLE [FlightManager].[dbo].[AircraftTypes](
    ID INT IDENTITY(1,1) NOT NULL,
    Description VARCHAR(650) NOT NULL,
    PassangerCapacity INT NOT NULL,
    MaxFuelUsageLiterPerKm DECIMAL(8,2) NOT NULL,
    MaxFuelUsageLiterPerHour DECIMAL(8,2) NOT NULL,
    MaxFuelTakeoffEffortLiter DECIMAL(8,2) NOT NULL,
    CONSTRAINT [PK_AircraftTypes] PRIMARY KEY CLUSTERED([ID] ASC)
);

CREATE TABLE [FlightManager].[dbo].[Flights](
    ID BIGINT IDENTITY(1,1) NOT NULL,
    AircraftTypeID INT NOT NULL,
    DepartureAirportID INT NOT NULL,
    DestinationAirportID INT NOT NULL,
    DistanceKm DECIMAL(10,2) NOT NULL,
    TotalFuelLiter DECIMAL(10,2)  NOT NULL
    CONSTRAINT [PK_Flights] PRIMARY KEY CLUSTERED([ID] ASC)
);

ALTER TABLE [FlightManager].[dbo].[Flights] WITH CHECK ADD CONSTRAINT FK_Flights_AircraftTypes FOREIGN KEY (AircraftTypeID) REFERENCES [FlightManager].[dbo].[AircraftTypes](ID);
ALTER TABLE [FlightManager].[dbo].[Flights] WITH CHECK ADD CONSTRAINT FK_Flights_AiportsDeparture FOREIGN KEY (DepartureAirportID) REFERENCES [FlightManager].[dbo].[Airports](ID);
ALTER TABLE [FlightManager].[dbo].[Flights] WITH CHECK ADD CONSTRAINT FK_Flights_AirportsDestination FOREIGN KEY (DestinationAirportID) REFERENCES [FlightManager].[dbo].[Airports](ID);

INSERT INTO [FlightManager].[dbo].[Airports] (Name, ICAOCode, IATACode, Latitude, Longitude) VALUES ('London Heathrow Airport','EGLL','LHR', 51.4706001282, -0.4619410038);
INSERT INTO [FlightManager].[dbo].[Airports] (Name, ICAOCode, IATACode, Latitude, Longitude) VALUES ('Tokyo Narita International', 'RJAA','NRT', 35.7647018433 ,140.386001587);
INSERT INTO [FlightManager].[dbo].[Airports] (Name, ICAOCode, IATACode, Latitude, Longitude) VALUES ('Los Angeles International',	'KLAX',	'LAX', 33.94250107, -118.4079971);
INSERT INTO [FlightManager].[dbo].[Airports] (Name, ICAOCode, IATACode, Latitude, Longitude) VALUES ('Sydney Kingsford Smith Airport',	'YSSY',	'SYD', -33.9460983276, 151.177002907);
INSERT INTO [FlightManager].[dbo].[Airports] (Name, ICAOCode, IATACode, Latitude, Longitude) VALUES ('São Paulo-Guarulhos International',	'SBGR',	'GRU', -23.4355564117, -46.4730567932);

INSERT INTO [FlightManager].[dbo].[AircraftTypes] (Description, PassangerCapacity, MaxFuelUsageLiterPerKm, MaxFuelUsageLiterPerHour, MaxFuelTakeoffEffortLiter) VALUES ('Boeing 737-800', 189, 12.9,	11250, 2650);
INSERT INTO [FlightManager].[dbo].[AircraftTypes] (Description, PassangerCapacity, MaxFuelUsageLiterPerKm, MaxFuelUsageLiterPerHour, MaxFuelTakeoffEffortLiter) VALUES ('Airbus A320neo', 194, 9.4, 8250, 2460);
INSERT INTO [FlightManager].[dbo].[AircraftTypes] (Description, PassangerCapacity, MaxFuelUsageLiterPerKm, MaxFuelUsageLiterPerHour, MaxFuelTakeoffEffortLiter) VALUES ('Boeing 787-9', 296	,25.9, 22000, 5300);
INSERT INTO [FlightManager].[dbo].[AircraftTypes] (Description, PassangerCapacity, MaxFuelUsageLiterPerKm, MaxFuelUsageLiterPerHour, MaxFuelTakeoffEffortLiter) VALUES ('Airbus A380', 853,	23.5, 20000, 11400);
INSERT INTO [FlightManager].[dbo].[AircraftTypes] (Description, PassangerCapacity, MaxFuelUsageLiterPerKm, MaxFuelUsageLiterPerHour, MaxFuelTakeoffEffortLiter) VALUES ('Bombardier Dash 8-Q400', 90, 8.0, 4000, 1330);