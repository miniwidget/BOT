using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;

namespace Tdm
{
    internal class Gun
    {

        #region primary
        readonly List<string> TYPES = new List<string>() { "ap", "ag", "ar", "sm", "lm", "sg", "sn" };
        int[] RANDOM_MAX = new[] { 3, 5, 9, 5, 4, 5, 5, 6 };
        int[] RANDOM_INT = new[] { 0, 0, 0, 0, 0, 0, 0, 0 };

        public Gun(int[] rnd_int)
        {
            RANDOM_INT = rnd_int;
        }

        int RandomInt(int idx)
        {
            if (RANDOM_INT[idx] != RANDOM_MAX[idx]) return ++RANDOM_INT[idx];

            return RANDOM_INT[idx] = 0;
        }

        internal void GiveWeaponByType(Entity player, string wep)
        {
            int i = TYPES.IndexOf(wep);
            GiveWeaponTo(player, BaseGunByType(i, RandomInt(i)));
        }
        string BaseGunByType(int j, int i)
        {
            if (j == 0) return AP_LIST[i];
            if (j == 1) return AG_LIST[i];
            if (j == 2) return AR_LIST[i] + GetCamo;
            if (j == 3) return SM_LIST[i] + GetCamo;
            if (j == 4) return LM_LIST[i] + GetCamo;
            if (j == 5) return SG_LIST[i] + GetCamo;

            return SN_LIST[i] + GetCamo;
        }

        internal void GiveWeaponByNum(Entity player, string gunType, int num)
        {
            GiveWeaponTo(player, BaseGunByNum(TYPES.IndexOf(gunType), num));
        }
        string BaseGunByNum(int j, int i)
        {
            if (j == 0) { if (i > 3) i = 0; return AP_LIST[i]; }
            if (j == 1) { if (i > 5) i = 0; return AG_LIST[i]; }
            if (j == 2) { if (i > 9) i = 0; return AR_LIST[i] + GetCamo; }
            if (j == 3) { if (i > 5) i = 0; return SM_LIST[i] + GetCamo; }
            if (j == 4) { if (i > 4) i = 0; return LM_LIST[i] + GetCamo; }
            if (j == 5) { if (i > 5) i = 0; return SG_LIST[i] + GetCamo; }

            if (i > 5) i = 0; return SN_LIST[i] + GetCamo;
        }

        internal void GiveWeaponTo(Entity player, string weapon)
        {
            player.TakeWeapon(player.CurrentWeapon);
            player.GiveWeapon(weapon);
            player.Call(33523, weapon);//givemaxammo
            player.SwitchToWeaponImmediate(weapon);

            Tdm.H_FIELD[player.EntRef].GUN = weapon;
        }
        internal void GiveGunTo(Entity player)
        {
            string weapon = Tdm.H_FIELD[player.EntRef].GUN;
            player.TakeWeapon(player.CurrentWeapon);
            player.GiveWeapon(weapon);
            player.Call(33523, weapon);//givemaxammo
            player.SwitchToWeaponImmediate(weapon);
        }
        internal string GetInitialWeapon()
        {
            int i = RandomInt(7);

            return BaseGunByType(i, RandomInt(i));
        }
        #endregion

        #region weapon list

        int _camo;
        string GetCamo
        {
            get
            {
                _camo++;

                if (_camo == 12) _camo = 1;

                if (_camo < 10) return "_camo0" + _camo;

                return "_camo" + _camo;
            }
        }

        readonly string[] AP_LIST = new string[4] { "iw5_fmg9_mp_akimbo", "iw5_skorpion_mp_akimbo", "iw5_mp9_mp_akimbo", "iw5_g18_mp_akimbo", };
        readonly string[] AG_LIST = new string[6] { "iw5_mp412_mp_akimbo", "iw5_p99_mp_akimbo", "iw5_44magnum_mp_akimbo", "iw5_usp45_mp_akimbo", "iw5_fnfiveseven_mp_akimbo", "iw5_deserteagle_mp_akimbo" };
        readonly string[] AR_LIST = new string[10] { "iw5_ak47_mp_gp25", "iw5_m16_mp_gl", "iw5_m4_mp_gl", "iw5_fad_mp_m320", "iw5_acr_mp_m320", "iw5_type95_mp_m320", "iw5_mk14_mp_m320", "iw5_scar_mp_m320", "iw5_g36c_mp_m320", "iw5_cm901_mp_m320", };
        readonly string[] SM_LIST = new string[6] { "iw5_mp5_mp", "iw5_m9_mp", "iw5_p90_mp", "iw5_pp90m1_mp", "iw5_ump45_mp", "iw5_mp7_mp", };
        readonly string[] LM_LIST = new string[5] { "iw5_m60_mp", "iw5_mk46_mp", "iw5_pecheneg_mp", "iw5_sa80_mp", "iw5_mg36_mp" };
        readonly string[] SG_LIST = new string[6] { "iw5_ksg_mp", "iw5_spas12_mp", "iw5_aa12_mp", "iw5_striker_mp", "iw5_1887_mp", "iw5_usas12_mp", };
        readonly string[] SN_LIST = new string[6] { "iw5_dragunov_mp_dragunovscope", "iw5_msr_mp_msrscope", "iw5_barrett_mp_barrettscope", "iw5_rsass_mp_rsassscope", "iw5_as50_mp_as50scope", "iw5_l96a1_mp_l96a1scope", };

