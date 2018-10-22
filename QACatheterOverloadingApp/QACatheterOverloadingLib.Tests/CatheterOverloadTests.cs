using EvilDICOM.Core;
using Moq;
using NUnit.Framework;
using QACatheterOverloadingLib.CatheterOverload;
using QACatheterOverloadingLib.CatheterPartition;
using QACatheterOverloadingLib.Dicom;
using QACatheterOverloadingLib.Enum;
using QACatheterOverloadingLib.Interfaces;
using QACatheterOverloadingLib.Services;

namespace QACatheterOverloadingLib.Tests
{
    [TestFixture]
    public class CatheterOverloadTests
    {
        [Test]
        public void OncentraBrachy_ctor_Test()
        {
            var mockCatheterOverloadingParameters = new Mock<ICatheterOverloadingParameters>();
            mockCatheterOverloadingParameters.SetupGet(u => u.OutputPlanName).Returns("outputplan.dcm");
            mockCatheterOverloadingParameters.SetupGet(u => u.PatientPlan)
                .Returns(DICOMObject.Read(DicomResources.rtplan));
            mockCatheterOverloadingParameters.SetupGet(u => u.VerificationPlan)
                .Returns(DICOMObject.Read(DicomResources.rtplan));
            mockCatheterOverloadingParameters.SetupGet(u => u.TpsType)
                .Returns(PlanningSystemType.ONCENTRA_BRACHY);
            
            IDicomServices dicomServices = new DicomServices();
            ICatheterBlocks catheterBlocks = new CatheterBlocks(8);
            ICatheterBlockService catheterBlockService = new CatheterBlockService(catheterBlocks);

            var catheterOverloader =
                    CatheterOverloaderFactory.Create(mockCatheterOverloadingParameters.Object, 
                                                     dicomServices, 
                                                     catheterBlockService);

            catheterOverloader.RunOverloading();
            catheterOverloader.SaveToFile("outputFile.dcm");
        }

        
    }
}