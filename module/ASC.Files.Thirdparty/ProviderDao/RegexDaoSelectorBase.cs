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
using ASC.Files.Core;
using ASC.Files.Core.Security;
using log4net;

namespace ASC.Files.Thirdparty.ProviderDao
{
    internal abstract class RegexDaoSelectorBase<T> : IDaoSelector
    {
        public Regex Selector { get; set; }
        public Func<object, IFileDao> FileDaoActivator { get; set; }
        public Func<object, ISecurityDao> SecurityDaoActivator { get; set; }
        public Func<object, IFolderDao> FolderDaoActivator { get; set; }
        public Func<object, ITagDao> TagDaoActivator { get; set; }
        public Func<object, T> IDConverter { get; set; }

        protected RegexDaoSelectorBase(Regex selector)
            : this(selector, null, null, null, null)
        {
        }

        protected RegexDaoSelectorBase(Regex selector,
                                       Func<object, IFileDao> fileDaoActivator,
                                       Func<object, IFolderDao> folderDaoActivator,
                                       Func<object, ISecurityDao> securityDaoActivator,
                                       Func<object, ITagDao> tagDaoActivator
            )
            : this(selector, fileDaoActivator, folderDaoActivator, securityDaoActivator, tagDaoActivator, null)
        {
        }

        protected RegexDaoSelectorBase(
            Regex selector,
            Func<object, IFileDao> fileDaoActivator,
            Func<object, IFolderDao> folderDaoActivator,
            Func<object, ISecurityDao> securityDaoActivator,
            Func<object, ITagDao> tagDaoActivator,
            Func<object, T> idConverter)
        {
            if (selector == null) throw new ArgumentNullException("selector");

            Selector = selector;
            FileDaoActivator = fileDaoActivator;
            FolderDaoActivator = folderDaoActivator;
            SecurityDaoActivator = securityDaoActivator;
            TagDaoActivator = tagDaoActivator;
            IDConverter = idConverter;
        }

        public virtual ISecurityDao GetSecurityDao(object id)
        {
            return SecurityDaoActivator != null ? SecurityDaoActivator(id) : null;
        }



        public virtual object ConvertId(object id)
        {
            try
            {
                if (id == null) return null;

                return IDConverter != null ? IDConverter(id) : id;
            }
            catch (Exception fe)
            {
                throw new FormatException("Can not convert id: " + id, fe);
            }
        }

        public virtual object GetIdCode(object id)
        {
            return null;
        }

        public virtual bool IsMatch(object id)
        {
            return id != null && Selector.IsMatch(Convert.ToString(id, CultureInfo.InvariantCulture));
        }



        public virtual IFileDao GetFileDao(object id)
        {
            return FileDaoActivator != null ? FileDaoActivator(id) : null;
        }

        public virtual IFolderDao GetFolderDao(object id)
        {
            return FolderDaoActivator != null ? FolderDaoActivator(id) : null;
        }

        public virtual ITagDao GetTagDao(object id)
        {
            return TagDaoActivator != null ? TagDaoActivator(id) : null;
        }
    }
}