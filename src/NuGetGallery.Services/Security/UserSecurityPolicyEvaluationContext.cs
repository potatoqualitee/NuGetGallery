// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Services.Entities;
#if NETFRAMEWORK
using System.Web;
#else
using Microsoft.AspNetCore.Http;
using NuGetGallery.Services.Helpers;
#endif

namespace NuGetGallery.Security
{
    /// <summary>
    /// Context providing user security policy handlers with resources necessary for policy evaluation.
    /// </summary>
    public class UserSecurityPolicyEvaluationContext
    {
        private Lazy<HttpContext> _httpContext;
        private User _sourceAccount;
        private User _targetAccount;
#if !NETFRAMEWORK
        private IHttpContextHelper _httpContextHelper;

        public UserSecurityPolicyEvaluationContext(IHttpContextHelper httpContextHelper)
        {
            _httpContextHelper = httpContextHelper;                
        }
#endif

        /// <summary>
        /// Current http context. This has been required for some user security policies in order
        /// to get the current user and/or current request details.
        /// </summary>
        public HttpContext HttpContext
        {
            get
            {
                return _httpContext.Value;
            }
        }

        /// <summary>
        /// Current user.
        /// </summary>
        public User CurrentUser
        {
            get
            {
                return HttpContext.GetCurrentUser();
            }
        }

        /// <summary>
        /// Account where the security policy came from.
        /// </summary>
        public User SourceAccount
        {
            get
            {
                return _sourceAccount ?? CurrentUser;
            }
        }

        /// <summary>
        /// Account under policy evaluation.
        /// </summary>
        public User TargetAccount
        {
            get
            {
                return _targetAccount ?? CurrentUser;
            }
        }

        /// <summary>
        /// Security policies to be evaluated.
        /// </summary>
        public IEnumerable<UserSecurityPolicy> Policies { get; }

        /// <summary>
        /// Create a policy (user) context, which uses the httpContext.
        /// </summary>
        public UserSecurityPolicyEvaluationContext(
            IEnumerable<UserSecurityPolicy> policies,
            HttpContext httpContext)
        {
            Policies = policies ?? throw new ArgumentNullException(nameof(policies));

#if NETFRAMEWORK
            _httpContext = new Lazy<HttpContext>(() => httpContext
                ?? new HttpContextWrapper(System.Web.HttpContext.Current));
#else
            _httpContext = new Lazy<HttpContext>(() => httpContext
                ?? new HttpContextWrapper(System.Web.HttpContext.Current));
#endif
        }

        /// <summary>
        /// Create a policy (organization) context, which requires the source (organization) and target (member) accounts for context.
        /// </summary>
        public UserSecurityPolicyEvaluationContext(
            IEnumerable<UserSecurityPolicy> policies,
            User sourceAccount,
            User targetAccount,
            HttpContext httpContext = null)
            : this(policies, httpContext)
        {
            _sourceAccount = sourceAccount;
            _targetAccount = targetAccount;
        }
    }
}