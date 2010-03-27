/*******************************************************************
 * This file was used to test netAudio and netDiscographer.
 * 
 * This source may be distributed or modified without explicit
 * written permission.
 * 
 * This source is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * *****************************************************************
 * Copyright (C) 2009-2010 Matt Razza
 * This software is distributed under the Microsoft Public License (Ms-PL).
 *******************************************************************/

using System;
using System.Windows.Forms;
using netAudio.core;
using netAudio.netVLC;
using netDiscographer.core;
using netDiscographer.core.dynamicQueryCore;
using netDiscographer.core.mediaSearch;
using netDiscographer.sqlite;

namespace testing
{
    public partial class mainForm : Form
    {
        private netAudioPlayer player;
        private mediaEntry[] entries;
        private string[] stdPlaylistNames;
        private int[] stdPlaylistIDs;
        private string[] dynPlaylistNames;
        private int[] dynPlaylistIDs;
        private dynamicQuery[] dynPlaylistQuery;
        private Timer searchIt = new Timer();
        private int iLastTrack;
        discographerDatabase db = new sqliteDatabase("music.db");

        public mainForm()
        {
            InitializeComponent();
            player = new netVLCPlayer();
            EventHandler<netAudio.core.events.positionChangedEventArgs> test = new EventHandler<netAudio.core.events.positionChangedEventArgs>(player_ePositionChanged);
            player.ePositionChanged += new EventHandler<netAudio.core.events.positionChangedEventArgs>(player_ePositionChanged);
            player.eStateChanged += new EventHandler<netAudio.core.events.stateChangedEventArgs>(player_eStateChanged);
            iLastTrack = -1;
        }

