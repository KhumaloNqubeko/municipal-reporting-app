using System.Collections.Generic;
using MunicipalCitizenReporting.Part3.Models;

namespace MunicipalCitizenReporting.Part3.Adapters
{
    public interface IRequestReadAdapter { IList<ServiceRequestViewModel> ReadSnapshot(); }
}
