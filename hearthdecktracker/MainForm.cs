using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using LINQtoCSV;
using System.IO;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using BobNetProto;
using PegasusGame;
using PegasusShared;
using PegasusUtil;
using hearthdecktracker.data;
using System.Threading;

namespace hearthdecktracker
{
    public partial class MainForm : Form
    {
        private bool dragging;
        private Point offset;

        private static SortedDictionary<int, ConnectAPI.PacketDecoder> s_packetDecoders = new SortedDictionary<int, ConnectAPI.PacketDecoder>();
        private static Dictionary<int, string> entity_dictionary;
        private static string cardid;
        private Thread thread;

        //Tinkered with the AllDeckCardLists class a bit. Updated for new list access
        public List<DeckCardList> deckCardLists = AllDeckCardLists.GetLists();


        /****************************
         * DECK LISTS ARE HERE
         ***************************/
        //Decklists now defined in JSON

        public MainForm()
        {
			setuplistener();
            InitializeComponent();
            cbDeckCardLists.DataSource = deckCardLists;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        
        private void MainForm_Load(object sender, EventArgs e)
        {
            dragging = false;
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            offset = new Point(e.X, e.Y);
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
           if (dragging)
            {
                Point currentScreenPos = PointToScreen(e.Location);
                Location = new Point(currentScreenPos.X - offset.X, currentScreenPos.Y - offset.Y);
            }
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }

        /*******************
         * Displaying stuff
         *******************/

        private void cbDeckCardLists_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbDeckCardLists.SelectedIndex != 0)
            {
                update_decklist();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (cbDeckCardLists.SelectedIndex != 0)
            {
                update_decklist();
            }
        }

       

       private void choose_row(string cardname)
        {
            if (gvDeckCardList.InvokeRequired)
            {
                Invoke(new Action<string>(choose_row), cardname);
            }
            else
            {
                int oldamt;
                int chg;

                foreach (DataGridViewRow row in gvDeckCardList.Rows)
                {
                    if (row.Cells["Name"].Value.ToString().Equals(cardname))
                    {
                        gvDeckCardList.CurrentCell = gvDeckCardList.Rows[row.Index].Cells[0];
                        //dataGridView1.Rows[row.Index + 1].Selected = true;

                        oldamt = int.Parse(gvDeckCardList.CurrentRow.Cells["Amt"].Value.ToString());
                        chg = -1;
                        if (oldamt != 0)
                        {
                            gvDeckCardList.CurrentRow.DefaultCellStyle.SelectionBackColor = Color.Red;

                            int newamt = Math.Max(0, oldamt + chg);
                            gvDeckCardList.CurrentRow.Cells["Amt"].Value = newamt;

                            if (newamt == 0)
                            {
                                gvDeckCardList.CurrentRow.DefaultCellStyle.ForeColor = Color.DarkGray;
                                gvDeckCardList.CurrentRow.DefaultCellStyle.SelectionForeColor = Color.DarkGray;
                            }
                            else
                            {
                                //dataGridView1.CurrentRow.DefaultCellStyle.ForeColor = Color.DarkGray;
                            }
                        }
                    }
                }
            }
        }

        private void gvDeckCardList_CellContentClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int oldamt;
            int chg;