        //string[] CAMO_LIST = new string[11] { "_camo01", "_camo02", "_camo03", "_camo04", "_camo05", "_camo06", "_camo07", "_camo08", "_camo09", "_camo10", "_camo11" };

        readonly List<string[]> VIEWS_List = new List<string[]>()
        {
            null,//ap
            null,//ag
            new []{"", "reflex", "acog","thermal","eotech","hybrid" },//ar
            new [] { "", "reflexsmg", "eotechsmg", "hamrhybrid"},//sm
            new [] { "", "reflexlmg","eotechlmg","acog","thermal"},//lm
            new [] { "", "reflex","eotech"},//sg
            new [] { "", "", "acog","thermal"}//sn
        };
        List<string[]> ATTACHES = new List<string[]>()
        {
            new [] {"akimbo", "silencer02","xmags"},//ap
            new [] { "akimbo","xmags"},//ag
            new [] { "heartbeat","launcher","xmags",},//ar "m320","gp25","gl"
            new [] { "rof","xmags",},//sm
            new [] {  "rof","grip","heartbeat","xmags"},//lm
            new [] { "xmags" },//sg
            new [] {"heartbeat","xmags"},//sn
        };
        readonly string[] ALL_GUN_NAME =
        {
            "fmg9", "skorpion", "mp9", "g18",//AP 0~3
            "mp412", "p99", "44magnum", "usp45", "fnfiveseven", "deserteagle", //AG 4~9
            "ak47", "m16", "m4", "fad", "acr", "type95", "mk14", "scar", "g36c", "cm901",//AR 10~19
            "mp5", "m9", "p90", "pp90m1", "ump45", "mp7",//SM 20~25
            "m60", "mk46", "pecheneg", "sa80", "mg36",//LM 26~30
            "ksg","spas12", "aa12", "striker", "1887", "usas12",//SG 31~36
            "dragunov", "msr", "barrett", "rsass", "as50", "l96a1",//SN 37~42
        };
        readonly string[] SILENCERS = { "silencer02", null, "silencer", "silencer", "silencer", "silencer03", "silencer03" };

        #endregion

