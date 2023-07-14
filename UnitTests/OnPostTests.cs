using WebApp.Pages.EventInfos;
using WebApp.Pages.Participators;
using DAL;
using Domain;
using Microsoft.EntityFrameworkCore;
using CreateModel = WebApp.Pages.EventInfos.CreateModel;
using DeleteModel = WebApp.Pages.EventInfos.DeleteModel;
using EditModel = WebApp.Pages.Participators.EditModel;

namespace UnitTests
{
    public class OnPostTests
    {
        [Fact]
        public async Task OnPostAsync_CreatesEvent()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase"); // Provide a unique name for the in-memory database

            var context = new AppDbContext(optionsBuilder.Options);
            await context.Database.EnsureCreatedAsync();

            var createModel = new CreateModel(context);
            var expectedEventInfo = new EventInfo
            {
                EventName = "Test Event",
                EventLocation = "Test Location",
                EventDateTime = new DateTime(2023, 8, 19, 10, 0, 0),
                EventDescription = "Test Description"
            };
            createModel.EventInfo = expectedEventInfo;

            // Act
            var result = await createModel.OnPostAsync();

            // Assert
            var eventCount = await context.EventInfos
                .CountAsync();
            Assert.Equal(1, eventCount);

            var actualEventInfo = await context.EventInfos
                .FirstOrDefaultAsync();

            Assert.NotNull(actualEventInfo);
            Assert.Equal(expectedEventInfo.EventName, actualEventInfo.EventName);
            Assert.Equal(expectedEventInfo.EventLocation, actualEventInfo.EventLocation);
            Assert.Equal(expectedEventInfo.EventDateTime, actualEventInfo.EventDateTime);
            Assert.Equal(expectedEventInfo.EventDescription, actualEventInfo.EventDescription);
            // Reset db
            await context.Database.EnsureDeletedAsync();
        }