            if (e.Button == MouseButtons.Left)
            {
                oldamt = int.Parse(gvDeckCardList.CurrentRow.Cells["Amt"].Value.ToString());
                chg = -1;
                if (oldamt != 0)
                {
                    gvDeckCardList.CurrentRow.DefaultCellStyle.SelectionBackColor = Color.Red;

                    int newamt = Math.Max(0, oldamt + chg);
                    gvDeckCardList.CurrentRow.Cells["Amt"].Value = newamt;



                    if (newamt == 0)
                    {
                        gvDeckCardList.CurrentRow.DefaultCellStyle.ForeColor = Color.DarkGray;
                        gvDeckCardList.CurrentRow.DefaultCellStyle.SelectionForeColor = Color.DarkGray;
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                gvDeckCardList.ClearSelection();
                gvDeckCardList.CurrentCell = gvDeckCardList.Rows[e.RowIndex].Cells[e.ColumnIndex];
                gvDeckCardList.Rows[e.RowIndex].Selected = true;
                gvDeckCardList.Focus();

                oldamt = int.Parse(gvDeckCardList.CurrentRow.Cells["Amt"].Value.ToString());
                chg = 1;

                if (oldamt != 2)
                {
                    gvDeckCardList.CurrentRow.DefaultCellStyle.SelectionBackColor = Color.Green;

                    int newamt = Math.Max(0, oldamt + chg);
                    gvDeckCardList.CurrentRow.Cells["Amt"].Value = newamt;

                    gvDeckCardList.CurrentRow.DefaultCellStyle.ForeColor = SystemColors.ControlText;
                    gvDeckCardList.CurrentRow.DefaultCellStyle.SelectionForeColor = SystemColors.ControlText;
                }
            }
            else
            {
                chg = 0;
            }
        }

        private void update_decklist()
        {
            gvDeckCardList.Rows.Clear();
			entity_dictionary = new Dictionary<int, string>();

            DeckCardList selectedDeckCardList = (DeckCardList)cbDeckCardLists.SelectedValue;

            foreach (DeckCard card in selectedDeckCardList.DeckCards)
            {
                string ad;
                if (card.Card.Atk == 0 && card.Card.Def == 0)
                {
                    ad = "---";
                }
                else {
                    ad = card.Card.Atk + "/" + card.Card.Def;
                }
                gvDeckCardList.Rows.Add(
                    Convert.ToInt32(card.Card.Mana),
                    card.Card.Name,
                    ad,
                    card.Card.Dmg + "|" + card.Card.Heal + "|" + card.Card.Catk,
                    card.Card.Targ,
                    Convert.ToInt32(card.Amount)
                );
            }

            gvDeckCardList.Visible = true;
 			thread = new Thread(this.readtcpdata);
            thread.Start();
        }

        private void setuplistener()
        {
            s_packetDecoders.Add(1, new ConnectAPI.DefaultProtobufPacketDecoder<GetGameState, GetGameState.Builder>());
            s_packetDecoders.Add(2, new ConnectAPI.DefaultProtobufPacketDecoder<ChooseOption, ChooseOption.Builder>());
            s_packetDecoders.Add(3, new ConnectAPI.DefaultProtobufPacketDecoder<ChooseEntities, ChooseEntities.Builder>());
            s_packetDecoders.Add(4, new ConnectAPI.DefaultProtobufPacketDecoder<Precast, Precast.Builder>());
            s_packetDecoders.Add(6, new ConnectAPI.DefaultProtobufPacketDecoder<ClientPacket, ClientPacket.Builder>());
            s_packetDecoders.Add(11, new ConnectAPI.DefaultProtobufPacketDecoder<GiveUp, GiveUp.Builder>());
            s_packetDecoders.Add(13, new ConnectAPI.DefaultProtobufPacketDecoder<ForcedEntityChoice, ForcedEntityChoice.Builder>());
            s_packetDecoders.Add(100, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasPlayer, AtlasPlayer.Builder>());
            s_packetDecoders.Add(101, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasError, AtlasError.Builder>());
            s_packetDecoders.Add(102, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasCollection, AtlasCollection.Builder>());
            s_packetDecoders.Add(103, new ConnectAPI.DefaultProtobufPacketDecoder<AutoLogin, AutoLogin.Builder>());
            s_packetDecoders.Add(104, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasDecks, AtlasDecks.Builder>());
            s_packetDecoders.Add(105, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasSuccess, AtlasSuccess.Builder>());
            s_packetDecoders.Add(106, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasOrders, AtlasOrders.Builder>());
            s_packetDecoders.Add(107, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasAchieves, AtlasAchieves.Builder>());
            s_packetDecoders.Add(108, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasAchieveInfo, AtlasAchieveInfo.Builder>());
            s_packetDecoders.Add(109, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasBoosters, AtlasBoosters.Builder>());
            s_packetDecoders.Add(110, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasDrafts, AtlasDrafts.Builder>());
            s_packetDecoders.Add(111, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasCurrencyDetails, AtlasCurrencyDetails.Builder>());
            s_packetDecoders.Add(112, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasCardBacks, AtlasCardBacks.Builder>());
            s_packetDecoders.Add(113, new ConnectAPI.DefaultProtobufPacketDecoder<BeginPlaying, BeginPlaying.Builder>());
            s_packetDecoders.Add(123, new ConnectAPI.DefaultProtobufPacketDecoder<DebugConsoleCommand, DebugConsoleCommand.Builder>());
            s_packetDecoders.Add(124, new ConnectAPI.DefaultProtobufPacketDecoder<DebugConsoleResponse, DebugConsoleResponse.Builder>());
            s_packetDecoders.Add(125, new ConnectAPI.DefaultProtobufPacketDecoder<DebugConsoleGetCmdList, DebugConsoleGetCmdList.Builder>());
            s_packetDecoders.Add(142, new ConnectAPI.DefaultProtobufPacketDecoder<DebugPaneNewItems, DebugPaneNewItems.Builder>());
            s_packetDecoders.Add(143, new ConnectAPI.DefaultProtobufPacketDecoder<DebugPaneDelItems, DebugPaneDelItems.Builder>());
            s_packetDecoders.Add(145, new ConnectAPI.DefaultProtobufPacketDecoder<DebugConsoleUpdateFromPane, DebugConsoleUpdateFromPane.Builder>());
            s_packetDecoders.Add(146, new ConnectAPI.DefaultProtobufPacketDecoder<DebugConsoleCmdList, DebugConsoleCmdList.Builder>());
            s_packetDecoders.Add(147, new ConnectAPI.DefaultProtobufPacketDecoder<DebugConsoleGetZones, DebugConsoleGetZones.Builder>());
            s_packetDecoders.Add(148, new ConnectAPI.DefaultProtobufPacketDecoder<DebugConsoleZones, DebugConsoleZones.Builder>());
            s_packetDecoders.Add(168, new ConnectAPI.DefaultProtobufPacketDecoder<AuroraHandshake, AuroraHandshake.Builder>());
            s_packetDecoders.Add(201, new ConnectAPI.DefaultProtobufPacketDecoder<GetAccountInfo, GetAccountInfo.Builder>());
            s_packetDecoders.Add(203, new ConnectAPI.DefaultProtobufPacketDecoder<UtilHandshake, UtilHandshake.Builder>());
            s_packetDecoders.Add(204, new ConnectAPI.DefaultProtobufPacketDecoder<UtilAuth, UtilAuth.Builder>());
            s_packetDecoders.Add(205, new ConnectAPI.DefaultProtobufPacketDecoder<UpdateLogin, UpdateLogin.Builder>());
            s_packetDecoders.Add(206, new ConnectAPI.DefaultProtobufPacketDecoder<DebugAuth, DebugAuth.Builder>());
            s_packetDecoders.Add(209, new ConnectAPI.DefaultProtobufPacketDecoder<CreateDeck, CreateDeck.Builder>());
            s_packetDecoders.Add(210, new ConnectAPI.DefaultProtobufPacketDecoder<DeleteDeck, DeleteDeck.Builder>());
            s_packetDecoders.Add(211, new ConnectAPI.DefaultProtobufPacketDecoder<RenameDeck, RenameDeck.Builder>());
            s_packetDecoders.Add(213, new ConnectAPI.DefaultProtobufPacketDecoder<AckNotice, AckNotice.Builder>());
            s_packetDecoders.Add(214, new ConnectAPI.DefaultProtobufPacketDecoder<GetDeck, GetDeck.Builder>());
            s_packetDecoders.Add(220, new ConnectAPI.DefaultProtobufPacketDecoder<DeckGainedCard, DeckGainedCard.Builder>());
            s_packetDecoders.Add(221, new ConnectAPI.DefaultProtobufPacketDecoder<DeckLostCard, DeckLostCard.Builder>());
            s_packetDecoders.Add(222, new ConnectAPI.DefaultProtobufPacketDecoder<DeckSetData, DeckSetData.Builder>());
            s_packetDecoders.Add(223, new ConnectAPI.DefaultProtobufPacketDecoder<AckCardSeen, AckCardSeen.Builder>());
            s_packetDecoders.Add(225, new ConnectAPI.DefaultProtobufPacketDecoder<OpenBooster, OpenBooster.Builder>());
            s_packetDecoders.Add(228, new ConnectAPI.DefaultProtobufPacketDecoder<ClientTracking, ClientTracking.Builder>());
            s_packetDecoders.Add(229, new ConnectAPI.DefaultProtobufPacketDecoder<SubmitBug, SubmitBug.Builder>());
            s_packetDecoders.Add(230, new ConnectAPI.DefaultProtobufPacketDecoder<SetProgress, SetProgress.Builder>());
            s_packetDecoders.Add(235, new ConnectAPI.DefaultProtobufPacketDecoder<DraftBegin, DraftBegin.Builder>());
            s_packetDecoders.Add(237, new ConnectAPI.DefaultProtobufPacketDecoder<GetBattlePayConfig, GetBattlePayConfig.Builder>());
            s_packetDecoders.Add(239, new ConnectAPI.DefaultProtobufPacketDecoder<SetOptions, SetOptions.Builder>());
            s_packetDecoders.Add(240, new ConnectAPI.DefaultProtobufPacketDecoder<GetOptions, GetOptions.Builder>());
            s_packetDecoders.Add(242, new ConnectAPI.DefaultProtobufPacketDecoder<DraftRetire, DraftRetire.Builder>());
            s_packetDecoders.Add(243, new ConnectAPI.DefaultProtobufPacketDecoder<AckAchieveProgress, AckAchieveProgress.Builder>());
            s_packetDecoders.Add(244, new ConnectAPI.DefaultProtobufPacketDecoder<DraftGetPicksAndContents, DraftGetPicksAndContents.Builder>());
            s_packetDecoders.Add(245, new ConnectAPI.DefaultProtobufPacketDecoder<DraftMakePick, DraftMakePick.Builder>());
            s_packetDecoders.Add(250, new ConnectAPI.DefaultProtobufPacketDecoder<GetPurchaseMethod, GetPurchaseMethod.Builder>());
            s_packetDecoders.Add(253, new ConnectAPI.DefaultProtobufPacketDecoder<GetAchieves, GetAchieves.Builder>());
            s_packetDecoders.Add(255, new ConnectAPI.DefaultProtobufPacketDecoder<GetBattlePayStatus, GetBattlePayStatus.Builder>());
            s_packetDecoders.Add(257, new ConnectAPI.DefaultProtobufPacketDecoder<BuySellCard, BuySellCard.Builder>());
            s_packetDecoders.Add(259, new ConnectAPI.DefaultProtobufPacketDecoder<DevBnetIdentify, DevBnetIdentify.Builder>());
            s_packetDecoders.Add(261, new ConnectAPI.DefaultProtobufPacketDecoder<GuardianTrack, GuardianTrack.Builder>());
            s_packetDecoders.Add(263, new ConnectAPI.DefaultProtobufPacketDecoder<CloseCardMarket, CloseCardMarket.Builder>());
            s_packetDecoders.Add(267, new ConnectAPI.DefaultProtobufPacketDecoder<CheckAccountLicenses, CheckAccountLicenses.Builder>());
            s_packetDecoders.Add(268, new ConnectAPI.DefaultProtobufPacketDecoder<MassDisenchantRequest, MassDisenchantRequest.Builder>());
            s_packetDecoders.Add(273, new ConnectAPI.DefaultProtobufPacketDecoder<DoPurchase, DoPurchase.Builder>());
            s_packetDecoders.Add(274, new ConnectAPI.DefaultProtobufPacketDecoder<CancelPurchase, CancelPurchase.Builder>());
            s_packetDecoders.Add(276, new ConnectAPI.DefaultProtobufPacketDecoder<CheckGameLicenses, CheckGameLicenses.Builder>());
            s_packetDecoders.Add(279, new ConnectAPI.DefaultProtobufPacketDecoder<PurchaseWithGold, PurchaseWithGold.Builder>());
            s_packetDecoders.Add(281, new ConnectAPI.DefaultProtobufPacketDecoder<CancelQuest, CancelQuest.Builder>());
            s_packetDecoders.Add(284, new ConnectAPI.DefaultProtobufPacketDecoder<ValidateAchieve, ValidateAchieve.Builder>());
            s_packetDecoders.Add(287, new ConnectAPI.DefaultProtobufPacketDecoder<DraftAckRewards, DraftAckRewards.Builder>());
            s_packetDecoders.Add(291, new ConnectAPI.DefaultProtobufPacketDecoder<SetCardBack, SetCardBack.Builder>());
            s_packetDecoders.Add(293, new ConnectAPI.DefaultProtobufPacketDecoder<DoThirdPartyPurchase, DoThirdPartyPurchase.Builder>());
            s_packetDecoders.Add(294, new ConnectAPI.DefaultProtobufPacketDecoder<GetThirdPartyPurchaseStatus, GetThirdPartyPurchaseStatus.Builder>());
            s_packetDecoders.Add(298, new ConnectAPI.DefaultProtobufPacketDecoder<TriggerLaunchDayEvent, TriggerLaunchDayEvent.Builder>());
            s_packetDecoders.Add(401, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasGetPlayerInfo, AtlasGetPlayerInfo.Builder>());
            s_packetDecoders.Add(402, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasGetCollection, AtlasGetCollection.Builder>());
            s_packetDecoders.Add(403, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasGetCardDetails, AtlasGetCardDetails.Builder>());
            s_packetDecoders.Add(404, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasGetDecks, AtlasGetDecks.Builder>());
            s_packetDecoders.Add(405, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasAddCard, AtlasAddCard.Builder>());
            s_packetDecoders.Add(406, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasRemoveCard, AtlasRemoveCard.Builder>());
            s_packetDecoders.Add(407, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasChangeArcaneDust, AtlasChangeArcaneDust.Builder>());
            s_packetDecoders.Add(408, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasRestoreCard, AtlasRestoreCard.Builder>());
            s_packetDecoders.Add(409, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasGetOrders, AtlasGetOrders.Builder>());
            s_packetDecoders.Add(410, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasGetAchieves, AtlasGetAchieves.Builder>());
            s_packetDecoders.Add(411, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasGetAchieveInfo, AtlasGetAchieveInfo.Builder>());
            s_packetDecoders.Add(412, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasGetBoosters, AtlasGetBoosters.Builder>());
            s_packetDecoders.Add(413, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasAddBooster, AtlasAddBooster.Builder>());
            s_packetDecoders.Add(414, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasRemoveBooster, AtlasRemoveBooster.Builder>());
            s_packetDecoders.Add(415, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasGetDrafts, AtlasGetDrafts.Builder>());
            s_packetDecoders.Add(416, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasAddDraft, AtlasAddDraft.Builder>());
            s_packetDecoders.Add(417, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasRemoveDraft, AtlasRemoveDraft.Builder>());
            s_packetDecoders.Add(418, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasChangeGold, AtlasChangeGold.Builder>());
            s_packetDecoders.Add(419, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasGetCurrencyDetails, AtlasGetCurrencyDetails.Builder>());
            s_packetDecoders.Add(420, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasChangeBonusGold, AtlasChangeBonusGold.Builder>());
            s_packetDecoders.Add(421, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasGetCardBacks, AtlasGetCardBacks.Builder>());
            s_packetDecoders.Add(422, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasAddCardBack, AtlasAddCardBack.Builder>());
            s_packetDecoders.Add(423, new ConnectAPI.DefaultProtobufPacketDecoder<AtlasRemoveCardBack, AtlasRemoveCardBack.Builder>());

            s_packetDecoders.Add(5, new ConnectAPI.DefaultProtobufPacketDecoder<DebugMessage, DebugMessage.Builder>());
            s_packetDecoders.Add(7, new ConnectAPI.DefaultProtobufPacketDecoder<StartGameState, StartGameState.Builder>());
            s_packetDecoders.Add(8, new ConnectAPI.DefaultProtobufPacketDecoder<FinishGameState, FinishGameState.Builder>());
            s_packetDecoders.Add(9, new ConnectAPI.DefaultProtobufPacketDecoder<PegasusGame.TurnTimer, PegasusGame.TurnTimer.Builder>());
            s_packetDecoders.Add(10, new ConnectAPI.DefaultProtobufPacketDecoder<NAckOption, NAckOption.Builder>());
            s_packetDecoders.Add(12, new ConnectAPI.DefaultProtobufPacketDecoder<GameCanceled, GameCanceled.Builder>());
            s_packetDecoders.Add(14, new ConnectAPI.DefaultProtobufPacketDecoder<AllOptions, AllOptions.Builder>());
            //s_packetDecoders.Add(15, new ConnectAPI.DefaultProtobufPacketDecoder<UserUI, UserUI.Builder>());
            s_packetDecoders.Add(16, new ConnectAPI.DefaultProtobufPacketDecoder<GameSetup, GameSetup.Builder>());
            s_packetDecoders.Add(17, new ConnectAPI.DefaultProtobufPacketDecoder<EntityChoice, EntityChoice.Builder>());
            s_packetDecoders.Add(18, new ConnectAPI.DefaultProtobufPacketDecoder<PreLoad, PreLoad.Builder>());
            s_packetDecoders.Add(19, new ConnectAPI.DefaultProtobufPacketDecoder<PowerHistory, PowerHistory.Builder>());
            s_packetDecoders.Add(21, new ConnectAPI.DefaultProtobufPacketDecoder<PegasusGame.Notification, PegasusGame.Notification.Builder>());
            s_packetDecoders.Add(114, new ConnectAPI.DefaultProtobufPacketDecoder<GameStarting, GameStarting.Builder>());
            s_packetDecoders.Add(167, new ConnectAPI.DefaultProtobufPacketDecoder<DeadendUtil, DeadendUtil.Builder>());
            s_packetDecoders.Add(169, new ConnectAPI.DefaultProtobufPacketDecoder<Deadend, Deadend.Builder>());
            s_packetDecoders.Add(202, new ConnectAPI.DefaultProtobufPacketDecoder<DeckList, DeckList.Builder>());
            s_packetDecoders.Add(207, new ConnectAPI.DefaultProtobufPacketDecoder<Collection, Collection.Builder>());
            s_packetDecoders.Add(208, new ConnectAPI.DefaultProtobufPacketDecoder<GamesInfo, GamesInfo.Builder>());
            s_packetDecoders.Add(212, new ConnectAPI.DefaultProtobufPacketDecoder<ProfileNotices, ProfileNotices.Builder>());
            s_packetDecoders.Add(215, new ConnectAPI.DefaultProtobufPacketDecoder<DeckContents, DeckContents.Builder>());
            s_packetDecoders.Add(216, new ConnectAPI.DefaultProtobufPacketDecoder<DBAction, DBAction.Builder>());
            s_packetDecoders.Add(217, new ConnectAPI.DefaultProtobufPacketDecoder<DeckCreated, DeckCreated.Builder>());
            s_packetDecoders.Add(218, new ConnectAPI.DefaultProtobufPacketDecoder<DeckDeleted, DeckDeleted.Builder>());
            s_packetDecoders.Add(219, new ConnectAPI.DefaultProtobufPacketDecoder<DeckRenamed, DeckRenamed.Builder>());
            s_packetDecoders.Add(224, new ConnectAPI.DefaultProtobufPacketDecoder<BoosterList, BoosterList.Builder>());
            s_packetDecoders.Add(226, new ConnectAPI.DefaultProtobufPacketDecoder<BoosterContent, BoosterContent.Builder>());
            s_packetDecoders.Add(227, new ConnectAPI.DefaultProtobufPacketDecoder<ProfileLastLogin, ProfileLastLogin.Builder>());
            s_packetDecoders.Add(231, new ConnectAPI.DefaultProtobufPacketDecoder<ProfileDeckLimit, ProfileDeckLimit.Builder>());
            s_packetDecoders.Add(232, new ConnectAPI.DefaultProtobufPacketDecoder<MedalInfo, MedalInfo.Builder>());
            s_packetDecoders.Add(233, new ConnectAPI.DefaultProtobufPacketDecoder<ProfileProgress, ProfileProgress.Builder>());
            s_packetDecoders.Add(234, new ConnectAPI.DefaultProtobufPacketDecoder<MedalHistory, MedalHistory.Builder>());
            s_packetDecoders.Add(236, new ConnectAPI.DefaultProtobufPacketDecoder<CardBacks, CardBacks.Builder>());
            s_packetDecoders.Add(238, new ConnectAPI.DefaultProtobufPacketDecoder<BattlePayConfigResponse, BattlePayConfigResponse.Builder>());
            s_packetDecoders.Add(241, new ConnectAPI.DefaultProtobufPacketDecoder<ClientOptions, ClientOptions.Builder>());
            s_packetDecoders.Add(246, new ConnectAPI.DefaultProtobufPacketDecoder<DraftBeginning, DraftBeginning.Builder>());
            s_packetDecoders.Add(247, new ConnectAPI.DefaultProtobufPacketDecoder<DraftRetired, DraftRetired.Builder>());
            s_packetDecoders.Add(248, new ConnectAPI.DefaultProtobufPacketDecoder<DraftChoicesAndContents, DraftChoicesAndContents.Builder>());
            s_packetDecoders.Add(249, new ConnectAPI.DefaultProtobufPacketDecoder<DraftChosen, DraftChosen.Builder>());
            s_packetDecoders.Add(251, new ConnectAPI.DefaultProtobufPacketDecoder<DraftError, DraftError.Builder>());
            s_packetDecoders.Add(252, new ConnectAPI.DefaultProtobufPacketDecoder<Achieves, Achieves.Builder>());
            s_packetDecoders.Add(254, new ConnectAPI.NoOpPacketDecoder());
            s_packetDecoders.Add(256, new ConnectAPI.DefaultProtobufPacketDecoder<PurchaseResponse, PurchaseResponse.Builder>());
            s_packetDecoders.Add(258, new ConnectAPI.DefaultProtobufPacketDecoder<BoughtSoldCard, BoughtSoldCard.Builder>());
            s_packetDecoders.Add(260, new ConnectAPI.DefaultProtobufPacketDecoder<CardValues, CardValues.Builder>());
            s_packetDecoders.Add(262, new ConnectAPI.DefaultProtobufPacketDecoder<ArcaneDustBalance, ArcaneDustBalance.Builder>());
            s_packetDecoders.Add(264, new ConnectAPI.DefaultProtobufPacketDecoder<GuardianVars, GuardianVars.Builder>());
            s_packetDecoders.Add(265, new ConnectAPI.DefaultProtobufPacketDecoder<BattlePayStatusResponse, BattlePayStatusResponse.Builder>());
            s_packetDecoders.Add(266, new ConnectAPI.ThrottlePacketDecoder());
            s_packetDecoders.Add(269, new ConnectAPI.DefaultProtobufPacketDecoder<MassDisenchantResponse, MassDisenchantResponse.Builder>());
            s_packetDecoders.Add(270, new ConnectAPI.DefaultProtobufPacketDecoder<PlayerRecords, PlayerRecords.Builder>());
            s_packetDecoders.Add(271, new ConnectAPI.DefaultProtobufPacketDecoder<RewardProgress, RewardProgress.Builder>());
            s_packetDecoders.Add(272, new ConnectAPI.DefaultProtobufPacketDecoder<PurchaseMethod, PurchaseMethod.Builder>());
            s_packetDecoders.Add(275, new ConnectAPI.DefaultProtobufPacketDecoder<CancelPurchaseResponse, CancelPurchaseResponse.Builder>());
            s_packetDecoders.Add(277, new ConnectAPI.DefaultProtobufPacketDecoder<CheckLicensesResponse, CheckLicensesResponse.Builder>());
            s_packetDecoders.Add(278, new ConnectAPI.DefaultProtobufPacketDecoder<GoldBalance, GoldBalance.Builder>());
            s_packetDecoders.Add(280, new ConnectAPI.DefaultProtobufPacketDecoder<PurchaseWithGoldResponse, PurchaseWithGoldResponse.Builder>());
            s_packetDecoders.Add(282, new ConnectAPI.DefaultProtobufPacketDecoder<CancelQuestResponse, CancelQuestResponse.Builder>());
            s_packetDecoders.Add(283, new ConnectAPI.DefaultProtobufPacketDecoder<HeroXP, HeroXP.Builder>());
            s_packetDecoders.Add(285, new ConnectAPI.DefaultProtobufPacketDecoder<ValidateAchieveResponse, ValidateAchieveResponse.Builder>());
            s_packetDecoders.Add(286, new ConnectAPI.DefaultProtobufPacketDecoder<PlayQueue, PlayQueue.Builder>());
            s_packetDecoders.Add(288, new ConnectAPI.DefaultProtobufPacketDecoder<DraftRewardsAcked, DraftRewardsAcked.Builder>());
            s_packetDecoders.Add(289, new ConnectAPI.DefaultProtobufPacketDecoder<Disconnected, Disconnected.Builder>());
            s_packetDecoders.Add(292, new ConnectAPI.DefaultProtobufPacketDecoder<SetCardBackResponse, SetCardBackResponse.Builder>());
            s_packetDecoders.Add(295, new ConnectAPI.DefaultProtobufPacketDecoder<ThirdPartyPurchaseStatusResponse, ThirdPartyPurchaseStatusResponse.Builder>());
            s_packetDecoders.Add(296, new ConnectAPI.DefaultProtobufPacketDecoder<SetProgressResponse, SetProgressResponse.Builder>());
            s_packetDecoders.Add(299, new ConnectAPI.DefaultProtobufPacketDecoder<TriggerEventResponse, TriggerEventResponse.Builder>());
            s_packetDecoders.Add(300, new ConnectAPI.DefaultProtobufPacketDecoder<MassiveLoginReply, MassiveLoginReply.Builder>());

	}

	private void readtcpdata()
        {
            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;
            PacketDevice selectedDevice = allDevices[1]; //3

            using (PacketCommunicator communicator = selectedDevice.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1000))
            {
                communicator.SetFilter("port 1119 or port 3724");
                communicator.ReceivePackets(0, DispatcherHandler);
            }
        }

