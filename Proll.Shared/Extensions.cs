using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;

namespace Proll.Shared
{
    public static class Extensions
    {
        public static int GetUserId(this ClaimsPrincipal principal) =>
            Convert.ToInt32(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
    }
}
