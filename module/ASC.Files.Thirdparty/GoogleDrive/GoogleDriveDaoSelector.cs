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
using System.Globalization;
using System.Text.RegularExpressions;
using ASC.Core;
using ASC.Files.Thirdparty.ProviderDao;
using ASC.Files.Core;
using ASC.Files.Core.Security;
using ASC.Web.Files.Classes;

namespace ASC.Files.Thirdparty.GoogleDrive
{
    internal class GoogleDriveDaoSelector : RegexDaoSelectorBase<string>
    {
        internal class GoogleDriveInfo
        {
            public GoogleDriveProviderInfo GoogleDriveProviderInfo { get; set; }

            public string Path { get; set; }
            public string PathPrefix { get; set; }
        }

        public GoogleDriveDaoSelector()
            : base(new Regex(@"^drive-(?'id'\d+)(-(?'path'.*)){0,1}$", RegexOptions.Singleline | RegexOptions.Compiled))
        {
        }

        public override IFileDao GetFileDao(object id)
        {
            return new GoogleDriveFileDao(GetInfo(id), this);
        }

        public override IFolderDao GetFolderDao(object id)
        {
            return new GoogleDriveFolderDao(GetInfo(id), this);
        }

        public override ITagDao GetTagDao(object id)
        {
            return new GoogleDriveTagDao(GetInfo(id), this);
        }

        public override ISecurityDao GetSecurityDao(object id)
        {
            return new GoogleDriveSecurityDao(GetInfo(id), this);
        }

        public override object ConvertId(object id)
        {
            if (id != null)
            {
                var match = Selector.Match(Convert.ToString(id, CultureInfo.InvariantCulture));
                if (match.Success)
                {
                    return match.Groups["path"].Value.Replace('|', '/');
                }
                throw new ArgumentException("Id is not a GoogleDrive id");
            }
            return base.ConvertId(null);
        }

        private GoogleDriveInfo GetInfo(object objectId)
        {
            if (objectId == null) throw new ArgumentNullException("objectId");
            var id = Convert.ToString(objectId, CultureInfo.InvariantCulture);
            var match = Selector.Match(id);
            if (match.Success)
            {
                var providerInfo = GetProviderInfo(Convert.ToInt32(match.Groups["id"].Value));

                return new GoogleDriveInfo
                           {
                               Path = match.Groups["path"].Value,
                               GoogleDriveProviderInfo = providerInfo,
                               PathPrefix = "drive-" + match.Groups["id"].Value
                           };
            }
            throw new ArgumentException("Id is not a GoogleDrive id");
        }

        public override object GetIdCode(object id)
        {
            if (id != null)
            {
                var match = Selector.Match(Convert.ToString(id, CultureInfo.InvariantCulture));
                if (match.Success)
                {
                    return match.Groups["id"].Value;
                }
            }
            return base.GetIdCode(id);
        }

        private GoogleDriveProviderInfo GetProviderInfo(int linkId)
        {
            GoogleDriveProviderInfo info;

            using (var dbDao = Global.DaoFactory.GetProviderDao())
            {
                try
                {
                    info = (GoogleDriveProviderInfo)dbDao.GetProviderInfo(linkId);
                }
                catch (InvalidOperationException)
                {
                    throw new ArgumentException("Provider id not found or you have no access");
                }
            }
            return info;
        }

        public void RenameProvider(GoogleDriveProviderInfo googleDriveProviderInfo, string newTitle)
        {
            using (var dbDao = new ProviderAccountDao(CoreContext.TenantManager.GetCurrentTenant().TenantId, FileConstant.DatabaseId))
            {
                dbDao.UpdateProviderInfo(googleDriveProviderInfo.ID, newTitle, googleDriveProviderInfo.RootFolderType);
                googleDriveProviderInfo.UpdateTitle(newTitle); //This will update cached version too
            }
        }
    }
}