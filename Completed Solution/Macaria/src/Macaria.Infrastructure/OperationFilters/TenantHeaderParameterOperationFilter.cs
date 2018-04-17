﻿using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace Macaria.Infrastructure.OperationFilters
{
    public class TenantHeaderParameterOperationFilter: IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<IParameter>();

            operation.Parameters.Add(new NonBodyParameter
            {
                Name = "TenantId",
                In = "header",
                Description = "Tenant",
                Required = true,
                Type = "string"
            });
        }
    }
}
