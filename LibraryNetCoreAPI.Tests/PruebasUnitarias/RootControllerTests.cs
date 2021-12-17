using LibraryNetCoreAPI.Controllers.v1;
using LibraryNetCoreAPI.Tests.Mocks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LibraryNetCoreAPI.Tests.PruebasUnitarias
{
    [TestClass]
    public class RootControllerTests
    {
        [TestMethod]
        public async Task SiUsuarioEsAdmin_Obtiene4Links()
        {
            //preparacion
            var authorizationService = new AuthorizationServiceMock();
            authorizationService.authorizationResult = AuthorizationResult.Success();

            var rootController = new RootController(authorizationService);
            rootController.Url = new URLHelperMock();

            //ejecución de la prueba
            var resultado = await rootController.Get();

            //verificación
            Assert.AreEqual(4, resultado.Value.Count());

        }

        [TestMethod]
        public async Task SiUsuarioNoEsAdmin_Obtiene3Links()
        {
            //preparacion
            var authorizationService = new AuthorizationServiceMock();
            authorizationService.authorizationResult = AuthorizationResult.Failed();

            var rootController = new RootController(authorizationService);
            rootController.Url = new URLHelperMock();

            //ejecución de la prueba
            var resultado = await rootController.Get();
            
            //verificación
            Assert.AreEqual(3, resultado.Value.Count());

        }

        [TestMethod]
        public async Task SiUsuarioNoEsAdmin_Obtiene3Links_usandoMoq()
        {
            //preparacion
            //var authorizationService = new AuthorizationServiceMock();
            //authorizationService.authorizationResult = AuthorizationResult.Failed();

            var mockAuthorizationService = new Mock<IAuthorizationService>();
            //para simular el método AuthorizeAsync
            mockAuthorizationService.Setup(x => x.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<object>(),
                It.IsAny<IEnumerable<IAuthorizationRequirement>>()
                )).Returns(Task.FromResult(AuthorizationResult.Failed()));

            //para simular el método AuthorizeAsync sobrecargado
            mockAuthorizationService.Setup(x => x.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(), 
                It.IsAny<object>(),
                It.IsAny<string>()
                )).Returns(Task.FromResult(AuthorizationResult.Failed()));

            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns(string.Empty);

            var rootController = new RootController(mockAuthorizationService.Object);
            rootController.Url = mockUrlHelper.Object;

            //ejecución de la prueba
            var resultado = await rootController.Get();

            //verificación
            Assert.AreEqual(3, resultado.Value.Count());

        }
    }
}
