using BundleTransformer.Core.Bundles;
using BundleTransformer.Core.Orderers;
using BundleTransformer.Core.Transformers;
using System.Web.Optimization;

namespace AuditManager.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;

            bundles.UseCdn = true;
            //var cssTransformer = new CssTransformer();
            //var jsTransformer = new JsTransformer();
            var cssTransformer = new StyleTransformer();
            var jsTransformer = new ScriptTransformer();
            var nullOrderer = new NullOrderer();

            //Style
            var cssBundle = new CustomStyleBundle("~/bundles/css");
            cssBundle.Include("~/Content/Site.less");
            //cssBundle.Include("~/Content/themes/base/jquery-ui.css");
            cssBundle.Include("~/Content/themes/base/all.css");
            cssBundle.Transforms.Add(cssTransformer);
            cssBundle.Orderer = nullOrderer;
            bundles.Add(cssBundle);

            var cssBootstrapBundle = new CustomStyleBundle("~/bundles/cssBootstrap");
            //cssBootstrapBundle.Include("~/Content/bootstrap/bootstrap.less");
            cssBootstrapBundle.Include("~/Content/bootstrap/bootstrap.css");
            cssBootstrapBundle.Include("~/Content/bootstrap/bootstrap-responsive.css");
            cssBootstrapBundle.Transforms.Add(cssTransformer);
            cssBootstrapBundle.Orderer = nullOrderer;
            bundles.Add(cssBootstrapBundle);

            var cssKendoBundle = new CustomStyleBundle("~/bundles/cssKendo");
            
            cssKendoBundle.Include("~/Content/kendo/2014.3.1119/kendo.common.min.css");
            cssKendoBundle.Include("~/Content/kendo/2014.3.1119/kendo.default.min.css");

            cssKendoBundle.Transforms.Add(cssTransformer);
            cssKendoBundle.Orderer = nullOrderer;
            bundles.Add(cssKendoBundle);

            var cssAppBundle = new CustomStyleBundle("~/bundles/cssApp");
            cssAppBundle.Include("~/Content/app/AppCommon.css");
            cssAppBundle.Include("~/Content/app/AppCommon.less");
            cssAppBundle.Include("~/Content/app/AmTree.less");
            cssAppBundle.Transforms.Add(cssTransformer);
            cssAppBundle.Orderer = nullOrderer;
            bundles.Add(cssAppBundle);

            //Script
            var jqueryBundle = new CustomScriptBundle("~/bundles/jquery");
            jqueryBundle.Include("~/Scripts/jquery-{version}.js");
            jqueryBundle.Include("~/Scripts/jquery.xdomainrequest.min.js");
            jqueryBundle.Include("~/Scripts/jquery.placeholder.js");
            jqueryBundle.Transforms.Add(jsTransformer);
            jqueryBundle.Orderer = nullOrderer;
            bundles.Add(jqueryBundle);

            var jqueryUiBundle = new CustomScriptBundle("~/bundles/jquery-ui");
            jqueryUiBundle.Include("~/Scripts/jquery-ui-{version}.js");
            jqueryUiBundle.Transforms.Add(jsTransformer);
            jqueryUiBundle.Orderer = nullOrderer;
            bundles.Add(jqueryUiBundle);

            var koBundle = new CustomScriptBundle("~/bundles/ko");
            koBundle.Include("~/Scripts/knockout-{version}.js");
            koBundle.Include("~/Scripts/knockout.mapping-latest.js");
            koBundle.Transforms.Add(jsTransformer);
            koBundle.Orderer = nullOrderer;
            bundles.Add(koBundle);

            //var jqueryvalBundle = new CustomScriptBundle("~/bundles/jqueryval");
            //jqueryvalBundle.Include("~/Scripts/jquery.validate*");
            //jqueryvalBundle.Transforms.Add(jsTransformer);
            //jqueryvalBundle.Orderer = nullOrderer;
            //bundles.Add(jqueryvalBundle);

            var jqueryval_1_Bundle = new CustomScriptBundle("~/bundles/jqueryval_1");
            jqueryval_1_Bundle.Include("~/Scripts/jquery.validate.js");
            jqueryval_1_Bundle.Transforms.Add(jsTransformer);
            jqueryval_1_Bundle.Orderer = nullOrderer;
            bundles.Add(jqueryval_1_Bundle);

            var jqueryval_2_Bundle = new CustomScriptBundle("~/bundles/jqueryval_2");
            jqueryval_2_Bundle.Include("~/Scripts/jquery.validate.unobtrusive.js");
            jqueryval_2_Bundle.Transforms.Add(jsTransformer);
            jqueryval_2_Bundle.Orderer = nullOrderer;
            bundles.Add(jqueryval_2_Bundle);

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.

            var modernizrBundle = new CustomScriptBundle("~/bundles/modernizr");
            modernizrBundle.Include("~/Scripts/modernizr-*");
            modernizrBundle.Transforms.Add(jsTransformer);
            modernizrBundle.Orderer = nullOrderer;
            bundles.Add(modernizrBundle);

            var bootstrapBundle = new CustomScriptBundle("~/bundles/bootstrap");
            bootstrapBundle.Include("~/Scripts/bootstrap.js");
            //bootstrapBundle.Include("~/Scripts/respond.js");
            bootstrapBundle.Transforms.Add(jsTransformer);
            bootstrapBundle.Orderer = nullOrderer;
            bundles.Add(bootstrapBundle);

            var kendoKoBundle = new CustomScriptBundle("~/bundles/kendoKo");

            kendoKoBundle.Include("~/Scripts/kendo/2014.3.1119/jszip.min.js");
            kendoKoBundle.Include("~/Scripts/kendo/2014.3.1119/kendo.web.min.js");
            
            kendoKoBundle.Include("~/Scripts/knockout-kendo.js");
            kendoKoBundle.Transforms.Add(jsTransformer);
            kendoKoBundle.Orderer = nullOrderer;
            bundles.Add(kendoKoBundle);

            var appBundle = new CustomScriptBundle("~/bundles/app");
            appBundle.Include("~/Scripts/App/AppKo.js");
            appBundle.Include("~/Scripts/App/AppCommon.js");
            appBundle.Include("~/Scripts/App/UpDown.js");
            appBundle.Include("~/Scripts/App/dimension.js");
            appBundle.Include("~/Scripts/App/dialog.js");
            appBundle.Include("~/Scripts/App/kGrid.js");
            appBundle.Include("~/Scripts/App/message.js");
            appBundle.Include("~/Scripts/App/AppMenuNToolTip.js");
            appBundle.Include("~/Scripts/App/RemoteEvent.js");
            appBundle.Transforms.Add(jsTransformer);
            appBundle.Orderer = nullOrderer;
            bundles.Add(appBundle);

            var appBundle_Local = new CustomScriptBundle("~/bundles/app_Local");
            appBundle_Local.Include("~/Scripts/App/AppKo_Local.js");
            appBundle_Local.Transforms.Add(jsTransformer);
            appBundle_Local.Orderer = nullOrderer;
            bundles.Add(appBundle_Local);

            var appBundle_Remote = new CustomScriptBundle("~/bundles/app_Remote");
            appBundle_Remote.Include("~/Scripts/App/AppKo_Remote.js");
            appBundle_Remote.Transforms.Add(jsTransformer);
            appBundle_Remote.Orderer = nullOrderer;
            bundles.Add(appBundle_Remote);

            var retNRfBundle = new CustomScriptBundle("~/bundles/retNRf");
            retNRfBundle.Include("~/Scripts/App/RETnRF.js");
            retNRfBundle.Transforms.Add(jsTransformer);
            retNRfBundle.Orderer = nullOrderer;
            bundles.Add(retNRfBundle);

        }
    }
}
