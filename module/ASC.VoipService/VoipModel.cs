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

namespace ASC.VoipService
{
    public class Agent
    {
        public Guid Id { get; set; }

        public AnswerType Answer { get; set; }

        public string ClientID { get { return PhoneNumber + PostFix; } }

        public bool Record { get; set; }

        public int TimeOut { get; set; }

        public AgentStatus Status { get; set; }

        public bool AllowOutgoingCalls { get; set; }

        public string PostFix { get; set; }

        public string PhoneNumber { get; set; }

        public string RedirectToNumber { get; set; }

        public Agent()
        {
            Status = AgentStatus.Offline;
            TimeOut = 30;
            AllowOutgoingCalls = true;
            Record = true;
        }

        public Agent(Guid id, AnswerType answer, VoipPhone phone, string postFix)
            : this()
        {
            Id = id;
            Answer = answer;
            PhoneNumber = phone.Number;
            AllowOutgoingCalls = phone.Settings.AllowOutgoingCalls;
            Record = phone.Settings.Record;
            PostFix = postFix;
        }
    }

    public class Queue
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public string WaitUrl { get; set; }
        public int WaitTime { get; set; }

        public Queue(){}
        public Queue(string name, string waitUrl)
        {
            Name = name;
            WaitUrl = waitUrl;
            WaitTime = 30;
            Size = 5;
        }

        public Queue(string id, string name, int size, string waitUrl, int waitTime)
        {
            Id = id;
            Name = name;
            WaitUrl = waitUrl;
            WaitTime = waitTime;
            Size = size;
        }
    }

    public class WorkingHours
    {
        public bool Enabled { get; set; }
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }

        public WorkingHours(){}

        public WorkingHours(TimeSpan from, TimeSpan to)
        {
            From = from;
            To = to;
        }


        protected bool Equals(WorkingHours other)
        {
            return Enabled.Equals(other.Enabled) && From.Equals(other.From) && To.Equals(other.To);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((WorkingHours)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Enabled.GetHashCode();
                hashCode = (hashCode * 397) ^ From.GetHashCode();
                hashCode = (hashCode * 397) ^ To.GetHashCode();
                return hashCode;
            }
        }
    }

    public class VoiceMail
    {

        public string Url { get; set; }

        public bool Enabled { get; set; }

        public int TimeOut { get; set; }

        public VoiceMail()
        {
            TimeOut = 30;
        }

        public VoiceMail(string url, bool enabled)
        {
            Url = url;
            Enabled = enabled;
            TimeOut = 30;
            
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((VoiceMail) obj);
        }

        protected bool Equals(VoiceMail other)
        {
            return string.Equals(Url, other.Url) && Enabled.Equals(other.Enabled) && TimeOut == other.TimeOut;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Url != null ? Url.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Enabled.GetHashCode();
                hashCode = (hashCode * 397) ^ TimeOut;
                return hashCode;
            }
        }
    }

    public class VoipUpload
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public AudioType AudioType { get; set; }
    }

    #region Enum

    public enum AnswerType
    {
        Number,
        Sip,
        Client
    }

    public enum GreetingMessageVoice
    {
        Man,
        Woman,
        Alice
    }

    public enum AgentStatus
    {
        Online,
        Paused,
        Offline
    }

    public enum AudioType
    {
        Greeting,
        HoldUp,
        VoiceMail,
        Queue
    }

    #endregion
}
