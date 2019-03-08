using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;


namespace Common.Monitoring.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ConfigurationHelperTests
    {
        [TestMethod]
        [TestCategory("UnitTest"), TestCategory("Framework")]
        [Description("GetOptionalApplicationSetting<bool> retourne la valeur false par défaut si aucune clé correspondante")]
        public void GetOptionalApplicationSetting_Bool_Default_False_Not_Present_Returns_Default()
        {
            bool expectedResult = false;
            Assert.AreEqual(expectedResult, ConfigurationHelper.GetOptionalApplicationSetting<bool>("NotFound") );
        }

        [TestMethod]
        [TestCategory("UnitTest"), TestCategory("Framework")]
        [Description("GetOptionalApplicationSetting<bool> retourne la valeur true par défaut si aucune clé correspondante")]
        public void GetOptionalApplicationSetting_Bool_Default_True_Not_Present_Returns_Default()
        {
            bool expectedResult = true;
            Assert.AreEqual(expectedResult, ConfigurationHelper.GetOptionalApplicationSetting<bool>("NotFound", true));
        }

    }

}
