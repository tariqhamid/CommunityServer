/*
(c) Copyright Ascensio System SIA 2010-2014

This program is a free software product.
You can redistribute it and/or modify it under the terms 
of the GNU Affero General Public License (AGPL) version 3 as published by the Free Software
Foundation. In accordance with Section 7(a) of the GNU AGPL its Section 15 shall be amended
to the effect that Ascensio System SIA expressly excludes the warranty of non-infringement of 
any third-party rights.

This program is distributed WITHOUT ANY WARRANTY; without even the implied warranty 
of MERCHANTABILITY or FITNESS FOR A PARTICULAR  PURPOSE. For details, see 
the GNU AGPL at: http://www.gnu.org/licenses/agpl-3.0.html

You can contact Ascensio System SIA at Lubanas st. 125a-25, Riga, Latvia, EU, LV-1021.

The  interactive user interfaces in modified source and object code versions of the Program must 
display Appropriate Legal Notices, as required under Section 5 of the GNU AGPL version 3.
 
Pursuant to Section 7(b) of the License you must retain the original Product logo when 
distributing the program. Pursuant to Section 7(e) we decline to grant you any rights under 
trademark law for use of our trademarks.
 
All the Product's GUI elements, including illustrations and icon sets, as well as technical writing
content are licensed under the terms of the Creative Commons Attribution-ShareAlike 4.0
International. See the License terms at http://creativecommons.org/licenses/by-sa/4.0/legalcode
*/

using System;
using System.Globalization;
using System.Web;
using AjaxPro;
using ASC.Core;
using ASC.Core.Tenants;
using ASC.Core.Users;
using ASC.Web.Community.News.Code;
using ASC.Web.Community.News.Code.DAO;
using ASC.Web.Community.News.Resources;
using ASC.Web.Community.Product;
using ASC.Web.Studio;
using ASC.Web.Studio.Core;
using ASC.Web.Studio.Utility;
using ASC.Web.Studio.Utility.HtmlUtility;
using FeedNS = ASC.Web.Community.News.Code;

namespace ASC.Web.Community.News
{
    [AjaxNamespace("EditNews")]
    public partial class EditNews : MainPage
    {
        private RequestInfo info;
        protected string _text = "";

        private RequestInfo Info
        {
            get { return info ?? (info = new RequestInfo(Request)); }
        }

        public long FeedId
        {
            get { return ViewState["FeedID"] != null ? Convert.ToInt32(ViewState["FeedID"], CultureInfo.CurrentCulture) : 0; }
            set { ViewState["FeedID"] = value; }
        }

