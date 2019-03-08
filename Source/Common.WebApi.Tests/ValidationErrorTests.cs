using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using Common.WebApi;
using Newtonsoft.Json;


namespace Common.WebApi.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ValidationErrorTests
    {
        [TestMethod]
        [TestCategory("UnitTest"), TestCategory("Framework")]
        [Description("Une ValidationError simple comporte un message 'Erreur de validation' en JSON")]
        public void ValidationError_As_Json()
        {
            string expectedMessage = "{\"Message\":\"Une erreur de validation de la demande s'est produite.\",\"Errors\":[]}";

            var systemUnderTest = new ValidationError();

            Assert.AreEqual(expectedMessage, JsonConvert.SerializeObject(systemUnderTest));
        }

        [TestMethod]
        [TestCategory("UnitTest"), TestCategory("Framework")]
        [Description("Une ValidationError avec des erreurs multiples comporte une collection errors en JSON")]
        public void ValidationError_With_Multiple_Errors_As_Json()
        {
            string expectedMessage = "{\"Message\":\"Une erreur de validation de la demande s'est produite.\",\"Errors\":[\"Email invalide\",\"Adresse obligatoire\"]}";

            var systemUnderTest = new ValidationError(new[] { "Email invalide", "Adresse obligatoire" });

            Assert.AreEqual(expectedMessage, JsonConvert.SerializeObject(systemUnderTest));
        }

    }

}
