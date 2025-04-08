using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using YourNamespace.Services; // замени на свое пространство имён

namespace AdPlatformService.Tests
{
    [TestClass]
    public class AdPlatformServiceTests
    {
        private AdPlatformService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new AdPlatformService();

            _service.LoadTestData(new List<(string, List<string>)>
            {
                ("Яндекс.Директ", new List<string> { "/ru" }),
                ("Ревдинский рабочий", new List<string> { "/ru/svrd/revda", "/ru/svrd/pervik" }),
                ("Газета уральских москвичей", new List<string> { "/ru/msk", "/ru/permobl", "/ru/chelobl" }),
                ("Крутая реклама", new List<string> { "/ru/svrd" })
            });
        }

        [TestMethod]
        public void ShouldReturnCorrectPlatforms_For_ru_svrd_revda()
        {
            var result = _service.FindPlatformsByLocation("/ru/svrd/revda");

            CollectionAssert.AreEquivalent(
                new List<string> { "Яндекс.Директ", "Ревдинский рабочий", "Крутая реклама" },
                result
            );
        }

        [TestMethod]
        public void ShouldReturnOnlyYandex_For_ru()
        {
            var result = _service.FindPlatformsByLocation("/ru");
            CollectionAssert.AreEqual(new List<string> { "Яндекс.Директ" }, result);
        }

        [TestMethod]
        public void ShouldReturnEmptyList_ForUnknownLocation()
        {
            var result = _service.FindPlatformsByLocation("/us/ny");
            Assert.AreEqual(0, result.Count);
        }
    }
}
