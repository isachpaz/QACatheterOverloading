using EvilDICOM.Core;
using EvilDICOM.Core.Element;
using QACatheterOverloadingLib.Base;
using QACatheterOverloadingLib.Interfaces;

namespace QACatheterOverloadingLib.CatheterOverload
{
    internal class OncentraBrachyCatheterOverloader : CatheterOverloaderBase
    {
        protected override void CopyPrivateTags(DICOMObject patientPlan, DICOMObject verificationPlan)
        {
            var prescriptionDose = patientPlan.FindFirst(new Tag("3007", "1000"));
            var prescriptionDoseUnit = patientPlan.FindFirst(new Tag("3007", "1002"));

            if (prescriptionDoseUnit != null) verificationPlan.Replace(prescriptionDoseUnit);
            if (prescriptionDose != null) verificationPlan.Replace(prescriptionDose);
            CopyPrivateSequence_3007_100B(patientPlan, verificationPlan);
            
        }

        private void CopyPrivateSequence_3007_100B(DICOMObject patientPlan, DICOMObject verificationPlan)
        {
            var unknowneffect = patientPlan.FindFirst(new Tag("3007", "100B"));
            if (unknowneffect != null) verificationPlan.Replace(unknowneffect);
        }

        protected override bool IsPrivateTagsNeededToBeTransfered()
        {
            return true;
        }


        public OncentraBrachyCatheterOverloader(ICatheterOverloadingParameters configuration, IDicomServices dicomServices, ICatheterBlockService catheterBlockService) : base(configuration, dicomServices, catheterBlockService)
        {
        }
    }
}