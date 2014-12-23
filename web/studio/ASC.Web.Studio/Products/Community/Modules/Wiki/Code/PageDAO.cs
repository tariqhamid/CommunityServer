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
using System.Collections.Generic;
using System.Linq;
using ASC.Common.Data.Sql;
using ASC.Common.Data.Sql.Expressions;
using ASC.Core.Tenants;
using ASC.FullTextIndex;

namespace ASC.Web.UserControls.Wiki.Data
{
    class PageDAO : BaseDao
    {
        private const int MAX_FIND = 100;


        public PageDAO(string dbid, int tenant)
            : base(dbid, tenant)
        {
        }


        public List<Page> GetPages()
        {
            var q = PageQuery(Exp.Empty)
                .OrderBy("p.pagename", true);
            return ExecQuery(q);
        }

        public List<Page> GetPages(string category)
        {
            var q = PageQuery(EqExp.Eq("c.categoryname", category))
                .InnerJoin("wiki_categories c", Exp.EqColumns("p.pagename", "c.pagename") & Exp.EqColumns("p.tenant", "c.tenant"))
                .OrderBy("p.pagename", true);
            return ExecQuery(q);
        }

        public List<Page> GetPages(Guid userId)
        {
            var q = PageQuery(EqExp.Eq("h.create_by", userId.ToString()) & Exp.Eq("h.version", 1))
                .OrderBy("p.pagename", true);
            return ExecQuery(q);
        }

        public int GetPagesCount(Guid userId)
        {
            var q = Query("wiki_pages_history")
                .SelectCount()
                .Where("create_by", userId.ToString())
                .Where("version", 1);
            return db.ExecuteScalar<int>(q);
        }

        public List<Page> GetRecentEditedPages(int max)
        {
            var q = PageQuery(Exp.Empty)
                .SetMaxResults(max)
                .OrderBy("p.modified_on", false);
            return ExecQuery(q);
        }

        public List<Page> GetNewPages(int max)
        {
            var q = PageQuery(Exp.Empty)
                .SetMaxResults(max)
                .OrderBy("h.create_on", false);
            return ExecQuery(q);
        }

        public List<Page> GetPages(IEnumerable<string> pagenames)
        {
            var q = PageQuery(Exp.In("p.pagename", pagenames.ToArray()))
                .OrderBy("p.pagename", true);
            return ExecQuery(q);
        }


        public Page GetPage(string pagename, int version)
        {
            var q = Query("wiki_pages_history h")
                .Select("h.pagename", "h.version", "h.create_by", "h.create_on")
                .Select(new SqlQuery("wiki_pages_history t").Select("t.create_by").Where(Exp.EqColumns("h.tenant", "t.tenant") & Exp.EqColumns("h.pagename", "t.pagename") & Exp.Eq("t.version", 1)))
                .Select("h.body")
                .Where("h.pagename", pagename)
                .OrderBy("h.version", false)
                .SetMaxResults(1);
            if (0 < version)
            {
                q.Where("h.version", version);
            }

            return ExecQuery(q)
                .SingleOrDefault();
        }

        public List<Page> GetPageHistory(string pagename)
        {
            var q = Query("wiki_pages_history h")
                .Select("h.pagename", "h.version", "h.create_by", "h.create_on")
                .Select(new SqlQuery("wiki_pages_history t").Select("t.create_by").Where(Exp.EqColumns("h.tenant", "t.tenant") & Exp.EqColumns("h.pagename", "t.pagename") & Exp.Eq("t.version", 1)))
                .Where("h.pagename", pagename)
                .OrderBy("h.version", false);
            return ExecQuery(q);
        }

        public int GetPageMaxVersion(string pagename)
        {
            var q = Query("wiki_pages")
                .Select("version")
                .Where("pagename", pagename);
            return db.ExecuteScalar<int>(q);
        }


        public List<Page> SearchPagesByName(string name, bool startwith)
        {
            var q = PageQuery(Exp.Like("upper(p.pagename)", name, startwith ? SqlLike.StartWith : SqlLike.AnyWhere))
                .OrderBy("p.pagename", true)
                .SetMaxResults(MAX_FIND);
            return ExecQuery(q);
        }

        public List<Page> SearchPagesByContent(string content)
        {
            if (string.IsNullOrEmpty(content)) return new List<Page>();

            IEnumerable<string> pagenames = null;

            if (FullTextSearch.SupportModule(FullTextSearch.WikiModule))
            {
                pagenames = FullTextSearch.Search(content, FullTextSearch.WikiModule).GetIdentifiers();
            }
            else
            {
                var keys = content.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(k => k.Trim())
                    .Where(k => 3 <= k.Length);

                var where = Exp.Empty;
                foreach (var k in keys)
                {
                    where &= Exp.Like("h.pagename", k) | Exp.Like("h.body", k);
                }

                var q = Query("wiki_pages p")
                    .Select("p.pagename")
                    .InnerJoin("wiki_pages_history h", Exp.EqColumns("p.tenant", "h.tenant") & Exp.EqColumns("p.pagename", "h.pagename") & Exp.EqColumns("p.version", "h.version"))
                    .Where(where)
                    .OrderBy("p.modified_on", false)
                    .SetMaxResults(MAX_FIND);

                pagenames = db
                    .ExecuteList(q)
                    .ConvertAll(r => (string)r[0]);
            }

            return GetPages(pagenames);
        }


        public Page SavePage(Page page)
        {
            if (page == null) throw new ArgumentNullException("page");

            var i1 = Insert("wiki_pages")
                .InColumnValue("pagename", page.PageName)
                .InColumnValue("version", page.Version)
                .InColumnValue("modified_by", page.UserID)
                .InColumnValue("modified_on", DateTime.UtcNow);

            var i2 = Insert("wiki_pages_history")
                .InColumnValue("pagename", page.PageName)
                .InColumnValue("version", page.Version)
                .InColumnValue("create_by", page.UserID)
                .InColumnValue("create_on", DateTime.UtcNow)
                .InColumnValue("body", page.Body);

            db.ExecuteBatch(new[] { i1, i2 });

            return page;
        }

        public void RevomePage(string pagename)
        {
            var d1 = Delete("wiki_pages").Where("pagename", pagename);
            var d2 = Delete("wiki_pages_history").Where("pagename", pagename);
            var d3 = Delete("wiki_comments").Where("pagename", pagename);

            db.ExecuteBatch(new[] { d1, d2, d3 });
        }


        private SqlQuery PageQuery(Exp where)
        {
            return Query("wiki_pages p")
                .InnerJoin("wiki_pages_history h", Exp.EqColumns("p.tenant", "h.tenant") & Exp.EqColumns("p.pagename", "h.pagename") & Exp.Eq("h.version", 1))
                .Select("p.pagename", "p.version", "p.modified_by", "p.modified_on", "h.create_by")
                .Where(where);
        }

        private List<Page> ExecQuery(SqlQuery q)
        {
            return db
                .ExecuteList(q)
                .ConvertAll(r => new Page
                {
                    Tenant = tenant,
                    PageName = (string)r[0],
                    Version = Convert.ToInt32(r[1]),
                    UserID = new Guid((string)r[2]),
                    Date = TenantUtil.DateTimeFromUtc((DateTime)r[3]),
                    OwnerID = r[4] != null ? new Guid((string)r[4]) : default(Guid),
                    Body = 5 < r.Length ? (string)r[5] : string.Empty,
                });
        }
    }
}