using DDA.Business.Interfaces;
using SSSFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace DDA.Web.Controllers
{
    [NoCache]
    public class BaseController : SSSBaseController
    {
        protected string manualLanguageToggleSelected = "{0}UserLanguageManualSelect";
        protected string loadedDefaultLanguage = "{0}UserDefaultLanguageLoaded";


        public ActionResult SwitchLanguage(string lang)
        {
            var languageExpression = new Regex(".*/(en|ar)/.*");
            var otherlanguage = (lang == "ar" ? "en" : "ar");
            var urlReferrer = HttpContext.Request.UrlReferrer;
            var responseUrl = string.Empty;

            if (urlReferrer != null)
            {
                var urlWithSlash = urlReferrer.ToString();
                if (!urlWithSlash.Contains("?"))
                {
                    var hasSlash = urlWithSlash.EndsWith("/");
                    urlWithSlash = (!hasSlash) ? urlWithSlash + "/" : urlWithSlash;
                }

                if (languageExpression.IsMatch(urlWithSlash))
                {
                    string url = urlWithSlash;
                    responseUrl = url.Replace(string.Format("/{0}/", otherlanguage), string.Format("/{0}/", lang));
                }
                else
                {
                    var insertLanguageIndex = urlReferrer.PathAndQuery.IndexOf("/", 1, System.StringComparison.Ordinal);
                    var localizedPathandQuery = urlReferrer.PathAndQuery.Insert(insertLanguageIndex, "/" + lang);
                    string url = string.Format("{0}://{1}{2}", urlReferrer.Scheme, urlReferrer.Authority, localizedPathandQuery);
                    responseUrl = url;
                }
            }
            else
            {
                //** if the current URL holds a culture name, do not redirect, just go to that path.
                if (languageExpression.IsMatch(Request.Url.AbsolutePath))
                    responseUrl = Request.Url.AbsoluteUri;

                string url = Url.Action("Index", "Home", new { lang = lang });
                responseUrl = url;
            }

            Response.Redirect(responseUrl);

            return null;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!Request.IsAjaxRequest())
            {
            }
        }

        protected override void DoAuthorization(AuthorizationContext filterContext)
        {
            //if (HttpContext.User != null && HttpContext.User.Identity.IsAuthenticated)
            //{
            //    var userIdentity = HttpContext.User.Identity.Name.Split(',');
            //    var userName = userIdentity[0];
            //    var authenticateType = HttpContext.User.Identity.AuthenticationType;
            //    UserDTO user = IoC.Container.Resolve<IUserService>().GetUserByUsername(userName);

            //    if (user != null)
            //    {
            //        ViewBag.UserFullNameTopBar = CultureHelper.IsArabic ? user.ArabicName : user.EnglishName;
            //        ViewBag.UsernameTopBar = user.Username;
            //    }

            //    if (!(HttpContext.User.Identity is UserIdentity))
            //    {
            //        var identity = new UserIdentity(userIdentity[0], authenticateType, new IdentityUser(user));
            //        var principal = new UserPrincipal(identity);
            //        HttpContext.User = principal;
            //        Thread.CurrentPrincipal = principal;
            //    }

            //}
        }
    }
}