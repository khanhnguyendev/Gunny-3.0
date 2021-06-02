﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;

namespace GameServerScript.AI.NPC
{
    public class MyNpcAi : ABrain
    {
        protected Player m_targer;

        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            m_body.CurrentDamagePlus = 1;
            m_body.CurrentShootMinus = 1;
            if (m_body.IsSay)
            {
                string msg = GetOneChat();
                int delay = Game.Random.Next(0, 5000);
                m_body.Say(msg, 0, delay);
            }
        }

        public override void OnCreated()
        {
            base.OnCreated();
            Body.Direction = -1;
        }

        public override void OnStartAttacking()
        {
            base.OnStartAttacking();
            m_targer = Game.FindNearestPlayer(Body.X, Body.Y);
            Beating();
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }

        public void MoveToPlayer(Player player)
        {
            int dis = (int)player.Distance(Body.X, Body.Y);
            int ramdis = Game.Random.Next(((SimpleNpc)Body).NpcInfo.MoveMin, ((SimpleNpc)Body).NpcInfo.MoveMax);
            if (dis > 97)
            {
                if (dis > ((SimpleNpc)Body).NpcInfo.MoveMax)
                {
                    dis = ramdis;
                }
                else
                {
                    dis = dis - 90;
                }

                if (player.Y < 420 && player.X < 210)
                {
                    if (Body.Y > 420)
                    {
                        if (Body.X - dis < 50)
                        {
                            Body.MoveTo(25, Body.Y, "walk", 1200, new LivingCallBack(Jump));
                        }
                        else
                        {
                            Body.MoveTo(Body.X - dis, Body.Y, "walk", 1200, new LivingCallBack(MoveBeat));
                        }
                    }
                    else
                    {
                        if (player.X > Body.X)
                        {
                            Body.MoveTo(Body.X + dis, Body.Y, "walk", 1200, new LivingCallBack(MoveBeat));
                        }
                        else
                        {
                            Body.MoveTo(Body.X - dis, Body.Y, "walk", 1200, new LivingCallBack(MoveBeat));
                        }
                    }
                }
                else
                {
                    if (Body.Y < 420)
                    {
                        //if (Body.X > 200 && Body.X < 300)
                        //{
                        //    Body.FallFrom(Body.X, Body.Y + 240, null, 0, 0, 12, new LivingCallBack(FallBeat));
                        //}
                        //else
                        //{
                        //    Body.MoveTo(Body.X + dis, Body.Y, "walk", 1200, new LivingCallBack(MoveBeat));
                        //}
                        if (Body.X + dis > 200)
                        {
                            Body.MoveTo(200, Body.Y, "walk", 1200, new LivingCallBack(Fall));
                        }
                    }
                    else
                    {
                        if (player.X > Body.X)
                        {
                            Body.MoveTo(Body.X + dis, Body.Y, "walk", 1200, new LivingCallBack(MoveBeat));
                        }
                        else
                        {
                            Body.MoveTo(Body.X - dis, Body.Y, "walk", 1200, new LivingCallBack(MoveBeat));
                        }
                    }
                }
            }
        }

        public void MoveBeat()
        {
            Body.Beat(m_targer, "beat", 100, 0, 0);
        }

        public void FallBeat()
        {
            Body.Beat(m_targer, "beat", 100, 0, 2000);
        }

        public void Jump()
        {
            Body.Direction = 1;
            Body.JumpTo(Body.X, Body.Y - 240, "Jump", 0, 2, 3, new LivingCallBack(Beating));
        }

        public void Beating()
        {
            if (m_targer != null)
            {
                Player target = Game.FindRandomPlayer();
                //MoveToPlayer(m_targer);
                int rand = random.Next(2, 5);
                switch (rand)
                {
                    case 2:
                        for (int i = 0; i < 3; i++)
                        {
                            Body.ShootPoint(target.X, target.Y, 51, 1000, 10000, 1, 3.0f, 2550);
                        }
                        //Body.ShootPoint(m_targer.X,m_targer.Y,51,500,10000,2,100,3000);
                        Body.PlayMovie("beatA", 1000, 2000);
                        break;
                    case 3:
                        for (int i = 0; i < 2; i++)
                        {
                            Body.ShootPoint(target.X, target.Y, 67, 1000, 10000, 3, 3.0f, 2550);
                        }
                        Body.PlayMovie("beatB", 1000, 2000);
                        break;
                    case 4:
                        for (int i = 0; i < 10; i++)
                        {
                            Body.Shoot(67, random.Next(0, 500), 5, 20, 45, 1, 2000);
                        }
                        Body.PlayMovie("beatC", 1000, 2000);
                        break;
                    case 5:
                        Body.PlayMovie("beatD", 1000, 2000);
                        break;
                    case 6:
                        Body.PlayMovie("beatF", 1000, 2000);
                        break;
                    default:
                        break;
                }
               // Body.ShootImp(51

            }
        }

        public void Fall()
        {
            Body.FallFrom(Body.X, Body.Y + 240, null, 0, 0, 12, new LivingCallBack(Beating));
        }

        #region NPC 小怪说话

        private static Random random = new Random();
        private static string[] listChat = new string[] { 
            "Để tôn vinh! Để giành chiến thắng! !",
            "Tổ chức cướp vũ khí của họ, không run sợ!",
            "Cho vua để chống lại!",
            "Kẻ thù ở phía trước, sẵn sàng chiến đấu!",
            "Cảm thấy hành vi của nhà vua và bất thường hơn ...",
            "Để Boo Goo chiến thắng! ! Brothers phí!",
            "Nhanh chóng để tiêu diệt kẻ thù!",
            "Sức mạnh số!",
            "Với một sửa chữa nhanh chóng!",
            "Vây quanh kẻ thù và tiêu diệt chúng.",
            "Quân tiếp viện! Quân tiếp viện! Chúng tôi cần thêm quân tiếp viện! !",
            "Hy sinh bản thân, sẽ không cho phép bạn có được đi với.",
            "Đừng đánh giá thấp sức mạnh của Boo Goo, nếu không bạn sẽ phải trả cho việc này."
        };

        public static string GetOneChat()
        {
            int index = random.Next(0, listChat.Length);
            return listChat[index];
        }


        /// <summary>
        /// 小怪说话
        /// </summary>
        public static void LivingSay(List<Living> livings)
        {
            if (livings == null || livings.Count == 0)
                return;
            int sayCount = 0;
            int livCount = livings.Count;
            foreach (Living living in livings)
            {
                living.IsSay = false;
            }
            if (livCount <= 5)
            {
                sayCount = random.Next(0, 2);
            }
            else if (livCount > 5 && livCount <= 10)
            {
                sayCount = random.Next(1, 3);
            }
            else
            {
                sayCount = random.Next(1, 4);
            }

            if (sayCount > 0)
            {
                int[] sayIndexs = new int[sayCount];
                for (int i = 0; i < sayCount;)
                {
                    int index = random.Next(0, livCount);
                    if (livings[index].IsSay == false)
                    {
                        livings[index].IsSay = true;
                        int delay = random.Next(0, 5000);
                        livings[index].Say(SimpleNpcAi.GetOneChat(), 0, delay);
                        i++;
                    }
                }
            }
        }

        #endregion
    }
}