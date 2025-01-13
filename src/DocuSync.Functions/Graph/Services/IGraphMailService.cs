using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocuSync.Functions.Graph.Services
{
    public interface IGraphMailService
    {
        Task ProcessNewMailAsync(ChangeType? changeType, Message message);
        Task SetupSubscriptionAsync();
    }
}
