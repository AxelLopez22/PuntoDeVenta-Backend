using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POS.Application.Dtos.Request;
using POS.Application.Interfaces;
using POS.Utilities.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Tests.Category
{
    [TestClass]
    public class CategoryApplicationTests
    {
        private static WebApplicationFactory<Program>? _factory = null;
        private static IServiceScopeFactory? _scopeFactory = null;

        [ClassInitialize]
        public static void Initialize(TestContext _test)
        {
            _factory = new CustomWebApplicationFactory();
            _scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
        }

        [TestMethod]
        public async Task RegisterCategory_WhenSendingNullValuesOrEmpty_ValidatorErrors()
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope?.ServiceProvider.GetService<ICategoryApplication>();

            var name = "";
            var description = "";
            var state = 1;
            var expected = ReplyMessage.MESSAGE_VALIDATION;

            var result = await context.RegisterCategory(new CategoryRequestDTO()
            {
                Name = name,
                Description = description,
                State = state
            });
            var current = result.Message;

            Assert.AreEqual(expected, current);
        }
    }
}
