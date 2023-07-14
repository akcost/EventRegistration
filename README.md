# EventRegistration

### A website where you can
* Create an event
* Add participators(people/companies) to the event
* View all the future events
* View all the past events
* See all the participators for the events
* Change future events info
* Change participator info

## Database setup


1. Go to `WebApp` project
2. Open `appsettings.json`
3. Edit DefaultConnection with your own path to where you want .db file to be stored at
4. Do the same for `AppDbContextFactory.cs` in `DAL`project.
5. Run these commands below

#### Create migration
dotnet ef migrations add InitialCreate --project DAL --startup-project WebApp
#### Apply migration
dotnet ef database update --project DAL --startup-project WebApp

## Project structure

### Domain
Holds all the database entities.

### DAL
Handles the database setup

### UnitTests
Tests out different methods like Event creation, adding participator to an event etc.

### WebApp
Handles everything website related.

