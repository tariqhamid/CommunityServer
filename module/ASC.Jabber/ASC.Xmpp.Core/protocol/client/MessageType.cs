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

namespace ASC.Xmpp.Core.protocol.client
{
    /// <summary>
    ///   Enumeration that represents the type of a message
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        ///   This in a normal message, much like an email. You dont expect a fast
        /// </summary>
        normal = -1,

        /// <summary>
        ///   a error messages
        /// </summary>
        error,

        /// <summary>
        ///   is for chat like messages, person to person. Send this if you expect a fast reply. reply or no reply at all.
        /// </summary>
        chat,

        /// <summary>
        ///   is used for sending/receiving messages from/to a chatroom (IRC style chats)
        /// </summary>
        groupchat,

        /// <summary>
        ///   Think of this as a news broadcast, or RRS Feed, the message will normally have a URL and Description Associated with it.
        /// </summary>
        headline
    }
}