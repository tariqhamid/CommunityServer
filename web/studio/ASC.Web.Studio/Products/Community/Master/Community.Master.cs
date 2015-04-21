/*
 *
 * (c) Copyright Ascensio System Limited 2010-2015
 *
 * This program is freeware. You can redistribute it and/or modify it under the terms of the GNU 
 * General Public License (GPL) version 3 as published by the Free Software Foundation (https://www.gnu.org/copyleft/gpl.html). 
 * In accordance with Section 7(a) of the GNU GPL its Section 15 shall be amended to the effect that 
 * Ascensio System SIA expressly excludes the warranty of non-infringement of any third-party rights.
 *
 * THIS PROGRAM IS DISTRIBUTED WITHOUT ANY WARRANTY; WITHOUT EVEN THE IMPLIED WARRANTY OF MERCHANTABILITY OR
 * FITNESS FOR A PARTICULAR PURPOSE. For more details, see GNU GPL at https://www.gnu.org/copyleft/gpl.html
 *
 * You can contact Ascensio System SIA by email at sales@onlyoffice.com
 *
 * The interactive user interfaces in modified source and object code versions of ONLYOFFICE must display 
 * Appropriate Legal Notices, as required under Section 5 of the GNU GPL version 3.
 *
 * Pursuant to Section 7 § 3(b) of the GNU GPL you must retain the original ONLYOFFICE logo which contains 
 * relevant author attributions when distributing the software. If the display of the logo in its graphic 
 * form is not reasonably feasible for technical reasons, you must include the words "Powered by ONLYOFFICE" 
 * in every copy of the program you distribute. 
 * Pursuant to Section 7 § 3(e) we decline to grant you any rights under trademark law for use of our trademarks.
 *
*/


using System;
using System.Text;
using System.Web.UI;
using ASC.Web.Community.Controls;
using ASC.Data.Storage;
using System.Web;
using ASC.Web.Community.Resources;

namespace ASC.Web.Community
{
    public partial class CommunityMasterPage : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.RegisterBodyScripts(GetFileStaticRelativePath("common.js"));

            _sideNavigation.Controls.Add(LoadControl(NavigationSidePanel.Location));

            if (!(Page is _Default))
            {
                var script = new StringBuilder();
                script.Append("window.ASC=window.ASC||{};");
                script.Append("window.ASC.Community=window.ASC.Community||{};");
                script.Append("window.ASC.Community.Resources={};");
                script.AppendFormat("window.ASC.Community.Resources.HelpTitleAddNew=\"{0}\";", CommunityResource.HelpTitleAddNew);
                script.AppendFormat("window.ASC.Community.Resources.HelpContentAddNew=\"{0}\";", CommunityResource.HelpContentAddNew);
                script.AppendFormat("window.ASC.Community.Resources.HelpTitleSettings=\"{0}\";", CommunityResource.HelpTitleSettings);
                script.AppendFormat("window.ASC.Community.Resources.HelpContentSettings=\"{0}\";", CommunityResource.HelpContentSettings);
                script.AppendFormat("window.ASC.Community.Resources.HelpTitleNavigateRead=\"{0}\";", CommunityResource.HelpTitleNavigateRead);
                script.AppendFormat("window.ASC.Community.Resources.HelpContentNavigateRead=\"{0}\";", CommunityResource.HelpContentNavigateRead);
                script.AppendFormat("window.ASC.Community.Resources.HelpTitleSwitchModules=\"{0}\";", CommunityResource.HelpTitleSwitchModules);
                script.AppendFormat("window.ASC.Community.Resources.HelpContentSwitchModules=\"{0}\";", CommunityResource.HelpContentSwitchModules);

                Page.RegisterInlineScript(script.ToString());
            }
            else
            {
                Master.DisabledHelpTour = true;
            }
        }

        protected string GetFileStaticRelativePath(String fileName)
        {
            if (fileName.EndsWith(".js"))
            {
                return ResolveUrl("~/products/community/js/" + fileName);
            }
            if (fileName.EndsWith(".ascx"))
            {
                return VirtualPathUtility.ToAbsolute("~/products/community/controls/" + fileName);
            }
            if (fileName.EndsWith(".css"))
            {
                return ResolveUrl("~/products/community/app_themes/default/" + fileName);
            }
            if (fileName.EndsWith(".png") || fileName.EndsWith(".gif") || fileName.EndsWith(".jpg"))
            {
                return WebPath.GetPath("/products/community/app_themes/default/images/" + fileName);
            }
            return fileName;
        }
    }
}