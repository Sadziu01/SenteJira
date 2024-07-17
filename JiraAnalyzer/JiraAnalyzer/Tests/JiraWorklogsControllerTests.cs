using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JiraAnalyzer.Controllers;
using JiraAnalyzer.Data;
using JiraAnalyzer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace JiraAnalyzer.Tests
{
    public class JiraWorklogsControllerTests
    {
        private readonly DbContextOptions<DataContext> _options;

        public JiraWorklogsControllerTests()
        {
            // Ustawienia dla DbContext w pamięci
            _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            // Dodaj przykładowe dane do testów
            using (var context = new DataContext(_options))
            {
                context.JiraWorklogs.AddRange(new List<JiraWorklog>
                {
                    new JiraWorklog
                    {
                        Ref = 1,
                        WorklogDate = DateTime.Now.AddDays(-5),
                        Author = "John",
                        Qualification = "RP",
                        TimeSpent = 5,
                        Components = "Comp1",
                        Descript = "Description for John",
                        Issue = "ISSUE-1",
                        IssueSummary = "Summary for ISSUE-1",
                        Project = "ProjectX"
                    },
                    new JiraWorklog
                    {
                        Ref = 2,
                        WorklogDate = DateTime.Now.AddDays(-5),
                        Author = "Jane",
                        Qualification = "R",
                        TimeSpent = 8,
                        Components = "Comp2",
                        Descript = "Description for Jane",
                        Issue = "ISSUE-2",
                        IssueSummary = "Summary for ISSUE-2",
                        Project = "ProjectY"
                    },
                    new JiraWorklog
                    {
                        Ref = 3,
                        WorklogDate = DateTime.Now.AddDays(-10),
                        Author = "John",
                        Qualification = "HD",
                        TimeSpent = 3,
                        Components = "Comp3",
                        Descript = "Description for John HD",
                        Issue = "ISSUE-3",
                        IssueSummary = "Summary for ISSUE-3",
                        Project = "ProjectZ"
                    },
                    new JiraWorklog
                    {
                        Ref = 4,
                        WorklogDate = DateTime.Now.AddDays(-3),
                        Author = "Alice",
                        Qualification = "SZ",
                        TimeSpent = 10,
                        Components = "Comp4",
                        Descript = "Description for Alice",
                        Issue = "ISSUE-4",
                        IssueSummary = "Summary for ISSUE-4",
                        Project = "ProjectX"
                    }
                });
                context.SaveChanges();
            }
        }


        [Fact]
        public async Task GetWorklogs_Returns_BadRequestResult_For_Invalid_Period()
        {
            // Arrange
            using (var context = new DataContext(_options))
            {
                var controller = new JiraWorklogsController(context);

                // Act
                var result = await controller.GetWorklogs("invalidPeriod", null);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal("Nieprawidłowy okres", badRequestResult.Value);
            }
        }

        [Fact]
        public async Task GetWorklogs_Returns_NotFoundResult_When_No_Worklogs()
        {
            // Arrange
            using (var context = new DataContext(_options))
            {
                var controller = new JiraWorklogsController(context);

                // Act
                var result = await controller.GetWorklogs("year", "NonExistingAuthor");

                // Assert
                var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
                Assert.Equal("Brak danych dla podanego okresu i autora.", notFoundResult.Value);
            }
        }

        [Fact]
        public async Task GetAuthors_Returns_OkResult()
        {
            // Arrange
            using (var context = new DataContext(_options))
            {
                var controller = new JiraWorklogsController(context);

                // Act
                var result = await controller.GetAuthors();

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var authors = Assert.IsAssignableFrom<IEnumerable<string>>(okResult.Value);
                Assert.Equal(3, authors.Count());
            }
        }
    }
}
