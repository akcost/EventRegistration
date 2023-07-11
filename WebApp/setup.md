
# Database setup

### Create migration
dotnet ef migrations add InitialCreate --project DAL --startup-project WebApp
### Apply migration
dotnet ef database update --project DAL --startup-project WebApp
### Drop database
dotnet ef database drop --project DAL --startup-project WebApp

### Scaffolding
dotnet aspnet-codegenerator razorpage -m Person -outDir Pages/Persons -dc AppDbContext -udl --referenceScriptLibraries

dotnet aspnet-codegenerator razorpage -m Company -outDir Pages/Companies -dc AppDbContext -udl --referenceScriptLibraries

dotnet aspnet-codegenerator razorpage -m Participator -outDir Pages/Participators -dc AppDbContext -udl --referenceScriptLibraries

dotnet aspnet-codegenerator razorpage -m EventInfo -outDir Pages/EventInfos -dc AppDbContext -udl --referenceScriptLibraries

dotnet aspnet-codegenerator razorpage -m EventParticipator -outDir Pages/EventParticipators -dc AppDbContext -udl --referenceScriptLibraries