        private delegate void player_eStateChangedDel(object sender, netAudio.core.events.stateChangedEventArgs e);
        void player_eStateChanged(object sender, netAudio.core.events.stateChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new player_eStateChangedDel(player_eStateChanged), sender, e);
                Console.WriteLine("State: " + e.pState);
            }
            else if (e.pState == playerState.ended)
            {
                iLastTrack++;
                if (iLastTrack >= entries.Length)
                {
                    return;
                }
                textBox1.Text = entries[iLastTrack].sPath;
                button2_Click(null, null);
                button3_Click(null, null);
            }
        }

        private delegate void positionChangedDelegate(object sender, netAudio.core.events.positionChangedEventArgs e);

        void player_ePositionChanged(object sender, netAudio.core.events.positionChangedEventArgs e)
        {
            if (InvokeRequired)
                BeginInvoke(new positionChangedDelegate(player_ePositionChanged), sender, e);
            else
            {
                int per = Convert.ToInt32(Math.Floor(((float)e.lPosition / player.lLength) * 100000));
                progressBar1.Value = per;
                progressBar1.Refresh();
                lblPer.Text = (per / 1000).ToString() + "%";
                lblPer.Refresh();
                lblCurr.Text = (e.lPosition / 1000).ToString();
                lblCurr.Refresh();
            }
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            // Let's deal with the virt mode shit
            libDisplay.VirtualMode = true;
            stdPlay.VirtualMode = true;
            dynPlay.VirtualMode = true;

            // Connect the virtual-mode events to event handlers
            libDisplay.RetrieveVirtualItem += new RetrieveVirtualItemEventHandler(libDisplay_RetrieveVirtualItem);
            libDisplay.ItemActivate += new EventHandler(libDisplay_ItemActivate);
            stdPlay.RetrieveVirtualItem += new RetrieveVirtualItemEventHandler(stdPlay_RetrieveVirtualItem);
            stdPlay.ItemActivate += new EventHandler(stdPlay_ItemActivate);
            dynPlay.RetrieveVirtualItem += new RetrieveVirtualItemEventHandler(dynPlay_RetrieveVirtualItem);
            dynPlay.ItemActivate += new EventHandler(dynPlay_ItemActivate);

            // Add columns to the library
            libDisplay.Columns.Add("#");
            libDisplay.Columns.Add("Track");
            libDisplay.Columns.Add("Artist");
            libDisplay.Columns.Add("Album");
            libDisplay.View = View.Details;
            libDisplay.GridLines = true;
            libDisplay.FullRowSelect = true;
            libDisplay.Columns[0].Width = libDisplay.Width / 14;
            libDisplay.Columns[1].Width = (int)(libDisplay.Width / 3.25);
            libDisplay.Columns[2].Width = (int)(libDisplay.Width / 3.25);
            libDisplay.Columns[3].Width = (int)(libDisplay.Width / 3.25);

            // Add columns to the stdPlay
            stdPlay.Columns.Add("Name");
            stdPlay.View = View.Details;
            stdPlay.GridLines = true;
            stdPlay.FullRowSelect = true;
            stdPlay.Columns[0].Width = libDisplay.Width;

            // Add columns to the dynPlay
            dynPlay.Columns.Add("Name");
            dynPlay.View = View.Details;
            dynPlay.GridLines = true;
            dynPlay.FullRowSelect = true;
            dynPlay.Columns[0].Width = libDisplay.Width;

            // Set up everything else
            searchIt.Interval = 400;
            searchIt.Tick += new EventHandler(doSearch);

            trackBar1.Value = player.iVolume * 2;
            vol.Text = trackBar1.Value.ToString();
            updateStdPlay();
            updateDynPlay();
            rdLib.Checked = true;
        }

        void updateStdPlay()
        {
            stdPlaylistIDs = db.getStandardPlaylistIDs();
            stdPlaylistNames = db.getStandardPlaylistName();
            if (stdPlay.VirtualListSize == stdPlaylistNames.Length)
            {
                if (stdPlay.VirtualListSize == 1)
                    stdPlay.VirtualListSize = 2;
                else
                    stdPlay.VirtualListSize = 1;
            }

            stdPlay.VirtualListSize = stdPlaylistNames.Length;
        }

        void updateDynPlay()
        {
            dynPlaylistIDs = db.getDynamicPlaylistIDs();
            dynPlaylistNames = db.getDynamicPlaylistName();
            dynPlaylistQuery = db.getDynamicPlaylistQuery();
            if (dynPlay.VirtualListSize == dynPlaylistQuery.Length)
            {
                if (dynPlay.VirtualListSize == 1)
                    dynPlay.VirtualListSize = 2;
                else
                    dynPlay.VirtualListSize = 1;
            }

            dynPlay.VirtualListSize = dynPlaylistQuery.Length;
        }

        void dynPlay_ItemActivate(object sender, EventArgs e)
        {
            rdDyPlay.Checked = true;
        }

        void stdPlay_ItemActivate(object sender, EventArgs e)
        {
            rdPlay.Checked = true;
        }

        void dynPlay_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            // If this is the row for new records, no values are needed.
            if (dynPlaylistNames.Length - 1 < e.ItemIndex)
            {
                e.Item = new ListViewItem(new string[] { "", "" });
                return;
            }

            e.Item = new ListViewItem(new string[] { dynPlaylistNames[e.ItemIndex] });
        }

        void stdPlay_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            // If this is the row for new records, no values are needed.
            if (stdPlaylistNames.Length - 1 < e.ItemIndex)
            {
                e.Item = new ListViewItem("");
                return;
            }

            e.Item = new ListViewItem(stdPlaylistNames[e.ItemIndex]);
        }

        void libDisplay_ItemActivate(object sender, EventArgs e)
        {
            iLastTrack = libDisplay.SelectedIndices[0];
            textBox1.Text = entries[iLastTrack].sPath;
            button2_Click(null, null);
            button3_Click(null, null);
        }

        private void libDisplay_RetrieveVirtualItem(object sender,
        System.Windows.Forms.RetrieveVirtualItemEventArgs e)
        {
            // If this is the row for new records, no values are needed.
            if (entries.Length - 1 < e.ItemIndex)
            {
                e.Item = new ListViewItem(new string[] { "", "", "", "" });
                return;
            }

            mediaEntry customerTmp = entries[e.ItemIndex];
            e.Item = new ListViewItem(new string[] { customerTmp[metaDataFieldTypes.track].ToString(), customerTmp[metaDataFieldTypes.title], customerTmp[metaDataFieldTypes.artist], customerTmp[metaDataFieldTypes.album] });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (player.sMediaPath != textBox1.Text)
                button2_Click(sender, e);

            player.playMedia();
            lblTotal.Text = ((int)entries[iLastTrack].tLength.TotalSeconds).ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            player.sMediaPath = textBox1.Text;
            metaData data = player.mTrackData;
            lblTitle.Text = data.sTitle;
            lblArtist.Text = data.sArtist;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.ShowDialog();

            textBox1.Text = open.FileName;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            player.pauseMedia();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            player.stopMedia();
        }

        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            player.Dispose();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog open = new FolderBrowserDialog();
            open.Description = "Select a folder to search for media in (searches all subfolders)...";
            if (open.ShowDialog() == DialogResult.Cancel)
                return;
            string sPath = open.SelectedPath;
            mediaSearchSystem search = new mediaSearchSystem();
            
            db.addMedia(search.searchDirectory(player, sPath, true));

            GC.Collect();
            doSearch(null, null);
        }

        private void doSearch(object o, EventArgs e)
        {
            searchIt.Stop();
            libDisplay.Enabled = false;
            entries = new mediaEntry[0];
            if (libDisplay.VirtualListSize == entries.Length)
            {
                if (libDisplay.VirtualListSize == 1)
                    libDisplay.VirtualListSize = 2;
                else
                    libDisplay.VirtualListSize = 1;
            }
            libDisplay.Refresh();
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            if (txtSearch.Text.Equals(""))
            {
                if (rdLib.Checked)
                    entries = db.searchMedia();
                else if (rdPlay.Checked && stdPlay.SelectedIndices.Count > 0)
                    entries = db.searchStandardPlaylistMedia(stdPlaylistIDs[stdPlay.SelectedIndices[0]]);
                else if (rdDyPlay.Checked && dynPlay.SelectedIndices.Count > 0)
                    entries = db.searchDynamicPlaylistMedia(dynPlaylistIDs[dynPlay.SelectedIndices[0]]);
            }
            else
            {
                /*metaDataFieldTypes[] temp = new metaDataFieldTypes[txtSearch.Text.Split(' ').Length];
                for (int iLoop = 0; iLoop < temp.Length; iLoop++)
                    temp[iLoop] = metaDataFieldTypes.all;*/

                if (rdLib.Checked)
                    entries = db.searchDatabase(new searchRequest(txtSearch.Text.Split(' ')));//db.searchMedia(txtSearch.Text.Split(' '), temp, searchMethod.normal);
                else if (rdPlay.Checked && stdPlay.SelectedIndices.Count > 0)
                    entries = db.searchDatabase(new searchRequest(stdPlaylistIDs[stdPlay.SelectedIndices[0]], txtSearch.Text.Split(' ')));//db.searchStandardPlaylistMedia(stdPlaylistIDs[stdPlay.SelectedIndices[0]], txtSearch.Text.Split(' '), temp, searchMethod.normal);
                else if (rdDyPlay.Checked && dynPlay.SelectedIndices.Count > 0)
                    entries = db.searchDatabase(new searchRequest(dynPlaylistIDs[dynPlay.SelectedIndices[0]], txtSearch.Text.Split(' ')) { sType = searchType.dynamic }); //db.searchDynamicPlaylistMedia(dynPlaylistIDs[dynPlay.SelectedIndices[0]], txtSearch.Text.Split(' '), temp, searchMethod.normal);
            }

            if (rdLib.Checked || rdDyPlay.Checked)
                mediaEntry.sortMedia(entries, new metaDataFieldTypes[] { metaDataFieldTypes.artist, metaDataFieldTypes.album, metaDataFieldTypes.disk, metaDataFieldTypes.track, metaDataFieldTypes.title }, new sortOrder[] { sortOrder.ascending, sortOrder.ascending, sortOrder.ascending, sortOrder.ascending, sortOrder.ascending });

            timer.Stop();
            lblResult.Text = entries.Length + " results in " + Math.Round(timer.Elapsed.TotalSeconds, 3).ToString() + " seconds.";

            if (libDisplay.VirtualListSize == entries.Length)
            {
                if (libDisplay.VirtualListSize == 1)
                    libDisplay.VirtualListSize = 2;
                else
                    libDisplay.VirtualListSize = 1;
            }

            libDisplay.VirtualListSize = entries.Length;

            libDisplay.Enabled = true;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            searchIt.Stop();
            searchIt.Start();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            player.iVolume = trackBar1.Value / 2;
            vol.Text = trackBar1.Value.ToString();
        }

        private void rdLib_CheckedChanged(object sender, EventArgs e)
        {
            if (rdLib.Checked)
                doSearch(null, null);
        }

        private void rdPlay_CheckedChanged(object sender, EventArgs e)
        {
            if (rdPlay.Checked)
                doSearch(null, null);
        }

        private void rdDyPlay_CheckedChanged(object sender, EventArgs e)
        {
            if (rdDyPlay.Checked)
                doSearch(null, null);
        }

        private void cmdAddPlay_Click(object sender, EventArgs e)
        {
            Console.Write("Enter Playlist Name: ");
            string name = Console.ReadLine();

            int[] songs = new int[libDisplay.SelectedIndices.Count];
            for (int iLoop = 0; iLoop < songs.Length; iLoop++)
                songs[iLoop] = entries[libDisplay.SelectedIndices[iLoop]].iEntryID;

            db.createStandardPlaylist(name, songs);

            updateStdPlay();
        }

        private void cmdAddDynPlay_Click(object sender, EventArgs e)
        {
            Console.Write("Enter Playlist Name: ");
            string name = Console.ReadLine();

            dynamicQuery query = new dynamicQuery();
            query.addCondition(metaDataFieldTypes.artist, comparisonOperators.equals, "Civil Civic", logicOperators.or);
            query.addCondition(metaDataFieldTypes.artist, comparisonOperators.equals, "Tobacco", logicOperators.or);

            db.createDynamicPlaylist(name, query);

            updateDynPlay();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int[] songs = new int[libDisplay.SelectedIndices.Count];
            int[] loc = new int[libDisplay.SelectedIndices.Count];
            int total = db.searchStandardPlaylistMedia(stdPlaylistIDs[stdPlay.SelectedIndices[0]]).Length + 1;

            for (int iLoop = 0; iLoop < songs.Length; iLoop++)
            {
                songs[iLoop] = entries[libDisplay.SelectedIndices[iLoop]].iEntryID;
                loc[iLoop] = total + iLoop + 1;
            }

            db.addTrackToPlaylist(stdPlaylistIDs[stdPlay.SelectedIndices[0]], songs, loc);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            netAudioStreamer stream = new netVLCStreamer();
            stream.sNetworkAddr = "192.168.2.43";
            stream.iPort = 51920;
            stream.iBitRate = 96;
            stream.sMediaPath = @"P:\Music\OK Go\2010 - Of The Blue Colour Of The Sky\02 - This Too Shall Pass.mp3";
            stream.startStream();
        }
    }
}