        private void BindNewsTypes()
        {
            feedType.DataSource = new[]
                {
                    FeedTypeInfo.FromFeedType(FeedType.News),
                    FeedTypeInfo.FromFeedType(FeedType.Order),
                    FeedTypeInfo.FromFeedType(FeedType.Advert)
                };
            feedType.DataBind();

            if (!string.IsNullOrEmpty(Request["type"]))
            {
                var requestFeedType = (FeedType)Enum.Parse(typeof(FeedType), Request["type"], true);
                var feedTypeInfo = FeedTypeInfo.FromFeedType(requestFeedType);

                var item = feedType.Items.FindByText(feedTypeInfo.TypeName);

                feedType.SelectedValue = item.Value;
            }
            else
            {
                feedType.SelectedIndex = 0;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Utility.RegisterTypeForAjax(GetType());

            if (!CommunitySecurity.CheckPermissions(NewsConst.Action_Add))
                Response.Redirect(FeedUrls.MainPageUrl, true);

            var storage = FeedStorageFactory.Create();
            FeedNS.Feed feed = null;
            if (!string.IsNullOrEmpty(Request["docID"]))
            {
                long docID;
                if (long.TryParse(Request["docID"], out docID))
                {
                    feed = storage.GetFeed(docID);
                    (Master as NewsMaster).CurrentPageCaption = NewsResource.NewsEditBreadCrumbsNews;
                    Title = HeaderStringHelper.GetPageTitle(NewsResource.NewsEditBreadCrumbsNews);
                    _text = (feed != null ? feed.Text : "").HtmlEncode();
                }
            }
            else
            {
                _text = "";
                (Master as NewsMaster).CurrentPageCaption = NewsResource.NewsAddBreadCrumbsNews;
                Title = HeaderStringHelper.GetPageTitle(NewsResource.NewsAddBreadCrumbsNews);
            }

            if (!IsPostBack)
            {
                BindNewsTypes();

                if (feed != null)
                {
                    if (!CommunitySecurity.CheckPermissions(feed, NewsConst.Action_Edit))
                    {
                        Response.Redirect(FeedUrls.MainPageUrl, true);
                    }
                    feedName.Text = feed.Caption;
                    _text = feed.Text;
                    FeedId = feed.Id;
                    feedType.SelectedIndex = (int)Math.Log((int)feed.FeedType, 2);
                }
                else
                {
                    if (!string.IsNullOrEmpty(Request["type"]))
                    {
                        var requestFeedType = (FeedType)Enum.Parse(typeof(FeedType), Request["type"], true);
                        var feedTypeInfo = FeedTypeInfo.FromFeedType(requestFeedType);
                        var item = feedType.Items.FindByText(feedTypeInfo.TypeName);

                        feedType.SelectedValue = item.Value;
                        feedType.SelectedIndex = (int)Math.Log((int)requestFeedType, 2);
                    }
                }
            }
            else
            {
                var control = FindControl(Request.Params["__EVENTTARGET"]);
                if (lbCancel.Equals(control))
                {
                    CancelFeed(sender, e);
                }
                else
                {
                    SaveFeed();
                }
            }

            RenderScripts();
        }

        protected void RenderScripts()
        {
            Page.RegisterBodyScripts(VirtualPathUtility.ToAbsolute("~/js/asc/core/decoder.js"));

            Page.RegisterBodyScripts(VirtualPathUtility.ToAbsolute("~/usercontrols/common/ckeditor/ckeditor.js"));

            Page.RegisterInlineScript("CKEDITOR.replace('ckEditor', { toolbar : 'ComNews', extraPlugins: '', filebrowserUploadUrl: '" + RedirectUpload() + @"'});");

            var scriptSb = new System.Text.StringBuilder();
            scriptSb.AppendLine(@"function FeedPrevShow(text) {
                    jq('#feedPrevDiv_Caption').val(jq('#" + feedName.ClientID + @"').val());
                    jq('#feedPrevDiv_Body').html(text);
                    jq('#feedPrevDiv').show();
                    jq.scrollTo(jq('#feedPrevDiv').position().top, {speed:500});
                }"
                );
            scriptSb.AppendLine(@"function HidePreview() {
                    jq('#feedPrevDiv').hide();
                    jq.scrollTo(jq('#newsCaption').position().top, {speed:500});
                }"
                );
            scriptSb.AppendLine(@"jq(function() {
                    jq('#" + feedType.ClientID + @"').tlCombobox();
                    jq('#" + feedType.ClientID + @"').removeClass('display-none');
                });"
                );

            Page.RegisterInlineScript(scriptSb.ToString(), true, false);
        }

        [AjaxMethod(HttpSessionStateRequirement.ReadWrite)]
        public string GetPreviewFull(string html)
        {
            return HtmlUtility.GetFull(html);
        }

        [AjaxMethod(HttpSessionStateRequirement.ReadWrite)]
        public FeedAjaxInfo FeedPreview(string captionFeed, string bodyFeed)
        {
            var feed = new FeedAjaxInfo
                {
                    FeedCaption = captionFeed,
                    FeedText = HtmlUtility.GetFull(bodyFeed, CommunityProduct.ID),
                    Date = TenantUtil.DateTimeNow().Ago(),
                    UserName = CoreContext.UserManager.GetUsers(SecurityContext.CurrentAccount.ID).RenderProfileLink(CommunityProduct.ID)
                };
            return feed;
        }

        protected void CancelFeed(object sender, EventArgs e)
        {
            var url = FeedUrls.MainPageUrl;

            if (FeedId != 0)
            {
                CommonControlsConfigurer.FCKEditingCancel("news", FeedId.ToString());
                url += "?docid=" + FeedId.ToString();
            }
            else
                CommonControlsConfigurer.FCKEditingCancel("news");

            Response.Redirect(url, true);
        }

        protected void SaveFeed()
        {

            if (string.IsNullOrEmpty(feedName.Text))
            {
                ((NewsMaster)Master).SetInfoMessage(NewsResource.RequaredFieldValidatorCaption, InfoType.Alert);
                return;
            }

            var storage = FeedStorageFactory.Create();
            var isEdit = (FeedId != 0);
            var feed = isEdit ? storage.GetFeed(FeedId) : new FeedNews();
            feed.Caption = feedName.Text;
            feed.Text = (Request["mobiletext"] ?? "");
            feed.FeedType = (FeedType)int.Parse(feedType.SelectedValue, CultureInfo.CurrentCulture);
            storage.SaveFeed(feed, isEdit, FeedType.News);

            CommonControlsConfigurer.FCKEditingComplete("news", feed.Id.ToString(), feed.Text, isEdit);

            Response.Redirect(FeedUrls.GetFeedUrl(feed.Id, Info.UserId));
        }

        protected string RedirectUpload()
        {
            return string.Format("{0}://{1}:{2}{3}", Request.GetUrlRewriter().Scheme, Request.GetUrlRewriter().Host, Request.GetUrlRewriter().Port,
                                 VirtualPathUtility.ToAbsolute("~/") + "fckuploader.ashx?esid=news&newEditor=true" + (FeedId != 0 ? "&iid=" + FeedId.ToString() : ""));
        }
    }

    public class FeedAjaxInfo
    {
        public string FeedCaption { get; set; }

        public string FeedText { get; set; }

        public string Date { get; set; }

        public string UserName { get; set; }
    }
}