        #region attachment
        internal void GiveOtherTypes(Entity player, string type, string gun)
        {
            if (gun[2] != '5') return;

            if (type == "vs") GiveViewscope(player, gun);
            else if (type == "at") GiveAttachment(player, gun);
            else GiveSilencer(player, gun);
        }
        void GiveViewscope(Entity player, string gun)
        {
            if (gun[2] != '5') return;

            string baseGun = gun.Split('_')[1];
            int type = GetGunType(baseGun);

            string[] views = VIEWS_List[type];

            if (views == null)//ap , ag
            {
                player.Call(33344, "NOT APPLIED TO THIS GUN");
                return;
            }
            int splitPoint = gun.IndexOf("_mp_");
            if (splitPoint == -1)
            {
                if (type != 6) GiveWeaponTo(player, BuildGunName(gun, views[1]));
                else GiveWeaponTo(player, BuildGunName(gun, baseGun + views[1]));

                player.Call(33344, "^2[  ^7" + views[1].ToUpper() + "  ^2]");

                return;
            }
            else splitPoint += 3;

            string attachment = gun.Substring(splitPoint + 1);
            string[] attaches = attachment.Split('_');

            string oldView = null;
            string newView = null;
            gun = gun.Substring(0, splitPoint);

            //Console.WriteLine("GiveViewscope # baseGun: " + gun);
            //Console.WriteLine("---------------------------------");

            if (type == 6)//in case of sniper
            {
                views[0] = baseGun + "scope";
                views[1] = baseGun + "scopevz";
            }

            oldView = GetCurrentViewscope(attaches, type, baseGun);
            int view_idx = Array.IndexOf(views, oldView);
            //Console.WriteLine(oldView + " " + view_idx);
            if (view_idx == -1)//viewscope 없는 경우
            {
                newView = views[1];

                int attachesLenth = attaches.Length;

                if (attaches.Contains("camo")) attachesLenth -= 1;

                if (attachesLenth < 3)
                {
                    attachment += "_" + newView;
                }
                else
                {
                    oldView = attaches.LastOrDefault(s => !s.Contains("camo"));
                    attachment = attachment.Replace(oldView, newView);
                }
            }
            else
            {
                int max = views.Length - 1;

                if (view_idx == max) view_idx = -1;

                newView = views[++view_idx];

                attachment = attachment.Replace(oldView, newView);
            }

            player.Call(33344, "^2[  ^7" + newView.ToUpper() + "  ^2]");

            GiveWeaponTo(player, BuildGunName(gun, attachment));

        }
        void GiveAttachment(Entity player, string gun)
        {

            string baseGun = gun.Split('_')[1];
            int type = GetGunType(baseGun);

            if (type == 5)//sg
            {
                player.Call(33344, "NOT APPLIED TO THIS GUN");
                return;
            }

            #region 첨가물 자르기
            string attachment = null;
            int splitPoint = gun.IndexOf("_mp_");
            string[] ATTS = ATTACHES[type];

            if (splitPoint == -1)//아무런 첨가물이 없는 경우
            {
                if (ATTS.Length == 1) attachment = ATTS[0]; else attachment = ATTS[1];

                GiveWeaponTo(player, BuildGunName(gun, attachment));
                player.Call(33344, attachment.ToUpper());
                return;
            }
            else splitPoint += 3;

            attachment = gun.Substring(splitPoint + 1);
            string[] attaches = attachment.Split('_');
            #endregion


            string oldAtt = null;
            string newAtt = null;
            int length = attaches.Length;
            if (attaches.Contains("camo")) length -= 1;
            gun = gun.Substring(0, splitPoint);

            if (type < 2)//ap ag 
            {
                //Console.WriteLine("-------------------------------");

                oldAtt = attaches.LastOrDefault();
                int oldAttIdx = Array.IndexOf(ATTS, oldAtt);
                int max = 1; if (type == 0) max = 2;

                //Console.WriteLine("■ oldAtt: " + oldAtt + " ■ oldAttIdx: " + oldAttIdx + " ■ max: " + max);

                if (oldAttIdx >= max)
                {
                    GiveWeaponTo(player, BuildGunName(gun, ATTS[0]));
                    //Console.WriteLine("■ ATTS[0] " + max);
                }
                else
                {
                    //new [] {"xmags","akimbo", "silencer02"},
                    //new[] { "xmags", "akimbo" },
                    attachment += "_" + ATTS[++oldAttIdx];

                    //Console.WriteLine("■ gun: " + gun + " ■ attachment: " + attachment);

                    GiveWeaponTo(player, BuildGunName(gun, attachment));

                }
                //Console.WriteLine("-------------------------------");
                return;
            }


            //Console.WriteLine("GiveAttachment # baseGun: " + gun);
            //Console.WriteLine("---------------------------------");

            if (type == 2) ATTS[1] = GetArLauncher(gun);//update AR launcher

            var results = attaches.Where(a => ATTS.Contains(a)).ToList();

            //int num = 0;
            //foreach (string r in results) Console.WriteLine("results " + (++num) + " : " + r);

            string viewScope = GetCurrentViewscope(attaches, type, null);
            string silencer = GetCurrentSilencer(attaches);

            if (silencer != null) results.Insert(0, silencer);
            if (viewScope != null) results.Insert(0, viewScope);

            //Console.WriteLine("VS: " + viewScope);
            //Console.WriteLine("SL: " + silencer);

            //num = 0;
            //foreach (string r in results) Console.WriteLine("results " + (++num) + " : " + r);

            oldAtt = ATTS.LastOrDefault(s => results.Contains(s));

            //Console.WriteLine("OLDATT 는 " + oldAtt + " 입니다");

            if (oldAtt != null)
            {
                int attIdx = Array.IndexOf(ATTS, oldAtt); if (attIdx == ATTS.Length - 1) attIdx = -1;
                newAtt = ATTS[++attIdx];
                //Console.WriteLine("NEWATT 는 " + newAtt + " 입니다");
            }
            else
            {
                newAtt = ATTS[0];
                //Console.WriteLine("첨가물이 없는 상태입니다. NEWATT는 " + newAtt + " 입니다");
            }

            if (results.Count < 3)//etc 삽입
            {
                if (!results.Contains(newAtt)) results.Add(newAtt);

                else results.Add(ATTS.FirstOrDefault(s => !results.Contains(s)));

                //Console.WriteLine("첨가물이 3보다 작습니다. 총 첨가물은 " + string.Join("_", results) + " 입니다");
            }
            else//etc 삽입
            {
                for (int i = results.Count - 1; i != -1; i--)
                {
                    string s = results[i];
                    //Console.Write("■ " + s);
                    if (ATTS.Contains(s))
                    {
                        results.RemoveAt(i);
                        //Console.Write(" 제거됨 ");
                    }
                }
                results.Add(newAtt);
                //Console.Write(" 추가됨 : " + newAtt);
            }

            attachment = string.Join("_", results);

            //Console.WriteLine("");
            //Console.WriteLine("최종 첨가물은 " + attachment + "입니다");
            player.Call(33344, "^2[ ^7" + attachment.ToUpper().Replace("_", "^2 | ^7") + " ^2]");//   ATT1_ATT2_ATT3 ATT1 | ATT2 | ATT3
            GiveWeaponTo(player, BuildGunName(gun, attachment));

        }
        void GiveSilencer(Entity player, string gun)
        {

            string baseGun = gun.Split('_')[1];
            int type = GetGunType(baseGun);

            string silencer = SILENCERS[type];
            if (silencer == null)
            {
                player.Call(33344, "NOT APPLIED TO THIS GUN");
                return;
            }

            int splitPoint = gun.IndexOf("_mp_");
            if (splitPoint == -1)
            {
                GiveWeaponTo(player, BuildGunName(gun, silencer));

                return;
            }
            else splitPoint += 3;

            string attachment = gun.Substring(splitPoint + 1);

            string[] attaches = attachment.Split('_');
            int length = attaches.Length;
            if (attaches.Contains("camo")) length -= 1;

            gun = gun.Substring(0, splitPoint);

            string oldSilencer = GetCurrentSilencer(attaches);

            if (oldSilencer == null)
            {
                if (attaches.Length < 3)
                {
                    attachment += "_" + silencer;
                }
                else
                {
                    oldSilencer = attaches.LastOrDefault(s => !s.Contains("camo"));

                    attachment = attachment.Replace(oldSilencer, silencer);
                }
                player.Call(33344, "^2[  ^7SILENCER ON  ^2]");
            }
            else
            {
                attachment = attachment.Replace("_" + silencer, "");
                player.Call(33344, "^2[  ^7SILENCER OFF  ^2]");
            }

            GiveWeaponTo(player, BuildGunName(gun, attachment));

        }

