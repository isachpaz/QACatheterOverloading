
using EvilDICOM.Core;
using QACatheterOverloadingLib.Base;
using QACatheterOverloadingLib.Interfaces;

namespace QACatheterOverloadingLib.CatheterOverload
{
    internal class NullCatheterOverloader : CatheterOverloaderBase
    {
        public NullCatheterOverloader(ICatheterOverloadingParameters configuration, IDicomServices dicomServices, ICatheterBlockService catheterBlockService) : base(configuration, dicomServices, catheterBlockService)
        {
        }

        protected override void CopyPrivateTags(DICOMObject patientPlan, DICOMObject verificationPlan)
        {
            throw new System.NotImplementedException();
        }
    }
}