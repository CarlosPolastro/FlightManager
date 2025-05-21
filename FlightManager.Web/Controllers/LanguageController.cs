using Microsoft.AspNetCore.Http;
using System;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

public class LanguageController : Controller
{
    [HttpPost]
    public IActionResult SetLanguage(string culture, string returnUrl = null)
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(
                new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
        );

        if (string.IsNullOrWhiteSpace(returnUrl))
            returnUrl = Request.Headers["Referer"].ToString();

        if (!Url.IsLocalUrl(returnUrl))
            return RedirectToAction("Index", "Flight");

        return LocalRedirect(returnUrl);
    }
}
