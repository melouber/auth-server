﻿using Fingo.Auth.Domain.Models.ProjectModels;

namespace Fingo.Auth.Domain.Projects.Interfaces
{
    public interface IAddProject
    {
        void Invoke(ProjectModel project);
    }
}