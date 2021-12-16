using LibraryNetCoreAPI.Controllers.v1;
using LibraryNetCoreAPI.Tests.Mocks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task SiUsuarioNoEsAdmin_Obtiene1Link()
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
    }
}
