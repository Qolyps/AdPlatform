using Microsoft.VisualStudio.TestTools.UnitTesting;
using AdPlatformSelector.Services;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdPlatformSelector.Tests
{
    [TestClass]
    public class AdPlatformServiceTests
    {
        private IFormFile CreateTestFile(string content)
        {
            var bytes = Encoding.UTF8.GetBytes(content);
            var stream = new MemoryStream(bytes);
            return new FormFile(stream, 0, bytes.Length, "file", "test.txt");
        }

        [TestMethod]
        public async Task LoadFromFileAsync_Should_Parse_Correctly()
        {
            var content = "Яндекс.Директ:/ru\nКрутая реклама:/ru/svrd";
            var file = CreateTestFile(content);
            var service = new AdPlatformService();

            await service.LoadFromFileAsync(file);
            var result = service.FindPlatformsByLocation("/ru/svrd/revda");

            CollectionAssert.Contains(result, "Яндекс.Директ");
            CollectionAssert.Contains(result, "Крутая реклама");
        }

        [TestMethod]
        public async Task FindPlatformsByLocation_Should_Return_Correct_Results()
        {
            var content = @"
                Газета уральских москвичей:/ru/msk,/ru/permobl
                Яндекс.Директ:/ru
                Ревдинский рабочий:/ru/svrd/revda,/ru/svrd/pervik
                Крутая реклама:/ru/svrd
                ";
            var file = CreateTestFile(content);
            var service = new AdPlatformService();

            await service.LoadFromFileAsync(file); 
            var result = service.FindPlatformsByLocation("/ru/svrd/revda");

            CollectionAssert.Contains(result, "Яндекс.Директ");
            CollectionAssert.Contains(result, "Крутая реклама");
            CollectionAssert.Contains(result, "Ревдинский рабочий");
            CollectionAssert.DoesNotContain(result, "Газета уральских москвичей");
        }

        [TestMethod]
        public async Task FindPlatformsByLocation_Should_Be_Case_Insensitive()
        {
            var content = "Яндекс.Директ:/Ru";
            var file = CreateTestFile(content);
            var service = new AdPlatformService();

            await service.LoadFromFileAsync(file);
            var result = service.FindPlatformsByLocation("/ru/msk");

            CollectionAssert.Contains(result, "Яндекс.Директ");
        }

        [TestMethod]
        public async Task FindPlatformsByLocation_Should_Return_Empty_If_Nothing_Found()
        {
            var content = "Some Platform:/ru/svrd";
            var file = CreateTestFile(content);
            var service = new AdPlatformService();

            await service.LoadFromFileAsync(file);
            var result = service.FindPlatformsByLocation("/kz/astana");

            Assert.AreEqual(0, result.Count);
        }
    }
}
