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

ASC.Projects.navSidePanel = (function() {
    var currentProjectId;
    var isInit;
    
    var init = function () {
        if (isInit) {
            return;
        }

        isInit = true;

        currentProjectId = jq.getURLParam('prjID');
        highlightMenu();
        initNavMenuItems();
        
        jq("#createNewTask").click(function() {
            var taskParams = {};
            var taskRespFromHash = jq.getURLParam("tasks_responsible");
            if (currentProjectId && taskRespFromHash && ASC.Projects.Common.userInProjectTeam(taskRespFromHash)) {
                taskParams.responsibles = [{id: taskRespFromHash}];
            }
            
            var proj = currentProjectId || jq.getAnchorParam('project', ASC.Controls.AnchorController.getAnchor());

            if (proj) {
                taskParams.projectId = proj;
                
                var milId = jq.getAnchorParam('milestone', ASC.Controls.AnchorController.getAnchor());
                var milest = ASC.Projects.Master.Milestones.find(function(item) {
                    return item.id == milId && (currentProjectId || item.projectId == proj);
                });

                if (milest)
                    taskParams.milestoneId = milest.id;
            }

            ASC.Projects.TaskAction.showCreateNewTaskForm(taskParams);
            jq("#createNewButton").hide();
            return false;
        });
        jq("#createNewMilestone").click(function() {
            ASC.Projects.MilestoneAction.showNewMilestonePopup();
            jq("#createNewButton").hide();
            return false;
        });
        jq("#createNewTimer").click(function() {
            var currentCategory = jq(".menu-list").find(".menu-item.currentCategory").attr("id");
            var taskId = jq.getURLParam("ID");
            if (currentProjectId) {
                if ((currentCategory == "menuTasks" || document.location.href.indexOf("timetracking.aspx") > 0) && taskId) {
                    ASC.Projects.Common.showTimer('timer.aspx?prjID=' + currentProjectId + '&ID=' + taskId);
                } else {
                    ASC.Projects.Common.showTimer('timer.aspx?prjID=' + currentProjectId);
                }
            } else {
                ASC.Projects.Common.showTimer('timer.aspx');
            }
            jq("#createNewButton").hide();
            return false;
        });
        jq("#myProjectsConteiner .expander").click(function(event) {
            var menuItem = jq(this).closest(".menu-sub-item");
            if (jq(menuItem).hasClass("open")) {
                jq(menuItem).removeClass("open");
            } else {
                jq(menuItem).addClass("open");
            }
            event.stopPropagation();
        });
        jq(".page-menu #menuMyProjects").click(function() {
            jq(".active").removeClass("active");
            jq(this).closest("div").addClass("active");
        });
    };

    var highlightMenu = function () {
        jq(".menu-list li.filter, li.menu-item, div.menu-item").removeClass("active currentCategory");
        var pathnamearr = location.pathname.split('/');
        var currentPage = pathnamearr[pathnamearr.length - 1];
        
        if (currentPage == "tmdocs.aspx") {
            jq("#menuTMDocs").removeClass("none-sub-list");
            jq("#menuTMDocs").addClass("sub-list open");
            if (!currentProjectId) {
                jq("#menuTMDocs a").attr("href", "#");
                jq("#menuTMDocs").addClass("active currentCategory");
            } else {
                jq("#menuTMDocs a").attr("href", "tmdocs.aspx");
            }
        }
        
        if (currentProjectId) {
            currentPage = "projects.aspx";
        }
        if (currentPage == "messages.aspx") {
            jq("#menuMessages").addClass("active currentCategory");
        }

        if (currentPage == "projects.aspx") {
            if (!jq("#myProjectsConteiner li[id=" + currentProjectId + "]").length) {
                jq("#menuProjects").addClass("active");
            } else {
                jq("#menuProjects").addClass("open");
                jq("#myProjectsConteiner").addClass("open");
                jq("#myProjectsConteiner").addClass("currentCategory");
                jq("#myProjectsConteiner li[id=" + currentProjectId + "]").addClass("active currentCategory");
            }

            jq("#menuProjects").addClass("currentCategory");
        }

        if (currentPage == "milestones.aspx") {
            jq("#menuMilestones").addClass("active currentCategory");
        }

        if (currentPage == "tasks.aspx") {
            jq("#menuTasks").addClass("active currentCategory");
        }

        if (currentPage == "timeTracking.aspx") {
            jq("#menuTimeTracking").addClass("active currentCategory");
        }

        if (currentPage == "reports.aspx") {
            jq("#menuReports").addClass("active");
        }

        if (currentPage == "projectTemplates.aspx") {
            jq("#menuTemplates").addClass("active");
            jq("#menuSettings").addClass("currentCategory open");
        }

        if (currentPage == "import.aspx") {
            jq("#menuImport").addClass("active");
            jq("#menuSettings").addClass("currentCategory open");
        }



        var currentCategory = jq(".menu-list").find(".menu-item.active").attr("id");
        var hash = document.location.hash;
        var flag = false;
        
        var pageProjectsFlag = location.href.indexOf("projects.aspx");
        var pageActionFlag = jq.getURLParam('action');

        if ((!currentProjectId && !pageActionFlag) && pageProjectsFlag > 0) {
            if (hash.indexOf("team_member=" + Teamlab.profile.id) > 0) {
                if (jq("#myProjectsConteiner").length) {
                    jq(".page-menu #menuMyProjects").closest("div").addClass("active");
                } else {
                    jq(".page-menu #menuMyProjects").closest("li").addClass("active");
                }

                flag = true;
            } else if (hash.indexOf("followed=true") > 0) {
                jq(".page-menu #menuFollowedProjects").parent("li").addClass("active");
                flag = true;
            } else {
                if (hash.indexOf("status=open") > 0) {
                    jq(".page-menu #menuActiveProjects").parent("li").addClass("active");
                    jq("#menuProjects").addClass("open");
                    flag = true;
                }
            }
            if (flag) {
                jq("#menuProjects").removeClass("active");
            }
        }

        if (currentCategory == "menuMilestones") {
            flag = false;
            if (hash.indexOf("user_tasks=" + Teamlab.profile.id) > 0 && hash.indexOf("status=open") > 0) {
                jq(".page-menu #menuMyMilestones").parent("li").addClass("active");
                flag = true;
            } else if (hash.indexOf("user_tasks=" + Teamlab.profile.id) > 0 && hash.indexOf("&deadlineStart=") > 0) {
                jq(".page-menu #menuUpcomingMilestones").parent("li").addClass("active");
                flag = true;
            }
            if (flag) {
                jq("#menuMilestones").removeClass("active");
            }
        }
        
        if (currentCategory == "menuTasks" && jq("#menuTasks").hasClass("open")) {
            flag = false;
            if (hash.indexOf("tasks_responsible=" + Teamlab.profile.id) > 0 && hash.indexOf("status=open") > 0) {
                jq(".page-menu #menuMyTasks").parent("li").addClass("active");
                flag = true;
            } else if (hash.indexOf("tasks_responsible=" + Teamlab.profile.id) > 0 && hash.indexOf("&deadlineStart=") > 0) {
                jq(".page-menu #menuUpcomingTasks").parent("li").addClass("active");
                flag = true;
            }
            if (flag) {
                jq("#menuTasks").removeClass("active");
            }
        } 
        
        if (currentCategory == "menuMessages" && jq("#menuMessages").hasClass("open")) {
            flag = false;
            if (hash.indexOf("author=" + Teamlab.profile.id) > 0) {
                jq(".page-menu #menuMyDiscussions").parent("li").addClass("active");
                flag = true;
            } else if (hash.indexOf("createdStart=") > 0) {
                jq(".page-menu #menuLatestDiscussion").parent("li").addClass("active");
                flag = true;
            }
            if (flag) {
                jq("#menuMessages").removeClass("active");
            }
        } 
    };

    var initNavMenuItems = function () {
        var date = new Date();
        var deadlineStart = date.getTime();
        date.setDate(date.getDate() + 7);
        var deadlineStop = date.getTime();

        date = new Date();
        var createdStop = date.getTime();
        date.setDate(date.getDate() - 7);
        var createdStart = date.getTime();
        var path;
        
        var menuitems = [];
        menuitems.push({ id: '#menuProjects .category-wrapper a', href: 'projects.aspx', onclick: highlightMenu });
        menuitems.push({ id: '.page-menu #menuMyProjects', href: "projects.aspx#" + ASC.Projects.AllProject.basePath +"&team_member=" + Teamlab.profile.id + "&status=open", onclick: highlightMenu });
        menuitems.push({ id: '.page-menu #menuFollowedProjects', href: "projects.aspx#" + ASC.Projects.AllProject.basePath + "&followed=true&status=open", onclick: highlightMenu });
        menuitems.push({ id: '.page-menu #menuActiveProjects', href: "projects.aspx#" + ASC.Projects.AllProject.basePath + "&status=open", onclick: highlightMenu });
        
        path = "#" + ASC.Projects.AllMilestones.basePath + "&user_tasks=" + Teamlab.profile.id + "&deadlineStart=" + deadlineStart + "&deadlineStop=" + deadlineStop;
        menuitems.push({ id: '#menuMilestones .category-wrapper a', href: 'milestones.aspx', onclick: highlightMenu });
        menuitems.push({ id: '.page-menu #menuMyMilestones', href: "milestones.aspx#" + ASC.Projects.AllMilestones.basePath + "&user_tasks=" + Teamlab.profile.id + "&status=open", onclick: highlightMenu });
        menuitems.push({ id: '.page-menu #menuUpcomingMilestones', href: "milestones.aspx" + path, onclick: highlightMenu });
        
        path = "#" + ASC.Projects.TasksManager.basePath + "&tasks_responsible=" + Teamlab.profile.id + "&deadlineStart=" + deadlineStart + "&deadlineStop=" + deadlineStop;
        menuitems.push({ id: '#menuTasks .category-wrapper a', href: 'tasks.aspx', onclick: highlightMenu });
        menuitems.push({ id: '.page-menu #menuMyTasks', href: "tasks.aspx#" + ASC.Projects.TasksManager.basePath + "&tasks_responsible=" + Teamlab.profile.id + "&status=open", onclick: highlightMenu });
        menuitems.push({ id: '.page-menu #menuUpcomingTasks', href: "tasks.aspx" + path, onclick: highlightMenu });
        
        path = "#" + ASC.Projects.Discussions.basePath + "&createdStart=" + createdStart + "&createdStop=" + createdStop;
        menuitems.push({ id: '#menuMessages .category-wrapper a', href: 'messages.aspx', onclick: highlightMenu });
        menuitems.push({ id: '.page-menu #menuLatestDiscussion', href: "messages.aspx" + path, onclick: highlightMenu });
        menuitems.push({ id: '.page-menu #menuMyDiscussions', href: "messages.aspx#" + ASC.Projects.Discussions.basePath + "&author=" + Teamlab.profile.id, onclick: highlightMenu });
        
        menuitems.push({ id: '#menuTimeTracking a', href: 'timeTracking.aspx', onclick: highlightMenu });

        initMenuItems(menuitems, true);
    };

    var initMenuItems = function (items, checkNotInPrj) {
        var currentPage = location.pathname.split('/')[3];
        var prjId = checkNotInPrj ? jq.getURLParam('prjID') : 0;
        items.forEach(function (item) {
            jq(item.id).attr('href', item.href);
        });
        if (checkInit(currentPage, prjId)) {
            items.forEach(function (item) {
                initMenuItem(item);
            });
        }
    };

    var checkInit = function (currentPage, prjId) {
        return !!(window.history && window.history.replaceState) && !prjId && !jq.getURLParam('action') &&
            (currentPage == "projects.aspx" || currentPage == "messages.aspx" || currentPage == "tasks.aspx" || currentPage == "milestones.aspx"|| currentPage.toLowerCase() == "timetracking.aspx");
    };

    var initMenuItem = function (item) {
        jq(item.id).on('click', function (event) {
            if (event.which == 1 && event.ctrlKey || event.which == 2) return true;
            event.stopPropagation();
            history.pushState({ href: item.href }, { href: item.href }, item.href);
            ASC.Controls.AnchorController.historyCheck();
            if (!jq(this).attr("href").contains("#")) {
                location.hash = "";
            } 
            ASC.Projects.Common.baseInit();
            item.onclick();
            return false;
        });
    };

    return {
        init: init,
        initMenuItems: initMenuItems
    };
})(jQuery);