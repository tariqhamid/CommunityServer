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
using System.Web;
using ASC.MessagingSystem;
using AjaxPro;
using ASC.Core;
using ASC.Core.Users;
using ASC.Web.Studio.Core.Notify;
using ASC.Web.Studio.Core.Users;

namespace ASC.Web.Studio.Core
{
    [AjaxNamespace("EmailOperationService")]
    public class EmailOperationService
    {
        public class InvalidEmailException : Exception
        {
            public InvalidEmailException()
            {
            }

            public InvalidEmailException(string message) : base(message)
            {
            }
        }

        public class AccessDeniedException : Exception
        {
            public AccessDeniedException()
            {
            }

            public AccessDeniedException(string message) : base(message)
            {
            }
        }

        public class UserNotFoundException : Exception
        {
            public UserNotFoundException()
            {
            }

            public UserNotFoundException(string message) : base(message)
            {
            }
        }

        public class InputException : Exception
        {
            public InputException()
            {
            }

            public InputException(string message) : base(message)
            {
            }
        }

        /// <summary>
        /// Sends the email activation instructions to the specified email
        /// </summary>
        /// <param name="userID">The ID of the user who should activate the email</param>
        /// <param name="email">Email</param>
        /// <param name="changeCurrentEmail">Defines whether must change the current user email to specified. 
        ///  The using of this option is available for admin only</param>
        [AjaxMethod]
        public string SendEmailActivationInstructions(Guid userID, string email)
        {
            if (userID == Guid.Empty) throw new ArgumentNullException("userID");
            if (String.IsNullOrEmpty(email)) throw new ArgumentNullException(Resources.Resource.ErrorEmailEmpty);
            if (!email.TestEmailRegex()) throw new InvalidEmailException(Resources.Resource.ErrorNotCorrectEmail);

            try
            {
                var viewer = CoreContext.UserManager.GetUsers(SecurityContext.CurrentAccount.ID);
                var user = CoreContext.UserManager.GetUsers(userID);

                if (user == null) throw new UserNotFoundException(Resources.Resource.ErrorUserNotFound);

                if (viewer == null) throw new AccessDeniedException(Resources.Resource.ErrorAccessDenied);

                if (viewer.IsAdmin() || viewer.ID == user.ID)
                {
                    var existentUser = CoreContext.UserManager.GetUserByEmail(email);
                    if (existentUser.ID != ASC.Core.Users.Constants.LostUser.ID && existentUser.ID != userID)
                        throw new InputException(CustomNamingPeople.Substitute<Resources.Resource>("ErrorEmailAlreadyExists"));

                    user.Email = email;
                    if (user.ActivationStatus == EmployeeActivationStatus.Activated)
                    {
                        user.ActivationStatus = EmployeeActivationStatus.NotActivated;
                    }
                    CoreContext.UserManager.SaveUserInfo(user);
                }
                else
                {
                    email = user.Email;
                }

                if (user.ActivationStatus == EmployeeActivationStatus.Pending)
                {
                    if (user.IsVisitor())
                    {
                        StudioNotifyService.Instance.GuestInfoActivation(user);
                    }
                    else
                    {
                        StudioNotifyService.Instance.UserInfoActivation(user);
                    }
                }
                else
                {
                    StudioNotifyService.Instance.SendEmailActivationInstructions(user, email);
                }

                MessageService.Send(HttpContext.Current.Request, MessageAction.UserSentActivationInstructions, user.DisplayUserName(false));

                return String.Format(Resources.Resource.MessageEmailActivationInstuctionsSentOnEmail, "<b>" + email + "</b>");
            }
            catch(UserNotFoundException)
            {
                throw;
            }
            catch(AccessDeniedException)
            {
                throw;
            }
            catch(InputException)
            {
                throw;
            }
            catch(Exception)
            {
                throw new Exception(Resources.Resource.UnknownError);
            }
        }

        /// <summary>
        /// Sends the email change instructions to the specified email
        /// </summary>
        /// <param name="userID">The ID of the user who is changing the email</param>
        /// <param name="email">Email</param>
        [AjaxMethod]
        public string SendEmailChangeInstructions(Guid userID, string email)
        {
            if (userID == Guid.Empty)
                throw new ArgumentNullException("userID");

            if (String.IsNullOrEmpty(email))
                throw new Exception(Resources.Resource.ErrorEmailEmpty);

            if (!email.TestEmailRegex())
                throw new Exception(Resources.Resource.ErrorNotCorrectEmail);

            try
            {
                UserInfo viewer = CoreContext.UserManager.GetUsers(SecurityContext.CurrentAccount.ID);
                UserInfo user = CoreContext.UserManager.GetUsers(userID);

                if (user == null)
                    throw new UserNotFoundException(Resources.Resource.ErrorUserNotFound);

                if (viewer == null)
                    throw new AccessDeniedException(Resources.Resource.ErrorAccessDenied);

                if (!viewer.IsAdmin())
                {
                    email = user.Email;
                    StudioNotifyService.Instance.SendEmailChangeInstructions(user, email);
                }

                if (viewer.IsAdmin())
                {
                    if (email == user.Email)
                        throw new InputException(Resources.Resource.ErrorEmailsAreTheSame);

                    UserInfo existentUser = CoreContext.UserManager.GetUserByEmail(email);
                    if (existentUser.ID != ASC.Core.Users.Constants.LostUser.ID)
                        throw new InputException(CustomNamingPeople.Substitute<Resources.Resource>("ErrorEmailAlreadyExists"));

                    user.Email = email;
                    user.ActivationStatus = EmployeeActivationStatus.NotActivated;
                    CoreContext.UserManager.SaveUserInfo(user);
                    StudioNotifyService.Instance.SendEmailActivationInstructions(user, email);
                }

                MessageService.Send(HttpContext.Current.Request, MessageAction.UserSentEmailChangeInstructions, user.DisplayUserName(false));

                return String.Format(Resources.Resource.MessageEmailChangeInstuctionsSentOnEmail, "<b>" + email + "</b>");
            }
            catch(AccessDeniedException ex)
            {
                throw ex;
            }
            catch(UserNotFoundException ex)
            {
                throw ex;
            }
            catch(InputException ex)
            {
                throw ex;
            }
            catch(Exception)
            {
                throw new Exception(Resources.Resource.UnknownError);
            }
        }
    }
}