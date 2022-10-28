using System;

namespace WindowsFormsApp4
{
    public partial class Form1
    {
        private void TimerUpdate_Tick(object sender, EventArgs e)
        {
            if (Data.RollingDices == false)
            {
                if (Data.MovingPion)
                {
                    MovePion();
                }
                else
                {
                    Data.Pions[Data.Turn].Update();
                }
            }
            else
            {
                if (Data.DiceRollTime == 0)
                {
                    if(Data.DiceRollTimeMax >= 30)
                    {
                        Console.WriteLine($"Dices rolled, resulting' {Data.ResultDices}' for pion '{Data.Pions[Data.Turn].ID}'");
                        Data.DiceRollTimeMax = 1F;
                        Data.DiceRollTime = 0;
                        Data.EndRollingDices = true;
                        Data.MovingPion = true;
                        Data.t = 0F;

                        int Count = Data.ResultDices;
                        int countUntilEnd = 0;
                        Card untilEnd = Data.Pions[Data.Turn].Card;
                        while (untilEnd != Data.Carte.EndCard && countUntilEnd <= Count)
                        {
                            untilEnd = untilEnd.Next;
                            countUntilEnd++;
                        }
                        if (Count > countUntilEnd) Count = countUntilEnd;
                        Card card = Data.Pions[Data.Turn].Card;
                        for (int i = 0; i < Count; i++)
                            card = card.Next;
                        Data.TargetCardForMovePion = card;
                    }
                    else
                    {
                        int pv;
                        for (int i = 0; i < Data.Dices.Count; i++)
                        {
                            pv = Data.Dices[i];
                            do { Data.Dices[i] = Data.RND.Next(1, 7); } while (Data.Dices[i] == pv);
                        }
                        Data.DiceRollTime++;
                    }
                }
                else
                {
                    Data.DiceRollTime++;
                    if (Data.DiceRollTime >= Data.DiceRollTimeMax)
                    {
                        Data.DiceRollTime = 0;
                        Data.DiceRollTimeMax = Data.DiceRollTimeMax * 1.2F;
                    }
                }
            }
        }

        public static void MovePion()
        {
            Pion Pion = Data.Pions[Data.Turn];
            Data.Pions[Data.Turn].X = (int)Tools.Lerp(Pion.Card.Coord.X * Data.TileSz, Pion.Card.Next.Coord.X * Data.TileSz, Data.t);
            Data.Pions[Data.Turn].Y = (int)Tools.Lerp(Pion.Card.Coord.Y * Data.TileSz, Pion.Card.Next.Coord.Y * Data.TileSz, Data.t);
            if (Data.t >= 1F)
            {
                Pion.Card = Pion.Card.Next;
                if (Pion.Card != Data.TargetCardForMovePion)
                {
                    Data.t = 0F;
                }
                else
                {
                    Data.MovingPion = false;
                }
            }
            else
            {
                Data.t += 1.25F / Data.TileSz;
            }
        }
    }
}
