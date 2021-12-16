using LibraryNetCoreAPI.Validaciones;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryNetCoreAPI.Tests.PruebasUnitarias
{
    [TestClass]
    public class PrimeraLetraMayusculaAttributeTests
    {
        [TestMethod]
        public void PrimeraLetraMinuscula_devuelveError()
        {
            //preparacion -> Aquí se instancia cualquier objeto o clase que necesite para la fase de pruebas
            var primeraLetraMayuscula = new PrimeraLetraMayusculaAttribute();
            var valor = "gerson";
            var validationContext = new ValidationContext(new { Nombre = valor });

            //ejecución de prueba
            var resultado = primeraLetraMayuscula.GetValidationResult(valor, validationContext);

            //verificación
            Assert.AreEqual("La primera letra debe ser mayúscula", resultado.ErrorMessage);

        }

        [TestMethod]
        public void ValorNulo_NoDevuelveError()
        {
            //preparacion -> Aquí se instancia cualquier objeto o clase que necesite para la fase de pruebas
            var primeraLetraMayuscula = new PrimeraLetraMayusculaAttribute();
            string valor = null;
            var validationContext = new ValidationContext(new { Nombre = valor });

            //ejecución de prueba
            var resultado = primeraLetraMayuscula.GetValidationResult(valor, validationContext);

            //verificación
            Assert.IsNull(resultado);//cuando no hay errores resultado devuelve null
        }

        [TestMethod]
        public void PrimeraLetraMayuscula_NoDevuelveError()
        {
            //preparacion -> Aquí se instancia cualquier objeto o clase que necesite para la fase de pruebas
            var primeraLetraMayuscula = new PrimeraLetraMayusculaAttribute();
            var valor = "Gerson";
            var validationContext = new ValidationContext(new { Nombre = valor });

            //ejecución de prueba
            var resultado = primeraLetraMayuscula.GetValidationResult(valor, validationContext);

            //verificación
            Assert.IsNull(resultado);
        }
    }
}
