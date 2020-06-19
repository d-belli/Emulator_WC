using System;
using System.Runtime.InteropServices;

namespace Emulator
{
    /// <summary>
    /// Commando - size 160
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct P_334
    {
        public SHeader Header;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] Cmd;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 132)]
        public byte[] Msg;

        public string Mensagem
        {
            get => Functions.GetString(this.Msg);
            set
            {
                this.Msg = Config.Encoding.GetBytes(value);
                Array.Resize(ref this.Msg, 16);
            }
        }

        public string Comando
        {
            get => Functions.GetString(this.Cmd);
            set
            {
                this.Cmd = Config.Encoding.GetBytes(value);
                Array.Resize(ref this.Cmd, 16);
            }
        }

        // Construtores
        public static P_334 New(Client client, P_334 cmd)
        {
            P_334 tmp = new P_334
            {
                Header = SHeader.New(0x0334, Marshal.SizeOf<P_334>(), 0),
                Cmd = cmd.Cmd,
                Msg = cmd.Msg
            };
            return tmp;
        }

        public static void controller(Client client, P_334 p334)
        {
            SMob mob = client.Character.Mob;
            switch (p334.Comando)
            {
                case "tab":
                    mob.Tab = p334.Mensagem;
                    client.Send(P_364.New(mob, EnterVision.Normal));
                    break;
                case "getout\0dao": // fim cidadao
                    mob.CityId = 0;
                    client.Send(P_101.New("Você não possui cidadania."));
                    break;
                case "king":
                case "reino":
                    if (mob.CapaReino == 1)
                        P_290.Teleport(client, SPosition.New(1690, 1618));
                    else if (mob.CapaReino == 2)
                        P_290.Teleport(client, SPosition.New(1690, 1842));
                    else
                        P_290.Teleport(client, SPosition.New(1702, 1726));
                    break;
            }
        }
    }
}
