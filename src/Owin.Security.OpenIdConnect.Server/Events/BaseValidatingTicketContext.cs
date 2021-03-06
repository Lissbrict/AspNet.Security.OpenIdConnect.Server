/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OpenIdConnect.Server
 * for more information concerning the license and the contributors participating to this project.
 */

using System;
using System.Security.Claims;
using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace Owin.Security.OpenIdConnect.Server
{
    /// <summary>
    /// Base class used for certain event contexts
    /// </summary>
    public abstract class BaseValidatingTicketContext : BaseValidatingContext
    {
        /// <summary>
        /// Initializes base class used for certain event contexts
        /// </summary>
        protected BaseValidatingTicketContext(
            IOwinContext context,
            OpenIdConnectServerOptions options,
            OpenIdConnectRequest request,
            AuthenticationTicket ticket)
            : base(context, options, request)
        {
            Ticket = ticket;
        }

        /// <summary>
        /// Contains the identity and properties for the application to authenticate. If the Validated method
        /// is invoked with an AuthenticationTicket or ClaimsIdentity argument, that new value is assigned to
        /// this property in addition to changing IsValidated to true.
        /// </summary>
        public AuthenticationTicket Ticket { get; private set; }

        /// <summary>
        /// Replaces the ticket information on this context and marks it as as validated by the application.
        /// IsValidated becomes true and HasError becomes false as a result of calling.
        /// </summary>
        /// <param name="ticket">Assigned to the Ticket property</param>
        public void Validate(AuthenticationTicket ticket)
        {
            if (ticket == null)
            {
                throw new ArgumentNullException(nameof(ticket));
            }

            Ticket = ticket;
            base.Validate();
        }

        /// <summary>
        /// Alters the ticket information on this context and marks it as as validated by the application.
        /// IsValidated becomes true and HasError becomes false as a result of calling.
        /// </summary>
        /// <param name="identity">Assigned to the Ticket.Identity property</param>
        public void Validate(ClaimsIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            var properties = Ticket?.Properties ?? new AuthenticationProperties();
            Validate(new AuthenticationTicket(identity, properties));
        }
    }
}
