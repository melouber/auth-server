﻿using Fingo.Auth.Domain.Infrastructure.EventBus.Events.Base;

namespace Fingo.Auth.Domain.Infrastructure.EventBus.Events.Policy
{
    public class PolicySaved : EventBase
    {
        public PolicySaved(int projectId , DbAccess.Models.Policies.Enums.Policy policy)
        {
            Policy = policy;
            ProjectId = projectId;
        }

        public DbAccess.Models.Policies.Enums.Policy Policy { get; }
        public int ProjectId { get; }

        public override string ToString()
        {
            return $"Policy ({Policy}) saved in project (id: {ProjectId}).";
        }
    }
}