// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Autofac;
using Microsoft.Extensions.DependencyInjection;
using NuGetGallery.Services.Helpers;

namespace NuGetGallery.Services
{
    internal class DefaultDependenciesModule: Module
    {
        public void ConfigureServices (IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextHelper, HttpContextHelper>();
        }
    }
}
