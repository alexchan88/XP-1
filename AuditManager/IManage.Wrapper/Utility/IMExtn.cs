using System.ComponentModel;

namespace IM.Wrapper.Utility
{
    public static class IMExtn
    {
        public static string ToEnumDesc<T>(this T value) where T : struct
        {
            if (typeof(T).IsEnum)
            {
                var descAttrs = (DescriptionAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (descAttrs != null && descAttrs.Length > 0)
                    return descAttrs[0].Description;
            }

            return value.ToString();
        }
    }
}
