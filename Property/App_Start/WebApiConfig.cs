using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Routing;

namespace Property
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
            //config.MapHttpAttributeRoutes();

            RouteTable.Routes.MapHttpRoute(
               name: "DefaultApi",
               routeTemplate: "api/{controller}/{id}",
               defaults: new { id = RouteParameter.Optional }
           ).RouteHandler = new SessionRouteHandler();
        }
        public class SessionRouteHandler : System.Web.Routing.IRouteHandler
        {
            System.Web.IHttpHandler System.Web.Routing.IRouteHandler.GetHttpHandler(System.Web.Routing.RequestContext requestContext)
            {
                return new SessionControllerHandler(requestContext.RouteData);
            }
        }

        public class SessionControllerHandler : System.Web.Http.WebHost.HttpControllerHandler, System.Web.SessionState.IRequiresSessionState
        {
            public SessionControllerHandler(System.Web.Routing.RouteData routeData)
                : base(routeData)
            { }
        }
    }
}