        private void DispatcherHandler(Packet packet)
        {
            int le = packet.Ethernet.IpV4.Tcp.Http.Length;
            var d = packet.Buffer.Reverse().Take(le).Reverse().ToArray();

            PegasusPacket x = new PegasusPacket();
            try
            {
                x.Decode(d, 0, d.Length);
            }
            catch
            {
            }
            finally
            {
                ConnectAPI.PacketDecoder decoder;

                if (s_packetDecoders.TryGetValue(x.Type, out decoder))
                {
                    PegasusPacket item = decoder.HandlePacket(x);

                    var bod = item.Body;
                    var typ = item.Type;

                    if (typ == 19)
                    {
                        PowerHistory history = (PowerHistory)bod;
                        IList<PowerHistoryData> listy = history.ListList;
                        foreach (var phd in listy)
                        {
                            if (phd.HasShowEntity)
                            {
                                try
                                {
                                    entity_dictionary.Add(phd.ShowEntity.Entity, phd.ShowEntity.Name);
                                }
                                catch
                                {
                                }
                            }
                            else if (phd.HasPowerStart)
                            {
                                if (phd.PowerStart.Type == PegasusGame.PowerHistoryStart.Types.Type.PLAY)
                                {
                                    if (entity_dictionary.TryGetValue(phd.PowerStart.Source, out cardid))
                                    {
                                        string cardname = CardList.List.Find(r => r.ID == cardid).Name;
                                        choose_row(cardname);
                                    }

                                }
                            }
                        }
                    }

                    else
                    {
                        //Console.WriteLine(x.Body.ToString());
                    }
                }
            }
        }
	}
}
