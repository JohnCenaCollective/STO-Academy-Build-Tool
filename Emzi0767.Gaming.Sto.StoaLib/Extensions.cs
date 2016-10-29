using System;

namespace Emzi0767.Gaming.Sto.StoaLib
{
    internal static class Extensions
    {
        internal static TAttribute GetAttributeFromEnum<TEnum, TAttribute>(this TEnum enum_value) 
            where TEnum : struct, IConvertible 
            where TAttribute : Attribute
        {
            try
            {
                var t1 = typeof(TEnum);
                var t2 = typeof(TAttribute);

                if (!t1.IsEnum)
                    throw new InvalidOperationException("TEnum must be an enumerated type");

                var mi = t1.GetMember(enum_value.ToString());
                var at = (TAttribute[])mi[0].GetCustomAttributes(t2, false);

                return at[0];
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
