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
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Services;
using ASC.Core;
using ASC.Mail.Aggregator;
using ASC.Mail.Aggregator.DataStorage;
using ASC.Web.Core.Files;
using Ionic.Zip;
using Ionic.Zlib;

namespace ASC.Web.Mail.HttpHandlers
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class DownloadAllHandler : IHttpHandler
    {
        private int TenantId
        {
            get { return CoreContext.TenantManager.GetCurrentTenant().TenantId; }
        }

        private string Username
        {
            get { return SecurityContext.CurrentAccount.ID.ToString(); }
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                if (!MailPage.IsTurnOnAttachmentsGroupOperations())
                    throw new Exception("Operation is turned off.");

                context.Response.ContentType = "application/octet-stream";
                context.Response.Charset = Encoding.UTF8.WebName;

                int message_id = Convert.ToInt32(context.Request.QueryString["messageid"]);

                DownloadAllZipped(message_id, context);

            }
            catch (Exception)
            {
                context.Response.Redirect("404.html");
            }
            finally
            {
                context.Response.End();
            }
        }

        // ToDo : move constant to config
        private const string ArchiveName = "download.zip";

        private void DownloadAllZipped(int message_id, HttpContext context)
        {
            var mail_box_manager = new MailBoxManager(0);

            var attachments = mail_box_manager.GetMessageAttachments(TenantId, Username, message_id);

            if (attachments.Any())
            {
                using (var zip = new ZipFile())
                {
                    zip.CompressionLevel = CompressionLevel.Level3;
                    zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                    zip.AlternateEncoding = Encoding.GetEncoding(Thread.CurrentThread.CurrentCulture.TextInfo.OEMCodePage);

                    foreach (var attachment in attachments)
                    {
                        using (var file = AttachmentManager.GetAttachmentStream(attachment))
                        {
                            var filename = file.FileName;

                            if (zip.ContainsEntry(filename))
                            {
                                var counter = 1;
                                var temp_name = filename;
                                while (zip.ContainsEntry(temp_name))
                                {
                                    temp_name = filename;
                                    var suffix = " (" + counter + ")";
                                    temp_name = 0 < temp_name.IndexOf('.')
                                                   ? temp_name.Insert(temp_name.LastIndexOf('.'), suffix)
                                                   : temp_name + suffix;

                                    counter++;
                                }
                                filename = temp_name;
                            }

                            zip.AddEntry(filename, file.FileStream.GetCorrectBuffer());
                        }
                    }

                    context.Response.AddHeader("Content-Disposition", ContentDispositionUtil.GetHeaderValue(ArchiveName));

                    zip.Save(context.Response.OutputStream);

                }

            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
