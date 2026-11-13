using System;
using System.Collections.Generic;
using MunicipalCitizenReporting.Part3.DataStructures;
using MunicipalCitizenReporting.Part3.Models;

namespace MunicipalCitizenReporting.Part3.Services
{
    public sealed class RequestIndexService
    {
        private readonly AvlTree<string, ServiceRequestViewModel> index = new AvlTree<string, ServiceRequestViewModel>(StringComparer.Ordinal);
        public RequestIndexService(IEnumerable<ServiceRequestViewModel> requests)
        { foreach (var request in requests) if (!string.IsNullOrWhiteSpace(request.Reference)) index.Insert(request.Reference, request); }
        public ServiceRequestViewModel Find(string canonicalReference)
        { ServiceRequestViewModel request; return canonicalReference != null && index.TryFind(canonicalReference, out request) ? request : null; }
    }
}
