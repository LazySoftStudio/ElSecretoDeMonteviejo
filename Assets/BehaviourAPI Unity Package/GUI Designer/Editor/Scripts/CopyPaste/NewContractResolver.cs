#if HAS_NEWTONSOFT
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BehaviourAPI.UnityToolkit.GUIDesigner.Editor
{
    public class NewContractResolver:DefaultContractResolver
    {
        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            List<MemberInfo> members = new List<MemberInfo>();
           members.AddRange(base.GetSerializableMembers(objectType));

            // Find private fields marked with [SerializeField] and add them
            var fields = objectType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                if (field.IsDefined(typeof(SerializeField), true))
                {
                    // Ensure we don't add duplicates
                    if (!members.Contains(field))
                    {
                        members.Add(field);
                    }
                }
            }
            return members;
        }
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            // Force private [SerializeField] fields to be serializable and readable
            if (member is FieldInfo field && field.IsDefined(typeof(SerializeField), true))
            {
                property.Writable = true;
                property.Readable = true;
            }
            if (typeof(Delegate).IsAssignableFrom(property.PropertyType))
            {
                property.ShouldSerialize = _ => false;
                property.ShouldDeserialize = _ => false;
                property.Ignored = true;
            }
            return property;
        }
    }

}
#endif