using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp6
{
    [Serializable]
    public class FullMoney
    {
        public enum MoneyTier
        {
            TU = 0, BU, MU, KU, U, tT, bT, mT, kT, T, B, M, K, Units
        }
        private int m_TU = 0;
        private int m_BU = 0;
        private int m_MU = 0;
        private int m_KU = 0;
        private int m_U = 0;
        private int m_tT = 0;
        private int m_bT = 0;
        private int m_mT = 0;
        private int m_kT = 0;
        private int m_T = 0;
        private int m_B = 0;
        private int m_M = 0;
        private int m_K = 0;
        private int m_Units = 0;
        public int TU { get => m_TU; set { m_TU = value; if (m_TU > 999) SetAll(999); if (m_TU < 0) SetAll(0); } }
        public int BU { get => m_BU; set { m_BU = value; if (m_BU > 999) { m_TU += m_BU / 1000; m_BU -= m_BU - m_BU % 1000; } if (m_BU < 0) { if (this >= new FullMoney(1, MoneyTier.TU)) { m_TU--; m_BU += 1000; } else m_BU = 0; } } }
        public int MU { get => m_MU; set { m_MU = value; if (m_MU > 999) { m_BU += m_MU / 1000; m_MU -= m_MU - m_MU % 1000; } if (m_MU < 0) { if (this >= new FullMoney(1, MoneyTier.BU)) { m_BU--; m_MU += 1000; } else m_MU = 0; } } }
        public int KU { get => m_KU; set { m_KU = value; if (m_KU > 999) { m_MU += m_KU / 1000; m_KU -= m_KU - m_KU % 1000; } if (m_KU < 0) { if (this >= new FullMoney(1, MoneyTier.MU)) { m_MU--; m_KU += 1000; } else m_KU = 0; } } }
        public int U { get => m_U; set { m_U = value; if (m_U > 999) { m_KU += m_U / 1000; m_U -= m_U - m_U % 1000; } if (m_U < 0) { if (this >= new FullMoney(1, MoneyTier.KU)) { m_KU--; m_U += 1000; } else m_U = 0; } } }
        public int tT { get => m_tT; set { m_tT = value; if (m_tT > 999) { m_U += m_tT / 1000; m_tT -= m_tT - m_tT % 1000; } if (m_tT < 0) { if (this >= new FullMoney(1, MoneyTier.U)) { m_U--; m_tT += 1000; } else m_tT = 0; } } }
        public int bT { get => m_bT; set { m_bT = value; if (m_bT > 999) { m_tT += m_bT / 1000; m_bT -= m_bT - m_bT % 1000; } if (m_bT < 0) { if (this >= new FullMoney(1, MoneyTier.tT)) { m_tT--; m_bT += 1000; } else m_bT = 0; } } }
        public int mT { get => m_mT; set { m_mT = value; if (m_mT > 999) { m_bT += m_mT / 1000; m_mT -= m_mT - m_mT % 1000; } if (m_mT < 0) { if (this >= new FullMoney(1, MoneyTier.bT)) { m_bT--; m_mT += 1000; } else m_mT = 0; } } }
        public int kT { get => m_kT; set { m_kT = value; if (m_kT > 999) { m_mT += m_kT / 1000; m_kT -= m_kT - m_kT % 1000; } if (m_kT < 0) { if (this >= new FullMoney(1, MoneyTier.mT)) { m_mT--; m_kT += 1000; } else m_kT = 0; } } }
        public int T { get => m_T; set { m_T = value; if (m_T > 999) { m_kT += m_T / 1000; m_T -= m_T - m_T % 1000; } if (m_T < 0) { if (this >= new FullMoney(1, MoneyTier.kT)) { m_kT--; m_T += 1000; } else m_T = 0; } } }
        public int B { get => m_B; set { m_B = value; if (m_B > 999) { m_T += m_B / 1000; m_B -= m_B - m_B % 1000; } if (m_B < 0) { if (this >= new FullMoney(1, MoneyTier.T)) { m_T--; m_B += 1000; } else m_B = 0; } } }
        public int M { get => m_M; set { m_M = value; if (m_M > 999) { m_B += m_M / 1000; m_M -= m_M - m_M % 1000; } if (m_M < 0) { if (this >= new FullMoney(1, MoneyTier.B)) { m_B--; m_M += 1000; } else m_M = 0; } } }
        public int K { get => m_K; set { m_K = value; if (m_K > 999) { m_M += m_K / 1000; m_K -= m_K - m_K % 1000; } if (m_K < 0) { if (this >= new FullMoney(1, MoneyTier.M)) { m_M--; m_K += 1000; } else m_K = 0; } } }
        public int Units { get => m_Units; set { m_Units = value; if (m_Units > 999) { m_K += m_Units / 1000; m_Units -= m_Units - m_Units % 1000; } if (m_Units < 0) { if (this >= new FullMoney(1, MoneyTier.K)) { m_K--; m_Units += 1000; } else m_Units = 0; if (m_Units < 0) m_Units = 0; } } }


        [JsonConstructor]
        public FullMoney()
        {
        }
        public FullMoney(int TU, int BU, int MU, int KU, int U, int tT, int bT, int mT, int kT, int T, int B, int M, int K, int Units)
        {
            this.TU = TU;
            this.BU = BU;
            this.MU = MU;
            this.KU = KU;
            this.U = U;
            this.tT = tT;
            this.bT = bT;
            this.mT = mT;
            this.kT = kT;
            this.T = T;
            this.B = B;
            this.M = M;
            this.K = K;
            this.Units = Units;
        }
        public FullMoney(int count, MoneyTier tier = MoneyTier.Units)
        {
            switch(tier)
            {
                case MoneyTier.TU : TU = count; break;
                case MoneyTier.BU : BU = count; break;
                case MoneyTier.MU : MU = count; break;
                case MoneyTier.KU : KU = count; break;
                case MoneyTier.U : U = count; break;
                case MoneyTier.tT : tT = count; break;
                case MoneyTier.bT : bT = count; break;
                case MoneyTier.mT : mT = count; break;
                case MoneyTier.kT : kT = count; break;
                case MoneyTier.T : T = count; break;
                case MoneyTier.B : B = count; break;
                case MoneyTier.M : M = count; break;
                case MoneyTier.K : K = count; break;
                default:
                case MoneyTier.Units : Units = count; break;
            }
        }
        public FullMoney(FullMoney money)
        {
            TU = money.TU;
            BU = money.BU;
            MU = money.MU;
            KU = money.KU;
            U = money.U;
            tT = money.tT;
            bT = money.bT;
            mT = money.mT;
            kT = money.kT;
            T = money.T;
            B = money.B;
            M = money.M;
            K = money.K;
            Units = money.Units;
        }

        public void SetAll(int count)
        {
            TU = count;
            BU = count;
            MU = count;
            KU = count;
            U = count;
            tT = count;
            bT = count;
            mT = count;
            kT = count;
            T = count;
            B = count;
            M = count;
            K = count;
            Units = count;
        }

        public void Add(FullMoney money)
        {
            Units += money.Units;
            K += money.K;
            M += money.M;
            B += money.B;
            T += money.T;
            kT += money.kT;
            mT += money.mT;
            bT += money.bT;
            tT += money.tT;
            U += money.U;
            KU += money.KU;
            MU += money.MU;
            BU += money.BU;
            TU += money.TU;
        }
        public void Remove(FullMoney money)
        {
            Units -= money.Units;
            K -= money.K;
            M -= money.M;
            B -= money.B;
            T -= money.T;
            kT -= money.kT;
            mT -= money.mT;
            bT -= money.bT;
            tT -= money.tT;
            U -= money.U;
            KU -= money.KU;
            MU -= money.MU;
            BU -= money.BU;
            TU -= money.TU;
        }
        public void Multiply(FullMoney money)
        {
            Units *= money.Units;
            K *= money.K;
            M *= money.M;
            B *= money.B;
            T *= money.T;
            kT *= money.kT;
            mT *= money.mT;
            bT *= money.bT;
            tT *= money.tT;
            U *= money.U;
            KU *= money.KU;
            MU *= money.MU;
            BU *= money.BU;
            TU *= money.TU;
        }
        public void Multiply(int count)
        {
            var _K = K;
            var _M = M;
            var _B = B;
            var _T = T;
            var _kT = kT;
            var _mT = mT;
            var _bT = bT;
            var _tT = tT;
            var _U = U;
            var _KU = KU;
            var _MU = MU;
            var _BU = BU;
            var _TU = TU;

            Units *= count;
            K += _K *count;
            M += _M * count;
            B += _B * count;
            T += _T * count;
            kT += _kT * count;
            mT += _mT * count;
            bT += _bT * count;
            tT += _tT * count;
            U += _U * count;
            KU += _KU * count;
            MU += _MU * count;
            BU += _BU * count;
            TU += _TU * count;
        }
        public bool GreaterThan(FullMoney money)
        {
            if (TU > money.TU) return true;
            if (BU > money.BU) return true;
            if (MU > money.MU) return true;
            if (KU > money.KU) return true;
            if (U > money.U) return true;
            if (tT > money.tT) return true;
            if (bT > money.bT) return true;
            if (mT > money.mT) return true;
            if (kT > money.kT) return true;
            if (T > money.T) return true;
            if (B > money.B) return true;
            if (M > money.M) return true;
            if (K > money.K) return true;
            if (Units > money.Units) return true;
            return false;
        }
        public bool LessThan(FullMoney money)
        {
            if (TU < money.TU) return true;
            if (BU < money.BU) return true;
            if (MU < money.MU) return true;
            if (KU < money.KU) return true;
            if (U < money.U) return true;
            if (tT < money.tT) return true;
            if (bT < money.bT) return true;
            if (mT < money.mT) return true;
            if (kT < money.kT) return true;
            if (T < money.T) return true;
            if (B < money.B) return true;
            if (M < money.M) return true;
            if (K < money.K) return true;
            if (Units < money.Units) return true;
            return false;
        }
        public bool GreaterOrEqualThan(FullMoney money)
        {
            if (ToString() == money.ToString()) return true;

            if(TU > 0 || money.TU > 0) if (TU < money.TU) return false;
            if (BU > 0 || money.BU > 0) if (BU < money.BU) return false;
            if (TU > 0 || money.MU > 0) if (MU < money.MU) return false;
            if (KU > 0 || money.KU > 0) if (KU < money.KU) return false;
            if (U > 0 || money.U > 0) if (U < money.U) return false;
            if (tT > 0 || money.tT > 0) if (tT < money.tT) return false;
            if (bT > 0 || money.bT > 0) if (bT < money.bT) return false;
            if (mT > 0 || money.mT > 0) if (mT < money.mT) return false;
            if (kT > 0 || money.kT > 0) if (kT < money.kT) return false;
            if (T > 0 || money.T > 0) if (T < money.T) return false;
            if (B > 0 || money.B > 0) if (B < money.B) return false;
            if (M > 0 || money.M > 0) if (M < money.M) return false;
            if (K > 0 || money.K > 0) if (K < money.K) return false;
            if(Units > 0 || money.Units > 0) if (Units < money.Units) return false;
            return true;
        }
        public bool LessOrEqualThan(FullMoney money)
        {
            if (TU > money.TU) return false;
            if (BU > money.BU) return false;
            if (MU > money.MU) return false;
            if (KU > money.KU) return false;
            if (U > money.U) return false;
            if (tT > money.tT) return false;
            if (bT > money.bT) return false;
            if (mT > money.mT) return false;
            if (kT > money.kT) return false;
            if (T > money.T) return false;
            if (B > money.B) return false;
            if (M > money.M) return false;
            if (K > money.K) return false;
            if (Units > money.Units) return false;
            return true;
        }

        public override string ToString()
        {
            if (TU > 0) return $"{TU}TU{BU}";
            if (BU > 0) return $"{BU}BU{MU}";
            if (MU > 0) return $"{MU}MU{KU}";
            if (KU > 0) return $"{KU}KU{U}";
            if (U > 0) return $"{U}U{tT}";
            if (tT > 0) return $"{tT}tT{bT}";
            if (bT > 0) return $"{bT}bT{mT}";
            if (mT > 0) return $"{mT}mT{kT}";
            if (kT > 0) return $"{kT}kT{T}";
            if (T > 0) return $"{T}T{B}";
            if (B > 0) return $"{B}B{M}";
            if (M > 0) return $"{M}M{K}";
            if (K > 0) return $"{K}K{Units}";
            return Units.ToString();
        }


        public static FullMoney operator +(FullMoney money, FullMoney other) { var newMoney = new FullMoney(money); newMoney.Add(other); return newMoney; }
        public static FullMoney operator -(FullMoney money, FullMoney other) { var newMoney = new FullMoney(money); newMoney.Remove(other); return newMoney; }
        public static FullMoney operator *(FullMoney money, FullMoney other) { var newMoney = new FullMoney(money); newMoney.Multiply(other); return newMoney; }
        public static FullMoney operator *(FullMoney money, int other) { var newMoney = new FullMoney(money); newMoney.Multiply(other); return newMoney; }
        public static FullMoney operator *(int other, FullMoney money) { var newMoney = new FullMoney(money); newMoney.Multiply(other); return newMoney; }
        public static bool operator >(FullMoney money, FullMoney other) => money.GreaterThan(other);
        public static bool operator <(FullMoney money, FullMoney other) => money.LessThan(other);
        public static bool operator >=(FullMoney money, FullMoney other) => money.GreaterOrEqualThan(other);
        public static bool operator <=(FullMoney money, FullMoney other) => money.LessOrEqualThan(other);
    }
}
