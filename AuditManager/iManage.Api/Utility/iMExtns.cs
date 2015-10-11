using System;

namespace iManage.Api
{
    public static class iMExtns
    {
        public static U ToEnumAttr<T, U>(this T value)
            where T : struct
            where U : Attribute
        {
            if (typeof(T).IsEnum)
            {
                var attr = (U[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(U), false);

                if (attr != null && attr.Length > 0)
                    return attr[0];
            }

            return null;
        }
    }
}
