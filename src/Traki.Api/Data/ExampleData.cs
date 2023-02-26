﻿using Traki.Api.Models.Project;

namespace Traki.Api.Data
{
    public static class ExampleData
    {
        public static IEnumerable<Project> Projects => new[]
        {
            new Project {
            Name = $"Test-Project-A"
            },
            new Project {
            Name = $"Test-Project-B"
            }
        };
    }
}