using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;

namespace Debug
{
    public static class SourceAlgorithm
    {
        public static bool Original(string[] value)
        {
            var target = new CrackMe.Form1();
            var res = (bool?) typeof(CrackMe.Form1)
                .GetMethod(
                    "eval_a",
                    BindingFlags.Instance | BindingFlags.NonPublic,
                    null,
                    CallingConventions.Any,
                    new [] { typeof(string[]) },
                    null)
                ?.Invoke(target, new[] { value });

            if (res == null)
            {
                throw new MissingMethodException("Method eval_a not exists.");
            }

            return res.Value;
        }

        public static bool Decompiled(string[] A_0_1)
        {
            int num1;
            // ISSUE: variable of a compiler-generated type
            // Form1.eval_a evalA;

            byte[] a;
            int[] b;

            NetworkInterface networkInterface;
            switch (0)
            {
                case 0:
                label_2:
                    // ISSUE: object of a compiler-generated type is created
                    // evalA = new Form1.eval_a();
                    networkInterface = ((IEnumerable<NetworkInterface>)NetworkInterface.GetAllNetworkInterfaces()).FirstOrDefault<NetworkInterface>();
                    num1 = 0;
                    goto default;
                default:
                    while (true)
                    {
                        switch (num1)
                        {
                            case 0:
                                switch (true ? 1 : 0)
                                {
                                    case 0:
                                    case 2:
                                        goto label_8;
                                    default:
                                        if (false)
                                            ;
                                        if (true)
                                            ;
                                        num1 = networkInterface != null ? 1 : 2;
                                        continue;
                                }
                            case 1:
                                goto label_7;
                            case 2:
                            label_8:
                                num1 = 3;
                                continue;
                            case 3:
                                goto label_9;
                            default:
                                goto label_2;
                        }
                    }
                label_7:
                    byte[] addressBytes = networkInterface.GetPhysicalAddress().GetAddressBytes();
                    // ISSUE: reference to a compiler-generated field
                    a = BitConverter.GetBytes(DateTime.Now.Date.ToBinary());
                    // ISSUE: reference to a compiler-generated method
                    // Func<byte, int, int> selector1 = new Func<byte, int, int>(evalA.eval_a);
                    int[] array = addressBytes.Select(delegate (byte A_0, int A_1)
                    {
                        //Discarded unreachable code: IL_0040, IL_005f
                        int num10 = 7823;
                        int num11 = num10;
                        num10 = 7823;
                        switch (num11 == num10)
                        {
                            default:
                                if (true)
                                {
                                }
                                if (false)
                                {
                                }
                                num10 = 0;
                                _ = num10;
                                return A_0 ^ a[A_1];
                        }
                    }).Select(delegate (int A_0)
                    {
                        //Discarded unreachable code: IL_004c, IL_0076
                        if (A_0 <= 999)
                        {
                            int num8 = 17361;
                            int num9 = num8;
                            num8 = 17361;
                            switch (num9 == num8)
                            {
                                default:
                                    if (false)
                                    {
                                    }
                                    num8 = 0;
                                    _ = num8;
                                    if (true)
                                    {
                                    }
                                    return A_0 * 10;
                                case false:
                                case true:
                                    break;
                            }
                        }
                        return A_0;
                    }).ToArray();
                    // ISSUE: reference to a compiler-generated field
                    b = ((IEnumerable<string>)A_0_1).Select<string, int>(new Func<string, int>(int.Parse)).ToArray<int>();
                    // ISSUE: reference to a compiler-generated method
                    //Func<int, int, int> selector2 = new Func<int, int, int>(evalA.eval_a);
                    return ((IEnumerable<int>)array).Select(delegate (int A_0, int A_1)
                    {
                        //Discarded unreachable code: IL_0040, IL_005f
                        int num6 = -18476;
                        int num7 = num6;
                        num6 = -18476;
                        switch (num7 == num6)
                        {
                            default:
                                if (true)
                                {
                                }
                                if (false)
                                {
                                }
                                num6 = 0;
                                _ = num6;
                                return A_0 - b[A_1];
                        }
                    }).All(delegate (int A_0)
                    {
                        //Discarded unreachable code: IL_0040, IL_005f
                        int num4 = 862;
                        int num5 = num4;
                        num4 = 862;
                        switch (num5 == num4)
                        {
                            default:
                                if (true)
                                {
                                }
                                if (false)
                                {
                                }
                                num4 = 0;
                                _ = num4;
                                return A_0 == 0;
                        }
                    });
                label_9:
                    return false;
            }
        }
    }
}
