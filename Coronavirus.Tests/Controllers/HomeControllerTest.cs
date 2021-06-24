using Coronavirus;
using Coronavirus.Controllers;
using Coronavirus.Helpers;
using Coronavirus.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Coronavirus.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public async Task Index_OnConstructing_ReturnsView()
        {
            // Arrange
            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mockControllerContext.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);

            var mockHttpClientHelper = new Mock<IHttpClientHelper>();
            var controller = new HomeController(mockHttpClientHelper.Object);
            controller.ControllerContext = mockControllerContext.Object;

            // Act
            var result = await controller.Index();

            // Assert
            Assert.IsNotNull(result);
            mockControllerContext.Verify();
        }

        [TestMethod]
        public void ExportToCsv_OnLoadingViewModel_ReturnsFileResult()
        {
            // Arrange
            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mockControllerContext.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);

            var mockHttpClientHelper = new Mock<IHttpClientHelper>();
            var controller = new HomeController(mockHttpClientHelper.Object);
            controller.ControllerContext = mockControllerContext.Object;

            // Act
            var result = controller.ExportToCsv();

            // Assert
            Assert.IsNotNull(result);
            mockControllerContext.Verify();
        }

        [TestMethod]
        public void ExportToJson_OnLoadingViewModel_ReturnsFileResult()
        {
            // Arrange  
            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mockControllerContext.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);

            var mockHttpClientHelper = new Mock<IHttpClientHelper>();
            var controller = new HomeController(mockHttpClientHelper.Object);
            controller.ControllerContext = mockControllerContext.Object;

            // Act
            var result = controller.ExportToJson();

            // Assert
            Assert.IsNotNull(result);
            mockControllerContext.Verify();
        }

        [TestMethod]
        public void ExportToXml_OnLoadingViewModel_ReturnsFileResult()
        {
            // Arrange
            var mockControllerContext = new Mock<ControllerContext>();
            var mockSession = new Mock<HttpSessionStateBase>();
            mockControllerContext.Setup(p => p.HttpContext.Session).Returns(mockSession.Object);

            var mockHttpClientHelper = new Mock<IHttpClientHelper>();
            var controller = new HomeController(mockHttpClientHelper.Object);
            controller.ControllerContext = mockControllerContext.Object;

            // Act
            var result = controller.ExportToXml();

            // Assert
            Assert.IsNotNull(result);
            mockControllerContext.Verify();
        }
    }
}
