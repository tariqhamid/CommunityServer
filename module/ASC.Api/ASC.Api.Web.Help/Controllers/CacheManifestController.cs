/*
 * 
 * (c) Copyright Ascensio System SIA 2010-2014
 * 
 * This program is a free software product.
 * You can redistribute it and/or modify it under the terms of the GNU Affero General Public License
 * (AGPL) version 3 as published by the Free Software Foundation. 
 * In accordance with Section 7(a) of the GNU AGPL its Section 15 shall be amended to the effect 
 * that Ascensio System SIA expressly excludes the warranty of non-infringement of any third-party rights.
 * 
 * This program is distributed WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
 * For details, see the GNU AGPL at: http://www.gnu.org/licenses/agpl-3.0.html
 * 
 * You can contact Ascensio System SIA at Lubanas st. 125a-25, Riga, Latvia, EU, LV-1021.
 * 
 * The interactive user interfaces in modified source and object code versions of the Program 
 * must display Appropriate Legal Notices, as required under Section 5 of the GNU AGPL version 3.
 * 
 * Pursuant to Section 7(b) of the License you must retain the original Product logo when distributing the program. 
 * Pursuant to Section 7(e) we decline to grant you any rights under trademark law for use of our trademarks.
 * 
 * All the Product's GUI elements, including illustrations and icon sets, as well as technical 
 * writing content are licensed under the terms of the Creative Commons Attribution-ShareAlike 4.0 International. 
 * See the License terms at http://creativecommons.org/licenses/by-sa/4.0/legalcode
 * 
*/

using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web.Mvc;

namespace ASC.Api.Web.Help.Controllers
{
    public class CacheManifestController : Controller
    {
        //
        // GET: /CacheManifest/
        public ActionResult GetCacheManifest()
        {
            if (string.Equals(ConfigurationManager.AppSettings["offline_cache"],bool.TrueString,StringComparison.OrdinalIgnoreCase))
                return new CacheActionResult(MvcApplication.CacheManifest);
            return new HttpNotFoundResult();
        }

    }

    public class HttpNotFoundResult : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            context.RequestContext.HttpContext.Response.StatusCode = (int) HttpStatusCode.NotFound;
        }
    }

    public class CacheActionResult : ActionResult
    {
        private readonly CacheManifest _mainfest;

        public CacheActionResult(CacheManifest mainfest)
        {
            _mainfest = mainfest;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "text/cache-manifest";
            _mainfest.Write(context.HttpContext.Response.Output);
        }
    }

    public static class ControllerExtensions
    {
        public static string RenderView(this Controller controller, string viewName, object model)
        {
            return RenderView(controller, viewName, new ViewDataDictionary(model));
        }

        public static string RenderView(this Controller controller, string viewName, ViewDataDictionary viewData)
        {
            var controllerContext = controller.ControllerContext;

            var viewResult = ViewEngines.Engines.FindView(controllerContext, viewName, null);

            StringWriter stringWriter;

            using (stringWriter = new StringWriter())
            {
                var viewContext = new ViewContext(
                    controllerContext,
                    viewResult.View,
                    viewData,
                    controllerContext.Controller.TempData,
                    stringWriter);

                viewResult.View.Render(viewContext, stringWriter);
                viewResult.ViewEngine.ReleaseView(controllerContext, viewResult.View);
            }

            return stringWriter.ToString();
        }
    }
}
