using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Emulator
{
    public class Tasks
    {
        public Random _rand { get; private set; }

        public bool Active { get; private set; }

        public List<Client> Client { get; private set; }

        public async void OnTask(List<Client> client)
        {
            this.Client = client;
            this._rand = new Random();

            if (!this.Active)
            {
                this.Active = true;

                //Thread resp = new Thread(new ThreadStart(ExecutaRespawMob));
                //resp.Start();

                //Thread andar = new Thread(new ThreadStart(ExecutaAndarMob));
                //andar.Start();
            }
        }

        private void ExecutaRespawMob()
        {
            while (this.Active)
            {
                if (Client == null)
                    return;

                // Varre todos os cliente
                foreach (Client client in Client)
                {
                    if (client.Character == null || client.Character.Mob.ClientId == 0)
                        return;

                    //Traz somente os mob morto que esteja na visao do client
                    SMobList[] mobList = client.MobView.Where(a => a.Mob.GameStatus.CurHP < 0).ToArray();

                    short p1x = client.Character.Mob.LastPosition.X;
                    short p1y = client.Character.Mob.LastPosition.Y;

                    // Varre todos os mobs mortos que o client matou
                    for (int i = 0; i < mobList.Length; i++)
                    {
                        client.MobView.Remove(mobList[i]);

                        short p2x = mobList[i].Mob.LastPosition.X;
                        short p2y = mobList[i].Mob.LastPosition.Y;

                        double ctOposto = p1x - p2x;
                        double ctAdjacente = p1y - p2y;

                        double hipotenusa = Math.Sqrt((ctOposto * ctOposto) + (ctAdjacente * ctAdjacente));

                        if (hipotenusa < 30)
                        {
                            mobList[i].Mob.GameStatus.CurHP = mobList[i].Mob.GameStatus.MaxHP;
                            client.MobView.Add(mobList[i]);

                            //O mob esta morto mas continua na visao do client, entao da respaw no mob
                            client.Send(P_364.New(mobList[i].Mob, EnterVision.LogIn));
                        }
                        else
                        {
                            //O mob esta morto e nao esta na visao do client
                            client.MobView.Remove(mobList[i]);
                        }
                    }
                }
                // Nasce os mobs a cada 10s
                Thread.Sleep(10000);
            }
        }

        private async Task ExecutaSkill()
        {
            foreach (Client item in Client)
            {

            }

            // Executa skill do client a cada 8s
            await Task.Delay(8000);
        }

        public void ExecutaAndarMob()
        {
            while (this.Active)
            {
                for (int i = 0; i < Client.Count(); i++)
                {
                    for (int a = 0; a < Client[i].MobView.Where(b => b.Mob.Merchant == 0).Count(); a++)
                    {
                        //if que decide se o mob anda ou não (50% de chance de andar)
                        if (_rand.Next(0, 100) > 50)
                        {
                            SMobList mob = Client[i].MobView[a];
                            int x, y;

                            //if para evitar erro do random
                            if (mob.PositionInicial.X < mob.PositionFinal.X)
                                x = _rand.Next(mob.PositionInicial.X, mob.PositionFinal.X);
                            else
                                x = _rand.Next(mob.PositionFinal.X, mob.PositionInicial.X);

                            if (mob.PositionInicial.Y < mob.PositionFinal.Y)
                                y = _rand.Next(mob.PositionInicial.Y, mob.PositionFinal.Y);
                            else
                                y = _rand.Next(mob.PositionFinal.Y, mob.PositionInicial.Y);


                            // Prepara o pacote de andar dos mobs / npc
                            P_36C p36c = P_36C.New(mob.Mob.ClientId, mob.PositionInicial, SPosition.New(x, y), 0, mob.Speed, new byte[12]);

                            // Envia o pacote de andar
                            Client[i].Send(p36c);
                        }
                    }
                }
                // Mob / Npc anda a cada 5s
                Thread.Sleep(5000);
            }
        }
    }
}
