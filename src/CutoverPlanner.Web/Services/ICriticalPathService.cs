using CutoverPlanner.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CutoverPlanner.Web.Services
{
    public interface ICriticalPathService
    {
        Task<(IReadOnlyList<Atividade> path, TimeSpan totalDur)> GetCriticalPathAsync();
    }
}