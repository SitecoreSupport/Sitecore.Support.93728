namespace Sitecore.Support.Pipelines.RenderLayout
{
    using Sitecore;
    using Sitecore.Mvc.Pipelines.Request.RequestBegin;
    using Sitecore.Pipelines.RenderLayout;
    using Sitecore.Publishing;
    using Sitecore.Sites;
    using Sitecore.Web.UI.HtmlControls;
    using System;

    public class SecurityCheck : Sitecore.Pipelines.RenderLayout.SecurityCheck
    {
        private RequestBeginArgs originalArgs;

        protected override bool HasAccess()
        {
            SiteContext site = Context.Site;
            if (((site != null) && site.RequireLogin) && (!Context.User.IsAuthenticated && !this.IsLoginPageRequest()))
            {
                this.originalArgs.AbortPipeline();
                return false;
            }
            if ((((site != null) && (site.DisplayMode != DisplayMode.Normal)) && (!Context.User.IsAuthenticated && (PreviewManager.GetShellUser() == string.Empty))) && !this.IsLoginPageRequest())
            {
                this.originalArgs.AbortPipeline();
                return false;
            }
            return true;
        }

        public virtual void Process(RequestBeginArgs args)
        {
            this.originalArgs = args;
            RenderLayoutArgs args2 = new RenderLayoutArgs(new Page(), null);
            base.Process(args2);
        }
    }
}
