using HearthstoneLogData.DAL;
using HearthstoneLogData.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthstoneLogReader
{
    public class LogReader
    {
        public string FilePath { get; set; }

        private Match _match { get; set; }
        public Match LogMatch { get { return _match; } }

        private int _logLines { get; set; }
        public int LogLines { get { return _logLines; } }

        private HearthstoneLogDataContext db = new HearthstoneLogDataContext();

        private int _lastLineRead { get; set; }

        private FileStream fsHearthstoneLog;
        private StreamReader srHearthstoneLog;


        public void StartReadLog()
        {
            //TODO: Enable NET logging, generate model objects, instantiate lo
            if (LogMatch == null)
            {
                CreateMatch(DateTime.Now);
            }

            _logLines = File.ReadAllLines(FilePath).Length;
            _lastLineRead = 0;

            fsHearthstoneLog = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            srHearthstoneLog = new StreamReader(fsHearthstoneLog);

            while (_lastLineRead < _logLines)
            {
                string line = srHearthstoneLog.ReadLine();

                ReadLine(line);

                _lastLineRead++;
            }            
        }

        private void ReadLine(string line)
        {
            //BTW, I suck at string manipulation. This *works*, but will break SUPER easy
            //Have fun!
            if (line.StartsWith("[Zone]"))
            {
                ZoneChange zc = new ZoneChange();
                zc.MatchID = _match.MatchID;

                ZoneEntity ze = new ZoneEntity();

                if (line.StartsWith("[Zone] ZoneChangeList.ProcessChanges() - "))
                    line = line.Substring(("[Zone] ZoneChangeList.ProcessChanges() - ").Length - 1, line.Length - ("[Zone] ZoneChangeList.ProcessChanges() - ").Length);
                else
                    //TODO: remove this return. temporary because I just want to handle certain lines
                    return;

                var lineParts = line.Split(new char[] { ' ' });

                foreach (var linePart in lineParts)
                {
                    var keyValue = linePart.Split(new char[] { '=' });

                    switch (keyValue[0])
                    {
                        case "id":
                            if (zc.LogID == null)
                            {
                                zc.LogID = int.Parse(keyValue[1]);
                                break;
                            }
                            else
                            {
                                ze.LogID = int.Parse(keyValue[1]);
                                break;
                            }

                        case "local":
                            zc.Local = bool.Parse(keyValue[1]);
                            break;


                    }

                    ze.ZoneID = 13;

                    db.ZoneEntities.Add(ze);
                    db.SaveChanges();

                    zc.ZoneEntity = ze;
                    zc.ZoneEntityID = ze.ZoneEntityID;

                    db.ZoneChanges.Add(zc);
                    db.SaveChanges();

                }

                //string zcLogIdPart = line.Substring(0, "id=".Length + line.IndexOf(" "));
                //zc.LogID = int.Parse(zcLogIdPart.Split(new char[] { '=' })[1]);
                //line = line.Substring(("id=" + zc.LogID.ToString() + " ").Length - 1, line.Length);

                //string zcLocalPart = line.Substring(0, "local=".Length + line.IndexOf(" "));
                //zc.Local = bool.Parse(zcLogIdPart.Split(new char[] { '=' })[1]);
                //line = line.Substring(("local=" + zc.Local.ToString() + " ").Length - 1, line.Length);

                //#region ZoneEntity
                //ZoneEntity ze = new ZoneEntity();
                //string zePart = line.Substring(0, line.IndexOf(']') + 1);

                //string zeLogIdPart = line.Substring(0, "id=".Length + line.IndexOf(" "));
                //ze.LogID = int.Parse(zeLogIdPart.Split(new char[] { '=' })[1]);
                //zePart = zePart.Substring(("id=" + ze.LogID.ToString() + " ").Length - 1, zePart.Length);

                //string zeLogIdPart = line.Substring(0, "id=".Length + line.IndexOf(" "));
                //ze.LogID = int.Parse(zeLogIdPart.Split(new char[] { '=' })[1]);
                //zePart = zePart.Substring(("id=" + ze.LogID.ToString() + " ").Length - 1, zePart.Length);

                //#endregion

                //line = line.Substring(line.IndexOf(']') + 1, line.Length);
            }
        }


        private void CreateMatch(DateTime matchStart)
        {
            Match match = new Match()
                {
                    MatchStart = matchStart,
                    MatchEnd = matchStart
                };

            db.Matches.Add(match);
            db.SaveChanges();

            _match = match;
        }
    }
}
