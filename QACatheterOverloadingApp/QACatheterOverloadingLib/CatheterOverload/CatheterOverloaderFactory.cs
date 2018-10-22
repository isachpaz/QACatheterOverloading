
using System;
using QACatheterOverloadingLib.Enum;
using QACatheterOverloadingLib.Interfaces;

namespace QACatheterOverloadingLib.CatheterOverload
{
    public static class CatheterOverloaderFactory
    {
        public static ICatheterOverloaderBase Create(ICatheterOverloadingParameters configuration,
                                                    IDicomServices dicomServices, 
                                                    ICatheterBlockService catheterBlockService)
        {
            switch (configuration.TpsType)
            {
                case PlanningSystemType.ONCENTRA_BRACHY:
                    return new OncentraBrachyCatheterOverloader(configuration,dicomServices, catheterBlockService);
                case PlanningSystemType.ONCENTRA_PROSTATE:
                    throw new ArgumentException("Not implemented yet.");
                case PlanningSystemType.VARIAN_BRACHY:
                    throw new ArgumentException("Not implemented yet.");
            }
            return new NullCatheterOverloader(null, dicomServices, catheterBlockService);
        }
    }
}