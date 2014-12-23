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

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34011
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASC.Web.CRM.Services.NotifyService {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class CRMPatternResource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CRMPatternResource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ASC.Web.CRM.Services.NotifyService.CRMPatternResource", typeof(CRMPatternResource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to h1. New Event Added to &quot;$EntityTitle&quot;:&quot;${__VirtualRootPath}/${EntityRelativeURL}&quot;            
        ///$__DateTime &quot;$__AuthorName&quot;:&quot;$__AuthorUrl&quot; has added a new event to &quot;$EntityTitle&quot;:&quot;${__VirtualRootPath}/${EntityRelativeURL}&quot;:                                                               
        ///          $AdditionalData.get_item(&quot;EventContent&quot;)
        ///
        ///          #foreach($fileInfo in $AdditionalData.get_item(&quot;Files&quot;).Keys)
        ///
        ///          #beforeall
        ///
        ///          ----------------------------------------
        ///
        ///          #each
        ///        /// [rest of string was truncated]&quot;;.
        /// </summary>
        public static string pattern_AddRelationshipEvent {
            get {
                return ResourceManager.GetString("pattern_AddRelationshipEvent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to h1. CRM. New Contact Created Using the &apos;Website Contact Form&apos; &quot;$EntityTitle&quot;:&quot;${__VirtualRootPath}/products/crm/default.aspx?ID=$EntityID&quot;
        ///
        ///$__DateTime A new contact has been created using the &apos;Website Contact Form&apos; &quot;$EntityTitle&quot;:&quot;${__VirtualRootPath}/products/crm/default.aspx?ID=$EntityID&quot;
        ///
        ///Contact information:
        ///
        ///#foreach($contactInfo in $AdditionalData.Keys)
        ///#each
        ///
        ///$contactInfo: $AdditionalData.get_item($contactInfo)
        ///
        ///#end
        ///
        ///
        ///^You receive this email because you are a registered user of the &quot;$ [rest of string was truncated]&quot;;.
        /// </summary>
        public static string pattern_CreateNewContact {
            get {
                return ResourceManager.GetString("pattern_CreateNewContact", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to h1. CRM. Data Export Successfully Completed
        ///
        ///Please, follow this link to download the archive: &quot;exportdata.zip&quot;:&quot;${EntityRelativeURL}&quot;
        ///
        ///*Note*: this link is valid for 24 hours only.
        ///
        ///If you have any questions or need assistance please feel free to contact us at &quot;support@teamlab.com&quot;:&quot;mailto:support@teamlab.com&quot;
        ///
        ///Best regards,
        ///TeamLab Support Team
        ///&quot;www.teamlab.com&quot;:&quot;http://teamlab.com/&quot;
        ///
        ///^You receive this email because you are a registered user of the &quot;${__VirtualRootPath}&quot;:&quot;${__VirtualRootPath}&quot; [rest of string was truncated]&quot;;.
        /// </summary>
        public static string pattern_ExportCompleted {
            get {
                return ResourceManager.GetString("pattern_ExportCompleted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to h1. CRM. Data Import Successfully Completed
        ///
        ///Go to the &quot;${EntityListTitle}&quot;:&quot;$__VirtualRootPath/${EntityListRelativeURL}&quot; list.
        ///
        ///If you have any questions or need assistance please feel free to contact us at &quot;support@teamlab.com&quot;:&quot;mailto:support@teamlab.com&quot;
        ///
        ///Best regards,
        ///TeamLab Support Team
        ///&quot;www.teamlab.com&quot;:&quot;http://teamlab.com/&quot;
        ///
        ///^You receive this email because you are a registered user of the &quot;${__VirtualRootPath}&quot;:&quot;${__VirtualRootPath}&quot; portal. To change the notification type, please manage  [rest of string was truncated]&quot;;.
        /// </summary>
        public static string pattern_ImportCompleted {
            get {
                return ResourceManager.GetString("pattern_ImportCompleted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to h1.You were Appointed Responsible for the Opportunity: &quot;$EntityTitle&quot;:&quot;${__VirtualRootPath}/products/crm/deals.aspx?id=$EntityID&quot;
        ///
        ///$__DateTime &quot;$__AuthorName&quot;:&quot;$__AuthorUrl&quot; has appointed you responsible for the opportunity: $EntityTitle .
        ///
        ///#if($AdditionalData.get_item(&quot;OpportunityDescription&quot;))
        ///
        ///Opportunity description:
        ///$AdditionalData.get_item(&quot;OpportunityDescription&quot;)
        ///#end
        ///
        ///^You receive this email because you are a registered user of the &quot;${__VirtualRootPath}&quot;:&quot;${__VirtualRootPath}&quot; portal. To  [rest of string was truncated]&quot;;.
        /// </summary>
        public static string pattern_ResponsibleForOpportunity {
            get {
                return ResourceManager.GetString("pattern_ResponsibleForOpportunity", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to h1.Task Assigned to You: $EntityTitle
        ///
        ///$__DateTime &quot;$__AuthorName&quot;:&quot;$__AuthorUrl&quot; has appointed you responsible for the task: $EntityTitle.
        ///#if($AdditionalData.get_item(&quot;TaskCategory&quot;))
        ///
        ///Task category:  $AdditionalData.get_item(&quot;TaskCategory&quot;)
        ///#end
        ///#if($AdditionalData.get_item(&quot;LinkWithContact&quot;))
        ///
        ///Link with contact: $AdditionalData.get_item(&quot;LinkWithContact&quot;)
        ///#end
        ///#if($AdditionalData.get_item(&quot;TaskDescription&quot;))
        ///
        ///Task description:
        ///$AdditionalData.get_item(&quot;TaskDescription&quot;)
        ///#end
        ///
        ///Due date:  [rest of string was truncated]&quot;;.
        /// </summary>
        public static string pattern_ResponsibleForTask {
            get {
                return ResourceManager.GetString("pattern_ResponsibleForTask", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to h1.Access Granted to &quot;$EntityTitle&quot;:&quot;${__VirtualRootPath}/${EntityRelativeURL}&quot;
        ///
        ///$__DateTime &quot;$__AuthorName&quot;:&quot;$__AuthorUrl&quot; has granted you the access to &quot;$EntityTitle&quot;:&quot;${__VirtualRootPath}/${EntityRelativeURL}&quot;.
        ///
        ///
        ///^You receive this email because you are a registered user of the &quot;${__VirtualRootPath}&quot;:&quot;${__VirtualRootPath}&quot; portal. If you don&apos;t want to receive the notifications about the access granted to the CRM items, please manage your &quot;subscription settings&quot;:&quot;$RecipientSubscriptionConfigURL&quot;.^.
        /// </summary>
        public static string pattern_SetAccess {
            get {
                return ResourceManager.GetString("pattern_SetAccess", resourceCulture);
            }
        }
        
        /// <summary>
        
        ///
        ///
        ///$AdditionalData.get_item(&quot;TaskDescription&quot;)
        ///
        
        /// </summary>
        public static string pattern_TaskReminder {
            get {
                return ResourceManager.GetString("pattern_TaskReminder", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;patterns&gt;
        ///  &lt;formatter type=&quot;ASC.Notify.Patterns.NVelocityPatternFormatter, ASC.Common&quot; /&gt;
        ///
        ///  &lt;!--Export is completed--&gt;
        ///  &lt;pattern id=&quot;ExportCompleted&quot; sender=&quot;email.sender&quot;&gt;
        ///    &lt;subject resource=&quot;|subject_ExportCompleted|ASC.Web.CRM.Services.NotifyService.CRMPatternResource,ASC.Web.CRM&quot; /&gt;
        ///    &lt;body styler=&quot;ASC.Notify.Textile.TextileStyler,ASC.Notify.Textile&quot; resource=&quot;|pattern_ExportCompleted|ASC.Web.CRM.Services.NotifyService.CRMPatternResource,ASC.Web.CRM&quot; /&gt;
        ///  &lt;/pattern&gt;
        ///  &lt;pattern id=&quot;Expor [rest of string was truncated]&quot;;.
        /// </summary>
        public static string patterns {
            get {
                return ResourceManager.GetString("patterns", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CRM. New Event Added to $EntityTitle.
        /// </summary>
        public static string subject_AddRelationshipEvent {
            get {
                return ResourceManager.GetString("subject_AddRelationshipEvent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CRM. New Contact Created Using &apos;Website Contact Form&apos;.
        /// </summary>
        public static string subject_CreateNewContact {
            get {
                return ResourceManager.GetString("subject_CreateNewContact", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CRM. Data Export Successfully Completed.
        /// </summary>
        public static string subject_ExportCompleted {
            get {
                return ResourceManager.GetString("subject_ExportCompleted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CRM. Data Import Successfully Completed.
        /// </summary>
        public static string subject_ImportCompleted {
            get {
                return ResourceManager.GetString("subject_ImportCompleted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CRM. You were Appointed as a Responsible Person for the Opportunity: $EntityTitle.
        /// </summary>
        public static string subject_ResponsibleForOpportunity {
            get {
                return ResourceManager.GetString("subject_ResponsibleForOpportunity", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CRM. Task Assigned to You: $EntityTitle.
        /// </summary>
        public static string subject_ResponsibleForTask {
            get {
                return ResourceManager.GetString("subject_ResponsibleForTask", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CRM. Access Granted to $EntityTitle.
        /// </summary>
        public static string subject_SetAccess {
            get {
                return ResourceManager.GetString("subject_SetAccess", resourceCulture);
            }
        }
        
        /// <summary>
        
        /// </summary>
        public static string subject_TaskReminder {
            get {
                return ResourceManager.GetString("subject_TaskReminder", resourceCulture);
            }
        }
    }
}
