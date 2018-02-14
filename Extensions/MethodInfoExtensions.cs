using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;


public static class MethodInfoExtensions
{
    public static bool HasAttribute(this MethodInfo I, Type AttributeType)
    {
        if (!AttributeType.IsAbstract && typeof(Attribute).IsAssignableFrom(AttributeType))
        {
            foreach (object attribute in I.GetCustomAttributes(true))
            {
                if (attribute.GetType() == AttributeType)
                {
                    return true;
                }
            }
            return false;
        }
        else
        {
            throw new Exception("{0} is not a attribute");
        }
    }

    public static AttributeType GetAttribute<AttributeType> (this MethodInfo I) where AttributeType:Attribute 
    {
        if (!typeof(AttributeType).IsAbstract)
        {
            return (AttributeType)I.GetCustomAttributes(typeof(AttributeType),true).FirstOrDefault();
        }
        else
        {
            throw new Exception("{0} is abstract");
        }

    }

}