        [Fact]
        public async Task OnPostAsync_UpdatesParticipatorInfo()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new AppDbContext(options))
            {
                // Initialize the in-memory database with sample data
                var person = new Person
                {
                    Id = 1,
                    PersonFirstName = "John",
                    PersonLastName = "Doe",
                    PersonIdCode = "52221120833"
                };

                var participator = new Participator
                {
                    Id = 1,
                    Person = person,
                    PaymentType = PaymentType.Cash
                };

                context.Persons.Add(person);
                context.Participators.Add(participator);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var editModel = new EditModel(context);

                
                var existingParticipatorInfo = await context.Participators
                    .Include(p => p.Person)
                    .FirstOrDefaultAsync(p => p.Id == 1);

                existingParticipatorInfo!.PaymentType = PaymentType.BankTransfer;
                existingParticipatorInfo.Person!.PersonFirstName = "Max";
                existingParticipatorInfo.Person.PersonLastName = "Smith";
                existingParticipatorInfo.Person.PersonIdCode = "40981683321";

                context.Entry(existingParticipatorInfo).State = EntityState.Detached;

                editModel.Participator = existingParticipatorInfo;

                // Act
                var result = await editModel.OnPostAsync(null);

                // Assert
                var updatedParticipator = await context.Participators.FindAsync(1);
                Assert.NotNull(updatedParticipator);
                Assert.Equal(PaymentType.BankTransfer, updatedParticipator.PaymentType);
                Assert.NotNull(updatedParticipator.Person);
                Assert.Equal("Max", updatedParticipator.Person.PersonFirstName);
                Assert.Equal("Smith", updatedParticipator.Person.PersonLastName);
                Assert.Equal("40981683321", updatedParticipator.Person.PersonIdCode);
                // Reset db
                await context.Database.EnsureDeletedAsync();
            }
        }
        
        [Fact]
        public async Task OnPostAsync_UpdatesEventInfo()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new AppDbContext(options))
            {
                // Initialize the in-memory database with sample data
                context.EventInfos.Add(new EventInfo
                {
                    Id = 10,
                    EventName = "Test Event",
                    EventLocation = "Test Location",
                    EventDateTime = new DateTime(2023, 10, 12, 10, 0, 0),
                    EventDescription = "Test Description"
                });
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var editModel = new WebApp.Pages.EventInfos.EditModel(context);

                // Get the existing event info
                var existingEventInfo = await context.EventInfos.FindAsync(10);

                // Modify the event info
                existingEventInfo!.EventName = "Updated Event Name";
                existingEventInfo.EventLocation = "Updated Event Location";
                existingEventInfo.EventDateTime = new DateTime(2023, 11, 22, 0, 0, 0);

                // Detach the existing event info from the context
                context.Entry(existingEventInfo).State = EntityState.Detached;

                // Set the modified event info
                editModel.EventInfo = existingEventInfo;

                // Act
                var result = await editModel.OnPostAsync();

                // Assert
                var updatedEventInfo = await context.EventInfos.FindAsync(10); // Event ID
                Assert.NotNull(updatedEventInfo);
                Assert.Equal("Updated Event Name", updatedEventInfo.EventName);
                Assert.Equal("Updated Event Location", updatedEventInfo.EventLocation);
                Assert.Equal(new DateTime(2023, 11, 22, 0, 0, 0), updatedEventInfo.EventDateTime);
                
                // Reset db
                await context.Database.EnsureDeletedAsync();
            }
        }


        [Fact]
        public async Task OnPostAsync_AddsPersonToEvent()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            await using (var context = new AppDbContext(options))
            {
                // Initialize the in-memory database with sample data
                context.EventInfos.Add(new EventInfo
                {
                    Id = 1,
                    EventName = "Test Event",
                    EventLocation = "Test Location",
                    EventDateTime = DateTime.Now.AddDays(1),
                    EventDescription = "Test Description"
                });
                await context.SaveChangesAsync();
            }

            await using (var context = new AppDbContext(options))
            {
                var addParticipatorModel = new AddParticipator(context);

                addParticipatorModel.Person = new Person
                {
                    PersonFirstName = "John",
                    PersonLastName = "Doe",
                    PersonIdCode = "50003320844"
                };

                // Act
                var result = await addParticipatorModel.OnPostAsync(1); // Event ID

                // Assert
                var participatorCount = await context.EventParticipators
                    .CountAsync(ep => ep.EventInfoId == 1); // Event ID
                
                Assert.Equal(1, participatorCount);
                await context.Database.EnsureDeletedAsync();
            }
        }

        [Fact]
        public async Task OnPostAsync_AddsCompanyToEvent()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            await using (var context = new AppDbContext(options))
            {
                // Initialize the in-memory database with sample data
                context.EventInfos.Add(new EventInfo
                {
                    Id = 1,
                    EventName = "Test Event 2",
                    EventLocation = "Test Location 2",
                    EventDateTime = DateTime.Now.AddDays(1),
                    EventDescription = "Test Description"
                });
                await context.SaveChangesAsync();
            }

            await using (var context = new AppDbContext(options))
            {
                var addParticipatorModel = new AddParticipator(context);

                addParticipatorModel.Company = new Company
                {
                    CompanyName = "Coca Cola",
                    CompanyMemberCount = 33,
                    CompanyRegistryCode = "44637281"
                };

                // Act
                var result = await addParticipatorModel.OnPostAsync(1);

                // Assert
                var participatorCount = await context.EventParticipators
                    .CountAsync(ep => ep.EventInfoId == 1);
                Assert.Equal(1, participatorCount);
                // Reset db
                await context.Database.EnsureDeletedAsync();
            }
        }
        
        
        
        
        [Fact]
        public async Task OnPostAsync_DeletesEventInfoAndAssociatedEntities()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            await using (var context = new AppDbContext(options))
            {
                // Initialize the in-memory database with sample data
                var eventInfo = new EventInfo
                {
                    Id = 1, // Event ID
                    EventName = "Test Event",
                    EventLocation = "Test Location",
                    EventDateTime = new DateTime(2023, 9, 12, 10, 0, 0),
                    EventDescription = "Test Description"
                };

                var person = new Person
                {
                    Id = 1,
                    PersonFirstName = "John",
                    PersonLastName = "Doe",
                    PersonIdCode = "50003320844"
                };

                var company = new Company
                {
                    Id = 2,
                    CompanyName = "Test Company",
                    CompanyRegistryCode = "12345678"
                };

                var personParticipator = new Participator
                {
                    Id = 1,
                    Person = person,
                    PaymentType = PaymentType.BankTransfer
                };

                var companyParticipator = new Participator
                {
                    Id = 2,
                    Company = company,
                    PaymentType = PaymentType.Cash
                };

                var personEventParticipator = new EventParticipator
                {
                    EventInfo = eventInfo,
                    Participator = personParticipator
                };

                var companyEventParticipator = new EventParticipator
                {
                    EventInfo = eventInfo,
                    Participator = companyParticipator
                };

                context.EventInfos.Add(eventInfo);
                context.Persons.Add(person);
                context.Companies.Add(company);
                context.Participators.AddRange(personParticipator, companyParticipator);
                context.EventParticipators.AddRange(personEventParticipator, companyEventParticipator);

                await context.SaveChangesAsync();
            }

            await using (var context = new AppDbContext(options))
            {
                var deleteModel = new DeleteModel(context);

                // Act
                var result = await deleteModel.OnPostAsync(1); // Event ID

                // Assert
                var eventInfo = await context.EventInfos.FindAsync(1);
                var personParticipator = await context.Participators.FindAsync(1);
                var companyParticipator = await context.Participators.FindAsync(2);
                var person = await context.Persons.FindAsync(1);
                var company = await context.Companies.FindAsync(2);
                var personEventParticipator = await context.EventParticipators.FindAsync(1);
                var companyEventParticipator = await context.EventParticipators.FindAsync(2);

                Assert.Null(eventInfo);
                Assert.Null(personParticipator);
                Assert.Null(companyParticipator);
                Assert.Null(person);
                Assert.Null(company);
                Assert.Null(personEventParticipator);
                Assert.Null(companyEventParticipator);
                // Reset db
                await context.Database.EnsureDeletedAsync();
            }
        }
    }
}