        string BuildGunName(string gun, string attachment)
        {
            if (attachment.Length == 0) return gun;

            if (attachment[0] != '_') attachment = attachment.Insert(0, "_");

            if (attachment.EndsWith("_")) attachment = attachment.Remove(attachment.Length - 1, 1);

            if (attachment.Contains("__")) attachment = attachment.Replace("__", "");

            gun += attachment;



            return gun;
        }

        string GetArLauncher(string baseGun)
        {
            for (int i = 0; i < AR_LIST.Length; i++)
            {
                string model = AR_LIST[i];
                if (model.Contains(baseGun))
                {
                    return model.Split('_')[3];
                }
            }

            return null;
        }
        string GetCurrentViewscope(string[] attaches, int type, string sniper)
        {
            string[] views = VIEWS_List[type];
            foreach (string att in attaches)
            {
                //Console.WriteLine("찾는 중: " + att);
                if (views.Contains(att))
                {
                    return att;
                }
            }
            return null;
        }
        string GetCurrentSilencer(string[] attaches)
        {
            foreach (string att in attaches)
            {
                if (SILENCERS.Contains(att))
                {
                    return att;
                }
            }
            return null;
        }

        int GetGunType(string baseGun)
        {
            int idx = Array.IndexOf(ALL_GUN_NAME, baseGun);
            if (idx == -1) return 0;

            if (idx < 4) return 0;
            if (idx < 10) return 1;
            if (idx < 20) return 2;
            if (idx < 26) return 3;
            if (idx < 31) return 4;
            if (idx < 37) return 5;
            else return 6;
        }

        #endregion

    }

}

