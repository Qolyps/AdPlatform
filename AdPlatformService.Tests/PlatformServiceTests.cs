using Microsoft.VisualStudio.TestTools.UnitTesting;
using AdPlatformService.Models;
using AdPlatformService.Services;
using System.Collections.Generic;

namespace AdPlatformService.Tests.Services
{
    [TestClass]
    public class PlatformServiceTests
    {
        [TestMethod]
        public void GetPlatformsByLocation_ShouldReturnCorrectPlatforms()
        {
            // Arrange
            var service = new PlatformService();

            var platforms = new List<Platform>
            {
                new Platform("Яндекс.Директ", new[] { "/ru" }),
                new Platform("Ревдинский рабочий", new[] { "/ru/svrd/revda", "/ru/svrd/pervik" }),
                new Platform("Крутая реклама", new[] { "/ru/svrd" }),
            };

            service.LoadPlatforms(platforms);

            // Act
            var result = service.GetPlatformsByLocation("/ru/svrd/revda");

            // Assert
            CollectionAssert.AreEquivalent(
                new List<string> { "Яндекс.Директ", "Крутая реклама", "Ревдинский рабочий" },
                new List<string>(result)
            );
        }

        [TestMethod]
        public void GetPlatformsByLocation_NoMatch_ReturnsEmptyList()
        {
            // Arrange
            var service = new PlatformService();
            service.LoadPlatforms(new List<Platform>
            {
                new Platform("Газета", new[] { "/ru/msk" })
            });

            // Act
            var result = service.GetPlatformsByLocation("/us/ny");

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void LoadPlatforms_ShouldReplaceOldData()
        {
            // Arrange
            var service = new PlatformService();

            service.LoadPlatforms(new List<Platform>
            {
                new Platform("Первая", new[] { "/ru" })
            });

            // Act
            service.LoadPlatforms(new List<Platform>
            {
                new Platform("Вторая", new[] { "/ru/msk" })
            });

            var result = service.GetPlatformsByLocation("/ru");

            // Assert
            CollectionAssert.DoesNotContain(result, "Первая");
            CollectionAssert.Contains(result, "Вторая");
        }
    }
}
