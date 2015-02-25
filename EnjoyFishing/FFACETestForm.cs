﻿using FFACETools;
using MiscTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace EnjoyFishing
{
    public partial class FFACETestForm : Form
    {
        private PolTool pol;
        private FFACETools.FFACE fface;
        public FFACETestForm(PolTool iPol)
        {
            InitializeComponent();
            pol = iPol;
            fface = iPol.FFACE;
        }

        private void FFACETestForm_Load(object sender, EventArgs e)
        {
            lblProcID.Text = pol.ProcessID.ToString();
            lblPlayerName.Text = fface.Player.Name;
            chkTopMost.Checked = false;
            trcOpacity.Value = 100;

            timRefresh.Enabled = true;
            gridStatus.RowsDefaultCellStyle.BackColor = Color.White;
            gridStatus.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
        }

        private void timRefresh_Tick(object sender, EventArgs e)
        {
            Dictionary<string, string> dicStatus = new Dictionary<string, string>();
            //Player
            dicStatus.Add("ログインステータス", fface.Player.GetLoginStatus.ToString());
            dicStatus.Add("プレイヤーステータス", fface.Player.Status.ToString());
            dicStatus.Add("エリア", string.Format("{0}({1})", fface.Player.Zone.ToString("D"), FFACE.ParseResources.GetAreaName(fface.Player.Zone)));
            //dicStatus.Add("天気", string.Format("{0}({1})", fface.Player.Weather.ToString("D"), FFACE.ParseResources.GetWeatherName(fface.Player.Weather)));
            string statusEffectsStr = string.Empty;
            foreach(StatusEffect statusEffect in fface.Player.StatusEffects)
            {
                if (statusEffect != StatusEffect.Unknown)
                {
                    statusEffectsStr += string.Format("{0}({1}),", statusEffect.ToString("D"), statusEffect);
                }
            }
            dicStatus.Add("強化", statusEffectsStr);
            dicStatus.Add("釣りスキル", fface.Player.GetCraftDetails(Craft.Fishing).Level.ToString());
            dicStatus.Add("位置X", fface.Player.PosX.ToString());
            dicStatus.Add("位置Y", fface.Player.PosY.ToString());
            dicStatus.Add("位置Z", fface.Player.PosZ.ToString());
            dicStatus.Add("方向", fface.Player.PosH.ToString());
            dicStatus.Add("MP残", fface.Player.MPCurrent.ToString());
            dicStatus.Add("スニーク覚えてる？", fface.Player.KnowsSpell(SpellList.Sneak).ToString());
            dicStatus.Add("残り詠唱時間", fface.Player.CastPercentEx.ToString());
            dicStatus.Add("詠唱時間", fface.Player.CastMax.ToString());
            //Timer
            dicStatus.Add("スニークリキャスト時間", fface.Timer.GetSpellRecast(SpellList.Sneak).ToString());
            dicStatus.Add("ヴァナ時間", fface.Timer.GetVanaTime().ToString());
            //Fish
            dicStatus.Add("魚-ID", string.Format("{0}-{1}-{2}-{3}",fface.Fish.ID.ID1, fface.Fish.ID.ID2, fface.Fish.ID.ID3, fface.Fish.ID.ID4));
            dicStatus.Add("魚-最大HP", fface.Fish.HPMax.ToString());
            dicStatus.Add("魚-現在HP", fface.Fish.HPCurrent.ToString());
            dicStatus.Add("魚-残り時間", fface.Fish.Timeout.ToString());
            //Item
            dicStatus.Add("鞄", string.Format("{0}/{1}", fface.Item.InventoryCount, fface.Item.InventoryMax));
            //dicStatus.Add("金庫", string.Format("{0}/{1}", fface.Item.SafeCount, fface.Item.SafeMax));
            //dicStatus.Add("家具", string.Format("{0}/{1}", fface.Item.StorageCount, fface.Item.StorageMax));
            dicStatus.Add("ロッカー", string.Format("{0}/{1}", fface.Item.LockerCount, fface.Item.LockerMax));
            dicStatus.Add("サッチェル", string.Format("{0}/{1}", fface.Item.SatchelCount, fface.Item.SatchelMax));
            dicStatus.Add("サック", string.Format("{0}/{1}", fface.Item.SackCount, fface.Item.SackMax));
            //dicStatus.Add("テンポラリ", string.Format("{0}/{1}", fface.Item.TemporaryCount, fface.Item.TemporaryMax));
            dicStatus.Add("ケース", string.Format("{0}/{1}", fface.Item.CaseCount, fface.Item.CaseMax));
            dicStatus.Add("ワードローブ", string.Format("{0}/{1}", fface.Item.WardrobeCount, fface.Item.WardrobeMax));
            dicStatus.Add("竿-装備", string.Format("{0}({1})", fface.Item.GetEquippedItemID(EquipSlot.Range), FFACE.ParseResources.GetItemName(fface.Item.GetEquippedItemID(EquipSlot.Range))));
            dicStatus.Add("竿-装備-鞄残数", fface.Item.GetItemCount(fface.Item.GetEquippedItemID(EquipSlot.Range), InventoryType.Inventory).ToString());
            dicStatus.Add("竿-装備-ワードローブ残数", fface.Item.GetItemCount(fface.Item.GetEquippedItemID(EquipSlot.Range), InventoryType.Wardrobe).ToString());
            dicStatus.Add("エサ-装備", string.Format("{0}({1})", fface.Item.GetEquippedItemID(EquipSlot.Ammo), FFACE.ParseResources.GetItemName(fface.Item.GetEquippedItemID(EquipSlot.Ammo))));
            dicStatus.Add("エサ-装備-鞄残数", fface.Item.GetItemCount(fface.Item.GetEquippedItemID(EquipSlot.Ammo), InventoryType.Inventory).ToString());
            dicStatus.Add("エサ-装備-サッチェル残数", fface.Item.GetItemCount(fface.Item.GetEquippedItemID(EquipSlot.Ammo), InventoryType.Satchel).ToString());
            dicStatus.Add("エサ-装備-サック残数", fface.Item.GetItemCount(fface.Item.GetEquippedItemID(EquipSlot.Ammo), InventoryType.Sack).ToString());
            dicStatus.Add("エサ-装備-ケース残数", fface.Item.GetItemCount(fface.Item.GetEquippedItemID(EquipSlot.Ammo), InventoryType.Case).ToString());
            dicStatus.Add("エサ-装備-ワードローブ残数", fface.Item.GetItemCount(fface.Item.GetEquippedItemID(EquipSlot.Ammo), InventoryType.Wardrobe).ToString());
            //Windower
            //dicStatus.Add("Windower.ArgumentCount", fface.Windower.ArgumentCount().ToString());
            //string argumentStr = string.Empty;
            //for (short i = 0; i < fface.Windower.ArgumentCount(); i++)
            //{
            ////    argumentStr += fface.Windower.GetArgument(i) + " ";
            //}
            //dicStatus.Add("Windower.GetArgument", argumentStr);
            //dicStatus.Add("Windower.IsNewCommand", fface.Windower.IsNewCommand().ToString());

            refreshStatus(dicStatus);
        }

        private void refreshStatus(Dictionary<string, string> iStatus)
        {
            //gridStatus
            foreach (KeyValuePair<string, string> status in iStatus)
            {
                int idx = -1;
                for (int i = 0; i < gridStatus.RowCount; i++)
                {
                    if (gridStatus.Rows[i].Cells[0].Value.ToString() == status.Key)
                    {
                        idx = i;
                        break;
                    }
                }
                if (idx < 0)
                {
                    gridStatus.Rows.Add();
                    idx = gridStatus.Rows.Count - 1;
                }
                gridStatus.Rows[idx].Cells[0].Value = status.Key;
                gridStatus.Rows[idx].Cells[1].Value = status.Value;
            }
            //gridChat
            if (fface.Chat.IsNewLine)
            {
                FFACE.ChatTools.ChatLine cl = fface.Chat.GetNextLine();
                while (cl != null)
                {
                    gridChat.Rows.Add();
                    gridChat.Rows[gridChat.Rows.Count - 1].Cells[0].Value = cl.Index;
                    gridChat.Rows[gridChat.Rows.Count - 1].Cells[1].Value = cl.NowDate.ToString("hh:mm:ss");
                    gridChat.Rows[gridChat.Rows.Count - 1].Cells[2].Value = cl.Type;
                    gridChat.Rows[gridChat.Rows.Count - 1].Cells[3].Value = cl.RawString[20];
                    gridChat.Rows[gridChat.Rows.Count - 1].Cells[4].Value = cl.Text;
                    cl = fface.Chat.GetNextLine();
                    gridChat.FirstDisplayedScrollingRowIndex = gridChat.RowCount-1;
                }
            }
        }

        private void chkTopMost_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = chkTopMost.Checked;
        }
        private void trcOpacity_Scroll(object sender, EventArgs e)
        {
            this.Opacity = trcOpacity.Value / 100f;
        }

    }
}
